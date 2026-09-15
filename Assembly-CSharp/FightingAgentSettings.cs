using System;
using UnityEngine;

// Token: 0x020002DA RID: 730
[CreateAssetMenu(fileName = "FightingAgentSettings", menuName = "GK2/Fighting/FightingAgentSettings")]
public class FightingAgentSettings : ScriptableObject
{
	// Token: 0x04001475 RID: 5237
	[Space]
	public float aiPathRadius = 0.22f;

	// Token: 0x04001476 RID: 5238
	public float aiPathHeight = 2f;

	// Token: 0x04001477 RID: 5239
	[Space]
	public bool enableRvoDensityBehavior;

	// Token: 0x04001478 RID: 5240
	[Range(0f, 1f)]
	public float rvoDensityBehaviorDensityThreshold = 0.5f;

	// Token: 0x04001479 RID: 5241
	public bool useDockPointPrioritizationByWeapon;

	// Token: 0x0400147A RID: 5242
	public float defaultRvoPriority = 0.5f;

	// Token: 0x0400147B RID: 5243
	[Space]
	public bool slowWhenNotFacingTarget;

	// Token: 0x0400147C RID: 5244
	public bool preventMovingBackwards;

	// Token: 0x0400147D RID: 5245
	public float wallForce = 3f;

	// Token: 0x0400147E RID: 5246
	public float wallDist = 1f;

	// Token: 0x0400147F RID: 5247
	[Range(0.1f, 4f)]
	public float rvoAgentTimeHorizon = 0.3f;

	// Token: 0x04001480 RID: 5248
	[Range(0.1f, 1f)]
	public float rvoAgentObstacleTimeHorizon = 0.15f;

	// Token: 0x04001481 RID: 5249
	[Range(4f, 32f)]
	public int rvoMaxNeighbours = 10;

	// Token: 0x04001482 RID: 5250
	[Range(0f, 1f)]
	public float mainHeroPushAllyRvoPriority = 0.15f;

	// Token: 0x04001483 RID: 5251
	[Range(0.05f, 2f)]
	public float mainHeroPushHoldTime = 0.35f;
}
