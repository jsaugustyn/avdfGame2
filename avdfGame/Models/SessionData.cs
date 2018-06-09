using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avdfGame
{
    public class SessionData
    {
        List<Narrative> SessionPlayerNarratives { get; set; }
        List<Capability> SessionCapabilityData { get; set; }
        // daya structure to hold assessment data

        public SessionData()
        {
            SessionPlayerNarratives = new List<Narrative>();
            SessionCapabilityData = new List<Capability>();
        }

    }
}
