using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020005FE RID: 1534
public class ConveyorCellAnimator : ConveyorAnimator
{
	// Token: 0x170006A6 RID: 1702
	// (get) Token: 0x06002954 RID: 10580 RVA: 0x00028294 File Offset: 0x00026494
	public override ConveyorSystemAnimatorType Type
	{
		get
		{
			return ConveyorSystemAnimatorType.Cell;
		}
	}

	// Token: 0x170006A7 RID: 1703
	// (get) Token: 0x06002955 RID: 10581 RVA: 0x000C2DB8 File Offset: 0x000C0FB8
	public Wgo Parent
	{
		get
		{
			Wgo wgo;
			if ((wgo = this.parent) == null)
			{
				wgo = (this.parent = base.GetComponentInParent<Wgo>(true));
			}
			return wgo;
		}
	}

	// Token: 0x06002956 RID: 10582 RVA: 0x000C2DDF File Offset: 0x000C0FDF
	public override void OnOrchestratorRegistered(string globalState, float phase)
	{
		this.SetupAnimatorForOrchestrator();
		this.MarkPlaybackDirty();
	}

	// Token: 0x06002957 RID: 10583 RVA: 0x000C2DED File Offset: 0x000C0FED
	public override void MarkPlaybackDirty()
	{
		this.playbackDirty = true;
	}

	// Token: 0x06002958 RID: 10584 RVA: 0x000C2DF6 File Offset: 0x000C0FF6
	public override void TryRegister()
	{
		if (this.animator == null)
		{
			return;
		}
		if (LazySingleton<ConveyorSystemAnimationOrchestrator>.Instance.TryAddAnimator(this))
		{
			MainGame.Instance.conveyorSystem.OnUpdated += this.UpdateAnimatorValues;
		}
	}

	// Token: 0x06002959 RID: 10585 RVA: 0x000C2E2F File Offset: 0x000C102F
	public override void Unregister()
	{
		if (this.animator == null)
		{
			return;
		}
		this.parent = null;
		LazySingleton<ConveyorSystemAnimationOrchestrator>.Instance.RemoveAnimator(this);
		MainGame.Instance.conveyorSystem.OnUpdated -= this.UpdateAnimatorValues;
	}

	// Token: 0x0600295A RID: 10586 RVA: 0x000C2E70 File Offset: 0x000C1070
	public override void CustomUpdate()
	{
		bool flag = this.shouldEnableScaleIn && base.CurrentState == "In";
		bool flag2 = this.shouldEnableScaleOut && base.CurrentState == "Out";
		if (!flag && !flag2)
		{
			this.itemScale = Vector3.one;
		}
		if (this.isItemIdleLocked || base.CurrentState == "Idle")
		{
			this.itemRotation = Vector3.zero;
		}
		base.CustomUpdate();
	}

	// Token: 0x0600295B RID: 10587 RVA: 0x000C2EEF File Offset: 0x000C10EF
	public void UpdateAnimatorValues()
	{
		this.UpdateAnimatorValuesCommonCell();
	}

	// Token: 0x0600295C RID: 10588 RVA: 0x000C2EF7 File Offset: 0x000C10F7
	private void UpdateScaleLayers(ConveyorWgoData current)
	{
		this.UpdateScaleLayersCommonCell(current);
	}

	// Token: 0x0600295D RID: 10589 RVA: 0x000C2F00 File Offset: 0x000C1100
	private void SetLayerWeight(int layerIndex, bool enabled)
	{
		if (layerIndex < 0 || layerIndex >= this.animator.layerCount)
		{
			return;
		}
		this.animator.SetLayerWeight(layerIndex, enabled ? 1f : 0f);
	}

	// Token: 0x0600295E RID: 10590 RVA: 0x000C2F30 File Offset: 0x000C1130
	public override void Play(float phase)
	{
		if (this.animator == null)
		{
			return;
		}
		int stateHash = ConveyorCellAnimator.GetStateHash(base.CurrentState);
		int stateHash2 = ConveyorCellAnimator.GetStateHash(this.isItemIdleLocked ? "Idle" : base.CurrentState);
		float num = ((base.CurrentState == "Idle") ? 0f : phase);
		bool flag = stateHash == ConveyorCellAnimator.idleStateHash;
		object obj = this.playbackDirty || stateHash != this.lastLayer0StateHash || (!flag && !Mathf.Approximately(num, this.lastPlayedPhase));
		bool flag2 = this.playbackDirty || stateHash2 != this.lastLayer1StateHash || !Mathf.Approximately(phase, this.lastPlayedPhase);
		object obj2 = obj;
		if (obj2 != null)
		{
			this.animator.Play(stateHash, 0, num);
		}
		if (flag2)
		{
			this.animator.Play(stateHash2, 1, phase);
		}
		if (obj2 == null && !flag2)
		{
			return;
		}
		this.lastPlayedPhase = phase;
		this.lastLayer0StateHash = stateHash;
		this.lastLayer1StateHash = stateHash2;
		this.playbackDirty = false;
	}

	// Token: 0x0600295F RID: 10591 RVA: 0x000C302B File Offset: 0x000C122B
	public override void UpdateView()
	{
		this.UpdateViewCommonCell();
	}

	// Token: 0x06002960 RID: 10592 RVA: 0x000C3034 File Offset: 0x000C1234
	private void UpdateAnimatorValuesCommonCell()
	{
		ConveyorWgoData conveyorWgoData = this.Parent.Data as ConveyorWgoData;
		if (conveyorWgoData == null)
		{
			return;
		}
		if (this.animator == null)
		{
			return;
		}
		ConveyorMovableItemData inItem = conveyorWgoData.ConveyorComponent.InItem;
		ConveyorMovableItemData outItem = conveyorWgoData.ConveyorComponent.OutItem;
		Direction direction = Direction.Left;
		switch (conveyorWgoData.MainWgoPartData.rotationIndex)
		{
		case 0:
			direction = Direction.Down;
			break;
		case 1:
			direction = Direction.Left;
			break;
		case 2:
			direction = Direction.Up;
			break;
		case 3:
			direction = Direction.Right;
			break;
		}
		Direction direction2 = ((inItem == null) ? direction : inItem.direction);
		Direction direction3 = ((outItem == null) ? direction : outItem.direction);
		this.animator.SetFloat(ConveyorCellAnimator.directionIn, (float)direction2);
		this.animator.SetFloat(ConveyorCellAnimator.directionOut, (float)direction3);
		this.UpdateScaleLayers(conveyorWgoData);
		bool flag = inItem == null && outItem == null && conveyorWgoData.Inventory.Data.Inventory.Count > 0;
		if (this.isItemIdleLocked != flag)
		{
			this.isItemIdleLocked = flag;
			this.MarkPlaybackDirty();
		}
		base.CurrentState = "Out";
	}

	// Token: 0x06002961 RID: 10593 RVA: 0x000C3144 File Offset: 0x000C1344
	public void UpdateDropViewIn()
	{
		ConveyorWgoData conveyorWgoData = this.Parent.Data as ConveyorWgoData;
		if (conveyorWgoData == null)
		{
			return;
		}
		this.Parent.MainWgoPart.UpdateDropViewFromItemData(conveyorWgoData.ConveyorComponent.InItem);
	}

	// Token: 0x06002962 RID: 10594 RVA: 0x000C3184 File Offset: 0x000C1384
	public void UpdateDropViewOut()
	{
		ConveyorWgoData conveyorWgoData = this.Parent.Data as ConveyorWgoData;
		if (conveyorWgoData == null)
		{
			return;
		}
		this.Parent.MainWgoPart.UpdateDropViewFromItemData(conveyorWgoData.ConveyorComponent.OutItem);
	}

	// Token: 0x06002963 RID: 10595 RVA: 0x000C31C1 File Offset: 0x000C13C1
	public void UpdateDropView()
	{
		if (!(this.Parent.Data is ConveyorWgoData) || this.Parent.MainWgoPart == null)
		{
			return;
		}
		this.Parent.MainWgoPart.UpdateDropViewFromInventory(null);
	}

	// Token: 0x06002964 RID: 10596 RVA: 0x000C31FC File Offset: 0x000C13FC
	private void UpdateScaleLayersCommonCell(ConveyorWgoData current)
	{
		this.shouldEnableScaleIn = current.ConveyorComponent.InItem != null && !current.ConveyorComponent.InItem.isCommon;
		this.shouldEnableScaleOut = current.ConveyorComponent.OutItem != null && !current.ConveyorComponent.OutItem.isCommon;
		this.SetLayerWeight(ConveyorCellAnimator.itemScaleInLayerIndex, this.shouldEnableScaleIn);
		this.SetLayerWeight(ConveyorCellAnimator.itemScaleOutLayerIndex, this.shouldEnableScaleOut);
	}

	// Token: 0x06002965 RID: 10597 RVA: 0x000C3280 File Offset: 0x000C1480
	private void UpdateViewCommonCell()
	{
		if (base.CurrentState == "Idle" || this.isItemIdleLocked)
		{
			this.UpdateDropView();
			return;
		}
		if (base.CurrentState == "In")
		{
			this.UpdateDropViewIn();
			return;
		}
		if (base.CurrentState == "Out")
		{
			this.UpdateDropViewOut();
		}
	}

	// Token: 0x06002966 RID: 10598 RVA: 0x000C32E0 File Offset: 0x000C14E0
	private void SetupAnimatorForOrchestrator()
	{
		if (this.animator == null)
		{
			return;
		}
		this.animator.updateMode = AnimatorUpdateMode.Normal;
		this.animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		this.animator.applyRootMotion = false;
		this.animator.speed = 0f;
	}

	// Token: 0x06002967 RID: 10599 RVA: 0x000C3330 File Offset: 0x000C1530
	private static int GetStateHash(string state)
	{
		if (state == "In")
		{
			return ConveyorCellAnimator.inStateHash;
		}
		if (state == "Out")
		{
			return ConveyorCellAnimator.outStateHash;
		}
		return ConveyorCellAnimator.idleStateHash;
	}

	// Token: 0x04002251 RID: 8785
	private static readonly int directionIn = Animator.StringToHash("directionIn");

	// Token: 0x04002252 RID: 8786
	private static readonly int directionOut = Animator.StringToHash("directionOut");

	// Token: 0x04002253 RID: 8787
	private static readonly int idleStateHash = Animator.StringToHash("Idle");

	// Token: 0x04002254 RID: 8788
	private static readonly int inStateHash = Animator.StringToHash("In");

	// Token: 0x04002255 RID: 8789
	private static readonly int outStateHash = Animator.StringToHash("Out");

	// Token: 0x04002256 RID: 8790
	private static int itemScaleInLayerIndex = 2;

	// Token: 0x04002257 RID: 8791
	private static int itemScaleOutLayerIndex = 3;

	// Token: 0x04002258 RID: 8792
	[SerializeField]
	private bool isUndergroundCell;

	// Token: 0x04002259 RID: 8793
	private Wgo parent;

	// Token: 0x0400225A RID: 8794
	private bool shouldEnableScaleIn;

	// Token: 0x0400225B RID: 8795
	private bool shouldEnableScaleOut;

	// Token: 0x0400225C RID: 8796
	private bool playbackDirty = true;

	// Token: 0x0400225D RID: 8797
	private float lastPlayedPhase = -1f;

	// Token: 0x0400225E RID: 8798
	private int lastLayer0StateHash;

	// Token: 0x0400225F RID: 8799
	private int lastLayer1StateHash;
}
