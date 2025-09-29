using AirlineTicketSystem;
using System.Windows;
using System.Windows.Controls;

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
            SetupEventHandlers();
        }

        private void SetupEventHandlers()
        {
            // Event handlers are now set up in XAML, no need for additional setup
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

            // Columns are already set up in XAML for flights
        }

        private void SetupPassengerUI()
        {
            // Clear existing columns and add passenger columns
            MainDataGrid.Columns.Clear();
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Full Name", Binding = new System.Windows.Data.Binding("Name"), Width = 200 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Email", Binding = new System.Windows.Data.Binding("Email"), Width = 200 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Phone", Binding = new System.Windows.Data.Binding("PhoneNumber"), Width = 150 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Age", Binding = new System.Windows.Data.Binding("Age"), Width = 80 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Gender", Binding = new System.Windows.Data.Binding("Gender"), Width = 100 });
            AddActionColumn();
        }

        private void SetupTicketUI()
        {
            // Clear existing columns and add ticket columns
            MainDataGrid.Columns.Clear();
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Ticket ID", Binding = new System.Windows.Data.Binding("TicketId"), Width = 120 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Passenger", Binding = new System.Windows.Data.Binding("NamePass"), Width = 180 });
             MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Phone Number", Binding = new System.Windows.Data.Binding("PassengerPhone"), Width = 130 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Flight", Binding = new System.Windows.Data.Binding("FlightNumber"), Width = 130 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Class", Binding = new System.Windows.Data.Binding("TicketTypeName"), Width = 100 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Seat", Binding = new System.Windows.Data.Binding("SeatNumber"), Width = 80 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Price", Binding = new System.Windows.Data.Binding("TicketPrice") { StringFormat = "${0:N2}" }, Width = 100 });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Status", Binding = new System.Windows.Data.Binding("Status"), Width = 100 });
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
            editButton.SetValue(Button.ContentProperty, "✏️");
            editButton.SetValue(Button.BackgroundProperty, System.Windows.Media.Brushes.Transparent);
            editButton.SetValue(Button.ForegroundProperty, System.Windows.Media.Brushes.White);
            editButton.SetValue(Button.BorderThicknessProperty, new Thickness(0));
            editButton.SetValue(Button.PaddingProperty, new Thickness(6, 4, 6, 4));
            editButton.SetValue(Button.ToolTipProperty, "Edit");

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

            deleteBorder.AppendChild(deleteButton);

            stackPanel.AppendChild(editBorder);
            stackPanel.AppendChild(deleteBorder);

            cellTemplate.VisualTree = stackPanel;
            actionColumn.CellTemplate = cellTemplate;

            MainDataGrid.Columns.Add(actionColumn);
        }

        private void LoadData()
        {
            try
            {
                switch (currentDataType)
                {
                    case DataType.Flights:
                        var flights = airlineManager.Flights;
                        MainDataGrid.ItemsSource = flights;
                        UpdateRecordCount($"Showing {flights.Count} flights");
                        break;

                    case DataType.Passengers:
                        var passengers = airlineManager.Passengers;
                        MainDataGrid.ItemsSource = passengers;
                        UpdateRecordCount($"Showing {passengers.Count} passengers");
                        break;

                    case DataType.Tickets:
                        var tickets = airlineManager.Tickets;
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

        // Event handlers for buttons
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
            string message = currentDataType switch
            {
                DataType.Flights => "Add new flight functionality will be implemented here.",
                DataType.Passengers => "Add new passenger functionality will be implemented here.",
                DataType.Tickets => "Issue new ticket functionality will be implemented here.",
                _ => "Add functionality will be implemented here."
            };

            MessageBox.Show(message, "Add New",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Implement search functionality
            SearchPlaceholder.Visibility = string.IsNullOrEmpty(SearchBox.Text)
                ? Visibility.Visible
                : Visibility.Hidden;

            // Add actual search logic here
            // FilterDataBySearch(SearchBox.Text);
        }

        private void FilterDataBySearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                LoadData(); // Reload all data
                return;
            }

            // Implement search filtering based on current data type
            // This is a placeholder - implement according to your data structure
        }
    }
}