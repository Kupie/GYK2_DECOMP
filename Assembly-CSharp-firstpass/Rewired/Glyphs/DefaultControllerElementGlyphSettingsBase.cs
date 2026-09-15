using System;
using UnityEngine;

namespace Rewired.Glyphs
{
	// Token: 0x02000060 RID: 96
	public abstract class DefaultControllerElementGlyphSettingsBase : MonoBehaviour
	{
		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0000D3FD File Offset: 0x0000B5FD
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x0000D405 File Offset: 0x0000B605
		public ControllerElementGlyphSelectorOptions options
		{
			get
			{
				return this._options;
			}
			set
			{
				this._options = value;
				this.SetDefaults();
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0000D414 File Offset: 0x0000B614
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x0000D41C File Offset: 0x0000B61C
		public GameObject glyphOrTextPrefab
		{
			get
			{
				return this._glyphOrTextPrefab;
			}
			set
			{
				this._glyphOrTextPrefab = value;
				this.SetDefaults();
			}
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0000D42B File Offset: 0x0000B62B
		protected virtual void OnEnable()
		{
			this.SetDefaults();
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00003466 File Offset: 0x00001666
		protected virtual void OnDisable()
		{
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0000D433 File Offset: 0x0000B633
		protected virtual void SetDefaults()
		{
			this.SetDefaultOptions();
			this.SetDefaultGlyphOrTextPrefab();
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0000D441 File Offset: 0x0000B641
		protected virtual void SetDefaultOptions()
		{
			ControllerElementGlyphSelectorOptions.defaultOptions = this.options;
		}

		// Token: 0x0600058F RID: 1423
		protected abstract void SetDefaultGlyphOrTextPrefab();

		// Token: 0x040002FE RID: 766
		[Tooltip("The Controller element glyph options.")]
		[SerializeField]
		private ControllerElementGlyphSelectorOptions _options;

		// Token: 0x040002FF RID: 767
		[Tooltip("The prefab used for each glyph or text object.")]
		[SerializeField]
		private GameObject _glyphOrTextPrefab;
	}
}
