using AirlineTicketSystem;

public class BusinessTicket : Ticket
{
    public BusinessTicket(Passenger passenger, Flight flight, string ticketId = null, string seat = null)
        : base(passenger, flight, ticketId, seat)
    {
        TicketTypeChar = 'b';
    }

    protected override double CalculatePrice()
    {
        return 450 * 1.1;
    }

    public override void Print()
    {
        Console.WriteLine($"Business Ticket - {TicketId} - {PassengerPhone}");
    }
}
