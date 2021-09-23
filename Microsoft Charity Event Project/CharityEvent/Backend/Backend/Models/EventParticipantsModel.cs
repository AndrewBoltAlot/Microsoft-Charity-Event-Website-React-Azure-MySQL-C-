using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class EventParticipantsModel
    {

        public string Event_id { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        public double Prize { get; set; }

        public bool Eliminated{ get; set; }
    }
}
