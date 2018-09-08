using Nethereum.Web3;
using System.Net;
using System.Threading.Tasks;



namespace CVProof.Utils
{
    public class Web3Util
    {
        public static async Task<decimal> GetEthereumFoundationBalance()
        {
            decimal ret;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Web3 web3 = new Web3("https://mainnet.infura.io/ALJyYuZ7YioSxeuzglYz");

            var balance = await web3.Eth.GetBalance.SendRequestAsync("0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae");

            ret = Web3.Convert.FromWei(balance.Value);

            return ret;
        }

        private HashStore GetHashStore()
        {
            return new HashStore();
        }
        public static async Task<bool> TestHashStore(string hash)
        {
            var web3 = new Web3("https://ropsten.infura.io/ALJyYuZ7YioSxeuzglYz");
            //var service = new HashStore(web3, "0xdab48ba055663eff50a52c0da2ef1cd40a1a2a20");
            //return service;

            var contractAddress = "0x816a772c93dd3d62c05d58eff9e3739502fcf2b6";//"0xdab48ba055663eff50a52c0da2ef1cd40a1a2a20";
            var abi = @"[{""constant"":false,""inputs"":[{""name"":""headerHash"",""type"":""bytes32""}],""name"":""saveHeaderHash"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""header_hash"",""type"":""bytes32""}],""name"":""does_header_exist"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":"""",""type"":""bytes32""}],""name"":""Hashes"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""owner"",""outputs"":[{""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""validator"",""type"":""address""},{""indexed"":false,""name"":""headerHash"",""type"":""bytes32""},{""indexed"":false,""name"":""creation_timestamp"",""type"":""uint256""},{""indexed"":false,""name"":""headers_count"",""type"":""uint256""}],""name"":""FileProofHashCreated"",""type"":""event""}]";

            var contract = web3.Eth.GetContract(abi,contractAddress);

            var isExist = contract.GetFunction("does_header_exist");

            var result = await isExist.CallAsync<bool>(hash);

            return result;
        }
    }
}
