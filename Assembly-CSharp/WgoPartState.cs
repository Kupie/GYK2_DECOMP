using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000698 RID: 1688
[Serializable]
public class WgoPartState
{
	// Token: 0x0400242D RID: 9261
	public string variationId;

	// Token: 0x0400242E RID: 9262
	public int rotationIndex;

	// Token: 0x0400242F RID: 9263
	public GameObject gameObject;

	// Token: 0x04002430 RID: 9264
	public bool mirror;

	// Token: 0x04002431 RID: 9265
	public bool isDefault;

	// Token: 0x04002432 RID: 9266
	public Transform customBubblePoint;

	// Token: 0x04002433 RID: 9267
	public List<UnityEvent> customEvents;
}
