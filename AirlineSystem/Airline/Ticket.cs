namespace AirlineTicketSystem
{
    public abstract class Ticket
    {
        private string ticketId;
        private double ticketPrice;
        private string seat;
        protected Passenger passenger;
        protected Flight flight;
        public char TicketTypeChar { get; protected set; }

        protected Ticket(Passenger passenger, Flight flight, string ticketId = null, string seat = null)
        {
            this.ticketId = string.IsNullOrWhiteSpace(ticketId) ? "TK" + new Random().Next(100000, 999999) : ticketId;
            this.passenger = passenger;
            this.flight = flight;
            this.ticketPrice = CalculatePrice();
            this.seat = seat;
        }

        public string TicketId => ticketId;
        public double TicketPrice => ticketPrice;
        public string PassengerPhone => passenger?.PhoneNumber ?? "";
        public string NamePass => passenger?.Name ?? "";

        public string FlightNumber => flight?.GetFlightNumber() ?? "";

        protected abstract double CalculatePrice();
        public abstract void Print();
        public string TicketTypeName => TicketTypeChar switch
        {
            'e' => "Economy",
            'b' => "Business",
            'f' => "First Class",
            _ => "Unknown"
        };
    }

    
}