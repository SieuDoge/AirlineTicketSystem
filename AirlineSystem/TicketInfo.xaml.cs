using AirlineTicketSystem;
using System.Windows;

namespace AirlineSystem
{
    public partial class TicketInfo : Window
    {
        private Passenger passenger;
        private Ticket ticket;
        private Flight flight;

        public TicketInfo(Passenger passenger, Ticket ticket, Flight flight)
        {
            InitializeComponent();
            this.passenger = passenger;
            this.ticket = ticket;
            this.flight = flight;

            this.DataContext = new TicketViewModel(passenger, flight, ticket);

        }


        public class TicketViewModel
        {
            public Passenger Passenger { get; }
            public Flight Flight { get; }

            public Ticket Ticket { get; }

            public double TotalAmount => Ticket.TicketPrice * 1.1;
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


            private Random random = new Random();

            public string SeatNumber
            {
                get
                {
                    return Ticket.TicketTypeChar switch
                    {
                        'f' => $"{random.Next(1, 4)}{(char)('A' + random.Next(0, 4))}", // First class: rows 1-3
                        'b' => $"{random.Next(4, 10)}{(char)('A' + random.Next(0, 6))}", // Business: rows 4-9
                        _ => $"{random.Next(10, 40)}{(char)('A' + random.Next(0, 6))}"   // Economy: rows 10-39
                    };
                }
            }




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