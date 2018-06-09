using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avdfGame
{
    public class Capability : IComparable<Capability>
    {
        public int CapabilityId { get; set; }
        public string CapabilityName { get; set; }
        public string CapabilityDetail { get; set; }
        public int Votes { get; set; }
        public bool OwnerDidVote { get; set; }

        public Capability()
        {
        }

        public int CompareTo(Capability otherCapability)
        {
            //return this.Votes.CompareTo(otherCapability.Votes);
            return otherCapability.Votes.CompareTo(this.Votes);
        }
    }
}
