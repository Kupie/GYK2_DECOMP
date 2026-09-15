using System;
using System.Collections.Generic;

// Token: 0x02000270 RID: 624
[Serializable]
public class CraftSystem : ICustomUpdatable
{
	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x0600103E RID: 4158 RVA: 0x00052252 File Offset: 0x00050452
	private static CraftSystemData CraftSystemData
	{
		get
		{
			return MainGame.Instance.GameSave.craftSystemData;
		}
	}

	// Token: 0x0600103F RID: 4159 RVA: 0x00052264 File Offset: 0x00050464
	public void CustomUpdate(float deltaTime)
	{
		List<CraftComponent> activeCrafts = CraftSystem.CraftSystemData.activeCrafts;
		if (activeCrafts != null)
		{
			this.craftsCheckTimer += deltaTime;
			if (this.craftsCheckTimer >= 1f)
			{
				foreach (CraftComponent craftComponent in activeCrafts)
				{
					if (craftComponent.Status != CraftComponentStatus.ReadyToFinishAutoCraft)
					{
						if (craftComponent.IsQueueDelayed || craftComponent.Status == CraftComponentStatus.ReadyToStartCraft || craftComponent.Status == CraftComponentStatus.FinishDelayed)
						{
							craftComponent.UpdateQueueElementsCraftStatus();
						}
						if (!craftComponent.IsAutoCraftable)
						{
							craftComponent.UpdateCanContinueManualCraftState(deltaTime);
						}
					}
				}
				this.craftsCheckTimer = 0f;
			}
			for (int i = 0; i < activeCrafts.Count; i++)
			{
				CraftComponent craftComponent2 = activeCrafts[i];
				if (craftComponent2.IsAutoCraftable && !craftComponent2.HasPreFinishUpdate && !craftComponent2.IsDestroyingCraftActive)
				{
					craftComponent2.Update(deltaTime);
				}
				if (craftComponent2.HasPreFinishUpdate)
				{
					craftComponent2.PreFinishUpdate(deltaTime);
				}
			}
		}
		for (int j = CraftSystem.CraftSystemData.zombieCraftActivities.Count - 1; j >= 0; j--)
		{
			CraftSystem.CraftSystemData.zombieCraftActivities[j].Update(deltaTime);
		}
		for (int k = CraftSystem.CraftSystemData.zombieHPActivities.Count - 1; k >= 0; k--)
		{
			CraftSystem.CraftSystemData.zombieHPActivities[k].Update(deltaTime);
		}
	}

	// Token: 0x06001040 RID: 4160 RVA: 0x000523D8 File Offset: 0x000505D8
	public void AddCraftObject(CraftComponent craftComponent)
	{
		CraftSystemData craftSystemData = CraftSystem.CraftSystemData;
		if (craftSystemData.activeCrafts == null)
		{
			craftSystemData.activeCrafts = new List<CraftComponent>();
		}
		if (CraftSystem.CraftSystemData.activeCrafts.Contains(craftComponent))
		{
			return;
		}
		CraftSystem.CraftSystemData.activeCrafts.Add(craftComponent);
	}

	// Token: 0x06001041 RID: 4161 RVA: 0x00052421 File Offset: 0x00050621
	public void RemoveCraftObject(CraftComponent craftComponent)
	{
		List<CraftComponent> activeCrafts = CraftSystem.CraftSystemData.activeCrafts;
		if (activeCrafts == null)
		{
			return;
		}
		activeCrafts.Remove(craftComponent);
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x0005243C File Offset: 0x0005063C
	public void AddWorker(ZombieCraftActivity craftActivity)
	{
		CraftSystem.CraftSystemData.zombieCraftActivities.RemoveAll((ZombieCraftActivity a) => a.ZombieUniqueId.Guid == craftActivity.ZombieUniqueId.Guid);
		CraftSystem.CraftSystemData.zombieCraftActivities.Add(craftActivity);
	}

	// Token: 0x06001043 RID: 4163 RVA: 0x00052488 File Offset: 0x00050688
	public void RemoveWorker(ZombieCraftActivity craftActivity)
	{
		CraftSystem.CraftSystemData.zombieCraftActivities.RemoveAll((ZombieCraftActivity a) => a.ZombieUniqueId.Guid == craftActivity.ZombieUniqueId.Guid);
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x000524C0 File Offset: 0x000506C0
	public void AddHPWorker(ZombieHPActivity hpActivity)
	{
		CraftSystem.CraftSystemData.zombieHPActivities.RemoveAll((ZombieHPActivity a) => a.ZombieUniqueId.Guid == hpActivity.ZombieUniqueId.Guid);
		CraftSystem.CraftSystemData.zombieHPActivities.Add(hpActivity);
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x0005250B File Offset: 0x0005070B
	public void RemoveHPWorker(ZombieHPActivity hpActivity)
	{
		CraftSystem.CraftSystemData.zombieHPActivities.Remove(hpActivity);
	}

	// Token: 0x06001046 RID: 4166 RVA: 0x00052520 File Offset: 0x00050720
	public ZombieCraftActivity TryGetCraftActivity(ZombieWgoData zombieWgoData)
	{
		foreach (ZombieCraftActivity zombieCraftActivity in CraftSystem.CraftSystemData.zombieCraftActivities)
		{
			if (zombieCraftActivity.ZombieUniqueId.Guid == zombieWgoData.UniqueId.Guid)
			{
				return zombieCraftActivity;
			}
		}
		return null;
	}

	// Token: 0x06001047 RID: 4167 RVA: 0x00052594 File Offset: 0x00050794
	public ZombieHPActivity TryGetHPActivity(ZombieWgoData zombieWgoData)
	{
		foreach (ZombieHPActivity zombieHPActivity in CraftSystem.CraftSystemData.zombieHPActivities)
		{
			if (zombieHPActivity.ZombieUniqueId.Guid == zombieWgoData.UniqueId.Guid)
			{
				return zombieHPActivity;
			}
		}
		return null;
	}

	// Token: 0x040012A2 RID: 4770
	private const float CRAFTS_CHECK_TIME = 1f;

	// Token: 0x040012A3 RID: 4771
	private float craftsCheckTimer;
}
