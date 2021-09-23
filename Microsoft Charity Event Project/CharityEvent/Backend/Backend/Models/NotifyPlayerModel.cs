using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class NotifyPlayerModel
    {

        public int EventId { get; set; }

        public int TicketId { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string name { get; set; }

        public double Prize { get; set; }


    }
}
