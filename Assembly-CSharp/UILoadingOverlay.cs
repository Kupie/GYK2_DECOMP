using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009F4 RID: 2548
public class UILoadingOverlay : LazyWidget<LoadingWindowData>
{
	// Token: 0x060044B0 RID: 17584 RVA: 0x001458F9 File Offset: 0x00143AF9
	protected override void SetData(LoadingWindowData data)
	{
		base.SetData(data);
		this.onAnimationComplete = data.OnAnimationComplete;
		this.SetNotShowDuringCrossSceneActive(!data.IsCrossSceneLoading);
	}

	// Token: 0x060044B1 RID: 17585 RVA: 0x0014591D File Offset: 0x00143B1D
	private void Awake()
	{
		this.UpdateDemoDependentStuff();
	}

	// Token: 0x060044B2 RID: 17586 RVA: 0x00145925 File Offset: 0x00143B25
	private void UpdateDemoDependentStuff()
	{
		this.demoLogo.SetActive(false);
		this.releaseLogo.SetActive(true);
	}

	// Token: 0x060044B3 RID: 17587 RVA: 0x0014593F File Offset: 0x00143B3F
	public override void Draw(LoadingWindowData data)
	{
		this.canUpdateProgress = false;
		this.hadNormalFramesAtStall = false;
		this.stallHitchPending = false;
		this.SetPhase(UILoadingOverlay.SaveLoadProgressPhase.FadeIn);
		base.Draw(data);
		this.ApplyProgress(0f);
		this.isShown = true;
		UILoadingOverlay.SetNotDirectlyInGame(true);
	}

	// Token: 0x060044B4 RID: 17588 RVA: 0x0014597C File Offset: 0x00143B7C
	public override void Redraw()
	{
		base.Redraw();
		this.canvasGroup.DOKill(false);
		this.canvasGroup.alpha = 0f;
		this.ApplyProgress(0f);
		this.canvasGroup.DOFade(1f, this.windowFadeTime).SetUpdate(true).OnComplete(delegate
		{
			this.CompleteShowAsync().Forget();
		});
	}

	// Token: 0x060044B5 RID: 17589 RVA: 0x001459E8 File Offset: 0x00143BE8
	protected void Update()
	{
		this.TickPhase();
		float num = this.EvaluateProgress();
		if (!this.isShown || !this.canUpdateProgress || LoadingPipeline.Instance == null)
		{
			return;
		}
		this.ApplyProgress(num);
	}

	// Token: 0x17000A7C RID: 2684
	// (get) Token: 0x060044B6 RID: 17590 RVA: 0x00145A21 File Offset: 0x00143C21
	public bool IsShown
	{
		get
		{
			return this.isShown;
		}
	}

	// Token: 0x060044B7 RID: 17591 RVA: 0x00145A29 File Offset: 0x00143C29
	public void NotifyAfterSceneWorkStarted()
	{
		this.SetPhase(UILoadingOverlay.SaveLoadProgressPhase.AfterSceneLoaded);
	}

	// Token: 0x060044B8 RID: 17592 RVA: 0x00145A34 File Offset: 0x00143C34
	public override void Hide()
	{
		if (!this.isShown)
		{
			base.Hide();
			return;
		}
		this.isShown = false;
		this.canUpdateProgress = false;
		this.onAnimationComplete = null;
		Slider slider = this.progressSlider;
		if (slider != null)
		{
			slider.DOKill(false);
		}
		this.canvasGroup.DOKill(false);
		UILoadingOverlay.SetNotDirectlyInGame(false);
		this.SetPhase(UILoadingOverlay.SaveLoadProgressPhase.Complete);
		this.canvasGroup.DOFade(0f, this.windowFadeTime).SetUpdate(true).OnComplete(delegate
		{
			base.Hide();
		});
	}

	// Token: 0x060044B9 RID: 17593 RVA: 0x00145AC0 File Offset: 0x00143CC0
	private async UniTaskVoid CompleteShowAsync()
	{
		this.canUpdateProgress = true;
		await UniTask.NextFrame();
		Action action = this.onAnimationComplete;
		if (action != null)
		{
			action();
		}
	}

	// Token: 0x060044BA RID: 17594 RVA: 0x00145B04 File Offset: 0x00143D04
	private void TickPhase()
	{
		if (!this.isShown)
		{
			return;
		}
		switch (this.phase)
		{
		case UILoadingOverlay.SaveLoadProgressPhase.FadeIn:
			if (this.canUpdateProgress)
			{
				this.SetPhase(UILoadingOverlay.HasGameplaySceneLoad() ? UILoadingOverlay.SaveLoadProgressPhase.SceneAsync : UILoadingOverlay.SaveLoadProgressPhase.AwaitingScene);
				return;
			}
			break;
		case UILoadingOverlay.SaveLoadProgressPhase.AwaitingScene:
			if (UILoadingOverlay.IsGameplaySceneLoadComplete())
			{
				this.SetPhase(UILoadingOverlay.SaveLoadProgressPhase.Complete);
				return;
			}
			if (UILoadingOverlay.HasGameplaySceneLoad())
			{
				this.SetPhase(UILoadingOverlay.SaveLoadProgressPhase.SceneAsync);
				return;
			}
			break;
		case UILoadingOverlay.SaveLoadProgressPhase.SceneAsync:
			if (UILoadingOverlay.IsGameplaySceneLoadComplete())
			{
				this.SetPhase(UILoadingOverlay.SaveLoadProgressPhase.Complete);
				return;
			}
			if (UILoadingOverlay.GetSceneAsyncMappedProgress() >= 0.35999998f)
			{
				this.SetPhase(UILoadingOverlay.SaveLoadProgressPhase.StallAtHalf);
				return;
			}
			break;
		case UILoadingOverlay.SaveLoadProgressPhase.StallAtHalf:
			if (UILoadingOverlay.IsGameplaySceneLoadComplete())
			{
				this.SetPhase(UILoadingOverlay.SaveLoadProgressPhase.Complete);
				return;
			}
			if (Time.unscaledDeltaTime >= 1f)
			{
				if (this.hadNormalFramesAtStall)
				{
					this.stallHitchPending = true;
					return;
				}
			}
			else
			{
				this.hadNormalFramesAtStall = true;
				this.phaseProgress = Mathf.Min(this.phaseProgress + 0.005f, 0.65f);
				if (this.stallHitchPending)
				{
					this.SetPhase(UILoadingOverlay.SaveLoadProgressPhase.PostStallJump);
					return;
				}
			}
			break;
		case UILoadingOverlay.SaveLoadProgressPhase.PostStallJump:
			this.SetPhase(UILoadingOverlay.SaveLoadProgressPhase.PostStallCrawl);
			return;
		case UILoadingOverlay.SaveLoadProgressPhase.PostStallCrawl:
			this.phaseProgress = Mathf.Min(this.phaseProgress + 0.005f, 0.85f);
			if (UILoadingOverlay.IsGameplaySceneLoadComplete())
			{
				this.SetPhase(UILoadingOverlay.SaveLoadProgressPhase.Complete);
				return;
			}
			break;
		case UILoadingOverlay.SaveLoadProgressPhase.AfterSceneLoaded:
			if (UILoadingOverlay.IsGameplaySceneLoadComplete())
			{
				this.SetPhase(UILoadingOverlay.SaveLoadProgressPhase.Complete);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060044BB RID: 17595 RVA: 0x00145C4C File Offset: 0x00143E4C
	private float EvaluateProgress()
	{
		switch (this.phase)
		{
		case UILoadingOverlay.SaveLoadProgressPhase.FadeIn:
		case UILoadingOverlay.SaveLoadProgressPhase.AwaitingScene:
			return UILoadingOverlay.GetBackgroundHoldProgress();
		case UILoadingOverlay.SaveLoadProgressPhase.SceneAsync:
			return UILoadingOverlay.GetSceneAsyncMappedProgress();
		case UILoadingOverlay.SaveLoadProgressPhase.StallAtHalf:
			return this.phaseProgress;
		case UILoadingOverlay.SaveLoadProgressPhase.PostStallJump:
			return 0.75f;
		case UILoadingOverlay.SaveLoadProgressPhase.PostStallCrawl:
			return this.phaseProgress;
		case UILoadingOverlay.SaveLoadProgressPhase.AfterSceneLoaded:
			return 0.9f;
		case UILoadingOverlay.SaveLoadProgressPhase.Complete:
			return 1f;
		default:
			return 0f;
		}
	}

	// Token: 0x060044BC RID: 17596 RVA: 0x00145CBC File Offset: 0x00143EBC
	private void SetPhase(UILoadingOverlay.SaveLoadProgressPhase next)
	{
		if (next != UILoadingOverlay.SaveLoadProgressPhase.FadeIn && next <= this.phase)
		{
			return;
		}
		this.phase = next;
		switch (next)
		{
		case UILoadingOverlay.SaveLoadProgressPhase.FadeIn:
			this.hadNormalFramesAtStall = false;
			this.stallHitchPending = false;
			this.phaseProgress = 0f;
			return;
		case UILoadingOverlay.SaveLoadProgressPhase.AwaitingScene:
		case UILoadingOverlay.SaveLoadProgressPhase.SceneAsync:
		case UILoadingOverlay.SaveLoadProgressPhase.PostStallCrawl:
			break;
		case UILoadingOverlay.SaveLoadProgressPhase.StallAtHalf:
			this.phaseProgress = 0.38f;
			return;
		case UILoadingOverlay.SaveLoadProgressPhase.PostStallJump:
			this.phaseProgress = 0.75f;
			return;
		case UILoadingOverlay.SaveLoadProgressPhase.AfterSceneLoaded:
			this.phaseProgress = 0.9f;
			return;
		case UILoadingOverlay.SaveLoadProgressPhase.Complete:
			this.phaseProgress = 1f;
			break;
		default:
			return;
		}
	}

	// Token: 0x060044BD RID: 17597 RVA: 0x00145D50 File Offset: 0x00143F50
	private void ApplyProgress(float normalized01)
	{
		if (this.progressSlider == null)
		{
			return;
		}
		normalized01 = Mathf.Clamp01(normalized01);
		if (Mathf.Approximately(this.progressSliderTargetValue, normalized01))
		{
			return;
		}
		this.progressSliderTargetValue = normalized01;
		this.progressSlider.DOKill(false);
		UILoadingOverlay.SaveLoadProgressPhase saveLoadProgressPhase = this.phase;
		if (saveLoadProgressPhase == UILoadingOverlay.SaveLoadProgressPhase.StallAtHalf || saveLoadProgressPhase == UILoadingOverlay.SaveLoadProgressPhase.PostStallJump || saveLoadProgressPhase == UILoadingOverlay.SaveLoadProgressPhase.PostStallCrawl || Mathf.Approximately(normalized01, 0f))
		{
			this.progressSlider.value = normalized01;
			return;
		}
		this.progressSlider.DOValue(normalized01, 0.2f, false).SetUpdate(true);
	}

	// Token: 0x060044BE RID: 17598 RVA: 0x00145DE8 File Offset: 0x00143FE8
	private static float GetBackgroundHoldProgress()
	{
		LoadingPipeline instance = LoadingPipeline.Instance;
		if (instance == null)
		{
			return 0.3f;
		}
		if (!instance.IsStageComplete(LoadingStage.BackgroundPreload))
		{
			return instance.GetStageProgress(LoadingStage.BackgroundPreload) * 0.3f;
		}
		return 0.3f;
	}

	// Token: 0x060044BF RID: 17599 RVA: 0x00145E20 File Offset: 0x00144020
	private static float GetSceneAsyncMappedProgress()
	{
		LoadingPipeline instance = LoadingPipeline.Instance;
		if (instance == null || !instance.HasStage(LoadingStage.GameplaySceneLoad))
		{
			return 0.3f;
		}
		float stageProgress = instance.GetStageProgress(LoadingStage.GameplaySceneLoad);
		if (stageProgress >= 0.8f)
		{
			return 0.38f;
		}
		return Mathf.Lerp(0.3f, 0.38f, stageProgress / 0.8f);
	}

	// Token: 0x060044C0 RID: 17600 RVA: 0x00145E71 File Offset: 0x00144071
	private static bool HasGameplaySceneLoad()
	{
		return LoadingPipeline.Instance != null && LoadingPipeline.Instance.HasStage(LoadingStage.GameplaySceneLoad);
	}

	// Token: 0x060044C1 RID: 17601 RVA: 0x00145E87 File Offset: 0x00144087
	private static bool IsGameplaySceneLoadComplete()
	{
		return LoadingPipeline.Instance != null && LoadingPipeline.Instance.IsStageComplete(LoadingStage.GameplaySceneLoad);
	}

	// Token: 0x060044C2 RID: 17602 RVA: 0x00145EA0 File Offset: 0x001440A0
	private void SetNotShowDuringCrossSceneActive(bool isActive)
	{
		if (this.notShowDuringCrossScene == null)
		{
			return;
		}
		foreach (GameObject gameObject in this.notShowDuringCrossScene)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(isActive);
			}
		}
	}

	// Token: 0x060044C3 RID: 17603 RVA: 0x00070D6F File Offset: 0x0006EF6F
	private static void SetNotDirectlyInGame(bool active)
	{
		if (WeatherSystem.Instance == null)
		{
			return;
		}
		WeatherSystem.Instance.AudioMixerStateController.SetLayerActive(AudioMixerSnapshotLayer.NotDirectlyInGame, active);
	}

	// Token: 0x060044C4 RID: 17604 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x0400358B RID: 13707
	private const float BACKGROUND_PRELOAD_WEIGHT = 0.3f;

	// Token: 0x0400358C RID: 13708
	private const float SCENE_STALL_PROGRESS = 0.38f;

	// Token: 0x0400358D RID: 13709
	private const float SCENE_STALL_CAP = 0.65f;

	// Token: 0x0400358E RID: 13710
	private const float SCENE_POST_STALL_JUMP = 0.75f;

	// Token: 0x0400358F RID: 13711
	private const float SCENE_POST_STALL_CAP = 0.85f;

	// Token: 0x04003590 RID: 13712
	private const float SCENE_POST_STALL_STEP = 0.005f;

	// Token: 0x04003591 RID: 13713
	private const float SCENE_ASYNC_PROGRESS_END = 0.8f;

	// Token: 0x04003592 RID: 13714
	private const float AFTER_SCENE_PROGRESS = 0.9f;

	// Token: 0x04003593 RID: 13715
	private const float HITCH_JUMP_UDT_THRESHOLD = 1f;

	// Token: 0x04003594 RID: 13716
	private const float PROGRESS_SLIDER_ANIMATION_TIME = 0.2f;

	// Token: 0x04003595 RID: 13717
	[SerializeField]
	private RectTransform rootRect;

	// Token: 0x04003596 RID: 13718
	[SerializeField]
	private GameObject[] notShowDuringCrossScene;

	// Token: 0x04003597 RID: 13719
	[SerializeField]
	private float windowFadeTime = 0.4f;

	// Token: 0x04003598 RID: 13720
	[SerializeField]
	private GameObject releaseLogo;

	// Token: 0x04003599 RID: 13721
	[SerializeField]
	private GameObject demoLogo;

	// Token: 0x0400359A RID: 13722
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x0400359B RID: 13723
	[SerializeField]
	private Slider progressSlider;

	// Token: 0x0400359C RID: 13724
	private Action onAnimationComplete;

	// Token: 0x0400359D RID: 13725
	private float progressSliderTargetValue = -1f;

	// Token: 0x0400359E RID: 13726
	private bool isShown;

	// Token: 0x0400359F RID: 13727
	private bool canUpdateProgress;

	// Token: 0x040035A0 RID: 13728
	private UILoadingOverlay.SaveLoadProgressPhase phase;

	// Token: 0x040035A1 RID: 13729
	private float phaseProgress;

	// Token: 0x040035A2 RID: 13730
	private bool hadNormalFramesAtStall;

	// Token: 0x040035A3 RID: 13731
	private bool stallHitchPending;

	// Token: 0x020009F5 RID: 2549
	public enum SaveLoadProgressPhase
	{
		// Token: 0x040035A5 RID: 13733
		Inactive,
		// Token: 0x040035A6 RID: 13734
		FadeIn,
		// Token: 0x040035A7 RID: 13735
		AwaitingScene,
		// Token: 0x040035A8 RID: 13736
		SceneAsync,
		// Token: 0x040035A9 RID: 13737
		StallAtHalf,
		// Token: 0x040035AA RID: 13738
		PostStallJump,
		// Token: 0x040035AB RID: 13739
		PostStallCrawl,
		// Token: 0x040035AC RID: 13740
		AfterSceneLoaded,
		// Token: 0x040035AD RID: 13741
		Complete
	}
}
