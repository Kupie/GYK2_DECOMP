using System;
using LazyBearTechnology;

// Token: 0x02000A83 RID: 2691
public class ZombieProgressionWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000B1F RID: 2847
	// (get) Token: 0x06004953 RID: 18771 RVA: 0x0015A983 File Offset: 0x00158B83
	// (set) Token: 0x06004954 RID: 18772 RVA: 0x0015A98B File Offset: 0x00158B8B
	public ZombieTalentData ZombieTalentData { get; private set; }

	// Token: 0x17000B20 RID: 2848
	// (get) Token: 0x06004955 RID: 18773 RVA: 0x0015A994 File Offset: 0x00158B94
	// (set) Token: 0x06004956 RID: 18774 RVA: 0x0015A99C File Offset: 0x00158B9C
	public ZombieWgoData ZombieWgoData { get; private set; }

	// Token: 0x06004957 RID: 18775 RVA: 0x0015A9A8 File Offset: 0x00158BA8
	public ZombieProgressionWidgetData(ZombieWgoData zombieWgoData, string talentId = null)
	{
		this.ZombieWgoData = zombieWgoData;
		string text = ((!string.IsNullOrEmpty(talentId)) ? talentId : ((!string.IsNullOrEmpty(ZombieProgressionWidgetData.lastShownTalentId)) ? ZombieProgressionWidgetData.lastShownTalentId : "talent_orange"));
		this.SwitchTalent(text);
	}

	// Token: 0x06004958 RID: 18776 RVA: 0x0015A9ED File Offset: 0x00158BED
	public void SwitchTalent(string talentId)
	{
		this.ZombieTalentData = this.ZombieWgoData.GetTalentBranch(talentId);
		ZombieProgressionWidgetData.lastShownTalentId = this.ZombieTalentData.id;
	}

	// Token: 0x04003927 RID: 14631
	private const string DEFAULT_TALENT_ID_TO_DRAW = "talent_orange";

	// Token: 0x04003928 RID: 14632
	private static string lastShownTalentId;
}
