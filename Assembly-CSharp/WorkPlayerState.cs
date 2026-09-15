using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020003B3 RID: 947
public class WorkPlayerState : SSMState
{
	// Token: 0x06001988 RID: 6536 RVA: 0x00078B17 File Offset: 0x00076D17
	public WorkPlayerState(PlayerController playerController)
		: base(playerController)
	{
		this.playerWorkComponent = playerController.PlayerWorkComponent;
		this.playerInputHandler = playerController.PlayerInputHandler;
		this.updateInteractionHandler = new WorkPlayerState.FixedUpdateInteractionHandler(this.UpdateInteraction);
	}

	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x06001989 RID: 6537 RVA: 0x00078B55 File Offset: 0x00076D55
	public override bool CanEnter
	{
		get
		{
			return this.IsActive && this.CanWork();
		}
	}

	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x0600198A RID: 6538 RVA: 0x00078B67 File Offset: 0x00076D67
	public override bool IsActive
	{
		get
		{
			return LazySingleton<FightingGameController>.Instance.CurrentFightState != FightState.ActiveFight && (LazyInput.GetKey(GameKey.Action) || this.playerWorkComponent.WorkIsTookControl) && this.playerController.IsControlsEnabled;
		}
	}

	// Token: 0x0600198B RID: 6539 RVA: 0x00078B9C File Offset: 0x00076D9C
	public override void FixedUpdate()
	{
		this.fixedUpdateInteractionHandler(Time.fixedDeltaTime);
	}

	// Token: 0x0600198C RID: 6540 RVA: 0x00078BB0 File Offset: 0x00076DB0
	public override void Update()
	{
		base.Update();
		bool flag = false;
		if (this.playerWorkComponent.CanUpdateMagnetismDelayTimer)
		{
			this.playerWorkComponent.UpdateMagnetismDelay(Time.deltaTime);
			flag = true;
		}
		if (this.IsActive && !MainGame.IsGamePaused)
		{
			if (this.targetWgo != this.playerWorkComponent.Wgo && this.playerWorkComponent.Wgo != null)
			{
				if (this.targetWgo && !this.targetWgo.IsDespawning)
				{
					this.targetWgo.InteractionHandler.OnInteractionTargetExit();
				}
				this.targetWgo = this.playerWorkComponent.Wgo;
				this.targetWgo.InteractionHandler.OnInteractionTargetEnter(this.playerController);
			}
			this.fixedUpdateInteractionHandler = ((this.playerWorkComponent.IsActive && ((this.playerWorkComponent.WorkInProgress && this.playerWorkComponent.ToolComponent.IsActionActive) || this.playerWorkComponent.TryStartInteraction())) ? this.updateInteractionHandler : WorkPlayerState.skipInteractionHandler);
		}
		else
		{
			this.fixedUpdateInteractionHandler = WorkPlayerState.skipInteractionHandler;
			if (this.playerWorkComponent.WorkInProgress || this.playerWorkComponent.IsMoving)
			{
				this.playerWorkComponent.StopInteraction();
			}
		}
		if (this.IsActive && !flag)
		{
			this.playerInputHandler.UpdateHotBarInteraction();
		}
	}

	// Token: 0x0600198D RID: 6541 RVA: 0x00078D0C File Offset: 0x00076F0C
	public override void OnEnter()
	{
		this.fixedUpdateInteractionHandler = WorkPlayerState.skipInteractionHandler;
		PlayerData playerData = MainGame.PlayerData;
		if (playerData.HasOverheadItem && !playerData.HasMultipleOverheadItems)
		{
			playerData.DropOverheadItem();
		}
		this.underInteractionWgo = this.playerController.PlayerInteractionComponent.WgoUnderInteraction;
		if (this.playerWorkComponent.Wgo == null)
		{
			PlayerWorkComponent playerWorkComponent = this.playerWorkComponent;
			object obj;
			if (!this.underInteractionWgo)
			{
				obj = null;
			}
			else
			{
				(obj = new List<Wgo>()).Add(this.underInteractionWgo);
			}
			playerWorkComponent.FindWgoToWork(obj);
		}
		this.playerController.PlayerInteractionComponent.SetPauseState(PlayerInteractionPauseType.ByWork, true);
		if (this.playerWorkComponent.Wgo)
		{
			this.targetWgo = this.playerWorkComponent.Wgo;
			this.targetWgo.InteractionHandler.OnInteractionTargetEnter(this.playerController);
		}
	}

	// Token: 0x0600198E RID: 6542 RVA: 0x00078DE0 File Offset: 0x00076FE0
	public override void OnExit()
	{
		this.fixedUpdateInteractionHandler = WorkPlayerState.skipInteractionHandler;
		this.playerWorkComponent.StopInteraction();
		if (this.targetWgo)
		{
			if (this.playerController.PlayerInteractionComponent.WgoUnderInteraction != this.targetWgo)
			{
				this.targetWgo.InteractionHandler.OnInteractionTargetExit();
			}
			this.targetWgo = null;
		}
		this.playerController.PlayerInteractionComponent.SetPauseState(PlayerInteractionPauseType.ByWork, false);
		this.underInteractionWgo = null;
		this.playerWorkComponent.SetMagnetismDelayState(false);
	}

	// Token: 0x0600198F RID: 6543 RVA: 0x00078E6C File Offset: 0x0007706C
	private bool CanWork()
	{
		Wgo wgoUnderInteraction = this.playerController.PlayerInteractionComponent.WgoUnderInteraction;
		PlayerWorkComponent playerWorkComponent = this.playerWorkComponent;
		object obj;
		if (!wgoUnderInteraction)
		{
			obj = null;
		}
		else
		{
			(obj = new List<Wgo>()).Add(wgoUnderInteraction);
		}
		Wgo wgo = playerWorkComponent.FindWgoToWorkNoAssign(obj);
		return (!(this.playerController.PlayerInteractionComponent.BigDropUnderInteraction != null) || !this.playerController.PlayerInteractionComponent.BigDropUnderInteraction.InteractionHandler.HasInteraction2()) && !this.playerController.PlayerData.HasMultipleOverheadItems && (!this.playerController.PlayerData.HasOverheadItem || !this.playerController.PlayerData.overheadItem.Definition.itemGroupIds.Contains("zombie")) && ((this.targetWgo != null && this.targetWgo.Data.Worker is PlayerController) || ((!(wgoUnderInteraction != null) || !(wgoUnderInteraction != wgo)) && wgo != null));
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x00078F78 File Offset: 0x00077178
	private void UpdateInteraction(float deltaTime)
	{
		this.playerWorkComponent.UpdateInteraction(deltaTime);
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x00002318 File Offset: 0x00000518
	private static void SkipInteractionUpdate(float deltaTime)
	{
	}

	// Token: 0x040018CF RID: 6351
	private static readonly WorkPlayerState.FixedUpdateInteractionHandler skipInteractionHandler = new WorkPlayerState.FixedUpdateInteractionHandler(WorkPlayerState.SkipInteractionUpdate);

	// Token: 0x040018D0 RID: 6352
	private readonly WorkPlayerState.FixedUpdateInteractionHandler updateInteractionHandler;

	// Token: 0x040018D1 RID: 6353
	private WorkPlayerState.FixedUpdateInteractionHandler fixedUpdateInteractionHandler = WorkPlayerState.skipInteractionHandler;

	// Token: 0x040018D2 RID: 6354
	private readonly PlayerWorkComponent playerWorkComponent;

	// Token: 0x040018D3 RID: 6355
	private Wgo targetWgo;

	// Token: 0x040018D4 RID: 6356
	private Wgo underInteractionWgo;

	// Token: 0x040018D5 RID: 6357
	private PlayerInputHandler playerInputHandler;

	// Token: 0x020003B4 RID: 948
	// (Invoke) Token: 0x06001994 RID: 6548
	private delegate void FixedUpdateInteractionHandler(float deltaTime);
}
