namespace AirlineTicketSystem;

public class AirlineManager
{
    private List<Flight> flights;
    private List<Passenger> passenger;
    private List<Ticket> ticket;

    public AirlineManager()
    {
        flights = new List<Flight>();
        passenger = new List<Passenger>();
        ticket = new List<Ticket>();
    }

    public void addFlight(Flight flight)
    {
        if (flight == null)
        {
            throw new ArgumentNullException("flight can't be null");
        }

        foreach (var exitsingFlight in flights)
        {
            //if (exitsingFlight.flightNumber == flights.Number)
            {
                
            }
        }

        flights.Add(flight);
    }

    public void addPassenger(Passenger passenger)
    {
        
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

    public void importFormExcel()
    {
        
    }
}