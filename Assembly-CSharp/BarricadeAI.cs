using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000288 RID: 648
[CreateAssetMenu(menuName = "GK2/Fighting/AIs/Barricade")]
public class BarricadeAI : AgentAI
{
	// Token: 0x060010C8 RID: 4296 RVA: 0x00054915 File Offset: 0x00052B15
	public override MobCommand GetCommand(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets)
	{
		return null;
	}
}
