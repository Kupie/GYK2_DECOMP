using System;
using LazyBearTechnology;

// Token: 0x02000630 RID: 1584
public class ZombieDropInteractionHandler : BigDropInteractionHandler
{
	// Token: 0x06002A2C RID: 10796 RVA: 0x000C73CC File Offset: 0x000C55CC
	public override InteractionInfos GetInteractionInfos()
	{
		InteractionInfos interactionInfos = new InteractionInfos();
		if (this.HasInteraction())
		{
			interactionInfos.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_take", GameKey.Interaction)));
		}
		if (this.HasInteraction2())
		{
			interactionInfos.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("action_inspect", GameKey.Action)));
		}
		return interactionInfos;
	}

	// Token: 0x06002A2D RID: 10797 RVA: 0x000C7426 File Offset: 0x000C5626
	public override bool HasInteraction()
	{
		return base.HasInteraction();
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x000C742E File Offset: 0x000C562E
	public override bool Interact()
	{
		this.LinkItemToZombieData();
		return base.Interact();
	}

	// Token: 0x06002A2F RID: 10799 RVA: 0x000C743C File Offset: 0x000C563C
	public override bool HasInteraction2()
	{
		return this.TryGetZombie() != null;
	}

	// Token: 0x06002A30 RID: 10800 RVA: 0x000C7448 File Offset: 0x000C5648
	public override bool Interact2()
	{
		DropView drop = this.drop;
		if (((drop != null) ? drop.Data : null) == null || this.TryGetZombie() == null)
		{
			return false;
		}
		this.LinkItemToZombieData();
		UIZombieWorkerWindowData uizombieWorkerWindowData = new UIZombieWorkerWindowData(this.drop.Data);
		LazyUI.GetWindow<UIZombieWorkerWindow>().Open(uizombieWorkerWindowData);
		return true;
	}

	// Token: 0x06002A31 RID: 10801 RVA: 0x000C7496 File Offset: 0x000C5696
	public override void OnInteractionTargetEnter()
	{
		DropView drop = this.drop;
		if (((drop != null) ? drop.Data : null) == null || UIObjectBubbleManager.Instance == null)
		{
			return;
		}
		UIObjectBubbleManager.Instance.Display(this.drop);
	}

	// Token: 0x06002A32 RID: 10802 RVA: 0x000C74CA File Offset: 0x000C56CA
	public override void OnInteractionTargetExit()
	{
		if (this.drop == null || UIObjectBubbleManager.Instance == null)
		{
			return;
		}
		UIObjectBubbleManager.Instance.Hide(this.drop);
	}

	// Token: 0x06002A33 RID: 10803 RVA: 0x000C74F8 File Offset: 0x000C56F8
	private ZombieWgoData TryGetZombie()
	{
		DropView drop = this.drop;
		bool flag;
		if (drop == null)
		{
			flag = null != null;
		}
		else
		{
			DropData data = drop.Data;
			flag = ((data != null) ? data.Item : null) != null;
		}
		if (!flag)
		{
			return null;
		}
		return MainGame.ZombieSystemData.GetZombie(this.drop.Data.Item.UniqueId);
	}

	// Token: 0x06002A34 RID: 10804 RVA: 0x000C7548 File Offset: 0x000C5748
	private void LinkItemToZombieData()
	{
		ZombieWgoData zombieWgoData = this.TryGetZombie();
		if (zombieWgoData != null)
		{
			DropView drop = this.drop;
			bool flag;
			if (drop == null)
			{
				flag = null != null;
			}
			else
			{
				DropData data = drop.Data;
				flag = ((data != null) ? data.Item : null) != null;
			}
			if (flag)
			{
				zombieWgoData.SetZombieItem(this.drop.Data.Item);
				return;
			}
		}
	}
}
