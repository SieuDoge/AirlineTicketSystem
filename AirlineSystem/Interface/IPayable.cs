namespace AirlineTicketSystem
{
    public interface IPayable
    {
        double Amount { get; }
        bool Pay();
        string PaymentStatus { get; }
    }
}


