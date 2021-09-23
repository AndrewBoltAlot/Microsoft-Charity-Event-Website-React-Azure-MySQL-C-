using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class EventSignUpPageModel
    {

        public string OrganiserName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public double Cost { get; set; }

        public string Image_path { get; set; }

        public int NumberOfParticipants { get; set; }

        public int MaxNumberOfParticipants { get; set; }

        public DateTime Registration_end { get; set; }

        public List<String> AvailableSelections { get; set; }

        public int PayoutSplitPercentageForWinner { get; set; }

        public bool CompetitionCompleted { get; set; }

        public bool CompetitionStarted { get; set; }

    }
}