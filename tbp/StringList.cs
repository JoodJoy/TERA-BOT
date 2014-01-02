

using System.Collections.Generic;

namespace tbp
{
  internal class StringList
  {
    public List<string> nodeStrings = new List<string>();
    public List<string> pickupStrings = new List<string>();

    public StringList()
    {

            // GATHER
      this.nodeStrings.Add("Krymetal Ore");
      this.nodeStrings.Add("Linmetal Ore");
      this.nodeStrings.Add("Normetal Ore");
      this.nodeStrings.Add("Shadmetal Ore");
      this.nodeStrings.Add("Xermetal Ore");
      this.nodeStrings.Add("Verdra Fibers");
      this.nodeStrings.Add("Sylva Fibers");
      this.nodeStrings.Add("Shetla Fibers");
      this.nodeStrings.Add("Toira Fibers");
      this.nodeStrings.Add("Luria Fibers");
      this.nodeStrings.Add("Sun Essence");
      this.nodeStrings.Add("Essence of Wind");
      this.nodeStrings.Add("Star Essence");
      this.nodeStrings.Add("Essence of Ice");
      this.nodeStrings.Add("Lightning Essence");
      this.nodeStrings.Add("Sun Essence"); // does not show for some reason?

            // PICK UP
      this.pickupStrings.Add("Mote");
      this.pickupStrings.Add("Dawnhide");
      this.pickupStrings.Add("Greenhide");
      this.pickupStrings.Add("Rough Hide");
      this.pickupStrings.Add("Bloodhide");
      this.pickupStrings.Add("Gravehide");
      this.pickupStrings.Add("Ore");
      this.pickupStrings.Add("Fibers");
      this.pickupStrings.Add("Essence");
      this.pickupStrings.Add("Paverune");
      this.pickupStrings.Add("Silrune");
      this.pickupStrings.Add("Quoirune");
      this.pickupStrings.Add("Archrune");
      this.pickupStrings.Add("Keyrune");
    }
  }
}
