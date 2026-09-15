using System;
using System.Collections.Generic;
using LinqTools;
using UnityEngine;

// Token: 0x0200029F RID: 671
[CreateAssetMenu(menuName = "GK2/Fighting/AIs/EnemyWithDoorAI")]
public class EnemyWithDoorAI : EnemyDefaultAI
{
	// Token: 0x06001134 RID: 4404 RVA: 0x00056EB0 File Offset: 0x000550B0
	protected override EnemyDefaultAI.EnemyDecisionContext BuildContext(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets)
	{
		Func<ICombatEntity, bool> <>9__2;
		Func<IEnumerable<ICombatEntity>> func = delegate
		{
			IEnumerable<ICombatEntity> enumerable = targets();
			Func<ICombatEntity, bool> func2;
			if ((func2 = <>9__2) == null)
			{
				func2 = (<>9__2 = delegate(ICombatEntity t)
				{
					Wgo tWgo = t as Wgo;
					return tWgo != null && this.objGroupsToAttack.Any((string g) => tWgo.Data.Definition.wgoGroup.Contains(g));
				});
			}
			return enumerable.Where(func2);
		};
		ICombatEntity combatEntity = base.FindFrontmostTargetOnLine(agent, agent.ParentController.FightingLine, false, delegate(ICombatEntity t)
		{
			Wgo tWgo = t as Wgo;
			return tWgo != null && this.objGroupsToAttack.Any((string g) => tWgo.Data.Definition.wgoGroup.Contains(g));
		});
		return new EnemyDefaultAI.EnemyDecisionContext(this, agent, this.WrapPotentialTargets(agent, func), this.aggroDistance, combatEntity);
	}

	// Token: 0x0400133C RID: 4924
	[SerializeField]
	private List<string> objGroupsToAttack = new List<string>();

	// Token: 0x020002A0 RID: 672
	public readonly struct EnemyWithDoorContext
	{
		// Token: 0x06001136 RID: 4406 RVA: 0x00056F27 File Offset: 0x00055127
		public EnemyWithDoorContext(EnemyDefaultAI.EnemyDecisionContext baseContext)
		{
			this.Base = baseContext;
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x00056F30 File Offset: 0x00055130
		public EnemyDefaultAI.EnemyDecisionContext Base { get; }
	}
}
