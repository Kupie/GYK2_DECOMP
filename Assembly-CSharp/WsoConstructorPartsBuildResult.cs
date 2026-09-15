using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x0200069E RID: 1694
public class WsoConstructorPartsBuildResult
{
	// Token: 0x0400246D RID: 9325
	public readonly List<ConstructorPart> Parts = new List<ConstructorPart>();

	// Token: 0x0400246E RID: 9326
	public readonly List<AsyncOperationHandle<Texture2D>> LutHandles = new List<AsyncOperationHandle<Texture2D>>();
}
