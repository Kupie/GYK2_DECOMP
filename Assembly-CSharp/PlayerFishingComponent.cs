using System;
using System.Collections;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200038D RID: 909
public class PlayerFishingComponent : MonoBehaviour
{
	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x06001848 RID: 6216 RVA: 0x000727AB File Offset: 0x000709AB
	public bool IsMiniGameActive
	{
		get
		{
			FishingMiniGame fishingMiniGame = this.miniGame;
			return fishingMiniGame != null && fishingMiniGame.IsActive;
		}
	}

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06001849 RID: 6217 RVA: 0x000727BE File Offset: 0x000709BE
	// (set) Token: 0x0600184A RID: 6218 RVA: 0x000727C6 File Offset: 0x000709C6
	public FishingMiniGame MiniGame
	{
		get
		{
			return this.miniGame;
		}
		set
		{
			this.miniGame = value;
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x0600184B RID: 6219 RVA: 0x000727CF File Offset: 0x000709CF
	private FishingContainer FishingContainer
	{
		get
		{
			PlayerView playerView = this.playerView;
			if (playerView == null)
			{
				return null;
			}
			return playerView.FishingContainer;
		}
	}

	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x0600184C RID: 6220 RVA: 0x000727E4 File Offset: 0x000709E4
	private ReservoirConfig ReservoirConfig
	{
		get
		{
			ReservoirView component = this.reservoir.MainWgoPart.GetComponent<ReservoirView>();
			if (!(component != null))
			{
				return LazySingletonSerializedSO<FishingSettings>.Instance.DefaultReservoirConfig;
			}
			return component.Config;
		}
	}

	// Token: 0x0600184D RID: 6221 RVA: 0x0007281C File Offset: 0x00070A1C
	public void StartActivity(Wgo wgo)
	{
		MainGame.PlayerController.SetControlTakenType(TakenControlType.ByFishing, false);
		this.playerView = MainGame.PlayerController.View;
		this.fishingSettings = LazySingletonSerializedSO<FishingSettings>.Instance;
		this.reservoir = wgo;
		this.playerView.PlayerAnimation.SetDirection(this.reservoir.TryGetDockPointForWorker(false, default(Vector3)).Direction);
		this.playerView.PlayerAnimation.SetState(global::AnimationState.FishingStart);
		this.isStarted = true;
	}

	// Token: 0x0600184E RID: 6222 RVA: 0x000728A0 File Offset: 0x00070AA0
	public void StopActivity(bool forceStop = false, Action onFinish = null)
	{
		if (forceStop)
		{
			FishingMiniGame fishingMiniGame = this.miniGame;
			if (fishingMiniGame != null)
			{
				fishingMiniGame.CancelGame();
			}
			base.StartCoroutine(this.OnFinishMiniGame(false, onFinish));
			this.needToMoveToStart = true;
			this.isStarted = false;
			LazyUI.GetWindow<UIFishingWindow>().Close();
			this.playerView.FishingContainer.gameObject.SetActive(false);
			MainGame.PlayerController.SetControlTakenType(TakenControlType.ByFishing, true);
			this.playerView.PlayerAnimation.SetState(global::AnimationState.Idle);
			return;
		}
		if (this.miniGame != null && this.miniGame.IsActive)
		{
			this.miniGame.FinishGame();
			return;
		}
		if (this.miniGame != null)
		{
			this.miniGame.CancelGame();
		}
		this.needToMoveToStart = true;
		this.isStarted = false;
		LazyUI.GetWindow<UIFishingWindow>().Close();
		this.playerView.FishingContainer.gameObject.SetActive(false);
		MainGame.PlayerController.SetControlTakenType(TakenControlType.ByFishing, true);
		this.playerView.PlayerAnimation.SetState(global::AnimationState.Idle);
	}

	// Token: 0x0600184F RID: 6223 RVA: 0x0007299C File Offset: 0x00070B9C
	protected void LateUpdate()
	{
		if (!this.isStarted)
		{
			return;
		}
		FishingMiniGame fishingMiniGame = this.miniGame;
		if (fishingMiniGame != null)
		{
			fishingMiniGame.UpdateGame();
		}
		if (this.IsMiniGameActive)
		{
			FishingMiniGame fishingMiniGame2 = this.miniGame;
			if (fishingMiniGame2 != null && fishingMiniGame2.CurrentStage == FishingMiniGame.Stage.PlayingWithFish)
			{
				FishingContainer fishingContainer = this.FishingContainer;
				if (((fishingContainer != null) ? fishingContainer.Bob : null) != null && this.ReservoirConfig != null)
				{
					Vector3 vector = new Vector3(MainGame.PlayerData.Direction.x, 0f, 0f);
					Vector3 vector2 = this.bobStartPos - vector * (Mathf.Clamp(this.miniGame.progress / 100f, -1f, 1f) * this.ReservoirConfig.progressRange);
					FishingMiniGame fishingMiniGame3 = this.miniGame;
					float num = ((fishingMiniGame3 != null) ? fishingMiniGame3.IdleDriftOffset : 0f);
					FishingMiniGame fishingMiniGame4 = this.miniGame;
					Vector3 vector3 = ((fishingMiniGame4 != null) ? fishingMiniGame4.IdleDriftAxis : Vector3.forward);
					Vector3 vector4 = vector2 + vector3 * num;
					this.FishingContainer.Bob.transform.position = vector4;
				}
				float tension = this.miniGame.tension;
				float num2 = Mathf.Max(this.ropeDefaultEmission, this.ropeEmissionByTension.Evaluate(tension / 100f));
				FishingContainer fishingContainer2 = this.FishingContainer;
				if (fishingContainer2 != null)
				{
					fishingContainer2.SetRopeEmission(num2);
				}
				if (tension >= this.ropeBlinkTensionAmount)
				{
					if (!this.isBlinking)
					{
						this.isBlinking = true;
						this.blinkTimer = 0f;
					}
					this.blinkTimer += Time.deltaTime * this.blinkSpeed;
					this.blinkTimer = MathUtilities.ClampCycle(this.blinkTimer, 0f, 1f);
					Color color = this.fishingSettings.ropeGradient.Evaluate(this.blinkTimer);
					FishingContainer fishingContainer3 = this.FishingContainer;
					if (fishingContainer3 == null)
					{
						return;
					}
					fishingContainer3.SetRopeColor(color);
					return;
				}
				else
				{
					this.isBlinking = false;
					Color color2 = this.fishingSettings.ropeGradient.Evaluate(Mathf.Clamp01(tension / 100f) * 0.8f);
					FishingContainer fishingContainer4 = this.FishingContainer;
					if (fishingContainer4 == null)
					{
						return;
					}
					fishingContainer4.SetRopeColor(color2);
					return;
				}
			}
		}
		if (!this.IsMiniGameActive && this.FishingContainer.gameObject.activeInHierarchy && this.needToMoveToStart)
		{
			this.BringBobToStart();
		}
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x00072BEC File Offset: 0x00070DEC
	private void BringBobToStart()
	{
		this.moveProgress += Time.deltaTime / this.bobMovementDuration;
		float num = Mathf.Clamp01(this.moveProgress);
		float num2 = this.bobMovementCurve.Evaluate(num);
		Vector3 vector = Vector3.Lerp(this.FishingContainer.CastStartPoint.position, this.bobStartPos, num2);
		float num3 = this.bobDivingCurve.Evaluate(num) * this.bobDivingAmount;
		Vector3 vector2 = vector;
		vector2.y += num3;
		this.FishingContainer.Bob.transform.position = vector2;
		if (num2 >= 0.2f && !this.hasActivatedAtPeak)
		{
			FXContainerEventListener fxcontainerEventListener;
			if (this.playerView.PlayerAnimation.Animator.TryGetComponent<FXContainerEventListener>(out fxcontainerEventListener))
			{
				this.fishingCatchFx.position = new Vector3(this.FishingContainer.Bob.GetWorldPosition().x, this.bobStartPos.y + this.fishingCatchFxVertOffset, this.FishingContainer.Bob.GetWorldPosition().z);
				fxcontainerEventListener.PlayFx("fishing_catch");
				LazyAudio.PlayAtGameObject("fishing_blop", base.transform, SpatialType.sound3D, true);
			}
			this.hasActivatedAtPeak = true;
		}
		if (this.moveProgress >= 1f)
		{
			this.needToMoveToStart = false;
			this.moveProgress = 0f;
			this.hasActivatedAtPeak = false;
		}
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x00072D45 File Offset: 0x00070F45
	public void RunMiniGame(FishingDef fishingDef, WgoData wgoData, ItemDef fishingRodDef)
	{
		base.StartCoroutine(this.StartMiniGame(fishingDef, wgoData, fishingRodDef));
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x00072D57 File Offset: 0x00070F57
	private IEnumerator StartMiniGame(FishingDef fishingDef, WgoData wgoData, ItemDef fishingRodDef)
	{
		UIFishingWindow fishingWindow = LazyUI.GetWindow<UIFishingWindow>();
		fishingWindow.SetWindowVisible(false);
		this.bobStartPos = this.GetBobStartPos();
		this.playerView.PlayerAnimation.SetState(global::AnimationState.FishingCast);
		this.playerView.FishingContainer.AnimationComponent.SetTrigger("fishCast");
		yield return new WaitUntil(() => !this.needToMoveToStart);
		LazyAudio.PlayAtGameObject("fishing_bite_success", base.transform, SpatialType.sound3D, true);
		this.playerView.FishingContainer.AnimationComponent.SetTrigger("fishStart");
		this.playerView.PlayerAnimation.SetState(global::AnimationState.FishingIdle);
		this.miniGame = new FishingMiniGame();
		this.miniGame.StartNewGame(fishingDef, wgoData, fishingRodDef, new Action<FishingMiniGame.Stage>(this.OnPhaseChange));
		fishingWindow.SetWindowVisible(true);
		yield break;
	}

	// Token: 0x06001853 RID: 6227 RVA: 0x00072D7B File Offset: 0x00070F7B
	private IEnumerator DoBiteAnimation()
	{
		Vector3 startPos = this.playerView.FishingContainer.Bob.transform.position;
		float timer = 0f;
		while (timer < this.biteAnimationDuration)
		{
			timer += Time.deltaTime;
			float num = timer / this.biteAnimationDuration;
			float num2 = this.biteAnimationCurve.Evaluate(num);
			Vector3 vector = startPos + Vector3.up * (num2 * this.biteDiveAmount);
			this.playerView.FishingContainer.Bob.transform.position = vector;
			yield return null;
		}
		this.playerView.FishingContainer.Bob.transform.position = startPos;
		yield return new WaitForSeconds(this.bitePauseAfterReturn);
		yield break;
	}

	// Token: 0x06001854 RID: 6228 RVA: 0x00072D8A File Offset: 0x00070F8A
	private IEnumerator OnFinishMiniGame(bool openChoicePanel = false, Action onFinish = null)
	{
		UIFishingWindow fishingWindow = LazyUI.GetWindow<UIFishingWindow>();
		this.fishingCatchFx.position = new Vector3(this.FishingContainer.Bob.GetWorldPosition().x, this.bobStartPos.y + this.fishingCatchFxVertOffset, this.FishingContainer.Bob.GetWorldPosition().z);
		this.FishingContainer.Bob.transform.position = this.FishingContainer.CastStartPoint.position;
		if (openChoicePanel)
		{
			fishingWindow.SetWindowVisible(false);
		}
		for (;;)
		{
			this.playerView.PlayerAnimation.SetState(global::AnimationState.FishingCatch);
			if (this.playerView.PlayerAnimation.Animator.GetCurrentAnimatorStateInfo(0).IsName("FishingCatch"))
			{
				break;
			}
			yield return null;
		}
		yield return null;
		while (!Mathf.Clamp01(this.playerView.PlayerAnimation.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime).EqualsTo(1f, 0.01f))
		{
			yield return null;
		}
		this.needToMoveToStart = true;
		if (openChoicePanel)
		{
			fishingWindow.DisplayChoicePanel();
			fishingWindow.SetWindowVisible(true);
		}
		else
		{
			MainGame.PlayerController.SetControlTakenType(TakenControlType.ByFishing, true);
			fishingWindow.Close();
			this.playerView.PlayerAnimation.SetState(global::AnimationState.Idle);
		}
		yield return null;
		if (onFinish != null)
		{
			onFinish();
		}
		yield break;
	}

	// Token: 0x06001855 RID: 6229 RVA: 0x00072DA8 File Offset: 0x00070FA8
	private void OnPhaseChange(FishingMiniGame.Stage newStage)
	{
		switch (newStage)
		{
		case FishingMiniGame.Stage.Start:
			this.playerView.PlayerAnimation.SetState(global::AnimationState.FishingIdle);
			LazyUI.GetWindow<UIFishingWindow>().DisplayChoicePanel();
			return;
		case FishingMiniGame.Stage.WaitingForBite:
		case FishingMiniGame.Stage.PlayingWithFish:
			break;
		case FishingMiniGame.Stage.Biting:
			base.StartCoroutine(this.DoBiteAnimation());
			return;
		case FishingMiniGame.Stage.Finish:
			this.FishingContainer.gameObject.SetActive(false);
			this.FishingContainer.ResetColor();
			this.FishingContainer.SetRopeEmission(this.ropeDefaultEmission);
			base.StartCoroutine(this.OnFinishMiniGame(true, null));
			break;
		default:
			return;
		}
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x00072E3C File Offset: 0x0007103C
	private Vector3 GetBobStartPos()
	{
		Vector3 vector = new Vector3(MainGame.PlayerData.Direction.x, 0f, 0f);
		return MainGame.PlayerController.transform.position + vector.normalized * this.ReservoirConfig.fishSpawnHorOffset + Vector3.down * this.ReservoirConfig.fishSpawnVertOffset;
	}

	// Token: 0x040017C6 RID: 6086
	[Header("Mini-game")]
	[SerializeField]
	private Transform fishingCatchFx;

	// Token: 0x040017C7 RID: 6087
	[SerializeField]
	private float fishingCatchFxVertOffset = -0.18f;

	// Token: 0x040017C8 RID: 6088
	[SerializeField]
	private FishingMiniGame miniGame;

	// Token: 0x040017C9 RID: 6089
	[SerializeField]
	private float blinkSpeed = 10f;

	// Token: 0x040017CA RID: 6090
	private PlayerView playerView;

	// Token: 0x040017CB RID: 6091
	private Wgo reservoir;

	// Token: 0x040017CC RID: 6092
	private bool isBlinking;

	// Token: 0x040017CD RID: 6093
	private float blinkTimer;

	// Token: 0x040017CE RID: 6094
	private float moveProgress;

	// Token: 0x040017CF RID: 6095
	private Vector3 bobStartPos;

	// Token: 0x040017D0 RID: 6096
	private bool needToMoveToStart = true;

	// Token: 0x040017D1 RID: 6097
	private FishingSettings fishingSettings;

	// Token: 0x040017D2 RID: 6098
	private bool isStarted;

	// Token: 0x040017D3 RID: 6099
	private bool hasActivatedAtPeak;

	// Token: 0x040017D4 RID: 6100
	[Header("Bob Movement")]
	[SerializeField]
	private float bobMovementDuration = 0.1f;

	// Token: 0x040017D5 RID: 6101
	[SerializeField]
	private float bobDivingAmount = 0.3f;

	// Token: 0x040017D6 RID: 6102
	[SerializeField]
	private AnimationCurve bobMovementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	// Token: 0x040017D7 RID: 6103
	[SerializeField]
	private AnimationCurve bobDivingCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.5f, -1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x040017D8 RID: 6104
	[Header("Bite Animation")]
	[SerializeField]
	private AnimationCurve biteAnimationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.5f, -1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x040017D9 RID: 6105
	[SerializeField]
	private float biteAnimationDuration = 0.4f;

	// Token: 0x040017DA RID: 6106
	[SerializeField]
	private float biteDiveAmount = 0.2f;

	// Token: 0x040017DB RID: 6107
	[SerializeField]
	private float bitePauseAfterDive = 0.1f;

	// Token: 0x040017DC RID: 6108
	[SerializeField]
	private float bitePauseAfterReturn = 0.3f;

	// Token: 0x040017DD RID: 6109
	[Header("Rope Blink")]
	[SerializeField]
	private float ropeBlinkTensionAmount = 85f;

	// Token: 0x040017DE RID: 6110
	[SerializeField]
	private float ropeDefaultEmission = 0.3f;

	// Token: 0x040017DF RID: 6111
	[SerializeField]
	private AnimationCurve ropeEmissionByTension = AnimationCurve.Linear(0f, 0f, 1f, 1f);
}
