namespace AirlineTicketSystem
{
    public abstract class Ticket : IPrintable, IPriceable, ISearchable, IExportable, IValidatable
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

        public string Seat
        {
            get => seat;
            set => seat = value;
        }
        public string PassengerPhone => passenger?.PhoneNumber ?? "";
        public string NamePass => passenger?.Name ?? "";


        public string FlightNumber => flight?.GetFlightNumber() ?? "";

        protected abstract double CalculatePrice();
        public abstract void Print();

        public double GetPrice()
        {
            return TicketPrice;
        }

        public bool Matches(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return true;
            term = term.Trim().ToLower();
            return (TicketId?.ToLower().Contains(term) == true)
                   || (NamePass?.ToLower().Contains(term) == true)
                   || (FlightNumber?.ToLower().Contains(term) == true)
                   || (PassengerPhone?.Contains(term) == true)
                   || (TicketTypeName?.ToLower().Contains(term) == true)
                   || TicketPrice.ToString("N2").Contains(term);
        }

        public string ToCsvHeader()
        {
            return "TicketId,Price,TicketType,PassengerPhone,FlightNumber,Seat";
        }

        public string ToCsvRow()
        {
            return $"{TicketId},{TicketPrice},{TicketTypeChar},{PassengerPhone},{FlightNumber},{Seat}";
        }

        public bool IsValid(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(TicketId)) { errorMessage = "TicketId required"; return false; }
            if (passenger == null) { errorMessage = "Passenger required"; return false; }
            if (flight == null) { errorMessage = "Flight required"; return false; }
            errorMessage = string.Empty;
            return true;
        }
        public string TicketTypeName => TicketTypeChar switch
        {
            'e' => "Economy",
            'b' => "Business",
            'f' => "First Class",
            _ => "Unknown"
        };
    }

    
}