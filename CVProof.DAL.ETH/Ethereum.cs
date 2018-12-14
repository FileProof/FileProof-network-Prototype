using Nethereum.Web3;
using System.Numerics;
using Nethereum.Util;
using System.Threading.Tasks;
using System.Threading;
using CVProof.Models;
using Nethereum.RPC.Eth.DTOs;
using CVProof.Utils;


namespace CVProof.DAL.ETH
{
    public class Ethereum
    {
        Web3 _web3;

        string _contractAddress;
        string _abi;    
        string _senderAddress;
        string _senderPK;

        public Ethereum(string contractAddress, string abi, string senderAddress, string senderPrimaryKey, string node)
        {
            _contractAddress = contractAddress;
            _abi = abi;
            _senderAddress = senderAddress;
            _senderPK = senderPrimaryKey;
            _web3 = new Web3(node);
        }

        public async Task<bool> Unlock()
        {
            return await _web3.Personal.UnlockAccount.SendRequestAsync(_senderAddress, _senderPK, 30);            
        }

        public async Task<bool> TestHashStore(string hash)
        {
            var contract = _web3.Eth.GetContract(_abi, _contractAddress);

            var isExist = contract.GetFunction("does_header_exist");

            var result = await isExist.CallAsync<bool>(hash);

            return result;
        }
              
        public async Task<HeaderModel> SendToNetwork(HeaderModel header)
        {
            try
            {
                var contract = _web3.Eth.GetContract(_abi, _contractAddress);

                var saveHash = contract.GetFunction("saveHeaderHash");              

                var data = saveHash.GetData(Convert.ToBytes(header.GlobalHash));

                var txCount = await _web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(_senderAddress);

                var gasPriceGwei =  await _web3.Eth.GasPrice.SendRequestAsync();

                var gasPrice = new BigInteger(UnitConversion.Convert.FromWei(gasPriceGwei));

                var encoded = Web3.OfflineTransactionSigner.SignTransaction(_senderPK, new BigInteger(3), _contractAddress, 0, txCount.Value, gasPriceGwei, 300000, data);

                var txId = await _web3.Eth.Transactions.SendRawTransaction.SendRequestAsync("0x" + encoded);

                var receipt = await GetReceiptAsync(_web3, txId);

                //var transactionHash = await saveHash.SendTransactionAsync(_senderAddress, password);

                var hashCreated = contract.GetEvent("FileProofHashCreated");
                var filterAll = hashCreated.CreateFilterInput(new Nethereum.RPC.Eth.DTOs.BlockParameter(receipt.BlockNumber),null);
                var log = await hashCreated.GetAllChanges<HashCreatedEvent>(filterAll);
                
                //Infura doesn't support the filters, so have to request GetAllChanges.
                //In case using another service, could be done this way:
                //var filterAll = await hashCreated.CreateFilterAsync();
                //var log = await hashCreated.GetFilterChanges<HashCreatedEvent>(filterAll);

                header.BlockNumber = receipt.BlockNumber.Value.ToString();
                header.Timestamp = log[0].Event.Timestamp.ToString();
                header.DataAddress = receipt.TransactionHash;
                
                header.GlobalHash = Utils.Convert.ToHexString(log[0].Event.HeaderHash);
            }
            catch (System.Exception e)
            {               
            }

            return header;
        }

        public async Task<TransactionReceipt> GetReceiptAsync(Web3 web3, string transactionHash)
        {
            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

            while (receipt == null)
            {
                Thread.Sleep(1000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            }

            return receipt;
        }
    }
}
