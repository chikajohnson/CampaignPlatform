using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Campaign.API.ViewModels
{
    public class PollParticipationModel
{
        public string PollId { get; set; }
        public string SelectedPollAnswerOption { get; set; }
        public string SelectedPollAnswerOptionText { get; set; }
    }
}