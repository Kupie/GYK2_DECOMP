using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Glyphs
{
	// Token: 0x0200005A RID: 90
	public abstract class ControllerElementGlyphBase : MonoBehaviour
	{
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x0000CDD2 File Offset: 0x0000AFD2
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x0000CDDA File Offset: 0x0000AFDA
		public virtual GameObject glyphOrTextPrefab
		{
			get
			{
				return this._glyphOrTextPrefab;
			}
			set
			{
				this._glyphOrTextPrefab = value;
				this.RequireRebuild();
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x0000CDE9 File Offset: 0x0000AFE9
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x0000CDF1 File Offset: 0x0000AFF1
		public virtual ControllerElementGlyphBase.AllowedTypes allowedTypes
		{
			get
			{
				return this._allowedTypes;
			}
			set
			{
				this._allowedTypes = value;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x0000CDFA File Offset: 0x0000AFFA
		protected List<ControllerElementGlyphBase.GlyphOrTextObject> entries
		{
			get
			{
				return this._entries;
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00003466 File Offset: 0x00001666
		protected virtual void Awake()
		{
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00003466 File Offset: 0x00001666
		protected virtual void Start()
		{
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00003466 File Offset: 0x00001666
		protected virtual void OnDestroy()
		{
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00003466 File Offset: 0x00001666
		protected virtual void OnEnable()
		{
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00003466 File Offset: 0x00001666
		protected virtual void OnDisable()
		{
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0000CE02 File Offset: 0x0000B002
		protected virtual void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (this._lastGlyphOrTextPrefab != this.GetGlyphOrTextPrefabOrDefault())
			{
				this._lastGlyphOrTextPrefab = this.GetGlyphOrTextPrefabOrDefault();
				this.RequireRebuild();
			}
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0000CE31 File Offset: 0x0000B031
		public virtual void RequireRebuild()
		{
			this.ClearObjects();
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0000CE3C File Offset: 0x0000B03C
		protected virtual void ClearObjects()
		{
			for (int i = 0; i < this._entries.Count; i++)
			{
				if (this._entries[i] != null)
				{
					this._entries[i].Destroy();
				}
			}
			this._entries.Clear();
			this.Hide();
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0000CE90 File Offset: 0x0000B090
		protected virtual void EvaluateObjectVisibility()
		{
			for (int i = 0; i < this._entries.Count; i++)
			{
				if (this._entries[i] != null)
				{
					this._entries[i].HideIfIdle();
				}
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0000CED2 File Offset: 0x0000B0D2
		protected virtual void EvaluateObjectVisibility(Transform transform)
		{
			this.EvaluateObjectVisibility(transform, this._entries);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0000CEE4 File Offset: 0x0000B0E4
		protected virtual void EvaluateObjectVisibility(Transform transform, List<ControllerElementGlyphBase.GlyphOrTextObject> entries)
		{
			if (transform == base.transform)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].isVisible)
				{
					flag = true;
				}
			}
			if (transform.gameObject.activeSelf != flag)
			{
				transform.gameObject.SetActive(flag);
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0000CF40 File Offset: 0x0000B140
		protected virtual int ShowGlyphsOrText(ActionElementMap actionElementMap, Transform parent, List<ControllerElementGlyphBase.GlyphOrTextObject> entries)
		{
			this._tempGlyphs.Clear();
			int num = 0;
			if (this.IsAllowed(ControllerElementGlyphBase.AllowedTypes.Glyphs) && ControllerElementGlyphBase.GetGlyphs(actionElementMap, this._tempGlyphs) > 0)
			{
				if (!this.CreateObjectsAsNeeded(parent, entries, this._tempGlyphs.Count))
				{
					return 0;
				}
				for (int i = 0; i < this._tempGlyphs.Count; i++)
				{
					entries[i].ShowGlyph(this._tempGlyphs[i]);
				}
				num += this._tempGlyphs.Count;
			}
			else if (this.IsAllowed(ControllerElementGlyphBase.AllowedTypes.Text) && actionElementMap != null)
			{
				if (!this.CreateObjectsAsNeeded(parent, entries, 1))
				{
					return 0;
				}
				entries[0].ShowText(actionElementMap.elementIdentifierName);
				num++;
			}
			return num;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
		protected virtual int ShowGlyphsOrText(ActionElementMap actionElementMap)
		{
			return this.ShowGlyphsOrText(actionElementMap, base.transform, this._entries);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0000D00C File Offset: 0x0000B20C
		protected virtual int ShowGlyphsOrText(ControllerElementIdentifier elementIdentifier, AxisRange axisRange, Transform parent, List<ControllerElementGlyphBase.GlyphOrTextObject> entries)
		{
			if (elementIdentifier == null)
			{
				return 0;
			}
			int num = 0;
			object glyph;
			if (this.IsAllowed(ControllerElementGlyphBase.AllowedTypes.Glyphs) && (glyph = elementIdentifier.GetGlyph(axisRange)) != null)
			{
				if (!this.CreateObjectsAsNeeded(parent, entries, 1))
				{
					return 0;
				}
				entries[0].ShowGlyph(glyph);
				num++;
			}
			else if (this.IsAllowed(ControllerElementGlyphBase.AllowedTypes.Text))
			{
				if (!this.CreateObjectsAsNeeded(parent, entries, 1))
				{
					return 0;
				}
				entries[0].ShowText(elementIdentifier.GetDisplayName(axisRange));
				num++;
			}
			return num;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0000D086 File Offset: 0x0000B286
		protected virtual int ShowGlyphsOrText(ControllerElementIdentifier elementIdentifier, AxisRange axisRange)
		{
			return this.ShowGlyphsOrText(elementIdentifier, axisRange, base.transform, this._entries);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0000D09C File Offset: 0x0000B29C
		protected virtual void Hide()
		{
			for (int i = 0; i < this._entries.Count; i++)
			{
				if (this._entries[i] != null)
				{
					this._entries[i].Hide();
				}
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0000D0DE File Offset: 0x0000B2DE
		protected virtual GameObject GetGlyphOrTextPrefabOrDefault()
		{
			if (!(this._glyphOrTextPrefab != null))
			{
				return this.GetDefaultGlyphOrTextPrefab();
			}
			return this._glyphOrTextPrefab;
		}

		// Token: 0x0600056C RID: 1388
		protected abstract GameObject GetDefaultGlyphOrTextPrefab();

		// Token: 0x0600056D RID: 1389 RVA: 0x0000D0FC File Offset: 0x0000B2FC
		protected virtual bool CreateObjectsAsNeeded(Transform parent, List<ControllerElementGlyphBase.GlyphOrTextObject> entries, int count)
		{
			if (count <= 0)
			{
				return false;
			}
			GameObject glyphOrTextPrefabOrDefault = this.GetGlyphOrTextPrefabOrDefault();
			if (glyphOrTextPrefabOrDefault == null)
			{
				Debug.LogError("Rewired: Default prefab is null.");
				return false;
			}
			if (entries == null)
			{
				return false;
			}
			for (int i = entries.Count; i < count; i++)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(glyphOrTextPrefabOrDefault);
				gameObject.name = "Object";
				gameObject.hideFlags = HideFlags.DontSave;
				gameObject.transform.SetParent(parent, false);
				GlyphOrTextBase component = gameObject.GetComponent<GlyphOrTextBase>();
				if (component == null)
				{
					string text = "Rewired: Prefab does not contain a ";
					Type typeFromHandle = typeof(GlyphOrTextBase);
					Debug.LogError(text + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " component.");
					global::UnityEngine.Object.Destroy(gameObject);
				}
				else
				{
					ControllerElementGlyphBase.GlyphOrTextObject glyphOrTextObject = new ControllerElementGlyphBase.GlyphOrTextObject(component);
					entries.Add(glyphOrTextObject);
					if (entries != this._entries)
					{
						this._entries.Add(glyphOrTextObject);
					}
				}
			}
			return true;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0000D1D7 File Offset: 0x0000B3D7
		protected virtual bool IsAllowed(ControllerElementGlyphBase.AllowedTypes allowedType)
		{
			return this._allowedTypes == ControllerElementGlyphBase.AllowedTypes.All || allowedType == this._allowedTypes;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0000D1EC File Offset: 0x0000B3EC
		protected static int GetGlyphs(ActionElementMap actionElementMap, List<object> results)
		{
			if (actionElementMap == null)
			{
				return 0;
			}
			int num = 1;
			if (actionElementMap.hasModifiers)
			{
				if (actionElementMap.modifierKey1 != ModifierKey.None)
				{
					num++;
				}
				if (actionElementMap.modifierKey2 != ModifierKey.None)
				{
					num++;
				}
				if (actionElementMap.modifierKey3 != ModifierKey.None)
				{
					num++;
				}
			}
			if (actionElementMap.elementIdentifierGlyphCount != num)
			{
				return 0;
			}
			actionElementMap.GetElementIdentifierGlyphs(results);
			return num;
		}

		// Token: 0x040002EE RID: 750
		[Tooltip("If set, when glyph/text objects are created, they will be instantiated from this prefab. If left blank, the global default prefab will be used.")]
		[SerializeField]
		private GameObject _glyphOrTextPrefab;

		// Token: 0x040002EF RID: 751
		[Tooltip("Determines what types of objects are allowed.")]
		[SerializeField]
		private ControllerElementGlyphBase.AllowedTypes _allowedTypes;

		// Token: 0x040002F0 RID: 752
		[NonSerialized]
		private readonly List<ControllerElementGlyphBase.GlyphOrTextObject> _entries = new List<ControllerElementGlyphBase.GlyphOrTextObject>();

		// Token: 0x040002F1 RID: 753
		[NonSerialized]
		private List<object> _tempGlyphs = new List<object>();

		// Token: 0x040002F2 RID: 754
		[NonSerialized]
		private GameObject _lastGlyphOrTextPrefab;

		// Token: 0x0200005B RID: 91
		protected class GlyphOrTextObject
		{
			// Token: 0x170002A0 RID: 672
			// (get) Token: 0x06000571 RID: 1393 RVA: 0x0000D25E File Offset: 0x0000B45E
			// (set) Token: 0x06000572 RID: 1394 RVA: 0x0000D266 File Offset: 0x0000B466
			public virtual bool isVisible
			{
				get
				{
					return this._isVisible;
				}
				protected set
				{
					this._isVisible = value;
				}
			}

			// Token: 0x170002A1 RID: 673
			// (get) Token: 0x06000573 RID: 1395 RVA: 0x0000D26F File Offset: 0x0000B46F
			// (set) Token: 0x06000574 RID: 1396 RVA: 0x0000D277 File Offset: 0x0000B477
			public GlyphOrTextBase glyphOrText
			{
				get
				{
					return this._glyphOrText;
				}
				set
				{
					this._glyphOrText = value;
				}
			}

			// Token: 0x06000575 RID: 1397 RVA: 0x0000D280 File Offset: 0x0000B480
			public GlyphOrTextObject(GlyphOrTextBase glyphOrText)
			{
				this._glyphOrText = glyphOrText;
			}

			// Token: 0x06000576 RID: 1398 RVA: 0x0000D28F File Offset: 0x0000B48F
			public virtual void ShowGlyph(object glyph)
			{
				if (this._glyphOrText == null)
				{
					return;
				}
				this._glyphOrText.ShowGlyph(glyph);
				this._frame = Time.frameCount;
				this._isVisible = true;
			}

			// Token: 0x06000577 RID: 1399 RVA: 0x0000D2BE File Offset: 0x0000B4BE
			public virtual void ShowText(string text)
			{
				if (this._glyphOrText == null)
				{
					return;
				}
				this._glyphOrText.ShowText(text);
				this._frame = Time.frameCount;
				this._isVisible = true;
			}

			// Token: 0x06000578 RID: 1400 RVA: 0x0000D2ED File Offset: 0x0000B4ED
			public virtual void Hide()
			{
				if (this._glyphOrText == null)
				{
					return;
				}
				if (!this._isVisible)
				{
					return;
				}
				this._glyphOrText.Hide();
				this._isVisible = false;
			}

			// Token: 0x06000579 RID: 1401 RVA: 0x0000D319 File Offset: 0x0000B519
			public virtual void HideIfIdle()
			{
				if (this._frame == Time.frameCount)
				{
					return;
				}
				this.Hide();
			}

			// Token: 0x0600057A RID: 1402 RVA: 0x0000D32F File Offset: 0x0000B52F
			public virtual void Destroy()
			{
				if (this._glyphOrText == null)
				{
					return;
				}
				global::UnityEngine.Object.Destroy(this._glyphOrText.gameObject);
				this._glyphOrText = null;
				this._isVisible = false;
			}

			// Token: 0x040002F3 RID: 755
			private GlyphOrTextBase _glyphOrText;

			// Token: 0x040002F4 RID: 756
			private int _frame;

			// Token: 0x040002F5 RID: 757
			private bool _isVisible;
		}

		// Token: 0x0200005C RID: 92
		public enum AllowedTypes
		{
			// Token: 0x040002F7 RID: 759
			All,
			// Token: 0x040002F8 RID: 760
			Glyphs,
			// Token: 0x040002F9 RID: 761
			Text
		}
	}
}
