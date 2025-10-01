using AirlineTicketSystem;
using System.Windows;

namespace AirlineSystem
{
    public partial class TicketInfo : Window
    {
        private Passenger passenger;
        private Ticket ticket;
        private Flight flight;
        private AirlineManager airlineManager;

        public TicketInfo(AirlineManager airlineManager, Passenger passenger, Ticket ticket, Flight flight)
        {
            InitializeComponent();
            this.passenger = passenger;
            this.ticket = ticket;
            this.flight = flight;
            this.airlineManager = airlineManager;

            // Generate and save seat number if not already assigned
            if (string.IsNullOrEmpty(ticket.Seat))
            {
                ticket.Seat = GenerateSeatNumber(ticket.TicketTypeChar);
                SaveData(); // Save the updated ticket with seat number
            }

            this.DataContext = new TicketViewModel(passenger, flight, ticket);
        }

        // Move seat number generation to a separate method
        private string GenerateSeatNumber(char ticketType)
        {
            Random random = new Random();
            return ticketType switch
            {
                'f' => $"{random.Next(1, 10)}{(char)('A' + random.Next(0, 4))}", // First class: rows 1-9
                'b' => $"{random.Next(10, 30)}{(char)('A' + random.Next(0, 6))}", // Business: rows 10-29
                _ => $"{random.Next(30, 60)}{(char)('A' + random.Next(0, 6))}"   // Economy: rows 30-59
            };
        }

        public class TicketViewModel
        {
            public Passenger Passenger { get; }
            public Flight Flight { get; }
            public Ticket Ticket { get; }

            public double TotalAmount => Ticket.TicketPrice;
            public double TaxAmount => Math.Round(Ticket.TicketPrice * 0.1, 2);
            public DateTime DepartureTimePlus3 => Flight.DepartureTime.AddHours(3);
            public DateTime Boarding => Flight.DepartureTime.AddHours(-1);

            public string TicketClassName
            {
                get
                {
                    return Ticket.TicketTypeChar switch
                    {
                        'e' => "Economy",
                        'b' => "Business",
                        'f' => "First Class",
                        _ => "Economy"
                    };
                }
            }

            // Use the saved seat number from ticket
            public string SeatNumber => Ticket.Seat;

            // Not use
            private Random random = new Random();
            public string GenerateGate
            {
                get
                {
                    char gateLetter = (char)('A' + random.Next(0, 5)); // A-E
                    int gateNumber = random.Next(1, 20);
                    return $"{gateLetter}{gateNumber}";
                }
            }

            public TicketViewModel(Passenger passenger, Flight flight, Ticket ticket)
            {
                Passenger = passenger;
                Flight = flight;
                Ticket = ticket;
            }
        }

        private void SaveData()
        {
            try
            {
                string flightFile = @"..\..\..\UserData\FlightData.csv";
                string ticketFile = @"..\..\..\UserData\TicketData.csv";
                string allDataFile = @"..\..\..\UserData\AirlineData.csv";
                string Passenger = @"..\..\..\UserData\Passenger.csv";

                airlineManager.ExportAirlineData(allDataFile);
                airlineManager.ExportTicketsToCSV(ticketFile);
                airlineManager.ExportFlightsToCSV(flightFile);
                airlineManager.ExportPassengerToCSV(Passenger);
            }
            catch (Exception ex)
            {
                // Log error but don't stop the process
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        private void WireUpEvents()
        {

        }

        private void EmailTicket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // In a real application, you would implement email functionality
                MessageBox.Show($"Ticket has been sent to {passenger.Email}", "Email Sent",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending email: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DownloadPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // In a real application, you would generate and save a PDF
                MessageBox.Show("PDF ticket has been downloaded to your Downloads folder", "PDF Downloaded",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading PDF: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.BackToMenu();
        }
    }
}