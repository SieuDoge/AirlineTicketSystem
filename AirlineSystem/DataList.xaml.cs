using AirlineTicketSystem;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AirlineSystem
{
    public partial class DataList : UserControl
    {
        public enum DataType
        {
            Flights,
            Passengers,
            Tickets
        }

        private DataType currentDataType;
        private AirlineManager airlineManager;

        public DataList(DataType dataType, AirlineManager manager)
        {
            InitializeComponent();
            currentDataType = dataType;
            airlineManager = manager;

            SetupUI();
            LoadData();
        }

        private void SetupUI()
        {
            switch (currentDataType)
            {
                case DataType.Flights:
                    SetupFlightUI();
                    break;
                case DataType.Passengers:
                    SetupPassengerUI();
                    break;
                case DataType.Tickets:
                    SetupTicketUI();
                    break;
            }
        }

        private void SetupFlightUI()
        {
            HeaderIcon.Text = "✈️";
            HeaderTitle.Text = "ALL FLIGHTS";
            HeaderSubtitle.Text = "Manage and view all flight information";
            SearchPlaceholder.Text = "🔍 Search flights...";
            AddNewButton.Content = "➕ Add Flight";
        }
/// <summary>
/// // x:Name="MainDataGrid" 
/// </summary>
        private void SetupPassengerUI()
        {
            HeaderIcon.Text = "👤";
            HeaderTitle.Text = "ALL PASSENGERS";
            HeaderSubtitle.Text = "Manage and view all passenger information";
            SearchPlaceholder.Text = "🔍 Search passengers...";
            AddNewButton.Content = "➕ Add Passenger";
            
            MainDataGrid.Columns.Clear(); // x:Name="MainDataGrid"
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Full Name", Binding = new System.Windows.Data.Binding("Name"), Width = 200 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Email", Binding = new System.Windows.Data.Binding("Email"), Width = 200 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Phone", Binding = new System.Windows.Data.Binding("PhoneNumber"), Width = 150 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Age", Binding = new System.Windows.Data.Binding("Age"), Width = 80 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Gender", Binding = new System.Windows.Data.Binding("Gender"), Width = 100 });
            AddActionColumn();
        }

        private void SetupTicketUI()
        {
            HeaderIcon.Text = "🎫";
            HeaderTitle.Text = "ALL TICKETS";
            HeaderSubtitle.Text = "Manage and view all ticket information";
            SearchPlaceholder.Text = "🔍 Search tickets...";
            AddNewButton.Content = "➕ Issue Ticket";

            MainDataGrid.Columns.Clear();
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Ticket ID", Binding = new System.Windows.Data.Binding("TicketId"), Width = 120 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Passenger", Binding = new System.Windows.Data.Binding("NamePass"), Width = 180 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Phone Number", Binding = new System.Windows.Data.Binding("PassengerPhone"), Width = 130 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Flight", Binding = new System.Windows.Data.Binding("FlightNumber"), Width = 130 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Class", Binding = new System.Windows.Data.Binding("TicketTypeName"), Width = 100 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Seat Number", Binding = new System.Windows.Data.Binding("Seat"), Width = 130 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Price", Binding = new System.Windows.Data.Binding("TicketPrice") { StringFormat = "${0:N2}" }, Width = 100 });
            AddActionColumn();
        }

        private void AddActionColumn()
        {
            var actionColumn = new DataGridTemplateColumn
            {
                Header = "Actions",
                Width = 120
            };

            var cellTemplate = new DataTemplate();
            var stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            // Edit Button
            var editBorder = new FrameworkElementFactory(typeof(Border));
            editBorder.SetValue(Border.CornerRadiusProperty, new CornerRadius(4));
            editBorder.SetValue(Border.BackgroundProperty, System.Windows.Media.Brushes.CornflowerBlue);
            editBorder.SetValue(Border.MarginProperty, new Thickness(2));

            var editButton = new FrameworkElementFactory(typeof(Button));
            if (currentDataType == DataType.Tickets)
            {
                // Đổi chữ & tooltip khi đang ở trang "All Tickets"
                editButton.SetValue(Button.ContentProperty, "👁 Show Ticket");
                editButton.SetValue(Button.ToolTipProperty, "Open Ticket");
            }
            else
            {
                editButton.SetValue(Button.ContentProperty, "✏️ Edit");
                editButton.SetValue(Button.ToolTipProperty, "Edit");
            }
            editButton.SetValue(Button.BackgroundProperty, System.Windows.Media.Brushes.Transparent);
            editButton.SetValue(Button.ForegroundProperty, System.Windows.Media.Brushes.White);
            editButton.SetValue(Button.BorderThicknessProperty, new Thickness(0));
            editButton.SetValue(Button.PaddingProperty, new Thickness(6, 4, 6, 4));
            editButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(EditButton_Click));

            editBorder.AppendChild(editButton);

            // Delete Button
            var deleteBorder = new FrameworkElementFactory(typeof(Border));
            deleteBorder.SetValue(Border.CornerRadiusProperty, new CornerRadius(4));
            deleteBorder.SetValue(Border.BackgroundProperty, System.Windows.Media.Brushes.Crimson);
            deleteBorder.SetValue(Border.MarginProperty, new Thickness(2));

            var deleteButton = new FrameworkElementFactory(typeof(Button));
            deleteButton.SetValue(Button.ContentProperty, "🗑️");
            deleteButton.SetValue(Button.BackgroundProperty, System.Windows.Media.Brushes.Transparent);
            deleteButton.SetValue(Button.ForegroundProperty, System.Windows.Media.Brushes.White);
            deleteButton.SetValue(Button.BorderThicknessProperty, new Thickness(0));
            deleteButton.SetValue(Button.PaddingProperty, new Thickness(6, 4, 6, 4));
            deleteButton.SetValue(Button.ToolTipProperty, "Delete");
            deleteButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(DeleteButton_Click));

            deleteBorder.AppendChild(deleteButton);

            stackPanel.AppendChild(editBorder);
            stackPanel.AppendChild(deleteBorder);

            cellTemplate.VisualTree = stackPanel;
            actionColumn.CellTemplate = cellTemplate;

            MainDataGrid.Columns.Add(actionColumn);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = (button?.Parent as Border)?.DataContext;

            if (item == null) return;

            switch (currentDataType)
            {
                case DataType.Flights:
                    EditFlight(item as Flight);
                    break;
                case DataType.Passengers:
                    EditPassenger(item as Passenger);
                    break;
                case DataType.Tickets:
                    EditTicket(item as Ticket);
                    break;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = (button?.Parent as Border)?.DataContext;

            if (item == null) return;

            var result = MessageBox.Show(
                "Are you sure you want to delete this item?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                switch (currentDataType)
                {
                    case DataType.Flights:
                        DeleteFlight(item as Flight);
                        break;
                    case DataType.Passengers:
                        DeletePassenger(item as Passenger);
                        break;
                    case DataType.Tickets:
                        DeleteTicket(item as Ticket);
                        break;
                }
            }
        }

        private void EditFlight(Flight flight)
        {
            if (flight == null) return;
            var dialog = new EditDialog(flight) { Owner = Window.GetWindow(this) };
            if (dialog.ShowDialog() == true)
            {
                LoadData();
                airlineManager.ExportFlightsToCsv(@"..\..\..\UserData\FlightData.csv");   // lưu lại
            }
        }

        private void EditPassenger(Passenger passenger)
        {
            if (passenger == null) return;
            var dialog = new EditDialog(passenger) { Owner = Window.GetWindow(this) };
            if (dialog.ShowDialog() == true)
            {
                LoadData();
                airlineManager.ExportPassengerToCsv(@"..\..\..\UserData\Passenger.csv"); // lưu lại
            }
        }


        private void EditTicket(Ticket ticket)
        {
    

            string filePath = $@"..\..\..\UserData\QRCode\{ticket.TicketId}.png";

            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });

        }

        private void DeleteFlight(Flight flight)
        {
            if (flight == null) return;

            try
            {
                airlineManager.Flights.Remove(flight);

                // Lưu lại CSV
                airlineManager.ExportFlightsToCsv(@"..\..\..\UserData\FlightData.csv");
                airlineManager.ExportAirlineData(@"..\..\..\UserData\AirlineData.csv");

                LoadData();
                MessageBox.Show($"Flight {flight.FlightNumber} has been deleted successfully.",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting flight: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DeletePassenger(Passenger passenger)
        {
            if (passenger == null) return;

            try
            {
                // Nếu passenger có vé thì hỏi trước
                var hasTickets = airlineManager.Tickets.Any(t => t.PassengerPhone == passenger.PhoneNumber);
                if (hasTickets)
                {
                    var result = MessageBox.Show(
                        "This passenger has existing tickets. Deleting will also remove all associated tickets. Continue?",
                        "Warning",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No) return;

                    // Xóa hết vé liên quan
                    airlineManager.Tickets.RemoveAll(t => t.PassengerPhone == passenger.PhoneNumber);
                    airlineManager.ExportTicketsToCsv(@"..\..\..\UserData\TicketData.csv");
                    airlineManager.ExportAirlineData(@"..\..\..\UserData\AirlineData.csv");
                }

                // Xóa passenger
                airlineManager.Passengers.Remove(passenger);
                airlineManager.ExportPassengerToCsv(@"..\..\..\UserData\Passenger.csv");

                LoadData();
                MessageBox.Show($"Passenger {passenger.Name} has been deleted successfully.",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting passenger: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DeleteTicket(Ticket ticket)
        {
            if (ticket == null) return;

            try
            {
                airlineManager.Tickets.Remove(ticket);

                // Lưu lại CSV
                airlineManager.ExportTicketsToCsv(@"..\..\..\UserData\TicketData.csv");
                airlineManager.ExportAirlineData(@"..\..\..\UserData\AirlineData.csv");

                LoadData();
                MessageBox.Show($"Ticket {ticket.TicketId} has been deleted successfully.",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting ticket: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void LoadData()
        {
            try
            {
                switch (currentDataType)
                {
                    case DataType.Flights:
                        var flights = airlineManager.Flights;
                        MainDataGrid.ItemsSource = null;
                        MainDataGrid.ItemsSource = flights;
                        UpdateRecordCount($"Showing {flights.Count} flights");
                        break;

                    case DataType.Passengers:
                        var passengers = airlineManager.Passengers;
                        MainDataGrid.ItemsSource = null;
                        MainDataGrid.ItemsSource = passengers;
                        UpdateRecordCount($"Showing {passengers.Count} passengers");
                        break;

                    case DataType.Tickets:
                        var tickets = airlineManager.Tickets;
                        MainDataGrid.ItemsSource = null;
                        MainDataGrid.ItemsSource = tickets;
                        UpdateRecordCount($"Showing {tickets.Count} tickets");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateRecordCount(string text)
        {
            RecordCount.Text = text;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            var managerView = new Manager(airlineManager);
            mainWindow?.NavigateToFullScreen(managerView);
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Filter functionality will be implemented here.", "Filter",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            string type = currentDataType switch
            {
                DataType.Flights => "Flight",
                DataType.Passengers => "Passenger",
                DataType.Tickets => "Ticket",
                _ => ""
            };

            var dialog = new AddDialog(type, airlineManager) { Owner = Window.GetWindow(this) };
            if (dialog.ShowDialog() == true)
            {
                LoadData();
                MessageBox.Show($"{type} added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchPlaceholder.Visibility = string.IsNullOrEmpty(SearchBox.Text)
                ? Visibility.Visible
                : Visibility.Hidden;

            FilterDataBySearch(SearchBox.Text);
        }

        private void FilterDataBySearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                LoadData();
                return;
            }

            searchTerm = searchTerm.Trim().ToLower();

            try
            {
                switch (currentDataType)
                {
                    case DataType.Flights:
                        var filteredFlights = airlineManager.Flights.Where(f => (f as ISearchable)?.Matches(searchTerm) == true).ToList();
                        MainDataGrid.ItemsSource = null;
                        MainDataGrid.ItemsSource = filteredFlights;
                        UpdateRecordCount($"Showing {filteredFlights.Count} of {airlineManager.Flights.Count} flights");
                        break;

                    case DataType.Passengers:
                        var filteredPassengers = airlineManager.Passengers.Where(p => (p as ISearchable)?.Matches(searchTerm) == true).ToList();
                        MainDataGrid.ItemsSource = null;
                        MainDataGrid.ItemsSource = filteredPassengers;
                        UpdateRecordCount($"Showing {filteredPassengers.Count} of {airlineManager.Passengers.Count} passengers");
                        break;

                    case DataType.Tickets:
                        var filteredTickets = airlineManager.Tickets.Where(t => (t as ISearchable)?.Matches(searchTerm) == true).ToList();
                        MainDataGrid.ItemsSource = null;
                        MainDataGrid.ItemsSource = filteredTickets;
                        UpdateRecordCount($"Showing {filteredTickets.Count} of {airlineManager.Tickets.Count} tickets");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering data: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                LoadData(); // Fallback to show all data
            }
        }
    }
}