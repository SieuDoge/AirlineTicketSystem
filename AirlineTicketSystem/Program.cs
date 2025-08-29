using System;

namespace AirlineTicketSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Tạo hành khách
            Passenger passenger1 = new Passenger("Nguyen Van A");
            Passenger passenger2 = new Passenger("Tran Thi B");

            // Tạo chuyến bay
            Flight flight1 = new Flight("VN123");
            Flight flight2 = new Flight("VN456");

            // Tạo vé (Economy, Business, First)
            Ticket ticket1 = new Ticket(passenger1, flight1, 'e');
            Ticket ticket2 = new Ticket(passenger1, flight2, 'b');
            Ticket ticket3 = new Ticket(passenger2, flight1, 'f');

            // In thông tin vé
            ticket1.Print();
            ticket2.Print();
            ticket3.Print();

            Console.ReadLine(); // chờ user bấm phím để đóng console
        }
    }

    // Mock Passenger
    public class Passenger
    {
        private string name;
        public Passenger(string name)
        {
            this.name = name;
        }
        public string GetName() => name;
    }

    // Mock Flight
    public class Flight
    {
        private string flightNumber;
        public Flight(string flightNumber)
        {
            this.flightNumber = flightNumber;
        }
        public string GetFlightNumber() => flightNumber;
    }
}
