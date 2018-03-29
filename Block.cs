using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace BlockchainHelloWorld
{
    public class Block
    {
        #region Properties

        public string PreviousHash {get;set;}
        public DateTime Timestamp{get;}
        public List<Transaction> TransactionData{get;set;}
        public string Hash{get;set;}
        public int Nonce {get;set;} = 0;

        #endregion

        public Block(List<Transaction> tranaction, string previousHash){
            TransactionData = tranaction;
            PreviousHash = previousHash;

            Timestamp = DateTime.Now;
            Hash = CalculateHash();
        }

        //使用SHA256算法计算该区块Hash值
        public string CalculateHash(){
            var sha = SHA256.Create();
            var valueBits = Encoding.UTF8.GetBytes(GetHashStr());
            var hashBits = sha.ComputeHash(valueBits);
            return BitConverter.ToString(hashBits).Replace("-", "");
        }

        // 获取有效的Hash值
        public void MineBlock(int difficulty){
            var difficultyWarp = difficulty + 1;
            int i = 0;
            while(Hash.Substring(0, difficultyWarp) != 0.ToString($"D{difficultyWarp}")){
                Nonce++;
                Hash = CalculateHash();
                i++;
            }

            //System.Console.WriteLine($"Try Times:{i}; Hash：{Hash}");
        }

        private string GetHashStr(){
            return $"{PreviousHash}{Timestamp}{JsonConvert.SerializeObject(TransactionData)}{Nonce}";
        }

        public override string ToString(){
            return $"Hash:{Hash}; Previous Hash:{PreviousHash}; Time:{Timestamp}; Nonce:{Nonce}; Data:{JsonConvert.SerializeObject(TransactionData)}";
        }
    }
}
