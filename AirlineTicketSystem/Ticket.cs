using System.Diagnostics.Contracts;

namespace AirlineTicketSystem;

public abstract class Ticket
{
    private string ticketId;
    private double ticketPrice;
    private char ticketType;
    private string passengerPhone;
    public Ticket(string ticketId, double ticketPrice, char ticketType, string passengerPhone)
    {
        Random rnd = new Random();
        /*this.ticketId = "TK" + rnd.Next(100, 99999);*/
        this.ticketId = ticketId;
        this.ticketPrice = ticketPrice;
        this.ticketType = ticketType;
        this.passengerPhone = passengerPhone;

    }
    public string Flight { get; set; }
    
    public string PassengerPhone => passengerPhone;
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
<<<<<<< HEAD
    public abstract double TicketPrice() { }
    

    public abstract void Print() { }
=======
    public char TicketType
    {
        get { return ticketType; }
        set
        {
            if (value == 'e' || value == 'f' || value == 'b')
            {
                ticketType = value;
                switch (ticketType)
                {
                    case 'e':
                        ticketPrice = 200;
                        break;
                    case 'b':
                        ticketPrice = 400;
                        break;
                    case 'f':
                        ticketPrice = 800;  
                        break;
                }
            }
            else
            {
                throw new ArgumentException("Error!");
            }
        }
>>>>>>> 724ded50c1bd196b6c78c252dbb606f0a1156d32
       
    
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

<<<<<<< HEAD
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
=======
    public void Print() {
        Console.WriteLine($"Ticket {ticketId} - {ticketType} - {ticketPrice} USD");
>>>>>>> 724ded50c1bd196b6c78c252dbb606f0a1156d32
    }
}