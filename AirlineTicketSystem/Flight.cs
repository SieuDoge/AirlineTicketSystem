namespace AirlineTicketSystem
{
    public class Flight
    {
        private string flightNumber;
        private string departure;
        private string destination;
        private DateTime departureTime;
        //Tổng số ghế cho từng hạng 
        private const int FIRST_MAX = 100;
        private const int BUSINESS_MAX = 200;
        private const int ECONOMY_MAX = 200;
        //danh sách ghế đã được đặt 
        private List<int> bookedFirst = new List<int>();
        private List<int> bookedBusiness=new List<int>();
        private List<int> bookedEconomy=new List<int>();    
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
           
        }

        public string GetFlightNumber() => flightNumber;
        public string GetDestination() => destination;
        public DateTime GetDepartureTime() => departureTime;
        public string GetDeparture() => departure;
        // in ra số ghế còn lại
        public void PrintSeatsInfo()
        {
            Console.WriteLine( $"Seats Left: First={FIRST_MAX - bookedFirst.Count}, " +
                               $"Business={BUSINESS_MAX - bookedBusiness.Count}, " +
                               $"Economy={ECONOMY_MAX - bookedEconomy.Count}"
                 );
        }
        //số ghế trống của 1 hạng
        public List<int> GetAvailableSeats(char type)
        {
            List<int> result = new List<int>();
            if (type == 'f')
            {
                for (int i = 1; i <= FIRST_MAX; i++)
                    if (!bookedFirst.Contains(i)) result.Add(i);
            }
            else if (type == 'b')
            {
                for (int i = 1; i <= BUSINESS_MAX; i++)
                    if (!bookedBusiness.Contains(i)) result.Add(i);
            }
            else if (type == 'e')
            {
                for (int i = 1; i <= ECONOMY_MAX; i++)
                    if (!bookedEconomy.Contains(i)) result.Add(i);
            }
            return result;
        }

        public bool BookSeat(char type , int seatNumber)
        {
            switch (type)
            {
                case 'f':
                    if (seatNumber >= 1 && seatNumber <= FIRST_MAX && !bookedFirst.Contains(seatNumber))
                    { bookedFirst.Add(seatNumber); return true; }
                    break;
                case 'b':
                    if (seatNumber >= 1 && seatNumber <= BUSINESS_MAX && !bookedBusiness.Contains(seatNumber))
                    { bookedBusiness.Add(seatNumber); return true; }
                    break;
                case 'e':
                    if (seatNumber >= 1 && seatNumber <= ECONOMY_MAX && !bookedEconomy.Contains(seatNumber))
                    { bookedEconomy.Add(seatNumber); return true; }
                    break;
            }
            return false;
        }
        
        public void ShowEmptySeats(char ticketType)
        {
            List<int> available = GetAvailableSeats(ticketType);
            if (available.Count == 0)
            {
                Console.WriteLine("Không còn ghế trống trong hạng này.");
                return;
            }

            string className = ticketType switch
            {
                'f' => "First Class",
                'b' => "Business",
                'e' => "Economy",
                _ => "Unknown"
            };

            Console.WriteLine($"\nGhế trống ({className}):");
            Console.WriteLine(string.Join(", ", available));
        }

     
        public bool IsSeatAvailable(char ticketType, int seatNumber)
        {
            return GetAvailableSeats(ticketType).Contains(seatNumber);
        }


        public void Print()
        {
            Console.WriteLine($"Flight Number: {GetFlightNumber()} - " +
                              $"Route: {GetDeparture()} -> {GetDestination()} - " +
                              $"Departure Time: {GetDepartureTime()} - " +
            PrintSeatsInfo();
        }
    }
}
