using System;
using UnityEngine;

// Token: 0x02000313 RID: 787
public class FightingCapturePointCollider : MonoBehaviour
{
	// Token: 0x06001503 RID: 5379 RVA: 0x00066CA0 File Offset: 0x00064EA0
	private void Awake()
	{
		this.capturePoint = base.GetComponentInParent<FightingCapturePoint>();
	}

	// Token: 0x06001504 RID: 5380 RVA: 0x00066CB0 File Offset: 0x00064EB0
	private void OnTriggerEnter(Collider other)
	{
		ICombatEntity componentInParent = other.GetComponentInParent<ICombatEntity>();
		if (componentInParent != null && componentInParent.IsActiveCombatant)
		{
			LazyConsts.Fighting.TeamType teamType = componentInParent.TeamType;
			if (teamType != LazyConsts.Fighting.TeamType.Player)
			{
				if (teamType == LazyConsts.Fighting.TeamType.WildZombie)
				{
					if (this.capturePoint.enemies.Contains(componentInParent))
					{
						return;
					}
					this.capturePoint.enemies.Add(componentInParent);
				}
			}
			else
			{
				if (this.capturePoint.allies.Contains(componentInParent))
				{
					return;
				}
				this.capturePoint.allies.Add(componentInParent);
			}
			this.capturePoint.CustomUpdate();
		}
	}

	// Token: 0x06001505 RID: 5381 RVA: 0x00066D38 File Offset: 0x00064F38
	private void OnTriggerExit(Collider other)
	{
		ICombatEntity componentInParent = other.GetComponentInParent<ICombatEntity>();
		if (componentInParent != null && (this.capturePoint.allies.Remove(componentInParent) || this.capturePoint.enemies.Remove(componentInParent)))
		{
			this.capturePoint.CustomUpdate();
		}
	}

	// Token: 0x040015C2 RID: 5570
	private FightingCapturePoint capturePoint;
}
