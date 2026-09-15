using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x020006A6 RID: 1702
public class WsoOptimizedStagesBuildResult
{
	// Token: 0x04002487 RID: 9351
	public readonly List<GameObject> Instances = new List<GameObject>();

	// Token: 0x04002488 RID: 9352
	public readonly List<AsyncOperationHandle<GameObject>> Handles = new List<AsyncOperationHandle<GameObject>>();
}
