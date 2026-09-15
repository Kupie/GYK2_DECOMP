using System;
using UnityEngine;

// Token: 0x020007D8 RID: 2008
public class UICraftHintCompletionProgressCell : MonoBehaviour
{
	// Token: 0x170007C8 RID: 1992
	// (get) Token: 0x060033AD RID: 13229 RVA: 0x000F93ED File Offset: 0x000F75ED
	public CanvasGroup CanvasGroup
	{
		get
		{
			this.EnsureReferences();
			return this.canvasGroup;
		}
	}

	// Token: 0x170007C9 RID: 1993
	// (get) Token: 0x060033AE RID: 13230 RVA: 0x000F93FB File Offset: 0x000F75FB
	public RectTransform RectTransform
	{
		get
		{
			this.EnsureReferences();
			return this.rectTransform;
		}
	}

	// Token: 0x060033AF RID: 13231 RVA: 0x000F9409 File Offset: 0x000F7609
	public void ShowOver(RectTransform source, RectTransform parent)
	{
		this.EnsureReferences();
		this.CopyRectTransform(source, parent);
		this.SetAlpha(0f);
		base.gameObject.SetActive(true);
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x000F9430 File Offset: 0x000F7630
	public void SetAlpha(float alpha)
	{
		this.canvasGroup.alpha = alpha;
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x000F9440 File Offset: 0x000F7640
	private void EnsureReferences()
	{
		if (this.canvasGroup == null && !base.TryGetComponent<CanvasGroup>(out this.canvasGroup))
		{
			this.canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
		}
		if (this.rectTransform == null)
		{
			this.rectTransform = base.transform as RectTransform;
		}
	}

	// Token: 0x060033B2 RID: 13234 RVA: 0x000F949C File Offset: 0x000F769C
	private void CopyRectTransform(RectTransform source, RectTransform parent)
	{
		RectTransform rectTransform = this.rectTransform;
		RectTransform rectTransform2 = this.rectTransform;
		Vector2 vector = new Vector2(0.5f, 0.5f);
		rectTransform2.anchorMax = vector;
		rectTransform.anchorMin = vector;
		this.rectTransform.pivot = source.pivot;
		this.rectTransform.sizeDelta = source.rect.size;
		this.rectTransform.rotation = source.rotation;
		this.rectTransform.localScale = source.localScale;
		this.rectTransform.anchoredPosition = this.GetLocalCenter(source, parent);
	}

	// Token: 0x060033B3 RID: 13235 RVA: 0x000F9534 File Offset: 0x000F7734
	private Vector2 GetLocalCenter(RectTransform source, RectTransform parent)
	{
		Vector3 vector = source.TransformPoint(source.rect.center);
		return parent.InverseTransformPoint(vector);
	}

	// Token: 0x0400293D RID: 10557
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x0400293E RID: 10558
	[SerializeField]
	private RectTransform rectTransform;
}
