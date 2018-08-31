using Nethereum.Web3;
using System.Net;
using System.Threading.Tasks;
using CVProof.Models;

namespace CVProof.DAL.ETH
{
    public class Ethereum
    {
        Web3 web3 = new Web3("https://ropsten.infura.io/ALJyYuZ7YioSxeuzglYz");
        //var service = new HashStore(web3, "0xdab48ba055663eff50a52c0da2ef1cd40a1a2a20");
        //return service;

        string contractAddress = "0x816a772c93dd3d62c05d58eff9e3739502fcf2b6";//"0xdab48ba055663eff50a52c0da2ef1cd40a1a2a20";
        string abi = @"[{""constant"":false,""inputs"":[{""name"":""headerHash"",""type"":""bytes32""}],""name"":""saveHeaderHash"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""header_hash"",""type"":""bytes32""}],""name"":""does_header_exist"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":"""",""type"":""bytes32""}],""name"":""Hashes"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""owner"",""outputs"":[{""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""inputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""validator"",""type"":""address""},{""indexed"":false,""name"":""headerHash"",""type"":""bytes32""},{""indexed"":false,""name"":""creation_timestamp"",""type"":""uint256""},{""indexed"":false,""name"":""headers_count"",""type"":""uint256""}],""name"":""FileProofHashCreated"",""type"":""event""}]";

        public Ethereum()
        {}

        //private HashStore GetHashStore()
        //{
        //    var web3 = new Web3.Web3("https://mainnet.infura.io/ALJyYuZ7YioSxeuzglYz");
        //    var service = new DaoService(web3, "0xbb9bc244d798123fde783fcc1c72d3bb8c189413");
        //    return service;
        //}

        public async Task<bool> TestHashStore(string hash)
        {

            var contract = web3.Eth.GetContract(abi, contractAddress);

            var isExist = contract.GetFunction("does_header_exist");

            var result = await isExist.CallAsync<bool>(hash);

            return result;
        }

        public async Task<HeaderModel> SendToNetwork(string hash)
        {
            var contract = web3.Eth.GetContract(abi, contractAddress);



            var saveHash = contract.GetFunction("saveHeaderHash");

            var result = await saveHash.CallAsync<>(hash);



            return result;
        }

    }
}
