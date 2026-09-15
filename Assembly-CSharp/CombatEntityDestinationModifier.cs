using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020002E0 RID: 736
public class CombatEntityDestinationModifier : GoToDestinationModifier
{
	// Token: 0x17000344 RID: 836
	// (get) Token: 0x0600133F RID: 4927 RVA: 0x0005E284 File Offset: 0x0005C484
	public override bool IsValid
	{
		get
		{
			return !this.wasDockPointValid || this.targetDockPoint != null || this.targetObj == null;
		}
	}

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06001340 RID: 4928 RVA: 0x0005E2A1 File Offset: 0x0005C4A1
	public override bool ShouldAnchorOnArrival
	{
		get
		{
			return this.targetDockPoint != null;
		}
	}

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06001341 RID: 4929 RVA: 0x0005E2AC File Offset: 0x0005C4AC
	public WgoData TargeObj
	{
		get
		{
			return this.targetObj;
		}
	}

	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06001342 RID: 4930 RVA: 0x0005E2B4 File Offset: 0x0005C4B4
	public DockPointData TargetDockPoint
	{
		get
		{
			return this.targetDockPoint;
		}
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x0005E2BC File Offset: 0x0005C4BC
	public CombatEntityDestinationModifier(DockPointData dockPointData = null)
	{
		this.customDockPoint = dockPointData;
		this.wasDockPointValid = this.customDockPoint != null;
	}

	// Token: 0x06001344 RID: 4932 RVA: 0x0005E2DA File Offset: 0x0005C4DA
	public override void Init(FightingAgent agent)
	{
		base.Init(agent);
		this.UpdateTargetData();
	}

	// Token: 0x06001345 RID: 4933 RVA: 0x0005E2EC File Offset: 0x0005C4EC
	private void UpdateTargetData()
	{
		if (base.Agent.MobCommand.TargetEntity == null)
		{
			this.targetObj = null;
			this.targetDockPoint = null;
			return;
		}
		this.targetObj = MainGame.WorldData.GetWgoData(base.Agent.MobCommand.TargetEntity.CombatEntityUID);
		if (this.customDockPoint != null)
		{
			this.targetDockPoint = this.customDockPoint;
			return;
		}
		CombatEntityDestinationModifier.<>c__DisplayClass14_0 CS$<>8__locals1 = new CombatEntityDestinationModifier.<>c__DisplayClass14_0();
		CombatEntityDestinationModifier.<>c__DisplayClass14_0 CS$<>8__locals2 = CS$<>8__locals1;
		FightingGameController instance = LazySingleton<FightingGameController>.Instance;
		CS$<>8__locals2.recastGraph = ((instance != null) ? instance.RecastGraph : null);
		WgoData wgoData = this.targetObj;
		this.targetDockPoint = ((wgoData != null) ? wgoData.MainWgoPartData.GetNearestDockPoint(this.targetObj, base.Wgo.Data.Position, DockPointData.Availability.OnlyNotOccupied, DockPointData.Filter.All, (CS$<>8__locals1.recastGraph == null) ? null : new Func<DockPointData, Vector3, bool>((DockPointData data, Vector3 parentPos) => data.IsOnRecast(parentPos, CS$<>8__locals1.recastGraph))) : null);
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x0005E3C0 File Offset: 0x0005C5C0
	public override bool TryGetTargetPositionForPathfinding(out ICombatEntity combatEntity, out Vector3 position)
	{
		combatEntity = null;
		if (this.targetDockPoint != null && this.targetObj != null && base.Agent.MobCommand.TargetEntity != null)
		{
			position = this.targetObj.GetDockPointDataWorldPosition(this.targetDockPoint);
			return true;
		}
		if (base.Agent.MobCommand.TargetEntity != null)
		{
			position = base.Agent.MobCommand.TargetEntity.CombatEntityPosition;
			combatEntity = base.Agent.MobCommand.TargetEntity;
			return true;
		}
		position = base.Agent.MobCommand.Position;
		return true;
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x0005E463 File Offset: 0x0005C663
	public override Vector3 GetCurrentTargetPosition()
	{
		if (this.targetDockPoint != null && this.targetObj != null)
		{
			return this.targetObj.GetDockPointDataWorldPosition(this.targetDockPoint);
		}
		return base.Agent.MobCommand.Position;
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x0005E497 File Offset: 0x0005C697
	public override void OnReachedDestination()
	{
		if (base.Wgo.TeamType == LazyConsts.Fighting.TeamType.Player)
		{
			return;
		}
		ICombatEntity targetEntity = base.Agent.MobCommand.TargetEntity;
		if (targetEntity == null)
		{
			return;
		}
		targetEntity.OnOtherCombatTargetReachedToMe(base.Wgo);
	}

	// Token: 0x06001349 RID: 4937 RVA: 0x0005E4C8 File Offset: 0x0005C6C8
	public override GoToDestinationModifierDebugInfo GetDebugInfo()
	{
		GoToDestinationModifierDebugInfo debugInfo = base.GetDebugInfo();
		WgoData wgoData = this.targetObj;
		debugInfo.TargetWgoId = ((wgoData != null) ? wgoData.id : null);
		debugInfo.TargetUniqueId = ((this.targetObj != null) ? this.targetObj.UniqueId.ToString() : null);
		FightingAgent agent = base.Agent;
		ICombatEntity combatEntity;
		if (agent == null)
		{
			combatEntity = null;
		}
		else
		{
			MobCommand mobCommand = agent.MobCommand;
			combatEntity = ((mobCommand != null) ? mobCommand.TargetEntity : null);
		}
		debugInfo.TargetWgo = GoToDestinationModifierDebugInfo.ResolveTargetWgoView(combatEntity, this.targetObj);
		debugInfo.ActiveDockPoint = this.targetDockPoint;
		debugInfo.CustomDockPoint = this.customDockPoint;
		debugInfo.ActiveDockPointView = GoToDestinationModifierDebugInfo.ResolveDockPointView(debugInfo.TargetWgo, this.targetDockPoint);
		debugInfo.CustomDockPointView = GoToDestinationModifierDebugInfo.ResolveDockPointView(debugInfo.TargetWgo, this.customDockPoint);
		debugInfo.WasDockPointValid = this.wasDockPointValid;
		return debugInfo;
	}

	// Token: 0x04001498 RID: 5272
	private DockPointData targetDockPoint;

	// Token: 0x04001499 RID: 5273
	private WgoData targetObj;

	// Token: 0x0400149A RID: 5274
	private DockPointData customDockPoint;

	// Token: 0x0400149B RID: 5275
	private bool wasDockPointValid;
}
