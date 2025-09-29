using AirlineTicketSystem;
using System.Windows;
using System.Windows.Controls;

namespace AirlineSystem
{
    public partial class MainWindow : Window
    {
        private AirlineManager manager;
        public MainWindow()
        {
            InitializeComponent();
            manager = new AirlineManager();
            string flightFile = @"..\..\..\UserData\FlightData.csv";
            string passengerFile = @"..\..\..\UserData\Passenger.csv"; 
            string ticketFile = @"..\..\..\UserData\TicketData.csv";
            string allDataFile = @"..\..\..\UserData\AirlineData.csv";

            // Import in correct order
            manager.ImportFlightsFromCSV(flightFile);
            manager.ImportPassengerFormCSV(passengerFile); // Import passengers first
            manager.ImportTicketsFromCSV(ticketFile); // Then tickets
            manager.importFormExcel(allDataFile); // This will add more data

            Console.WriteLine($"Loaded: {manager.GetTotalFlights()} flights, {manager.GetTotalPassengers()} passengers, {manager.GetTotalTickets()} tickets");

        }

        private void Load_BookTicket(object sender, RoutedEventArgs e)
        {
            try
            {
                FullScreenBT.Content = new BookTicket();
                ShowFullScreenMode(); // Ẩn menu, content → chỉ hiển thị BookTicket full cửa sổ
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error loading Book Ticket: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Load_Manager(object sender, RoutedEventArgs e)
        {
            try
            {
                FullScreenBT.Content = new Manager(manager); // Use the field
                ShowFullScreenMode();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error loading Manager: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
       
        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            BackToMenu();
        }

        public void BackToMenu()
        {
            FullScreenBT.Content = null;
            ShowMenuMode();
        }

        public void NavigateToFullScreen(UserControl content)
        {
            FullScreenBT.Content = content;
            ShowFullScreenMode();
        }

        private void ShowMenuMode()
        {
            // Show menu panel and content area
            MenuPanel.Visibility = Visibility.Visible;
            FullScreenContent.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Collapsed;
        }

        private void ShowFullScreenMode()
        {
            // Hide menu panel, show full screen content
            MenuPanel.Visibility = Visibility.Visible;
            FullScreenContent.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
            FooterBorder.Visibility = Visibility.Collapsed;
            //HeaderBorder.Visibility = Visibility.Collapsed;
        }
       
    }
}