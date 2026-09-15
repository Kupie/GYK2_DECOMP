using System;
using System.Collections.Generic;
using LazyBearTechnology;
using LinqTools;
using UnityEngine;

// Token: 0x020002CD RID: 717
public class AgentsGroupFlagController : MonoBehaviour, IWgoCustomComponent<WCCD_AgentGroupFlagController>
{
	// Token: 0x17000303 RID: 771
	// (get) Token: 0x0600125F RID: 4703 RVA: 0x0005B262 File Offset: 0x00059462
	// (set) Token: 0x06001260 RID: 4704 RVA: 0x0005B28D File Offset: 0x0005948D
	public SGuid AttachedSGuid
	{
		get
		{
			return SGuid.Parse(this.flagWgo.Data.GameResStr.Get("attached_sguid", SGuid.Empty.ToString()));
		}
		set
		{
			this.flagWgo.Data.GameResStr.Set("attached_sguid", (!SGuid.IsNullOrEmpty(value)) ? value.ToString() : SGuid.Empty.ToString());
		}
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06001261 RID: 4705 RVA: 0x0005B2C3 File Offset: 0x000594C3
	// (set) Token: 0x06001262 RID: 4706 RVA: 0x0005B2E3 File Offset: 0x000594E3
	public bool IsSetAtPoint
	{
		get
		{
			Wgo wgo = this.flagWgo;
			return wgo != null && wgo.Data.GetGameResInt("is_set_on_point") == 1;
		}
		set
		{
			Wgo wgo = this.flagWgo;
			if (wgo == null)
			{
				return;
			}
			wgo.Data.SetGameRes("is_set_on_point", value ? 1 : 0);
		}
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x06001263 RID: 4707 RVA: 0x0005B308 File Offset: 0x00059508
	public Wgo FlagWgo
	{
		get
		{
			if (this.flagWgo != null)
			{
				return this.flagWgo;
			}
			WgoPart component = base.GetComponent<WgoPart>();
			this.flagWgo = ((component != null) ? component.Wgo : null);
			if (!this.flagWgo)
			{
				Debug.LogError("Flag Wgo is null", this);
				return null;
			}
			return this.flagWgo;
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06001264 RID: 4708 RVA: 0x0005B362 File Offset: 0x00059562
	public AgentsGroupBehaviourController AgentsController
	{
		get
		{
			return this.agentsController;
		}
	}

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06001265 RID: 4709 RVA: 0x0005B36A File Offset: 0x0005956A
	// (set) Token: 0x06001266 RID: 4710 RVA: 0x0005B372 File Offset: 0x00059572
	public FightingCapturePoint CapturePoint { get; private set; }

	// Token: 0x06001267 RID: 4711 RVA: 0x0005B37B File Offset: 0x0005957B
	public static bool IsInteractionLocked(Wgo flagWgo)
	{
		return flagWgo.Data.GetGameResInt("is_interaction_locked") == 1;
	}

	// Token: 0x06001268 RID: 4712 RVA: 0x0005B390 File Offset: 0x00059590
	public static void SetInteractionLocked(Wgo flagWgo, bool isLocked)
	{
		flagWgo.Data.SetGameRes("is_interaction_locked", isLocked ? 1 : 0);
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x0005B3A9 File Offset: 0x000595A9
	public WCCD_AgentGroupFlagController OnSave()
	{
		WCCD_AgentGroupFlagController wccd_AgentGroupFlagController = new WCCD_AgentGroupFlagController();
		wccd_AgentGroupFlagController.presetWgoUIds = this.preSetWgos.Select((Wgo wgo) => wgo.Data.UniqueId).ToList<SGuid>();
		return wccd_AgentGroupFlagController;
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x0005B3E5 File Offset: 0x000595E5
	public void OnLoad(WCCD_AgentGroupFlagController data)
	{
		this.presetWgoUIds = data.presetWgoUIds;
	}

	// Token: 0x0600126B RID: 4715 RVA: 0x0005B3F3 File Offset: 0x000595F3
	public void OnUnload()
	{
		this.presetWgoUIds.Clear();
	}

	// Token: 0x0600126C RID: 4716 RVA: 0x0005B400 File Offset: 0x00059600
	public void Init()
	{
		if (this.isInited)
		{
			return;
		}
		this.isInited = true;
		base.enabled = false;
		this.preSetWgos = this.presetWgoUIds.Select(new Func<SGuid, Wgo>(GameScene.GetWgoViewGlobal)).ToList<Wgo>();
		WgoPart component = base.GetComponent<WgoPart>();
		this.flagWgo = ((component != null) ? component.Wgo : null);
		if (!this.flagWgo)
		{
			Debug.LogError("Flag Wgo is null", this);
			this.isInited = false;
			return;
		}
		this.agentsController.Init();
		LazySingleton<FightingGameController>.Instance.RegisterTargetNonPersistent(this.flagWgo, -1, -1, true);
		MainGame.PlayerController.CurrentGameScene.GameSceneData.OnWgoDataPreRemove += this.HandleZombieRemoved;
		for (int i = 0; i < this.preSetWgos.Count; i++)
		{
			Wgo wgo = this.preSetWgos[i];
			if (wgo)
			{
				wgo.UpdateFlag(ChunkingIgnoreType.Fighting, true);
				FightingAgent fightingAgent = this.agentsController.AddWgoAsAgent(wgo, null, true);
				fightingAgent.FlagController = this;
				fightingAgent.AssignWeaponFromInventory();
			}
		}
	}

	// Token: 0x0600126D RID: 4717 RVA: 0x0005B50C File Offset: 0x0005970C
	public void DeInit()
	{
		this.isInited = false;
		this.ClearCapturePointBinding();
		if (this.agentsController)
		{
			for (int i = this.agentsController.Agents.Count - 1; i >= 0; i--)
			{
				FightingAgent fightingAgent = this.agentsController.Agents[i];
				if (fightingAgent)
				{
					fightingAgent.FlagController = null;
				}
				this.agentsController.RemoveAgent(fightingAgent);
			}
			this.agentsController.DeInit();
		}
		GameScene gameScene;
		if (!MainGame.PlayerController.TryGetCurrentGameScene(out gameScene) || gameScene.GameSceneData == null)
		{
			return;
		}
		gameScene.GameSceneData.OnWgoDataPreRemove -= this.HandleZombieRemoved;
	}

	// Token: 0x0600126E RID: 4718 RVA: 0x0005B5B6 File Offset: 0x000597B6
	public void SetEnabled(bool isEnabled)
	{
		base.enabled = isEnabled;
	}

	// Token: 0x0600126F RID: 4719 RVA: 0x0005B5C0 File Offset: 0x000597C0
	public void BindCapturePoint(FightingCapturePoint capturePoint)
	{
		this.CapturePoint = capturePoint;
		if (this.agentsController == null)
		{
			return;
		}
		FightingLine fightingLine;
		if (!(capturePoint != null))
		{
			fightingLine = null;
		}
		else
		{
			FightingSector sector = capturePoint.Sector;
			fightingLine = ((sector != null) ? sector.fightingLine : null);
		}
		FightingLine fightingLine2 = fightingLine;
		this.agentsController.FightingLine = fightingLine2;
	}

	// Token: 0x06001270 RID: 4720 RVA: 0x0005B60E File Offset: 0x0005980E
	public void ClearCapturePointBinding()
	{
		this.BindCapturePoint(null);
	}

	// Token: 0x06001271 RID: 4721 RVA: 0x0005B618 File Offset: 0x00059818
	private void OnDestroy()
	{
		GameScene currentGameScene = MainGame.PlayerController.CurrentGameScene;
		if (currentGameScene)
		{
			currentGameScene.GameSceneData.OnWgoDataPreRemove -= this.HandleZombieRemoved;
		}
		if (this.flagWgo != null)
		{
			LazySingleton<FightingGameController>.Instance.UnregisterTarget(this.flagWgo);
		}
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x0005B66D File Offset: 0x0005986D
	public void CustomUpdate(float deltaTime)
	{
		if (!base.enabled)
		{
			return;
		}
		this.agentsController.CustomUpdate(deltaTime);
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x0005B684 File Offset: 0x00059884
	private void HandleZombieRemoved(WgoData wgoData)
	{
		this.agentsController.RemoveAgent(wgoData.UniqueId);
	}

	// Token: 0x0400140D RID: 5133
	private const string IS_SET_ON_POINT_KEY = "is_set_on_point";

	// Token: 0x0400140E RID: 5134
	private const string ATTACHED_SGUID_KEY = "attached_sguid";

	// Token: 0x0400140F RID: 5135
	private Wgo flagWgo;

	// Token: 0x04001410 RID: 5136
	[SerializeField]
	private AgentsGroupBehaviourController agentsController;

	// Token: 0x04001411 RID: 5137
	[SerializeField]
	[Space]
	private List<Wgo> preSetWgos = new List<Wgo>();

	// Token: 0x04001412 RID: 5138
	[SerializeField]
	private List<SGuid> presetWgoUIds = new List<SGuid>();

	// Token: 0x04001413 RID: 5139
	private bool isInited;
}
