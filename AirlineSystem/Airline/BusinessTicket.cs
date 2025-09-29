using AirlineTicketSystem;

public class BusinessTicket : Ticket
{
    public BusinessTicket(Passenger passenger, Flight flight, string ticketId = null)
        : base(passenger, flight, ticketId)
    {
        TicketTypeChar = 'b';
    }

    protected override double CalculatePrice()
    {
        return 1.1;
    }

    public override void Print()
    {
        Console.WriteLine($"Business Ticket - {TicketId} - {PassengerPhone}");
    }
}
