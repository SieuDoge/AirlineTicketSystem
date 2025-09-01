namespace AirlineTicketSystem;

public class AirlineManager
{
    private List<Flight> flights;
    private List<Passenger> passengers;
    private List<Ticket> tickets;
    
    public List<Flight> Flights => flights;
    public List<Passenger> Passengers => passengers;
    public List<Ticket> Tickets => tickets;

    public AirlineManager()
    {
        flights = new List<Flight>();
        passengers = new List<Passenger>();
        tickets = new List<Ticket>();
    }

    public void addFlight(Flight flight)
    {
        if (flight == null)
        {
            throw new ArgumentNullException("flight can't be null");
        }

        foreach (var exitsingFlight in flights)
        {
            if (exitsingFlight.GetFlightNumber == flight.GetFlightNumber)
            {
                Console.WriteLine("Flight already exists");
            }
        }
        flights.Add(flight);
    }

    public void addPassenger(Passenger passenger)
    {
        if (passenger == null)
        {
            throw new ArgumentNullException("passenger can't be null");
        }
        foreach (var exitsingPassenger in passengers)
        {
            if (exitsingPassenger.PhoneNumber == passenger.PhoneNumber)
            {
                Console.WriteLine("Flight already exists");
            }
        }
        passengers.Add(passenger);
    }
    public void addTicket(Ticket ticket)
    {
        if (ticket == null)
        {
            throw new ArgumentNullException("ticket can't be null");
        }
        foreach (var exitsingTicketID in tickets)
        {
            if (exitsingTicketID.TicketId == ticket.TicketId)
            {
                Console.WriteLine("Flight already exists");
            }
        }
        tickets.Add(ticket);
    }

    public void showAllFlight()
    {
        Console.WriteLine("Flight list:");
        for (int i = 0; i < flights.Count; i++)
        {
            flights[i].Print();
        }
        Console.WriteLine($"Total: {flights.Count}");
    }

    public void showAllPassenger()
    {
        Console.WriteLine("Passenger list:");
        for (int i = 0; i < passengers.Count; i++)
        {
            passengers[i].Print();
        }
        Console.WriteLine($"Total: {passengers.Count}");
    }

    public void showAllTicket()
    {
        Console.WriteLine("Ticket list:");
        for (int i = 0; i < tickets.Count; i++)
        {
            tickets[i].Print();
        }
        Console.WriteLine($"Total: {tickets.Count}");
    }

    // Function ExportToExcel - Append data vào file CSV gốc
    public void ExportAirlineData(string filename)
    {
        try
        {
            
            if (!File.Exists(filename))
            {
                // Tạo file mới với header nếu chưa có
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.WriteLine("TicketId,Name,Email,Phone,Gender,Age,FlightNumber,Departure,Destination,DepartureDate,AvailableSeats,TicketType,Price");
                }
            }

            
            using (StreamWriter sw = new StreamWriter(filename, true)) // true = append
            {
                int recordCount = 0;
            
                // Ghi tất cả tickets, passengers, flights hiện có
                for (int i = 0; i < tickets.Count; i++)
                {
                    var ticket = tickets[i];
                    var passenger = passengers[i]; 
                    var flight = flights[i];
                
                    sw.WriteLine($"{ticket.TicketId},{passenger.Name},{passenger.Email},{passenger.PhoneNumber}," +
                                 $"{passenger.Gender},{passenger.Age},{flight.GetFlightNumber()},{flight.GetDeparture()}," +
                                 $"{flight.GetDestination()},{flight.GetDepartureTime():yyyy-MM-dd HH:mm}," +
                                 $"{flight.GetAvailableSeats()},{ticket.TicketType},{ticket.TicketPrice}");
                    recordCount++;
                }
            
                Console.WriteLine($"Đã thêm {recordCount} records vào file {filename}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Export thất bại: {ex.Message}");
        }
    }
    public void importFormExcel(string filename)
    {
        try
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("File not found", filename);
            }

            using StreamReader sr = new StreamReader(filename);
            // Bỏ qua tên cột
            string Headerline = sr.ReadLine();
            if (Headerline == null)
            {
                throw new ArgumentException("File Data can't be Empty");
            }

            string line;
            int importCount = 0;
            int skip = 0;
            while ((line = sr.ReadLine()) != null)
            {
                string[] columns = line.Split(',');
                try
                {
                    if (columns.Length >= 13)
                    {
                        string tickId = columns[0].Trim();
                        string Name = columns[1].Trim();
                        string Email = columns[2].Trim();
                        string Phone = columns[3].Trim();
                        char Gender = Convert.ToChar(columns[4]);
                        int Age = Convert.ToInt32(columns[5].Trim());
                        string FlightNumber = columns[6].Trim();
                        string Departure = columns[7].Trim();
                        string Destination =  columns[8].Trim();
                        DateTime departureDate = DateTime.Parse(columns[9].Trim());
                        int AvailableSeats = Convert.ToInt32(columns[10].Trim());
                        char TicketType = Convert.ToChar(columns[11]);
                        double Price = Convert.ToDouble(columns[12].Trim());
                    
                        
                        Passenger addPassenger = new Passenger(Name, Email, Gender, Age, Phone);
                        passengers.Add(addPassenger);
                        
                        Ticket addTicket = new Ticket(tickId, Price, TicketType, Phone);
                        tickets.Add(addTicket);
                        
                        Flight addFlight = new Flight(FlightNumber, Departure, Destination, departureDate, AvailableSeats); 
                        flights.Add(addFlight);

                        importCount++;
                    }
                    else
                    {
                        Console.WriteLine($"Skip line: {line}, Total skipped: {skip++}");
                    }
                }
                catch(FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public void ImportFlightsFromCSV(string filename)
    {
        try
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("FlightData.csv not found", filename);
            }

            using StreamReader sr = new StreamReader(filename);
            string headerLine = sr.ReadLine(); // Bỏ qua header
        
            string line;
            int importCount = 0;
        
            while ((line = sr.ReadLine()) != null)
            {
                string[] columns = line.Split(',');
                try
                {
                    if (columns.Length >= 5)
                    {
                        string flightNumber = columns[0].Trim();
                        string departure = columns[1].Trim();
                        string destination = columns[2].Trim();
                        DateTime departureTime = DateTime.Parse(columns[3].Trim());
                        int availableSeats = Convert.ToInt32(columns[4].Trim());
                    
                        Flight flight = new Flight(flightNumber, departure, destination, departureTime, availableSeats);
                        addFlight(flight);
                        importCount++;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error parsing flight line: {line} - {ex.Message}");
                }
            }
        
            Console.WriteLine($"Imported {importCount} flights from {filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Import flights failed: {ex.Message}");
        }
    }
    public void ImportTicketsFromCSV(string filename)
    {
        try
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine($"Ticket file {filename} not found, skipping...");
                return;
            }

            using StreamReader sr = new StreamReader(filename);
            string headerLine = sr.ReadLine(); // Bỏ qua header
        
            string line;
            int importCount = 0;
        
            while ((line = sr.ReadLine()) != null)
            {
                string[] columns = line.Split(',');
                try
                {
                    if (columns.Length >= 4)
                    {
                        string ticketId = columns[0].Trim();
                        double price = Convert.ToDouble(columns[1].Trim());
                        char ticketType = Convert.ToChar(columns[2].Trim());
                        string passengerPhone = columns[3].Trim();
                    
                        Ticket ticket = new Ticket(ticketId, price, ticketType, passengerPhone);
                        addTicket(ticket);
                        importCount++;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error parsing ticket line: {line} - {ex.Message}");
                }
            }
        
            Console.WriteLine($"Imported {importCount} tickets from {filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Import tickets failed: {ex.Message}");
        }
    }
    public void ExportFlightsToCSV(string filename)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(filename, false))
            {
                sw.WriteLine("FlightNumber,Departure,Destination,DepartureTime,AvailableSeats");
            
                foreach (var flight in flights)
                {
                    sw.WriteLine($"{flight.GetFlightNumber()},{flight.GetDeparture()},{flight.GetDestination()}," +
                                 $"{flight.GetDepartureTime():yyyy-MM-dd HH:mm},{flight.GetAvailableSeats()}");
                }
            
                Console.WriteLine($"Exported {flights.Count} flights to {filename}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Export flights failed: {ex.Message}");
        }
    }
    public void ExportTicketsToCSV(string filename)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(filename, false))
            {
                sw.WriteLine("TicketId,Price,TicketType,PassengerPhone");
            
                foreach (var ticket in tickets)
                {
                    sw.WriteLine($"{ticket.TicketId},{ticket.TicketPrice},{ticket.TicketType},{ticket.PassengerPhone}");
                }
            
                Console.WriteLine($"Exported {tickets.Count} tickets to {filename}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Export tickets failed: {ex.Message}");
        }
    }
}