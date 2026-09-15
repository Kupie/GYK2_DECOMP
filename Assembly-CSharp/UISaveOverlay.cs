using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x02000A2A RID: 2602
public class UISaveOverlay : MonoBehaviour, ILazyGUIElement
{
	// Token: 0x0600461E RID: 17950 RVA: 0x0014BE99 File Offset: 0x0014A099
	public void Init()
	{
		SaveSystem.OnSaveWriteStarted += this.EnableOverlay;
		SaveSystem.OnSaveWriteStartedInstant += this.EnableOverlayInstant;
		SaveSystem.OnSaveWriteEnded += this.DisableOverlay;
	}

	// Token: 0x0600461F RID: 17951 RVA: 0x0014BECE File Offset: 0x0014A0CE
	protected void Update()
	{
		if (this.isTimerActive && Time.unscaledTime - this.timerStartTime >= this.timer)
		{
			this.isTimerActive = false;
			Action action = this.onTimerComplete;
			if (action == null)
			{
				return;
			}
			action();
		}
	}

	// Token: 0x06004620 RID: 17952 RVA: 0x0014BF04 File Offset: 0x0014A104
	private void EnableOverlay()
	{
		if (this.isShown)
		{
			return;
		}
		base.gameObject.SetActive(true);
		this.isShown = true;
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOFade(1f, this.fadeDuration).SetUpdate(true);
		this.openedTime = Time.unscaledTime;
	}

	// Token: 0x06004621 RID: 17953 RVA: 0x0014BF68 File Offset: 0x0014A168
	private void EnableOverlayInstant()
	{
		if (this.isShown)
		{
			return;
		}
		base.gameObject.SetActive(true);
		this.isShown = true;
		this.canvasGroup.DOKill(false);
		this.canvasGroup.alpha = 1f;
		this.openedTime = Time.unscaledTime;
	}

	// Token: 0x06004622 RID: 17954 RVA: 0x0014BFBC File Offset: 0x0014A1BC
	private void DisableOverlay()
	{
		if (!this.isShown)
		{
			return;
		}
		float num = Time.unscaledTime - this.openedTime;
		this.timer = Mathf.Clamp(this.showTime, 0f, this.showTime - num);
		this.isTimerActive = true;
		this.timerStartTime = Time.unscaledTime;
		this.onTimerComplete = delegate
		{
			this.canvasGroup.alpha = 1f;
			TweenerCore<float, float, FloatOptions> tweenerCore = this.canvasGroup.DOFade(0f, this.fadeDuration);
			tweenerCore.onComplete = delegate
			{
				base.gameObject.SetActive(false);
				this.isShown = false;
			};
			tweenerCore.SetUpdate(true);
		};
	}

	// Token: 0x040036D6 RID: 14038
	public CanvasGroup canvasGroup;

	// Token: 0x040036D7 RID: 14039
	public float fadeDuration = 0.25f;

	// Token: 0x040036D8 RID: 14040
	public float showTime = 1.5f;

	// Token: 0x040036D9 RID: 14041
	private float openedTime;

	// Token: 0x040036DA RID: 14042
	private bool isTimerActive;

	// Token: 0x040036DB RID: 14043
	private float timerStartTime;

	// Token: 0x040036DC RID: 14044
	private float timer;

	// Token: 0x040036DD RID: 14045
	private Action onTimerComplete;

	// Token: 0x040036DE RID: 14046
	private bool isShown;
}
