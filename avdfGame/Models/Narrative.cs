using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avdfGame
{
    public class Narrative
    {
        public string AuthorName { get; set; }
        public string SessionName { get; set; }
        public Capability Capability { get; set; }
        public DateTime SessionDateTime { get; set; }
        public List<NarrativeElement> NarrativeData { get; set; }

        public Narrative()
        {
            NarrativeData = new List<NarrativeElement>();
            SessionDateTime = System.DateTime.Now;
            Capability = new Capability();
        }
    }
}
