using System;
using System.Collections.Generic;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Localization
{
	// Token: 0x02000057 RID: 87
	[AddComponentMenu("Rewired/Localization/Localized String Provider")]
	public class LocalizedStringProvider : LocalizedStringProviderBase
	{
		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0000CB0C File Offset: 0x0000AD0C
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x0000CB14 File Offset: 0x0000AD14
		protected virtual Dictionary<string, string> dictionary
		{
			get
			{
				return this._dictionary;
			}
			set
			{
				this._dictionary = value;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x0000CB1D File Offset: 0x0000AD1D
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x0000CB25 File Offset: 0x0000AD25
		public virtual TextAsset localizedStringsFile
		{
			get
			{
				return this._localizedStringsFile;
			}
			set
			{
				this._localizedStringsFile = value;
				this.Reload();
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x0000CB34 File Offset: 0x0000AD34
		protected override bool initialized
		{
			get
			{
				return this._initialized;
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0000CB3C File Offset: 0x0000AD3C
		protected override bool Initialize()
		{
			this._initialized = this.TryLoadLocalizedStringData();
			return this._initialized;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0000CB50 File Offset: 0x0000AD50
		protected virtual bool TryLoadLocalizedStringData()
		{
			this._dictionary.Clear();
			if (this._localizedStringsFile != null)
			{
				try
				{
					this._dictionary = JsonParser.FromJson<Dictionary<string, string>>(this._localizedStringsFile.text);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
				}
			}
			return this._dictionary.Count > 0;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0000CBB4 File Offset: 0x0000ADB4
		protected override bool TryGetLocalizedString(string key, out string result)
		{
			if (!this._initialized)
			{
				result = null;
				return false;
			}
			return this._dictionary.TryGetValue(key, out result);
		}

		// Token: 0x040002E7 RID: 743
		[SerializeField]
		[Tooltip("A JSON file containing localizied string key value pairs.")]
		private TextAsset _localizedStringsFile;

		// Token: 0x040002E8 RID: 744
		[NonSerialized]
		private Dictionary<string, string> _dictionary = new Dictionary<string, string>();

		// Token: 0x040002E9 RID: 745
		[NonSerialized]
		private bool _initialized;
	}
}
