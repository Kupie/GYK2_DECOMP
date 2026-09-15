using System;
using System.Collections;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000405 RID: 1029
[Serializable]
public class FishingMiniGame
{
	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x0007CD66 File Offset: 0x0007AF66
	public FishingMiniGame.Stage CurrentStage
	{
		get
		{
			return this.currentStage;
		}
	}

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06001AD7 RID: 6871 RVA: 0x0007CD70 File Offset: 0x0007AF70
	public bool IsActive
	{
		get
		{
			FishingMiniGame.Stage stage = this.currentStage;
			return stage != FishingMiniGame.Stage.Start && stage != FishingMiniGame.Stage.Finish;
		}
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x06001AD8 RID: 6872 RVA: 0x0007CD90 File Offset: 0x0007AF90
	public bool IsFishResisting
	{
		get
		{
			return this.isFishResisting;
		}
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06001AD9 RID: 6873 RVA: 0x0007CD98 File Offset: 0x0007AF98
	public float IdleDriftOffset
	{
		get
		{
			return this.idleDriftOffset;
		}
	}

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06001ADA RID: 6874 RVA: 0x0007CDA0 File Offset: 0x0007AFA0
	public Vector3 IdleDriftAxis
	{
		get
		{
			return this.idleDriftAxis;
		}
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x0007CDA8 File Offset: 0x0007AFA8
	public void StartNewGame(FishingDef fishingDef, WgoData reservoir, ItemDef fishingRod, Action<FishingMiniGame.Stage> OnMiniGameStageChanged)
	{
		this.fishingDef = fishingDef;
		this.reservoir = reservoir;
		this.fishingRod = fishingRod;
		this.fishingContainer = MainGame.PlayerController.View.FishingContainer;
		this.fishingSettings = LazySingletonSerializedSO<FishingSettings>.Instance;
		this.fishUnderwaterGfx = this.fishingContainer.FishUnderwaterGfx;
		this.SetSignActive(false);
		this.OnStageChanged = (Action<FishingMiniGame.Stage>)Delegate.Combine(this.OnStageChanged, OnMiniGameStageChanged);
		this.playerAnim = MainGame.PlayerController.View.PlayerAnimation;
		this.gameTime = 0f;
		this.splashTimer = 0f;
		this.splashResistActive = false;
		this.playerTalentLevel = MainGame.PlayerController.GetMasteryLevelForTalentBranch("talent_green", null);
		this.fishAnger = ((this.fishingDef == null) ? 0 : fishingDef.anger.EvaluateInt());
		this.fishWaitTime = ((this.fishingDef == null) ? (-1f) : this.GetRandomFloatInRange(fishingDef.WaitTimeLeft, fishingDef.WaitTimeRight, 2));
		this.fishIdleTime = ((this.fishingDef == null) ? (-1f) : this.GetRandomFloatInRange(fishingDef.IntervalTimeLeft, fishingDef.IntervalTimeRight, 2));
		this.fishResistTime = ((this.fishingDef == null) ? (-1f) : this.GetRandomFloatInRange(fishingDef.ResistTimeLeft, fishingDef.ResistTimeRight, 2));
		this.usePhaseDurations = this.fishIdleTime > 0f && this.fishResistTime > 0f;
		this.fishUnderwaterGfx.UpdateGfx(FishGfxUnderwaterType.None);
		this.ResetIdleDrift(false);
		this.StartGameFlow(OnMiniGameStageChanged);
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x0007CF34 File Offset: 0x0007B134
	private float GetRandomFloatInRange(float min, float max, int decimalPlaces = 2)
	{
		if (min >= max)
		{
			return min;
		}
		float num = Mathf.Clamp(global::UnityEngine.Random.Range(min, max), min, max);
		float num2 = Mathf.Pow(10f, (float)decimalPlaces);
		return Mathf.Round(num * num2) / num2;
	}

	// Token: 0x06001ADD RID: 6877 RVA: 0x0007CF6C File Offset: 0x0007B16C
	public void FinishGame()
	{
		this.StopAllRoutines();
		this.EnterStage(FishingMiniGame.Stage.Finish);
		if (this.fishingContainer != null)
		{
			this.fishingContainer.AnimationComponent.SetLayerWeight(1, 0f);
			this.fishingContainer.AnimationComponent.Animator.SetBool(FishingMiniGame.FishBiting, false);
			this.fishingContainer.AnimationComponent.Animator.SetBool(FishingMiniGame.FishPulling, false);
			this.SetSignActive(false);
		}
		SoundHandler soundHandler = this.reelSound;
		if (soundHandler != null)
		{
			soundHandler.Stop();
		}
		LazyAudio.Stop("fishing_floundering");
		FishUnderwaterGfx fishUnderwaterGfx = this.fishUnderwaterGfx;
		if (fishUnderwaterGfx != null)
		{
			fishUnderwaterGfx.UpdateGfx(FishGfxUnderwaterType.None);
		}
		this.OnStageChanged = null;
	}

	// Token: 0x06001ADE RID: 6878 RVA: 0x0007D01C File Offset: 0x0007B21C
	public void CancelGame()
	{
		this.StopAllRoutines();
		this.OnStageChanged = null;
		this.EnterStage(FishingMiniGame.Stage.Finish);
		if (this.fishingContainer != null)
		{
			this.fishingContainer.AnimationComponent.SetLayerWeight(1, 0f);
			this.fishingContainer.AnimationComponent.Animator.SetBool(FishingMiniGame.FishBiting, false);
			this.fishingContainer.AnimationComponent.Animator.SetBool(FishingMiniGame.FishPulling, false);
			this.SetSignActive(false);
		}
		SoundHandler soundHandler = this.reelSound;
		if (soundHandler != null)
		{
			soundHandler.Stop();
		}
		LazyAudio.Stop("fishing_floundering");
		this.ResetIdleDrift(false);
	}

	// Token: 0x06001ADF RID: 6879 RVA: 0x0007D0C1 File Offset: 0x0007B2C1
	public void UpdateGame()
	{
		this.HandleInput();
		this.UpdateStage(this.currentStage);
		this.UpdateIdleDrift();
	}

	// Token: 0x06001AE0 RID: 6880 RVA: 0x0007D0DC File Offset: 0x0007B2DC
	private void UpdateStage(FishingMiniGame.Stage stage)
	{
		switch (stage)
		{
		case FishingMiniGame.Stage.WaitingForBite:
			if (this.isPlayerPulling)
			{
				this.HandleFailure(false);
				return;
			}
			return;
		case FishingMiniGame.Stage.Biting:
			this.SetSignActive(true);
			if (this.isPlayerPulling)
			{
				this.EnterStage(FishingMiniGame.Stage.PlayingWithFish);
			}
			this.fishUnderwaterGfx.UpdateGfx(this.fishingDef.underwaterGfxType);
			return;
		case FishingMiniGame.Stage.PlayingWithFish:
		{
			this.isFishResisting = this.resistance > 0.05f;
			this.UpdateFishSplashes();
			this.playerAnim.SetState(this.isPlayerPulling ? global::AnimationState.FishingPull : global::AnimationState.FishingRelease);
			bool flag = this.isPlayerPulling || this.isFishResisting;
			if (flag && !this.isReelSoundStarted)
			{
				SoundHandler soundHandler = this.reelSound;
				if (soundHandler != null)
				{
					soundHandler.Stop();
				}
				this.reelSound = LazyAudio.PlayAtGameObject("fishing_reel_long", this.playerAnim.transform, SpatialType.sound3D, true);
				this.isReelSoundStarted = true;
			}
			else if (!flag && this.isReelSoundStarted)
			{
				SoundHandler soundHandler2 = this.reelSound;
				if (soundHandler2 != null)
				{
					soundHandler2.Stop();
				}
				this.isReelSoundStarted = false;
			}
			this.fishingContainer.AnimationComponent.Animator.SetBool(FishingMiniGame.PlayerPulling, this.isPlayerPulling);
			this.fishingContainer.AnimationComponent.Animator.SetBool(FishingMiniGame.FishPulling, this.isFishResisting);
			this.SetSignActive(!this.isFishResisting);
			this.UpdateProgress();
			return;
		}
		case FishingMiniGame.Stage.Finish:
			this.ResetIdleDrift(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x0007D254 File Offset: 0x0007B454
	private void EnterStage(FishingMiniGame.Stage newStage)
	{
		this.currentStage = newStage;
		switch (this.currentStage)
		{
		case FishingMiniGame.Stage.Biting:
			LazyAudio.PlayAtGameObject("fishing_bite", this.playerAnim.transform, SpatialType.sound3D, true);
			this.fishingContainer.AnimationComponent.Animator.SetBool(FishingMiniGame.FishBiting, true);
			this.SetSignActive(true);
			break;
		case FishingMiniGame.Stage.PlayingWithFish:
			LazyAudio.PlayAtGameObject("fishing_bite_success", this.playerAnim.transform, SpatialType.sound3D, true);
			this.fishingContainer.AnimationComponent.SetLayerWeight(1, 1f);
			this.fishingContainer.AnimationComponent.Animator.SetBool(FishingMiniGame.FishBiting, false);
			this.StartResistanceRoutine();
			this.StartIdleDriftRoutine();
			break;
		case FishingMiniGame.Stage.Finish:
			this.StopResistanceRoutine();
			this.StopIdleDriftRoutine();
			break;
		}
		Action<FishingMiniGame.Stage> onStageChanged = this.OnStageChanged;
		if (onStageChanged == null)
		{
			return;
		}
		onStageChanged(newStage);
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x0007D33C File Offset: 0x0007B53C
	private void SetSignActive(bool active)
	{
		FishingContainer fishingContainer = this.fishingContainer;
		if (!((fishingContainer != null) ? fishingContainer.SignGameObject : null))
		{
			return;
		}
		if (this.fishingContainer.SignGameObject.activeSelf != active)
		{
			this.fishingContainer.SignGameObject.SetActive(active);
		}
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x0007D37C File Offset: 0x0007B57C
	private void HandleInput()
	{
		bool flag = LazyInput.GetKey(GameKey.LeftClick) || LazyInput.GetKey(GameKey.Interaction);
		if (this.IsActive && this.isPlayerPulling != flag)
		{
			if (flag)
			{
				LazyAudio.PlayAtGameObject("fishing_reel_short", this.playerAnim.transform, SpatialType.sound3D, true);
			}
			this.isPlayerPulling = flag;
		}
		if (!this.IsActive)
		{
			this.isPlayerPulling = false;
		}
		if (LazyInput.GetKeyDown(GameKey.Back))
		{
			this.FinishGame();
		}
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x0007D3F8 File Offset: 0x0007B5F8
	private void UpdateFishSplashes()
	{
		if (!this.isFishResisting)
		{
			this.splashTimer = 0f;
			this.splashResistActive = false;
			return;
		}
		float num = ((this.fishingDef != null) ? this.fishingDef.splashCoef : 1f);
		FishingSettings.FishDriftStateSettings fishDriftStateSettings = this.GetCurrentDriftPreset();
		float num2 = ((fishDriftStateSettings != null) ? fishDriftStateSettings.MinSplashPeriodTime : 0.8f);
		if (num2 <= 0f)
		{
			num2 = 0.8f;
		}
		float num3 = num2 / Mathf.Max(num, 0.01f);
		bool flag = !this.splashResistActive;
		this.splashResistActive = true;
		this.splashTimer += Time.deltaTime;
		if (!flag && this.splashTimer < num3)
		{
			return;
		}
		FishingContainer fishingContainer = this.fishingContainer;
		Transform transform = ((((fishingContainer != null) ? fishingContainer.Bob : null) != null) ? this.fishingContainer.Bob.transform : ((this.fishUnderwaterGfx != null) ? this.fishUnderwaterGfx.transform : this.playerAnim.transform));
		if (LazyAudio.PlayAtGameObject("fishing_floundering", transform, SpatialType.sound3D, false) != null)
		{
			this.splashTimer = 0f;
		}
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x0007D50C File Offset: 0x0007B70C
	private void UpdateProgress()
	{
		this.gameTime += Time.deltaTime;
		if (this.usePhaseDurations)
		{
			if (!this.resistance.EqualsTo(this.targetResistance, 1E-05f))
			{
				float num = Mathf.Abs(this.targetResistance - this.resistance) / this.fishingDef.speedChangeTime;
				this.resistance = Mathf.Clamp01(Mathf.MoveTowards(this.resistance, this.targetResistance, num * Time.deltaTime));
			}
		}
		else
		{
			this.resistance = this.fishingSettings.GetCurveById(this.fishingDef.curvePresetId).Evaluate(this.gameTime);
		}
		float num3;
		float num4;
		if (this.isPlayerPulling)
		{
			float num2 = (float)this.playerTalentLevel - (float)this.fishAnger * this.resistance * this.fishingSettings.CurveMultiplier;
			num3 = Mathf.Sign(num2) * Mathf.Pow(Mathf.Abs(num2), 0.5f) * this.fishingSettings.PullingProgressConst;
			num4 = (10f + this.resistance * 500f + (float)(this.fishAnger * 10)) / (5f + 0.1f * (float)(this.fishingRod.talentBonus + this.playerTalentLevel));
			this.fishUnderwaterGfx.SetFishState(FishUnderwaterGfx.FishGfxState.Losing);
		}
		else
		{
			float num5 = (float)(-(float)this.fishAnger) * this.resistance * this.fishingSettings.CurveMultiplier;
			num3 = Mathf.Sign(num5) * Mathf.Pow(Mathf.Abs(num5), 0.5f) * this.fishingSettings.IdleProgressConst;
			num4 = (float)(-(float)(80 + 3 * (this.fishingRod.talentBonus + this.playerTalentLevel)));
			this.fishUnderwaterGfx.SetFishState(FishUnderwaterGfx.FishGfxState.Pulling);
		}
		this.progress = Mathf.Clamp(this.progress + num3 * Time.deltaTime, -100f, 100f);
		this.tension = Mathf.Clamp(this.tension + num4 * Time.deltaTime, 0f, 100f);
		if (this.tension >= 100f)
		{
			this.HandleFailure(true);
			return;
		}
		if (this.progress <= -100f)
		{
			this.HandleFailure(false);
			return;
		}
		if (this.progress >= 100f)
		{
			this.HandleSuccess();
		}
	}

	// Token: 0x06001AE6 RID: 6886 RVA: 0x0007D740 File Offset: 0x0007B940
	private void HandleSuccess()
	{
		this.reservoir.SubGameRes(this.fishingDef.fishId, 1);
		this.reservoir.SetGameRes(this.fishingDef.fishId + "_caught", 1);
		GameLogicData gameLogicData = MainGame.Instance.GameSave.gameLogicSystemData.gameLogics.Find((GameLogicData x) => x.id == this.reservoir.id + "_restore");
		if (gameLogicData != null)
		{
			gameLogicData.Init();
		}
		Item item = new Item(this.fishingDef.fishId, 1);
		this.reservoir.MakeDrop(item);
		this.fishingDef.expressionsOnEnd.ForEach(delegate(LazyExpression x)
		{
			x.Evaluate();
		});
		AchievementsSystem.Instance.TriggerCountable("fish_caught", 1);
		LazyAudio.PlayAtGameObject("fishing_success", this.playerAnim.transform, SpatialType.sound3D, true);
		this.FinishGame();
	}

	// Token: 0x06001AE7 RID: 6887 RVA: 0x0007D830 File Offset: 0x0007BA30
	private void HandleFailure(bool isFishingLineBroke = false)
	{
		LazyAudio.PlayAtGameObject(isFishingLineBroke ? "fishing_line_break" : "fishing_fail", this.playerAnim.transform, SpatialType.sound3D, true);
		this.FinishGame();
		Bubble.Talk(new PhraseData(true, null, LLBase.L("fishing_next_time"), null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
	}

	// Token: 0x06001AE8 RID: 6888 RVA: 0x0007D885 File Offset: 0x0007BA85
	private void StartGameFlow(Action<FishingMiniGame.Stage> stageCallback)
	{
		this.StopAllRoutines();
		this.OnStageChanged = (Action<FishingMiniGame.Stage>)Delegate.Combine(this.OnStageChanged, stageCallback);
		this.gameFlowRoutine = FishingMiniGame.FishingCoroutineRunner.StartRoutine(this.GameFlow());
	}

	// Token: 0x06001AE9 RID: 6889 RVA: 0x0007D8B5 File Offset: 0x0007BAB5
	private IEnumerator GameFlow()
	{
		this.EnterStage(FishingMiniGame.Stage.WaitingForBite);
		if (this.fishWaitTime > 0f)
		{
			yield return new WaitForSeconds(this.fishWaitTime);
		}
		if (!this.IsActive || this.currentStage != FishingMiniGame.Stage.WaitingForBite)
		{
			yield break;
		}
		this.EnterStage(FishingMiniGame.Stage.Biting);
		this.biteTimeoutRoutine = FishingMiniGame.FishingCoroutineRunner.StartRoutine(this.BiteTimeout());
		while (this.IsActive && this.currentStage == FishingMiniGame.Stage.Biting)
		{
			yield return null;
		}
		if (this.biteTimeoutRoutine != null)
		{
			FishingMiniGame.FishingCoroutineRunner.StopRoutine(this.biteTimeoutRoutine);
		}
		if (!this.IsActive || this.currentStage != FishingMiniGame.Stage.PlayingWithFish)
		{
			yield break;
		}
		while (this.IsActive && this.currentStage == FishingMiniGame.Stage.PlayingWithFish)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001AEA RID: 6890 RVA: 0x0007D8C4 File Offset: 0x0007BAC4
	private IEnumerator BiteTimeout()
	{
		yield return new WaitForSeconds(this.fishingDef.baitTime);
		if (this.IsActive && this.currentStage == FishingMiniGame.Stage.Biting)
		{
			this.HandleFailure(false);
		}
		yield break;
	}

	// Token: 0x06001AEB RID: 6891 RVA: 0x0007D8D3 File Offset: 0x0007BAD3
	private void StartResistanceRoutine()
	{
		this.StopResistanceRoutine();
		if (!this.usePhaseDurations)
		{
			this.targetResistance = 0f;
			return;
		}
		this.resistanceRoutine = FishingMiniGame.FishingCoroutineRunner.StartRoutine(this.ResistanceLoop());
	}

	// Token: 0x06001AEC RID: 6892 RVA: 0x0007D900 File Offset: 0x0007BB00
	private void StopResistanceRoutine()
	{
		if (this.resistanceRoutine != null)
		{
			FishingMiniGame.FishingCoroutineRunner.StopRoutine(this.resistanceRoutine);
		}
		this.resistanceRoutine = null;
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x0007D91C File Offset: 0x0007BB1C
	private IEnumerator ResistanceLoop()
	{
		this.targetResistance = 0f;
		while (this.IsActive && this.currentStage == FishingMiniGame.Stage.PlayingWithFish)
		{
			float randomFloatInRange = this.GetRandomFloatInRange(this.fishingDef.IntervalTimeLeft, this.fishingDef.IntervalTimeRight, 2);
			yield return new WaitForSeconds(randomFloatInRange);
			if (!this.IsActive || this.currentStage != FishingMiniGame.Stage.PlayingWithFish)
			{
				yield break;
			}
			this.targetResistance = 1f;
			float randomFloatInRange2 = this.GetRandomFloatInRange(this.fishingDef.ResistTimeLeft, this.fishingDef.ResistTimeRight, 2);
			yield return new WaitForSeconds(randomFloatInRange2);
			if (!this.IsActive || this.currentStage != FishingMiniGame.Stage.PlayingWithFish)
			{
				yield break;
			}
			this.targetResistance = 0f;
		}
		yield break;
	}

	// Token: 0x06001AEE RID: 6894 RVA: 0x0007D92C File Offset: 0x0007BB2C
	private void StopAllRoutines()
	{
		if (this.gameFlowRoutine != null)
		{
			FishingMiniGame.FishingCoroutineRunner.StopRoutine(this.gameFlowRoutine);
		}
		if (this.biteTimeoutRoutine != null)
		{
			FishingMiniGame.FishingCoroutineRunner.StopRoutine(this.biteTimeoutRoutine);
		}
		this.StopResistanceRoutine();
		this.StopIdleDriftRoutine();
		this.gameFlowRoutine = null;
		this.biteTimeoutRoutine = null;
	}

	// Token: 0x06001AEF RID: 6895 RVA: 0x0007D979 File Offset: 0x0007BB79
	private void StartIdleDriftRoutine()
	{
		this.ResetIdleDrift(false);
	}

	// Token: 0x06001AF0 RID: 6896 RVA: 0x0007D979 File Offset: 0x0007BB79
	private void StopIdleDriftRoutine()
	{
		this.ResetIdleDrift(false);
	}

	// Token: 0x06001AF1 RID: 6897 RVA: 0x0007D984 File Offset: 0x0007BB84
	private void ResetIdleDrift(bool smoothToZero = false)
	{
		if (smoothToZero && !this.isFadingOut && Math.Abs(this.idleDriftOffset) > Mathf.Epsilon)
		{
			float num = ((this.fishingSettings != null) ? this.fishingSettings.DriftFadeOutDuration : 0.35f);
			this.isFadingOut = true;
			this.idleDriftOffsetFadeSpeed = ((num > 0f) ? (Mathf.Abs(this.idleDriftOffset) / num) : 1000f);
			return;
		}
		this.idleDriftTimer = 0f;
		this.idleDriftDuration = 0f;
		this.idleDriftPhase = 0f;
		this.idleDriftAmplitude = 0f;
		this.idleDriftAmplitudeTarget = 0f;
		this.idleDriftAmplitudeSpeed = 0f;
		this.idleDriftFrequency = 0f;
		this.idleDriftFrequencyTarget = 0f;
		this.idleDriftOffset = 0f;
		this.idleDriftOffsetFadeSpeed = 0f;
		this.idleDriftAxis = Vector3.forward;
		this.idleDriftAxisTarget = Vector3.forward;
		this.currentDriftPreset = null;
		this.pendingPreset = null;
		this.presetInitialized = false;
		this.isFadingOut = false;
		this.lastOffsetWasPositive = false;
	}

	// Token: 0x06001AF2 RID: 6898 RVA: 0x0007DAA0 File Offset: 0x0007BCA0
	private void UpdateIdleDrift()
	{
		if (this.isFadingOut)
		{
			this.idleDriftOffset = Mathf.MoveTowards(this.idleDriftOffset, 0f, this.idleDriftOffsetFadeSpeed * Time.deltaTime);
			if (Mathf.Abs(this.idleDriftOffset) < Mathf.Epsilon)
			{
				this.isFadingOut = false;
				this.idleDriftOffset = 0f;
				this.idleDriftPhase = (this.lastOffsetWasPositive ? 3.1415927f : 0f);
				this.idleDriftAmplitude = 0f;
				if (this.pendingPreset != null)
				{
					FishingSettings.FishDriftStateSettings fishDriftStateSettings = this.pendingPreset;
					this.pendingPreset = null;
					this.currentDriftPreset = fishDriftStateSettings;
					this.ApplyPresetTargets(fishDriftStateSettings);
					return;
				}
				this.ResetIdleDrift(false);
			}
			return;
		}
		if (!this.IsActive || this.currentStage != FishingMiniGame.Stage.PlayingWithFish)
		{
			this.ResetIdleDrift(true);
			return;
		}
		FishingSettings.FishDriftStateSettings fishDriftStateSettings2 = this.GetCurrentDriftPreset();
		if (this.currentDriftPreset != fishDriftStateSettings2 && this.pendingPreset != fishDriftStateSettings2)
		{
			this.ApplyPresetTargets(fishDriftStateSettings2);
			if (!this.isFadingOut)
			{
				this.currentDriftPreset = fishDriftStateSettings2;
			}
		}
		else if (this.currentDriftPreset != null && this.currentDriftPreset.EnableRandomization && (this.idleDriftDuration <= 0f || this.idleDriftTimer >= this.idleDriftDuration))
		{
			this.RandomizeCurrentPreset();
		}
		this.idleDriftTimer += Time.deltaTime;
		this.idleDriftAmplitude = Mathf.MoveTowards(this.idleDriftAmplitude, this.idleDriftAmplitudeTarget, this.idleDriftAmplitudeSpeed * Time.deltaTime);
		this.idleDriftFrequency = Mathf.MoveTowards(this.idleDriftFrequency, this.idleDriftFrequencyTarget, this.fishingSettings.FrequencyTransitionSpeed * Time.deltaTime);
		this.idleDriftAxis = Vector3.MoveTowards(this.idleDriftAxis, this.idleDriftAxisTarget, this.fishingSettings.AxisTransitionSpeed * Time.deltaTime);
		this.idleDriftPhase += this.idleDriftFrequency * Time.deltaTime;
		if (this.idleDriftPhase > 6.2831855f)
		{
			this.idleDriftPhase -= 6.2831855f;
		}
		this.idleDriftOffset = this.idleDriftAmplitude * Mathf.Sin(this.idleDriftPhase);
	}

	// Token: 0x06001AF3 RID: 6899 RVA: 0x0007DCB0 File Offset: 0x0007BEB0
	private FishingSettings.FishDriftStateSettings GetCurrentDriftPreset()
	{
		bool flag = this.isFishResisting;
		bool flag2 = this.isPlayerPulling;
		if (flag && flag2)
		{
			return this.fishingSettings.BothPullingDrift;
		}
		if (flag && !flag2)
		{
			return this.fishingSettings.FishPullingOnlyDrift;
		}
		return this.fishingSettings.IdleDrift;
	}

	// Token: 0x06001AF4 RID: 6900 RVA: 0x0007DCFC File Offset: 0x0007BEFC
	private void ApplyPresetTargets(FishingSettings.FishDriftStateSettings preset)
	{
		if (this.pendingPreset == null && this.currentDriftPreset == null && Mathf.Abs(this.idleDriftOffset) > Mathf.Epsilon)
		{
			this.lastOffsetWasPositive = this.idleDriftOffset > 0f;
			this.pendingPreset = preset;
			float num = ((this.fishingSettings != null) ? this.fishingSettings.DriftFadeOutDuration : 0.35f);
			this.isFadingOut = true;
			this.idleDriftOffsetFadeSpeed = ((num > 0f) ? (Mathf.Abs(this.idleDriftOffset) / num) : 1000f);
			return;
		}
		this.idleDriftTimer = 0f;
		if (preset.EnableRandomization)
		{
			this.idleDriftDuration = global::UnityEngine.Random.Range(preset.MinDuration, preset.MaxDuration);
			this.idleDriftAmplitudeTarget = global::UnityEngine.Random.Range(preset.MinAmplitude, preset.MaxAmplitude);
		}
		else
		{
			this.idleDriftDuration = preset.MinDuration;
			this.idleDriftAmplitudeTarget = preset.MinAmplitude;
		}
		this.idleDriftFrequencyTarget = preset.Frequency;
		if (this.fishingDef != null)
		{
			this.idleDriftFrequencyTarget *= this.fishingDef.wriggleCoef;
		}
		this.idleDriftAxisTarget = preset.OscillationAxis;
		float num2 = Mathf.Abs(this.idleDriftAmplitudeTarget - this.idleDriftAmplitude);
		this.idleDriftAmplitudeSpeed = ((this.idleDriftDuration > 0f) ? (num2 / Mathf.Max(0.25f, this.idleDriftDuration * 0.5f)) : num2);
		if (!this.presetInitialized)
		{
			this.idleDriftFrequency = this.idleDriftFrequencyTarget;
			this.idleDriftAxis = this.idleDriftAxisTarget;
			this.presetInitialized = true;
		}
	}

	// Token: 0x06001AF5 RID: 6901 RVA: 0x0007DE90 File Offset: 0x0007C090
	private void RandomizeCurrentPreset()
	{
		if (this.currentDriftPreset == null)
		{
			return;
		}
		this.idleDriftTimer = 0f;
		this.idleDriftDuration = global::UnityEngine.Random.Range(this.currentDriftPreset.MinDuration, this.currentDriftPreset.MaxDuration);
		this.idleDriftAmplitudeTarget = global::UnityEngine.Random.Range(this.currentDriftPreset.MinAmplitude, this.currentDriftPreset.MaxAmplitude);
		float num = Mathf.Abs(this.idleDriftAmplitudeTarget - this.idleDriftAmplitude);
		this.idleDriftAmplitudeSpeed = ((this.idleDriftDuration > 0f) ? (num / Mathf.Max(0.25f, this.idleDriftDuration * 0.5f)) : num);
	}

	// Token: 0x040019F6 RID: 6646
	private static readonly int FishBiting = Animator.StringToHash("fishBiting");

	// Token: 0x040019F7 RID: 6647
	private static readonly int FishPulling = Animator.StringToHash("fishPulling");

	// Token: 0x040019F8 RID: 6648
	private static readonly int PlayerPulling = Animator.StringToHash("playerPulling");

	// Token: 0x040019F9 RID: 6649
	public Action<FishingMiniGame.Stage> OnStageChanged;

	// Token: 0x040019FA RID: 6650
	[Header("Fishing Params")]
	private FishingDef fishingDef;

	// Token: 0x040019FB RID: 6651
	private ItemDef fishingRod;

	// Token: 0x040019FC RID: 6652
	[Header("Calculated time indicators")]
	private float fishWaitTime;

	// Token: 0x040019FD RID: 6653
	private float fishIdleTime;

	// Token: 0x040019FE RID: 6654
	private float fishResistTime;

	// Token: 0x040019FF RID: 6655
	[Header("Debug")]
	[SerializeField]
	private FishingMiniGame.Stage currentStage;

	// Token: 0x04001A00 RID: 6656
	private float gameTime;

	// Token: 0x04001A01 RID: 6657
	private bool isFishResisting;

	// Token: 0x04001A02 RID: 6658
	public bool isPlayerPulling;

	// Token: 0x04001A03 RID: 6659
	public float progress;

	// Token: 0x04001A04 RID: 6660
	public float tension;

	// Token: 0x04001A05 RID: 6661
	public float resistance;

	// Token: 0x04001A06 RID: 6662
	private WgoData reservoir;

	// Token: 0x04001A07 RID: 6663
	private PlayerAnimation playerAnim;

	// Token: 0x04001A08 RID: 6664
	private FishUnderwaterGfx fishUnderwaterGfx;

	// Token: 0x04001A09 RID: 6665
	private int playerTalentLevel;

	// Token: 0x04001A0A RID: 6666
	private int fishAnger;

	// Token: 0x04001A0B RID: 6667
	private bool isReelSoundStarted;

	// Token: 0x04001A0C RID: 6668
	private float targetResistance;

	// Token: 0x04001A0D RID: 6669
	private bool usePhaseDurations;

	// Token: 0x04001A0E RID: 6670
	private float splashTimer;

	// Token: 0x04001A0F RID: 6671
	private bool splashResistActive;

	// Token: 0x04001A10 RID: 6672
	private FishingSettings fishingSettings;

	// Token: 0x04001A11 RID: 6673
	private FishingContainer fishingContainer;

	// Token: 0x04001A12 RID: 6674
	private SoundHandler reelSound;

	// Token: 0x04001A13 RID: 6675
	private Coroutine gameFlowRoutine;

	// Token: 0x04001A14 RID: 6676
	private Coroutine biteTimeoutRoutine;

	// Token: 0x04001A15 RID: 6677
	private Coroutine resistanceRoutine;

	// Token: 0x04001A16 RID: 6678
	private float idleDriftTimer;

	// Token: 0x04001A17 RID: 6679
	private float idleDriftDuration;

	// Token: 0x04001A18 RID: 6680
	private float idleDriftPhase;

	// Token: 0x04001A19 RID: 6681
	private float idleDriftAmplitude;

	// Token: 0x04001A1A RID: 6682
	private float idleDriftAmplitudeTarget;

	// Token: 0x04001A1B RID: 6683
	private float idleDriftAmplitudeSpeed;

	// Token: 0x04001A1C RID: 6684
	private float idleDriftFrequency;

	// Token: 0x04001A1D RID: 6685
	private float idleDriftFrequencyTarget;

	// Token: 0x04001A1E RID: 6686
	private float idleDriftOffset;

	// Token: 0x04001A1F RID: 6687
	private float idleDriftOffsetFadeSpeed;

	// Token: 0x04001A20 RID: 6688
	private Vector3 idleDriftAxis = Vector3.forward;

	// Token: 0x04001A21 RID: 6689
	private Vector3 idleDriftAxisTarget = Vector3.forward;

	// Token: 0x04001A22 RID: 6690
	private FishingSettings.FishDriftStateSettings currentDriftPreset;

	// Token: 0x04001A23 RID: 6691
	private FishingSettings.FishDriftStateSettings pendingPreset;

	// Token: 0x04001A24 RID: 6692
	private bool presetInitialized;

	// Token: 0x04001A25 RID: 6693
	private bool isFadingOut;

	// Token: 0x04001A26 RID: 6694
	private bool lastOffsetWasPositive;

	// Token: 0x02000406 RID: 1030
	public enum Stage
	{
		// Token: 0x04001A28 RID: 6696
		Start,
		// Token: 0x04001A29 RID: 6697
		WaitingForBite,
		// Token: 0x04001A2A RID: 6698
		Biting,
		// Token: 0x04001A2B RID: 6699
		PlayingWithFish,
		// Token: 0x04001A2C RID: 6700
		Finish
	}

	// Token: 0x02000407 RID: 1031
	private class FishingCoroutineRunner : MonoBehaviour
	{
		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x0007DFA3 File Offset: 0x0007C1A3
		private static FishingMiniGame.FishingCoroutineRunner Instance
		{
			get
			{
				if (FishingMiniGame.FishingCoroutineRunner.instance == null)
				{
					GameObject gameObject = new GameObject("FishingMiniGameRunner");
					global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
					FishingMiniGame.FishingCoroutineRunner.instance = gameObject.AddComponent<FishingMiniGame.FishingCoroutineRunner>();
				}
				return FishingMiniGame.FishingCoroutineRunner.instance;
			}
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x0007DFD1 File Offset: 0x0007C1D1
		public static Coroutine StartRoutine(IEnumerator routine)
		{
			if (routine == null)
			{
				return null;
			}
			return FishingMiniGame.FishingCoroutineRunner.Instance.StartCoroutine(routine);
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x0007DFE3 File Offset: 0x0007C1E3
		public static void StopRoutine(Coroutine routine)
		{
			if (routine == null || FishingMiniGame.FishingCoroutineRunner.instance == null)
			{
				return;
			}
			FishingMiniGame.FishingCoroutineRunner.instance.StopCoroutine(routine);
		}

		// Token: 0x04001A2D RID: 6701
		private static FishingMiniGame.FishingCoroutineRunner instance;
	}
}
