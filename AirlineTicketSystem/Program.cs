using System;

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
                string flightFile =
                    "F:\\Project\\OOP-Project\\AirlineTicketSystem\\AirlineTicketSystem\\UserData\\FlightData.csv";
                string ticketFile =
                    "F:\\Project\\OOP-Project\\AirlineTicketSystem\\AirlineTicketSystem\\UserData\\TicketData.csv";
                string allDataFile =
                    "F:\\Project\\OOP-Project\\AirlineTicketSystem\\AirlineTicketSystem\\UserData\\AirlineData.csv";

                airlineManager.ImportFlightsFromCSV(flightFile);
                airlineManager.ImportTicketsFromCSV(ticketFile);

                if (File.Exists(allDataFile))
                {
                    airlineManager.importFormExcel(allDataFile);
                }

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
    AirlineManager airlineManager = LoadingData(); // chỉ dùng 1 biến

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
        int age = Convert.ToInt32(Console.ReadLine());

        Console.Write("Gender (m/f/u): ");
        char gender = Convert.ToChar(Console.ReadLine().ToLower());

        Passenger newPassenger = new Passenger(name, email, gender, age, phoneNumber);

        Console.WriteLine("\nFlight list: ");
        airlineManager.showAllFlight();

        Console.Write("\nEnter Flight Number: ");
        string selectedFlightNumber = Console.ReadLine();

        Flight selectedFlight =
            airlineManager.Flights.FirstOrDefault(f => f.GetFlightNumber() == selectedFlightNumber);

        if (selectedFlight == null)
        {
            Console.WriteLine("Flight not found!");
            return;
        }

        if (selectedFlight.GetAvailableSeats() <= 0)
        {
            Console.WriteLine("No available seats on this flight!");
            return;
        }

        Console.WriteLine("\nTicket Types:");
        Console.WriteLine("e - Economy ($200)");
        Console.WriteLine("b - Business ($400)");
        Console.WriteLine("f - First Class ($800)");
        Console.Write("Choose ticket type (e/b/f): ");
        char ticketType = Convert.ToChar(Console.ReadLine().ToLower());

        double price = 0;
        switch (ticketType)
        {
            case 'e': price = 200; break;
            case 'b': price = 400; break;
            case 'f': price = 800; break;
            default:
                Console.WriteLine("Invalid ticket type!");
                return;
        }

        Random rnd = new Random();
        string ticketId = "TK" + rnd.Next(100000, 999999);

        Ticket newTicket = new Ticket(ticketId, price, ticketType, phoneNumber);
        newTicket.Flight = selectedFlightNumber;

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

            string dataDir = "F:\\Project\\OOP-Project\\AirlineTicketSystem\\AirlineTicketSystem\\UserData\\";
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
                /*Console.WriteLine("6. Export All Data");
                Console.WriteLine("7. Export Flights Only");
                Console.WriteLine("8. Export Tickets Only");*/
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
                    /*case "6":
                        SaveAllData(airlineManager);
                        break;
                    case "7":
                        string flightFile = "F:\\Project\\OOP-Project\\AirlineTicketSystem\\AirlineTicketSystem\\UserData\\FlightData.csv";
                        airlineManager.ExportFlightsToCSV(flightFile);
                        break;
                    case "8":
                        string ticketFile = "F:\\Project\\OOP-Project\\AirlineTicketSystem\\AirlineTicketSystem\\UserData\\TicketData.csv";
                        airlineManager.ExportTicketsToCSV(ticketFile);
                        break;*/
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

                // Save to FlightData.csv
                Console.Write("\nSuccessfully added flight");

                string flightFile =
                    "F:\\Project\\OOP-Project\\AirlineTicketSystem\\AirlineTicketSystem\\UserData\\FlightData.csv";
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

        static void SaveAllData(AirlineManager airlineManager)
        {
            try
            {
                string flightFile =
                    "F:\\Project\\OOP-Project\\AirlineTicketSystem\\AirlineTicketSystem\\UserData\\FlightData.csv";
                string ticketFile =
                    "F:\\Project\\OOP-Project\\AirlineTicketSystem\\AirlineTicketSystem\\UserData\\Ticket.csv";
                string allDataFile =
                    "F:\\Project\\OOP-Project\\AirlineTicketSystem\\AirlineTicketSystem\\UserData\\AirlineData.csv";

                airlineManager.ExportFlightsToCSV(flightFile);
                airlineManager.ExportTicketsToCSV(ticketFile);
                airlineManager.ExportAirlineData(allDataFile);

                Console.WriteLine("All data saved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }
    }
}