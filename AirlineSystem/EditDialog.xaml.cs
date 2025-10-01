using System;
using System.Windows;
using System.Windows.Controls;
using AirlineTicketSystem;

namespace AirlineSystem
{
    public partial class EditDialog : Window
    {
        private object editingItem;

        public EditDialog(Flight flight)
        {
            InitializeComponent();
            editingItem = flight;

            ContentPanel.Children.Add(new TextBox { Text = flight.FlightNumber, IsReadOnly = true });
            ContentPanel.Children.Add(new TextBox { Text = flight.Departure, Tag = "Departure" });
            ContentPanel.Children.Add(new TextBox { Text = flight.Destination, Tag = "Destination" });
            ContentPanel.Children.Add(new DatePicker { SelectedDate = flight.DepartureTime, Tag = "DepartureTime" });
            ContentPanel.Children.Add(new TextBox { Text = flight.GetAvailableSeats().ToString(), Tag = "Seats" });
        }

        public EditDialog(Passenger passenger)
        {
            InitializeComponent();
            editingItem = passenger;

            ContentPanel.Children.Add(new TextBox { Text = passenger.Name, Tag = "Name" });
            ContentPanel.Children.Add(new TextBox { Text = passenger.Email, Tag = "Email" });
            ContentPanel.Children.Add(new TextBox { Text = passenger.PhoneNumber, Tag = "PhoneNumber", IsReadOnly = true });
            ContentPanel.Children.Add(new TextBox { Text = passenger.Age.ToString(), Tag = "Age" });
            ContentPanel.Children.Add(new TextBox { Text = passenger.Gender.ToString(), Tag = "Gender" });
        }

        public EditDialog(Ticket ticket)
        {
            InitializeComponent();
            editingItem = ticket;

            ContentPanel.Children.Add(new TextBox { Text = ticket.TicketId, IsReadOnly = true });
            ContentPanel.Children.Add(new TextBox { Text = ticket.NamePass, IsReadOnly = true });
            ContentPanel.Children.Add(new TextBox { Text = ticket.PassengerPhone, IsReadOnly = true });
            ContentPanel.Children.Add(new TextBox { Text = ticket.FlightNumber, IsReadOnly = true });
            ContentPanel.Children.Add(new TextBox { Text = ticket.TicketTypeName, IsReadOnly = true });
            ContentPanel.Children.Add(new TextBox { Text = ticket.TicketPrice.ToString("N2"), Tag = "Price" });
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (editingItem is Flight flight)
                {
                    foreach (var child in ContentPanel.Children)
                    {
                        if (child is TextBox tb && tb.Tag?.ToString() == "Departure") flight.GetType().GetField("departure", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(flight, tb.Text);
                        if (child is TextBox tb2 && tb2.Tag?.ToString() == "Destination") flight.GetType().GetField("destination", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(flight, tb2.Text);
                        if (child is TextBox tb3 && tb3.Tag?.ToString() == "Seats") flight.GetType().GetField("availableSeats", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(flight, int.Parse(tb3.Text));
                        if (child is DatePicker dp && dp.Tag?.ToString() == "DepartureTime") flight.GetType().GetField("departureTime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(flight, dp.SelectedDate ?? flight.DepartureTime);
                    }
                }
                else if (editingItem is Passenger passenger)
                {
                    foreach (var child in ContentPanel.Children)
                    {
                        if (child is TextBox tb)
                        {
                            switch (tb.Tag?.ToString())
                            {
                                case "Name": passenger.Name = tb.Text; break;
                                case "Email": passenger.Email = tb.Text; break;
                                case "Age": passenger.Age = int.Parse(tb.Text); break;
                                case "Gender": passenger.Gender = tb.Text[0]; break;
                            }
                        }
                    }
                }
                else if (editingItem is Ticket ticket)
                {
                    foreach (var child in ContentPanel.Children)
                    {
                        if (child is TextBox tb && tb.Tag?.ToString() == "Price")
                        {
                            ticket.GetType().GetField("ticketPrice", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(ticket, double.Parse(tb.Text));
                        }
                    }
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
