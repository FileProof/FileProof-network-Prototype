using Nethereum.Web3;
using System.Threading;

namespace CVProof.Utils
{
    class Tutorial
    {
        public async void Run()
        {
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var password = "password";
            var abi = @"[{""constant"":false,""inputs"":[{""name"":""val"",""type"":""int256""}],""name"":""multiply"",""outputs"":[{""name"":""d"",""type"":""int256""}],""type"":""function""},{""inputs"":[{""name"":""multiplier"",""type"":""int256""}],""type"":""constructor""}]";
            var byteCode =
                "0x60606040526040516020806052833950608060405251600081905550602b8060276000396000f3606060405260e060020a60003504631df4f1448114601a575b005b600054600435026060908152602090f3";

            var multiplier = 7;

            var web3 = new Web3();
            var unlockAccountResult = await web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, password, 120);
            var transactionHash = await web3.Eth.DeployContract.SendRequestAsync(abi, byteCode, senderAddress, multiplier);

            //var mineResult = await web3.Miner.Start.SendRequestAsync(6);

            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

            while (receipt == null)
            {
                Thread.Sleep(5000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            }

            //mineResult = await web3.Miner.Stop.SendRequestAsync();
            //Assert.True(mineResult);

            var contractAddress = receipt.ContractAddress;

            var contract = web3.Eth.GetContract(abi, contractAddress);

            var multiplyFunction = contract.GetFunction("multiply");

            var result = await multiplyFunction.CallAsync<int>(7);

            //Assert.Equal(49, result);

        }

    }
}
