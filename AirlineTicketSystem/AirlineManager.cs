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

    public bool addFlight(Flight flight)
    {
        if (flight == null)
        {
            throw new ArgumentNullException("flight can't be null");
        }

        // Check if flight already exists
        foreach (var existingFlight in flights)
        {
            if (existingFlight.GetFlightNumber() == flight.GetFlightNumber())
            {
                Console.WriteLine("Flight already exists");
                return false;
            }
        }
        flights.Add(flight);     
        return true;
    }

    public bool addPassenger(Passenger passenger)
    {
        if (passenger == null)
        {
            throw new ArgumentNullException("passenger can't be null");
        }
        
        // Check if passenger already exists (by phone number)
        foreach (var existingPassenger in passengers)
        {
            if (existingPassenger.PhoneNumber == passenger.PhoneNumber)
            {
                Console.WriteLine("Passenger already exists");
                return false;
            }
        }
        passengers.Add(passenger);
        return true;
    }
    
    public bool addTicket(Ticket ticket)
    {
        if (ticket == null)
        {
            throw new ArgumentNullException("ticket can't be null");
        }
        
        // Check if ticket already exists
        foreach (var existingTicket in tickets)
        {
            if (existingTicket.TicketId == ticket.TicketId)
            {
                Console.WriteLine("Ticket already exists");
                return false;
            }
        }
        tickets.Add(ticket);
        return true;
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

    // Fixed ExportAirlineData - Only export complete booking records
    public void ExportAirlineData(string filename)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(filename, true)) // false = overwrite
            {
                // Write header
                // sw.WriteLine("TicketId,Name,Email,Phone,Gender,Age,FlightNumber,Departure,Destination,DepartureDate,SeatsNumber,TicketType,Price");
                
                int recordCount = 0;
                
                // Only export tickets that have corresponding passengers and flights
                foreach (var ticket in tickets)
                {
                    // Find passenger by phone number
                    var passenger = passengers.FirstOrDefault(p => p.PhoneNumber == ticket.PassengerPhone);
                    if (passenger == null) continue;
                    
                    // Find flight by flight number
                    var flight = flights.FirstOrDefault(f => f.GetFlightNumber() == ticket.Flight);
                    if (flight == null) continue;
                    
                    // Write complete record
                    sw.WriteLine($"{ticket.TicketId},{passenger.Name},{passenger.Email},{passenger.PhoneNumber}," +
                                 $"{passenger.Gender},{passenger.Age},{flight.GetFlightNumber()},{flight.GetDeparture()}," +
                                 $"{flight.GetDestination()},{flight.GetDepartureTime():yyyy-MM-dd HH:mm}," +
                                 $"{flight.GetAvailableSeats()},{ticket.TicketType},{ticket.TicketPrice}");
                    recordCount++;
                }
                
                Console.WriteLine($"Exported {recordCount} complete records to {filename}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Export failed: {ex.Message}");
        }
    }
    
    // Fixed importFromExcel - Clear existing data and avoid duplicates
    public void importFormExcel(string filename)
    {
        try
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("File not found", filename);
            }

            using StreamReader sr = new StreamReader(filename);
            // Skip header
            string headerLine = sr.ReadLine();
            if (headerLine == null)
            {
                throw new ArgumentException("File Data can't be Empty");
            }

            string line;
            int importCount = 0;
            int skipCount = 0;
            
            while ((line = sr.ReadLine()) != null)
            {
                string[] columns = line.Split(',');
                try
                {
                    if (columns.Length >= 13)
                    {
                        string ticketId = columns[0].Trim();
                        string name = columns[1].Trim();
                        string email = columns[2].Trim();
                        string phone = columns[3].Trim();
                        char gender = Convert.ToChar(columns[4].Trim());
                        int age = Convert.ToInt32(columns[5].Trim());
                        string flightNumber = columns[6].Trim();
                        string departure = columns[7].Trim();
                        string destination = columns[8].Trim();
                        DateTime departureDate = DateTime.Parse(columns[9].Trim());
                        int availableSeats = Convert.ToInt32(columns[10].Trim());
                        char ticketType = Convert.ToChar(columns[11].Trim());
                        double price = Convert.ToDouble(columns[12].Trim());

                        // Create and add passenger
                        addPassenger(new Passenger(name, email, gender, age, phone));

                        // Create and add flight
                        addFlight(new Flight(flightNumber, departure, destination, departureDate, availableSeats));

                        // Create and add ticket (check for duplicates)
                        addTicket(new Ticket(ticketId, price, ticketType, phone));

                        importCount++;
                    }
                    else
                    {
                        Console.WriteLine($"Skip line: {line}");
                        skipCount++;
                    }
                }
                catch(FormatException ex)
                {
                    Console.WriteLine($"Error parsing line: {line} - {ex.Message}");
                    skipCount++;
                }
            }
            
            Console.WriteLine($"Imported {importCount} records, skipped {skipCount} lines from {filename}");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Import failed: {ex.Message}");
        }
    }
    
    public void ImportFlightsFromCSV(string filename)
    {
        try
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine($"Flight file {filename} not found, skipping...");
                return;
            }


            using StreamReader sr = new StreamReader(filename);
            string headerLine = sr.ReadLine(); // Skip header
        
            string line;
            int importCount = 0;
            int skipCount = 0;
        
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
                        if (addFlight(new Flight(flightNumber, departure, destination, departureTime, availableSeats)))
                        {
                            importCount++;
                        }
                        else
                        {
                            skipCount++;
                        } 
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error parsing flight line: {line} - {ex.Message}");
                    skipCount++;
                }
            }
        
            Console.WriteLine($"Imported {importCount} flights, skipped {skipCount} duplicates from {filename}");
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
            string headerLine = sr.ReadLine(); // Skip header
        
            string line;
            int importCount = 0;
            int skipCount = 0;
        
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

                        if (addTicket(new Ticket(ticketId, price, ticketType, passengerPhone)))
                        {
                            importCount++;
                        }
                        else
                        {
                            skipCount++;
                        }
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error parsing ticket line: {line} - {ex.Message}");
                    skipCount++;
                }
            }
        
            Console.WriteLine($"Imported {importCount} tickets, skipped {skipCount} duplicates from {filename}");
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
    public void ExportPassengerToCSV(string filename)
    {
        try
        {
            string ticketId = tickets.First().TicketId;
            using (StreamWriter sw = new StreamWriter(filename, false))
            {
                sw.WriteLine("Name,Email,PhoneNumber,Gender,Age,TicketID");
            
                foreach (var Passenger in passengers)
                {
                    sw.WriteLine($"{Passenger.Name}, {Passenger.Email}, {Passenger.PhoneNumber}, {Passenger.Gender}, {Passenger.Age}, {ticketId}");
                }
            
                Console.WriteLine($"Exported {passengers.Count} Passenger to {filename}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Export Passenger failed: {ex.Message}");
        }
    }
}