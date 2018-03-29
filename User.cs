namespace BlockchainHelloWorld
{
    public class User
    {
        public string Address{get;set;}
        public string Name{get;set;}

        public User(string address, string name){
            Address = address;
            Name = name;
        }
    }
}