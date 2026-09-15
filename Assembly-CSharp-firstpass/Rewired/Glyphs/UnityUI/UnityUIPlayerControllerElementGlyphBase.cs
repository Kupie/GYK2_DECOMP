using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Glyphs.UnityUI
{
	// Token: 0x02000074 RID: 116
	public abstract class UnityUIPlayerControllerElementGlyphBase : UnityUIControllerElementGlyphBase
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x0000E752 File Offset: 0x0000C952
		// (set) Token: 0x060005FF RID: 1535 RVA: 0x0000E75A File Offset: 0x0000C95A
		public virtual ControllerElementGlyphSelectorOptionsSOBase options
		{
			get
			{
				return this._options;
			}
			set
			{
				this._options = value;
				this.RequireRebuild();
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000600 RID: 1536
		// (set) Token: 0x06000601 RID: 1537
		public abstract int playerId { get; set; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000602 RID: 1538
		// (set) Token: 0x06000603 RID: 1539
		public abstract int actionId { get; set; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x0000E769 File Offset: 0x0000C969
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x0000E771 File Offset: 0x0000C971
		public virtual AxisRange actionRange
		{
			get
			{
				return this._actionRange;
			}
			set
			{
				this._actionRange = value;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x0000E77A File Offset: 0x0000C97A
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x0000E782 File Offset: 0x0000C982
		public virtual Transform group1
		{
			get
			{
				return this._group1;
			}
			set
			{
				this._group1 = value;
				this.RequireRebuild();
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x0000E791 File Offset: 0x0000C991
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x0000E799 File Offset: 0x0000C999
		public virtual Transform group2
		{
			get
			{
				return this._group2;
			}
			set
			{
				this._group2 = value;
				this.RequireRebuild();
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0000E7A8 File Offset: 0x0000C9A8
		protected virtual bool isMousePrioritizedOverKeyboard
		{
			get
			{
				int num = 0;
				ControllerType controllerType;
				while (this.TryGetControllerTypeOrder(num, out controllerType))
				{
					if (controllerType == ControllerType.Mouse)
					{
						return true;
					}
					if (controllerType == ControllerType.Keyboard)
					{
						return false;
					}
					num++;
				}
				return false;
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0000E7D4 File Offset: 0x0000C9D4
		protected virtual bool TryGetControllerTypeOrder(int index, out ControllerType controllerType)
		{
			return this.GetOptionsOrDefault().TryGetControllerTypeOrder(index, out controllerType);
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0000E7E4 File Offset: 0x0000C9E4
		protected override void Update()
		{
			base.Update();
			if (!ReInput.isReady)
			{
				return;
			}
			ActionElementMap actionElementMap;
			ActionElementMap actionElementMap2;
			if (!GlyphTools.TryGetActionElementMaps(this.playerId, this.actionId, this.actionRange, this.GetOptionsOrDefault(), this._tempAems, out actionElementMap, out actionElementMap2))
			{
				this.Hide();
				return;
			}
			if (actionElementMap != null && actionElementMap2 != null)
			{
				this.ShowSplitAxisBindings(actionElementMap, actionElementMap2);
				return;
			}
			if (actionElementMap != null)
			{
				this.ShowBinding(actionElementMap);
				return;
			}
			if (actionElementMap2 != null)
			{
				this.ShowBinding(actionElementMap2);
			}
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0000E856 File Offset: 0x0000CA56
		protected override void ClearObjects()
		{
			this._group1Objects.Clear();
			this._group2Objects.Clear();
			base.ClearObjects();
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0000E874 File Offset: 0x0000CA74
		protected virtual bool ShowBinding(ActionElementMap actionElementMap)
		{
			if (actionElementMap == null)
			{
				return false;
			}
			int num = this.ShowGlyphsOrText(actionElementMap, this.GetObjectGroupTransform(0), this._group1Objects);
			this.EvaluateObjectVisibility();
			return num > 0;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0000E898 File Offset: 0x0000CA98
		protected virtual bool ShowSplitAxisBindings(ActionElementMap negativeAem, ActionElementMap positiveAem)
		{
			if (negativeAem == null && positiveAem == null)
			{
				return false;
			}
			int num = 0;
			if (negativeAem != null && positiveAem != null)
			{
				this._tempCombinedElementAems.Clear();
				this._tempCombinedElementAems.Add(negativeAem);
				this._tempCombinedElementAems.Add(positiveAem);
				num = this.ShowGlyphsOrText(this._tempCombinedElementAems, this.GetObjectGroupTransform(0), this._group1Objects);
			}
			if (num == 0)
			{
				num += this.ShowGlyphsOrText(negativeAem, this.GetObjectGroupTransform(0), this._group1Objects);
				num += this.ShowGlyphsOrText(positiveAem, this.GetObjectGroupTransform(1), this._group2Objects);
			}
			this.EvaluateObjectVisibility();
			return num > 0;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0000E930 File Offset: 0x0000CB30
		protected override void EvaluateObjectVisibility()
		{
			base.EvaluateObjectVisibility();
			Transform objectGroupTransform = this.GetObjectGroupTransform(0);
			Transform objectGroupTransform2 = this.GetObjectGroupTransform(1);
			if (objectGroupTransform == objectGroupTransform2)
			{
				this.EvaluateObjectVisibility(objectGroupTransform);
				return;
			}
			this.EvaluateObjectVisibility(objectGroupTransform, this._group1Objects);
			this.EvaluateObjectVisibility(objectGroupTransform2, this._group2Objects);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0000E980 File Offset: 0x0000CB80
		protected virtual int ShowGlyphsOrText(IList<ActionElementMap> bindings, Transform parent, List<ControllerElementGlyphBase.GlyphOrTextObject> objects)
		{
			if (bindings == null)
			{
				return 0;
			}
			object obj;
			if (this.IsAllowed(ControllerElementGlyphBase.AllowedTypes.Glyphs) && ActionElementMap.TryGetCombinedElementIdentifierGlyph(bindings, out obj))
			{
				if (!this.CreateObjectsAsNeeded(parent, objects, 1))
				{
					return 0;
				}
				objects[0].ShowGlyph(obj);
				return 1;
			}
			else
			{
				string text;
				if (!this.IsAllowed(ControllerElementGlyphBase.AllowedTypes.Text) || !ActionElementMap.TryGetCombinedElementIdentifierName(bindings, out text))
				{
					return 0;
				}
				if (!this.CreateObjectsAsNeeded(parent, objects, 1))
				{
					return 0;
				}
				objects[0].ShowText(text);
				return 1;
			}
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0000E9F4 File Offset: 0x0000CBF4
		protected override void Hide()
		{
			base.Hide();
			if (this._group1 != null && this._group1 != base.transform)
			{
				this._group1.gameObject.SetActive(false);
			}
			if (this._group2 != null && this._group2 != base.transform)
			{
				this._group2.gameObject.SetActive(false);
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0000EA6C File Offset: 0x0000CC6C
		protected virtual Transform GetObjectGroupTransform(int groupIndex)
		{
			if (groupIndex > 1)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (groupIndex != 0)
			{
				if (groupIndex != 1)
				{
					throw new NotImplementedException();
				}
				if (this._group1 == null)
				{
					return base.transform;
				}
				if (this._group2 != null)
				{
					return this._group2;
				}
				if (this._group1 != null)
				{
					return this._group1;
				}
				return base.transform;
			}
			else
			{
				if (!(this._group1 != null))
				{
					return base.transform;
				}
				return this._group1;
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0000EAF4 File Offset: 0x0000CCF4
		protected virtual ControllerElementGlyphSelectorOptions GetOptionsOrDefault()
		{
			if (this._options != null && this._options.options == null)
			{
				Debug.LogError("Rewired: Options missing on " + typeof(ControllerElementGlyphSelectorOptions).Name + ". Global default options will be used instead.");
				return ControllerElementGlyphSelectorOptions.defaultOptions;
			}
			if (!(this._options != null))
			{
				return ControllerElementGlyphSelectorOptions.defaultOptions;
			}
			return this._options.options;
		}

		// Token: 0x04000321 RID: 801
		[Tooltip("Optional reference to an object that defines options. If blank, the global default options will be used.")]
		[SerializeField]
		private ControllerElementGlyphSelectorOptionsSOBase _options;

		// Token: 0x04000322 RID: 802
		[Tooltip("The range of the Action for which to show glyphs / text. This determines whether to show the glyph for an axis-type Action (ex: Move Horizontal), or the positive/negative pole of an Action (ex: Move Right). For button-type Actions, Full and Positive are equivalent.")]
		[SerializeField]
		private AxisRange _actionRange;

		// Token: 0x04000323 RID: 803
		[Tooltip("Optional parent Transform of the first group of instantiated glyph / text objects. If an axis-type Action is bound to multiple elements, the glyphs bound to the negative pole of the Action will be instantiated under this Transform. This allows you to separate negative and positive groups in order to stack glyph groups horizontally or vertically, for example. If an Action is only bound to one element, the glyph will be instantiated under this transform. If blank, objects will be created as children of this object's Transform.")]
		[SerializeField]
		private Transform _group1;

		// Token: 0x04000324 RID: 804
		[Tooltip("Optional parent Transform of the second group of instantiated glyph / text objects. If an axis-type Action is bound to multiple elements, the glyphs bound to the positive pole of the Action will be instantiated under this Transform. This allows you to separate negative and positive groups in order to stack glyph groups horizontally or vertically, for example. If an Action is only bound to one element, the glyph will be instantiated under group1 instead. If blank, objects will be created as children of either group1 if set or the object's Transform.")]
		[SerializeField]
		private Transform _group2;

		// Token: 0x04000325 RID: 805
		[NonSerialized]
		private List<ActionElementMap> _tempAems = new List<ActionElementMap>();

		// Token: 0x04000326 RID: 806
		[NonSerialized]
		private List<ActionElementMap> _tempCombinedElementAems = new List<ActionElementMap>();

		// Token: 0x04000327 RID: 807
		[NonSerialized]
		private readonly List<ControllerElementGlyphBase.GlyphOrTextObject> _group1Objects = new List<ControllerElementGlyphBase.GlyphOrTextObject>();

		// Token: 0x04000328 RID: 808
		[NonSerialized]
		private readonly List<ControllerElementGlyphBase.GlyphOrTextObject> _group2Objects = new List<ControllerElementGlyphBase.GlyphOrTextObject>();
	}
}
