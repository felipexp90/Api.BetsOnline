namespace Entities.Utils
{
    public class Singleton
    {
        private static Singleton _instance;
        public string DbName { get; set; }
        public bool Audit { get; set; }
        public string AuthId { get; set; }
        public string Token { get; set; }
        public string Ip { get; set; }
        public static Singleton Instance => _instance ?? (_instance = new Singleton());
    }
}
