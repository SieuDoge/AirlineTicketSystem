using System;
using System.IO;
using System.Linq;

namespace AirlineTicketSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("WHAT DO U WANT?");
                Console.WriteLine("1. Book Ticket");
                Console.WriteLine("2. Airline Manager");
                Console.WriteLine("0. Exit");
                Console.Write("Choose option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        BookTicket();
                        break;
                    case "2":
                        AirlineManagerMenu();
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
            }
        }

        static AirlineManager LoadingData()
        {
            AirlineManager airlineManager = new AirlineManager();
            try
            {
                string flightFile = @"..\..\..\UserData\FlightData.csv";
                string ticketFile = @"..\..\..\UserData\TicketData.csv";
                string allDataFile = @"..\..\..\UserData\AirlineData.csv";

                airlineManager.ImportFlightsFromCSV(flightFile);
                airlineManager.ImportTicketsFromCSV(ticketFile);

                if (File.Exists(allDataFile))
                    airlineManager.importFormExcel(allDataFile);

                Console.WriteLine("All data loaded successfully");
                return airlineManager;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return airlineManager;
            }
        }

        static void BookTicket()
        {
            AirlineManager airlineManager = LoadingData();

            try
            {
                Console.WriteLine("Fill in the information");

                Console.Write("Full Name: ");
                string name = Console.ReadLine();

                Console.Write("Email: ");
                string email = Console.ReadLine();

                Console.Write("Phone Number: ");
                string phoneNumber = Console.ReadLine();

                Console.Write("Age: ");
                if (!int.TryParse(Console.ReadLine(), out int age))
                {
                    Console.WriteLine("Invalid age");
                    return;
                }

                Console.Write("Gender (m/f/u): ");
                string g = Console.ReadLine();
                if (string.IsNullOrEmpty(g)) { Console.WriteLine("Invalid gender"); return; }
                char gender = char.ToLower(g[0]);

                var newPassenger = new Passenger(name, email, gender, age, phoneNumber);

                airlineManager.showAllFlight();

                Console.Write("\nEnter Flight Number: ");
                string selectedFlightNumber = Console.ReadLine();

                var selectedFlight = airlineManager.Flights.FirstOrDefault(f => f.GetFlightNumber() == selectedFlightNumber);
                if (selectedFlight == null) { Console.WriteLine("Flight not found!"); return; }
                if (selectedFlight.GetAvailableSeats() <= 0) { Console.WriteLine("No available seats on this flight!"); return; }

                Console.WriteLine("\nTicket Types:");
                Console.WriteLine("e - Economy");
                Console.WriteLine("b - Business");
                Console.WriteLine("f - First Class");
                Console.Write("Choose ticket type (e/b/f): ");
                string tt = Console.ReadLine();
                if (string.IsNullOrEmpty(tt)) { Console.WriteLine("Invalid ticket type"); return; }
                char ticketType = char.ToLower(tt[0]);
                selectedFlight.ShowEmptySeats(ticketType);
                Ticket newTicket = null;
                Console.Write("\nEnter seat number you want to book: ");
                if (!int.TryParse(Console.ReadLine(), out int seatNumber))
                {
                    Console.WriteLine("Invalid seat number!");
                    return;
                }
                if (!selectedFlight.IsSeatAvailable(ticketType, seatNumber))
                {
                    Console.WriteLine("Seat not available or not in this class!");
                    return;
                }

                switch (ticketType)
                {
                    case 'e':
                        newTicket = new EconomyTicket(newPassenger, selectedFlight,seatNumber);
                        break;
                    case 'b':
                        newTicket = new BusinessTicket(newPassenger, selectedFlight,seatNumber);
                        break;
                    case 'f':
                        newTicket = new FirstClassTicket(newPassenger, selectedFlight,seatNumber);
                        break;
                    default:
                        Console.WriteLine("Invalid ticket type!");
                        return;
                }

                double price = newTicket.TicketPrice;
                Console.WriteLine($"\nTicket price: {price} USD");

                // Payment step
                Console.WriteLine("\nChoose Payment Method:");
                Console.WriteLine("1 - Credit Card");
                Console.WriteLine("2 - E-Wallet");
                Console.WriteLine("3 - Cash");
                Console.Write("Option: ");
                string payChoice = Console.ReadLine();
                Payment payment = null;
                switch (payChoice)
                {
                    case "1":
                        Console.Write("Enter card number: ");
                        string card = Console.ReadLine();
                        payment = new CreditCardPayment(price, card);
                        break;
                    case "2":
                        Console.Write("Enter wallet id: ");
                        string wid = Console.ReadLine();
                        payment = new EwalletPayment(price, wid);
                        break;
                    case "3":
                        payment = new CashPayment(price);
                        break;
                    default:
                        Console.WriteLine("Invalid payment option");
                        return;
                }

                bool paid = payment.Process();
                payment.Print();

                if (!paid)
                {
                    Console.WriteLine("Payment failed. Booking cancelled.");
                    return;
                }

                // thanh toán thành công -> book seat and save
                if (selectedFlight.BookSeat())
                {
                    airlineManager.addPassenger(newPassenger); 
                    airlineManager.addTicket(newTicket);

                    Console.WriteLine("\n=== BOOKING SUCCESSFUL ===");
                    Console.WriteLine("Passenger Info:");
                    newPassenger.Print();
                    Console.WriteLine("Ticket Info:");
                    newTicket.Print();
                    Console.WriteLine("Flight Info:");
                    selectedFlight.Print();

                    string dataDir = @"..\..\..\UserData";
                    Directory.CreateDirectory(dataDir);
                    airlineManager.ExportAirlineData(Path.Combine(dataDir, "AirlineData.csv"));
                    airlineManager.ExportTicketsToCSV(Path.Combine(dataDir, "TicketData.csv"));
                    airlineManager.ExportFlightsToCSV(Path.Combine(dataDir, "FlightData.csv"));
                }
                else
                {
                    Console.WriteLine("Failed to book seat!");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        static void AirlineManagerMenu()
        {
            while (true)
            {
                AirlineManager airlineManager = LoadingData();
                Console.WriteLine("\n=== AIRLINE MANAGER ===");
                Console.WriteLine("1. Show All Flights");
                Console.WriteLine("2. Show All Passengers");
                Console.WriteLine("3. Show All Tickets");
                Console.WriteLine("4. Add New Flight");
                Console.WriteLine("5. Add New Passenger");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Choose option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        airlineManager.showAllFlight();
                        break;
                    case "2":
                        airlineManager.showAllPassenger();
                        break;
                    case "3":
                        airlineManager.showAllTicket();
                        break;
                    case "4":
                        AddNewFlight(airlineManager);
                        break;
                    case "5":
                        AddNewPassenger(airlineManager);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void AddNewFlight(AirlineManager airlineManager)
        {
            try
            {
                Console.WriteLine("\n=== ADD NEW FLIGHT ===");

                Console.Write("Flight Number: ");
                string flightNumber = Console.ReadLine();

                Console.Write("Departure City: ");
                string departure = Console.ReadLine();

                Console.Write("Destination City: ");
                string destination = Console.ReadLine();

                Console.Write("Departure Date and Time (yyyy-MM-dd HH:mm): ");
                DateTime departureTime = DateTime.Parse(Console.ReadLine());

                Console.Write("Available Seats: ");
                int seats = Convert.ToInt32(Console.ReadLine());

                Flight newFlight = new Flight(flightNumber, departure, destination, departureTime, seats);
                airlineManager.addFlight(newFlight);

                Console.WriteLine("Flight added successfully!");
                newFlight.Print();

                string flightFile = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "UserData", "FlightData.csv");
                airlineManager.ExportFlightsToCSV(flightFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding flight: {ex.Message}");
            }
        }

        static void AddNewPassenger(AirlineManager airlineManager)
        {
            try
            {
                Console.WriteLine("\n=== ADD NEW PASSENGER ===");

                Console.Write("Full Name: ");
                string name = Console.ReadLine();

                Console.Write("Email: ");
                string email = Console.ReadLine();

                Console.Write("Phone Number (10 digits): ");
                string phoneNumber = Console.ReadLine();

                Console.Write("Age: ");
                int age = Convert.ToInt32(Console.ReadLine());

                Console.Write("Gender (m/f/u): ");
                char gender = Convert.ToChar(Console.ReadLine().ToLower());

                Passenger newPassenger = new Passenger(name, email, gender, age, phoneNumber);
                airlineManager.addPassenger(newPassenger);

                Console.WriteLine("Passenger added successfully!");
                newPassenger.Print();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding passenger: {ex.Message}");
            }
        }
    }
}
