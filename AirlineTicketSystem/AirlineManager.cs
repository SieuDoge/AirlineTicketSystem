using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AirlineTicketSystem
{
    public class AirlineManager
    {
        private List<Flight> flights;
        private List<Passenger> passengers;
        private List<Ticket> tickets;

        public List<Flight> Flights => flights;
        public List<Passenger> Passengers => passengers;
        public List<Ticket> Tickets => tickets;

        public AirlineManager()
        {
            flights = new List<Flight>();
            passengers = new List<Passenger>();
            tickets = new List<Ticket>();
        }

        public bool addFlight(Flight flight)
        {
            if (flight == null)
                throw new ArgumentNullException("flight can't be null");

            if (flights.Any(f => f.GetFlightNumber() == flight.GetFlightNumber()))
            {
                Console.WriteLine("Flight already exists");
                return false;
            }

            flights.Add(flight);
            return true;
        }

        public bool addPassenger(Passenger passenger)
        {
            if (passenger == null)
                throw new ArgumentNullException("passenger can't be null");

            if (passengers.Any(p => p.PhoneNumber == passenger.PhoneNumber))
            {
                Console.WriteLine("Passenger already exists");
                return false;
            }

            passengers.Add(passenger);
            return true;
        }

        public bool addTicket(Ticket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket can't be null");

            if (tickets.Any(t => t.TicketId == ticket.TicketId))
            {
                Console.WriteLine("Ticket already exists");
                return false;
            }

            tickets.Add(ticket);
            return true;
        }

        public void showAllFlight()
        {
            Console.WriteLine("Flight list:");
            foreach (var f in flights) f.Print();
            Console.WriteLine($"Total: {flights.Count}");
        }

        public void showAllPassenger()
        {
            Console.WriteLine("Passenger list:");
            foreach (var p in passengers) p.Print();
            Console.WriteLine($"Total: {passengers.Count}");
        }

        public void showAllTicket()
        {
            Console.WriteLine("Ticket list:");
            foreach (var t in tickets) t.Print();
            Console.WriteLine($"Total: {tickets.Count}");
        }

        // Export toàn bộ booking (ticket + passenger + flight) sang 1 file
        public void ExportAirlineData(string filename)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename, false))
                {
                    sw.WriteLine(
                        "TicketId,Name,Email,Phone,Gender,Age,FlightNumber,Departure,Destination,DepartureDate,SeatsNumber,TicketType,Price");
                    int recordCount = 0;
                    foreach (var ticket in tickets)
                    {
                        var passenger = passengers.FirstOrDefault(p => p.PhoneNumber == ticket.PassengerPhone);
                        var flight = flights.FirstOrDefault(f => f.GetFlightNumber() == ticket.FlightNumber);
                        if (passenger == null || flight == null) continue;

                        sw.WriteLine($"{ticket.TicketId},{passenger.Name},{passenger.Email},{passenger.PhoneNumber}," +
                                     $"{passenger.Gender},{passenger.Age},{flight.GetFlightNumber()},{flight.GetDeparture()}," +
                                     $"{flight.GetDestination()},{flight.GetDepartureTime():yyyy-MM-dd HH:mm}," +
                                     $"{flight.GetAvailableSeats()},{ticket.TicketTypeChar},{ticket.TicketPrice}");
                        recordCount++;
                    }

                   // Console.WriteLine($"Exported {recordCount} complete records to {filename}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export failed: {ex.Message}");
            }
        }

        // Import từ file AirlineData.csv (format giống ExportAirlineData)
        public void importFormExcel(string filename)
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
                        if (cols.Length >= 13)
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
                            int seats = Convert.ToInt32(cols[10].Trim());
                            char ticketType = Convert.ToChar(cols[11].Trim());
                            double price = Convert.ToDouble(cols[12].Trim());

                            addPassenger(new Passenger(name, email, gender, age, phone));
                            addFlight(new Flight(flightNumber, departure, destination, departureDate, seats));

                            var passenger = passengers.FirstOrDefault(p => p.PhoneNumber == phone);
                            var flight = flights.FirstOrDefault(f => f.GetFlightNumber() == flightNumber);
                            if (passenger != null && flight != null)
                            {
                                Ticket t = ticketType switch
                                {
                                    'e' => new EconomyTicket(passenger, flight, ticketId),
                                    'b' => new BusinessTicket(passenger, flight, ticketId),
                                    'f' => new FirstClassTicket(passenger, flight, ticketId),
                                    _ => null
                                };
                                if (t != null) addTicket(t);
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

        public void ImportFlightsFromCSV(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine($"Flight file {filename} not found, skipping...");
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
                        if (cols.Length >= 5)
                        {
                            string flightNumber = cols[0].Trim();
                            string departure = cols[1].Trim();
                            string destination = cols[2].Trim();
                            DateTime departureTime = DateTime.Parse(cols[3].Trim());
                            int availableSeats = Convert.ToInt32(cols[4].Trim());
                            if (addFlight(new Flight(flightNumber, departure, destination, departureTime,
                                    availableSeats)))
                                importCount++;
                            else
                                skipCount++;
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error parsing flight line: {line} - {ex.Message}");
                        skipCount++;
                    }
                }

                Console.WriteLine($"Imported {importCount} flights, skipped {skipCount} duplicates from {filename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import flights failed: {ex.Message}");
            }
        }

        // Sửa: Ticket CSV bây giờ có FlightNumber để import lại chính xác
        public void ImportTicketsFromCSV(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine($"Ticket file {filename} not found, skipping...");
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
                        if (cols.Length >= 5)
                        {
                            string ticketId = cols[0].Trim();
                            double price = Convert.ToDouble(cols[1].Trim());
                            char ticketType = Convert.ToChar(cols[2].Trim());
                            string passengerPhone = cols[3].Trim();
                            string flightNumber = cols[4].Trim();

                            var passenger = passengers.FirstOrDefault(p => p.PhoneNumber == passengerPhone);
                            var flight = flights.FirstOrDefault(f => f.GetFlightNumber() == flightNumber);
                            if (passenger == null || flight == null)
                            {
                                skipCount++;
                                continue;
                            }

                            Ticket t = ticketType switch
                            {
                                'e' => new EconomyTicket(passenger, flight, ticketId),
                                'b' => new BusinessTicket(passenger, flight, ticketId),
                                'f' => new FirstClassTicket(passenger, flight, ticketId),
                                _ => null
                            };
                            if (t != null)
                            {
                                addTicket(t);
                                importCount++;
                            }
                            else skipCount++;
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error parsing ticket line: {line} - {ex.Message}");
                        skipCount++;
                    }
                }

                Console.WriteLine($"Imported {importCount} tickets, skipped {skipCount} lines from {filename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import tickets failed: {ex.Message}");
            }
        }

        public void ExportFlightsToCSV(string filename)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename, false))
                {
                    sw.WriteLine("FlightNumber,Departure,Destination,DepartureTime,AvailableSeats");
                    foreach (var f in flights)
                        sw.WriteLine(
                            $"{f.GetFlightNumber()},{f.GetDeparture()},{f.GetDestination()},{f.GetDepartureTime():yyyy-MM-dd HH:mm},{f.GetAvailableSeats()}");
                  //  Console.WriteLine($"Exported {flights.Count} flights to {filename}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export flights failed: {ex.Message}");
            }
        }

        // Export tickets kèm flight để import lại chính xác
        public void ExportTicketsToCSV(string filename)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename, false))
                {
                    sw.WriteLine("TicketId,Price,TicketType,PassengerPhone,FlightNumber");
                    foreach (var t in tickets)
                    {
                        sw.WriteLine(
                            $"{t.TicketId},{t.TicketPrice},{t.TicketTypeChar},{t.PassengerPhone},{t.FlightNumber}");
                    }

                  // Console.WriteLine($"Exported {tickets.Count} tickets to {filename}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export tickets failed: {ex.Message}");
            }
        }
    }
}