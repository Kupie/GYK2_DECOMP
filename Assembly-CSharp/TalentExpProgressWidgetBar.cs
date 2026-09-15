using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200091F RID: 2335
public class TalentExpProgressWidgetBar : MonoBehaviour
{
	// Token: 0x06003D93 RID: 15763 RVA: 0x00126378 File Offset: 0x00124578
	public void PlayFillFromCenter(Color filledColor, float duration, Action onComplete = null, Action onInterrupted = null)
	{
		this.KillFillTween();
		base.transform.localScale = Vector3.one;
		Image image = this.EnsureFillOverlay();
		this.SyncOverlayWithImage(image);
		image.color = filledColor;
		image.rectTransform.localScale = new Vector3(0f, 1f, 1f);
		image.gameObject.SetActive(true);
		if (duration > 0f)
		{
			LazyAudio.PlayAndForget("blimp_grow");
			bool finished = false;
			this.fillTween = image.rectTransform.DOScaleX(1f, duration).SetEase(Ease.OutCubic).SetLink(base.gameObject, LinkBehaviour.KillOnDisable)
				.OnComplete(delegate
				{
					base.<PlayFillFromCenter>g__Finish|0(true);
				})
				.OnKill(delegate
				{
					base.<PlayFillFromCenter>g__Finish|0(false);
				});
			return;
		}
		this.FinishFill(filledColor);
		Action onComplete2 = onComplete;
		if (onComplete2 == null)
		{
			return;
		}
		onComplete2();
	}

	// Token: 0x06003D94 RID: 15764 RVA: 0x00126485 File Offset: 0x00124685
	public void ResetVisual(Color color)
	{
		this.StopFillAnimation();
		if (this.image != null)
		{
			this.image.color = color;
		}
	}

	// Token: 0x06003D95 RID: 15765 RVA: 0x001264A7 File Offset: 0x001246A7
	public void StopFillAnimation()
	{
		this.KillFillTween();
		base.transform.localScale = Vector3.one;
		this.HideFillOverlay();
	}

	// Token: 0x06003D96 RID: 15766 RVA: 0x001264C5 File Offset: 0x001246C5
	private void FinishFill(Color filledColor)
	{
		if (this.image != null)
		{
			this.image.color = filledColor;
		}
		this.HideFillOverlay();
	}

	// Token: 0x06003D97 RID: 15767 RVA: 0x001264E8 File Offset: 0x001246E8
	private Image EnsureFillOverlay()
	{
		if (this.fillOverlay != null)
		{
			return this.fillOverlay;
		}
		GameObject gameObject = new GameObject("FillOverlay", new Type[]
		{
			typeof(RectTransform),
			typeof(CanvasRenderer),
			typeof(Image)
		});
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.SetParent(base.transform, false);
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.offsetMin = Vector2.zero;
		component.offsetMax = Vector2.zero;
		component.pivot = new Vector2(0.5f, 0.5f);
		component.localScale = Vector3.one;
		this.fillOverlay = gameObject.GetComponent<Image>();
		this.fillOverlay.raycastTarget = false;
		gameObject.SetActive(false);
		return this.fillOverlay;
	}

	// Token: 0x06003D98 RID: 15768 RVA: 0x001265C8 File Offset: 0x001247C8
	private void SyncOverlayWithImage(Image overlay)
	{
		if (this.image == null)
		{
			return;
		}
		overlay.sprite = this.image.sprite;
		overlay.type = this.image.type;
		overlay.preserveAspect = this.image.preserveAspect;
		overlay.pixelsPerUnitMultiplier = this.image.pixelsPerUnitMultiplier;
		overlay.material = this.image.material;
	}

	// Token: 0x06003D99 RID: 15769 RVA: 0x00126639 File Offset: 0x00124839
	private void HideFillOverlay()
	{
		if (this.fillOverlay == null)
		{
			return;
		}
		this.fillOverlay.rectTransform.localScale = Vector3.one;
		this.fillOverlay.gameObject.SetActive(false);
	}

	// Token: 0x06003D9A RID: 15770 RVA: 0x00126670 File Offset: 0x00124870
	private void OnDisable()
	{
		this.KillFillTween();
		this.HideFillOverlay();
		base.transform.localScale = Vector3.one;
	}

	// Token: 0x06003D9B RID: 15771 RVA: 0x00126690 File Offset: 0x00124890
	private void KillFillTween()
	{
		Tween tween = this.fillTween;
		this.fillTween = null;
		if (tween != null && tween.IsActive())
		{
			tween.Kill(false);
		}
	}

	// Token: 0x04003062 RID: 12386
	public Image image;

	// Token: 0x04003063 RID: 12387
	private Image fillOverlay;

	// Token: 0x04003064 RID: 12388
	private Tween fillTween;
}
