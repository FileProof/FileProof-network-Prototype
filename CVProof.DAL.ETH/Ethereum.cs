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
        Web3 _web3 = new Web3("https://ropsten.infura.io/ALJyYuZ7YioSxeuzglYz");
        //var service = new HashStore(web3, "0xdab48ba055663eff50a52c0da2ef1cd40a1a2a20");
        //return service;

        string _contractAddress = "0x816a772c93dd3d62c05d58eff9e3739502fcf2b6"; //"0xdab48ba055663eff50a52c0da2ef1cd40a1a2a20";
        string _abi = @"[{""constant"":false,""inputs"":[{""name"":""headerHash"",""type"":""bytes32""}],""name"":""saveHeaderHash"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""header_hash"",""type"":""bytes32""}],""name"":""does_header_exist"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":"""",""type"":""bytes32""}],""name"":""Hashes"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""owner"",""outputs"":[{""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""validator"",""type"":""address""},{""indexed"":false,""name"":""headerHash"",""type"":""bytes32""},{""indexed"":false,""name"":""creation_timestamp"",""type"":""uint256""},{""indexed"":false,""name"":""headers_count"",""type"":""uint256""}],""name"":""FileProofHashCreated"",""type"":""event""}]";

        string _senderAddress = "0x949A6d7D69A72795301472199eB0255a030B0462";
        string _senderPK = "42e301d528240956beb9801af5be6e1fc7836630f86046700e7912585b24969d";

        public Ethereum()
        {}

        //private HashStore GetHashStore()
        //{
        //    var web3 = new Web3.Web3("https://mainnet.infura.io/ALJyYuZ7YioSxeuzglYz");
        //    var service = new DaoService(web3, "0xbb9bc244d798123fde783fcc1c72d3bb8c189413");
        //    return service;
        //}

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

                var data = saveHash.GetData(Convert.ToBytes(header.HeaderId.Substring(2)));

                var txCount = await _web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(_senderAddress);

                var gasPriceGwei =  await _web3.Eth.GasPrice.SendRequestAsync();

                var gasPrice = new BigInteger(UnitConversion.Convert.FromWei(gasPriceGwei));

                var encoded = Web3.OfflineTransactionSigner.SignTransaction(_senderPK, _contractAddress, 0, txCount.Value, gasPriceGwei, 300000, data);

                var txId = await _web3.Eth.Transactions.SendRawTransaction.SendRequestAsync("0x" + encoded);

                var receipt = await GetReceiptAsync(_web3, txId);

                //var transactionHash = await saveHash.SendTransactionAsync(_senderAddress, password);

                var hashCreated = contract.GetEvent("FileProofHashCreated");
                var filterAll = hashCreated.CreateFilterInput(new Nethereum.RPC.Eth.DTOs.BlockParameter(receipt.BlockNumber));
                var log = await hashCreated.GetAllChanges<HashCreatedEvent>(filterAll);
                
                //Infura doesn't support the filters, so have to request GetAllChanges.
                //In case using another service, could be done this way:
                //var filterAll = await hashCreated.CreateFilterAsync();
                //var log = await hashCreated.GetFilterChanges<HashCreatedEvent>(filterAll);

                header.BlockNumber = receipt.BlockNumber.Value.ToString();
                header.Timestamp = log[0].Event.Timestamp.ToString();
                header.DataAddress = receipt.TransactionHash;
                
                header.HeaderId = Utils.Convert.ToHexString(log[0].Event.HeaderHash);
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
