using AirlineTicketSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class FirstClassTicket : Ticket
    {
    public FirstClassTicket(Passenger passenger, Flight flight, string ticketId = null, string seat = null)
            : base(passenger, flight, ticketId, seat)
        {
            TicketTypeChar = 'f';
        }

        protected override double CalculatePrice()
        {
            return 800 * 1.1;
        }

        public override void Print()
        {
            Console.WriteLine($"First Class Ticket - {TicketId} - {PassengerPhone}");
        }
    }

