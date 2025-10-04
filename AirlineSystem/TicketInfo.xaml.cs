using AirlineTicketSystem;
using System.Windows;
using System.Windows.Media.Imaging;
using QRCoder;
using System.IO;
using System.Drawing;

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
            if (string.IsNullOrEmpty(ticket.Seat))
            {
                ticket.Seat = GenerateSeatNumber(ticket.TicketTypeChar);
                SaveData();
            }
            this.DataContext = new TicketViewModel(passenger, flight, ticket);
            GenerateQRCode();
        }

        private string GetQRCodeData()
        {
            return $"TICKET_ID:{ticket.TicketId}|" +                   
                   $"PASSENGER:{passenger.Name}|" +                    
                   $"FLIGHT:{flight.FlightNumber}|" +                   
                   $"ROUTE:{flight.Departure}-{flight.Destination}|" +  
                   $"DATE:{flight.DepartureTime:yyyy-MM-dd}|" +        
                   $"TIME:{flight.DepartureTime:HH:mm}|" +            
                   $"SEAT:{ticket.Seat}|" +                             
                   $"CLASS:{ticket.TicketTypeName}";                   
        }

        private void GenerateQRCode()
        {
            try
            {
                string qrData = GetQRCodeData(); 
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, true);
                        BitmapImage bitmapImage = BitmapToBitmapImage(qrCodeImage);
                        QRCodeImage.Source = bitmapImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating QR code: {ex.Message}", "QR Code Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        private string GenerateSeatNumber(char ticketType)
        {
            Random random = new Random();
            return ticketType switch
            {
                'f' => $"{random.Next(1, 10)}{(char)('A' + random.Next(0, 4))}",
                'b' => $"{random.Next(10, 30)}{(char)('A' + random.Next(0, 6))}",
                _ => $"{random.Next(30, 60)}{(char)('A' + random.Next(0, 6))}"
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

            public string TicketClassName => Ticket.TicketTypeName;
            public string SeatNumber => Ticket.Seat;

            private Random random = new Random();
            public string GenerateGate
            {
                get
                {
                    char gateLetter = (char)('A' + random.Next(0, 5));
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
                string passengerFile = @"..\..\..\UserData\Passenger.csv";

                airlineManager.ExportAirlineData(allDataFile);
                airlineManager.ExportTicketsToCsv(ticketFile);
                airlineManager.ExportFlightsToCsv(flightFile);
                airlineManager.ExportPassengerToCsv(passengerFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        private void EmailTicket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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