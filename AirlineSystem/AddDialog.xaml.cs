using AirlineTicketSystem;
using System.Windows;
using System.Windows.Controls;

namespace AirlineSystem
{
    public partial class AddDialog : Window
    {
        private string dataType;
        private AirlineManager airlineManager;

        public AddDialog(string type, AirlineManager manager)
        {
            InitializeComponent();
            dataType = type;
            airlineManager = manager;

            BuildForm();
        }

        private void BuildForm()
        {
            ContentPanel.Children.Clear(); // xóa hết control đang có sẵn → đảm bảo mỗi lần build lại form thì form sạch, không bị chồng control cũ.

            if (dataType == "Flight")
            {
                ContentPanel.Children.Add(new TextBlock { Text = "Flight Number:" });
                ContentPanel.Children.Add(new TextBox { Tag = "FlightNumber" });

                ContentPanel.Children.Add(new TextBlock { Text = "Departure:" });
                ContentPanel.Children.Add(new TextBox { Tag = "Departure" });

                ContentPanel.Children.Add(new TextBlock { Text = "Destination:" });
                ContentPanel.Children.Add(new TextBox { Tag = "Destination" });

                ContentPanel.Children.Add(new TextBlock { Text = "Departure Time:" });
                ContentPanel.Children.Add(new DatePicker { SelectedDate = DateTime.Now, Tag = "DepartureTime" });

                ContentPanel.Children.Add(new TextBlock { Text = "Available Seats:" });
                ContentPanel.Children.Add(new TextBox { Tag = "Seats" });
            }
            else if (dataType == "Passenger")
            {
                ContentPanel.Children.Add(new TextBlock { Text = "Full Name:" });
                ContentPanel.Children.Add(new TextBox { Tag = "Name" });

                ContentPanel.Children.Add(new TextBlock { Text = "Email:" });
                ContentPanel.Children.Add(new TextBox { Tag = "Email" });

                ContentPanel.Children.Add(new TextBlock { Text = "Phone Number:" });
                ContentPanel.Children.Add(new TextBox { Tag = "PhoneNumber" });

                ContentPanel.Children.Add(new TextBlock { Text = "Age:" });
                ContentPanel.Children.Add(new TextBox { Tag = "Age" });

                ContentPanel.Children.Add(new TextBlock { Text = "Gender (m/f/u):" });
                ContentPanel.Children.Add(new TextBox { Tag = "Gender" });
            }
            else if (dataType == "Ticket")
            {
                ContentPanel.Children.Add(new TextBlock { Text = "Passenger Phone:" });
                ContentPanel.Children.Add(new TextBox { Tag = "PassengerPhone" });

                ContentPanel.Children.Add(new TextBlock { Text = "Flight Number:" });
                ContentPanel.Children.Add(new TextBox { Tag = "FlightNumber" });

                ContentPanel.Children.Add(new TextBlock { Text = "Class:" });
                ContentPanel.Children.Add(new ComboBox
                {
                    ItemsSource = new[] { "Economy (e)", "Business (b)", "First Class (f)" },
                    SelectedIndex = 0,
                    Tag = "TicketType"
                });
            }
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataType == "Flight")
                {
                    string number = GetValue("FlightNumber");
                    string departure = GetValue("Departure");
                    string destination = GetValue("Destination");
                    DateTime time = (ContentPanel.Children[3] as DatePicker)?.SelectedDate ?? DateTime.Now;
                    int seats = int.Parse(GetValue("Seats"));

                    var flight = new Flight(number, departure, destination, time, seats, Flight.FlightStatus.Scheduled);
                    airlineManager.AddFlight(flight);
                    airlineManager.ExportFlightsToCsv(@"..\..\..\UserData\FlightData.csv");
                }
                else if (dataType == "Passenger")
                {
                    string name = GetValue("Name");
                    string email = GetValue("Email");
                    string phone = GetValue("PhoneNumber");
                    int age = int.Parse(GetValue("Age"));
                    char gender = GetValue("Gender")[0];

                    var passenger = new Passenger(name, email, gender, age, phone);
                    airlineManager.AddPassenger(passenger);
                    airlineManager.ExportPassengerToCsv(@"..\..\..\UserData\Passenger.csv");
                }
                else if (dataType == "Ticket")
                {
                    string phone = GetValue("PassengerPhone");
                    string flightNum = GetValue("FlightNumber");
                    string typeText = (ContentPanel.Children[2] as ComboBox)?.SelectedItem.ToString();
                    char ticketType = typeText.Contains("(b)") ? 'b' :
                                      typeText.Contains("(f)") ? 'f' : 'e';

                    var passenger = airlineManager.Passengers.FirstOrDefault(p => p.PhoneNumber == phone);
                    var flight = airlineManager.Flights.FirstOrDefault(f => f.FlightNumber == flightNum);

                    if (passenger == null || flight == null)
                        throw new Exception("Invalid passenger phone or flight number");

                    Ticket t = ticketType switch
                    {
                        'e' => new EconomyTicket(passenger, flight),
                        'b' => new BusinessTicket(passenger, flight),
                        'f' => new FirstClassTicket(passenger, flight),
                        _ => null
                    };

                    if (t != null)
                    {
                        airlineManager.AddTicket(t);
                        airlineManager.ExportTicketsToCsv(@"..\..\..\UserData\TicketData.csv");
                        airlineManager.ExportAirlineData(@"..\..\..\UserData\AirlineData.csv");
                    }
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding: {ex.Message}]\n" +
                                $"Or you dont have permi");
            }
        }

        private string GetValue(string tag) =>
            (ContentPanel.Children.OfType<FrameworkElement>().FirstOrDefault(c => (string)c.Tag == tag) as TextBox)?.Text ?? "";

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
