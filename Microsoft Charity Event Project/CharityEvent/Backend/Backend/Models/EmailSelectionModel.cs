using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class EmailSelectionModel
    {
        public string Selection { get; set; }

        public string Email { get; set; }

        public bool Eliminated { get; set; }

        public int Ticket_id { get; set; }

        public string EventOrganiser { get; set; }

        public string EventTitle { get; set; }

    }
}
