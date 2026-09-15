using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020001B1 RID: 433
public class NPCLifeSimulator : ICustomUpdatable
{
	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06000AEB RID: 2795 RVA: 0x00036C78 File Offset: 0x00034E78
	private NPCLifeSimulatorData Data
	{
		get
		{
			return MainGame.Instance.GameSave.npcLifeSimulatorData;
		}
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x00036C89 File Offset: 0x00034E89
	public void CustomUpdate(float deltaTime)
	{
		this.CheckActionsForWgos(deltaTime);
		this.CheckAnimationsRollForWgos(deltaTime);
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x00036C9C File Offset: 0x00034E9C
	public void RollActivityForWgo(SGuid wgoUniqueId, NPCGroupPointOfInterestData groupData, float delay)
	{
		WgoData wgoData = MainGame.WorldData.GetWgoData(wgoUniqueId);
		if (wgoData == null)
		{
			Debug.LogError(string.Format("RollActivityForWgo WGO data with sguid:[{0}] not found", wgoUniqueId));
			return;
		}
		if (wgoData.GetGameRes("npc_life_sim_disabled_flag") > 0f)
		{
			return;
		}
		if (groupData == null)
		{
			groupData = this.Data.GetGroupByWGOId(wgoUniqueId);
		}
		if (groupData == null)
		{
			Debug.LogError("RollActivityForWgo WGO:[" + wgoData.id + "] group not found");
			return;
		}
		if (!string.IsNullOrEmpty(wgoData.occupiedPointOfInterest))
		{
			NPCPointOfInterestData pointById = this.Data.GetPointById(wgoData.occupiedPointOfInterest);
			if (pointById != null)
			{
				this.ExecuteAnimationWhenLeavedPointForWgo(wgoData);
				pointById.Deoccupy();
			}
		}
		NPCPointOfInterestData npcpointOfInterestData = this.Data.RollPoint(groupData.Id);
		if (npcpointOfInterestData == null)
		{
			Debug.LogError("RollActivityForWgo WGO:[" + wgoData.id + "] no free points");
			return;
		}
		wgoData.occupiedPointOfInterest = npcpointOfInterestData.Id;
		npcpointOfInterestData.Occupy(wgoData.UniqueId);
		this.Data.AddActionData(new NPCLifeSimulatorActionData(wgoUniqueId, delay, NPCLifeSimulatorActionType.GoToPointOfInterest));
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x00036D98 File Offset: 0x00034F98
	public void RollActivityForGroup(string groupId)
	{
		NPCGroupPointOfInterestData groupById = this.Data.GetGroupById(groupId);
		if (groupById == null)
		{
			Debug.LogError("RollActivityForGroup no such group:[" + groupId + "]");
			return;
		}
		int num = 0;
		float minActionDelay = LazySingletonSO<NPCLifeSimulatorConfiguration>.Instance.MinActionDelay;
		float num2 = (LazySingletonSO<NPCLifeSimulatorConfiguration>.Instance.MaxActionDelay - minActionDelay) / (float)groupById.Wgos.Count;
		foreach (SGuid sguid in groupById.Wgos)
		{
			float num3 = minActionDelay + num2 * (float)num;
			float num4 = global::UnityEngine.Random.Range(0f, num2);
			this.RollActivityForWgo(sguid, groupById, num3 + num4);
			num++;
		}
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x00036E5C File Offset: 0x0003505C
	public void RollActivityForAllGroups()
	{
		foreach (NPCGroupPointOfInterestData npcgroupPointOfInterestData in this.Data.AllGroups)
		{
			this.RollActivityForGroup(npcgroupPointOfInterestData.Id);
		}
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x00036EBC File Offset: 0x000350BC
	public void SendWgoHome(SGuid wgoUniqueId, NPCGroupPointOfInterestData groupData, float delay)
	{
		WgoData wgoData = MainGame.WorldData.GetWgoData(wgoUniqueId);
		if (wgoData == null)
		{
			Debug.LogError(string.Format("SendGroupHome WGO data with sguid:[{0}] not found", wgoUniqueId));
			return;
		}
		if (wgoData.GetGameRes("npc_life_sim_disabled_flag") > 0f)
		{
			return;
		}
		if (groupData == null)
		{
			groupData = this.Data.GetGroupByWGOId(wgoUniqueId);
		}
		if (groupData == null)
		{
			Debug.LogError("SendWgoHome WGO:[" + wgoData.id + "] group not found");
			return;
		}
		this.ReleaseOccupiedPointForWgo(wgoData);
		this.Data.AddActionData(new NPCLifeSimulatorActionData(wgoUniqueId, delay, NPCLifeSimulatorActionType.GoHome));
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x00036F48 File Offset: 0x00035148
	public void SendGroupHome(string groupId)
	{
		NPCGroupPointOfInterestData groupById = this.Data.GetGroupById(groupId);
		if (groupById == null)
		{
			Debug.LogError("SendGroupHome no such group:[" + groupId + "]");
			return;
		}
		int num = 0;
		float minActionDelay = LazySingletonSO<NPCLifeSimulatorConfiguration>.Instance.MinActionDelay;
		float num2 = (LazySingletonSO<NPCLifeSimulatorConfiguration>.Instance.MaxActionDelay - minActionDelay) / (float)groupById.Wgos.Count;
		foreach (SGuid sguid in groupById.Wgos)
		{
			float num3 = minActionDelay + num2 * (float)num;
			float num4 = global::UnityEngine.Random.Range(0f, num2);
			this.SendWgoHome(sguid, groupById, num3 + num4);
			num++;
		}
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x0003700C File Offset: 0x0003520C
	public void SendAllGroupsHome()
	{
		foreach (NPCGroupPointOfInterestData npcgroupPointOfInterestData in this.Data.AllGroups)
		{
			this.SendGroupHome(npcgroupPointOfInterestData.Id);
		}
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x0003706C File Offset: 0x0003526C
	public void ForceAllGroupsTeleportHome()
	{
		foreach (NPCGroupPointOfInterestData npcgroupPointOfInterestData in this.Data.AllGroups)
		{
			foreach (SGuid sguid in npcgroupPointOfInterestData.Wgos)
			{
				WgoData wgoData = MainGame.WorldData.GetWgoData(sguid);
				if (wgoData == null)
				{
					Debug.LogError(string.Format("ForceAllGroupsTeleportHome WGO data with sguid:[{0}] not found", sguid));
				}
				else
				{
					string text = wgoData.GameResStr.Get("npc_life_sim_home", "");
					if (string.IsNullOrEmpty(text))
					{
						Debug.LogError("ForceAllGroupsTeleportHome Can't send wgo:[" + wgoData.id + "] home because home point is empty!!!");
					}
					else
					{
						GDPointData gdpointDataById = MainGame.WorldData.gdPointsData.GetGDPointDataById(text);
						if (gdpointDataById == null)
						{
							Debug.LogError("ForceAllGroupsTeleportHome Can't send wgo:[" + wgoData.id + "] home because home gd point is null!!!");
						}
						else
						{
							wgoData.MovementComponent.ForceStop();
							wgoData.Position = gdpointDataById.Position;
							this.ReleaseOccupiedPointForWgo(wgoData);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x000371CC File Offset: 0x000353CC
	public void ForceGroupTeleportHome(string groupId)
	{
		foreach (NPCGroupPointOfInterestData npcgroupPointOfInterestData in this.Data.AllGroups)
		{
			if (npcgroupPointOfInterestData.Id == groupId)
			{
				foreach (SGuid sguid in npcgroupPointOfInterestData.Wgos)
				{
					WgoData wgoData = MainGame.WorldData.GetWgoData(sguid);
					if (wgoData == null)
					{
						Debug.LogError(string.Format("ForceGroupTeleportHome WGO data with sguid:[{0}] not found", sguid));
					}
					else
					{
						string text = wgoData.GameResStr.Get("npc_life_sim_home", "");
						if (string.IsNullOrEmpty(text))
						{
							Debug.LogError("ForceGroupTeleportHome Can't send wgo:[" + wgoData.id + "] home because home point is empty!!!");
						}
						else
						{
							GDPointData gdpointDataById = MainGame.WorldData.gdPointsData.GetGDPointDataById(text);
							if (gdpointDataById != null)
							{
								wgoData.MovementComponent.ForceStop();
								wgoData.Position = gdpointDataById.Position;
								this.ReleaseOccupiedPointForWgo(wgoData);
								break;
							}
							Debug.LogError("ForceGroupTeleportHome Can't send wgo:[" + wgoData.id + "] home because home gd point is null!!!");
						}
					}
				}
			}
		}
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x0003734C File Offset: 0x0003554C
	public void OnWgoReachedPointOfInterest(WgoData wgoData)
	{
		if (this.Data.GetGroupByWGOId(wgoData.UniqueId) == null)
		{
			return;
		}
		NPCPointOfInterestData pointById = this.Data.GetPointById(wgoData.occupiedPointOfInterest);
		if (pointById == null)
		{
			return;
		}
		GDPointData gdpointData = pointById.GDPointData;
		if (gdpointData != null)
		{
			wgoData.direction.Value = gdpointData.Direction.ConvertToVector2XZ();
		}
		this.ExecuteAnimationWhenReachedPointForWgo(wgoData);
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x000373AC File Offset: 0x000355AC
	public void AddWgoToGroupFromBalance(string wgoId)
	{
		WGODef data = GameBalance.Me.GetData<WGODef>(wgoId);
		if (data == null)
		{
			Debug.LogError("AddWgoToGroupFromBalance WGO def with id:[" + wgoId + "] not found");
			return;
		}
		this.AddWgoToGroup(wgoId, data.npcLifeSimGroup);
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x000373EB File Offset: 0x000355EB
	public void AddWgoToGroupFromBalance(WgoData wgoData)
	{
		if (((wgoData != null) ? wgoData.Definition : null) == null)
		{
			Debug.LogError("AddWgoToGroupFromBalance WGO data not found");
			return;
		}
		this.AddWgoToGroup(wgoData.id, wgoData.Definition.npcLifeSimGroup);
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x00037420 File Offset: 0x00035620
	public void AddWgoToGroup(string wgoId, string groupId)
	{
		this.RemoveWgoFromGroup(wgoId);
		NPCGroupPointOfInterestData groupById = this.Data.GetGroupById(groupId);
		if (groupById == null)
		{
			Debug.LogError(string.Concat(new string[] { "AddWgoToGroup WGO:[", wgoId, "] group:[", groupId, "] not found" }));
			return;
		}
		WgoData wgoData = MainGame.WorldData.GetWgoData(wgoId);
		if (wgoData == null)
		{
			Debug.LogError("AddWgoToGroup WGO data with id:[" + wgoId + "] not found");
			return;
		}
		groupById.AddWgoToGroup(wgoData);
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x000374A4 File Offset: 0x000356A4
	public void AddWgoToGroup(WgoData wgoData, string groupId)
	{
		this.RemoveWgoFromGroup(wgoData);
		NPCGroupPointOfInterestData groupById = this.Data.GetGroupById(groupId);
		if (groupById == null)
		{
			Debug.LogError("AddWgoToGroup WGO: group:[" + groupId + "] not found");
			return;
		}
		if (wgoData == null)
		{
			Debug.LogError("AddWgoToGroup WGO data not found");
			return;
		}
		groupById.AddWgoToGroup(wgoData);
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x000374F4 File Offset: 0x000356F4
	public void RemoveWgoFromGroup(string wgoId)
	{
		WgoData wgoData = MainGame.WorldData.GetWgoData(wgoId);
		if (wgoData == null)
		{
			Debug.LogError("RemoveWgoFromGroup WGO data with id:[" + wgoId + "] not found");
			return;
		}
		foreach (NPCGroupPointOfInterestData npcgroupPointOfInterestData in this.Data.AllGroups)
		{
			using (List<SGuid>.Enumerator enumerator2 = npcgroupPointOfInterestData.Wgos.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.Guid == wgoData.UniqueId.Guid)
					{
						npcgroupPointOfInterestData.RemoveWgoFromGroup(wgoData);
						return;
					}
				}
			}
		}
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x000375C4 File Offset: 0x000357C4
	public void RemoveWgoFromGroup(WgoData wgoData)
	{
		if (wgoData == null)
		{
			Debug.LogError("RemoveWgoFromGroup WGO data not found");
			return;
		}
		foreach (NPCGroupPointOfInterestData npcgroupPointOfInterestData in this.Data.AllGroups)
		{
			using (List<SGuid>.Enumerator enumerator2 = npcgroupPointOfInterestData.Wgos.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.Guid == wgoData.UniqueId.Guid)
					{
						npcgroupPointOfInterestData.RemoveWgoFromGroup(wgoData);
						return;
					}
				}
			}
		}
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x00037680 File Offset: 0x00035880
	public void UnlockPointOfInterest(string pointId)
	{
		NPCPointOfInterestData pointById = MainGame.Instance.GameSave.npcLifeSimulatorData.GetPointById(pointId);
		if (pointById == null)
		{
			Debug.LogError("UnlockPointOfInterest : Point " + pointId + " not found");
			return;
		}
		pointById.Enabled = true;
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x000376C4 File Offset: 0x000358C4
	public void LockPointOfInterest(string pointId)
	{
		NPCPointOfInterestData pointById = MainGame.Instance.GameSave.npcLifeSimulatorData.GetPointById(pointId);
		if (pointById == null)
		{
			Debug.LogError("LockPointOfInterest : Point " + pointId + " not found");
			return;
		}
		pointById.Enabled = false;
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x00037708 File Offset: 0x00035908
	private void ReleaseOccupiedPointForWgo(WgoData wgoData)
	{
		if (string.IsNullOrEmpty(wgoData.occupiedPointOfInterest))
		{
			return;
		}
		NPCPointOfInterestData pointById = this.Data.GetPointById(wgoData.occupiedPointOfInterest);
		if (pointById != null)
		{
			this.ExecuteAnimationWhenLeavedPointForWgo(wgoData);
			pointById.Deoccupy();
		}
		wgoData.occupiedPointOfInterest = string.Empty;
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x00037750 File Offset: 0x00035950
	private void CheckActionsForWgos(float deltaTime)
	{
		for (int i = this.Data.ActionsData.Count - 1; i >= 0; i--)
		{
			NPCLifeSimulatorActionData npclifeSimulatorActionData = this.Data.ActionsData[i];
			npclifeSimulatorActionData.RemainingTimeToAction -= deltaTime;
			if (npclifeSimulatorActionData.RemainingTimeToAction <= 0f)
			{
				WgoData wgoData = MainGame.WorldData.GetWgoData(npclifeSimulatorActionData.WgoId);
				if (wgoData == null)
				{
					Debug.LogError(string.Format("CheckActionsForWgos WGO data with sguid:[{0}] not found", npclifeSimulatorActionData.WgoId));
					this.Data.ActionsData.RemoveAt(i);
				}
				else
				{
					NPCLifeSimulatorActionType actionType = npclifeSimulatorActionData.ActionType;
					if (actionType != NPCLifeSimulatorActionType.GoToPointOfInterest)
					{
						if (actionType != NPCLifeSimulatorActionType.GoHome)
						{
							throw new ArgumentOutOfRangeException();
						}
						this.ReleaseOccupiedPointForWgo(wgoData);
						string text = wgoData.GameResStr.Get("npc_life_sim_home", "");
						if (string.IsNullOrEmpty(text))
						{
							Debug.LogError("CheckActionsForWgos Can't send wgo:[" + wgoData.id + "] home because home point is empty!!!");
							this.Data.ActionsData.RemoveAt(i);
							goto IL_020B;
						}
						GDPointData gdpointDataById = MainGame.WorldData.gdPointsData.GetGDPointDataById(text);
						if (gdpointDataById == null)
						{
							Debug.LogError("CheckActionsForWgos Can't send wgo:[" + wgoData.id + "] home because home gd point is null!!!");
							this.Data.ActionsData.RemoveAt(i);
							goto IL_020B;
						}
						if (wgoData.MovementComponent.IsMoving)
						{
							wgoData.MovementComponent.ForceStop();
						}
						wgoData.MovementComponent.StartPath(gdpointDataById.Position, wgoData.WorldId, gdpointDataById.GameSceneDataId, MovementType.GDGraph, 1.5f, "", null, null, MovementComponent.DestinationType.Position);
					}
					else
					{
						NPCPointOfInterestData pointById = this.Data.GetPointById(wgoData.occupiedPointOfInterest);
						if (pointById == null)
						{
							Debug.LogError("CheckActionsForWgos WGO:[" + wgoData.id + "] can't find point");
							this.Data.ActionsData.RemoveAt(i);
							goto IL_020B;
						}
						if (wgoData.MovementComponent.IsMoving)
						{
							wgoData.MovementComponent.ForceStop();
						}
						wgoData.MovementComponent.StartPath(pointById, 1.125f);
					}
					this.Data.ActionsData.RemoveAt(i);
				}
			}
			IL_020B:;
		}
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x00037974 File Offset: 0x00035B74
	private void CheckAnimationsRollForWgos(float deltaTime)
	{
		for (int i = this.Data.AnimationDatas.Count - 1; i >= 0; i--)
		{
			NPCPointOfInterestAnimationData npcpointOfInterestAnimationData = this.Data.AnimationDatas[i];
			npcpointOfInterestAnimationData.RemainingTimeToRoll -= deltaTime;
			if (npcpointOfInterestAnimationData.RemainingTimeToRoll <= 0f)
			{
				WgoData wgoData = MainGame.WorldData.GetWgoData(npcpointOfInterestAnimationData.WgoId);
				if (wgoData == null)
				{
					Debug.LogError(string.Format("Update rolling animations WGO data with sguid:[{0}] not found", npcpointOfInterestAnimationData.WgoId));
					this.Data.AnimationDatas.RemoveAt(i);
				}
				else
				{
					this.ExecuteAnimationWhenReachedPointForWgo(wgoData);
				}
			}
		}
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x00037A10 File Offset: 0x00035C10
	private void ExecuteAnimationWhenReachedPointForWgo(WgoData wgoData)
	{
		if (this.Data.GetGroupByWGOId(wgoData.UniqueId) == null)
		{
			Debug.LogError("ExecuteAnimationWhenReachedPointForWgo WGO:[" + wgoData.id + "] group not found");
			return;
		}
		NPCPointOfInterestData pointById = this.Data.GetPointById(wgoData.occupiedPointOfInterest);
		if (pointById == null)
		{
			Debug.LogError("ExecuteAnimationWhenReachedPointForWgo WGO:[" + wgoData.id + "] no current point");
			return;
		}
		NPCPointOfInterestConfiguration configuration = pointById.Configuration;
		if (configuration == null)
		{
			Debug.LogError("ExecuteAnimationWhenReachedPointForWgo WGO:[" + wgoData.id + "] no config for point");
			return;
		}
		switch (configuration.AnimationType)
		{
		case NPCPointOfInterestAnimationType.None:
			break;
		case NPCPointOfInterestAnimationType.Roll:
			if (configuration.AnimationsForRoll.Count > 0)
			{
				NPCPointOfInterestAnimationConfiguration random = configuration.AnimationsForRoll.GetRandom<NPCPointOfInterestAnimationConfiguration>();
				this.Data.AddAnimationData(new NPCPointOfInterestAnimationData(wgoData, random));
				wgoData.SetTriggerToAnimator(random.TriggerId);
				wgoData.SetCustomAnimationTrigger(random.TriggerId);
				return;
			}
			break;
		case NPCPointOfInterestAnimationType.TriggerCustomIdle:
			if (!string.IsNullOrEmpty(configuration.IdleTriggerId))
			{
				wgoData.SetTriggerToAnimator(configuration.IdleTriggerId);
				wgoData.SetCustomAnimationTrigger(configuration.IdleTriggerId);
				return;
			}
			Debug.LogError("#npc_sim# ExecuteAnimationWhenReachedPointForWgo wgoData:[" + wgoData.id + "] configuration.IdleTriggerId is empty!!!");
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x00037B4C File Offset: 0x00035D4C
	private void ExecuteAnimationWhenLeavedPointForWgo(WgoData wgoData)
	{
		if (this.Data.GetGroupByWGOId(wgoData.UniqueId) == null)
		{
			Debug.LogError("ExecuteAnimationWhenLeavedPointForWgo WGO:[" + wgoData.id + "] group not found");
			return;
		}
		NPCPointOfInterestData pointById = this.Data.GetPointById(wgoData.occupiedPointOfInterest);
		if (pointById == null)
		{
			Debug.LogError("ExecuteAnimationWhenLeavedPointForWgo WGO:[" + wgoData.id + "] no current point");
			return;
		}
		NPCPointOfInterestConfiguration configuration = pointById.Configuration;
		if (configuration == null)
		{
			Debug.LogError("ExecuteAnimationWhenLeavedPointForWgo WGO:[" + wgoData.id + "] no config for point");
			return;
		}
		switch (configuration.AnimationType)
		{
		case NPCPointOfInterestAnimationType.None:
			return;
		case NPCPointOfInterestAnimationType.Roll:
			wgoData.SetCustomAnimationTrigger("");
			this.Data.TryRemoveAnimationData(wgoData.UniqueId);
			return;
		case NPCPointOfInterestAnimationType.TriggerCustomIdle:
			wgoData.SetCustomAnimationTrigger("");
			wgoData.SetTriggerToAnimator(AnimationComponentBase.RESET_TO_IDLE_TRIGGER);
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x04000C56 RID: 3158
	private int curRandomOffset;
}
