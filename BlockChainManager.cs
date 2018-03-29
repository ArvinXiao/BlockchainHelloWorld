using System.Collections.Generic;

namespace BlockchainHelloWorld
{
    public class BlockChainManager
    {
        //困难值
        private static int Difficulty = 2;

        //区块链
        private static List<Block> BlockChain = new List<Block>();

        //当前待打包的交易记录
        private List<Transaction> pendingTransactions = new List<Transaction>();

        //设置困难值
        public static void SetDifficulty(int difficulty){
            Difficulty = difficulty;
        }

        //保存新的交易
        public void CreateTransactoins(Transaction transaction){
            this.pendingTransactions.Add(transaction);
        }

        //打包交易、挖矿
        public void MinePendingTransactions(string miningRewardAddress){
            //获取前一个区块的Hash值
            var previousHash = "0";
            if(BlockChain.Count > 0){
                previousHash  = GetLastestBlock().Hash;
            } 

            //创建新的区块
            var newBlock = new Block(this.pendingTransactions, previousHash);

            //挖矿
            newBlock.MineBlock(Difficulty);

            //将区块添加到区块链上
            BlockChain.Add(newBlock);

            //计算奖励: 2%奖励
            var transactionAmount = 0m;
            foreach(var t in pendingTransactions){
                transactionAmount += t.Amount;
            }
            var miningRewards = Utils.FromatAmount(transactionAmount / 100 * 2); 

            //分配奖励给旷工
            this.pendingTransactions = new List<Transaction>(){new Transaction(string.Empty, miningRewardAddress, miningRewards)};
        }

        //获取最新的区块
        public Block GetLastestBlock(){
            return BlockChain[BlockChain.Count - 1];
        }

        //获取账户的金额
        public static decimal GetAmountOfAddress(string address){
            var balance = 0m;
            foreach(var block in BlockChain){
                foreach(var tran in block.TransactionData){
                    if(tran.FromAddress.Equals(address)){
                        balance -= tran.Amount;
                    }

                    if(tran.ToAddress.Equals(address)){
                        balance += tran.Amount;
                    }
                }
            }

            return Utils.FromatAmount(balance);
        }

        //验证区块链是否有效
        public bool IsChainValid(){
            for(int i = 1; i < BlockChain.Count; i++){
                var currentBlock = BlockChain[i];
                var priviousBlock = BlockChain[i-1];
                if(currentBlock.Hash != currentBlock.CalculateHash() || currentBlock.PreviousHash != priviousBlock.Hash){
                    return false;
                }
            }

            return true;
        }

        //输出所有区块明细
        public static void PrintBlockChain(){
            foreach(var block in BlockChain){
                System.Console.WriteLine(block.ToString());
            }
        }
    }
}
