using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// Token: 0x020003A0 RID: 928
public class PlayerWorkComponent : MonoBehaviour
{
	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x060018F9 RID: 6393 RVA: 0x000765E3 File Offset: 0x000747E3
	public bool WorkIsTookControl
	{
		get
		{
			return this.toolComponent.IsControlTakenByAnimation;
		}
	}

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x060018FA RID: 6394 RVA: 0x000765F0 File Offset: 0x000747F0
	public bool WorkInProgress
	{
		get
		{
			return this.workInProgress;
		}
	}

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x060018FB RID: 6395 RVA: 0x000765F8 File Offset: 0x000747F8
	public bool IsMoving
	{
		get
		{
			return this.playerLocalAreaMovement.IsMovementStarted;
		}
	}

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x060018FC RID: 6396 RVA: 0x00076605 File Offset: 0x00074805
	public bool IsActive
	{
		get
		{
			return this.isActive;
		}
	}

	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x060018FD RID: 6397 RVA: 0x0007660D File Offset: 0x0007480D
	public ToolComponent ToolComponent
	{
		get
		{
			return this.toolComponent;
		}
	}

	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x060018FE RID: 6398 RVA: 0x00076615 File Offset: 0x00074815
	public Wgo Wgo
	{
		get
		{
			return this.currentWgo;
		}
	}

	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x060018FF RID: 6399 RVA: 0x0007661D File Offset: 0x0007481D
	public bool CanUpdateMagnetismDelayTimer
	{
		get
		{
			return this.canUpdateMagnetismDelayTimer;
		}
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x00076625 File Offset: 0x00074825
	public void Init(PlayerController playerController, ToolComponent toolComponent, PlayerLocalAreaMovement playerLocalAreaMovement)
	{
		this.playerLocalAreaMovement = playerLocalAreaMovement;
		this.playerController = playerController;
		this.toolComponent = toolComponent;
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x0007663C File Offset: 0x0007483C
	public bool TryStartInteraction()
	{
		if (this.currentWgo == null)
		{
			Wgo wgoUnderInteraction = this.playerController.PlayerInteractionComponent.WgoUnderInteraction;
			object obj;
			if (!wgoUnderInteraction)
			{
				obj = null;
			}
			else
			{
				(obj = new List<Wgo>()).Add(wgoUnderInteraction);
			}
			this.FindWgoToWork(obj);
		}
		if (this.currentWgo == null)
		{
			return false;
		}
		if (this.playerLocalAreaMovement.IsMovementStarted && !this.IsOnWorkingSpot(this.currentWgo))
		{
			return true;
		}
		this.currentWgoData = this.currentWgo.Data;
		IComponent componentForInteraction = this.GetComponentForInteraction(this.currentWgo);
		if (componentForInteraction == null)
		{
			return false;
		}
		if (componentForInteraction.GetType() == typeof(CraftComponent))
		{
			this.playerController.SetCraftActivity(this.currentWgo.Data);
		}
		else
		{
			this.playerController.SetHPActivity(this.currentWgo.Data);
		}
		this.playerController.WorkerActivity.OnStartActivity();
		Item appropriateToolForWork = this.GetAppropriateToolForWork();
		if (appropriateToolForWork == null || appropriateToolForWork.IsEmpty)
		{
			return false;
		}
		if (this.IsOnWorkingSpot(this.currentWgo))
		{
			if (this.playerLocalAreaMovement.IsMovementStarted)
			{
				this.playerLocalAreaMovement.StopMovement(false);
			}
			this.playerController.SetPosition(this.currentDockPoint.transform.position, true, false);
			this.AlignPlayerToDockPoint();
			return this.toolComponent.TryStartInteraction(this.playerController.WorkerActivity, appropriateToolForWork, this.currentDockPoint.Direction);
		}
		if (this.playerLocalAreaMovement.IsMovementStarted)
		{
			return true;
		}
		this.playerLocalAreaMovement.StartMovement(this.currentDockPoint.transform.position, this.currentDockPoint.Direction);
		return true;
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x000767E8 File Offset: 0x000749E8
	public void UpdateInteraction(float deltaTime)
	{
		Action onInteractionUpdate = this.OnInteractionUpdate;
		if (onInteractionUpdate != null)
		{
			onInteractionUpdate();
		}
		if (this.currentWgo == null || !this.currentWgo.HasData)
		{
			if (this.workInProgress)
			{
				this.StopInteraction();
				base.StartCoroutine(this.PauseComponentForOneFrame());
				return;
			}
			if (!this.TryStartInteraction())
			{
				this.StopInteraction();
			}
		}
		this.workInProgress = false;
		if (!(this.currentWgo != null))
		{
			this.StopInteraction();
			return;
		}
		if (this.IsOnWorkingSpot(this.currentWgo))
		{
			if (this.playerLocalAreaMovement.IsMovementStarted)
			{
				this.playerLocalAreaMovement.StopMovement(false);
			}
			if (!this.playerLocalAreaMovement.IsMovementStarted && !this.WorkIsTookControl)
			{
				this.AlignPlayerToDockPoint();
			}
			if (!this.toolComponent.IsActionActive && !this.TryStartInteraction())
			{
				return;
			}
			if (!this.toolComponent.IsActionActive)
			{
				return;
			}
			this.toolComponent.TryUpdateToolInUse(this.GetAppropriateToolForWork());
			this.toolComponent.UpdateInteraction();
			this.workInProgress = true;
			return;
		}
		else
		{
			if (this.playerLocalAreaMovement.Status == PlayerLocalAreaMovement.MovementStatus.CalcPath)
			{
				return;
			}
			this.playerLocalAreaMovement.CustomUpdate(deltaTime);
			return;
		}
	}

	// Token: 0x06001903 RID: 6403 RVA: 0x00076910 File Offset: 0x00074B10
	public void StopInteraction()
	{
		this.playerLocalAreaMovement.StopMovement(true);
		this.playerController.ClearWorkActivity();
		this.toolComponent.StopInteraction();
		this.currentWgo = null;
		this.currentWgoData = null;
		this.currentDockPoint = null;
		this.workInProgress = false;
		Action onInteractionUpdate = this.OnInteractionUpdate;
		if (onInteractionUpdate == null)
		{
			return;
		}
		onInteractionUpdate();
	}

	// Token: 0x06001904 RID: 6404 RVA: 0x0007696C File Offset: 0x00074B6C
	public void FindWgoToWork(List<Wgo> onlyWgoInList)
	{
		if (this.isMagnetismDelayed)
		{
			this.canUpdateMagnetismDelayTimer = true;
			return;
		}
		if (onlyWgoInList != null && onlyWgoInList.Count > 0)
		{
			this.currentWgo = this.FindWgoToWorkWith(true, onlyWgoInList);
		}
		if (this.currentWgo == null)
		{
			this.currentWgo = this.FindWgoToWorkWith(false, null);
		}
		if (this.currentWgo != null)
		{
			this.currentWgoData = this.currentWgo.Data;
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.PlayerFindWgoToWork, this.currentWgo.Id ?? "");
			this.SetMagnetismDelayState(true);
		}
	}

	// Token: 0x06001905 RID: 6405 RVA: 0x00076A00 File Offset: 0x00074C00
	public Wgo FindWgoToWorkNoAssign(List<Wgo> onlyWgoInList)
	{
		Wgo wgo = null;
		if (onlyWgoInList != null && onlyWgoInList.Count > 0)
		{
			wgo = this.FindWgoToWorkWith(true, onlyWgoInList);
		}
		if (wgo == null)
		{
			wgo = this.FindWgoToWorkWith(false, null);
		}
		return wgo;
	}

	// Token: 0x06001906 RID: 6406 RVA: 0x00076A38 File Offset: 0x00074C38
	private Item GetAppropriateToolForWork()
	{
		ItemType requiredInteractionToolType = this.currentWgo.InteractionHandler.GetRequiredInteractionToolType();
		Item itemByType = this.playerController.PlayerData.toolBeltInventory.Data.GetItemByType(requiredInteractionToolType);
		if (!itemByType.IsEmpty)
		{
			return itemByType;
		}
		return Item.Empty;
	}

	// Token: 0x06001907 RID: 6407 RVA: 0x00076A84 File Offset: 0x00074C84
	private IComponent GetComponentForInteraction(Wgo wgo)
	{
		CraftComponent craftComponent = wgo.Data.CraftComponent;
		if (craftComponent.IsManualActualCraftable && craftComponent.HasCraftsInQueue)
		{
			return craftComponent;
		}
		if (wgo.Data.Definition.hp > 0 || wgo.Data.Definition.hasInfiniteHp)
		{
			return wgo.Data.HpComponent;
		}
		return null;
	}

	// Token: 0x06001908 RID: 6408 RVA: 0x00076AE4 File Offset: 0x00074CE4
	private Wgo FindWgoToWorkWith(bool ignorePlayerDirection = false, List<Wgo> onlyWgoInList = null)
	{
		PlayerData playerData = this.playerController.PlayerData;
		DockPoint dockPoint = this.FindNearestDockPoint(playerData.position.Value, playerData.Direction, ignorePlayerDirection, onlyWgoInList);
		this.currentDockPoint = dockPoint;
		if (dockPoint != null && dockPoint.Owner.Data.IsInteractable)
		{
			return dockPoint.Owner;
		}
		return null;
	}

	// Token: 0x06001909 RID: 6409 RVA: 0x00076B44 File Offset: 0x00074D44
	public bool IsOnWorkingSpot(Wgo wgo)
	{
		return this.currentDockPoint != null && (this.currentDockPoint.transform.position - this.playerController.transform.position).sqrMagnitude < 0.01f;
	}

	// Token: 0x0600190A RID: 6410 RVA: 0x00076B95 File Offset: 0x00074D95
	private void AlignPlayerToDockPoint()
	{
		if (this.currentDockPoint == null || this.currentDockPoint.Direction == Direction.None)
		{
			return;
		}
		this.playerController.PhysicalBody.SetFacingDirection(this.currentDockPoint.Direction.ConvertToVector2XZ());
	}

	// Token: 0x0600190B RID: 6411 RVA: 0x00076BD4 File Offset: 0x00074DD4
	private DockPoint FindNearestDockPoint(Vector3 position, Vector2 direction, bool ignorePlayerDirection = false, List<Wgo> onlyWgoInList = null)
	{
		List<DockPoint> list = new List<DockPoint>();
		List<DockPoint> list2 = new List<DockPoint>();
		this.playerLocalAreaMovement.RescanPlayerGraph();
		if (onlyWgoInList == null)
		{
			Array.Clear(this.dockPointResults, 0, this.dockPointResults.Length);
			Debug.DrawLine(position, position + Vector3.right * 1.5f);
			Debug.DrawLine(position, position + Vector3.left * 1.5f);
			Debug.DrawLine(position, position + new Vector3(0f, 0f, 1f) * 1.5f);
			Debug.DrawLine(position, position + new Vector3(0f, 0f, -1f) * 1.5f);
			int num = Physics.OverlapSphereNonAlloc(position, 1.5f, this.dockPointResults, 128);
			for (int i = 0; i < Mathf.Min(this.dockPointResults.Length, num); i++)
			{
				DockPoint dockPoint;
				if (this.dockPointResults[i].TryGetComponent<DockPoint>(out dockPoint))
				{
					list2.Add(dockPoint);
				}
			}
		}
		else
		{
			foreach (Wgo wgo in onlyWgoInList)
			{
				list2.AddRange(wgo.DockPoints);
			}
		}
		foreach (DockPoint dockPoint2 in list2)
		{
			if (dockPoint2.gameObject.activeInHierarchy && dockPoint2.Owner && dockPoint2.Owner.MainWgoPart.InteractableColliders != null && dockPoint2.Owner.MainWgoPart.InteractableColliders.Count != 0 && this.CanWorkOn(dockPoint2.Owner.Data) && this.playerLocalAreaMovement.IsReachable(dockPoint2.transform.position))
			{
				list.Add(dockPoint2);
			}
		}
		float num2 = float.MaxValue;
		DockPoint dockPoint3 = null;
		foreach (DockPoint dockPoint4 in list)
		{
			bool flag = onlyWgoInList != null;
			Vector3 position2 = dockPoint4.transform.position;
			Path path = ABPath.Construct(position, position2, null);
			this.playerLocalAreaMovement.Seeker.StartPath(path);
			path.BlockUntilCalculated();
			float num3 = path.GetTotalLength();
			float num4 = Vector2.Angle(position2 - position, direction);
			if (!ignorePlayerDirection)
			{
				num3 += num4 * 0.0026666666f;
			}
			if (num3 < num2 && (flag || num3 <= 2.2f))
			{
				num2 = num3;
				dockPoint3 = dockPoint4;
			}
		}
		return dockPoint3;
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x00076EBC File Offset: 0x000750BC
	public bool CanWorkOn(WgoData wgoData)
	{
		if (wgoData.Worker != null)
		{
			return false;
		}
		HPComponent hpComponent = wgoData.HpComponent;
		CraftComponent craftComponent = wgoData.CraftComponent;
		bool flag = (hpComponent.Hp > 0 && wgoData.Definition.playerHpActivityMod > 0) || (hpComponent.Hp < hpComponent.MaxHpValue && wgoData.Definition.playerHpActivityMod < 0);
		return (craftComponent.CraftableObject.CraftableType != CraftableType.ConveyorWorkbench || craftComponent.IsDestroyingCraftActive) && ((flag && !hpComponent.isDeathDelayed) || (craftComponent.IsManualActualCraftable && craftComponent.HasCraftsInQueue));
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x00076F51 File Offset: 0x00075151
	public void UpdateMagnetismDelay(float deltaTime)
	{
		if (!this.isMagnetismDelayed || !this.canUpdateMagnetismDelayTimer)
		{
			return;
		}
		this.magnetismDelayTimer += deltaTime;
		if (this.magnetismDelayTimer >= 0.25f)
		{
			this.SetMagnetismDelayState(false);
		}
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x00076F86 File Offset: 0x00075186
	public void SetMagnetismDelayState(bool isDelayed)
	{
		this.isMagnetismDelayed = isDelayed;
		this.magnetismDelayTimer = 0f;
		this.canUpdateMagnetismDelayTimer = false;
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x00076FA1 File Offset: 0x000751A1
	private IEnumerator PauseComponentForOneFrame()
	{
		this.isActive = false;
		yield return this.onePhysicFrameTime;
		this.isActive = true;
		yield break;
	}

	// Token: 0x0400186B RID: 6251
	public Action OnInteractionUpdate;

	// Token: 0x0400186C RID: 6252
	private PlayerLocalAreaMovement playerLocalAreaMovement;

	// Token: 0x0400186D RID: 6253
	private PlayerController playerController;

	// Token: 0x0400186E RID: 6254
	private ToolComponent toolComponent;

	// Token: 0x0400186F RID: 6255
	private Wgo currentWgo;

	// Token: 0x04001870 RID: 6256
	private WgoData currentWgoData;

	// Token: 0x04001871 RID: 6257
	private DockPoint currentDockPoint;

	// Token: 0x04001872 RID: 6258
	private bool workInProgress;

	// Token: 0x04001873 RID: 6259
	private bool isActive = true;

	// Token: 0x04001874 RID: 6260
	private WaitForFixedUpdate onePhysicFrameTime = new WaitForFixedUpdate();

	// Token: 0x04001875 RID: 6261
	private Collider[] dockPointResults = new Collider[100];

	// Token: 0x04001876 RID: 6262
	private bool isMagnetismDelayed;

	// Token: 0x04001877 RID: 6263
	private bool canUpdateMagnetismDelayTimer;

	// Token: 0x04001878 RID: 6264
	private float magnetismDelayTimer;
}
