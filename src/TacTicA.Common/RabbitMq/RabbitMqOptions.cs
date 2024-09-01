namespace TacTicA.Common.RabbitMq
{
    public class RabbitMqOptions
    {
        public RabbitMqOptions()
        {
            Hostnames = new List<string>();
        }

        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public List<string> Hostnames { get; set; }
    }
}