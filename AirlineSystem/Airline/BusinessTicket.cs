using AirlineTicketSystem;

public class BusinessTicket : Ticket, IBookable
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

    private bool isBooked;
    public bool Book()
    {
        if (isBooked) return false;
        if (flight?.BookSeat() == true)
        {
            isBooked = true;
            return true;
        }
        return false;
    }

    public bool Cancel()
    {
        if (!isBooked) return false;
        isBooked = false;
        return true;
    }

    public bool IsBooked => isBooked;
}
