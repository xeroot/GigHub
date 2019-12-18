using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigHub.ViewModels
{
    public class GigViewModel
    {
        public IEnumerable<Models.Gig> UpcominGigs { get; set; }

        public bool ShowActions { get; set; }

        public string Heading { get; set; }

        public GigViewModel()
        {
            UpcominGigs = new List<Models.Gig>();
        }

        public string SearchTerm { get; set; }
    }
}