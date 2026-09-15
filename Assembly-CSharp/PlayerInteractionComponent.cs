using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000373 RID: 883
public class PlayerInteractionComponent : MonoBehaviour
{
	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x0600176D RID: 5997 RVA: 0x0006F3D1 File Offset: 0x0006D5D1
	public bool HasWgoUnderInteraction
	{
		get
		{
			return this.hasWgoUnderInteraction;
		}
	}

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x0600176E RID: 5998 RVA: 0x0006F3D9 File Offset: 0x0006D5D9
	public DropView BigDropUnderInteraction
	{
		get
		{
			return this.bigDropUnderInteraction;
		}
	}

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x0600176F RID: 5999 RVA: 0x0006F3E1 File Offset: 0x0006D5E1
	public Wgo WgoUnderInteraction
	{
		get
		{
			return this.wgoUnderInteraction;
		}
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x06001770 RID: 6000 RVA: 0x0006F3E9 File Offset: 0x0006D5E9
	public bool IsPaused
	{
		get
		{
			return !this.pauseMultiFlag.ResultFlag;
		}
	}

	// Token: 0x06001771 RID: 6001 RVA: 0x0006F3FC File Offset: 0x0006D5FC
	public void Init()
	{
		if (this.interactionCollider == null)
		{
			Debug.Log("Incorrect InteractionComponent initialization: cannot find interaction collider");
		}
		this.pauseMultiFlag.Init(null, true);
		this.initialLocalPosition = base.transform.localPosition;
		this.wgoTargets.Capacity = 8;
		GameSceneData.OnWgoDataOnScenePreRemove += this.HandleWgoDataRemoved;
		GameSceneData.OnDropOnScenePreRemoved += this.HandeDropRemove;
		MainGame.PlayerController.OnControlStateChanged += this.OnPlayerControlStateChanged;
		this.HandleWgoInteractionExit(this.wgoUnderInteraction);
	}

	// Token: 0x06001772 RID: 6002 RVA: 0x0006F48F File Offset: 0x0006D68F
	public void SetPlayerData(PlayerData playerData)
	{
		this.playerData = playerData;
	}

	// Token: 0x06001773 RID: 6003 RVA: 0x0006F498 File Offset: 0x0006D698
	public void ResetInteractionState()
	{
		this.DoLeaveWgoInteraction();
		this.DoLeaveDropViewInteraction();
		this.Update();
	}

	// Token: 0x06001774 RID: 6004 RVA: 0x0006F4AC File Offset: 0x0006D6AC
	public void SetPauseState(PlayerInteractionPauseType type, bool isPaused)
	{
		Debug.Log(string.Format("Player SetPauseState:[{0}] isPaused:[{1}]", type, isPaused));
		this.pauseMultiFlag.UpdateFlag(type, !isPaused);
		this.interactionCollider.enabled = !this.IsPaused;
		if (this.IsPaused)
		{
			this.DoLeaveWgoInteraction();
			this.DoLeaveDropViewInteraction();
		}
	}

	// Token: 0x06001775 RID: 6005 RVA: 0x0006F50C File Offset: 0x0006D70C
	public DropView TryGetClosestInteractionTarget()
	{
		if (this.playerData == null)
		{
			return null;
		}
		Array.Clear(this.interactionTargets, 0, 8);
		base.transform.localPosition = Vector3.Scale(this.initialLocalPosition, new Vector3(this.playerData.Direction.x, 1f, this.playerData.Direction.y));
		Physics.OverlapBoxNonAlloc(base.transform.position, this.interactionCollider.bounds.extents, this.interactionTargets, Quaternion.identity, 65600, QueryTriggerInteraction.Collide);
		DropView dropView;
		this.TryGetClosestDropViewFromColliders(this.interactionTargets, out dropView);
		return dropView;
	}

	// Token: 0x06001776 RID: 6006 RVA: 0x0006F5B8 File Offset: 0x0006D7B8
	private void Update()
	{
		if (this.IsPaused || this.playerData == null)
		{
			return;
		}
		DropView dropView = this.TryGetClosestInteractionTarget();
		if (PlayerInteractionComponent.CanInteractWithDrop(dropView) && dropView.Data.Size == ItemSize.Big && this.TryInteractWithDropView(dropView))
		{
			this.DoLeaveWgoInteraction();
			return;
		}
		this.DoLeaveDropViewInteraction();
		this.wgoTargets.Clear();
		this.GetWgoTargetsFromColliders(this.interactionTargets, this.wgoTargets);
		Wgo wgo;
		if (!this.TryFindClosestWgo(this.wgoTargets, out wgo))
		{
			this.DoLeaveWgoInteraction();
			return;
		}
		if (!this.hasWgoUnderInteraction)
		{
			this.DoEnterInteraction(wgo);
			return;
		}
		if (wgo.Data.UniqueId != this.wgoUnderInteraction.Data.UniqueId)
		{
			Debug.Log("Changing interaction wgo");
			this.DoLeaveWgoInteraction();
			this.DoEnterInteraction(wgo);
		}
	}

	// Token: 0x06001777 RID: 6007 RVA: 0x0006F686 File Offset: 0x0006D886
	private void HandleWgoDataRemoved(string gameSceneId, WgoData removedWgo)
	{
		if (this.wgoUnderInteraction == null)
		{
			return;
		}
		if (this.wgoUnderInteraction.Data.UniqueId == removedWgo.UniqueId)
		{
			this.DoLeaveWgoInteraction();
		}
	}

	// Token: 0x06001778 RID: 6008 RVA: 0x0006F6BC File Offset: 0x0006D8BC
	private void HandeDropRemove(string gameSceneId, DropData removedDrop)
	{
		if (this.bigDropUnderInteraction == null)
		{
			return;
		}
		if (this.bigDropUnderInteraction.Data == null || removedDrop == null || this.bigDropUnderInteraction.Data.UniqueId == removedDrop.UniqueId)
		{
			this.DoLeaveDropViewInteraction();
		}
	}

	// Token: 0x06001779 RID: 6009 RVA: 0x0006F70C File Offset: 0x0006D90C
	private void DoEnterInteraction(Wgo wgo)
	{
		if (!MainGame.PlayerController.IsControlsEnabled)
		{
			return;
		}
		if (MainGame.PlayerData.isInTutorialMode && !MainGame.PlayerData.tutorialModeExcludedList.Contains(wgo.Data.UniqueId.Id))
		{
			return;
		}
		this.wgoUnderInteraction = wgo;
		this.hasWgoUnderInteraction = true;
		this.HandleWgoInteractionEnter(this.wgoUnderInteraction);
		Action<Wgo> onInteractionTargetEnter = this.OnInteractionTargetEnter;
		if (onInteractionTargetEnter != null)
		{
			onInteractionTargetEnter(this.wgoUnderInteraction);
		}
		Debug.Log("Interaction enter with [" + this.wgoUnderInteraction.Data.id + "]");
	}

	// Token: 0x0600177A RID: 6010 RVA: 0x0006F7AC File Offset: 0x0006D9AC
	private void DoEnterInteraction(DropView dropView)
	{
		this.bigDropUnderInteraction = dropView;
		this.hasBigDropUnderInteraction = true;
		Action<DropView> onInteractionBigDropTargetEnter = this.OnInteractionBigDropTargetEnter;
		if (onInteractionBigDropTargetEnter != null)
		{
			onInteractionBigDropTargetEnter(dropView);
		}
		this.HandleDropInteractionEnter(dropView);
		Debug.Log("Drop interaction enter with [" + this.bigDropUnderInteraction.Data.Id + "]");
	}

	// Token: 0x0600177B RID: 6011 RVA: 0x0006F804 File Offset: 0x0006DA04
	private void DoLeaveWgoInteraction()
	{
		if (!this.hasWgoUnderInteraction)
		{
			return;
		}
		Debug.Log("Interaction exit from [" + this.wgoUnderInteraction.Data.id + "]");
		Wgo wgo = this.wgoUnderInteraction;
		this.hasWgoUnderInteraction = false;
		this.wgoUnderInteraction = null;
		this.HandleWgoInteractionExit(wgo);
		Action onInteractionTargetExit = this.OnInteractionTargetExit;
		if (onInteractionTargetExit == null)
		{
			return;
		}
		onInteractionTargetExit();
	}

	// Token: 0x0600177C RID: 6012 RVA: 0x0006F86C File Offset: 0x0006DA6C
	private void DoLeaveDropViewInteraction()
	{
		if (!this.hasBigDropUnderInteraction)
		{
			return;
		}
		DropView dropView = this.bigDropUnderInteraction;
		if (((dropView != null) ? dropView.Data : null) != null)
		{
			Debug.Log("Drop interaction exit from [" + this.bigDropUnderInteraction.Data.Id + "]");
		}
		this.HandleDropInteractionExit();
		this.bigDropUnderInteraction = null;
		this.hasBigDropUnderInteraction = false;
		Action onInteractionBigDropTargetExit = this.OnInteractionBigDropTargetExit;
		if (onInteractionBigDropTargetExit == null)
		{
			return;
		}
		onInteractionBigDropTargetExit();
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x0006F8E0 File Offset: 0x0006DAE0
	private bool TryFindClosestWgo(List<Wgo> wgos, out Wgo closestWgo)
	{
		closestWgo = null;
		if (wgos.Count == 0)
		{
			return false;
		}
		closestWgo = wgos[0];
		float num = float.PositiveInfinity;
		for (int i = 0; i < wgos.Count; i++)
		{
			Wgo wgo = wgos[i];
			Debug.DrawLine(base.transform.position + Vector3.up, wgo.Data.Position + Vector3.up, Color.magenta);
			float magnitude = (wgo.Data.Position - base.transform.position).magnitude;
			if (magnitude < num)
			{
				num = magnitude;
				closestWgo = wgo;
			}
		}
		return true;
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x0006F985 File Offset: 0x0006DB85
	private static bool CanInteractWithDrop(DropView dropView)
	{
		return dropView != null && dropView.Data != null && !dropView.IsDespawning && !dropView.IsRiverDump;
	}

	// Token: 0x0600177F RID: 6015 RVA: 0x0006F9AC File Offset: 0x0006DBAC
	private bool TryInteractWithDropView(DropView dropView)
	{
		if (!PlayerInteractionComponent.CanInteractWithDrop(dropView) || dropView.Data.Size != ItemSize.Big)
		{
			return false;
		}
		if (!this.hasBigDropUnderInteraction)
		{
			this.DoEnterInteraction(dropView);
			return true;
		}
		DropView dropView2 = this.bigDropUnderInteraction;
		if (((dropView2 != null) ? dropView2.Data : null) == null || dropView.Data.UniqueId != this.bigDropUnderInteraction.Data.UniqueId)
		{
			Debug.Log("Changing interaction drop");
			this.DoLeaveDropViewInteraction();
			this.DoEnterInteraction(dropView);
		}
		return true;
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x0006FA30 File Offset: 0x0006DC30
	private bool TryGetClosestDropViewFromColliders(Collider[] colliders, out DropView dropView)
	{
		dropView = null;
		float num = float.PositiveInfinity;
		for (int i = 0; i < colliders.Length; i++)
		{
			if (!(colliders[i] == null))
			{
				DropView componentInParent = colliders[i].gameObject.GetComponentInParent<DropView>();
				if (componentInParent != null && PlayerInteractionComponent.CanInteractWithDrop(componentInParent))
				{
					float num2 = Vector3.Distance(componentInParent.Data.Position, base.transform.position);
					if (componentInParent != dropView && num2 < num)
					{
						dropView = componentInParent;
						num = num2;
					}
				}
			}
		}
		return dropView != null;
	}

	// Token: 0x06001781 RID: 6017 RVA: 0x0006FAB8 File Offset: 0x0006DCB8
	private void GetWgoTargetsFromColliders(Collider[] colliders, List<Wgo> wgoTargets)
	{
		foreach (Collider collider in colliders)
		{
			if (!(collider == null))
			{
				Wgo componentInParent = collider.GetComponentInParent<Wgo>();
				if (!(componentInParent == null) && !componentInParent.IsDespawning && !wgoTargets.Contains(componentInParent) && componentInParent.Data.IsInteractable)
				{
					ConveyorCellInteractionHandler conveyorCellInteractionHandler = componentInParent.InteractionHandler as ConveyorCellInteractionHandler;
					if (conveyorCellInteractionHandler == null || conveyorCellInteractionHandler.HasInteraction(MainGame.PlayerController))
					{
						wgoTargets.Add(componentInParent);
					}
				}
			}
		}
	}

	// Token: 0x06001782 RID: 6018 RVA: 0x0006FB31 File Offset: 0x0006DD31
	private void OnDestroy()
	{
		GameSceneData.OnWgoDataOnScenePreRemove -= this.HandleWgoDataRemoved;
		GameSceneData.OnDropOnScenePreRemoved -= this.HandeDropRemove;
	}

	// Token: 0x06001783 RID: 6019 RVA: 0x0006FB55 File Offset: 0x0006DD55
	private void HandleWgoInteractionEnter(Wgo wgo)
	{
		wgo.InteractionHandler.OnInteractionTargetEnter(MainGame.PlayerController);
	}

	// Token: 0x06001784 RID: 6020 RVA: 0x0006FB67 File Offset: 0x0006DD67
	private void HandleWgoInteractionExit(Wgo interactedWgo)
	{
		if (interactedWgo == null)
		{
			return;
		}
		interactedWgo.InteractionHandler.OnInteractionTargetExit();
	}

	// Token: 0x06001785 RID: 6021 RVA: 0x0006FB7E File Offset: 0x0006DD7E
	private void HandleDropInteractionEnter(DropView dropView)
	{
		IInteractionHandler interactionHandler = dropView.InteractionHandler;
		if (interactionHandler != null)
		{
			interactionHandler.OnInteractionTargetEnter();
		}
		dropView.SetInteractionVisualState(true);
	}

	// Token: 0x06001786 RID: 6022 RVA: 0x0006FB98 File Offset: 0x0006DD98
	private void HandleDropInteractionExit()
	{
		if (this.bigDropUnderInteraction == null)
		{
			return;
		}
		this.bigDropUnderInteraction.SetInteractionVisualState(false);
		IInteractionHandler interactionHandler = this.bigDropUnderInteraction.InteractionHandler;
		if (interactionHandler == null)
		{
			return;
		}
		interactionHandler.OnInteractionTargetExit();
	}

	// Token: 0x06001787 RID: 6023 RVA: 0x0006FBCA File Offset: 0x0006DDCA
	private void OnPlayerControlStateChanged()
	{
		if (!MainGame.PlayerController.IsControlsEnabled)
		{
			this.SetPauseState(PlayerInteractionPauseType.ByControl, true);
			this.ResetInteractionState();
		}
		else
		{
			this.SetPauseState(PlayerInteractionPauseType.ByControl, false);
			this.Update();
		}
		GameScene.RedrawWgosWithInteractionEvents();
	}

	// Token: 0x0400174D RID: 5965
	private const int COLLIDERS_LIMIT_COUNT = 8;

	// Token: 0x0400174E RID: 5966
	public Action<Wgo> OnInteractionTargetEnter;

	// Token: 0x0400174F RID: 5967
	public Action<Wgo> OnInteractionTargetChanged;

	// Token: 0x04001750 RID: 5968
	public Action OnInteractionTargetExit;

	// Token: 0x04001751 RID: 5969
	public Action<DropView> OnInteractionBigDropTargetEnter;

	// Token: 0x04001752 RID: 5970
	public Action<DropView> OnInteractionBigDropTargetChanged;

	// Token: 0x04001753 RID: 5971
	public Action OnInteractionBigDropTargetExit;

	// Token: 0x04001754 RID: 5972
	[SerializeField]
	private BoxCollider interactionCollider;

	// Token: 0x04001755 RID: 5973
	private Vector3 initialLocalPosition;

	// Token: 0x04001756 RID: 5974
	private Collider[] interactionTargets = new Collider[8];

	// Token: 0x04001757 RID: 5975
	private List<Wgo> wgoTargets = new List<Wgo>();

	// Token: 0x04001758 RID: 5976
	private Wgo wgoUnderInteraction;

	// Token: 0x04001759 RID: 5977
	private bool hasWgoUnderInteraction;

	// Token: 0x0400175A RID: 5978
	private PlayerData playerData;

	// Token: 0x0400175B RID: 5979
	private DropView bigDropUnderInteraction;

	// Token: 0x0400175C RID: 5980
	private bool hasBigDropUnderInteraction;

	// Token: 0x0400175D RID: 5981
	private MultiFlagAND<PlayerInteractionPauseType> pauseMultiFlag = new MultiFlagAND<PlayerInteractionPauseType>();
}
