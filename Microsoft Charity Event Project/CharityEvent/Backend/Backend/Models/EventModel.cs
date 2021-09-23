using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class EventModel
    {
        public int EventId { get; set; }

        public string Organiser { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public double Cost { get; set; }

        public DateTime Registration_begin { get; set; }

        public DateTime Registration_end { get; set; }

        public bool Privacy { get; set; }

        public string IBAN { get; set; }

        public string Image_path { get; set; }

        public string Description { get; set; }

        public int MaxNumberOfParticipants{ get; set; }

        public int NumberOfParticipants{ get; set; }

        public List<String> AvailableSelections { get; set; } 

        public int PayoutSplitPercentageForWinner { get; set; }

        public Boolean CompetitionStarted{ get; set; }

        public Boolean CompetitionCompleted { get; set; }

        public string Invite_id { get; set; }
        

    }
}
