using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
[Serializable]
public class KnowledgeSystem
{
	// Token: 0x06001F98 RID: 8088 RVA: 0x00095900 File Offset: 0x00093B00
	public void PrepareForGame()
	{
		if (this.unlockedCustomHudDaySprites == null)
		{
			this.unlockedCustomHudDaySprites = new List<string>();
		}
		this.SyncNewTechsFromBalance();
		this.RebuildUnlockedTechRewards();
		MainGame.Instance.GameSave.questSystemData.OnQuestCompleted -= this.HandleQuestCompleted;
		MainGame.Instance.GameSave.questSystemData.OnQuestCompleted += this.HandleQuestCompleted;
		this.SyncTalentLevelUpsVisibilityFromBalance();
		this.TryUnlockInspirationTab();
	}

	// Token: 0x06001F99 RID: 8089 RVA: 0x00095978 File Offset: 0x00093B78
	private void SyncNewTechsFromBalance()
	{
		GameBalance me = GameBalance.Me;
		List<TechDef> list = ((me != null) ? me.techDefs : null);
		if (list == null)
		{
			return;
		}
		if (this.revealedTechs == null)
		{
			this.revealedTechs = new List<string>();
			for (int i = 0; i < list.Count; i++)
			{
				TechDef techDef = list[i];
				if (techDef != null && !string.IsNullOrEmpty(techDef.id) && techDef.hiddenAtStart && !this.IsTechUnlocked(techDef.id) && !this.IsTechHidden(techDef.id))
				{
					this.revealedTechs.Add(techDef.id);
				}
			}
		}
		else
		{
			KnowledgeSystem.RemoveMissingTechs(this.revealedTechs);
		}
		for (int j = 0; j < list.Count; j++)
		{
			TechDef techDef2 = list[j];
			if (techDef2 != null && !string.IsNullOrEmpty(techDef2.id))
			{
				this.ApplyNewTechIntroState(techDef2);
			}
		}
		this.RevealTechsFromCompletedQuests();
		this.CatchUpRevealedTechsFromTree();
	}

	// Token: 0x06001F9A RID: 8090 RVA: 0x00095A5C File Offset: 0x00093C5C
	public void RevealTechsFromCompletedQuests()
	{
		QuestSystemData questSystemData = MainGame.Instance.GameSave.questSystemData;
		GameBalance me = GameBalance.Me;
		List<QuestDef> list = ((me != null) ? me.questDefs : null);
		if (questSystemData == null || list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			QuestDef questDef = list[i];
			if (questDef != null && !string.IsNullOrEmpty(questDef.id) && questSystemData.IsQuestInStatus(questDef.id, QuestStatus.Completed))
			{
				this.ReplayRevealTechExpressions(questDef.id, questDef.execExpressionsStart);
				this.ReplayRevealTechExpressions(questDef.id, questDef.execExpressionsFinish);
			}
		}
	}

	// Token: 0x06001F9B RID: 8091 RVA: 0x00095AF0 File Offset: 0x00093CF0
	private void ReplayRevealTechExpressions(string questId, List<LazyExpression> expressions)
	{
		if (expressions == null)
		{
			return;
		}
		for (int i = 0; i < expressions.Count; i++)
		{
			string text;
			if (KnowledgeSystem.TryGetRevealTechId(expressions[i], out text) && this.IsTechHidden(text))
			{
				this.RevealTech(text);
				Debug.Log(string.Concat(new string[] { "Knowledge: reveal tech [", text, "] from completed quest [", questId, "]" }));
			}
		}
	}

	// Token: 0x06001F9C RID: 8092 RVA: 0x00095B64 File Offset: 0x00093D64
	private static bool TryGetRevealTechId(LazyExpression expression, out string techId)
	{
		techId = null;
		string text = ((expression != null) ? expression.GetRawExpressionString() : null);
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		if (!text.StartsWith("RevealTech(\"", StringComparison.Ordinal) || !text.EndsWith("\")", StringComparison.Ordinal))
		{
			return false;
		}
		int num = text.Length - "RevealTech(\"".Length - 2;
		if (num <= 0)
		{
			return false;
		}
		techId = text.Substring("RevealTech(\"".Length, num);
		return true;
	}

	// Token: 0x06001F9D RID: 8093 RVA: 0x00095BD7 File Offset: 0x00093DD7
	public void CatchUpRevealedTechsFromTree()
	{
		this.FillRevealedTechsFromTreeState();
		this.RevealHiddenTechsIfParentVisibleInTree();
	}

	// Token: 0x06001F9E RID: 8094 RVA: 0x00095BE8 File Offset: 0x00093DE8
	public void FillRevealedTechsFromTreeState()
	{
		GameBalance me = GameBalance.Me;
		List<TechDef> list = ((me != null) ? me.techDefs : null);
		if (list == null)
		{
			return;
		}
		if (this.revealedTechs == null)
		{
			this.revealedTechs = new List<string>();
		}
		for (int i = 0; i < list.Count; i++)
		{
			TechDef techDef = list[i];
			if (techDef != null && !string.IsNullOrEmpty(techDef.id) && techDef.hiddenAtStart && !this.IsTechHidden(techDef.id))
			{
				this.MarkTechRevealed(techDef.id);
			}
		}
	}

	// Token: 0x06001F9F RID: 8095 RVA: 0x00095C6C File Offset: 0x00093E6C
	public void RevealHiddenTechsIfParentVisibleInTree()
	{
		GameBalance me = GameBalance.Me;
		List<TechDef> list = ((me != null) ? me.techDefs : null);
		if (list == null)
		{
			return;
		}
		bool flag;
		do
		{
			flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				TechDef techDef = list[i];
				if (techDef != null && !string.IsNullOrEmpty(techDef.id) && this.IsTechHidden(techDef.id) && this.HasVisibleHiddenAtStartParent(techDef))
				{
					this.RevealTech(techDef.id);
					flag = true;
				}
			}
		}
		while (flag);
	}

	// Token: 0x06001FA0 RID: 8096 RVA: 0x00095CE4 File Offset: 0x00093EE4
	private bool HasVisibleHiddenAtStartParent(TechDef techDef)
	{
		List<string> parents = techDef.parents;
		if (parents == null || parents.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < parents.Count; i++)
		{
			string text = parents[i];
			if (!string.IsNullOrEmpty(text) && !this.IsTechHidden(text))
			{
				TechDef dataOrNull = GameBalance.Me.GetDataOrNull<TechDef>(text);
				if (dataOrNull != null && dataOrNull.hiddenAtStart)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001FA1 RID: 8097 RVA: 0x00095D4C File Offset: 0x00093F4C
	private void ApplyNewTechIntroState(TechDef techDef)
	{
		bool flag = this.IsTechRevealed(techDef.id);
		if (techDef.hiddenAtStart && !this.IsTechUnlocked(techDef.id) && !this.IsTechHidden(techDef.id) && !flag)
		{
			this.hiddenTechs.Add(techDef.id);
			Debug.Log("Knowledge: hide new tech [" + techDef.id + "]");
		}
		else if (!techDef.hiddenAtStart && !this.IsTechUnlocked(techDef.id) && this.IsTechHidden(techDef.id))
		{
			this.hiddenTechs.Remove(techDef.id);
			Debug.Log("Knowledge: unhide tech [" + techDef.id + "]");
		}
		if (techDef.availableAtStart && !this.IsTechUnlocked(techDef.id))
		{
			this.UnlockTech(techDef.id, true);
		}
	}

	// Token: 0x06001FA2 RID: 8098 RVA: 0x00095E2D File Offset: 0x0009402D
	private void MarkTechRevealed(string techId)
	{
		if (this.revealedTechs == null)
		{
			this.revealedTechs = new List<string>();
		}
		if (!this.revealedTechs.Contains(techId))
		{
			this.revealedTechs.Add(techId);
		}
	}

	// Token: 0x06001FA3 RID: 8099 RVA: 0x00095E5C File Offset: 0x0009405C
	private static void RemoveMissingTechs(List<string> ids)
	{
		if (ids == null)
		{
			return;
		}
		for (int i = ids.Count - 1; i >= 0; i--)
		{
			if (GameBalance.Me.GetDataOrNull<TechDef>(ids[i]) == null)
			{
				ids.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001FA4 RID: 8100 RVA: 0x00095E9C File Offset: 0x0009409C
	private void RebuildUnlockedTechRewards()
	{
		GameBalance me = GameBalance.Me;
		List<TechDef> list = ((me != null) ? me.techDefs : null);
		if (list == null)
		{
			return;
		}
		HashSet<string> hashSet = ((this.unlockedTechs != null) ? new HashSet<string>(this.unlockedTechs) : new HashSet<string>());
		HashSet<string> hashSet2 = new HashSet<string>();
		HashSet<string> hashSet3 = new HashSet<string>();
		HashSet<string> hashSet4 = new HashSet<string>();
		HashSet<string> hashSet5 = new HashSet<string>();
		for (int i = 0; i < list.Count; i++)
		{
			TechDef techDef = list[i];
			if (techDef != null && hashSet.Contains(techDef.id))
			{
				KnowledgeSystem.CollectTechRewardIds(techDef.craftsAfterUnlock, hashSet2);
				KnowledgeSystem.CollectTechRewardIds(techDef.alchemyFormulasAfterUnlock, hashSet3);
				KnowledgeSystem.CollectTechRewardIds(techDef.buildingsAfterUnlock, hashSet4);
				KnowledgeSystem.CollectTechRewardIds(techDef.townBuildingsAfterUnlock, hashSet5);
			}
		}
		KnowledgeSystem.UnlockExpectedTechRewards(hashSet2, new Action<string>(this.UnlockCraft));
		KnowledgeSystem.UnlockExpectedTechRewards(hashSet3, new Action<string>(this.UnlockAlchemyFormula));
		KnowledgeSystem.UnlockExpectedTechRewards(hashSet4, new Action<string>(this.UnlockBuilding));
		KnowledgeSystem.UnlockExpectedTechRewards(hashSet5, new Action<string>(this.UnlockTownBuilding));
	}

	// Token: 0x06001FA5 RID: 8101 RVA: 0x00095FA8 File Offset: 0x000941A8
	private static void CollectTechRewardIds(List<string> source, HashSet<string> expected)
	{
		if (source == null)
		{
			return;
		}
		for (int i = 0; i < source.Count; i++)
		{
			string text = source[i];
			if (!string.IsNullOrEmpty(text))
			{
				expected.Add(text);
			}
		}
	}

	// Token: 0x06001FA6 RID: 8102 RVA: 0x00095FE4 File Offset: 0x000941E4
	private static void UnlockExpectedTechRewards(HashSet<string> expected, Action<string> unlock)
	{
		foreach (string text in expected)
		{
			unlock(text);
		}
	}

	// Token: 0x06001FA7 RID: 8103 RVA: 0x00096034 File Offset: 0x00094234
	public void UnPrepareForGame()
	{
		MainGame.Instance.GameSave.questSystemData.OnQuestCompleted -= this.HandleQuestCompleted;
	}

	// Token: 0x06001FA8 RID: 8104 RVA: 0x00096056 File Offset: 0x00094256
	private void HandleQuestCompleted(QuestData _)
	{
		this.TryRevealInspirations();
	}

	// Token: 0x06001FA9 RID: 8105 RVA: 0x00096060 File Offset: 0x00094260
	private void SyncTalentLevelUpsVisibilityFromBalance()
	{
		KnowledgeSystem.RemoveMissingTalentLevelUps(this.hiddenTalentLevelUps);
		KnowledgeSystem.RemoveMissingTalentLevelUps(this.unknownTalentLevelUps);
		TalentSystemData talentSystemData = MainGame.Instance.GameSave.talentSystemData;
		if (this.revealedTalentLevelUps == null)
		{
			this.revealedTalentLevelUps = new List<string>();
			using (List<TalentLevelUpDef>.Enumerator enumerator = GameBalance.Me.talentLevelUpDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TalentLevelUpDef talentLevelUpDef = enumerator.Current;
					if (talentLevelUpDef.isHidden && !KnowledgeSystem.IsPlayerTalentLevelUpStudied(talentSystemData, talentLevelUpDef) && !this.hiddenTalentLevelUps.Contains(talentLevelUpDef.id))
					{
						this.revealedTalentLevelUps.Add(talentLevelUpDef.id);
					}
				}
				goto IL_00AC;
			}
		}
		KnowledgeSystem.RemoveMissingTalentLevelUps(this.revealedTalentLevelUps);
		IL_00AC:
		foreach (TalentLevelUpDef talentLevelUpDef2 in GameBalance.Me.talentLevelUpDefs)
		{
			bool flag = KnowledgeSystem.IsPlayerTalentLevelUpStudied(talentSystemData, talentLevelUpDef2);
			bool flag2 = this.revealedTalentLevelUps.Contains(talentLevelUpDef2.id);
			if (talentLevelUpDef2.isHidden)
			{
				if (!flag && !flag2 && !this.hiddenTalentLevelUps.Contains(talentLevelUpDef2.id))
				{
					this.hiddenTalentLevelUps.Add(talentLevelUpDef2.id);
				}
			}
			else
			{
				this.hiddenTalentLevelUps.Remove(talentLevelUpDef2.id);
			}
			if (talentLevelUpDef2.isUnknown)
			{
				if (!flag && !flag2 && !this.unknownTalentLevelUps.Contains(talentLevelUpDef2.id))
				{
					this.unknownTalentLevelUps.Add(talentLevelUpDef2.id);
				}
			}
			else
			{
				this.unknownTalentLevelUps.Remove(talentLevelUpDef2.id);
			}
		}
	}

	// Token: 0x06001FAA RID: 8106 RVA: 0x0009621C File Offset: 0x0009441C
	private static bool IsPlayerTalentLevelUpStudied(TalentSystemData talentSystem, TalentLevelUpDef def)
	{
		if (def.isZombiePerk)
		{
			return false;
		}
		TalentData talentBranch = talentSystem.GetTalentBranch(def.talentId);
		return talentBranch != null && talentBranch.studiedLevelUps.Contains(def.id);
	}

	// Token: 0x06001FAB RID: 8107 RVA: 0x00096258 File Offset: 0x00094458
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

	// Token: 0x06001FAC RID: 8108 RVA: 0x00096292 File Offset: 0x00094492
	public void UnlockVendorForOrders(string vendorId)
	{
		if (!this.unlockedVendorsForOrders.Contains(vendorId))
		{
			this.unlockedVendorsForOrders.Add(vendorId);
			Debug.Log("Knowledge: unlock vendor for orders [" + vendorId + "]");
		}
	}

	// Token: 0x06001FAD RID: 8109 RVA: 0x000962C3 File Offset: 0x000944C3
	public bool IsVendorForOrdersUnlocked(string vendorId)
	{
		return this.unlockedVendorsForOrders.Contains(vendorId);
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x000962D1 File Offset: 0x000944D1
	public void UnlockTalentBranch(string talentBranchId)
	{
		if (this.unlockedTalentIds.Contains(talentBranchId))
		{
			Debug.LogWarning("TalentBranch [" + talentBranchId + "] is already unlocked");
			return;
		}
		this.unlockedTalentIds.Add(talentBranchId);
	}

	// Token: 0x06001FAF RID: 8111 RVA: 0x00096303 File Offset: 0x00094503
	public bool IsTalentBranchUnlocked(string talentBranchId)
	{
		return this.unlockedTalentIds.Contains(talentBranchId);
	}

	// Token: 0x06001FB0 RID: 8112 RVA: 0x00096303 File Offset: 0x00094503
	public bool IsTalentBranchHasWaitingInspiration(string talentBranchId)
	{
		return this.unlockedTalentIds.Contains(talentBranchId);
	}

	// Token: 0x06001FB1 RID: 8113 RVA: 0x00096311 File Offset: 0x00094511
	public void AddPhraseToBlackList(string phraseId)
	{
		if (!this.blackListPhrases.Contains(phraseId))
		{
			this.blackListPhrases.Add(phraseId);
		}
	}

	// Token: 0x06001FB2 RID: 8114 RVA: 0x0009632D File Offset: 0x0009452D
	public void RemovePhraseFromBlackList(string phraseId)
	{
		this.blackListPhrases.Remove(phraseId);
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x0009633C File Offset: 0x0009453C
	public void UnlockPhrase(string phraseId)
	{
		if (!this.unlockedPhrases.Contains(phraseId))
		{
			this.unlockedPhrases.Add(phraseId);
		}
	}

	// Token: 0x06001FB4 RID: 8116 RVA: 0x00096358 File Offset: 0x00094558
	public void UnlockTech(string techId, bool silent)
	{
		if (!this.unlockedTechs.Contains(techId))
		{
			this.unlockedTechs.Add(techId);
			Debug.Log("Knowledge: unlock tech [" + techId + "]");
			Action<string, bool> onTechUnlocked = KnowledgeSystem.OnTechUnlocked;
			if (onTechUnlocked == null)
			{
				return;
			}
			onTechUnlocked(techId, silent);
		}
	}

	// Token: 0x06001FB5 RID: 8117 RVA: 0x000963A8 File Offset: 0x000945A8
	public void AddDelayedDemoTechUnlock(string techId)
	{
		if (string.IsNullOrEmpty(techId) || this.IsTechUnlocked(techId))
		{
			return;
		}
		if (this.delayedDemoTechUnlocks == null)
		{
			this.delayedDemoTechUnlocks = new List<string>();
		}
		if (!this.delayedDemoTechUnlocks.Contains(techId))
		{
			this.delayedDemoTechUnlocks.Add(techId);
			Debug.Log("Knowledge: delay demo tech unlock [" + techId + "]");
		}
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x0009640C File Offset: 0x0009460C
	public void ApplyDelayedDemoTechUnlocks()
	{
		if (this.delayedDemoTechUnlocks == null || this.delayedDemoTechUnlocks.Count == 0)
		{
			return;
		}
		List<string> list = new List<string>(this.delayedDemoTechUnlocks);
		this.delayedDemoTechUnlocks.Clear();
		foreach (string text in list)
		{
			TechDef dataOrNull = GameBalance.Me.GetDataOrNull<TechDef>(text);
			if (dataOrNull != null && !this.IsTechUnlocked(text))
			{
				dataOrNull.Unlock(true);
			}
		}
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x000964A0 File Offset: 0x000946A0
	public void RemoveTech(string techId)
	{
		if (this.unlockedTechs.Contains(techId))
		{
			this.unlockedTechs.Remove(techId);
			Debug.Log("Knowledge: remove tech [" + techId + "]");
		}
	}

	// Token: 0x06001FB8 RID: 8120 RVA: 0x000964D2 File Offset: 0x000946D2
	public bool IsTechUnlocked(string techId)
	{
		return this.unlockedTechs.Contains(techId);
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x000964E0 File Offset: 0x000946E0
	public bool IsTechHidden(string techId)
	{
		return this.hiddenTechs.Contains(techId);
	}

	// Token: 0x06001FBA RID: 8122 RVA: 0x000964EE File Offset: 0x000946EE
	public bool IsTechRevealed(string techId)
	{
		return this.revealedTechs != null && this.revealedTechs.Contains(techId);
	}

	// Token: 0x06001FBB RID: 8123 RVA: 0x00096508 File Offset: 0x00094708
	public void HideTech(string techId)
	{
		if (this.revealedTechs != null)
		{
			this.revealedTechs.Remove(techId);
		}
		if (!this.IsTechHidden(techId))
		{
			this.hiddenTechs.Add(techId);
			Debug.Log("Knowledge: hide tech [" + techId + "]");
			foreach (TechDef techDef in GameBalance.Me.GetData<TechDef>(techId).childDefinitionList)
			{
				this.HideTech(techDef.id);
			}
		}
	}

	// Token: 0x06001FBC RID: 8124 RVA: 0x000965AC File Offset: 0x000947AC
	public void RevealTech(string techId)
	{
		this.MarkTechRevealed(techId);
		if (this.IsTechHidden(techId))
		{
			this.hiddenTechs.Remove(techId);
			Debug.Log("Knowledge: reveal tech [" + techId + "]");
			foreach (TechDef techDef in GameBalance.Me.GetData<TechDef>(techId).childDefinitionList)
			{
				this.RevealTech(techDef.id);
			}
			Action<string> onTechRevealed = KnowledgeSystem.OnTechRevealed;
			if (onTechRevealed == null)
			{
				return;
			}
			onTechRevealed(techId);
		}
	}

	// Token: 0x06001FBD RID: 8125 RVA: 0x00096650 File Offset: 0x00094850
	public void UnlockTechTab(TechTreeTab techTreeTab)
	{
		if (this.IsTechTabLocked(techTreeTab))
		{
			this.lockedTechTabs.Remove(techTreeTab);
		}
	}

	// Token: 0x06001FBE RID: 8126 RVA: 0x00096668 File Offset: 0x00094868
	public void LockTechTab(TechTreeTab techTreeTab)
	{
		if (!this.IsTechTabLocked(techTreeTab))
		{
			this.lockedTechTabs.Add(techTreeTab);
		}
	}

	// Token: 0x06001FBF RID: 8127 RVA: 0x0009667F File Offset: 0x0009487F
	public void UnlockMapFightIcon(string icon)
	{
		if (!this.activeMapFightIcons.Contains(icon))
		{
			this.activeMapFightIcons.Add(icon);
		}
	}

	// Token: 0x06001FC0 RID: 8128 RVA: 0x0009669B File Offset: 0x0009489B
	public void LockMapFightIcon(string icon)
	{
		if (this.activeMapFightIcons.Contains(icon))
		{
			this.activeMapFightIcons.Remove(icon);
		}
	}

	// Token: 0x06001FC1 RID: 8129 RVA: 0x000966B8 File Offset: 0x000948B8
	public bool IsTechTabLocked(TechTreeTab techTreeTab)
	{
		return this.lockedTechTabs.Contains(techTreeTab);
	}

	// Token: 0x06001FC2 RID: 8130 RVA: 0x000966C6 File Offset: 0x000948C6
	public void UnlockCharTab(CharacterWindowData.CharPage page)
	{
		if (this.IsCharTabLocked(page))
		{
			this.lockedCharacterWindowTabs.Remove(page);
		}
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x000966DE File Offset: 0x000948DE
	public void LockCharTab(CharacterWindowData.CharPage page)
	{
		if (!this.IsCharTabLocked(page))
		{
			this.lockedCharacterWindowTabs.Add(page);
		}
	}

	// Token: 0x06001FC4 RID: 8132 RVA: 0x000966F5 File Offset: 0x000948F5
	public bool IsCharTabLocked(CharacterWindowData.CharPage page)
	{
		return this.lockedCharacterWindowTabs.Contains(page);
	}

	// Token: 0x06001FC5 RID: 8133 RVA: 0x00096703 File Offset: 0x00094903
	public void TryUnlockInspirationTab()
	{
		if (!this.IsCharTabLocked(CharacterWindowData.CharPage.Inspiration))
		{
			return;
		}
		if (!MainGame.Instance.GameSave.talentSystemData.HasTwoZeroFaithInspirationsToBuyInSameBranch())
		{
			return;
		}
		this.UnlockCharTab(CharacterWindowData.CharPage.Inspiration);
	}

	// Token: 0x06001FC6 RID: 8134 RVA: 0x0009672D File Offset: 0x0009492D
	public bool IsTechTabUnlocked(TechTreeTab techTreeTab)
	{
		return !this.lockedTechTabs.Contains(techTreeTab);
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x0009673E File Offset: 0x0009493E
	public void UnlockCraft(string craftId)
	{
		if (!this.unlockedCrafts.Contains(craftId))
		{
			this.unlockedCrafts.Add(craftId);
			Debug.Log("Knowledge: unlock craft [" + craftId + "]");
		}
	}

	// Token: 0x06001FC8 RID: 8136 RVA: 0x0009676F File Offset: 0x0009496F
	public void RemoveUnlockedCraft(string craftId)
	{
		if (this.unlockedCrafts.Remove(craftId))
		{
			Debug.Log("Knowledge: remove unlocked craft [" + craftId + "]");
		}
	}

	// Token: 0x06001FC9 RID: 8137 RVA: 0x00096794 File Offset: 0x00094994
	public bool IsSurveyCompleted(SurveyDef surveyDef)
	{
		return surveyDef != null && surveyDef.isOneTimeCraft && (surveyDef.surveyedAtStart || this.oneTimeCompletedCrafts.Contains(surveyDef.id));
	}

	// Token: 0x06001FCA RID: 8138 RVA: 0x000967C0 File Offset: 0x000949C0
	public bool IsOneTimeCraftCompleted(CraftDefBase craftDef)
	{
		SurveyDef surveyDef = craftDef as SurveyDef;
		if (surveyDef != null)
		{
			return this.IsSurveyCompleted(surveyDef);
		}
		return craftDef != null && craftDef.IsOneTimeCraft() && this.oneTimeCompletedCrafts.Contains(craftDef.id);
	}

	// Token: 0x06001FCB RID: 8139 RVA: 0x00096800 File Offset: 0x00094A00
	public void CompleteOneTimeCraft(CraftDefBase craftDef)
	{
		if (craftDef == null || !craftDef.IsOneTimeCraft())
		{
			return;
		}
		if (this.oneTimeCompletedCrafts.Contains(craftDef.id))
		{
			return;
		}
		this.oneTimeCompletedCrafts.Add(craftDef.id);
		Debug.Log("Knowledge: one time craft completed [" + craftDef.id + "]");
	}

	// Token: 0x06001FCC RID: 8140 RVA: 0x00096858 File Offset: 0x00094A58
	public void UnlockOrgan(ItemType itemType)
	{
		if (!this.unlockedOrgans.Contains(itemType))
		{
			this.unlockedOrgans.Add(itemType);
			Debug.Log(string.Format("Knowledge: unlock organ [{0}]", itemType));
		}
	}

	// Token: 0x06001FCD RID: 8141 RVA: 0x00096889 File Offset: 0x00094A89
	public void UnlockTownBuilding(string buildingId)
	{
		if (!this.unlockedTownBuildings.Contains(buildingId))
		{
			this.unlockedTownBuildings.Add(buildingId);
			Debug.Log("Knowledge: unlock town building [" + buildingId + "]");
		}
	}

	// Token: 0x06001FCE RID: 8142 RVA: 0x000968BA File Offset: 0x00094ABA
	public void LockTownBuilding(string buildingId)
	{
		if (!this.lockedTownBuildings.Contains(buildingId))
		{
			this.lockedTownBuildings.Add(buildingId);
			Debug.Log("Knowledge: locked town building [" + buildingId + "]");
		}
	}

	// Token: 0x06001FCF RID: 8143 RVA: 0x000968EB File Offset: 0x00094AEB
	public void UnlockAlchemyFormula(string id)
	{
		if (!this.unlockedAlchemyFormulas.Contains(id))
		{
			this.unlockedAlchemyFormulas.Add(id);
			Debug.Log("Knowledge: alchemy formula [" + id + "]");
		}
	}

	// Token: 0x06001FD0 RID: 8144 RVA: 0x0009691C File Offset: 0x00094B1C
	public bool IsAlchemyFormulaKnown(AlchemyFormulaDef formula)
	{
		return formula == null || !formula.hiddenAtStart || this.unlockedAlchemyFormulas.Contains(formula.id);
	}

	// Token: 0x06001FD1 RID: 8145 RVA: 0x00096940 File Offset: 0x00094B40
	public void DiscoverAlchemyMix(AlchemyMixDef mixDef)
	{
		if (mixDef == null)
		{
			return;
		}
		if (!this.knownMixCrafts.Contains(mixDef.id))
		{
			this.knownMixCrafts.Add(mixDef.id);
		}
		CraftDef alchemyWorkBenchCraft = mixDef.AlchemyWorkBenchCraft;
		if (alchemyWorkBenchCraft != null)
		{
			this.UnlockCraft(alchemyWorkBenchCraft.id);
		}
		AlchemyFormulaDef formula = mixDef.Formula;
		if (formula != null)
		{
			this.UnlockAlchemyFormula(formula.id);
		}
	}

	// Token: 0x06001FD2 RID: 8146 RVA: 0x000969A1 File Offset: 0x00094BA1
	public void UnlockTutorial(string id)
	{
		if (!this.unlockedTutorials.Contains(id))
		{
			this.unlockedTutorials.Add(id);
		}
	}

	// Token: 0x06001FD3 RID: 8147 RVA: 0x000969BD File Offset: 0x00094BBD
	public void AddViewedTutorial(string id)
	{
		if (this.viewedTutorials == null)
		{
			this.viewedTutorials = new List<string>();
		}
		if (!this.viewedTutorials.Contains(id))
		{
			this.viewedTutorials.Add(id);
			Action<string> onTutorialViewed = KnowledgeSystem.OnTutorialViewed;
			if (onTutorialViewed == null)
			{
				return;
			}
			onTutorialViewed(id);
		}
	}

	// Token: 0x06001FD4 RID: 8148 RVA: 0x000969FC File Offset: 0x00094BFC
	public bool HasViewedTutorials()
	{
		return this.viewedTutorials != null && this.viewedTutorials.Count > 0;
	}

	// Token: 0x06001FD5 RID: 8149 RVA: 0x00096A16 File Offset: 0x00094C16
	public void BlackListCraft(string craftId)
	{
		if (!this.blackListCrafts.Contains(craftId))
		{
			this.blackListCrafts.Add(craftId);
			Debug.Log("Knowledge: craft to black list [" + craftId + "]");
		}
	}

	// Token: 0x06001FD6 RID: 8150 RVA: 0x00096A47 File Offset: 0x00094C47
	public void RemoveCraftFromBlackList(string craftId)
	{
		if (this.blackListCrafts.Contains(craftId))
		{
			this.blackListCrafts.Remove(craftId);
			Debug.Log("Knowledge: craft removed from black list [" + craftId + "]");
		}
	}

	// Token: 0x06001FD7 RID: 8151 RVA: 0x00096A79 File Offset: 0x00094C79
	public void UnlockBuilding(string buildingId)
	{
		if (!this.unlockedBuildings.Contains(buildingId))
		{
			this.unlockedBuildings.Add(buildingId);
			Debug.Log("Knowledge: unlock building [" + buildingId + "]");
		}
	}

	// Token: 0x06001FD8 RID: 8152 RVA: 0x00096AAA File Offset: 0x00094CAA
	public void LockBuilding(string buildingId)
	{
		if (!this.lockedBuildings.Contains(buildingId))
		{
			this.lockedBuildings.Add(buildingId);
			Debug.Log("Knowledge: lock building [" + buildingId + "]");
		}
	}

	// Token: 0x06001FD9 RID: 8153 RVA: 0x00096ADC File Offset: 0x00094CDC
	public void RevealTalentLevelUp(string talentLevelUpId)
	{
		if (this.revealedTalentLevelUps == null)
		{
			this.revealedTalentLevelUps = new List<string>();
		}
		if (!this.revealedTalentLevelUps.Contains(talentLevelUpId))
		{
			this.revealedTalentLevelUps.Add(talentLevelUpId);
		}
		if (this.hiddenTalentLevelUps.Contains(talentLevelUpId))
		{
			this.hiddenTalentLevelUps.Remove(talentLevelUpId);
			Debug.Log("Knowledge: reveal talent levelUp [" + talentLevelUpId + "]");
			Action<string> onTalentLevelUpRevealed = KnowledgeSystem.OnTalentLevelUpRevealed;
			if (onTalentLevelUpRevealed == null)
			{
				return;
			}
			onTalentLevelUpRevealed(talentLevelUpId);
		}
	}

	// Token: 0x06001FDA RID: 8154 RVA: 0x00096B58 File Offset: 0x00094D58
	public string GetZombieName()
	{
		bool flag;
		return this.GetZombieName(out flag);
	}

	// Token: 0x06001FDB RID: 8155 RVA: 0x00096B6D File Offset: 0x00094D6D
	public string GetZombieName(out bool nameRandomed)
	{
		if (this.freeZombieNames.Count > 0)
		{
			nameRandomed = false;
			return this.freeZombieNames.PopRandom<string>();
		}
		nameRandomed = true;
		return string.Format("zombie_name_{0}", global::UnityEngine.Random.Range(1, 41));
	}

	// Token: 0x06001FDC RID: 8156 RVA: 0x00096BA6 File Offset: 0x00094DA6
	public void ReturnZombieName(string zombieName, bool nameRandomed)
	{
		if (nameRandomed || string.IsNullOrEmpty(zombieName))
		{
			return;
		}
		this.freeZombieNames.AddIfNotContains(zombieName);
	}

	// Token: 0x06001FDD RID: 8157 RVA: 0x00096BC0 File Offset: 0x00094DC0
	public bool IsInspirationHidden(string inspirationId)
	{
		return this.hiddenInspirations.Contains(inspirationId);
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x00096BD0 File Offset: 0x00094DD0
	public void TryRevealInspirations()
	{
		for (int i = this.hiddenInspirations.Count - 1; i >= 0; i--)
		{
			InspirationData inspirationData;
			InspirationLevelData inspirationLevelData;
			if (TalentSystemCache.Instance.inspirations.TryGetValue(this.hiddenInspirations[i], out inspirationData) && GameBalance.Me.inspirationLevelsCache.TryGetValue(this.hiddenInspirations[i], out inspirationLevelData))
			{
				bool flag = true;
				using (List<string>.Enumerator enumerator = inspirationLevelData.inspirationLocks.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string inpsirationLock = enumerator.Current;
						int num = inpsirationLock.LastIndexOf('_');
						if (num == -1)
						{
							num = inpsirationLock.Length;
						}
						InspirationData inspirationData2;
						if (TalentSystemCache.Instance.inspirations.TryGetValue(inpsirationLock.Substring(0, num), out inspirationData2) && inspirationData2.PurchasedInspirations.Find((InspirationDef x) => x.id == inpsirationLock) == null)
						{
							flag = false;
							break;
						}
					}
				}
				foreach (string text in inspirationLevelData.techLocks)
				{
					if (!this.unlockedTechs.Contains(text))
					{
						flag = false;
						break;
					}
				}
				foreach (string text2 in inspirationLevelData.questLocks)
				{
					if (!MainGame.Instance.GameSave.questSystemData.IsQuestInStatus(text2, QuestStatus.Completed))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.RevealInspiration(inspirationData);
				}
			}
		}
	}

	// Token: 0x06001FDF RID: 8159 RVA: 0x00096DAC File Offset: 0x00094FAC
	public void RevealInspiration(string inspirationId)
	{
		if (!this.hiddenInspirations.Contains(inspirationId))
		{
			return;
		}
		InspirationData inspirationData;
		if (TalentSystemCache.Instance.inspirations.TryGetValue(inspirationId, out inspirationData))
		{
			this.RevealInspiration(inspirationData);
		}
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x00096DE4 File Offset: 0x00094FE4
	private void RevealInspiration(InspirationData inspirationData)
	{
		Debug.Log("Inspiration: reveal inspiration [" + inspirationData.id + "]");
		this.hiddenInspirations.Remove(inspirationData.id);
		MainGame.Instance.GameSave.talentSystemData.CompleteInspiration(inspirationData);
	}

	// Token: 0x04001C73 RID: 7283
	public const int ZOMBIE_NAMES_COUNT = 40;

	// Token: 0x04001C74 RID: 7284
	public static Action<string, bool> OnTechUnlocked;

	// Token: 0x04001C75 RID: 7285
	public static Action<string> OnTechRevealed;

	// Token: 0x04001C76 RID: 7286
	public static Action<string> OnTalentLevelUpRevealed;

	// Token: 0x04001C77 RID: 7287
	public static Action<string> OnTutorialViewed;

	// Token: 0x04001C78 RID: 7288
	public List<string> unlockedPhrases = new List<string>();

	// Token: 0x04001C79 RID: 7289
	public List<string> blackListPhrases = new List<string>();

	// Token: 0x04001C7A RID: 7290
	public List<string> unlockedTechs = new List<string>();

	// Token: 0x04001C7B RID: 7291
	public List<string> delayedDemoTechUnlocks = new List<string>();

	// Token: 0x04001C7C RID: 7292
	public List<string> hiddenTechs = new List<string>();

	// Token: 0x04001C7D RID: 7293
	public List<string> revealedTechs;

	// Token: 0x04001C7E RID: 7294
	public List<string> unlockedCrafts = new List<string>();

	// Token: 0x04001C7F RID: 7295
	public List<string> unlockedTownBuildings = new List<string>();

	// Token: 0x04001C80 RID: 7296
	public List<string> lockedTownBuildings = new List<string>();

	// Token: 0x04001C81 RID: 7297
	public List<string> oneTimeCompletedCrafts = new List<string>();

	// Token: 0x04001C82 RID: 7298
	public List<string> blackListCrafts = new List<string>();

	// Token: 0x04001C83 RID: 7299
	public List<string> unlockedBuildings = new List<string>();

	// Token: 0x04001C84 RID: 7300
	public List<string> lockedBuildings = new List<string>();

	// Token: 0x04001C85 RID: 7301
	public List<CharacterWindowData.CharPage> lockedCharacterWindowTabs = new List<CharacterWindowData.CharPage>();

	// Token: 0x04001C86 RID: 7302
	public List<TechTreeTab> lockedTechTabs = new List<TechTreeTab>();

	// Token: 0x04001C87 RID: 7303
	public List<string> unlockedTutorials = new List<string>();

	// Token: 0x04001C88 RID: 7304
	public List<string> viewedTutorials = new List<string>();

	// Token: 0x04001C89 RID: 7305
	public List<string> hiddenAlchemyFormulas = new List<string>();

	// Token: 0x04001C8A RID: 7306
	public List<string> unlockedAlchemyFormulas = new List<string>();

	// Token: 0x04001C8B RID: 7307
	public List<string> knownMixCrafts = new List<string>();

	// Token: 0x04001C8C RID: 7308
	public List<string> hiddenTalentLevelUps = new List<string>();

	// Token: 0x04001C8D RID: 7309
	public List<string> revealedTalentLevelUps;

	// Token: 0x04001C8E RID: 7310
	public List<string> unknownTalentLevelUps = new List<string>();

	// Token: 0x04001C8F RID: 7311
	public List<string> unlockedTalentIds = new List<string>();

	// Token: 0x04001C90 RID: 7312
	public List<string> freeZombieNames = new List<string>();

	// Token: 0x04001C91 RID: 7313
	public List<ItemType> unlockedOrgans = new List<ItemType>();

	// Token: 0x04001C92 RID: 7314
	public List<string> hiddenInspirations = new List<string>();

	// Token: 0x04001C93 RID: 7315
	public List<string> knownMapZones = new List<string>();

	// Token: 0x04001C94 RID: 7316
	public List<string> visitedWorldZones = new List<string>();

	// Token: 0x04001C95 RID: 7317
	public List<string> activeMapFightIcons = new List<string>();

	// Token: 0x04001C96 RID: 7318
	public List<string> unlockedVendorsForOrders = new List<string>();

	// Token: 0x04001C97 RID: 7319
	public List<string> unlockedCustomHudDaySprites = new List<string>();
}
