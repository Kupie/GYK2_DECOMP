using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000302 RID: 770
[Serializable]
public class FightingTargetsDatabase
{
	// Token: 0x14000020 RID: 32
	// (add) Token: 0x0600149B RID: 5275 RVA: 0x00065258 File Offset: 0x00063458
	// (remove) Token: 0x0600149C RID: 5276 RVA: 0x00065290 File Offset: 0x00063490
	public event Action<TargetInfo> OnTargetAdded;

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x0600149D RID: 5277 RVA: 0x000652C8 File Offset: 0x000634C8
	// (remove) Token: 0x0600149E RID: 5278 RVA: 0x00065300 File Offset: 0x00063500
	public event Action<TargetInfo> OnTargetRemoved;

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x0600149F RID: 5279 RVA: 0x00065335 File Offset: 0x00063535
	public IReadOnlyList<TargetInfo> AllTargets
	{
		get
		{
			return this.allTargets;
		}
	}

	// Token: 0x060014A0 RID: 5280 RVA: 0x00065340 File Offset: 0x00063540
	public FightingTargetsDatabase()
	{
		foreach (object obj in Enum.GetValues(typeof(LazyConsts.Fighting.TeamType)))
		{
			LazyConsts.Fighting.TeamType teamType = (LazyConsts.Fighting.TeamType)obj;
			this.cachedTargetsByTeam[teamType] = new List<ICombatEntity>();
		}
	}

	// Token: 0x060014A1 RID: 5281 RVA: 0x000653C8 File Offset: 0x000635C8
	public IReadOnlyList<ICombatEntity> GetTargetsByTeam(LazyConsts.Fighting.TeamType team)
	{
		List<ICombatEntity> list;
		if (!this.cachedTargetsByTeam.TryGetValue(team, out list))
		{
			return Array.Empty<ICombatEntity>();
		}
		return list;
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x000653F0 File Offset: 0x000635F0
	public int GetTargetCountByTeam(LazyConsts.Fighting.TeamType team)
	{
		List<ICombatEntity> list;
		if (!this.cachedTargetsByTeam.TryGetValue(team, out list))
		{
			return 0;
		}
		return list.Count;
	}

	// Token: 0x060014A3 RID: 5283 RVA: 0x00065418 File Offset: 0x00063618
	public TargetInfo GetTargetInfo(ICombatEntity entity)
	{
		return this.allTargets.FirstOrDefault((TargetInfo t) => t.entity == entity);
	}

	// Token: 0x060014A4 RID: 5284 RVA: 0x0006544C File Offset: 0x0006364C
	public void Register(ICombatEntity entity, int lineId = -1, int sectorId = -1, bool updateInfoIfExists = true, bool isPersistent = false)
	{
		TargetInfo targetInfo = this.allTargets.FirstOrDefault((TargetInfo t) => t.entity == entity);
		if (targetInfo != null && updateInfoIfExists)
		{
			targetInfo.LineId = lineId;
			targetInfo.SectorId = sectorId;
			return;
		}
		LazyConsts.Fighting.TeamType teamType = entity.TeamType;
		targetInfo = new TargetInfo(entity, teamType, lineId, sectorId, isPersistent);
		this.allTargets.Add(targetInfo);
		List<ICombatEntity> list;
		if (this.cachedTargetsByTeam.TryGetValue(teamType, out list))
		{
			list.Add(entity);
		}
		Action<TargetInfo> onTargetAdded = this.OnTargetAdded;
		if (onTargetAdded == null)
		{
			return;
		}
		onTargetAdded(targetInfo);
	}

	// Token: 0x060014A5 RID: 5285 RVA: 0x000654EC File Offset: 0x000636EC
	public void Unregister(ICombatEntity entity)
	{
		TargetInfo targetInfo = this.GetTargetInfo(entity);
		if (targetInfo != null)
		{
			this.allTargets.Remove(targetInfo);
			List<ICombatEntity> list;
			if (this.cachedTargetsByTeam.TryGetValue(targetInfo.Team, out list))
			{
				list.Remove(entity);
			}
			Action<TargetInfo> onTargetRemoved = this.OnTargetRemoved;
			if (onTargetRemoved == null)
			{
				return;
			}
			onTargetRemoved(targetInfo);
		}
	}

	// Token: 0x060014A6 RID: 5286 RVA: 0x00065540 File Offset: 0x00063740
	public void Unregister(ICombatEntity entity, int lineId, int sectorId)
	{
		TargetInfo targetInfo = this.GetTargetInfo(entity);
		if (targetInfo != null && targetInfo.LineId == lineId && targetInfo.SectorId == sectorId)
		{
			this.allTargets.Remove(targetInfo);
			List<ICombatEntity> list;
			if (this.cachedTargetsByTeam.TryGetValue(targetInfo.Team, out list))
			{
				list.Remove(entity);
			}
			Action<TargetInfo> onTargetRemoved = this.OnTargetRemoved;
			if (onTargetRemoved == null)
			{
				return;
			}
			onTargetRemoved(targetInfo);
		}
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x000655A8 File Offset: 0x000637A8
	public bool IsTargetOnLine(ICombatEntity entity, int lineId)
	{
		TargetInfo targetInfo = this.GetTargetInfo(entity);
		return targetInfo != null && targetInfo.LineId == lineId;
	}

	// Token: 0x060014A8 RID: 5288 RVA: 0x000655CC File Offset: 0x000637CC
	public int GetEnemyCountOnLine(int lineId)
	{
		return this.allTargets.Count((TargetInfo t) => t.Team == LazyConsts.Fighting.TeamType.WildZombie && t.LineId == lineId);
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x00065600 File Offset: 0x00063800
	public IEnumerable<ICombatEntity> GetTargets(int lineId = -1, int sectorId = -1)
	{
		return from t in this.allTargets
			where (lineId == -1 || t.LineId == lineId) && (sectorId == -1 || t.SectorId == sectorId)
			select t.entity;
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x0006565C File Offset: 0x0006385C
	public static bool IsCombatEntityAlive(ICombatEntity entity)
	{
		if (entity == null)
		{
			return false;
		}
		global::UnityEngine.Object @object = entity as global::UnityEngine.Object;
		return @object == null || !(@object == null);
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x00065684 File Offset: 0x00063884
	public void RemoveEntry(TargetInfo targetInfo)
	{
		if (targetInfo == null || !this.allTargets.Remove(targetInfo))
		{
			return;
		}
		List<ICombatEntity> list;
		if (this.cachedTargetsByTeam.TryGetValue(targetInfo.Team, out list))
		{
			list.Remove(targetInfo.entity);
		}
		Action<TargetInfo> onTargetRemoved = this.OnTargetRemoved;
		if (onTargetRemoved == null)
		{
			return;
		}
		onTargetRemoved(targetInfo);
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x000656D8 File Offset: 0x000638D8
	public bool PruneDeadTargets(Action<ICombatEntity> onRemoveCallback)
	{
		bool flag = false;
		for (int i = this.allTargets.Count - 1; i >= 0; i--)
		{
			TargetInfo targetInfo = this.allTargets[i];
			ICombatEntity entity = targetInfo.entity;
			bool flag2 = !FightingTargetsDatabase.IsCombatEntityAlive(entity);
			bool flag3 = FightingTargetsDatabase.IsCombatEntityAlive(entity) && entity.CombatEntityHpComponent.Hp <= 0 && entity.CombatEntityHpComponent.WasDamagedAtLeastOnce;
			if (flag2 || flag3)
			{
				if (flag2)
				{
					Debug.Log(string.Format("Pruned orphan target: {0}", targetInfo.Team));
				}
				else
				{
					Wgo wgo = entity as Wgo;
					string text = ((wgo != null) ? (" WgoId: " + wgo.Id) : string.Empty);
					Debug.Log(string.Format("Pruned Dead Target: {0}", entity.CombatEntityUID) + text);
				}
				if (onRemoveCallback != null)
				{
					onRemoveCallback(entity);
				}
				this.cachedTargetsByTeam[targetInfo.Team].Remove(targetInfo.entity);
				this.allTargets.RemoveAt(i);
				flag = true;
				Action<TargetInfo> onTargetRemoved = this.OnTargetRemoved;
				if (onTargetRemoved != null)
				{
					onTargetRemoved(targetInfo);
				}
			}
		}
		return flag;
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x00065800 File Offset: 0x00063A00
	public void Clear()
	{
		this.allTargets.Clear();
		foreach (List<ICombatEntity> list in this.cachedTargetsByTeam.Values)
		{
			list.Clear();
		}
	}

	// Token: 0x0400156C RID: 5484
	private readonly List<TargetInfo> allTargets = new List<TargetInfo>();

	// Token: 0x0400156D RID: 5485
	private readonly Dictionary<LazyConsts.Fighting.TeamType, List<ICombatEntity>> cachedTargetsByTeam = new Dictionary<LazyConsts.Fighting.TeamType, List<ICombatEntity>>();
}
