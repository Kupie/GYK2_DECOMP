using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000844 RID: 2116
public class UIMouseTooltip : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x1700080C RID: 2060
	// (get) Token: 0x060035FD RID: 13821 RVA: 0x001032E2 File Offset: 0x001014E2
	private RectTransform Target
	{
		get
		{
			if (!(this.overrideTarget != null))
			{
				return base.transform as RectTransform;
			}
			return this.overrideTarget;
		}
	}

	// Token: 0x060035FE RID: 13822 RVA: 0x00103304 File Offset: 0x00101504
	public static UIMouseTooltip Attach(GameObject go, string lngId, RectTransform overrideTarget = null, bool addRaycastTarget = false, bool disableChildRaycasts = false, UIMouseTooltipEdges edges = default(UIMouseTooltipEdges), Vector2 appearOffset = default(Vector2), string headerLngId = null)
	{
		if (go == null)
		{
			return null;
		}
		UIMouseTooltip uimouseTooltip = go.GetComponent<UIMouseTooltip>();
		if (uimouseTooltip == null)
		{
			uimouseTooltip = go.AddComponent<UIMouseTooltip>();
			uimouseTooltip.lngId = lngId;
		}
		else
		{
			uimouseTooltip.SetLocalizationId(lngId);
		}
		uimouseTooltip.overrideTarget = overrideTarget;
		uimouseTooltip.left = edges.left;
		uimouseTooltip.right = edges.right;
		uimouseTooltip.top = edges.top;
		uimouseTooltip.bottom = edges.bottom;
		uimouseTooltip.appearOffset = appearOffset;
		uimouseTooltip.headerLngId = headerLngId;
		uimouseTooltip.customShow = null;
		if (addRaycastTarget)
		{
			UIMouseTooltip.EnsureRaycastTarget(go);
		}
		if (disableChildRaycasts)
		{
			UIMouseTooltip.DisableChildRaycasts(go);
		}
		uimouseTooltip.ApplyEdgePadding();
		return uimouseTooltip;
	}

	// Token: 0x060035FF RID: 13823 RVA: 0x001033B0 File Offset: 0x001015B0
	public static UIMouseTooltip AttachCustom(GameObject go, Action show, bool addRaycastTarget = false, bool disableChildRaycasts = false, UIMouseTooltipEdges edges = default(UIMouseTooltipEdges), Vector2 appearOffset = default(Vector2))
	{
		UIMouseTooltip uimouseTooltip = UIMouseTooltip.Attach(go, string.Empty, null, addRaycastTarget, disableChildRaycasts, edges, appearOffset, null);
		if (uimouseTooltip != null)
		{
			uimouseTooltip.customShow = show;
		}
		return uimouseTooltip;
	}

	// Token: 0x06003600 RID: 13824 RVA: 0x001033E2 File Offset: 0x001015E2
	public void SetLocalizationId(string lngId)
	{
		if (this.lngId == lngId)
		{
			return;
		}
		this.HideTooltip(true);
		this.lngId = lngId;
	}

	// Token: 0x06003601 RID: 13825 RVA: 0x00103401 File Offset: 0x00101601
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.TryShowTooltip();
	}

	// Token: 0x06003602 RID: 13826 RVA: 0x00103409 File Offset: 0x00101609
	public void OnPointerExit(PointerEventData eventData)
	{
		if (!eventData.fullyExited)
		{
			return;
		}
		this.HideTooltip(false);
		this.TryShowUnderPointer(eventData);
	}

	// Token: 0x06003603 RID: 13827 RVA: 0x00103422 File Offset: 0x00101622
	private void OnEnable()
	{
		LazyInput.OnInputChanged += this.OnInputChanged;
		GameSettings.OnLanguageChanged += this.OnLanguageChanged;
		this.ApplyEdgePadding();
	}

	// Token: 0x06003604 RID: 13828 RVA: 0x0010344C File Offset: 0x0010164C
	private void OnDisable()
	{
		LazyInput.OnInputChanged -= this.OnInputChanged;
		GameSettings.OnLanguageChanged -= this.OnLanguageChanged;
		this.HideTooltip(true);
	}

	// Token: 0x06003605 RID: 13829 RVA: 0x00103477 File Offset: 0x00101677
	private void OnInputChanged()
	{
		if (!UIMouseTooltip.IsAvailable)
		{
			this.HideTooltip(true);
		}
	}

	// Token: 0x06003606 RID: 13830 RVA: 0x00103487 File Offset: 0x00101687
	private void OnLanguageChanged()
	{
		this.HideTooltip(true);
	}

	// Token: 0x06003607 RID: 13831 RVA: 0x00103490 File Offset: 0x00101690
	private void TryShowTooltip()
	{
		if (this.customShow != null)
		{
			if (UIMouseTooltip.IsAvailable)
			{
				this.customShow();
			}
			return;
		}
		UIMouseTooltip.TryShow(this.Target, this.lngId, this.GetResolvedAppearOffset(), this.headerLngId);
	}

	// Token: 0x06003608 RID: 13832 RVA: 0x001034CB File Offset: 0x001016CB
	public Vector2 GetResolvedAppearOffset()
	{
		return new Vector2(this.appearOffset.x, this.appearOffset.y - this.top);
	}

	// Token: 0x06003609 RID: 13833 RVA: 0x001034EF File Offset: 0x001016EF
	private void HideTooltip(bool immediately)
	{
		UIMouseTooltip.HideIfShowingAt(this.Target, immediately);
	}

	// Token: 0x1700080D RID: 2061
	// (get) Token: 0x0600360A RID: 13834 RVA: 0x001034FD File Offset: 0x001016FD
	public static bool IsAvailable
	{
		get
		{
			return !LazyInput.IsGamepadActive;
		}
	}

	// Token: 0x0600360B RID: 13835 RVA: 0x00103508 File Offset: 0x00101708
	public static bool TryShow(RectTransform target, string lngId, Vector2 appearOffset = default(Vector2), string headerLngId = null)
	{
		if (target == null || !UIMouseTooltip.IsAvailable)
		{
			return false;
		}
		if (!LL.HasLocalizedValueForCurrentLang(lngId))
		{
			return false;
		}
		string text = null;
		if (!string.IsNullOrEmpty(headerLngId) && LL.HasLocalizedValueForCurrentLang(headerLngId))
		{
			text = LLBase.L(headerLngId);
		}
		UITooltip.ShowSimpleInfo(target, LLBase.L(lngId), appearOffset, text);
		return true;
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x00103559 File Offset: 0x00101759
	public static void HideIfShowingAt(RectTransform target, bool immediately)
	{
		if (target == null)
		{
			return;
		}
		if (!UITooltip.IsTooltipShowingAtTarget(target))
		{
			return;
		}
		if (immediately)
		{
			UITooltip.HideImmediately();
			return;
		}
		UITooltip.Hide();
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x0010357C File Offset: 0x0010177C
	private void TryShowUnderPointer(PointerEventData eventData)
	{
		GameObject gameObject = eventData.pointerCurrentRaycast.gameObject;
		if (gameObject == null)
		{
			return;
		}
		UIMouseTooltip componentInParent = gameObject.GetComponentInParent<UIMouseTooltip>();
		if (componentInParent == null || componentInParent == this || !componentInParent.isActiveAndEnabled)
		{
			return;
		}
		componentInParent.TryShowTooltip();
	}

	// Token: 0x0600360E RID: 13838 RVA: 0x001035CC File Offset: 0x001017CC
	public static RectTransform GetOrCreateOverlay(RectTransform parent, string name)
	{
		if (parent == null)
		{
			return null;
		}
		RectTransform rectTransform = parent.Find(name) as RectTransform;
		if (rectTransform != null)
		{
			return rectTransform;
		}
		GameObject gameObject = new GameObject(name);
		RectTransform rectTransform2 = gameObject.AddComponent<RectTransform>();
		rectTransform2.SetParent(parent, false);
		gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
		return rectTransform2;
	}

	// Token: 0x0600360F RID: 13839 RVA: 0x00103618 File Offset: 0x00101818
	public static void FitOverlayToWorldRects(RectTransform overlay, IReadOnlyList<RectTransform> targets)
	{
		if (overlay == null || targets == null || targets.Count == 0)
		{
			return;
		}
		Vector3[] array = new Vector3[4];
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		float num3 = float.MinValue;
		float num4 = float.MinValue;
		Vector3 vector = overlay.position;
		bool flag = false;
		for (int i = 0; i < targets.Count; i++)
		{
			if (!(targets[i] == null))
			{
				targets[i].GetWorldCorners(array);
				num = Mathf.Min(new float[]
				{
					num,
					array[0].x,
					array[1].x
				});
				num3 = Mathf.Max(new float[]
				{
					num3,
					array[2].x,
					array[3].x
				});
				num2 = Mathf.Min(new float[]
				{
					num2,
					array[0].y,
					array[3].y
				});
				num4 = Mathf.Max(new float[]
				{
					num4,
					array[1].y,
					array[2].y
				});
				vector = array[0];
				flag = true;
			}
		}
		if (!flag)
		{
			return;
		}
		overlay.pivot = new Vector2(0.5f, 0.5f);
		overlay.anchorMin = new Vector2(0.5f, 0.5f);
		overlay.anchorMax = new Vector2(0.5f, 0.5f);
		overlay.position = new Vector3((num + num3) * 0.5f, (num2 + num4) * 0.5f, vector.z);
		Vector3 lossyScale = overlay.lossyScale;
		float num5 = Mathf.Abs(num3 - num) / Mathf.Max(Mathf.Abs(lossyScale.x), 0.0001f);
		float num6 = Mathf.Abs(num4 - num2) / Mathf.Max(Mathf.Abs(lossyScale.y), 0.0001f);
		overlay.sizeDelta = new Vector2(num5, num6);
	}

	// Token: 0x06003610 RID: 13840 RVA: 0x0010382C File Offset: 0x00101A2C
	public static void FitOverlayExtendRight(RectTransform overlay, RectTransform source, float widthMultiplier)
	{
		if (overlay == null || source == null)
		{
			return;
		}
		Vector3[] array = new Vector3[4];
		source.GetWorldCorners(array);
		float num = Mathf.Min(new float[]
		{
			array[0].x,
			array[1].x,
			array[2].x,
			array[3].x
		});
		float num2 = Mathf.Max(new float[]
		{
			array[0].x,
			array[1].x,
			array[2].x,
			array[3].x
		});
		float num3 = Mathf.Min(new float[]
		{
			array[0].y,
			array[1].y,
			array[2].y,
			array[3].y
		});
		float num4 = Mathf.Max(new float[]
		{
			array[0].y,
			array[1].y,
			array[2].y,
			array[3].y
		});
		Vector3 lossyScale = overlay.lossyScale;
		float num5 = Mathf.Abs(num2 - num) * widthMultiplier / Mathf.Max(Mathf.Abs(lossyScale.x), 0.0001f);
		float num6 = Mathf.Abs(num4 - num3) / Mathf.Max(Mathf.Abs(lossyScale.y), 0.0001f);
		overlay.pivot = new Vector2(0f, 0.5f);
		overlay.anchorMin = new Vector2(0.5f, 0.5f);
		overlay.anchorMax = new Vector2(0.5f, 0.5f);
		overlay.position = new Vector3(num, (num3 + num4) * 0.5f, array[0].z);
		overlay.sizeDelta = new Vector2(num5, num6);
	}

	// Token: 0x06003611 RID: 13841 RVA: 0x00103A38 File Offset: 0x00101C38
	public static void FitOverlayToPreferredSize(RectTransform overlay, RectTransform source)
	{
		if (overlay == null || source == null)
		{
			return;
		}
		overlay.pivot = new Vector2(0.5f, 0.5f);
		overlay.anchorMin = new Vector2(0.5f, 0.5f);
		overlay.anchorMax = new Vector2(0.5f, 0.5f);
		overlay.anchoredPosition = Vector2.zero;
		overlay.sizeDelta = new Vector2(Mathf.Max(LayoutUtility.GetPreferredWidth(source), 1f), Mathf.Max(LayoutUtility.GetPreferredHeight(source), 1f));
	}

	// Token: 0x06003612 RID: 13842 RVA: 0x00103AD0 File Offset: 0x00101CD0
	private void ApplyEdgePadding()
	{
		Graphic graphic = base.GetComponent<Image>();
		if (graphic == null)
		{
			graphic = base.GetComponent<Graphic>();
		}
		if (graphic == null)
		{
			return;
		}
		graphic.raycastPadding = new Vector4(this.left, this.bottom, this.right, this.top);
	}

	// Token: 0x06003613 RID: 13843 RVA: 0x00103B24 File Offset: 0x00101D24
	private static void EnsureRaycastTarget(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		Graphic component = go.GetComponent<Graphic>();
		if (!(component == null))
		{
			component.raycastTarget = true;
			return;
		}
		Image image = go.AddComponent<Image>();
		if (image == null)
		{
			return;
		}
		image.color = Color.clear;
		image.raycastTarget = true;
	}

	// Token: 0x06003614 RID: 13844 RVA: 0x00103B78 File Offset: 0x00101D78
	private static void DisableChildRaycasts(GameObject go)
	{
		Graphic[] componentsInChildren = go.GetComponentsInChildren<Graphic>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!(componentsInChildren[i].gameObject == go))
			{
				componentsInChildren[i].raycastTarget = false;
			}
		}
	}

	// Token: 0x04002B43 RID: 11075
	[SerializeField]
	private string lngId;

	// Token: 0x04002B44 RID: 11076
	[SerializeField]
	private RectTransform overrideTarget;

	// Token: 0x04002B45 RID: 11077
	[SerializeField]
	private float left;

	// Token: 0x04002B46 RID: 11078
	[SerializeField]
	private float right;

	// Token: 0x04002B47 RID: 11079
	[SerializeField]
	private float top;

	// Token: 0x04002B48 RID: 11080
	[SerializeField]
	private float bottom;

	// Token: 0x04002B49 RID: 11081
	[SerializeField]
	private Vector2 appearOffset;

	// Token: 0x04002B4A RID: 11082
	[SerializeField]
	private string headerLngId;

	// Token: 0x04002B4B RID: 11083
	private Action customShow;
}
