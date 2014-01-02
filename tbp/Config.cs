
namespace tbp
{
  public class Config
  {
    public string version { get; set; }

    public string client { get; set; }

    public string sPathName { get; set; }

    public string rPathName { get; set; }

    public string vPathName { get; set; }

    public string profileName { get; set; }

    public string classType { get; set; }

    public int kiteMode { get; set; }

    public int mode { get; set; }

    public int radarToggle { get; set; }

    public int roamDuration { get; set; }

    public int inventTotalRows { get; set; }

    public int inventStartRow { get; set; }

    public int inventStartColumn { get; set; }

    public float kiteDistance { get; set; }

    public float roamDist { get; set; }

    public float attackDist { get; set; }

    public float avoidNodeDist { get; set; }

    public bool useMount { get; set; }

    public bool useHealSkill { get; set; }

    public int HealMin { get; set; }

    public bool useRetreatSkill { get; set; }

    public float RetreatDist { get; set; }

    public bool useAutoKite { get; set; }

    public bool useAutoRest { get; set; }

    public bool useRestSkill { get; set; }

    public bool dontFilterPickups { get; set; }

    public bool dontFilterMobs { get; set; }

    public bool dontGather { get; set; }

    public string pickupFilterMode { get; set; }

    public string mobFilterMode { get; set; }

    public int RestMin { get; set; }

    public int RestMax { get; set; }
  }
}
