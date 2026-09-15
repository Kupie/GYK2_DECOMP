using System;
using UnityEngine;

namespace Rewired.Glyphs
{
	// Token: 0x02000065 RID: 101
	[Serializable]
	public abstract class GlyphSet : ScriptableObject
	{
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0000D9F2 File Offset: 0x0000BBF2
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x0000D9FA File Offset: 0x0000BBFA
		public string[] baseKeys
		{
			get
			{
				return this._baseKeys;
			}
			set
			{
				this._baseKeys = value;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060005B2 RID: 1458
		public abstract int glyphCount { get; }

		// Token: 0x060005B3 RID: 1459
		public abstract GlyphSet.EntryBase GetEntry(int index);

		// Token: 0x0400030B RID: 779
		[Tooltip("A list of base keys. Final keys will be composed of base key + glyph key. Setting multiple base keys allows one glyph set to apply to multiple controllers, for example.")]
		[SerializeField]
		private string[] _baseKeys;

		// Token: 0x02000066 RID: 102
		[Serializable]
		public abstract class EntryBase
		{
			// Token: 0x170002B2 RID: 690
			// (get) Token: 0x060005B5 RID: 1461 RVA: 0x0000DA03 File Offset: 0x0000BC03
			// (set) Token: 0x060005B6 RID: 1462 RVA: 0x0000DA0B File Offset: 0x0000BC0B
			public string key
			{
				get
				{
					return this._key;
				}
				set
				{
					this._key = value;
				}
			}

			// Token: 0x060005B7 RID: 1463
			public abstract object GetValue();

			// Token: 0x0400030C RID: 780
			[SerializeField]
			private string _key;
		}

		// Token: 0x02000067 RID: 103
		[Serializable]
		public abstract class EntryBase<TValue> : GlyphSet.EntryBase
		{
			// Token: 0x170002B3 RID: 691
			// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0000DA14 File Offset: 0x0000BC14
			// (set) Token: 0x060005BA RID: 1466 RVA: 0x0000DA1C File Offset: 0x0000BC1C
			public TValue value
			{
				get
				{
					return this._value;
				}
				set
				{
					this._value = value;
				}
			}

			// Token: 0x060005BB RID: 1467 RVA: 0x0000DA25 File Offset: 0x0000BC25
			public override object GetValue()
			{
				return this._value;
			}

			// Token: 0x0400030D RID: 781
			[SerializeField]
			private TValue _value;
		}
	}
}
