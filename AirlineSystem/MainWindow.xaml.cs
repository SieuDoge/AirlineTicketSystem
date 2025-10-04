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
            LoadAllData();
            WireUpSearchButton();
        }

        private void LoadAllData()
        {
            string flightFile = @"..\..\..\UserData\FlightData.csv";
            string passengerFile = @"..\..\..\UserData\Passenger.csv";
            string ticketFile = @"..\..\..\UserData\TicketData.csv";
            string allDataFile = @"..\..\..\UserData\AirlineData.csv";

            // Import in correct order
            manager.ImportFlightsFromCsv(flightFile);
            manager.ImportPassengerFormCsv(passengerFile);
            manager.ImportTicketsFromCsv(ticketFile);
            manager.ImportFormExcel(allDataFile);

            // Update flight statuses based on current time
            manager.UpdateFlightStatuses();

            Console.WriteLine($"Loaded: {manager.GetTotalFlights()} flights, {manager.GetTotalPassengers()} passengers, {manager.GetTotalTickets()} tickets");
        }

        // Add method to wire up search button
        private void WireUpSearchButton()
        {
            SearchButton.Click += Load_Search;
        }

        // Thêm method để reload data sau khi book ticket
        public void ReloadData()
        {
            LoadAllData();
        }

        private void Load_BookTicket(object sender, RoutedEventArgs e)
        {
            try
            {
                // QUAN TRỌNG: Truyền manager chung vào BookTicket
                FullScreenBT.Content = new BookTicket(manager);
                ShowFullScreenMode();
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
                // Reload data trước khi vào Manager để đảm bảo data mới nhất
                ReloadData();
                FullScreenBT.Content = new Manager(manager);
                ShowFullScreenMode();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error loading Manager: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Add Search loading method
        private void Load_Search(object sender, RoutedEventArgs e)
        {
            try
            {
                // Reload data để đảm bảo có data mới nhất
                ReloadData();
                FullScreenBT.Content = new Search(manager);
                ShowFullScreenMode();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error loading Search: {ex.Message}", "Error",
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
            MenuPanel.Visibility = Visibility.Visible;
            FullScreenContent.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Collapsed;
        }

        private void ShowFullScreenMode()
        {
            MenuPanel.Visibility = Visibility.Visible;
            FullScreenContent.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Visible;
        }
    }
}