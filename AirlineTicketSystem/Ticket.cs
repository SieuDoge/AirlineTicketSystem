using System.Diagnostics.Contracts;

namespace AirlineTicketSystem;

public abstract class Ticket
{
    private string ticketId;
    private double ticketPrice;
    private char ticketType;
    private Passenger passenger;
    private Flight flight;
    public Ticket(string ticketId, Passenger passenger, Flight flight, double ticketPrice, char ticketType)
    {
        Random rnd = new Random();
        this.ticketId = "TK" + rnd.Next(100, 99999);
        this.ticketPrice = ticketPrice;
        this.ticketType = ticketType;
        this.passenger = passenger;
        this.flight = flight;

    }
    public string TicketId
    {
        get
        {
            return ticketId;

        }
        set
        {
            ticketId = value;

        }
    }
    public abstract double TicketPrice() { }
    

    public abstract void Print() { }
       
    
}

public class Economy:Ticket
{
    public EconomyTicket(Passenger passenger, Flight flight) : base(passenger,flight)
    {
        this.passenger = passenger;
        this.flight = flight;

    }
    public override double TicketPrice()
    {
        get {
            return 200;
        }
        set{
            ticketPrice = value;
        }
    }
    public override void Print()
    {
        Console.WriteLine($"[Economy] - {Passenger.GetName()} - {Flight.GetFlightNumber()} - {TicketPrice} USD ");
    }
}
public class Business : Ticket
{
    public BusinessTicket(Passenger passenger, Flight flight) : base(passenger, flight)
    {
        this.passenger = passenger;
        this.flight = flight;

    }
    public override double TicketPrice()
    {
        get {
            return 400;
        }
        set{
            ticketPrice = value;
        }
    }
    public override void Print()
    {
        Console.WriteLine($"[Economy] - {Passenger.GetName()} - {Flight.GetFlightNumber()} - {TicketPrice} USD ");
    }
}
public class FirstClass : Ticket
{
    public FirstClassTicket(Passenger passenger, Flight flight) : base(passenger, flight)
    {
        this.passenger = passenger;
        this.flight = flight;

    }
    public override double TicketPrice()
    {
        get {
            return 800;
        }
        set{
            ticketPrice = value;
        }
    }
    public override void Print()
    {
        Console.WriteLine($"[Economy] - {Passenger.GetName()} - {Flight.GetFlightNumber()} - {TicketPrice} USD ");
    }
}