using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x020006F2 RID: 1778
public class ChunkableObjectComponentStatic3DObjectAssetReference : ChunkableObjectComponent
{
	// Token: 0x06002EFA RID: 12026 RVA: 0x000E06A4 File Offset: 0x000DE8A4
	protected override void ApplyVisibility()
	{
		bool shouldBeActive = base.ShouldBeActive;
		bool? flag = this.lastAppliedShouldBeActive;
		bool flag2 = shouldBeActive;
		if ((flag.GetValueOrDefault() == flag2) & (flag != null))
		{
			return;
		}
		this.lastAppliedShouldBeActive = new bool?(shouldBeActive);
		Debug.Log(string.Format("[{0}] ApplyVisibility: {1} {2}", "ChunkableObjectComponentStatic3DObjectAssetReference", base.name, shouldBeActive));
		if (shouldBeActive)
		{
			this.LoadAssetAsync().Forget();
			return;
		}
		this.UnloadAsset();
	}

	// Token: 0x06002EFB RID: 12027 RVA: 0x000E071A File Offset: 0x000DE91A
	private void Awake()
	{
		if (base.GetComponentInParent<ChunkableObjectComponentStatic3DObjectAssetReference>())
		{
			return;
		}
		this.StripEmbeddedObject3DHierarchy();
	}

	// Token: 0x06002EFC RID: 12028 RVA: 0x000E0730 File Offset: 0x000DE930
	public void StripEmbeddedObject3DHierarchy()
	{
		Object3D object3D;
		if (!base.TryGetComponent<Object3D>(out object3D))
		{
			return;
		}
		ChunkableObjectComponentStatic3DObjectAssetReference.DestroyObjectInternal(object3D);
		this.object3D = null;
		for (int i = base.transform.childCount - 1; i >= 0; i--)
		{
			ChunkableObjectComponentStatic3DObjectAssetReference.DestroyObjectInternal(base.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x06002EFD RID: 12029 RVA: 0x00002318 File Offset: 0x00000518
	public override void CalculateChunkBounds()
	{
	}

	// Token: 0x06002EFE RID: 12030 RVA: 0x000E0783 File Offset: 0x000DE983
	private static void DestroyObjectInternal(global::UnityEngine.Object obj)
	{
		if (!obj)
		{
			return;
		}
		global::UnityEngine.Object.Destroy(obj);
	}

	// Token: 0x06002EFF RID: 12031 RVA: 0x000E0794 File Offset: 0x000DE994
	private UniTaskVoid LoadAssetAsync()
	{
		ChunkableObjectComponentStatic3DObjectAssetReference.<LoadAssetAsync>d__10 <LoadAssetAsync>d__;
		<LoadAssetAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<LoadAssetAsync>d__.<>4__this = this;
		<LoadAssetAsync>d__.<>1__state = -1;
		<LoadAssetAsync>d__.<>t__builder.Start<ChunkableObjectComponentStatic3DObjectAssetReference.<LoadAssetAsync>d__10>(ref <LoadAssetAsync>d__);
		return <LoadAssetAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06002F00 RID: 12032 RVA: 0x000E07D7 File Offset: 0x000DE9D7
	private void UnloadAsset()
	{
		this.loadVersion++;
		if (this.loadedInstanceHandle.IsValid())
		{
			Addressables.ReleaseInstance(this.loadedInstanceHandle);
		}
		this.loadedInstanceHandle = default(AsyncOperationHandle<GameObject>);
		this.object3D = null;
	}

	// Token: 0x06002F01 RID: 12033 RVA: 0x000E0813 File Offset: 0x000DEA13
	private void ReleaseHandleIfCurrent(AsyncOperationHandle<GameObject> handle)
	{
		if (!this.loadedInstanceHandle.Equals(handle))
		{
			return;
		}
		if (handle.IsValid())
		{
			Addressables.ReleaseInstance(handle);
		}
		this.loadedInstanceHandle = default(AsyncOperationHandle<GameObject>);
		this.object3D = null;
	}

	// Token: 0x06002F02 RID: 12034 RVA: 0x000E0847 File Offset: 0x000DEA47
	private void OnDestroy()
	{
		this.UnloadAsset();
	}

	// Token: 0x040025E7 RID: 9703
	[SerializeField]
	private AssetReferenceGameObject assetReference;

	// Token: 0x040025E8 RID: 9704
	private AsyncOperationHandle<GameObject> loadedInstanceHandle;

	// Token: 0x040025E9 RID: 9705
	private int loadVersion;

	// Token: 0x040025EA RID: 9706
	private Object3D object3D;

	// Token: 0x040025EB RID: 9707
	private bool? lastAppliedShouldBeActive;
}
