using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x02000101 RID: 257
	[AddComponentMenu("")]
	[RequireComponent(typeof(RectTransform))]
	public sealed class UIPointer : UIBehaviour
	{
		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x00024ACA File Offset: 0x00022CCA
		// (set) Token: 0x06000CAC RID: 3244 RVA: 0x00024AD2 File Offset: 0x00022CD2
		public bool autoSort
		{
			get
			{
				return this._autoSort;
			}
			set
			{
				if (value == this._autoSort)
				{
					return;
				}
				this._autoSort = value;
				if (value)
				{
					base.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00024AF4 File Offset: 0x00022CF4
		protected override void Awake()
		{
			base.Awake();
			Graphic[] componentsInChildren = base.GetComponentsInChildren<Graphic>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].raycastTarget = false;
			}
			if (this._hideHardwarePointer)
			{
				Cursor.visible = false;
			}
			if (this._autoSort)
			{
				base.transform.SetAsLastSibling();
			}
			this.GetDependencies();
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00024B4C File Offset: 0x00022D4C
		private void Update()
		{
			if (this._autoSort && base.transform.GetSiblingIndex() < base.transform.parent.childCount - 1)
			{
				base.transform.SetAsLastSibling();
			}
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00024B80 File Offset: 0x00022D80
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.GetDependencies();
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00024B8E File Offset: 0x00022D8E
		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			this.GetDependencies();
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00024B9C File Offset: 0x00022D9C
		public void OnScreenPositionChanged(Vector2 screenPosition)
		{
			if (this._canvas == null)
			{
				return;
			}
			Camera camera = null;
			RenderMode renderMode = this._canvas.renderMode;
			if (renderMode != RenderMode.ScreenSpaceOverlay && renderMode - RenderMode.ScreenSpaceCamera <= 1)
			{
				camera = this._canvas.worldCamera;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, screenPosition, camera, out vector);
			base.transform.localPosition = new Vector3(vector.x, vector.y, base.transform.localPosition.z);
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00024C21 File Offset: 0x00022E21
		private void GetDependencies()
		{
			this._canvas = base.transform.root.GetComponentInChildren<Canvas>();
		}

		// Token: 0x0400067D RID: 1661
		[Tooltip("Should the hardware pointer be hidden?")]
		[SerializeField]
		private bool _hideHardwarePointer = true;

		// Token: 0x0400067E RID: 1662
		[Tooltip("Sets the pointer to the last sibling in the parent hierarchy. Do not enable this on multiple UIPointers under the same parent transform or they will constantly fight each other for dominance.")]
		[SerializeField]
		private bool _autoSort = true;

		// Token: 0x0400067F RID: 1663
		private Canvas _canvas;
	}
}
