using AirlineTicketSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class EconomyTicket : Ticket
    {
        public EconomyTicket(Passenger passenger, Flight flight, string ticketId = null, string seat = null)
            : base(passenger, flight, ticketId, seat)
        {
            TicketTypeChar = 'e';
        }

        protected override double CalculatePrice()
        {
            return 200 * 1.1;
        }

        public override void Print()
        {
            Console.WriteLine($"Economy Ticket - {TicketId} - {PassengerPhone}");
        }
    }

