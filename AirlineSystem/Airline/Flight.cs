namespace AirlineTicketSystem
{
    public class Flight : IPrintable, IExportable, ISearchable, IValidatable
    {
        private string flightNumber;
        private string departure;
        private string destination;
        private DateTime departureTime;
        private int availableSeats;
        private FlightStatus status;

        public Flight(string flightNumber, string departure, string destination, DateTime departureTime, int seats, FlightStatus status)
        {
            if (string.IsNullOrWhiteSpace(flightNumber))
                throw new ArgumentException("Flight number cannot be empty.");
            this.flightNumber = flightNumber;

            if (string.IsNullOrWhiteSpace(departure))
                throw new ArgumentException("Departure cannot be empty.");
            this.departure = departure;

            if (string.IsNullOrWhiteSpace(destination))
                throw new ArgumentException("Destination cannot be empty.");
            this.destination = destination;

            this.departureTime = departureTime;

            if (seats < 0 || seats > 500)
                throw new ArgumentOutOfRangeException(nameof(seats), "Seats must be between 0 and 500.");
            this.availableSeats = seats;

            this.status = status;
        }

        public string GetFlightNumber() => flightNumber;
        public string GetDeparture() => departure;
        public string GetDestination() => destination;
        public int GetAvailableSeats() => availableSeats;
        public DateTime GetDepartureTime()
        {
            return departureTime;
        }

        public FlightStatus GetStatus() => status;

        // XAML ko hỗ trợ gọi Method  nên phải thêm property wrapper cay vcl

        public FlightStatus Status => status;

        public string FlightNumber => flightNumber;
        public string Departure => departure;
        public string Destination => destination;
        public DateTime DepartureTime => departureTime;

        public DateTime ArrivalTime => DepartureTime.AddHours(3);



        public enum FlightStatus
        {
            Scheduled,  // Đã lên lịch
            OnGoing,    // Đang bay
            Delayed,    // Trì hoãn
            Cancelled,  // Đã hủy
            Completed   // Đã hoàn thành
        }


        // Optional adapter to IBookable could be added at higher-level entity (ticket)
        public bool BookSeat()
        {
            if (availableSeats > 0)
            {
                availableSeats--;
                return true;
            }
            return false;
        }

        public void Print()
        {
            Console.WriteLine($"Flight Number: {GetFlightNumber()} - " +
                              $"Route: {GetDeparture()} -> {GetDestination()} - " +
                              $"Departure Time: {GetDepartureTime()} - " +
                              $"Available Seats: {GetAvailableSeats()}");
        }

        public string ToCsvHeader()
        {
            return "FlightNumber,Departure,Destination,DepartureTime,AvailableSeats,Status";
        }

        public string ToCsvRow()
        {
            return $"{GetFlightNumber()},{GetDeparture()},{GetDestination()},{GetDepartureTime():yyyy-MM-dd HH:mm},{GetAvailableSeats()},{(int)Status}";
        }

        public bool Matches(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return true;
            term = term.Trim().ToLower();
            return (FlightNumber?.ToLower().Contains(term) == true)
                   || (Departure?.ToLower().Contains(term) == true)
                   || (Destination?.ToLower().Contains(term) == true)
                   || Status.ToString().ToLower().Contains(term)
                   || DepartureTime.ToString("MM/dd/yyyy").Contains(term);
        }

        public bool IsValid(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(flightNumber)) { errorMessage = "Flight number cannot be empty."; return false; }
            if (string.IsNullOrWhiteSpace(departure)) { errorMessage = "Departure cannot be empty."; return false; }
            if (string.IsNullOrWhiteSpace(destination)) { errorMessage = "Destination cannot be empty."; return false; }
            if (availableSeats < 0 || availableSeats > 500) { errorMessage = "Seats must be between 0 and 500."; return false; }
            errorMessage = string.Empty;
            return true;
        }
    }
}
