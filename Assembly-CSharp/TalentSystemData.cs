using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020005AA RID: 1450
[Serializable]
public class TalentSystemData
{
	// Token: 0x14000072 RID: 114
	// (add) Token: 0x06002573 RID: 9587 RVA: 0x000AFA3C File Offset: 0x000ADC3C
	// (remove) Token: 0x06002574 RID: 9588 RVA: 0x000AFA74 File Offset: 0x000ADC74
	public event TalentSystemData.DelTalentExpChanged OnTalentExpChanged;

	// Token: 0x14000073 RID: 115
	// (add) Token: 0x06002575 RID: 9589 RVA: 0x000AFAAC File Offset: 0x000ADCAC
	// (remove) Token: 0x06002576 RID: 9590 RVA: 0x000AFAE4 File Offset: 0x000ADCE4
	public event Action<string> OnInspirationProgressChanged;

	// Token: 0x14000074 RID: 116
	// (add) Token: 0x06002577 RID: 9591 RVA: 0x000AFB1C File Offset: 0x000ADD1C
	// (remove) Token: 0x06002578 RID: 9592 RVA: 0x000AFB54 File Offset: 0x000ADD54
	public event Action<string> OnInspirationCompleted;

	// Token: 0x14000075 RID: 117
	// (add) Token: 0x06002579 RID: 9593 RVA: 0x000AFB8C File Offset: 0x000ADD8C
	// (remove) Token: 0x0600257A RID: 9594 RVA: 0x000AFBC4 File Offset: 0x000ADDC4
	public event Action<string> OnInspirationPurchased;

	// Token: 0x14000076 RID: 118
	// (add) Token: 0x0600257B RID: 9595 RVA: 0x000AFBFC File Offset: 0x000ADDFC
	// (remove) Token: 0x0600257C RID: 9596 RVA: 0x000AFC34 File Offset: 0x000ADE34
	public event TalentSystemData.DelTalentLevelPurchased OnTalentLevelPurchased;

	// Token: 0x0600257D RID: 9597 RVA: 0x000AFC69 File Offset: 0x000ADE69
	public TalentSystemData()
	{
	}

	// Token: 0x0600257E RID: 9598 RVA: 0x000AFC7C File Offset: 0x000ADE7C
	public TalentSystemData(List<TalentDef> talentDefData)
	{
		foreach (TalentDef talentDef in talentDefData)
		{
			TalentData talentData = new TalentData(talentDef.id);
			this.talentData.Add(talentData);
		}
	}

	// Token: 0x0600257F RID: 9599 RVA: 0x000AFCEC File Offset: 0x000ADEEC
	public void PrepareForGame()
	{
		this.SyncPlayerTalentLevelUpsFromBalance();
		TalentSystemCache.Instance.ClearCache();
		TalentSystemCache.Instance.CreateCache();
		foreach (TalentData talentData in this.talentData)
		{
			talentData.PrepareForGame();
		}
	}

	// Token: 0x06002580 RID: 9600 RVA: 0x000AFD58 File Offset: 0x000ADF58
	public void UnPrepareFromGame()
	{
		TalentSystemCache.Instance.ClearCache();
	}

	// Token: 0x06002581 RID: 9601 RVA: 0x000AFD64 File Offset: 0x000ADF64
	public void AddToInspiration(string inspirationId, int counterToAdd)
	{
		InspirationLevelData inspirationLevelData;
		if (GameBalance.Me.inspirationLevelsCache.TryGetValue(inspirationId, out inspirationLevelData))
		{
			TalentData talentBranch = this.GetTalentBranch(inspirationLevelData.talentId);
			InspirationData inspirationData;
			if (TalentSystemCache.Instance.inspirations.TryGetValue(inspirationId, out inspirationData))
			{
				inspirationData.curProgressValue += counterToAdd;
				int num = talentBranch.inspirationsProgression.FindIndex((InspirationProgressData x) => x.id == inspirationId);
				if (num == -1)
				{
					talentBranch.inspirationsProgression.Add(new InspirationProgressData(inspirationId, inspirationData.curProgressValue, 0));
				}
				else
				{
					talentBranch.inspirationsProgression[num].currentValue = inspirationData.curProgressValue;
				}
				if (inspirationData.IsHidden)
				{
					return;
				}
				this.CompleteInspiration(inspirationData);
				Action<string> onInspirationProgressChanged = this.OnInspirationProgressChanged;
				if (onInspirationProgressChanged == null)
				{
					return;
				}
				onInspirationProgressChanged(inspirationData.id);
			}
		}
	}

	// Token: 0x06002582 RID: 9602 RVA: 0x000AFE4C File Offset: 0x000AE04C
	public void CompleteInspiration(InspirationData inspirationData)
	{
		if (!inspirationData.isAllLevelsBought && inspirationData.IsCompleted)
		{
			MainGame.Instance.GameSave.knowledgeSystem.TryUnlockInspirationTab();
			bool flag = false;
			for (int i = inspirationData.curLevel; i <= inspirationData.MaxLevel; i++)
			{
				InspirationDef dataForLevel = InspirationDef.GetDataForLevel(inspirationData.id, i);
				if (!inspirationData.CompletedInspirations.Contains(dataForLevel) && inspirationData.curProgressValue >= dataForLevel.completionGoalValue)
				{
					inspirationData.CompletedInspirations.Add(dataForLevel);
					if (!flag)
					{
						this.UnblockInspirationActionIndicator(inspirationData);
						flag = true;
					}
					Action<string> onInspirationCompleted = this.OnInspirationCompleted;
					if (onInspirationCompleted != null)
					{
						onInspirationCompleted(dataForLevel.id);
					}
				}
			}
		}
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x000AFEF4 File Offset: 0x000AE0F4
	private void UnblockInspirationActionIndicator(InspirationData inspirationData)
	{
		InspirationLevelData inspirationLevelData;
		if (!GameBalance.Me.inspirationLevelsCache.TryGetValue(inspirationData.id, out inspirationLevelData))
		{
			return;
		}
		TalentData talentBranch = this.GetTalentBranch(inspirationLevelData.talentId);
		if (talentBranch == null)
		{
			return;
		}
		talentBranch.UnblockInspirationActionIndicator();
	}

	// Token: 0x06002584 RID: 9604 RVA: 0x000AFF34 File Offset: 0x000AE134
	public bool HasTwoZeroFaithInspirationsToBuyInSameBranch()
	{
		foreach (TalentData talentData in this.talentData)
		{
			if (this.HasTwoZeroFaithInspirationsToBuy(talentData))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002585 RID: 9605 RVA: 0x000AFF90 File Offset: 0x000AE190
	public bool TryGetTalentIdWithTwoZeroFaithInspirationsToBuy(out string talentId)
	{
		foreach (TalentData talentData in this.talentData)
		{
			if (this.HasTwoZeroFaithInspirationsToBuy(talentData))
			{
				talentId = talentData.id;
				return true;
			}
		}
		talentId = null;
		return false;
	}

	// Token: 0x06002586 RID: 9606 RVA: 0x000AFFF8 File Offset: 0x000AE1F8
	public bool HasTwoZeroFaithInspirationsToBuy(TalentData talent)
	{
		return TalentSystemData.CountZeroFaithInspirationsToBuy(talent) >= 2;
	}

	// Token: 0x06002587 RID: 9607 RVA: 0x000B0008 File Offset: 0x000AE208
	private static int CountZeroFaithInspirationsToBuy(TalentData talent)
	{
		if (talent.activeInspirations == null)
		{
			return 0;
		}
		int num = 0;
		foreach (InspirationData inspirationData in talent.activeInspirations)
		{
			if (!inspirationData.IsHidden && inspirationData.IsCompleted && !inspirationData.isAllLevelsBought)
			{
				InspirationDef dataForLevel = InspirationDef.GetDataForLevel(inspirationData.id, inspirationData.curLevel);
				if (dataForLevel != null && dataForLevel.completionPrice == 0)
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06002588 RID: 9608 RVA: 0x000B009C File Offset: 0x000AE29C
	public void PurchaseInspiration(string inspirationId)
	{
		InspirationLevelData inspirationLevelData;
		if (GameBalance.Me.inspirationLevelsCache.TryGetValue(inspirationId, out inspirationLevelData))
		{
			TalentData talentBranch = this.GetTalentBranch(inspirationLevelData.talentId);
			InspirationData inspirationData;
			if (TalentSystemCache.Instance.inspirations.TryGetValue(inspirationId, out inspirationData))
			{
				int completionExp = InspirationDef.GetDataForLevel(inspirationId, inspirationData.curLevel).completionExp;
				this.AddExp(inspirationLevelData.talentId, completionExp);
				int num = talentBranch.inspirationsProgression.FindIndex((InspirationProgressData x) => x.id == inspirationId);
				if (num == -1)
				{
					talentBranch.inspirationsProgression.Add(new InspirationProgressData(inspirationId, inspirationData.curProgressValue, inspirationData.completionGoalValue));
				}
				else
				{
					talentBranch.inspirationsProgression[num].completionGoalValue += inspirationData.completionGoalValue;
				}
				inspirationData.DoLevelUp();
				LazyAudio.PlayAndForget("get_inspiration");
				if (talentBranch.CanUpgradeLevel)
				{
					talentBranch.talentExpPoints++;
					talentBranch.curExp -= talentBranch.talentExpLevelBalanceData.GetExpForLevel(talentBranch.curTalentLevel);
					talentBranch.curTalentLevel++;
				}
				AchievementsSystem.Instance.TriggerCountable("inspiration_unlocked", 1);
				Action<string> onInspirationPurchased = this.OnInspirationPurchased;
				if (onInspirationPurchased == null)
				{
					return;
				}
				onInspirationPurchased(inspirationId);
				return;
			}
			else
			{
				Debug.LogError(string.Concat(new string[] { "Inspiration [", inspirationId, "] wasn't found for talent [", inspirationLevelData.talentId, "]" }));
			}
		}
	}

	// Token: 0x06002589 RID: 9609 RVA: 0x000B0238 File Offset: 0x000AE438
	public TalentData GetTalentBranch(string talentId)
	{
		return this.talentData.Find((TalentData x) => x.id == talentId);
	}

	// Token: 0x0600258A RID: 9610 RVA: 0x000B026C File Offset: 0x000AE46C
	public bool AddTalentValue(string talentId, int value)
	{
		TalentData talentBranch = this.GetTalentBranch(talentId);
		if (talentBranch == null)
		{
			Debug.LogError("Talent [" + talentId + "] wasn't found");
			return false;
		}
		talentBranch.curTalentValue += value;
		return true;
	}

	// Token: 0x0600258B RID: 9611 RVA: 0x000B02AC File Offset: 0x000AE4AC
	public bool CanPurchaseLevel(string talentLevelId, out TalentData talentData)
	{
		talentData = null;
		TalentLevelUpDef data = GameBalance.Me.GetData<TalentLevelUpDef>(talentLevelId);
		if (data == null)
		{
			return false;
		}
		talentData = this.GetTalentBranch(data.talentId);
		return talentData.talentExpPoints >= data.talentExpPointsPrice && data.ParentsUnlocked;
	}

	// Token: 0x0600258C RID: 9612 RVA: 0x000B02F4 File Offset: 0x000AE4F4
	public void PurchaseLevel(string talentLevelId, bool free = false)
	{
		TalentData talentData;
		if (!this.CanPurchaseLevel(talentLevelId, out talentData))
		{
			return;
		}
		talentData.studiedLevelUps.Add(talentLevelId);
		TalentLevelUpDef data = GameBalance.Me.GetData<TalentLevelUpDef>(talentLevelId);
		if (!string.IsNullOrEmpty(data.linkedPerk))
		{
			MainGame.Instance.GameSave.perkSystemData.AddPerk(data.linkedPerk);
		}
		if (!free)
		{
			talentData.talentExpPoints -= data.talentExpPointsPrice;
		}
		if (data.talentValueAdd > 0)
		{
			talentData.curTalentValue += data.talentValueAdd;
		}
		foreach (LazyExpression lazyExpression in data.expressionsOnBuy)
		{
			lazyExpression.Evaluate();
		}
		TalentSystemData.DelTalentLevelPurchased onTalentLevelPurchased = this.OnTalentLevelPurchased;
		if (onTalentLevelPurchased == null)
		{
			return;
		}
		onTalentLevelPurchased(talentData.id, talentLevelId);
	}

	// Token: 0x0600258D RID: 9613 RVA: 0x000B03DC File Offset: 0x000AE5DC
	private void AddExp(string talentBranchId, int exp)
	{
		TalentData talentBranch = this.GetTalentBranch(talentBranchId);
		if (talentBranch != null)
		{
			talentBranch.curExp += exp;
			TalentSystemData.DelTalentExpChanged onTalentExpChanged = this.OnTalentExpChanged;
			if (onTalentExpChanged == null)
			{
				return;
			}
			onTalentExpChanged(talentBranchId, talentBranch.curExp);
		}
	}

	// Token: 0x0600258E RID: 9614 RVA: 0x000B041C File Offset: 0x000AE61C
	private void SyncPlayerTalentLevelUpsFromBalance()
	{
		foreach (TalentData talentData in this.talentData)
		{
			TalentSystemData.RemoveMissingTalentLevelUps(talentData.studiedLevelUps);
		}
		PerkSystemData perkSystemData = MainGame.Instance.GameSave.perkSystemData;
		foreach (TalentLevelUpDef talentLevelUpDef in GameBalance.Me.talentLevelUpDefs)
		{
			if (!talentLevelUpDef.isZombiePerk)
			{
				TalentData talentBranch = this.GetTalentBranch(talentLevelUpDef.talentId);
				if (talentBranch != null)
				{
					if (talentLevelUpDef.availableAtStart && !talentBranch.studiedLevelUps.Contains(talentLevelUpDef.id))
					{
						talentBranch.studiedLevelUps.Add(talentLevelUpDef.id);
						if (talentLevelUpDef.talentValueAdd > 0)
						{
							talentBranch.curTalentValue += talentLevelUpDef.talentValueAdd;
						}
					}
					if (talentBranch.studiedLevelUps.Contains(talentLevelUpDef.id) && !string.IsNullOrEmpty(talentLevelUpDef.linkedPerk) && !perkSystemData.HasPerk(talentLevelUpDef.linkedPerk))
					{
						perkSystemData.AddPerk(talentLevelUpDef.linkedPerk);
					}
				}
			}
		}
	}

	// Token: 0x0600258F RID: 9615 RVA: 0x000B056C File Offset: 0x000AE76C
	private static void RemoveMissingTalentLevelUps(List<string> ids)
	{
		for (int i = ids.Count - 1; i >= 0; i--)
		{
			if (GameBalance.Me.GetData<TalentLevelUpDef>(ids[i]) == null)
			{
				ids.RemoveAt(i);
			}
		}
	}

	// Token: 0x040020D8 RID: 8408
	public List<TalentData> talentData = new List<TalentData>();

	// Token: 0x020005AB RID: 1451
	// (Invoke) Token: 0x06002591 RID: 9617
	public delegate void DelTalentExpChanged(string talentId, int curExp);

	// Token: 0x020005AC RID: 1452
	// (Invoke) Token: 0x06002595 RID: 9621
	public delegate void DelTalentLevelPurchased(string talentId, string talentLevelId);

	// Token: 0x020005AD RID: 1453
	// (Invoke) Token: 0x06002599 RID: 9625
	public delegate void DelTalentLevelUnlocked(string talentLevelId, List<string> talentLevelIds);
}
