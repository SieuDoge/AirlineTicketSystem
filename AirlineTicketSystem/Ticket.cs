using System;

namespace AirlineTicketSystem
{
    public abstract class Ticket
    {
        private string ticketId;
        protected double ticketPrice;
        protected Passenger passenger;
        protected Flight flight;
        public char TicketTypeChar { get; protected set; }
        public int SeatNumber { get; protected set; }

        protected Ticket(Passenger passenger, Flight flight, int seatNumber,string ticketId = null)
            
        {
            this.ticketId = string.IsNullOrWhiteSpace(ticketId) ? "TK" + new Random().Next(100000, 999999) : ticketId;
            this.passenger = passenger;
            this.flight = flight;
            this.ticketPrice = CalculatePrice();
            this.SeatNumber=seatNumber;
        }

        public string TicketId => ticketId;
        public double TicketPrice => ticketPrice;
        public string PassengerPhone => passenger?.PhoneNumber ?? "";
        public string FlightNumber => flight?.GetFlightNumber() ?? "";

        protected abstract double CalculatePrice();
        public abstract void Print();
    }

    public class EconomyTicket : Ticket
    {
        public EconomyTicket(Passenger passenger, Flight flight, int seatNumber ,string ticketId = null)
            : base(passenger, flight, seatNumber, ticketId)
        {
            TicketTypeChar = 'e';
        }

        protected override double CalculatePrice() => 200;

        public override void Print()
        {
            Console.WriteLine($"[Economy] {TicketId}: {passenger.Name} - Flight {flight.GetFlightNumber()} - SeatNumber {SeatNumber} - {TicketPrice} USD");
        }
    }

    public class BusinessTicket : Ticket
    {
        public BusinessTicket(Passenger passenger, Flight flight, int seatNumber ,string ticketId = null)
            : base(passenger, flight, seatNumber, ticketId)
        {
            TicketTypeChar = 'b';
        }

        protected override double CalculatePrice() => 400;

        public override void Print()
        {
            Console.WriteLine($"[Business] {TicketId}: {passenger.Name} - Flight {flight.GetFlightNumber()} - SeatNumber {SeatNumber} - {TicketPrice} USD");
        }
    }

    public class FirstClassTicket : Ticket
    {
        public FirstClassTicket(Passenger passenger, Flight flight, int seatNumber,  string ticketId = null)
            : base(passenger, flight, seatNumber,ticketId)
        {
            TicketTypeChar = 'f';
        }

        protected override double CalculatePrice() => 800;

        public override void Print()
        {
            Console.WriteLine($"[First Class] {TicketId}: {passenger.Name} - Flight {flight.GetFlightNumber()} - SeatNumber {SeatNumber} - {TicketPrice} USD");
        }
    }
}
