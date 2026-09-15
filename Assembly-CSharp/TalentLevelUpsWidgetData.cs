using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000928 RID: 2344
public class TalentLevelUpsWidgetData : LazyWidgetDataBase
{
	// Token: 0x1700094C RID: 2380
	// (get) Token: 0x06003DC3 RID: 15811 RVA: 0x001274CC File Offset: 0x001256CC
	// (set) Token: 0x06003DC4 RID: 15812 RVA: 0x001274D4 File Offset: 0x001256D4
	public TalentData TalentData { get; private set; }

	// Token: 0x1700094D RID: 2381
	// (get) Token: 0x06003DC5 RID: 15813 RVA: 0x001274DD File Offset: 0x001256DD
	// (set) Token: 0x06003DC6 RID: 15814 RVA: 0x001274E5 File Offset: 0x001256E5
	public List<TalentLevelUpDef> LevelUps { get; private set; }

	// Token: 0x1700094E RID: 2382
	// (get) Token: 0x06003DC7 RID: 15815 RVA: 0x001274EE File Offset: 0x001256EE
	// (set) Token: 0x06003DC8 RID: 15816 RVA: 0x001274F6 File Offset: 0x001256F6
	public ZombieWgoData ZombieWgoData { get; private set; }

	// Token: 0x06003DC9 RID: 15817 RVA: 0x00127500 File Offset: 0x00125700
	public TalentLevelUpsWidgetData(TalentData talentData)
	{
		this.TalentData = talentData;
		this.LevelUps = new List<TalentLevelUpDef>();
		for (int i = 0; i < GameBalance.Me.talentLevelUpDefs.Count; i++)
		{
			TalentLevelUpDef talentLevelUpDef = GameBalance.Me.talentLevelUpDefs[i];
			if (!(talentData.id != talentLevelUpDef.talentId) && !MainGame.Instance.GameSave.knowledgeSystem.hiddenTalentLevelUps.Contains(talentLevelUpDef.id) && !talentLevelUpDef.isZombiePerk)
			{
				this.LevelUps.Add(talentLevelUpDef);
			}
		}
	}

	// Token: 0x06003DCA RID: 15818 RVA: 0x00127598 File Offset: 0x00125798
	public TalentLevelUpsWidgetData(ZombieWgoData zombieWgoData, ZombieTalentData zombieTalentData)
	{
		this.ZombieWgoData = zombieWgoData;
		this.LevelUps = new List<TalentLevelUpDef>();
		for (int i = 0; i < GameBalance.Me.talentLevelUpDefs.Count; i++)
		{
			TalentLevelUpDef talentLevelUpDef = GameBalance.Me.talentLevelUpDefs[i];
			if (!(zombieTalentData.id != talentLevelUpDef.talentId) && !MainGame.Instance.GameSave.knowledgeSystem.hiddenTalentLevelUps.Contains(talentLevelUpDef.id) && talentLevelUpDef.isZombiePerk)
			{
				this.LevelUps.Add(talentLevelUpDef);
			}
		}
	}

	// Token: 0x04003094 RID: 12436
	public TalentSystemData.DelTalentLevelPurchased onTalentLevelPurchased;
}
