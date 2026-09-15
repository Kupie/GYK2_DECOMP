using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002A4 RID: 676
public class Event_03_GoToDir : AgentAI
{
	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x06001140 RID: 4416 RVA: 0x0005703B File Offset: 0x0005523B
	// (set) Token: 0x06001141 RID: 4417 RVA: 0x00057043 File Offset: 0x00055243
	public Vector3 Direction { get; set; }

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06001142 RID: 4418 RVA: 0x0005704C File Offset: 0x0005524C
	// (set) Token: 0x06001143 RID: 4419 RVA: 0x00057054 File Offset: 0x00055254
	public float Distance { get; set; }

	// Token: 0x06001144 RID: 4420 RVA: 0x00057060 File Offset: 0x00055260
	public override MobCommand GetCommand(FightingAgent agent, Func<IEnumerable<ICombatEntity>> targets)
	{
		Vector3 vector = agent.Wgo.Data.Position + this.Direction * this.Distance;
		return new MobCommandGoTo(new CombatEntityDestinationModifier(null)).ToPosition(vector);
	}
}
