using AirlineTicketSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class EconomyTicket : Ticket, IBookable
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

        private bool isBooked;
        public bool Book()
        {
            if (isBooked) return false;
            if (flight?.BookSeat() == true)
            {
                isBooked = true;
                return true;
            }
            return false;
        }

        public bool Cancel()
        {
            if (!isBooked) return false;
            isBooked = false;
            return true;
        }

        public bool IsBooked => isBooked;
    }

