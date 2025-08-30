using System.Diagnostics.Contracts;

namespace AirlineTicketSystem;

public class Ticket
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
    
    public string PassengerPhone { get; set; }
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
        Console.WriteLine($"Ticket {ticketId} - {ticketType} - {ticketPrice} USD");
    }
}