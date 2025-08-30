using System.Diagnostics.Contracts;

namespace AirlineTicketSystem;

public class Ticket
{
    private string ticketId;
    private double ticketPrice;
    private char ticketType;
    private string flight;
    public Ticket(string ticketId, string flight, double ticketPrice, char ticketType)
    {
        Random rnd = new Random();
        this.ticketId = "TK" + rnd.Next(100, 99999);
        this.ticketPrice = ticketPrice;
        this.ticketType = ticketType;
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
       
            
    }
    public double TicketPrice {
        get { return ticketPrice; }

    }

    public void Print() {
        Console.WriteLine($"Ticket {ticketId} - Flight {flight.GetFlightNumber()} - {ticketType} - {ticketPrice} USD");
    }
}