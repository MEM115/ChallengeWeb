namespace ChallengeWeb.Models
{
    public class BalanceViewModel
    {
        public string Username { get; set; }
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastOperation { get; set; }
    }
}
