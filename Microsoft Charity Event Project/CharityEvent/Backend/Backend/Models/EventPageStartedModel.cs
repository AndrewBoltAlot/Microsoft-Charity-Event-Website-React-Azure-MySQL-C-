using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class EventPageStartedModel
    {
        public string OrganiserName { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public int Description { get; set; }

        public double Prize { get; set; }

        public List<EmailSelectionModel> AvailableSelections { get; set; }
    }
}
