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
            if (exitsingFlight.flightNumber == flight.flightNumber) // Đợi file
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
        passengers.Add(passenger);
    }
    public void bookTicket(Flight flight,  Passenger passenger)
    {

    }

    public void showAllFlight() 
    {
        
    }

    public void showAllPassenger()
    {
        
    }

    public void showAllTicket()
    {
        
    }

    public void exportToExcel()
    {
        
    }

    public void importFormExcel(string filename)
    {
        try
        {
            using StreamReader sr = new(filename)
            {
                
            }
        }
        catch
        {
            
        }
    }
}