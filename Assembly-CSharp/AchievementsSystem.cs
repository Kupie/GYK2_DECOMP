using System;
using System.Collections.Generic;
using LazyBearTechnology;
using Steamworks;
using UnityEngine;

// Token: 0x02000429 RID: 1065
[Serializable]
public class AchievementsSystem
{
	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x06001C1C RID: 7196 RVA: 0x00082EE4 File Offset: 0x000810E4
	public static AchievementsSystem Instance
	{
		get
		{
			return MainGame.Instance.GameSave.achievementsSystem;
		}
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x00082EF8 File Offset: 0x000810F8
	public void PrepareForGame()
	{
		this.cacheById = new Dictionary<string, AchievementsSystem.AchievementData>();
		foreach (AchievementsSystem.AchievementData achievementData in this.achievementDatas)
		{
			if (!this.cacheById.ContainsKey(achievementData.id))
			{
				this.cacheById.Add(achievementData.id, achievementData);
			}
			else
			{
				Debug.LogError("Duplicated achievement data found for id:[" + achievementData.id + "]");
			}
		}
		this.cacheByTrigger = new Dictionary<string, List<AchievementsSystem.AchievementData>>();
		foreach (AchievementDefinition achievementDefinition in GameBalance.Me.achievementDefs)
		{
			if (achievementDefinition.AchievementType == AchievementType.Countable)
			{
				if (string.IsNullOrEmpty(achievementDefinition.countTrigger))
				{
					Debug.LogError("Countable achievement:[" + achievementDefinition.id + "] has empty count trigger.");
				}
				else
				{
					if (!this.cacheByTrigger.ContainsKey(achievementDefinition.countTrigger))
					{
						this.cacheByTrigger.Add(achievementDefinition.countTrigger, new List<AchievementsSystem.AchievementData>());
					}
					this.cacheByTrigger[achievementDefinition.countTrigger].Add(this.GetOrCreateAchievementData(achievementDefinition.id));
				}
			}
		}
		this.VerifyAndTryUnlockAchievement();
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x00083064 File Offset: 0x00081264
	public void Unlock(string achievementId)
	{
		this.EnsureCache();
		AchievementsSystem.AchievementData orCreateAchievementData = this.GetOrCreateAchievementData(achievementId);
		if (!orCreateAchievementData.isCompleted)
		{
			AchievementDefinition definition = orCreateAchievementData.Definition;
			if (definition == null)
			{
				Debug.LogError("No achievement definition found for id:[" + achievementId + "]");
				return;
			}
			if (definition.AchievementType == AchievementType.Countable)
			{
				Debug.LogError("Trying to unlock countable achievement:[" + achievementId + "] with common unlock method.");
				return;
			}
			orCreateAchievementData.isCompleted = true;
			string idForPlatformUnlock = definition.GetIdForPlatformUnlock();
			if (idForPlatformUnlock != "-1")
			{
				this.TryUnlockAchievementOnPlatform(orCreateAchievementData, idForPlatformUnlock);
			}
		}
	}

	// Token: 0x06001C1F RID: 7199 RVA: 0x000830EC File Offset: 0x000812EC
	public void TriggerCountable(string trigger, int countProgress = 1)
	{
		this.EnsureCache();
		List<AchievementsSystem.AchievementData> list;
		if (this.cacheByTrigger.TryGetValue(trigger, out list))
		{
			using (List<AchievementsSystem.AchievementData>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AchievementsSystem.AchievementData achievementData = enumerator.Current;
					if (!achievementData.isCompleted)
					{
						AchievementDefinition definition = achievementData.Definition;
						if (definition == null)
						{
							Debug.LogError("No achievement definition found for id:[" + achievementData.id + "]");
						}
						else
						{
							achievementData.countProgress = Mathf.Clamp(achievementData.countProgress + countProgress, 0, definition.counter);
							string idForPlatformUnlock = definition.GetIdForPlatformUnlock();
							string platformCounterTrigger = definition.GetPlatformCounterTrigger();
							if (platformCounterTrigger != "-1")
							{
								this.TrySetAchievementProgressOnPlatform(achievementData, platformCounterTrigger, achievementData.countProgress, definition.counter);
							}
							if (achievementData.countProgress >= definition.counter)
							{
								achievementData.isCompleted = true;
								if (idForPlatformUnlock != "-1")
								{
									this.TryUnlockAchievementOnPlatform(achievementData, idForPlatformUnlock);
								}
							}
						}
					}
				}
				return;
			}
		}
		Debug.LogError("No achievement data found for trigger:[" + trigger + "]");
	}

	// Token: 0x06001C20 RID: 7200 RVA: 0x00083214 File Offset: 0x00081414
	public bool IsCompleted(string achievementId)
	{
		this.EnsureCache();
		AchievementsSystem.AchievementData achievementData;
		return this.cacheById.TryGetValue(achievementId, out achievementData) && achievementData.isCompleted;
	}

	// Token: 0x06001C21 RID: 7201 RVA: 0x00083240 File Offset: 0x00081440
	public int GetCountProgress(string achievementId)
	{
		this.EnsureCache();
		AchievementsSystem.AchievementData achievementData;
		if (!this.cacheById.TryGetValue(achievementId, out achievementData))
		{
			return 0;
		}
		return achievementData.countProgress;
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x0008326C File Offset: 0x0008146C
	public void VerifyAndTryUnlockAchievement()
	{
		this.EnsureCache();
		foreach (AchievementsSystem.AchievementData achievementData in this.achievementDatas)
		{
			AchievementDefinition definition = achievementData.Definition;
			if (definition == null)
			{
				Debug.LogError("No achievement definition found for id:[" + achievementData.id + "]");
			}
			else
			{
				if (definition.AchievementType == AchievementType.Countable)
				{
					string platformCounterTrigger = definition.GetPlatformCounterTrigger();
					bool flag = achievementData.countProgress >= definition.counter;
					if (achievementData.countProgress > 0 && platformCounterTrigger != "-1" && (!achievementData.isPlatformProgressCalled || (flag && !achievementData.isPlatformUnlockCalled)))
					{
						this.TrySetAchievementProgressOnPlatform(achievementData, platformCounterTrigger, Mathf.Clamp(achievementData.countProgress, 0, definition.counter), definition.counter);
					}
					if (flag)
					{
						achievementData.isCompleted = true;
					}
				}
				if (achievementData.isCompleted && !achievementData.isPlatformUnlockCalled)
				{
					string idForPlatformUnlock = definition.GetIdForPlatformUnlock();
					if (!(idForPlatformUnlock == "-1"))
					{
						this.TryUnlockAchievementOnPlatform(achievementData, idForPlatformUnlock);
					}
				}
			}
		}
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x000833A0 File Offset: 0x000815A0
	private AchievementsSystem.AchievementData GetOrCreateAchievementData(string id)
	{
		AchievementsSystem.AchievementData achievementData;
		if (!this.cacheById.TryGetValue(id, out achievementData))
		{
			achievementData = new AchievementsSystem.AchievementData(id);
			this.achievementDatas.Add(achievementData);
			this.cacheById.Add(id, achievementData);
		}
		return achievementData;
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x000833DE File Offset: 0x000815DE
	private void EnsureCache()
	{
		if (this.cacheById == null || this.cacheByTrigger == null)
		{
			this.PrepareForGame();
		}
	}

	// Token: 0x06001C25 RID: 7205 RVA: 0x000833F6 File Offset: 0x000815F6
	private void TrySetAchievementProgressOnPlatform(AchievementsSystem.AchievementData achievementData, string statId, int progress, int maxProgress)
	{
		LazyAPI.Platform.SetAchievementProgress(statId, progress, maxProgress);
		achievementData.isPlatformProgressCalled = true;
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x0008340D File Offset: 0x0008160D
	private void TryUnlockAchievementOnPlatform(AchievementsSystem.AchievementData achievementData, string platformId)
	{
		LazyAPI.Platform.UnlockAchievement(platformId);
		achievementData.isPlatformUnlockCalled = true;
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x00083424 File Offset: 0x00081624
	public void VerifyAndSetMissedAchievements()
	{
		this.EnsureCache();
		if (SteamManager.Initialized)
		{
			foreach (AchievementsSystem.AchievementData achievementData in this.achievementDatas)
			{
				AchievementDefinition definition = achievementData.Definition;
				if (definition == null)
				{
					Debug.LogError("No achievement definition found for id:[" + achievementData.id + "]");
				}
				else if (achievementData.isCompleted || (definition.AchievementType == AchievementType.Countable && achievementData.countProgress >= definition.counter))
				{
					achievementData.isCompleted = true;
					string idForPlatformUnlock = definition.GetIdForPlatformUnlock();
					if (!(idForPlatformUnlock == "-1"))
					{
						bool flag;
						SteamUserStats.GetAchievement(idForPlatformUnlock, out flag);
						Debug.Log(string.Format("#MSA# Ach:[{0}], counter:[{1}], achieved:[{2}]", idForPlatformUnlock, definition.counter, flag));
						if (!flag)
						{
							this.TryUnlockAchievementOnPlatform(achievementData, idForPlatformUnlock);
						}
					}
				}
			}
		}
	}

	// Token: 0x04001A97 RID: 6807
	public const string FIRST_TOWN_ORDER_ID = "ach_first_town_order";

	// Token: 0x04001A98 RID: 6808
	public const string GRAVEYARD_QUALITY_200_ID = "ach_graveyard_quality_200";

	// Token: 0x04001A99 RID: 6809
	public const string GRAVEYARD_QUALITY_200_ZONE_ID = "graveyard";

	// Token: 0x04001A9A RID: 6810
	public const int GRAVEYARD_QUALITY_200_THRESHOLD = 200;

	// Token: 0x04001A9B RID: 6811
	public const string ORDER_DONE_TRIGGER = "order_done";

	// Token: 0x04001A9C RID: 6812
	public const string SERMON_DONE_TRIGGER = "sermon_done";

	// Token: 0x04001A9D RID: 6813
	public const string INSPIRATION_UNLOCKED_TRIGGER = "inspiration_unlocked";

	// Token: 0x04001A9E RID: 6814
	public const string FISH_CAUGHT_TRIGGER = "fish_caught";

	// Token: 0x04001A9F RID: 6815
	[SerializeField]
	private List<AchievementsSystem.AchievementData> achievementDatas = new List<AchievementsSystem.AchievementData>();

	// Token: 0x04001AA0 RID: 6816
	private Dictionary<string, AchievementsSystem.AchievementData> cacheById;

	// Token: 0x04001AA1 RID: 6817
	private Dictionary<string, List<AchievementsSystem.AchievementData>> cacheByTrigger;

	// Token: 0x0200042A RID: 1066
	[Serializable]
	private class AchievementData : ObjectLinkedToDefinition<AchievementDefinition>
	{
		// Token: 0x06001C29 RID: 7209 RVA: 0x0008353B File Offset: 0x0008173B
		public AchievementData(string id)
			: base(id)
		{
		}

		// Token: 0x04001AA2 RID: 6818
		public int countProgress;

		// Token: 0x04001AA3 RID: 6819
		public bool isCompleted;

		// Token: 0x04001AA4 RID: 6820
		public bool isPlatformProgressCalled;

		// Token: 0x04001AA5 RID: 6821
		public bool isPlatformUnlockCalled;
	}
}
