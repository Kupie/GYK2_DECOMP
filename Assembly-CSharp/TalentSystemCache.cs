using System;
using System.Collections.Generic;

// Token: 0x020005A9 RID: 1449
public class TalentSystemCache
{
	// Token: 0x17000611 RID: 1553
	// (get) Token: 0x0600256D RID: 9581 RVA: 0x000AF748 File Offset: 0x000AD948
	public static TalentSystemCache Instance
	{
		get
		{
			if (TalentSystemCache.instance != null)
			{
				return TalentSystemCache.instance;
			}
			TalentSystemCache.instance = new TalentSystemCache();
			return TalentSystemCache.instance;
		}
	}

	// Token: 0x0600256E RID: 9582 RVA: 0x000AF768 File Offset: 0x000AD968
	public void CreateCache()
	{
		foreach (TalentDef talentDef in GameBalance.Me.talentDefs)
		{
			this.talents.Add(talentDef.id, talentDef);
		}
		foreach (TalentLevelUpDef talentLevelUpDef in GameBalance.Me.talentLevelUpDefs)
		{
			this.AddLevelUp(talentLevelUpDef);
		}
		foreach (InspirationLevelData inspirationLevelData in GameBalance.Me.inspirationLevels)
		{
			this.AddInspiration(inspirationLevelData);
		}
		foreach (string text in this.talents.Keys)
		{
			List<InspirationLevelData> list;
			if (this.inspirationsByTalentId.TryGetValue(text, out list))
			{
				foreach (InspirationLevelData inspirationLevelData2 in list)
				{
					InspirationData inspirationData = new InspirationData();
					inspirationData.id = inspirationLevelData2.id;
					inspirationData.curLevel = 1;
					inspirationData.curProgressValue = 0;
					inspirationData.completionGoalValue = inspirationLevelData2.levels[0].completionGoalValue;
					this.inspirations.Add(inspirationData.id, inspirationData);
				}
			}
		}
	}

	// Token: 0x0600256F RID: 9583 RVA: 0x000AF940 File Offset: 0x000ADB40
	public void ClearCache()
	{
		this.talents.Clear();
		this.levelUpsByTalentId.Clear();
		this.inspirations.Clear();
		this.inspirationsByTalentId.Clear();
	}

	// Token: 0x06002570 RID: 9584 RVA: 0x000AF970 File Offset: 0x000ADB70
	private void AddLevelUp(TalentLevelUpDef def)
	{
		if (!this.levelUpsByTalentId.ContainsKey(def.talentId))
		{
			this.levelUpsByTalentId[def.talentId] = new List<TalentLevelUpDef>();
		}
		this.levelUpsByTalentId[def.talentId].Add(def);
	}

	// Token: 0x06002571 RID: 9585 RVA: 0x000AF9C0 File Offset: 0x000ADBC0
	private void AddInspiration(InspirationLevelData inspirationLevelData)
	{
		string talentId = inspirationLevelData.talentId;
		if (!this.inspirationsByTalentId.ContainsKey(talentId))
		{
			this.inspirationsByTalentId[talentId] = new List<InspirationLevelData>();
		}
		this.inspirationsByTalentId[talentId].Add(inspirationLevelData);
	}

	// Token: 0x040020CE RID: 8398
	public Dictionary<string, TalentDef> talents = new Dictionary<string, TalentDef>();

	// Token: 0x040020CF RID: 8399
	public Dictionary<string, List<TalentLevelUpDef>> levelUpsByTalentId = new Dictionary<string, List<TalentLevelUpDef>>();

	// Token: 0x040020D0 RID: 8400
	public Dictionary<string, InspirationData> inspirations = new Dictionary<string, InspirationData>();

	// Token: 0x040020D1 RID: 8401
	public Dictionary<string, List<InspirationLevelData>> inspirationsByTalentId = new Dictionary<string, List<InspirationLevelData>>();

	// Token: 0x040020D2 RID: 8402
	private static TalentSystemCache instance;
}
