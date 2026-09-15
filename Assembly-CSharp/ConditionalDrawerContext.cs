using System;
using JetBrains.Annotations;

// Token: 0x020003F9 RID: 1017
public class ConditionalDrawerContext
{
	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06001A94 RID: 6804 RVA: 0x0007BEBB File Offset: 0x0007A0BB
	public WgoPart WgoPart { get; }

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06001A95 RID: 6805 RVA: 0x0007BEC3 File Offset: 0x0007A0C3
	[CanBeNull]
	public WgoData WgoData
	{
		get
		{
			WgoPart wgoPart = this.WgoPart;
			if (wgoPart == null)
			{
				return null;
			}
			Wgo wgo = wgoPart.Wgo;
			if (wgo == null)
			{
				return null;
			}
			return wgo.Data;
		}
	}

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06001A96 RID: 6806 RVA: 0x0007BEE1 File Offset: 0x0007A0E1
	[CanBeNull]
	public Inventory Inventory
	{
		get
		{
			WgoData wgoData = this.WgoData;
			if (wgoData == null)
			{
				return null;
			}
			return wgoData.Inventory;
		}
	}

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06001A97 RID: 6807 RVA: 0x0007BEF4 File Offset: 0x0007A0F4
	[CanBeNull]
	public CraftComponent CraftComponent
	{
		get
		{
			WgoData wgoData = this.WgoData;
			if (wgoData == null)
			{
				return null;
			}
			return wgoData.CraftComponent;
		}
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06001A98 RID: 6808 RVA: 0x0007BF08 File Offset: 0x0007A108
	[CanBeNull]
	public ConveyorComponent ConveyorComponent
	{
		get
		{
			ConveyorWgoData conveyorWgoData = this.WgoData as ConveyorWgoData;
			if (conveyorWgoData == null)
			{
				return null;
			}
			return conveyorWgoData.ConveyorComponent;
		}
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06001A99 RID: 6809 RVA: 0x0007A4E1 File Offset: 0x000786E1
	[CanBeNull]
	private PlayerWorkComponent PlayerWorkComp
	{
		get
		{
			PlayerController playerController = MainGame.PlayerController;
			if (playerController == null)
			{
				return null;
			}
			return playerController.PlayerWorkComponent;
		}
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06001A9A RID: 6810 RVA: 0x0007BF2C File Offset: 0x0007A12C
	[CanBeNull]
	private FightingAgent FightingAgent
	{
		get
		{
			WgoPart wgoPart = this.WgoPart;
			if (wgoPart == null)
			{
				return null;
			}
			return wgoPart.GetComponent<FightingAgent>();
		}
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x06001A9B RID: 6811 RVA: 0x0007BF3F File Offset: 0x0007A13F
	[CanBeNull]
	public ZombieWgoData ZombieWgoData
	{
		get
		{
			return this.WgoData as ZombieWgoData;
		}
	}

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06001A9C RID: 6812 RVA: 0x0007BF4C File Offset: 0x0007A14C
	// (set) Token: 0x06001A9D RID: 6813 RVA: 0x0007BF54 File Offset: 0x0007A154
	public bool IsInWork { get; private set; }

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x06001A9E RID: 6814 RVA: 0x0007BF5D File Offset: 0x0007A15D
	// (set) Token: 0x06001A9F RID: 6815 RVA: 0x0007BF65 File Offset: 0x0007A165
	public bool HasWorker { get; private set; }

	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x06001AA0 RID: 6816 RVA: 0x0007BF6E File Offset: 0x0007A16E
	// (set) Token: 0x06001AA1 RID: 6817 RVA: 0x0007BF76 File Offset: 0x0007A176
	public bool IsCraftStarted { get; private set; }

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x06001AA2 RID: 6818 RVA: 0x0007BF7F File Offset: 0x0007A17F
	// (set) Token: 0x06001AA3 RID: 6819 RVA: 0x0007BF87 File Offset: 0x0007A187
	public bool IsCalledFromEvent { get; private set; }

	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x06001AA4 RID: 6820 RVA: 0x0007BF90 File Offset: 0x0007A190
	// (set) Token: 0x06001AA5 RID: 6821 RVA: 0x0007BF98 File Offset: 0x0007A198
	public bool IsFightingAgentActive { get; private set; }

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x06001AA6 RID: 6822 RVA: 0x0007BFA1 File Offset: 0x0007A1A1
	// (set) Token: 0x06001AA7 RID: 6823 RVA: 0x0007BFA9 File Offset: 0x0007A1A9
	public bool IsBuildingModeActive { get; private set; }

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06001AA8 RID: 6824 RVA: 0x0007BFB2 File Offset: 0x0007A1B2
	// (set) Token: 0x06001AA9 RID: 6825 RVA: 0x0007BFBA File Offset: 0x0007A1BA
	public bool IsZombieCareTakerOnStation { get; private set; }

	// Token: 0x06001AAA RID: 6826 RVA: 0x0007BFC3 File Offset: 0x0007A1C3
	public ConditionalDrawerContext(WgoPart wgoPart, BuildController buildController)
	{
		this.WgoPart = wgoPart;
		this.buildController = buildController;
		this.UpdateCachedValues(false);
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x0007BFE0 File Offset: 0x0007A1E0
	public void UpdateCachedValues(bool fromCalledEvent = false)
	{
		this.IsBuildingModeActive = this.buildController != null && this.buildController.IsBuildModeActive;
		this.IsInWork = false;
		if (this.WgoData == null)
		{
			this.HasWorker = false;
			this.IsCraftStarted = false;
			return;
		}
		CraftComponent craftComponent = this.CraftComponent;
		this.IsCraftStarted = craftComponent != null && craftComponent.Status == CraftComponentStatus.Started;
		this.IsCalledFromEvent = fromCalledEvent;
		this.HasWorker = this.WgoData.Worker != null && !this.WgoData.Worker.Id.IsEmpty;
		if (this.HasWorker)
		{
			bool flag = this.WgoPart.Wgo.InteractionHandler is WorkInteractionHandler;
			if (this.WgoData.Worker is PlayerController)
			{
				this.IsInWork = (flag || this.IsCraftStarted) && this.PlayerWorkComp != null && this.PlayerWorkComp.Wgo == this.WgoPart.Wgo && this.PlayerWorkComp.WorkInProgress && this.PlayerWorkComp.ToolComponent.IsActionActive;
			}
			else
			{
				ZombieWgoData zombieWgoData = this.WgoData.Worker as ZombieWgoData;
				if (zombieWgoData != null)
				{
					bool flag2;
					if (flag || this.IsCraftStarted)
					{
						ZombieCraftActivity zombieCraftActivity = zombieWgoData.WorkerActivity as ZombieCraftActivity;
						if (zombieCraftActivity != null)
						{
							flag2 = zombieCraftActivity.IsActive;
							goto IL_015C;
						}
					}
					flag2 = false;
					IL_015C:
					this.IsInWork = flag2;
				}
			}
		}
		FightingAgent fightingAgent = this.FightingAgent;
		this.IsFightingAgentActive = fightingAgent != null && fightingAgent.IsValid;
		ZombieWgoData zombieWgoData2 = this.ZombieWgoData;
		this.IsZombieCareTakerOnStation = ((zombieWgoData2 != null) ? new ZombieWgoData.ZombieCaretakerState?(zombieWgoData2.CaretakerState) : null) == ZombieWgoData.ZombieCaretakerState.OnStation;
	}

	// Token: 0x040019BB RID: 6587
	private readonly BuildController buildController;
}
