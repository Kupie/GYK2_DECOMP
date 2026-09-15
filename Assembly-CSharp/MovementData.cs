using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200036B RID: 875
[Serializable]
public class MovementData
{
	// Token: 0x0400173D RID: 5949
	public List<Vector3> worldPath = new List<Vector3>();

	// Token: 0x0400173E RID: 5950
	public string transitionWorldId;

	// Token: 0x0400173F RID: 5951
	public bool lastPointTransitToNextMovementData;
}
