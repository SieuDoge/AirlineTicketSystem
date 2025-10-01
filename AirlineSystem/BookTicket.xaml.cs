using AirlineTicketSystem;
using System.Windows;
using System.Windows.Controls;

namespace AirlineSystem
{
    public partial class BookTicket : UserControl
    {
        private AirlineManager airlineManager;
        private Flight selectedFlight;

        // THAY ĐỔI: Constructor nhận airlineManager từ MainWindow
        public BookTicket(AirlineManager manager)
        {
            InitializeComponent();
            this.airlineManager = manager; // Sử dụng manager chung từ MainWindow

            // Wire up event handlers
            Select_Flight.Click += SelectFlight_Click;
            Checkout.Click += Checkout_Click;
            Back.Click += Back_Click;
            Exit.Click += Exit_Click;
        }

        private void SelectFlight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectionWindow = new SelectionFlight(airlineManager);
                selectionWindow.FlightSelected += OnFlightSelected;
                selectionWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening flight selection: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnFlightSelected(Flight flight)
        {
            selectedFlight = flight;
            Select_Flight.Content = $"✈️ {flight.GetFlightNumber()} - {flight.GetDeparture()} to {flight.GetDestination()} ✈️";
        }

        public string GetFlightNumber { get; set; }
        public string GetDeparture { get; set; }
        public string GetDestination { get; set; }

        public void FlightSelected(Flight flight)
        {
            GetFlightNumber = flight.GetFlightNumber();
            GetDeparture = flight.GetDeparture();
            GetDestination = flight.GetDestination();
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (!ValidateInput())
                    return;

                if (selectedFlight == null)
                {
                    MessageBox.Show("Please select a flight first!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (selectedFlight.GetAvailableSeats() <= 0)
                {
                    MessageBox.Show("No available seats on this flight!", "Booking Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Create passenger
                var passenger = CreatePassengerFromInput();
                if (passenger == null) return;

                // Create ticket based on selected class
                var ticket = CreateTicketFromSelection(passenger);
                if (ticket == null) return;

                // Navigate to checkout
                NavigateToCheckout(passenger, ticket);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during checkout: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Full_name.Text))
            {
                MessageBox.Show("Please enter your full name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                Full_name.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Phone_Number.Text))
            {
                MessageBox.Show("Please enter your phone number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                Phone_Number.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email.Text))
            {
                MessageBox.Show("Please enter your email.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                Email.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Age.Text) || !int.TryParse(Age.Text, out int age) || age < 0)
            {
                MessageBox.Show("Please enter a valid age.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                Age.Focus();
                return false;
            }

            if (!Male.IsChecked == true && !Female.IsChecked == true && !Unknown.IsChecked == true)
            {
                MessageBox.Show("Please select your gender.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!Eco.IsChecked == true && !Bus.IsChecked == true && !First.IsChecked == true)
            {
                MessageBox.Show("Please select a flight class.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private Passenger CreatePassengerFromInput()
        {
            try
            {
                string name = Full_name.Text.Trim();
                string email = Email.Text.Trim();
                string phone = Phone_Number.Text.Trim();
                int age = int.Parse(Age.Text.Trim());

                char gender = 'u';
                if (Male.IsChecked == true) gender = 'm';
                else if (Female.IsChecked == true) gender = 'f';

                return new Passenger(name, email, gender, age, phone);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
        }

        private Ticket CreateTicketFromSelection(Passenger passenger)
        {
            try
            {
                if (Eco.IsChecked == true)
                    return new EconomyTicket(passenger, selectedFlight);
                else if (Bus.IsChecked == true)
                    return new BusinessTicket(passenger, selectedFlight);
                else if (First.IsChecked == true)
                    return new FirstClassTicket(passenger, selectedFlight);

                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating ticket: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private void NavigateToCheckout(Passenger passenger, Ticket ticket)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                var checkout = new Checkout(airlineManager, passenger, ticket, selectedFlight);
                mainWindow.NavigateToFullScreen(checkout);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.BackToMenu();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}