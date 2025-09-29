using AirlineTicketSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AirlineSystem
{
    /// <summary>
    /// Interaction logic for Manager.xaml
    /// </summary>
    public partial class Manager : UserControl
    {
        private Passenger passenger;
        private Ticket ticket;
        private Flight flight;
        private AirlineManager airlinemanager;

        public Manager(AirlineManager airlinemanager)
        {
            InitializeComponent();
            this.airlinemanager = airlinemanager;
            this.DataContext = new ManagerViewModel(airlinemanager);
        }

        public class ManagerViewModel
        {
            public int TotalFlights { get; }
            public int TotalPassengers { get; }
            public int TotalTickets { get; }
            public decimal TotalRevenue { get; }

            public ManagerViewModel(AirlineManager airlineManager)
            {
                TotalFlights = airlineManager.GetTotalFlights();
                TotalPassengers = airlineManager.GetTotalPassengers();
                TotalTickets = airlineManager.GetTotalTickets();
                TotalRevenue = airlineManager.GetTotalRevenue();
            }
        }

        // Navigation methods to DataList views
        private void ShowAllFlights_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            var flightDataList = new DataList(DataList.DataType.Flights, airlinemanager);
            mainWindow?.NavigateToFullScreen(flightDataList);
        }

        private void ShowAllPassengers_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            var passengerDataList = new DataList(DataList.DataType.Passengers, airlinemanager);
            mainWindow?.NavigateToFullScreen(passengerDataList);
        }

        private void ShowAllTickets_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            var ticketDataList = new DataList(DataList.DataType.Tickets, airlinemanager);
            mainWindow?.NavigateToFullScreen(ticketDataList);
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.BackToMenu();
        }

    }
}