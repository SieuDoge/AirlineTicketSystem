namespace AirlineTicketSystem;
﻿namespace AirlineTicketSystem;

public class Flight
{
    1234567890-
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        bool next = true;
        while (next)
        {
            Console.WriteLine("Enter flight number: ");
            string fightNumber = Console.ReadLine();
            Console.WriteLine("Enter departure location: ");
            string departure = Console.ReadLine();
            Console.WriteLine("Enter destination: ");
            string destination = Console.ReadLine();
            Console.WriteLine("Enter departure time: ");
            DateTime destinationTime = DateTime.Now; //lấy thời gian hiện tại bỏ vào class flight 
            Console.ReadLine(); //  cho người nhập đại  vào đây 
            Console.WriteLine("Enter available seats: ");
            int availableSeats = int.Parse(Console.ReadLine());
            Flights bay = new Flights(fightNumber, departure, destination, DateTime.Now, availableSeats);
            bay.Print();

            Console.Write("\nDo you want to continue? (Y/N): ");
            string answer = Console.ReadLine();
            if (answer.ToUpper() != "Y")
            {
                next = false;
            }
            else
            {
                next = true;
            }



        }
    }
}
public class Flights
{
    // Thuộc tính (private fields)
    private string flightNumber;
    private string departure;
    private string destination;
    private DateTime departureTime;
    private int availableSeats;

    // Constructor
    public Flights(string flightNumber, string departure, string destination, DateTime departureTime, int seats)
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