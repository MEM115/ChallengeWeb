namespace ChallengeWeb.Models
{
    public class OperationViewModel
    {
        public int Total { get; set; }
        public List<Operation> Operations { get; set; }
    }
    public class Operation
    {
        public DateTime? OperationTime { get; set; }
        public double Amount { get; set; }
    }
}
