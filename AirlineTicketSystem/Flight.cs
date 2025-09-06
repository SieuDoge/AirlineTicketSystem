namespace AirlineTicketSystem
{
    public class Flight
    {
        private string flightNumber;
        private string departure;
        private string destination;
        private DateTime departureTime;
        private int availableSeats;

        public string FlightNumber
        {
            get => flightNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Flight number cannot be empty.");
                flightNumber = value;
            }
        }

        public string Departure
        {
            get => departure;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Departure cannot be empty.");
                departure = value;
            }
        }

        public string Destination
        {
            get => destination;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Destination cannot be empty.");
                destination = value;
            }
        }

        public DateTime DepartureTime
        {
            get => departureTime;
            set => departureTime = value;
        }

        public int AvailableSeats
        {
            get => availableSeats;
            set
            {
                if (value < 0 || value > 500)
                    throw new ArgumentOutOfRangeException(nameof(value), "Seats must be between 0 and 500.");
                availableSeats = value;
            }
        }

        public Flight(string flightNumber, string departure, string destination, DateTime departureTime, int seats)
        {
            FlightNumber = flightNumber;
            Departure = departure;
            Destination = destination;
            DepartureTime = departureTime;
            AvailableSeats = seats;
        }

        public string GetFlightNumber() => flightNumber;
        public int GetAvailableSeats() => availableSeats;
        public string GetDestination() => destination;
        public DateTime GetDepartureTime() => departureTime;
        public string GetDeparture() => departure;

        public bool BookSeat()
        {
            if (AvailableSeats > 0)
            {
                AvailableSeats--;
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
    }
}
