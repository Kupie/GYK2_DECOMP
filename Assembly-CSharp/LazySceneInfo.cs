using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

// Token: 0x02000715 RID: 1813
public class LazySceneInfo
{
	// Token: 0x04002686 RID: 9862
	public string sceneId;

	// Token: 0x04002687 RID: 9863
	public SceneStatus status;

	// Token: 0x04002688 RID: 9864
	public SceneInstance sceneInstance;

	// Token: 0x04002689 RID: 9865
	public AsyncOperationHandle sceneHandle;

	// Token: 0x0400268A RID: 9866
	public Action<SceneInstance> onLoadedCallback;
}
