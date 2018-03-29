using System;
using System.Collections.Generic;

namespace BlockchainHelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            //初始化区块链
            BlockChainManager.SetDifficulty(3);

            //创建10个用户
            var users = new List<User>();
            for (int i = 0; i < 10; i++)
            {
                users.Add(GenerateUser());
            }

            var blockChainManager = new BlockChainManager();

            //启动区块链
            var minerExceptAddress = new List<string>();

            for (int i = 0; i < 2; i++)
            {
                #region 创建1笔交易

                var fromAddress = SelectRandomUser(users).Address;               
                var toAddress = SelectRandomUser(users, new List<string>() { fromAddress }).Address;         
                blockChainManager.CreateTransactoins(new Transaction(fromAddress, toAddress, GenerateRandomAmount()));

                #endregion

                #region 挖矿

                var miner = SelectRandomUser(users, new List<string>(){fromAddress, toAddress}); //确保挖矿者不是交易人员
                blockChainManager.MinePendingTransactions(miner.Address);
                minerExceptAddress.Clear();

                #endregion
            }

            System.Console.WriteLine($"The chain is valid: {blockChainManager.IsChainValid()}");

            //显示所有用户金额
            foreach (var user in users)
            {
                System.Console.WriteLine($"Address:{user.Address}; Amount: {BlockChainManager.GetAmountOfAddress(user.Address)}");
            }

            BlockChainManager.PrintBlockChain();

            Console.ReadKey();
        }

        //生成随机金额
        private static decimal GenerateRandomAmount()
        {
            double max = 100;
            double min = 1;
            return Utils.FromatAmount((decimal)((new Random().NextDouble()) * (max - min) + min));
        }

        //生成测试账户
        private static User GenerateUser()
        {
            var address = Guid.NewGuid().ToString();
            var name = $"{new Random().Next(1, 10000)}{address}";
            return new User(address, name);
        }

        //随机选择一个账号
        private static User SelectRandomUser(List<User> users, List<string> filterAddress = null)
        {
            var randomUser = users[new Random().Next(0, users.Count)];
            if (filterAddress == null)
            {
                return randomUser;
            }
            else
            {
                while (filterAddress.Contains(randomUser.Address))
                {
                    randomUser = users[new Random().Next(0, users.Count)];
                }

                return randomUser;
            }
        }
    }
}
