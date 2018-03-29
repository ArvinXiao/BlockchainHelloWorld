using System;

namespace BlockchainHelloWorld
{
    public class Utils
    {
        //将交易金额保存为固定位数小数
        public static decimal FromatAmount(decimal amount){
            return Decimal.Round(amount, 4);
        }
    }
}