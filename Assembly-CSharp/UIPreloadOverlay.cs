using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A14 RID: 2580
public class UIPreloadOverlay : MonoBehaviour
{
	// Token: 0x0600457C RID: 17788 RVA: 0x001489C0 File Offset: 0x00146BC0
	private void Awake()
	{
		this.UpdateDemoDependentStuff();
	}

	// Token: 0x0600457D RID: 17789 RVA: 0x001489C8 File Offset: 0x00146BC8
	public async UniTask Open()
	{
		base.gameObject.SetActive(true);
		this.SetPhase(UIPreloadOverlay.StartupProgressPhase.FadeIn);
		this.SetProgressImmediate(0f);
		this.canvasGroup.DOKill(false);
		this.canvasGroup.alpha = 0f;
		UniTaskCompletionSource fadeCompletion = new UniTaskCompletionSource();
		this.canvasGroup.DOFade(1f, this.fadeInTime).SetUpdate(true).OnComplete(delegate
		{
			fadeCompletion.TrySetResult();
		})
			.OnKill(delegate
			{
				fadeCompletion.TrySetResult();
			});
		await fadeCompletion.Task;
		this.isUpdateEnabled = true;
	}

	// Token: 0x0600457E RID: 17790 RVA: 0x00148A0C File Offset: 0x00146C0C
	public async UniTask ShowInitialProgressAndWaitFrame()
	{
		this.SetPhase(UIPreloadOverlay.StartupProgressPhase.InitialVisible);
		this.SetProgressImmediate(0.15f);
		await UniTask.NextFrame();
	}

	// Token: 0x0600457F RID: 17791 RVA: 0x00148A4F File Offset: 0x00146C4F
	public void JumpToDownloadDependenciesLoaded()
	{
		this.SetPhase(UIPreloadOverlay.StartupProgressPhase.DownloadLoaded);
		this.SetProgressImmediate(Mathf.Max(this.progressSliderTargetValue, 0.3f));
	}

	// Token: 0x06004580 RID: 17792 RVA: 0x00148A6E File Offset: 0x00146C6E
	public void JumpToMainSceneLoaded()
	{
		this.SetPhase(UIPreloadOverlay.StartupProgressPhase.MainSceneLoaded);
		this.SetProgressImmediate(0.75f);
	}

	// Token: 0x06004581 RID: 17793 RVA: 0x00148A82 File Offset: 0x00146C82
	protected void Update()
	{
		this.TickPhase();
	}

	// Token: 0x06004582 RID: 17794 RVA: 0x00148A8C File Offset: 0x00146C8C
	private void TickPhase()
	{
		if (!this.isUpdateEnabled)
		{
			return;
		}
		LoadingPipeline instance = LoadingPipeline.Instance;
		bool flag = instance != null && instance.HasStage(this.loadingStage);
		bool flag2 = instance != null && instance.IsStageComplete(this.loadingStage);
		switch (this.phase)
		{
		case UIPreloadOverlay.StartupProgressPhase.InitialVisible:
			if (flag)
			{
				this.SetPhase(UIPreloadOverlay.StartupProgressPhase.PreMainMenuCrawl);
				return;
			}
			break;
		case UIPreloadOverlay.StartupProgressPhase.PreMainMenuCrawl:
		case UIPreloadOverlay.StartupProgressPhase.DownloadLoaded:
			if (flag2)
			{
				this.JumpToMainSceneLoaded();
				return;
			}
			this.SetProgressImmediate(Mathf.Min(this.progressSliderTargetValue + 0.002f, 0.6f));
			return;
		case UIPreloadOverlay.StartupProgressPhase.MainSceneLoaded:
			if (flag2)
			{
				this.SetProgressImmediate(0.75f);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06004583 RID: 17795 RVA: 0x00148B2E File Offset: 0x00146D2E
	private void SetPhase(UIPreloadOverlay.StartupProgressPhase next)
	{
		if (next == this.phase)
		{
			return;
		}
		if (next != UIPreloadOverlay.StartupProgressPhase.FadeIn && next != UIPreloadOverlay.StartupProgressPhase.Inactive && next <= this.phase)
		{
			return;
		}
		this.phase = next;
	}

	// Token: 0x06004584 RID: 17796 RVA: 0x00148B52 File Offset: 0x00146D52
	private void UpdateDemoDependentStuff()
	{
		this.demoLogo.SetActive(false);
		this.releaseLogo.SetActive(true);
	}

	// Token: 0x06004585 RID: 17797 RVA: 0x00148B6C File Offset: 0x00146D6C
	private void SetProgressImmediate(float normalized01)
	{
		normalized01 = Mathf.Clamp01(normalized01);
		this.progressSlider.DOKill(false);
		this.progressSliderTargetValue = normalized01;
		this.progressSlider.value = normalized01;
		if (this.progressSlider.fillRect != null)
		{
			this.progressSlider.fillRect.gameObject.SetActive(normalized01 > 0f);
		}
	}

	// Token: 0x17000AA2 RID: 2722
	// (get) Token: 0x06004586 RID: 17798 RVA: 0x00148BD1 File Offset: 0x00146DD1
	public float FadeOutTime
	{
		get
		{
			return this.fadeOutTime;
		}
	}

	// Token: 0x06004587 RID: 17799 RVA: 0x00148BDC File Offset: 0x00146DDC
	public RectTransform GetActiveLogoRect()
	{
		GameObject activeLogoRoot = this.GetActiveLogoRoot();
		if (activeLogoRoot == null)
		{
			return null;
		}
		Transform transform = activeLogoRoot.transform.Find("Logo");
		if (!(transform != null))
		{
			return (RectTransform)activeLogoRoot.transform;
		}
		return (RectTransform)transform;
	}

	// Token: 0x06004588 RID: 17800 RVA: 0x00148C28 File Offset: 0x00146E28
	public void HideActiveLogo()
	{
		GameObject activeLogoRoot = this.GetActiveLogoRoot();
		if (activeLogoRoot != null)
		{
			activeLogoRoot.SetActive(false);
		}
	}

	// Token: 0x06004589 RID: 17801 RVA: 0x00148C4C File Offset: 0x00146E4C
	public async UniTask CompleteProgressBar()
	{
		this.isUpdateEnabled = false;
		this.SetPhase(UIPreloadOverlay.StartupProgressPhase.Closing);
		UniTaskCompletionSource sliderCompletion = new UniTaskCompletionSource();
		this.progressSlider.DOKill(false);
		this.progressSlider.DOValue(1f, this.finalProgressSliderAnimationTime, false).SetUpdate(true).OnComplete(delegate
		{
			sliderCompletion.TrySetResult();
		})
			.OnKill(delegate
			{
				sliderCompletion.TrySetResult();
			});
		await sliderCompletion.Task;
	}

	// Token: 0x0600458A RID: 17802 RVA: 0x00148C90 File Offset: 0x00146E90
	public async UniTask FadeOut()
	{
		UniTaskCompletionSource fadeCompletion = new UniTaskCompletionSource();
		this.canvasGroup.DOKill(false);
		this.canvasGroup.DOFade(0f, this.fadeOutTime).SetUpdate(true).OnComplete(delegate
		{
			this.gameObject.SetActive(false);
			fadeCompletion.TrySetResult();
		})
			.OnKill(delegate
			{
				fadeCompletion.TrySetResult();
			});
		await fadeCompletion.Task;
		this.SetPhase(UIPreloadOverlay.StartupProgressPhase.Inactive);
	}

	// Token: 0x0600458B RID: 17803 RVA: 0x00148CD4 File Offset: 0x00146ED4
	public async UniTask Close()
	{
		await this.CompleteProgressBar();
		await this.FadeOut();
	}

	// Token: 0x0600458C RID: 17804 RVA: 0x00148D17 File Offset: 0x00146F17
	private GameObject GetActiveLogoRoot()
	{
		if (!this.releaseLogo.activeSelf)
		{
			return this.demoLogo;
		}
		return this.releaseLogo;
	}

	// Token: 0x04003645 RID: 13893
	[SerializeField]
	private Slider progressSlider;

	// Token: 0x04003646 RID: 13894
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x04003647 RID: 13895
	[SerializeField]
	private float fadeInTime = 0.3f;

	// Token: 0x04003648 RID: 13896
	[SerializeField]
	private float fadeOutTime = 0.5f;

	// Token: 0x04003649 RID: 13897
	[SerializeField]
	private float progressSliderAnimationTime = 0.2f;

	// Token: 0x0400364A RID: 13898
	[SerializeField]
	private float finalProgressSliderAnimationTime = 0.2f;

	// Token: 0x0400364B RID: 13899
	[SerializeField]
	private GameObject releaseLogo;

	// Token: 0x0400364C RID: 13900
	[SerializeField]
	private GameObject demoLogo;

	// Token: 0x0400364D RID: 13901
	private const float INITIAL_VISIBLE_PROGRESS = 0.15f;

	// Token: 0x0400364E RID: 13902
	private const float PRE_MAIN_MENU_PROGRESS_STEP = 0.002f;

	// Token: 0x0400364F RID: 13903
	private const float PRE_MAIN_MENU_PROGRESS_CAP = 0.6f;

	// Token: 0x04003650 RID: 13904
	public const float DOWNLOAD_DEPENDENCIES_WEIGHT = 0.3f;

	// Token: 0x04003651 RID: 13905
	public const float MAIN_SCENE_LOAD_WEIGHT = 0.75f;

	// Token: 0x04003652 RID: 13906
	private float progressSliderTargetValue = -1f;

	// Token: 0x04003653 RID: 13907
	private LoadingStage loadingStage;

	// Token: 0x04003654 RID: 13908
	private bool isUpdateEnabled;

	// Token: 0x04003655 RID: 13909
	private UIPreloadOverlay.StartupProgressPhase phase;

	// Token: 0x02000A15 RID: 2581
	public enum StartupProgressPhase
	{
		// Token: 0x04003657 RID: 13911
		Inactive,
		// Token: 0x04003658 RID: 13912
		FadeIn,
		// Token: 0x04003659 RID: 13913
		InitialVisible,
		// Token: 0x0400365A RID: 13914
		PreMainMenuCrawl,
		// Token: 0x0400365B RID: 13915
		DownloadLoaded,
		// Token: 0x0400365C RID: 13916
		MainSceneLoaded,
		// Token: 0x0400365D RID: 13917
		Closing
	}
}
