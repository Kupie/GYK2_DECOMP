using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02000289 RID: 649
[CreateAssetMenu(menuName = "GK2/Fighting/AIs/DevTestPlayerAttack")]
public class DevTestPlayerAttackAI : AgentAI
{
	// Token: 0x060010CA RID: 4298 RVA: 0x00054920 File Offset: 0x00052B20
	[CanBeNull]
	public override MobCommand GetCommand(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets)
	{
		PlayerController playerController = MainGame.PlayerController;
		ICombatEntity combatEntity = ((playerController != null) ? playerController.PhysicalBody : null);
		if (combatEntity == null)
		{
			return null;
		}
		if (combatEntity.CombatEntityHpComponent == null || combatEntity.CombatEntityHpComponent.Hp <= 0)
		{
			return null;
		}
		return this.DoGoAndAttackPlayer(agent, combatEntity);
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x00054964 File Offset: 0x00052B64
	private MobCommand DoGoAndAttackPlayer(FightingAgent agent, ICombatEntity player)
	{
		float num = this.DistToPos(agent, player.CombatEntityPosition);
		if ((num - this.attackDistance).More(0f, 0.0001f))
		{
			return new MobCommandGoTo(new CombatEntityDestinationModifier(null)).WithCustomTargetDestinationOffset(this.attackDistance).ToTarget(player);
		}
		if (num.More(0f, 0.0001f))
		{
			return new ZombieMeleeAttackCommand(this.attackDistance).WithDamage(this.meleeDamage).ToTarget(player);
		}
		return null;
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x000549E8 File Offset: 0x00052BE8
	private float DistToPos(FightingAgent agent, Vector3 pos)
	{
		return (agent.Wgo.Data.Position - pos).XZ().magnitude;
	}

	// Token: 0x040012F0 RID: 4848
	private const float TINY_EPSILON = 0.0001f;

	// Token: 0x040012F1 RID: 4849
	public float attackDistance = 1f;

	// Token: 0x040012F2 RID: 4850
	public int meleeDamage = 10;
}
