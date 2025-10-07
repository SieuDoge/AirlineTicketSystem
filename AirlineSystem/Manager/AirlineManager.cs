using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AirlineTicketSystem;
using static AirlineTicketSystem.Flight;

namespace AirlineSystem
{
    public class AirlineManager 
    {
        private List<Flight> flights;
        private List<Passenger> passengers;
        private List<Ticket> tickets;

        public List<Flight> Flights => flights;
        public List<Passenger> Passengers => passengers;
        public List<Ticket> Tickets => tickets;

        /// <summary>
        /// Method Get Total tất cả các mục
        /// </summary>s
        public int GetTotalFlights() => flights.Count;

        public int GetTotalPassengers() => passengers.Count;
        public int GetTotalTickets() => tickets.Count;
        public decimal GetTotalRevenue() => tickets.Sum(t => Convert.ToDecimal(t.TicketPrice));

        /// <summary>
        /// constructor AirlineManager with empty list
        /// </summary>
        public AirlineManager()
        {
            flights = new List<Flight>();
            passengers = new List<Passenger>();
            tickets = new List<Ticket>();
        }

        /// <summary>
        /// Hàm xử lý AddFlight thêm Flight vào danh sách
        /// Mục đích tách ra dễ bảo trì ( maybe )
        /// </summary>
        public bool AddFlight(Flight flight)
        {
            if (flight == null)
                throw new ArgumentNullException(nameof(flight), "Flight cannot be null");

            if (flights.Any(f => f.GetFlightNumber() == flight.GetFlightNumber()))
            {
                Console.WriteLine("Flight already exists");
                return false;
            }

            flights.Add(flight);
            return true;
        }

        /// <summary>
        /// Tương tự
        /// </summary>
        /// <param name="passenger"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool AddPassenger(Passenger passenger)
        {
            if (passenger == null)
                throw new ArgumentNullException(nameof(passenger), "Passenger cannot be null");

            if (passengers.Any(p => p.PhoneNumber == passenger.PhoneNumber))
            {
                Console.WriteLine("Passenger already exists");
                return false;
            }

            passengers.Add(passenger);
            return true;
        }

        public bool AddTicket(Ticket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket), "Ticket cannot be null");

            if (tickets.Any(t => t.TicketId == ticket.TicketId))
            {
                Console.WriteLine("Ticket already exists");
                return false;
            }

            tickets.Add(ticket);
            return true;
        }

        /// <summary>
        /// ghi file CSV 
        /// </summary>
        /// <param name="filename"></param>
        public void ExportAirlineData(string filename)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename, false))
                {
                    // Title
                    sw.WriteLine(
                        "TicketId,Name,Email,Phone,Gender,Age,FlightNumber,Departure,Destination,DepartureDate,SeatsNumber,TicketType,Price,Status");
                    int recordCount = 0;
                    foreach (var ticket in tickets)
                    {
                        var passenger = passengers.FirstOrDefault(p => p.PhoneNumber == ticket.PassengerPhone);
                        var flight = flights.FirstOrDefault(f => f.GetFlightNumber() == ticket.FlightNumber);
                        if (passenger == null || flight == null) continue;
                        // Read
                        sw.WriteLine($"{ticket.TicketId},{passenger.Name},{passenger.Email},{passenger.PhoneNumber}," +
                                     $"{passenger.Gender},{passenger.Age},{flight.GetFlightNumber()},{flight.GetDeparture()}," +
                                     $"{flight.GetDestination()},{flight.GetDepartureTime():yyyy-MM-dd HH:mm}," +
                                     $"{ticket.Seat},{ticket.TicketTypeChar},{ticket.TicketPrice},{(int)flight.Status}");
                        recordCount++; // record Count
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export failed: {ex.Message}");
            }
        }

/// <summary>
/// Đọc file CSV  AirlineData.csv
/// </summary>
/// <param name="filename"></param>
/// <exception cref="FileNotFoundException"></exception>
/// <exception cref="ArgumentException"></exception>
        public void ImportFormExcel(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                    throw new FileNotFoundException("File not found", filename);

                using StreamReader sr = new StreamReader(filename);
                string headerLine = sr.ReadLine();
                if (headerLine == null)
                    throw new ArgumentException("File Data can't be Empty");

                string line;
                int importCount = 0, skipCount = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] cols = line.Split(',');
                    try
                    {
                        if (cols.Length >= 15)
                        {
                            string ticketId = cols[0].Trim();
                            string name = cols[1].Trim();
                            string email = cols[2].Trim();
                            string phone = cols[3].Trim();
                            char gender = Convert.ToChar(cols[4].Trim());
                            int age = Convert.ToInt32(cols[5].Trim());
                            string flightNumber = cols[6].Trim();
                            string departure = cols[7].Trim();
                            string destination = cols[8].Trim();
                            DateTime departureDate = DateTime.Parse(cols[9].Trim());
                            int aseats = Convert.ToInt32(cols[10].Trim());
                            char ticketType = Convert.ToChar(cols[11].Trim());
                            double price = Convert.ToDouble(cols[12].Trim());
                            string seat = cols[13].Trim();
                            int statusInt = int.Parse(cols[14].Trim());

                            FlightStatus status;
                            if (Enum.IsDefined(typeof(FlightStatus), statusInt))
                                status = (FlightStatus)statusInt;
                            else
                                status = FlightStatus.Scheduled;

                            AddPassenger(new Passenger(name, email, gender, age, phone));
                            AddFlight(new Flight(flightNumber, departure, destination, departureDate, aseats, status));

                            var passenger = passengers.FirstOrDefault(p => p.PhoneNumber == phone);
                            var flight = flights.FirstOrDefault(f => f.GetFlightNumber() == flightNumber);
                            if (passenger != null && flight != null)
                            {
                                Ticket t = ticketType switch
                                {
                                    'e' => new EconomyTicket(passenger, flight, ticketId, seat),
                                    'b' => new BusinessTicket(passenger, flight, ticketId, seat),
                                    'f' => new FirstClassTicket(passenger, flight, ticketId, seat),
                                    _ => null
                                };
                                if (t != null) AddTicket(t);
                            }

                            importCount++;
                        }
                        else
                        {
                            Console.WriteLine($"Skip line: {line}");
                            skipCount++;
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error parsing line: {line} - {ex.Message}");
                        skipCount++;
                    }
                }

                Console.WriteLine($"Imported {importCount} records, skipped {skipCount} lines from {filename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import failed: {ex.Message}");
            }
        }

/// <summary>
/// Đọc file FlightData.csv
/// </summary>
/// <param name="filename"></param>
        public void ImportFlightsFromCsv(string filename)
        {
            ImportFromCsv(filename, "flights", (cols) =>
            {
                if (cols.Length >= 6)
                {
                    string flightNumber = cols[0].Trim();
                    string departure = cols[1].Trim();
                    string destination = cols[2].Trim();
                    DateTime departureTime = DateTime.Parse(cols[3].Trim());
                    int availableSeats = Convert.ToInt32(cols[4].Trim());
                    int statusInt = int.Parse(cols[5].Trim());

                    FlightStatus status = Enum.IsDefined(typeof(FlightStatus), statusInt)
                        ? (FlightStatus)statusInt
                        : FlightStatus.Scheduled;

                    return AddFlight(new Flight(flightNumber, departure, destination, departureTime, availableSeats,
                        status));
                }

                return false;
            });
        }
        /// <summary>
        /// Tương tự TicketData.csv
        /// </summary>
        /// <param name="filename"></param>
        public void ImportTicketsFromCsv(string filename)
        {
            ImportFromCsv(filename, "tickets", (cols) =>
            {
                if (cols.Length >= 6)
                {
                    string ticketId = cols[0].Trim();
                    double price = Convert.ToDouble(cols[1].Trim());
                    char ticketType = Convert.ToChar(cols[2].Trim());
                    string passengerPhone = cols[3].Trim();
                    string flightNumber = cols[4].Trim();
                    string seat = cols[5].Trim();

                    var passenger = passengers.FirstOrDefault(p => p.PhoneNumber == passengerPhone);
                    var flight = flights.FirstOrDefault(f => f.GetFlightNumber() == flightNumber);
                    if (passenger == null || flight == null)
                        return false;

                    Ticket t = ticketType switch
                    {
                        'e' => new EconomyTicket(passenger, flight, ticketId, seat),
                        'b' => new BusinessTicket(passenger, flight, ticketId, seat),
                        'f' => new FirstClassTicket(passenger, flight, ticketId, seat),
                        _ => null
                    };

                    if (t != null)
                    {
                        AddTicket(t);
                        return true;
                    }
                }

                return false;
            });
        }

        /// <summary>
        ///  ===== UNIFIED EXPORT METHODS =====
        /// </summary>
        /// <param name="filename"></param>
        public void ExportFlightsToCsv(string filename)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename, false))
                {
                    sw.WriteLine("FlightNumber,Departure,Destination,DepartureTime,AvailableSeats,Status");
                    foreach (var f in flights)
                        sw.WriteLine(
                            $"{f.GetFlightNumber()},{f.GetDeparture()},{f.GetDestination()},{f.GetDepartureTime():yyyy-MM-dd HH:mm},{f.GetAvailableSeats()},{(int)f.Status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export flights failed: {ex.Message}");
            }
        }

        public void ExportTicketsToCsv(string filename)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename, false))
                {
                    sw.WriteLine("TicketId,Price,TicketType,PassengerPhone,FlightNumber,Seat");
                    foreach (var t in tickets)
                    {
                        sw.WriteLine(
                            $"{t.TicketId},{t.TicketPrice},{t.TicketTypeChar},{t.PassengerPhone},{t.FlightNumber},{t.Seat}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export tickets failed: {ex.Message}");
            }
        }

        public void ExportPassengerToCsv(string filename)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename, false))
                {
                    sw.WriteLine("Name,PhoneNumber,Email,Age,Gender");
                    foreach (var p in passengers)
                    {
                        sw.WriteLine(
                            $"{p.Name},{p.PhoneNumber},{p.Email},{p.Age},{p.Gender}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export passenger failed: {ex.Message}");
            }
        }

        public void ImportPassengerFormCsv(string filename)
        {
            ImportFromCsv(filename, "passengers", (cols) =>
            {
                if (cols.Length >= 5)
                {
                    string name = cols[0].Trim();
                    string phone = cols[1].Trim();
                    string email = cols[2].Trim();
                    int age = Convert.ToInt32(cols[3].Trim());
                    char gender = Convert.ToChar(cols[4].Trim());
                    return AddPassenger(new Passenger(name, email, gender, age, phone));
                }

                return false;
            });
        }


        /// <summary>
        /// Flight Status Update
        /// </summary>
        public void UpdateFlightStatuses()
        {
            DateTime now = DateTime.Now;
            bool hasChanges = false;

            foreach (var flight in flights)
            {
                var currentStatus = flight.Status;
                var newStatus = DetermineFlightStatus(flight, now);

                if (currentStatus != newStatus)
                {
                    // Update status using reflection since status is private
                    var statusField = typeof(Flight).GetField("status",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    statusField?.SetValue(flight, newStatus);
                    hasChanges = true;
                }
            }

            // Save changes to CSV if any status was updated
            if (hasChanges)
            {
                ExportFlightsToCsv(@"..\..\..\UserData\FlightData.csv");
                ExportAirlineData(@"..\..\..\UserData\AirlineData.csv");
            }
        }

        private FlightStatus DetermineFlightStatus(Flight flight, DateTime currentTime)
        {
            var departureTime = flight.GetDepartureTime();
            var arrivalTime = flight.ArrivalTime; // 3 hours after departure

            // If flight is cancelled, keep it cancelled
            if (flight.Status == FlightStatus.Cancelled)
                return FlightStatus.Cancelled;

            // If current time is before departure time, it's still scheduled
            if (currentTime < departureTime)
                return FlightStatus.Scheduled;

            // If current time is between departure and arrival (3 hours), it's ongoing
            if (currentTime >= departureTime && currentTime < arrivalTime)
                return FlightStatus.OnGoing;

            // If current time is after arrival + 3 hours, it's completed
            if (currentTime >= arrivalTime)
                return FlightStatus.Completed;

            return flight.Status; // Keep current status if no condition matches
        }


        private void ImportFromCsv(string filename, string dataType, Func<string[], bool> processLine)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine($"{dataType} file {filename} not found, skipping...");
                    return;
                }

                using StreamReader sr = new StreamReader(filename);
                string headerLine = sr.ReadLine();

                string line;
                int importCount = 0, skipCount = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] cols = line.Split(',');
                    try
                    {
                        if (processLine(cols))
                            importCount++;
                        else
                            skipCount++;
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error parsing {dataType} line: {line} - {ex.Message}");
                        skipCount++;
                    }
                }

                Console.WriteLine($"Imported {importCount} {dataType}, skipped {skipCount} lines from {filename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import {dataType} failed: {ex.Message}");
            }
        }
    }
}