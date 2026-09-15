using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

// Token: 0x020007D3 RID: 2003
[ExecuteInEditMode]
[RequireComponent(typeof(Image))]
public class AssetReferenceImage : MonoBehaviour
{
	// Token: 0x170007BF RID: 1983
	// (get) Token: 0x06003380 RID: 13184 RVA: 0x000F8CB8 File Offset: 0x000F6EB8
	private Image Image
	{
		get
		{
			if (this.image == null)
			{
				this.image = base.GetComponent<Image>();
			}
			return this.image;
		}
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x000F8CDA File Offset: 0x000F6EDA
	private void OnEnable()
	{
		this.TryLoad();
	}

	// Token: 0x06003382 RID: 13186 RVA: 0x000F8CE2 File Offset: 0x000F6EE2
	private void OnDisable()
	{
		this.TryUnload();
	}

	// Token: 0x06003383 RID: 13187 RVA: 0x000F8CEC File Offset: 0x000F6EEC
	private void TryLoad()
	{
		if (this.assetReference == null || string.IsNullOrEmpty(this.assetReference.AssetGUID))
		{
			return;
		}
		if (this.loadHandle.IsValid())
		{
			return;
		}
		Sprite sprite;
		try
		{
			this.loadHandle = this.assetReference.LoadAssetAsync<Sprite>();
			sprite = this.loadHandle.WaitForCompletion();
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Failed to load sprite from AssetReference '{0}': {1}", this.assetReference.RuntimeKey, ex.Message), this);
			this.TryUnload();
			return;
		}
		if (sprite == null)
		{
			Debug.LogError(string.Format("Failed to load sprite from AssetReference '{0}'", this.assetReference.RuntimeKey), this);
			this.TryUnload();
			return;
		}
		this.Image.sprite = sprite;
	}

	// Token: 0x06003384 RID: 13188 RVA: 0x000F8DB4 File Offset: 0x000F6FB4
	internal void TryUnload()
	{
		if (this.Image.sprite != null)
		{
			this.Image.sprite = null;
		}
		if (this.loadHandle.IsValid())
		{
			if (this.assetReference != null && this.assetReference.IsValid())
			{
				this.assetReference.ReleaseAsset();
			}
			else
			{
				Addressables.Release<Sprite>(this.loadHandle);
			}
			this.loadHandle = default(AsyncOperationHandle<Sprite>);
		}
	}

	// Token: 0x0400292B RID: 10539
	[SerializeField]
	private AssetReferenceSprite assetReference;

	// Token: 0x0400292C RID: 10540
	private Image image;

	// Token: 0x0400292D RID: 10541
	private AsyncOperationHandle<Sprite> loadHandle;
}
