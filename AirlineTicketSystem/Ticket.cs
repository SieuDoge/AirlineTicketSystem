using System;

namespace AirlineTicketSystem
{
   
    public abstract class Ticket
    {
        private string ticketId;
        protected double ticketPrice; 
        protected Passenger passenger;
        protected Flight flight;

        public Ticket(Passenger passenger, Flight flight, string ticketId , double ticketPrice)
        {
            Random rnd = new Random();
            this.ticketId = "TK" + rnd.Next(100, 99999);
            this.passenger = passenger;
            this.flight = flight;
            this.ticketPrice = CalculatePrice(); 
        }

       
        public string TicketId => ticketId;

      
        public double TicketPrice => ticketPrice;

        
        protected abstract double CalculatePrice();

       
       public abstract void Print() {}
    }

    
    public class EconomyTicket : Ticket
    {
        public EconomyTicket(Passenger passenger, Flight flight) : base(passenger, flight) { }

        protected override double CalculatePrice() => 200;

        public override void Print()
        {
            Console.WriteLine($"[Economy] {TicketId}: {passenger.GetName()} - Flight {flight.GetFlightNumber()} - {TicketPrice} USD");
        }
    }

   
    public class BusinessTicket : Ticket
    {
        public BusinessTicket(Passenger passenger, Flight flight) : base(passenger, flight) { }

        protected override double CalculatePrice() => 400;

        public override void Print()
        {
            Console.WriteLine($"[Business] {TicketId}: {passenger.GetName()} - Flight {flight.GetFlightNumber()} - {TicketPrice} USD");
        }
    }

   
    public class FirstClassTicket : Ticket
    {
        public FirstClassTicket(Passenger passenger, Flight flight) : base(passenger, flight) { }

        protected override double CalculatePrice() => 800;

        public override void Print()
        {
            Console.WriteLine($"[First Class] {TicketId}: {passenger.GetName()} - Flight {flight.GetFlightNumber()} - {TicketPrice} USD");
        }
    }
}
