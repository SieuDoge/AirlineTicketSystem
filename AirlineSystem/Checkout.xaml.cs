using AirlineTicketSystem;
using System.Windows;
using System.Windows.Controls;

namespace AirlineSystem
{
    public partial class Checkout : UserControl
    {
        private AirlineManager airlineManager;
        private Passenger passenger;
        private Ticket ticket;
        private Flight flight;

        public Checkout(AirlineManager manager, Passenger passenger, Ticket ticket, Flight flight)
        {
            InitializeComponent();

            // Register Event RadioButton
            CardPayment.Checked += CardPayment_Checked;
            CardPayment.Unchecked += CardPayment_Unchecked;
            

            MoMoPayment.Checked += OtherPayment_Checked;
            CashPayment.Checked += OtherPayment_Checked;


            this.airlineManager = manager;
            this.passenger = passenger;
            this.ticket = ticket;
            this.flight = flight;


            // Set DataContext để XAML binding được
            this.DataContext = new CheckoutViewModel(passenger, flight, ticket);

        }

        public class CheckoutViewModel
        {
            public Passenger Passenger { get; }
            public Flight Flight { get; }

            public Ticket Ticket { get; }

            public double TaxAmount => Math.Round(Ticket.TicketPrice / 1.1 * 0.1, 2);

            public double NonTax => Ticket.TicketPrice / 1.1;



            public CheckoutViewModel(Passenger passenger, Flight flight, Ticket ticket)
            {
                Passenger = passenger;
                Flight = flight;
                Ticket = ticket;
            }
        }


        private void CardPayment_Checked(object sender, RoutedEventArgs e)
        {
            // Hiện CardDetails khi chọn Credit/Debit Card
            if (CardDetails != null)
            {
                CardDetails.Visibility = Visibility.Visible;
            }
        }

        private void CardPayment_Unchecked(object sender, RoutedEventArgs e)
        {
            // CardDetails 
            if (CardDetails != null)
            {
                CardDetails.Visibility = Visibility.Collapsed;
            }
        }

        private void OtherPayment_Checked(object sender, RoutedEventArgs e)
        {
            if (CardDetails != null)
            {
                CardDetails.Visibility = Visibility.Collapsed;
            }
        }


        private void CompletePayment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate payment method selection
                if (!MoMoPayment.IsChecked == true && !CardPayment.IsChecked == true && !CashPayment.IsChecked == true)
                {
                    MessageBox.Show("Please select a payment method.", "Payment Required",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validate card details if card payment is selected
                if (CardPayment.IsChecked == true && !ValidateCardDetails())
                {
                    return;
                }

                // Back to menu
                var mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow?.BackToMenu();

                // Create payment object
                Payment payment = CreatePayment();
                

                if (payment == null) return;

                // Process payment
                bool paymentSuccess = payment.Process();

                if (paymentSuccess)
                {
                    // Book the seat
                    if (flight.BookSeat())
                    {
                        // Add passenger and ticket to manager
                        airlineManager.AddPassenger(passenger);
                        airlineManager.AddTicket(ticket);

                        // Save data
                        // SaveData();

                        // Navigate to success page
                        NavigateToTicketInfo(airlineManager ,passenger, ticket, flight);

                        /*MessageBox.Show("Payment successful! Your ticket has been booked.", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);*/
                    }
                    else
                    {
                        MessageBox.Show("Failed to book seat. Please try again.", "Booking Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Payment failed. Please check your payment details and try again.", "Payment Failed",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing payment: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateCardDetails()
        {
            // Add validation for card number, expiry, CVV, and cardholder name
            // This would check the TextBox values in CardDetails panel

            if (string.IsNullOrWhiteSpace(CardNumber.Text) || CardNumber.Text == "Card Number")
            {
                MessageBox.Show("Please enter a valid card number.", "Invalid Card",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                CardNumber.Focus();
                return false;
            }

            // Add more validation as needed
            return true;
        }

        private Payment CreatePayment()
        {
            try
            {
                double totalAmount = ticket.TicketPrice * 1.1; // Include 10% tax

                if (MoMoPayment.IsChecked == true)
                {
                    return new EwalletPayment(totalAmount, "momo_" + passenger.PhoneNumber);
                }
                else if (CardPayment.IsChecked == true)
                {
                    string cardNumber = CardNumber.Text?.Trim();
                    return new CreditCardPayment(totalAmount, cardNumber);
                }
                else if (CashPayment.IsChecked == true)
                {
                    return new CashPayment(totalAmount);
                }

                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating payment: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /*private void SaveData()
        {
            try
            {
                string flightFile = @"..\..\..\UserData\FlightData.csv";
                string ticketFile = @"..\..\..\UserData\TicketData.csv";
                string allDataFile = @"..\..\..\UserData\AirlineData.csv";



                airlineManager.ExportAirlineData(allDataFile);
                airlineManager.ExportTicketsToCsv(ticketFile);
                airlineManager.ExportFlightsToCsv(flightFile);
            }
            catch (Exception ex)
            {
                // Log error but don't stop the process
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }*/

        private void NavigateToTicketInfo(AirlineManager airlinemanager, Passenger passenger, Ticket ticket, Flight flight)
        {


            var ticketInfo = new TicketInfo(airlinemanager, passenger, ticket, flight);
            ticketInfo.Show();  // hoặc ShowDialog()

        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.BackToMenu();
        }

        private void NavigateToTicketInfo(object sender, RoutedEventArgs e)
        {

        }
    }
}