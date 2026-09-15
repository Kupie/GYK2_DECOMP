using System;

// Token: 0x020003EF RID: 1007
[Serializable]
public class ParentWorkStatusCondition : ConditionalDrawerConditionBase
{
	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x06001A76 RID: 6774 RVA: 0x0007B948 File Offset: 0x00079B48
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.ParentWorkCondition;
		}
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x0007B94F File Offset: 0x00079B4F
	private bool IsCraftStarted(WgoData wgoData)
	{
		CraftComponent craftComponent = wgoData.CraftComponent;
		return craftComponent != null && craftComponent.Status == CraftComponentStatus.Started;
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x0007B965 File Offset: 0x00079B65
	private bool HasWorker(WgoData wgoData)
	{
		return wgoData.Worker != null && !wgoData.Worker.Id.IsEmpty;
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x0007B984 File Offset: 0x00079B84
	private bool IsInWork(WgoData wgoData)
	{
		if (!this.HasWorker(wgoData))
		{
			return false;
		}
		if (!this.IsCraftStarted(wgoData))
		{
			return false;
		}
		ZombieWgoData zombieWgoData = wgoData.Worker as ZombieWgoData;
		if (zombieWgoData != null)
		{
			ZombieCraftActivity zombieCraftActivity = zombieWgoData.WorkerActivity as ZombieCraftActivity;
			if (zombieCraftActivity != null)
			{
				return zombieCraftActivity.IsActive;
			}
		}
		PlayerController playerController = wgoData.Worker as PlayerController;
		return playerController != null && (playerController.PlayerWorkComponent != null && playerController.PlayerWorkComponent.Wgo.Data == wgoData && playerController.PlayerWorkComponent.WorkInProgress) && playerController.PlayerWorkComponent.ToolComponent.IsActionActive;
	}

	// Token: 0x06001A7A RID: 6778 RVA: 0x0007BA20 File Offset: 0x00079C20
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		foreach (SGuid sguid in context.WgoData.WorkbenchParents)
		{
			WgoData wgoData = MainGame.Instance.GameSave.WorldData.GetWgoData(sguid);
			if (wgoData != null && wgoData.CraftComponent != null)
			{
				return this.IsInWork(wgoData);
			}
		}
		return false;
	}
}
