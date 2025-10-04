using AirlineTicketSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AirlineSystem
{
    public partial class Search : UserControl
    {
        private AirlineManager airlineManager;
        private List<SearchResultItem> currentResults;

        public Search(AirlineManager manager)
        {
            InitializeComponent();
            airlineManager = manager;
            currentResults = new List<SearchResultItem>();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchPlaceholder.Visibility = string.IsNullOrEmpty(SearchBox.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            string searchTerm = SearchBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("Please enter a phone number or ticket ID to search.",
                    "Empty Search", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                currentResults.Clear();

                // Search by phone number (10 digits)
                if (searchTerm.Length == 10 && searchTerm.All(char.IsDigit))
                {
                    var ticketsByPhone = airlineManager.Tickets
                        .Where(t => t.PassengerPhone == searchTerm)
                        .ToList();

                    foreach (var ticket in ticketsByPhone)
                    {
                        var passenger = airlineManager.Passengers
                            .FirstOrDefault(p => p.PhoneNumber == ticket.PassengerPhone);
                        var flight = airlineManager.Flights
                            .FirstOrDefault(f => f.FlightNumber == ticket.FlightNumber);

                        if (passenger != null && flight != null)
                        {
                            currentResults.Add(new SearchResultItem
                            {
                                TicketId = ticket.TicketId,
                                TicketPrice = ticket.TicketPrice,
                                TicketTypeName = ticket.TicketTypeName,
                                Seat = ticket.Seat ?? "N/A",
                                FlightNumber = flight.FlightNumber,
                                FlightRoute = $"{flight.Departure} → {flight.Destination}",
                                DepartureDate = flight.DepartureTime.ToString("MMM dd, yyyy HH:mm"),
                                PassengerName = passenger.Name,
                                PassengerPhone = passenger.PhoneNumber,
                                PassengerEmail = passenger.Email,
                                TicketObject = ticket,
                                PassengerObject = passenger,
                                FlightObject = flight
                            });
                        }
                    }
                }
                // Search by Ticket ID
                else if (searchTerm.StartsWith("TK", StringComparison.OrdinalIgnoreCase))
                {
                    var ticket = airlineManager.Tickets
                        .FirstOrDefault(t => t.TicketId.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));

                    if (ticket != null)
                    {
                        var passenger = airlineManager.Passengers
                            .FirstOrDefault(p => p.PhoneNumber == ticket.PassengerPhone);
                        var flight = airlineManager.Flights
                            .FirstOrDefault(f => f.FlightNumber == ticket.FlightNumber);

                        if (passenger != null && flight != null)
                        {
                            currentResults.Add(new SearchResultItem
                            {
                                TicketId = ticket.TicketId,
                                TicketPrice = ticket.TicketPrice,
                                TicketTypeName = ticket.TicketTypeName,
                                Seat = ticket.Seat ?? "N/A",
                                FlightNumber = flight.FlightNumber,
                                FlightRoute = $"{flight.Departure} → {flight.Destination}",
                                DepartureDate = flight.DepartureTime.ToString("MMM dd, yyyy HH:mm"),
                                PassengerName = passenger.Name,
                                PassengerPhone = passenger.PhoneNumber,
                                PassengerEmail = passenger.Email,
                                TicketObject = ticket,
                                PassengerObject = passenger,
                                FlightObject = flight
                            });
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid search format. Please enter:\n• 10-digit phone number\n• Ticket ID (TK******)",
                        "Invalid Format", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DisplayResults();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during search: {ex.Message}",
                    "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayResults()
        {
            if (currentResults.Count > 0)
            {
                ResultsList.ItemsSource = currentResults;
                ResultCount.Text = $"{currentResults.Count} ticket(s) found";

                ResultsHeader.Visibility = Visibility.Visible;
                ResultsScrollViewer.Visibility = Visibility.Visible;
                NoResultsMessage.Visibility = Visibility.Collapsed;
            }
            else
            {
                ResultsHeader.Visibility = Visibility.Collapsed;
                ResultsScrollViewer.Visibility = Visibility.Collapsed;
                NoResultsMessage.Visibility = Visibility.Visible;

                MessageBox.Show("No tickets found with this search term.",
                    "No Results", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
            currentResults.Clear();
            ResultsList.ItemsSource = null;

            ResultsHeader.Visibility = Visibility.Collapsed;
            ResultsScrollViewer.Visibility = Visibility.Collapsed;
            NoResultsMessage.Visibility = Visibility.Visible;
        }

        private void SupportButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button?.Tag as SearchResultItem;

            if (item == null) return;

            var supportMessage = $"Support Request for Ticket: {item.TicketId}\n\n" +
                                $"Passenger: {item.PassengerName}\n" +
                                $"Phone: {item.PassengerPhone}\n" +
                                $"Email: {item.PassengerEmail}\n" +
                                $"Flight: {item.FlightNumber}\n\n" +
                                $"How can we assist you?\n\n" +
                                $"Our support team is available 24/7:\n" +
                                $"📞 Hotline 1: 0816-657-483 - Nguyễn Hữu Huynh\n" +
                                $"✉️ Email: Group7@DoAn.Hcmute.edu.vn\n" +
                                $"💬 Live Chat: Available on website";

            MessageBox.Show(supportMessage, "Customer Support",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CancelBookingButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button?.Tag as SearchResultItem;

            if (item == null) return;

            // Check if flight has already departed
            if (item.FlightObject.DepartureTime < DateTime.Now)
            {
                MessageBox.Show("Cannot cancel booking. The flight has already departed.",
                    "Cancellation Not Allowed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Confirm cancellation
            var result = MessageBox.Show(
                $"Are you sure you want to cancel this booking?\n\n" +
                $"Ticket ID: {item.TicketId}\n" +
                $"Passenger: {item.PassengerName}\n" +
                $"Flight: {item.FlightNumber} ({item.FlightRoute})\n" +
                $"Amount: ${item.TicketPrice:N2}\n\n" +
                $"⚠️ Cancellation Policy:\n" +
                $"• Refund: 80% of ticket price\n" +
                $"• Processing time: 7-10 business days\n\n" +
                $"This action cannot be undone.",
                "Confirm Cancellation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Remove ticket
                    airlineManager.Tickets.Remove(item.TicketObject);

                    // Update available seats
                    item.FlightObject.GetType()
                        .GetField("availableSeats", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.SetValue(item.FlightObject, item.FlightObject.GetAvailableSeats() + 1);

                    // Save changes
                    SaveAllData();

                    // Remove from current results
                    currentResults.Remove(item);
                    ResultsList.ItemsSource = null;
                    ResultsList.ItemsSource = currentResults;

                    if (currentResults.Count == 0)
                    {
                        ClearButton_Click(null, null);
                    }
                    else
                    {
                        ResultCount.Text = $"{currentResults.Count} ticket(s) found";
                    }

                    MessageBox.Show(
                        $"Booking cancelled successfully!\n\n" +
                        $"Refund amount: ${item.TicketPrice * 0.8:N2}\n" +
                        $"Confirmation has been sent to: {item.PassengerEmail}",
                        "Cancellation Successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error cancelling booking: {ex.Message}",
                        "Cancellation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveAllData()
        {
            try
            {
                string flightFile = @"..\..\..\UserData\FlightData.csv";
                string ticketFile = @"..\..\..\UserData\TicketData.csv";
                string passengerFile = @"..\..\..\UserData\Passenger.csv";
                string allDataFile = @"..\..\..\UserData\AirlineData.csv";

                airlineManager.ExportFlightsToCsv(flightFile);
                airlineManager.ExportTicketsToCsv(ticketFile);
                airlineManager.ExportPassengerToCsv(passengerFile);
                airlineManager.ExportAirlineData(allDataFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        // Helper class for search results
        public class SearchResultItem
        {
            public string TicketId { get; set; }
            public double TicketPrice { get; set; }
            public string TicketTypeName { get; set; }
            public string Seat { get; set; }
            public string FlightNumber { get; set; }
            public string FlightRoute { get; set; }
            public string DepartureDate { get; set; }
            public string PassengerName { get; set; }
            public string PassengerPhone { get; set; }
            public string PassengerEmail { get; set; }

            // Store original objects for operations
            public Ticket TicketObject { get; set; }
            public Passenger PassengerObject { get; set; }
            public Flight FlightObject { get; set; }
        }
    }
}