using System;
using System.Runtime.CompilerServices;

// Token: 0x020003C0 RID: 960
public class ToolComponent
{
	// Token: 0x14000040 RID: 64
	// (add) Token: 0x060019B9 RID: 6585 RVA: 0x00079374 File Offset: 0x00077574
	// (remove) Token: 0x060019BA RID: 6586 RVA: 0x000793AC File Offset: 0x000775AC
	public event Action OnInteractionStop;

	// Token: 0x1700046C RID: 1132
	// (get) Token: 0x060019BB RID: 6587 RVA: 0x000793E1 File Offset: 0x000775E1
	public ToolComponent.ToolUseStatus UseStatus
	{
		get
		{
			return this.toolUseStatus;
		}
	}

	// Token: 0x1700046D RID: 1133
	// (get) Token: 0x060019BC RID: 6588 RVA: 0x000793E9 File Offset: 0x000775E9
	public Item ToolInUse
	{
		get
		{
			return this.toolInUse;
		}
	}

	// Token: 0x1700046E RID: 1134
	// (get) Token: 0x060019BD RID: 6589 RVA: 0x000793F1 File Offset: 0x000775F1
	public bool IsControlTakenByAnimation
	{
		get
		{
			return this.isControlTakenByAnimation;
		}
	}

	// Token: 0x1700046F RID: 1135
	// (get) Token: 0x060019BE RID: 6590 RVA: 0x000793F9 File Offset: 0x000775F9
	public IWorkActivity ToolActor
	{
		get
		{
			return this.toolActor;
		}
	}

	// Token: 0x17000470 RID: 1136
	// (get) Token: 0x060019BF RID: 6591 RVA: 0x00079401 File Offset: 0x00077601
	public bool IsActionActive
	{
		get
		{
			return this.isActionActive;
		}
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x00079409 File Offset: 0x00077609
	public ToolComponent(AnimationComponentBase animation)
	{
		this.animation = animation;
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x00079418 File Offset: 0x00077618
	public bool TryStartInteraction(IWorkActivity toolActor, Item toolItem, Direction direction)
	{
		this.animation.OnToolLoopStarted += this.HandleAnimationLoopStart;
		this.animation.OnToolLoopFinished += this.HandleAnimationLoopFinish;
		this.toolActor = toolActor;
		this.SubscribeToCraftChanges();
		this.CacheSpendRates(toolItem);
		if (!this.CanProceedWork(toolItem))
		{
			this.StopInteraction();
			return false;
		}
		this.isAnimationDrivenAction = toolItem.Definition.IsAnimationDrivenTool;
		this.isActionActive = true;
		this.toolInUse = toolItem;
		this.animation.SetState(this.isAnimationDrivenAction ? this.GetAnimationStateForTool(this.toolInUse) : AnimationState.WorkHands);
		this.animation.SetDirection(direction);
		return true;
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x000794C8 File Offset: 0x000776C8
	public void TryUpdateToolInUse(Item toolItem)
	{
		if (!this.isActionActive)
		{
			return;
		}
		if (this.toolInUse != toolItem)
		{
			this.CacheSpendRates(toolItem);
			if (!this.CanProceedWork(toolItem))
			{
				this.StopInteraction();
				return;
			}
			this.toolInUse = toolItem;
			this.animation.SetState(this.isAnimationDrivenAction ? this.GetAnimationStateForTool(this.toolInUse) : AnimationState.WorkHands);
		}
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x00079528 File Offset: 0x00077728
	public void UpdateInteraction()
	{
		if (!this.isActionActive)
		{
			return;
		}
		if (!this.CanProceedWork(this.toolInUse))
		{
			this.StopInteraction();
			return;
		}
		if (!this.isAnimationDrivenAction)
		{
			this.ApplyAction();
		}
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x00079556 File Offset: 0x00077756
	public void StopInteraction()
	{
		if (this.isAnimationDrivenAction && this.isControlTakenByAnimation)
		{
			this.animation.OnToolLoopFinished += this.<StopInteraction>g__HandleLastLoopFinished|28_0;
			return;
		}
		this.ResetData();
		Action onInteractionStop = this.OnInteractionStop;
		if (onInteractionStop == null)
		{
			return;
		}
		onInteractionStop();
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x00079596 File Offset: 0x00077796
	public void OnUseToolActionAnimationEvent()
	{
		if (!this.isActionActive)
		{
			return;
		}
		this.ApplyAction();
	}

	// Token: 0x060019C6 RID: 6598 RVA: 0x000795A8 File Offset: 0x000777A8
	private void SubscribeToCraftChanges()
	{
		this.UnsubscribeFromCraftChanges();
		PlayerCraftActivity playerCraftActivity = this.toolActor as PlayerCraftActivity;
		if (playerCraftActivity == null)
		{
			return;
		}
		this.subscribedCraftComponent = playerCraftActivity.CraftComponent;
		this.subscribedCraftComponent.OnCraftStart += this.HandleCurrentCraftChanged;
	}

	// Token: 0x060019C7 RID: 6599 RVA: 0x000795EE File Offset: 0x000777EE
	private void UnsubscribeFromCraftChanges()
	{
		if (this.subscribedCraftComponent == null)
		{
			return;
		}
		this.subscribedCraftComponent.OnCraftStart -= this.HandleCurrentCraftChanged;
		this.subscribedCraftComponent = null;
	}

	// Token: 0x060019C8 RID: 6600 RVA: 0x00079618 File Offset: 0x00077818
	private void HandleCurrentCraftChanged()
	{
		if (!this.isActionActive || this.toolInUse == null || this.toolActor == null)
		{
			return;
		}
		if (this.subscribedCraftComponent != null && this.subscribedCraftComponent.CurrentCraftElement == null && !this.subscribedCraftComponent.IsQueueDelayed)
		{
			return;
		}
		this.CacheSpendRates(this.toolInUse);
	}

	// Token: 0x060019C9 RID: 6601 RVA: 0x0007966D File Offset: 0x0007786D
	private void CacheSpendRates(Item toolItem)
	{
		this.energySpendRate = this.toolActor.GetEnergyCostPerTick(toolItem);
		this.insanitySpendRate = this.toolActor.GetInsanityCostPerTick(toolItem);
	}

	// Token: 0x060019CA RID: 6602 RVA: 0x00079694 File Offset: 0x00077894
	private bool CanProceedWork(Item toolItem)
	{
		if (!this.toolActor.CanUseTool(toolItem))
		{
			this.toolUseStatus = ToolComponent.ToolUseStatus.CantUseTool;
			return false;
		}
		if (!this.toolActor.IsEnoughMastery())
		{
			this.toolUseStatus = ToolComponent.ToolUseStatus.NotEnoughMastery;
			return false;
		}
		if (!this.toolActor.IsEnoughEnergy(toolItem, this.energySpendRate))
		{
			this.toolUseStatus = ToolComponent.ToolUseStatus.OK;
			return false;
		}
		if (!this.toolActor.CanChangeInsanity(toolItem, this.insanitySpendRate))
		{
			this.toolUseStatus = ToolComponent.ToolUseStatus.NotEnoughInsanity;
			return false;
		}
		if (!this.toolActor.IsEnoughDurability(toolItem))
		{
			this.toolUseStatus = ToolComponent.ToolUseStatus.NotEnoughDurability;
			return false;
		}
		this.toolUseStatus = ToolComponent.ToolUseStatus.OK;
		return true;
	}

	// Token: 0x060019CB RID: 6603 RVA: 0x00079727 File Offset: 0x00077927
	private void HandleAnimationLoopStart(ItemType itemType)
	{
		if (this.isAnimationDrivenAction)
		{
			this.isControlTakenByAnimation = true;
		}
	}

	// Token: 0x060019CC RID: 6604 RVA: 0x00079738 File Offset: 0x00077938
	private void HandleAnimationLoopFinish(ItemType itemType)
	{
		if (this.isAnimationDrivenAction)
		{
			this.isControlTakenByAnimation = false;
		}
	}

	// Token: 0x060019CD RID: 6605 RVA: 0x0007974C File Offset: 0x0007794C
	private void ResetData()
	{
		this.animation.OnToolLoopStarted -= this.HandleAnimationLoopStart;
		this.animation.OnToolLoopFinished -= this.HandleAnimationLoopFinish;
		this.animation.SetState(AnimationState.Idle);
		this.isActionActive = false;
		this.toolInUse = null;
		this.energySpendRate = 0f;
		this.insanitySpendRate = 0f;
		this.UnsubscribeFromCraftChanges();
	}

	// Token: 0x060019CE RID: 6606 RVA: 0x000797BD File Offset: 0x000779BD
	private void ApplyAction()
	{
		this.toolActor.ConsumeEnergy(this.toolInUse, this.energySpendRate);
		this.toolActor.ChangeInsanity(this.toolInUse, this.insanitySpendRate);
		this.UseTool();
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x000797F4 File Offset: 0x000779F4
	private void UseTool()
	{
		this.toolActor.UseTool(this.ToolInUse, 1);
		DurabilitySerializedItemProperty durabilitySerializedItemProperty;
		if (this.toolInUse.TryGetProperty<DurabilitySerializedItemProperty>(out durabilitySerializedItemProperty))
		{
			durabilitySerializedItemProperty.Durability -= this.toolInUse.Definition.durDecreaseOnUse;
			if (durabilitySerializedItemProperty.Durability.EqualsTo(0f, 1E-05f))
			{
				this.BreakTool(this.toolInUse);
			}
		}
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x00002318 File Offset: 0x00000518
	private void BreakTool(Item toolItem)
	{
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x00079862 File Offset: 0x00077A62
	private AnimationState GetAnimationStateForTool(Item tool)
	{
		return (AnimationState)(tool.Definition.type + 19);
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x00079872 File Offset: 0x00077A72
	[CompilerGenerated]
	private void <StopInteraction>g__HandleLastLoopFinished|28_0(ItemType itemType)
	{
		this.animation.OnToolLoopFinished -= this.<StopInteraction>g__HandleLastLoopFinished|28_0;
		this.ResetData();
		Action onInteractionStop = this.OnInteractionStop;
		if (onInteractionStop == null)
		{
			return;
		}
		onInteractionStop();
	}

	// Token: 0x040018FE RID: 6398
	private ToolComponent.ToolUseStatus toolUseStatus;

	// Token: 0x040018FF RID: 6399
	private Item toolInUse;

	// Token: 0x04001900 RID: 6400
	private AnimationComponentBase animation;

	// Token: 0x04001901 RID: 6401
	private IWorkActivity toolActor;

	// Token: 0x04001902 RID: 6402
	private float energySpendRate;

	// Token: 0x04001903 RID: 6403
	private float insanitySpendRate;

	// Token: 0x04001904 RID: 6404
	private CraftComponent subscribedCraftComponent;

	// Token: 0x04001905 RID: 6405
	private bool isAnimationDrivenAction;

	// Token: 0x04001906 RID: 6406
	private bool isControlTakenByAnimation;

	// Token: 0x04001907 RID: 6407
	private bool isActionActive;

	// Token: 0x020003C1 RID: 961
	public enum ToolUseStatus
	{
		// Token: 0x04001909 RID: 6409
		None,
		// Token: 0x0400190A RID: 6410
		OK,
		// Token: 0x0400190B RID: 6411
		NotEnoughEnergy = 1,
		// Token: 0x0400190C RID: 6412
		NotEnoughInsanity,
		// Token: 0x0400190D RID: 6413
		NotEnoughDurability,
		// Token: 0x0400190E RID: 6414
		NeedAppropriateToolType,
		// Token: 0x0400190F RID: 6415
		CantUseTool,
		// Token: 0x04001910 RID: 6416
		NotEnoughMastery
	}
}
