using System;
using UnityEngine;

// Token: 0x020002DE RID: 734
public interface ICombatEntity
{
	// Token: 0x17000337 RID: 823
	// (get) Token: 0x0600132B RID: 4907
	SGuid CombatEntityUID { get; }

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x0600132C RID: 4908
	LazyConsts.Fighting.TeamType TeamType { get; }

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x0600132D RID: 4909
	LazyConsts.Fighting.EntityType EntityType { get; }

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x0600132E RID: 4910
	Vector3 CombatEntityPosition { get; }

	// Token: 0x0600132F RID: 4911
	float GetCombatEntityDistance(Vector3 from, LazyConsts.Fighting.TeamType teamType);

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x06001330 RID: 4912
	HPComponent CombatEntityHpComponent { get; }

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x06001331 RID: 4913
	int ArmorValue { get; }

	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06001332 RID: 4914
	int CombatEntityQuality { get; }

	// Token: 0x06001333 RID: 4915
	float GetCombatEntityGameRes(string resId);

	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06001334 RID: 4916
	// (set) Token: 0x06001335 RID: 4917
	int AttackPriority { get; set; }

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06001336 RID: 4918
	bool HasAnyDockPoint { get; }

	// Token: 0x06001337 RID: 4919
	void OnOtherCombatTargetReachedToMe(ICombatEntity other);

	// Token: 0x06001338 RID: 4920
	void OnOtherCombatTargetHitMe(AttackContext ctx);

	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06001339 RID: 4921
	// (set) Token: 0x0600133A RID: 4922
	bool IsActiveCombatant { get; set; }
}
