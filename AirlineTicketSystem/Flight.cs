namespace AirlineTicketSystem
{
    public class Flight
    {
        private string flightNumber;
        private string departure;
        private string destination;
        private DateTime departureTime;
        private int availableSeats;

        public Flight(string flightNumber, string departure, string destination, DateTime departureTime, int seats)
        {
            // FlightNumber
            if (string.IsNullOrWhiteSpace(flightNumber))
                throw new ArgumentException("Flight number cannot be empty.");
            this.flightNumber = flightNumber;

            // Departure
            if (string.IsNullOrWhiteSpace(departure))
                throw new ArgumentException("Departure cannot be empty.");
            this.departure = departure;

            // Destination
            if (string.IsNullOrWhiteSpace(destination))
                throw new ArgumentException("Destination cannot be empty.");
            this.destination = destination;

            // DepartureTime
            this.departureTime = departureTime;

            // Seats
            if (seats < 0 || seats > 500)
                throw new ArgumentOutOfRangeException(nameof(seats), "Seats must be between 0 and 500.");
            this.availableSeats = seats;
        }

        public string GetFlightNumber() => flightNumber;
        public string GetDeparture() => departure;
        public string GetDestination() => destination;

        public DateTime GetDepartureTime()
        {
            return departureTime;
        }

        public int GetAvailableSeats() => availableSeats;

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
    }
}
