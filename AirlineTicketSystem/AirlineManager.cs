namespace AirlineTicketSystem;

public class AirlineManager
{
    private List<Flight> flights;
    private List<Passenger> passengers;
    private List<Ticket> tickets;

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
    /*public void bookTicket(Flight flight,  Passenger passenger)
    {
        
    }*/

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
public void ExportToExcel(string filename)
{
    try
    {
        // Đọc tất cả TicketId hiện có trong file để tránh duplicate
        HashSet<string> existingTicketIds = new HashSet<string>();
        
        if (File.Exists(filename))
        {
            string[] existingLines = File.ReadAllLines(filename);
            for (int i = 1; i < existingLines.Length; i++) // Bỏ qua header
            {
                string[] columns = existingLines[i].Split(',');
                if (columns.Length > 0)
                {
                    existingTicketIds.Add(columns[0].Trim());
                }
            }
        }
        else
        {
            // Tạo file mới với header nếu file chưa tồn tại
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine("TicketId,Name,Email,Phone,Gender,Age,FlightNumber,Departure,Destination,DepartureDate,AvailableSeats,TicketType,Price");
            }
        }

        // Append chỉ những ticket mới vào file
        using (StreamWriter sw = new StreamWriter(filename, append: true))
        {
            int newRecords = 0;
            
            foreach (var ticket in tickets)
            {
                // Chỉ ghi những ticket chưa có trong file
                if (!existingTicketIds.Contains(ticket.TicketId))
                {
                    // Tìm passenger theo phone number
                    var passenger = passengers.FirstOrDefault(p => p.PhoneNumber == ticket.PassengerPhone);
                    
                    // Tìm flight theo flight name
                    var flight = flights.FirstOrDefault(f => f.GetFlightNumber() == ticket.Flight);
                    
                    if (passenger != null && flight != null)
                    {
                        sw.WriteLine($"{ticket.TicketId},{passenger.Name},{passenger.Email},{passenger.PhoneNumber}," +
                                   $"{passenger.Gender},{passenger.Age},{flight.GetFlightNumber()},{flight.GetDeparture()}," +
                                   $"{flight.GetDestination()},{flight.GetDepartureTime():yyyy-MM-dd HH:mm}," +
                                   $"{flight.GetAvailableSeats()},{ticket.TicketType},{ticket.TicketPrice}");
                        newRecords++;
                    }
                }
            }
            
            Console.WriteLine($"Đã thêm {newRecords} record mới vào file {filename}");
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
                        DateTime DepartureDate = Convert.ToDateTime(columns[9].Trim());
                        int AvailableSeats = Convert.ToInt32(columns[10].Trim());
                        char TicketType = Convert.ToChar(columns[11]);
                        double Price = Convert.ToDouble(columns[12].Trim());
                    
                        Passenger addPassenger = new Passenger(Name, Email, Gender, Age, Phone);
                        passengers.Add(addPassenger);
                        Flight addFlight = new Flight(FlightNumber, Departure, Destination, DepartureDate, AvailableSeats); 
                        flights.Add(addFlight);
                        Ticket addTicket = new Ticket(tickId, FlightNumber, Price, TicketType, Phone);
                        tickets.Add(addTicket);

                        importCount++;
                    }
                    else
                    {
                        Console.WriteLine($"Skip line: {line}, Total skipped: {skip++}");
                    }
                }
                catch(Exception ex)
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
}