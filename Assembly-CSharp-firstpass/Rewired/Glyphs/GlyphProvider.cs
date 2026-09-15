using System;
using System.Collections.Generic;
using System.Text;
using Rewired.Interfaces;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.Glyphs
{
	// Token: 0x02000064 RID: 100
	public class GlyphProvider : MonoBehaviour, IGlyphProvider
	{
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x0000D6C7 File Offset: 0x0000B8C7
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x0000D6CF File Offset: 0x0000B8CF
		public bool prefetch
		{
			get
			{
				return this._prefetch;
			}
			set
			{
				this._prefetch = value;
				if (base.isActiveAndEnabled && ReInput.isReady && ReInput.glyphs.glyphProvider == this)
				{
					ReInput.glyphs.prefetch = value;
				}
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0000D6FF File Offset: 0x0000B8FF
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x0000D707 File Offset: 0x0000B907
		public List<GlyphSetCollection> glyphSetCollections
		{
			get
			{
				return this._glyphSetCollections;
			}
			set
			{
				this._glyphSetCollections = value;
				this.Reload();
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0000D716 File Offset: 0x0000B916
		protected Dictionary<string, object> glyphs
		{
			get
			{
				return this._glyphs;
			}
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0000D71E File Offset: 0x0000B91E
		protected virtual void OnEnable()
		{
			if (!this._initialized)
			{
				this.Initialize();
			}
			this.TrySetGlyphProvider();
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0000D735 File Offset: 0x0000B935
		protected virtual void OnDisable()
		{
			if (ReInput.isReady && ReInput.glyphs.glyphProvider == this)
			{
				ReInput.glyphs.glyphProvider = null;
			}
			ReInput.InitializedEvent -= this.TrySetGlyphProvider;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00003466 File Offset: 0x00001666
		protected virtual void Update()
		{
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0000D768 File Offset: 0x0000B968
		protected virtual void TrySetGlyphProvider()
		{
			ReInput.InitializedEvent -= this.TrySetGlyphProvider;
			ReInput.InitializedEvent += this.TrySetGlyphProvider;
			if (!ReInput.isReady)
			{
				return;
			}
			if (!UnityTools.IsNullOrDestroyed<IGlyphProvider>(ReInput.glyphs.glyphProvider))
			{
				Debug.LogWarning("Rewired: A glyph provider is already set. Only one glyph provider can exist at a time.");
				return;
			}
			ReInput.glyphs.glyphProvider = this;
			ReInput.glyphs.prefetch = this._prefetch;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0000D7D8 File Offset: 0x0000B9D8
		protected virtual bool Initialize()
		{
			this._initialized = false;
			if (this._glyphSetCollections == null)
			{
				return false;
			}
			this._glyphs.Clear();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this._glyphSetCollections.Count; i++)
			{
				GlyphSetCollection glyphSetCollection = this._glyphSetCollections[i];
				if (!(glyphSetCollection == null))
				{
					foreach (GlyphSet glyphSet in glyphSetCollection.IterateSetsRecursively())
					{
						if (!(glyphSet == null) && glyphSet.baseKeys != null)
						{
							int num = glyphSet.baseKeys.Length;
							for (int j = 0; j < num; j++)
							{
								if (!string.IsNullOrEmpty(glyphSet.baseKeys[j]))
								{
									int glyphCount = glyphSet.glyphCount;
									for (int k = 0; k < glyphCount; k++)
									{
										GlyphSet.EntryBase entry = glyphSet.GetEntry(k);
										if (entry != null && !string.IsNullOrEmpty(entry.key) && entry.GetValue() != null)
										{
											stringBuilder.Append(glyphSet.baseKeys[j]);
											stringBuilder.Append('/');
											stringBuilder.Append(entry.key);
											string text = stringBuilder.ToString();
											stringBuilder.Length = 0;
											if (this._glyphs.ContainsKey(text))
											{
												Debug.LogError("Rewired: Duplicate glyph key found: " + text);
											}
											else
											{
												this._glyphs.Add(text, entry.GetValue());
											}
										}
									}
								}
							}
						}
					}
				}
			}
			this._initialized = true;
			return true;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0000D994 File Offset: 0x0000BB94
		public void Reload()
		{
			this.Initialize();
			if (base.isActiveAndEnabled && ReInput.isReady && ReInput.glyphs.glyphProvider == this)
			{
				ReInput.glyphs.Reload();
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0000D9C3 File Offset: 0x0000BBC3
		bool IGlyphProvider.TryGetGlyph(string key, out object result)
		{
			if (!this._initialized)
			{
				result = null;
				return false;
			}
			return this._glyphs.TryGetValue(key, out result);
		}

		// Token: 0x04000307 RID: 775
		[SerializeField]
		[Tooltip("Determines if glyphs should be fetched immediately in bulk when available. If false, glyphs will be fetched when queried.")]
		private bool _prefetch;

		// Token: 0x04000308 RID: 776
		[SerializeField]
		[Tooltip("A list of glyph set collections. At least one collection must be assigned.")]
		private List<GlyphSetCollection> _glyphSetCollections;

		// Token: 0x04000309 RID: 777
		[NonSerialized]
		private readonly Dictionary<string, object> _glyphs = new Dictionary<string, object>();

		// Token: 0x0400030A RID: 778
		[NonSerialized]
		private bool _initialized;
	}
}
