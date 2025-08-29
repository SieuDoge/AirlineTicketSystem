namespace AirlineTicketSystem;


public class Flight
{
    // Thuộc tính (private fields)
    private string flightNumber;
    private string departure;
    private string destination;
    private DateTime departureTime;
    private int availableSeats;

    // Constructor
    public Flight(string flightNumber, string departure, string destination, DateTime departureTime, int seats)
    {
        this.flightNumber = flightNumber;
        this.departure = departure;
        this.destination = destination;
        this.departureTime = departureTime;
        this.availableSeats = seats >= 0 ? seats : 0; // đảm bảo seats >= 0
    }




    // Getter methods
    public string GetFlightNumber()
    {
        return flightNumber;
    }

    public string GetDeparture()
    {
        return departure;
    }

    public string GetDestination()
    {
        return destination;
    }

    public DateTime GetDepartureTime()
    {
        // Bỏ phút và giây
        departureTime = new DateTime(
            departureTime.Year,
            departureTime.Month,
            departureTime.Day,
            departureTime.Hour, 0, 0
        );

        // Nếu phút/giây > 0 thì làm tròn lên 1 giờ
        if (departureTime.Minute > 0 || departureTime.Second > 0)
        {
            departureTime = departureTime.AddHours(1);
        }

        departureTime = departureTime.AddHours(3); // tăng thời gian thêm 3 tiếng trước giờ khởi hành 

        return departureTime;
    }

    public int GetAvailableSeats()
    {
        return availableSeats;
    }

    // Đặt chỗ (giảm số ghế trống nếu còn)
    public bool BookSeat()
    {
        if (availableSeats > 0)
        {
            availableSeats--;
            return true;
        }
        return false;
    }

    // In thông tin chuyến bay
    public void Print()
    {
        Console.WriteLine("Flight Number: " + GetFlightNumber());
        Console.WriteLine("Departure: " + GetDeparture());
        Console.WriteLine("Destination: " + GetDestination());
        Console.WriteLine("Departure Time: " + GetDepartureTime());
        Console.WriteLine("Available Seats: " + GetAvailableSeats());
    }

}
