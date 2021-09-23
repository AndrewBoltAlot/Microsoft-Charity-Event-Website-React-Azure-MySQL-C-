using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class TicketModel
    {
        public string Email { get; set; }

        public double Price { get; set; }

        public int EventId { get; set; }

        public string Selection { get; set; }

        public string EventTitle { get; set; }

        public string participantName { get; set; }
    }
}
