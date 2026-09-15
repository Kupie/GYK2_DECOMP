using System;
using Rewired.Interfaces;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.Localization
{
	// Token: 0x02000058 RID: 88
	public abstract class LocalizedStringProviderBase : MonoBehaviour, ILocalizedStringProvider
	{
		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x0000CBE3 File Offset: 0x0000ADE3
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x0000CBEB File Offset: 0x0000ADEB
		public virtual bool prefetch
		{
			get
			{
				return this._prefetch;
			}
			set
			{
				this._prefetch = value;
				if (base.gameObject.activeInHierarchy && base.enabled && ReInput.isReady && ReInput.localization.localizedStringProvider == this)
				{
					ReInput.localization.prefetch = value;
				}
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000544 RID: 1348
		protected abstract bool initialized { get; }

		// Token: 0x06000545 RID: 1349 RVA: 0x0000CC28 File Offset: 0x0000AE28
		protected virtual void OnEnable()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.TrySetLocalizedStringProvider();
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000CC3F File Offset: 0x0000AE3F
		protected virtual void OnDisable()
		{
			if (ReInput.isReady && ReInput.localization.localizedStringProvider == this)
			{
				ReInput.localization.localizedStringProvider = null;
			}
			ReInput.InitializedEvent -= this.TrySetLocalizedStringProvider;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00003466 File Offset: 0x00001666
		protected virtual void Update()
		{
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0000CC74 File Offset: 0x0000AE74
		protected virtual void TrySetLocalizedStringProvider()
		{
			ReInput.InitializedEvent -= this.TrySetLocalizedStringProvider;
			ReInput.InitializedEvent += this.TrySetLocalizedStringProvider;
			if (!ReInput.isReady)
			{
				return;
			}
			if (!UnityTools.IsNullOrDestroyed<ILocalizedStringProvider>(ReInput.localization.localizedStringProvider))
			{
				Debug.LogWarning("A localized string provider is already set. Only one localized string provider can exist at a time.");
				return;
			}
			ReInput.localization.localizedStringProvider = this;
			ReInput.localization.prefetch = this._prefetch;
		}

		// Token: 0x06000549 RID: 1353
		protected abstract bool Initialize();

		// Token: 0x0600054A RID: 1354 RVA: 0x0000CCE4 File Offset: 0x0000AEE4
		public virtual void Reload()
		{
			this.Initialize();
			if (base.gameObject.activeInHierarchy && base.enabled && ReInput.isReady && ReInput.localization.localizedStringProvider == this)
			{
				ReInput.localization.Reload();
			}
		}

		// Token: 0x0600054B RID: 1355
		protected abstract bool TryGetLocalizedString(string key, out string result);

		// Token: 0x0600054C RID: 1356 RVA: 0x0000CD20 File Offset: 0x0000AF20
		bool ILocalizedStringProvider.TryGetLocalizedString(string key, out string result)
		{
			return this.TryGetLocalizedString(key, out result);
		}

		// Token: 0x040002EA RID: 746
		[SerializeField]
		[Tooltip("Determines if localized strings should be fetched immediately in bulk when available. If false, strings will be fetched when queried.")]
		private bool _prefetch;
	}
}
