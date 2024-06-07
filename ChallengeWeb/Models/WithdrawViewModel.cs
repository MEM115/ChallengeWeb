namespace ChallengeWeb.Models
{
    public class WithdrawViewModel
    {
        public string? cardNumber { get; set; }
        public double Amount { get; set; }
    }
    public class WithdrawResponseViewModel
    {
        public int AccountId { get; set; }
        public double Amount { get; set; }
        public double OldBalance { get; set; }
        public double NewBalance { get; set; }
        public DateTime OperationTime { get; set; }
    }
}
