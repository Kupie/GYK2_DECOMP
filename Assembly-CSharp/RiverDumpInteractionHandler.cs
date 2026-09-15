using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200065C RID: 1628
public class RiverDumpInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B1E RID: 11038 RVA: 0x000CC77C File Offset: 0x000CA97C
	public override IWGOInteractionHandler Init(Wgo wgo)
	{
		this.receiver = wgo.GetComponentInChildren<RiverBodyReceiver>(true);
		if (this.receiver == null)
		{
			Debug.LogError("[RiverDumpInteractionHandler] RiverBodyReceiver not found on [" + wgo.name + "]");
		}
		return base.Init(wgo);
	}

	// Token: 0x06002B1F RID: 11039 RVA: 0x000CC7BA File Offset: 0x000CA9BA
	public override void OnInteractionTargetEnter(PlayerController interactor)
	{
		RiverBodyReceiver riverBodyReceiver = this.receiver;
		if (riverBodyReceiver != null)
		{
			riverBodyReceiver.SetDynamicBubbleEnabled(true);
		}
		base.OnInteractionTargetEnter(interactor);
	}

	// Token: 0x06002B20 RID: 11040 RVA: 0x000CC7D5 File Offset: 0x000CA9D5
	public override void OnInteractionTargetExit()
	{
		RiverBodyReceiver riverBodyReceiver = this.receiver;
		if (riverBodyReceiver != null)
		{
			riverBodyReceiver.SetDynamicBubbleEnabled(false);
		}
		base.OnInteractionTargetExit();
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x000CC7F0 File Offset: 0x000CA9F0
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		Item item;
		if (!this.TryGetInsertableOverheadBodyItem(out item) || this.receiver == null || !this.receiver.HasFlowSpline)
		{
			return false;
		}
		Vector3 overheadItemWorldPosition = interactor.View.PlayerAnimation.OverheadItemWorldPosition;
		if (!MainGame.Instance.riverDropSystem.BeginDump(item, this.receiver, overheadItemWorldPosition))
		{
			return false;
		}
		MainGame.PlayerData.RemoveOverheadItem(item);
		MainGame.PlayerData.SubRes("cur_bodies_count", 1f);
		if (MainGame.PlayerData.CurrentWorldZoneData != null && MainGame.PlayerData.CurrentWorldZoneData.Definition.id == "morgue")
		{
			GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
		}
		this.assignedWgo.DrawWidgets();
		return true;
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x000CC8C6 File Offset: 0x000CAAC6
	public override bool HasInteraction(PlayerController interactor)
	{
		return base.HasInteraction(interactor) || (this.HasInsertableOverheadBodyItem() && this.receiver != null && this.receiver.HasFlowSpline);
	}

	// Token: 0x06002B23 RID: 11043 RVA: 0x000CC8F8 File Offset: 0x000CAAF8
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		if (this.HasInsertableOverheadBodyItem())
		{
			string text = "hint_drop_body";
			if (LLBase.L(text) == text)
			{
				text = "hint_place_body";
			}
			return new InteractionInfos(new InteractionInfo(base.LocalizeHintWithActionIcon(text, GameKey.Interaction)));
		}
		return new InteractionInfos();
	}

	// Token: 0x06002B24 RID: 11044 RVA: 0x000CC954 File Offset: 0x000CAB54
	private bool HasInsertableOverheadBodyItem()
	{
		Item item;
		return this.TryGetInsertableOverheadBodyItem(out item);
	}

	// Token: 0x06002B25 RID: 11045 RVA: 0x000CC969 File Offset: 0x000CAB69
	private bool TryGetInsertableOverheadBodyItem(out Item overheadItem)
	{
		return MainGame.PlayerData.TryGetOverheadItem((Item item) => !item.HasItemsByItemType(ItemType.Demon) && item.Definition.itemGroupIds.Contains("corpse"), out overheadItem);
	}

	// Token: 0x04002350 RID: 9040
	private RiverBodyReceiver receiver;
}
