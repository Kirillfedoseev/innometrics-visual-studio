namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client("kirill1998fed@yandex.ru", "fkmlbyf123");

            client.Dispose();
        }
    }
}
