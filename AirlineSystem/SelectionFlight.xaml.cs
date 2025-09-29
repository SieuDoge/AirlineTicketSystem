using AirlineTicketSystem;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Media3D;

namespace AirlineSystem
{
    public partial class SelectionFlight : Window
    {
        private AirlineManager airlineManager;
        public event Action<Flight> FlightSelected;

        public ObservableCollection<FlightViewModel> FlightList { get; set; }

        public SelectionFlight(AirlineManager manager)
        {
            InitializeComponent();
            airlineManager = manager;
            FlightList = new ObservableCollection<FlightViewModel>();
            LoadFlights();
            lstFlights.ItemsSource = FlightList;

        }

        private void LoadFlights()
        {
            try
            {
                FlightList.Clear();

                foreach (var flight in airlineManager.Flights.Where(f => f.GetAvailableSeats() > 0))
                {
                    FlightList.Add(new FlightViewModel
                    {
                        FlightName = flight.GetFlightNumber(),
                        Route = $"{flight.GetDeparture()} → {flight.GetDestination()}",
                        DepartureTime = flight.GetDepartureTime().ToString("MM-dd-yyyy\nHH:mm"), // .ToString("HH:mm")
                        AvailableSeats = flight.GetAvailableSeats(),
                        Price = "$200+", // Base price, actual price depends on class
                        Flight = flight
                    });
                }

                if (FlightList.Count == 0)
                {
                    MessageBox.Show("No flights available at the moment.", "No Flights",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading flights: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectFlight_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (lstFlights.SelectedItem is FlightViewModel selectedFlightVM)
                {
                    FlightSelected?.Invoke(selectedFlightVM.Flight);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please select a flight first.", "No Selection",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting flight: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }

    public class FlightViewModel
    {
        public string FlightName { get; set; }
        public string Route { get; set; }
        public string DepartureTime { get; set; }
        
        public int AvailableSeats { get; set; }
        public string Price { get; set; }
        public Flight Flight { get; set; }
    }
}