using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000315 RID: 789
public class FlagStandComponent : MonoBehaviour, IWgoCustomComponent<WCCD_FlagStandComponent>
{
	// Token: 0x17000399 RID: 921
	// (get) Token: 0x0600150E RID: 5390 RVA: 0x00066FD4 File Offset: 0x000651D4
	public FlagPlacementPoint FlagPlacementPoint
	{
		get
		{
			return this.flagPlacementPoint;
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x0600150F RID: 5391 RVA: 0x00066FDC File Offset: 0x000651DC
	public Wgo StandWgo
	{
		get
		{
			if (!this.standWgo)
			{
				this.standWgo = base.GetComponentInParent<Wgo>(true);
			}
			return this.standWgo;
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x06001510 RID: 5392 RVA: 0x00066FFE File Offset: 0x000651FE
	public WgoData FlagWgoData
	{
		get
		{
			return MainGame.Instance.GameSave.worldData.GetWgoData(this.flagSguid);
		}
	}

	// Token: 0x06001511 RID: 5393 RVA: 0x0006701C File Offset: 0x0006521C
	public WCCD_FlagStandComponent OnSave()
	{
		WCCD_FlagStandComponent wccd_FlagStandComponent = new WCCD_FlagStandComponent();
		if (!this.linkedFlag)
		{
			return wccd_FlagStandComponent;
		}
		wccd_FlagStandComponent.flagSGuid = this.linkedFlag.Data.UniqueId;
		return wccd_FlagStandComponent;
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x00067055 File Offset: 0x00065255
	public void OnLoad(WCCD_FlagStandComponent data)
	{
		if (data == null)
		{
			return;
		}
		if (!SGuid.IsNullOrEmpty(data.flagSGuid))
		{
			this.flagSguid = data.flagSGuid;
			this.linkedFlag = GameScene.GetWgoViewGlobal(this.flagSguid);
		}
		this.standWgo = null;
		this.OnEnableLogic();
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x00067092 File Offset: 0x00065292
	public void OnUnload()
	{
		this.linkedFlag = null;
		this.flagSguid = SGuid.Empty;
		this.standWgo = null;
	}

	// Token: 0x06001514 RID: 5396 RVA: 0x000670AD File Offset: 0x000652AD
	public void AttachFlag(Wgo flag, bool bindCapturePoint = true)
	{
		this.linkedFlag = flag;
		this.flagSguid = flag.Data.UniqueId;
		if (bindCapturePoint)
		{
			this.TryBindFlagToCapturePoint(null);
			return;
		}
		AgentsGroupFlagController componentInChildren = flag.GetComponentInChildren<AgentsGroupFlagController>();
		if (componentInChildren == null)
		{
			return;
		}
		componentInChildren.ClearCapturePointBinding();
	}

	// Token: 0x06001515 RID: 5397 RVA: 0x000670E4 File Offset: 0x000652E4
	public void TryBindFlagToCapturePoint(FightingCapturePoint capturePoint = null)
	{
		Wgo wgo = (this.linkedFlag ? this.linkedFlag : GameScene.GetWgoViewGlobal(this.flagSguid));
		if (!wgo)
		{
			return;
		}
		AgentsGroupFlagController componentInChildren = wgo.GetComponentInChildren<AgentsGroupFlagController>();
		if (!componentInChildren)
		{
			return;
		}
		if (capturePoint == null)
		{
			capturePoint = this.ResolveCapturePoint();
		}
		if (capturePoint)
		{
			capturePoint.BindAllyFlagController(componentInChildren);
			return;
		}
		componentInChildren.ClearCapturePointBinding();
		Debug.LogWarning(string.Format("FlagStand '{0}' at {1} is not linked to any sector FightingCapturePoint. ", base.name, this.StandWgo ? this.StandWgo.Data.Position : base.transform.position) + "Place the stand inside a sector CP radius, or assign allyFlagStand on the capture point.", this);
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x0006719C File Offset: 0x0006539C
	private FightingCapturePoint ResolveCapturePoint()
	{
		FightingCapturePoint componentInParent = base.GetComponentInParent<FightingCapturePoint>(true);
		if (componentInParent)
		{
			if (componentInParent.isBasePoint)
			{
				return null;
			}
			return componentInParent;
		}
		else
		{
			Vector3 vector = (this.StandWgo ? this.StandWgo.Data.Position : base.transform.position);
			FightingGameController instance = LazySingleton<FightingGameController>.Instance;
			if (instance == null)
			{
				return null;
			}
			FightingLevel currentLevel = instance.CurrentLevel;
			if (currentLevel == null)
			{
				return null;
			}
			return currentLevel.FindCapturePointForFlagStand(vector);
		}
	}

	// Token: 0x06001517 RID: 5399 RVA: 0x0006720C File Offset: 0x0006540C
	public void DetachFlag()
	{
		if (!SGuid.IsNullOrEmpty(this.flagSguid))
		{
			Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(this.flagSguid);
			if (wgoViewGlobal)
			{
				AgentsGroupFlagController componentInChildren = wgoViewGlobal.GetComponentInChildren<AgentsGroupFlagController>();
				if (componentInChildren)
				{
					componentInChildren.ClearCapturePointBinding();
					wgoViewGlobal.UpdateFlag(ChunkingIgnoreType.Fighting, true);
					LazySingleton<FightingGameController>.Instance.AllDynamicObjectsInZone.Add(wgoViewGlobal);
					LazySingleton<FightingGameController>.Instance.customFlagControllers.Add(componentInChildren);
				}
			}
		}
		this.linkedFlag = null;
		this.flagSguid = SGuid.Empty;
	}

	// Token: 0x06001518 RID: 5400 RVA: 0x0006728B File Offset: 0x0006548B
	public void DestroyStandWgo()
	{
		if (this.StandWgo)
		{
			this.StandWgo.RemoveWithData();
		}
	}

	// Token: 0x06001519 RID: 5401 RVA: 0x000672A5 File Offset: 0x000654A5
	public void TryToSyncFlagPosition()
	{
		if (this.FlagWgoData == null || !this.flagPlacementPoint)
		{
			return;
		}
		this.FlagWgoData.Position = this.flagPlacementPoint.transform.position;
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x000672D8 File Offset: 0x000654D8
	private void OnEnable()
	{
		this.standWgo = null;
		this.OnEnableLogic();
	}

	// Token: 0x0600151B RID: 5403 RVA: 0x000672E8 File Offset: 0x000654E8
	private void OnEnableLogic()
	{
		this.TryToSyncFlagPosition();
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(this.flagSguid);
		if (wgoViewGlobal)
		{
			AgentsGroupFlagController.SetInteractionLocked(wgoViewGlobal, true);
			if (this.StandWgo)
			{
				this.StandWgo.Data.GameResStr.Set("flag_stand_sguid", wgoViewGlobal.Data.UniqueId.ToString());
			}
		}
	}

	// Token: 0x040015C8 RID: 5576
	[SerializeField]
	private Wgo linkedFlag;

	// Token: 0x040015C9 RID: 5577
	[SerializeField]
	private SGuid flagSguid;

	// Token: 0x040015CA RID: 5578
	[SerializeField]
	private FlagPlacementPoint flagPlacementPoint;

	// Token: 0x040015CB RID: 5579
	private Wgo standWgo;
}
