using System;

// Token: 0x02000301 RID: 769
[Serializable]
public class TargetInfo
{
	// Token: 0x17000384 RID: 900
	// (get) Token: 0x06001492 RID: 5266 RVA: 0x000651E5 File Offset: 0x000633E5
	public ICombatEntity entity { get; }

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x06001493 RID: 5267 RVA: 0x000651ED File Offset: 0x000633ED
	public LazyConsts.Fighting.TeamType Team { get; }

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x06001494 RID: 5268 RVA: 0x000651F5 File Offset: 0x000633F5
	// (set) Token: 0x06001495 RID: 5269 RVA: 0x000651FD File Offset: 0x000633FD
	public int LineId { get; set; }

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x06001496 RID: 5270 RVA: 0x00065206 File Offset: 0x00063406
	// (set) Token: 0x06001497 RID: 5271 RVA: 0x0006520E File Offset: 0x0006340E
	public int SectorId { get; set; }

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x06001498 RID: 5272 RVA: 0x00065217 File Offset: 0x00063417
	// (set) Token: 0x06001499 RID: 5273 RVA: 0x0006521F File Offset: 0x0006341F
	public bool IsPersistent { get; set; }

	// Token: 0x0600149A RID: 5274 RVA: 0x00065228 File Offset: 0x00063428
	public TargetInfo(ICombatEntity entity, LazyConsts.Fighting.TeamType team, int lineId = -1, int sectorId = -1, bool isPersistent = false)
	{
		this.entity = entity;
		this.Team = team;
		this.LineId = lineId;
		this.SectorId = sectorId;
		this.IsPersistent = isPersistent;
	}
}
