using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000314 RID: 788
public class FlagPlacementPoint : MonoBehaviour
{
	// Token: 0x17000397 RID: 919
	// (get) Token: 0x06001507 RID: 5383 RVA: 0x00066D80 File Offset: 0x00064F80
	public Vector3 FlagStandSpawnPosition
	{
		get
		{
			return this.flagStandSpawnPosition;
		}
	}

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x06001508 RID: 5384 RVA: 0x00066D88 File Offset: 0x00064F88
	// (set) Token: 0x06001509 RID: 5385 RVA: 0x00066D90 File Offset: 0x00064F90
	public Wgo FlagWgo
	{
		get
		{
			return this.flagWgo;
		}
		set
		{
			this.flagWgo = value;
		}
	}

	// Token: 0x0600150A RID: 5386 RVA: 0x00066D9C File Offset: 0x00064F9C
	public void Init(WgoData wgoData)
	{
		this.parentWgoData = wgoData;
		if (this.parentWgoData == null)
		{
			Debug.Log("FlagPlacementPoint must be a child of Wgo", base.gameObject);
			return;
		}
		if (!this.flagStandSpawnTransform)
		{
			Debug.Log("[FlagPlacementPoint]: FlagStandSpawnTransform is not set, using transform of FlagPlacementPoint", base.gameObject);
			this.flagStandSpawnTransform = base.transform;
		}
		this.flagStandSpawnPosition = this.flagStandSpawnTransform.position;
		this.parentWgoData.OnRemoveFromData += this.TrySpawnPlacementPointAndAttachFlag;
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x00066E1A File Offset: 0x0006501A
	public void DeInit()
	{
		if (this.parentWgoData == null || this.flagStandSpawnPosition.magnitude.EqualsTo(0f, 0.0001f))
		{
			return;
		}
		this.parentWgoData.OnRemoveFromData -= this.TrySpawnPlacementPointAndAttachFlag;
	}

	// Token: 0x0600150C RID: 5388 RVA: 0x00066E58 File Offset: 0x00065058
	private void TrySpawnPlacementPointAndAttachFlag()
	{
		Debug.Log(string.Format("TrySpawnPlacementPointAndAttachFlag: ParentWgo: {0}, FlagWgo: {1}", this.parentWgoData, this.flagWgo));
		if (this.parentWgoData == null)
		{
			return;
		}
		FightingGameController instance = LazySingleton<FightingGameController>.Instance;
		if (instance != null && instance.IsClearingFightEnvironment)
		{
			this.parentWgoData.OnRemoveFromData -= this.TrySpawnPlacementPointAndAttachFlag;
			return;
		}
		if (this.flagWgo && !this.flagStandSpawnPosition.magnitude.EqualsTo(0f, 0.0001f))
		{
			WgoData wgoData = new WgoData("flag_stand_one_time", this.flagStandSpawnPosition, this.FlagWgo.Data.WorldId);
			MainGame.Instance.GameSave.worldData.AddWgoData(wgoData);
			LazySingleton<FightingGameController>.Instance.AddTemporaryWgoData(wgoData);
			Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(wgoData.UniqueId);
			if (wgoViewGlobal)
			{
				AgentsGroupFlagController componentInChildren = this.flagWgo.GetComponentInChildren<AgentsGroupFlagController>();
				if (componentInChildren)
				{
					componentInChildren.IsSetAtPoint = false;
				}
				FlagStandComponent componentInChildren2 = wgoViewGlobal.GetComponentInChildren<FlagStandComponent>();
				if (componentInChildren2)
				{
					componentInChildren2.AttachFlag(this.flagWgo, true);
					LazySingleton<FightingGameController>.Instance.FlagStandComponents.Add(componentInChildren2);
				}
				wgoViewGlobal.Data.GameResStr.Set("flag_stand_sguid", this.flagWgo.Data.UniqueId.ToString());
			}
		}
		this.parentWgoData.OnRemoveFromData -= this.TrySpawnPlacementPointAndAttachFlag;
	}

	// Token: 0x040015C3 RID: 5571
	private const string FLAG_STAND_ONE_TIME_ID = "flag_stand_one_time";

	// Token: 0x040015C4 RID: 5572
	private WgoData parentWgoData;

	// Token: 0x040015C5 RID: 5573
	private Wgo flagWgo;

	// Token: 0x040015C6 RID: 5574
	public Transform flagStandSpawnTransform;

	// Token: 0x040015C7 RID: 5575
	private Vector3 flagStandSpawnPosition = Vector3.zero;
}
