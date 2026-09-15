using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x0200010A RID: 266
	[ExecuteInEditMode]
	[RequireComponent(typeof(Image))]
	public class LocalizedAssetReferenceImage : MonoBehaviour
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0001C6BE File Offset: 0x0001A8BE
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

		// Token: 0x0600054F RID: 1359 RVA: 0x0001C6E0 File Offset: 0x0001A8E0
		private void OnEnable()
		{
			this.Localize();
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001C6E8 File Offset: 0x0001A8E8
		private void OnDisable()
		{
			this.TryUnload();
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001C6F0 File Offset: 0x0001A8F0
		public void Localize()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.TryLoad();
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001C704 File Offset: 0x0001A904
		private void TryLoad()
		{
			AssetReferenceSprite assetReferenceForCurrentLanguage = this.GetAssetReferenceForCurrentLanguage();
			if (assetReferenceForCurrentLanguage == this.loadedAssetReference && (this.loadHandle.IsValid() || !Application.isPlaying))
			{
				return;
			}
			this.TryUnload();
			if (assetReferenceForCurrentLanguage == null || string.IsNullOrEmpty(assetReferenceForCurrentLanguage.AssetGUID))
			{
				return;
			}
			Sprite sprite;
			try
			{
				this.loadHandle = assetReferenceForCurrentLanguage.LoadAssetAsync<Sprite>();
				this.loadedAssetReference = assetReferenceForCurrentLanguage;
				sprite = this.loadHandle.WaitForCompletion();
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format("Failed to load localized sprite from AssetReference '{0}': {1}", assetReferenceForCurrentLanguage.RuntimeKey, ex.Message), this);
				this.TryUnload();
				return;
			}
			if (sprite == null)
			{
				Debug.LogError(string.Format("Failed to load localized sprite from AssetReference '{0}'", assetReferenceForCurrentLanguage.RuntimeKey), this);
				this.TryUnload();
				return;
			}
			this.Image.sprite = sprite;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001C7D8 File Offset: 0x0001A9D8
		private AssetReferenceSprite GetAssetReferenceForCurrentLanguage()
		{
			string currentLang = LLBase.CurrentLang;
			foreach (LocalizedAssetReferenceImage.LocalizedSpriteReference localizedSpriteReference in this.localizedAssetReferences)
			{
				if (localizedSpriteReference != null && localizedSpriteReference.AssetReference != null && !string.IsNullOrEmpty(localizedSpriteReference.AssetReference.AssetGUID) && !(localizedSpriteReference.LanguageId != currentLang))
				{
					return localizedSpriteReference.AssetReference;
				}
			}
			return this.defaultAssetReference;
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001C868 File Offset: 0x0001AA68
		internal void TryUnload()
		{
			if (this.Image.sprite != null)
			{
				this.Image.sprite = null;
			}
			if (this.loadHandle.IsValid())
			{
				if (this.loadedAssetReference != null && this.loadedAssetReference.IsValid())
				{
					this.loadedAssetReference.ReleaseAsset();
				}
				else
				{
					Addressables.Release<Sprite>(this.loadHandle);
				}
				this.loadHandle = default(AsyncOperationHandle<Sprite>);
			}
			this.loadedAssetReference = null;
		}

		// Token: 0x04000273 RID: 627
		[SerializeField]
		private AssetReferenceSprite defaultAssetReference;

		// Token: 0x04000274 RID: 628
		[SerializeField]
		private List<LocalizedAssetReferenceImage.LocalizedSpriteReference> localizedAssetReferences = new List<LocalizedAssetReferenceImage.LocalizedSpriteReference>();

		// Token: 0x04000275 RID: 629
		private Image image;

		// Token: 0x04000276 RID: 630
		private AsyncOperationHandle<Sprite> loadHandle;

		// Token: 0x04000277 RID: 631
		private AssetReferenceSprite loadedAssetReference;

		// Token: 0x020001E9 RID: 489
		[Serializable]
		private class LocalizedSpriteReference
		{
			// Token: 0x17000157 RID: 343
			// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0002EC66 File Offset: 0x0002CE66
			public string LanguageId
			{
				get
				{
					return this.languageId;
				}
			}

			// Token: 0x17000158 RID: 344
			// (get) Token: 0x06000A57 RID: 2647 RVA: 0x0002EC6E File Offset: 0x0002CE6E
			public AssetReferenceSprite AssetReference
			{
				get
				{
					return this.assetReference;
				}
			}

			// Token: 0x04000664 RID: 1636
			[SerializeField]
			private string languageId;

			// Token: 0x04000665 RID: 1637
			[SerializeField]
			private AssetReferenceSprite assetReference;
		}
	}
}
