using System;
using System.Collections;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200024B RID: 587
public class PlayerAnimation : AnimationComponent
{
	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x0004D97E File Offset: 0x0004BB7E
	public int PlayerDeathExitTriggerId
	{
		get
		{
			return Animator.StringToHash("player_death_exit");
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x0004D98A File Offset: 0x0004BB8A
	public MultiFlagAND<PlayerAnimation.EyesBlinkingReason> EyesBlinkingMultiflag
	{
		get
		{
			return this.eyesBlinkingMultiflag;
		}
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x0004D994 File Offset: 0x0004BB94
	public override void Init(SkinPresetGK2 skinPreset)
	{
		if (!this.isInitialized)
		{
			this.skinChanger = new SkinChangerGK2(base.gameObject, this.spriteRenderers, this.isCustomizationCharacter);
			base.ApplyCustomLayers();
			this.isInitialized = true;
			this.dropView.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x0004D9E4 File Offset: 0x0004BBE4
	public override void ChangeSkinPreset(SkinPresetGK2 preset)
	{
		if (!this.isInitialized || this.skinChanger == null)
		{
			this.skinChanger = new SkinChangerGK2(base.gameObject, this.spriteRenderers, this.isCustomizationCharacter);
			this.isInitialized = true;
		}
		this.skinPreset = preset;
		this.skinChanger.ApplySkin(preset, null);
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x0004DA3C File Offset: 0x0004BC3C
	public void ApplyPlayerColors(Texture2D palette, List<CustomizablePartType> affectedPartTypes, SkinPresetGK2 customSkinPreset = null)
	{
		SkinPresetGK2 skinPresetGK = ((customSkinPreset != null) ? customSkinPreset : PlayerSkinHelper.CurrentPreset);
		for (int i = 0; i < affectedPartTypes.Count; i++)
		{
			switch (affectedPartTypes[i])
			{
			case CustomizablePartType.Hair:
				skinPresetGK.hairstyle.palette = palette;
				break;
			case CustomizablePartType.Beard:
				skinPresetGK.beard.palette = palette;
				break;
			case CustomizablePartType.Body:
				skinPresetGK.body.palette = palette;
				break;
			case CustomizablePartType.Arms:
				skinPresetGK.arms.palette = palette;
				break;
			case CustomizablePartType.Head:
				skinPresetGK.head.palette = palette;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		this.skinChanger.ApplyShaderParameters();
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x0004DAE7 File Offset: 0x0004BCE7
	public void SetCustomDeathAnimationFinishedCallback(Action callback)
	{
		this.customDeathAnimationFinishedCallback = callback;
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x0004DAF0 File Offset: 0x0004BCF0
	public void OnSwordAttack()
	{
		MainGame.PlayerData.staminaSystem.ConsumeStamina(true);
		LazyAudio.PlayAtGameObject("sword_attack", base.transform, SpatialType.sound3D, true);
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x0004DB15 File Offset: 0x0004BD15
	public override void OnBowAimStart()
	{
		MainGame.PlayerData.staminaSystem.ConsumeStamina(false);
		base.OnBowAimStart();
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x0004DB30 File Offset: 0x0004BD30
	public override void OnBowAimShot()
	{
		base.OnBowAimShot();
		MainGame.PlayerData.staminaSystem.BeginRegenerationDelayIfSuspended();
		if (!this.attackComponent)
		{
			this.attackComponent = base.GetComponentInParent<AttackComponent>();
		}
		if (this.attackComponent)
		{
			this.attackComponent.PerformAttack(false, default(Vector3), false, null, true);
		}
	}

	// Token: 0x06000EFC RID: 3836 RVA: 0x0004DB90 File Offset: 0x0004BD90
	public override void OnSpearAttack()
	{
		MainGame.PlayerData.staminaSystem.ConsumeStamina(true);
		base.OnSpearAttack();
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x0004DBA8 File Offset: 0x0004BDA8
	public override void OnDeathAnimationFinished()
	{
		if (this.customDeathAnimationFinishedCallback != null)
		{
			this.customDeathAnimationFinishedCallback();
			return;
		}
		UIDialogWindowData uidialogWindowData = new UIDialogWindowData("ui_death", "ui_youre_dead", new UIDialogWindowData.ButtonData(new Action(LazyUI.GetWindow<UIDialogWindow>().Close), "btn_ok", null, true, GameKey.Select, ""));
		LazyUI.GetWindow<UIDialogWindow>().Open(uidialogWindowData, delegate(UIDialogWindowData _)
		{
			MainGame.PlayerController.SetControlTakenType(TakenControlType.ByDeath, true);
			base.SetTrigger(this.PlayerDeathExitTriggerId);
			this.animator.Update(0f);
		});
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x0004DC18 File Offset: 0x0004BE18
	public void OnClimbStep()
	{
		float num = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
		if (Mathf.Abs(num - this.lastClimbStepNormalizedTime).EqualsTo(0f, 1E-05f))
		{
			return;
		}
		this.lastClimbStepNormalizedTime = num;
		LazyAudio.PlayAndForget("ladder_climb");
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x0004DC70 File Offset: 0x0004BE70
	private void Start()
	{
		this.eyesBlinkingMultiflag.Init(new Action<bool>(this.HandleEyesBlinkingMultiflagChanged), true);
		this.HandleEyesBlinkingMultiflagChanged(this.eyesBlinkingMultiflag.ResultFlag);
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x0004DC9B File Offset: 0x0004BE9B
	private IEnumerator EyesBlinkCoroutine()
	{
		while (this.isEyesLayerActive)
		{
			float num = global::UnityEngine.Random.Range(3f, 7f);
			yield return new WaitForSeconds(num);
			if (!this.isEyesLayerActive)
			{
				break;
			}
			int blinkCount = ((global::UnityEngine.Random.value < 0.7f) ? 1 : 2);
			float blinkBetweenCountPause = global::UnityEngine.Random.Range(0.1f, 0.25f);
			base.SetLayerWeight(AnimationComponent.Layers.Eyes, 1f);
			while (blinkCount > 0 && this.isEyesLayerActive)
			{
				PlayerAnimation.<>c__DisplayClass24_0 CS$<>8__locals1 = new PlayerAnimation.<>c__DisplayClass24_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.waitTimeout = 10f;
				yield return new WaitUntil(delegate
				{
					CS$<>8__locals1.waitTimeout -= Time.deltaTime;
					return CS$<>8__locals1.waitTimeout <= 0f || !CS$<>8__locals1.<>4__this.animator.IsInTransition(17);
				});
				if (!this.isEyesLayerActive)
				{
					break;
				}
				this.animator.SetTrigger(this.eyesBlinkTriggerId);
				CS$<>8__locals1.waitTimeout = 10f;
				yield return new WaitUntil(delegate
				{
					CS$<>8__locals1.waitTimeout -= Time.deltaTime;
					return CS$<>8__locals1.waitTimeout <= 0f || CS$<>8__locals1.<>4__this.animator.GetCurrentAnimatorStateInfo(17).normalizedTime >= 1f;
				});
				int num2 = blinkCount;
				blinkCount = num2 - 1;
				if (blinkCount > 0 && this.isEyesLayerActive)
				{
					yield return new WaitForSeconds(blinkBetweenCountPause);
				}
				CS$<>8__locals1 = null;
			}
			base.SetLayerWeight(AnimationComponent.Layers.Eyes, 0f);
		}
		this.eyesBlinkCoroutine = null;
		yield break;
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x0004DCAC File Offset: 0x0004BEAC
	private void HandleEyesBlinkingMultiflagChanged(bool canBlink)
	{
		this.isEyesLayerActive = canBlink;
		if (canBlink)
		{
			if (this.eyesBlinkCoroutine == null)
			{
				this.eyesBlinkCoroutine = base.StartCoroutine(this.EyesBlinkCoroutine());
				return;
			}
		}
		else if (this.eyesBlinkCoroutine != null)
		{
			base.StopCoroutine(this.eyesBlinkCoroutine);
			this.eyesBlinkCoroutine = null;
			base.SetLayerWeight(AnimationComponent.Layers.Eyes, 0f);
		}
	}

	// Token: 0x040011DC RID: 4572
	[SerializeField]
	private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

	// Token: 0x040011DD RID: 4573
	[SerializeField]
	private bool isCustomizationCharacter;

	// Token: 0x040011DE RID: 4574
	private bool isEyesLayerActive;

	// Token: 0x040011DF RID: 4575
	private Coroutine eyesBlinkCoroutine;

	// Token: 0x040011E0 RID: 4576
	private int eyesBlinkTriggerId = Animator.StringToHash("do_eyes_blink");

	// Token: 0x040011E1 RID: 4577
	private MultiFlagAND<PlayerAnimation.EyesBlinkingReason> eyesBlinkingMultiflag = new MultiFlagAND<PlayerAnimation.EyesBlinkingReason>();

	// Token: 0x040011E2 RID: 4578
	private float lastClimbStepNormalizedTime = -1f;

	// Token: 0x040011E3 RID: 4579
	private AttackComponent attackComponent;

	// Token: 0x040011E4 RID: 4580
	private Action customDeathAnimationFinishedCallback;

	// Token: 0x0200024C RID: 588
	public enum EyesBlinkingReason
	{
		// Token: 0x040011E6 RID: 4582
		ControlValue,
		// Token: 0x040011E7 RID: 4583
		ArmorEquippedEquippedState,
		// Token: 0x040011E8 RID: 4584
		EnabledState
	}
}
