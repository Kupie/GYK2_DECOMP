using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020005A7 RID: 1447
[Serializable]
public class TalentData : ObjectLinkedToDefinition<TalentDef>
{
	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x06002560 RID: 9568 RVA: 0x000AF208 File Offset: 0x000AD408
	public bool CanUpgradeLevel
	{
		get
		{
			return this.curExp >= this.talentExpLevelBalanceData.GetExpForLevel(this.curTalentLevel);
		}
	}

	// Token: 0x1700060F RID: 1551
	// (get) Token: 0x06002561 RID: 9569 RVA: 0x000AF228 File Offset: 0x000AD428
	public bool CanPurchaseAnyInspiration
	{
		get
		{
			for (int i = 0; i < this.activeInspirations.Count; i++)
			{
				if (this.activeInspirations[i].IsAvailableToBuy)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x17000610 RID: 1552
	// (get) Token: 0x06002562 RID: 9570 RVA: 0x000AF261 File Offset: 0x000AD461
	public bool HasAvailableInspirationActionIndicator
	{
		get
		{
			return !this.isInspirationActionIndicatorBlocked && this.HasCompletedInspirationToBuy();
		}
	}

	// Token: 0x06002563 RID: 9571 RVA: 0x000AF273 File Offset: 0x000AD473
	public TalentData()
	{
	}

	// Token: 0x06002564 RID: 9572 RVA: 0x000AF294 File Offset: 0x000AD494
	public TalentData(string talentId)
		: base(talentId)
	{
		this.curExp = 0;
		this.curTalentLevel = 1;
		this.curTalentValue = 0;
		foreach (TalentLevelUpDef talentLevelUpDef in GameBalance.Me.talentLevelUpDefs)
		{
			if (!talentLevelUpDef.isZombiePerk && talentLevelUpDef.talentId == talentId && talentLevelUpDef.availableAtStart)
			{
				this.studiedLevelUps.Add(talentLevelUpDef.id);
				if (!string.IsNullOrEmpty(talentLevelUpDef.linkedPerk))
				{
					MainGame.Instance.GameSave.perkSystemData.AddPerk(talentLevelUpDef.linkedPerk);
				}
				if (talentLevelUpDef.talentValueAdd > 0)
				{
					this.curTalentValue += talentLevelUpDef.talentValueAdd;
				}
			}
		}
	}

	// Token: 0x06002565 RID: 9573 RVA: 0x000AF38C File Offset: 0x000AD58C
	public TalentLevelUpDef.State GetLevelUpState(TalentLevelUpDef def)
	{
		if (this.studiedLevelUps.Contains(def.id))
		{
			return TalentLevelUpDef.State.Unlocked;
		}
		if (MainGame.Instance.GameSave.knowledgeSystem.hiddenTalentLevelUps.Contains(def.id))
		{
			return TalentLevelUpDef.State.Hidden;
		}
		if (MainGame.Instance.GameSave.knowledgeSystem.unknownTalentLevelUps.Contains(def.id))
		{
			return TalentLevelUpDef.State.Unknown;
		}
		if (def.ParentsUnlocked && this.talentExpPoints >= def.talentExpPointsPrice)
		{
			return TalentLevelUpDef.State.Available;
		}
		return TalentLevelUpDef.State.Visible;
	}

	// Token: 0x06002566 RID: 9574 RVA: 0x000AF410 File Offset: 0x000AD610
	public bool HasAvailableTalentLevelUpToPurchase()
	{
		for (int i = 0; i < GameBalance.Me.talentLevelUpDefs.Count; i++)
		{
			TalentLevelUpDef talentLevelUpDef = GameBalance.Me.talentLevelUpDefs[i];
			if (!talentLevelUpDef.isZombiePerk && !(talentLevelUpDef.talentId != this.id) && this.GetLevelUpState(talentLevelUpDef) == TalentLevelUpDef.State.Available)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002567 RID: 9575 RVA: 0x000AF470 File Offset: 0x000AD670
	public bool HasCompletedInspirationToBuy()
	{
		if (this.activeInspirations == null)
		{
			return false;
		}
		for (int i = 0; i < this.activeInspirations.Count; i++)
		{
			InspirationData inspirationData = this.activeInspirations[i];
			if (!inspirationData.IsHidden && !inspirationData.isAllLevelsBought && inspirationData.IsCompleted)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002568 RID: 9576 RVA: 0x000AF4C5 File Offset: 0x000AD6C5
	public void BlockInspirationActionIndicator()
	{
		if (this.HasCompletedInspirationToBuy())
		{
			this.isInspirationActionIndicatorBlocked = true;
		}
	}

	// Token: 0x06002569 RID: 9577 RVA: 0x000AF4D6 File Offset: 0x000AD6D6
	public void UnblockInspirationActionIndicator()
	{
		this.isInspirationActionIndicatorBlocked = false;
	}

	// Token: 0x0600256A RID: 9578 RVA: 0x000AF4E0 File Offset: 0x000AD6E0
	public void PrepareForGame()
	{
		this.activeInspirations = new List<InspirationData>();
		this.talentExpLevelBalanceData = GameBalance.Me.talentExpLevelsCache[this.id];
		TalentSystemCache instance = TalentSystemCache.Instance;
		List<InspirationLevelData> list;
		if (instance.inspirationsByTalentId.TryGetValue(this.id, out list))
		{
			foreach (InspirationLevelData inspirationLevelData in list)
			{
				this.activeInspirations.Add(instance.inspirations[inspirationLevelData.id]);
			}
		}
		if (this.inspirationsProgression.Count != 0)
		{
			using (List<InspirationData>.Enumerator enumerator2 = this.activeInspirations.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					InspirationData activeInspiration = enumerator2.Current;
					int num = this.inspirationsProgression.FindIndex((InspirationProgressData x) => x.id == activeInspiration.id);
					if (num != -1)
					{
						int i = this.inspirationsProgression[num].completionGoalValue;
						int num2 = 1;
						InspirationDef inspirationDef = InspirationDef.GetDataForLevel(this.inspirationsProgression[num].id, num2);
						for (i -= inspirationDef.completionGoalValue; i >= 0; i -= inspirationDef.completionGoalValue)
						{
							num2++;
							inspirationDef = InspirationDef.GetDataForLevel(this.inspirationsProgression[num].id, num2);
							if (inspirationDef == null)
							{
								num2--;
								inspirationDef = InspirationDef.GetDataForLevel(this.inspirationsProgression[num].id, num2);
								break;
							}
						}
						activeInspiration.curLevel = num2;
						activeInspiration.curProgressValue = this.inspirationsProgression[num].currentValue;
						activeInspiration.completionGoalValue = inspirationDef.completionGoalValue;
						if (num2 == activeInspiration.MaxLevel && i >= 0)
						{
							activeInspiration.isAllLevelsBought = true;
						}
						activeInspiration.FillBoughtInspirations();
						activeInspiration.FillCompletedInspirations();
					}
				}
			}
		}
	}

	// Token: 0x040020C4 RID: 8388
	public int curExp;

	// Token: 0x040020C5 RID: 8389
	public int curTalentLevel;

	// Token: 0x040020C6 RID: 8390
	public int talentExpPoints;

	// Token: 0x040020C7 RID: 8391
	public int curTalentValue;

	// Token: 0x040020C8 RID: 8392
	public List<string> studiedLevelUps = new List<string>();

	// Token: 0x040020C9 RID: 8393
	public List<InspirationProgressData> inspirationsProgression = new List<InspirationProgressData>();

	// Token: 0x040020CA RID: 8394
	public bool isInspirationActionIndicatorBlocked;

	// Token: 0x040020CB RID: 8395
	[NonSerialized]
	public List<InspirationData> activeInspirations;

	// Token: 0x040020CC RID: 8396
	[NonSerialized]
	public TalentExpLevelBalanceData talentExpLevelBalanceData;
}
