using System;
using UnityEngine;

// Token: 0x02000244 RID: 580
public class AnimationComponent : AnimationComponentBase
{
	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06000E84 RID: 3716 RVA: 0x0004C290 File Offset: 0x0004A490
	// (remove) Token: 0x06000E85 RID: 3717 RVA: 0x0004C2C8 File Offset: 0x0004A4C8
	public event Action OnLateUpdate;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06000E86 RID: 3718 RVA: 0x0004C300 File Offset: 0x0004A500
	// (remove) Token: 0x06000E87 RID: 3719 RVA: 0x0004C338 File Offset: 0x0004A538
	public event Action OnDeathAnimFinished;

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0004C36D File Offset: 0x0004A56D
	public bool UseTalkingAnimation
	{
		get
		{
			return this.useTalkingAnimation;
		}
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0004C375 File Offset: 0x0004A575
	public bool HasTalkingHeadFrames
	{
		get
		{
			return !this.useTalkingAnimation && this.talkingHeadPreset != null && this.skinChanger != null && this.skinChanger.HasCustomHeadFrames;
		}
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0004C3A2 File Offset: 0x0004A5A2
	public bool AutoInitOnAwake
	{
		get
		{
			return this.autoInitOnAwake;
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0004C3AA File Offset: 0x0004A5AA
	public Direction Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x0004C3B4 File Offset: 0x0004A5B4
	public override void Init(string visualId)
	{
		if (this.isInitialized)
		{
			return;
		}
		if (this.isNeedSkinChanger)
		{
			if (this.skinPresetDefault)
			{
				base.Init(this.skinPresetDefault);
			}
			else
			{
				base.Init(visualId);
			}
		}
		if (this.armorPresetData != null)
		{
			this.ApplyArmorColor(this.armorColorIndex);
		}
		this.ApplyCustomLayers();
		this.isInitialized = true;
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x0004C41B File Offset: 0x0004A61B
	public override void Init(SkinPresetGK2 skinPreset)
	{
		if (this.isInitialized)
		{
			return;
		}
		base.Init(skinPreset);
		if (this.armorPresetData != null)
		{
			this.ApplyArmorColor(this.armorColorIndex);
		}
		this.ApplyCustomLayers();
		this.isInitialized = true;
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x0004C454 File Offset: 0x0004A654
	public override void InitWithSkinOrApplyCurrentSkin(string skinPreset)
	{
		if (!this.isInitialized)
		{
			this.Init(skinPreset);
			return;
		}
		if (this.skinChanger != null && this.skinPreset != null)
		{
			this.skinChanger.ApplySkin(this.skinPreset, null);
		}
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x0004C490 File Offset: 0x0004A690
	public override void InitWithSkinOrApplySkin(string skinPreset)
	{
		if (!this.isInitialized)
		{
			this.Init(skinPreset);
			return;
		}
		if (this.isNeedSkinChanger)
		{
			if (this.skinPresetDefault != null)
			{
				this.skinPreset = this.skinPresetDefault;
			}
			else if (!string.IsNullOrEmpty(skinPreset))
			{
				this.skinPreset = SkinPresetGK2.LoadAsset(skinPreset);
			}
		}
		if (this.skinChanger != null && this.skinPreset != null)
		{
			this.skinChanger.ApplySkin(this.skinPreset, null);
		}
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x0004C50D File Offset: 0x0004A70D
	public void ChangeSkinPreset(string presetId)
	{
		if (!this.isInitialized)
		{
			this.Init(presetId);
			return;
		}
		if (this.isNeedSkinChanger)
		{
			this.skinPreset = SkinPresetGK2.LoadAsset(presetId);
			this.skinChanger.ApplySkin(this.skinPreset, null);
		}
	}

	// Token: 0x06000E91 RID: 3729 RVA: 0x0004C548 File Offset: 0x0004A748
	public virtual void ChangeSkinPreset(SkinPresetGK2 preset)
	{
		if (!this.isInitialized && this.skinChanger == null)
		{
			base.Init(preset);
			this.skinChanger = new SkinChangerGK2(base.gameObject, false);
			this.isInitialized = true;
		}
		this.skinChanger.ApplySkin(preset, null);
		this.CreateTalkingHeadPlayer();
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x0004C598 File Offset: 0x0004A798
	public void SetLayerWeight(AnimationComponent.Layers layer, float weight)
	{
		base.SetLayerWeight((int)layer, weight);
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x0004C5A2 File Offset: 0x0004A7A2
	public float GetLayerWeight(AnimationComponent.Layers layer)
	{
		return base.GetLayerWeight((int)layer);
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x0004C5AB File Offset: 0x0004A7AB
	public virtual void LateUpdate()
	{
		if (this.talkingHeadPlayer != null)
		{
			this.talkingHeadPlayer.Tick(Time.deltaTime);
		}
		if (this.skinChanger != null)
		{
			this.skinChanger.CustomLateUpdate();
		}
		Action onLateUpdate = this.OnLateUpdate;
		if (onLateUpdate == null)
		{
			return;
		}
		onLateUpdate();
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x0004C5E8 File Offset: 0x0004A7E8
	protected override void InitInternal(SkinPresetGK2 skinPreset)
	{
		this.skinChanger = new SkinChangerGK2(base.gameObject, false);
		this.skinChanger.ApplySkin(skinPreset, null);
		this.CreateTalkingHeadPlayer();
		base.InitInternal(skinPreset);
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x0004C618 File Offset: 0x0004A818
	public void StartTalkingHead()
	{
		if (!this.HasTalkingHeadFrames)
		{
			return;
		}
		if (this.talkingHeadPlayer == null)
		{
			this.CreateTalkingHeadPlayer();
		}
		if (this.talkingHeadPlayer == null)
		{
			return;
		}
		if (!this.talkingHeadPlayer.IsPlaying)
		{
			this.talkingHeadPlayer.PlaySeries();
			return;
		}
		this.talkingHeadPlayer.Resume();
	}

	// Token: 0x06000E97 RID: 3735 RVA: 0x0004C669 File Offset: 0x0004A869
	public void PauseTalkingHead()
	{
		TalkingHeadPlayer talkingHeadPlayer = this.talkingHeadPlayer;
		if (talkingHeadPlayer == null)
		{
			return;
		}
		talkingHeadPlayer.Pause();
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x0004C67B File Offset: 0x0004A87B
	public void StopTalkingHead()
	{
		TalkingHeadPlayer talkingHeadPlayer = this.talkingHeadPlayer;
		if (talkingHeadPlayer == null)
		{
			return;
		}
		talkingHeadPlayer.Stop();
	}

	// Token: 0x06000E99 RID: 3737 RVA: 0x0004C690 File Offset: 0x0004A890
	public void PlayTalkingHeadClip(int clipIndex)
	{
		if (this.talkingHeadPreset == null || this.talkingHeadPreset.clips == null)
		{
			return;
		}
		if (clipIndex < 0 || clipIndex >= this.talkingHeadPreset.clips.Count)
		{
			return;
		}
		if (this.talkingHeadPlayer == null)
		{
			this.CreateTalkingHeadPlayer();
		}
		TalkingHeadPlayer talkingHeadPlayer = this.talkingHeadPlayer;
		if (talkingHeadPlayer == null)
		{
			return;
		}
		talkingHeadPlayer.Play(this.talkingHeadPreset.clips[clipIndex]);
	}

	// Token: 0x06000E9A RID: 3738 RVA: 0x0004C700 File Offset: 0x0004A900
	private void CreateTalkingHeadPlayer()
	{
		this.talkingHeadPlayer = null;
		if (this.talkingHeadPreset == null || this.skinChanger == null)
		{
			return;
		}
		this.talkingHeadPlayer = new TalkingHeadPlayer(this, this.skinChanger, this.talkingHeadPreset);
	}

	// Token: 0x06000E9B RID: 3739 RVA: 0x0004C738 File Offset: 0x0004A938
	private void Awake()
	{
		if (this.isNeedSkinChanger && this.autoInitOnAwake && this.skinPresetDefault != null)
		{
			this.Init(this.skinPresetDefault);
			if (this.direction.IsValid())
			{
				base.SetDirection(this.direction);
			}
		}
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x0004C788 File Offset: 0x0004A988
	private void OnEnable()
	{
		if (!this.isInitialized)
		{
			return;
		}
		this.ApplyCustomLayers();
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x0004C799 File Offset: 0x0004A999
	private void OnDestroy()
	{
		SkinPresetGK2.ReleaseAsset(this.skinPreset);
		this.OnDeathAnimFinished = null;
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x0004C7B0 File Offset: 0x0004A9B0
	public void ResetArmorLayers()
	{
		this.SetLayerWeight(AnimationComponent.Layers.Armor, 0f);
		this.SetLayerWeight(AnimationComponent.Layers.ArmorWithSword, 0f);
		this.SetLayerWeight(AnimationComponent.Layers.ArmorWithPike, 0f);
		this.SetLayerWeight(AnimationComponent.Layers.ArmorWithBow, 0f);
		this.SetLayerWeight(AnimationComponent.Layers.ArmorNoHelmet, 0f);
		this.animator.Update(0f);
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x0004C80C File Offset: 0x0004AA0C
	public override void OnDeathAnimationFinished()
	{
		Action onDeathAnimFinished = this.OnDeathAnimFinished;
		if (onDeathAnimFinished == null)
		{
			return;
		}
		onDeathAnimFinished();
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x0004C81E File Offset: 0x0004AA1E
	public override void ResetForPool()
	{
		base.ResetForPool();
		this.OnLateUpdate = null;
		this.OnDeathAnimFinished = null;
		this.StopTalkingHead();
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x0004C83A File Offset: 0x0004AA3A
	protected override void OnDisable()
	{
		this.StopTalkingHead();
		base.OnDisable();
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x0004C848 File Offset: 0x0004AA48
	public void ApplyArmorColor(int index)
	{
		Texture2D palette = this.armorPresetData.GetColorReplacePalette(index).palette;
		for (int i = 0; i < this.armorPresetData.affectedPartTypes.Count; i++)
		{
			CustomizablePartType customizablePartType = this.armorPresetData.affectedPartTypes[i];
			if (customizablePartType != CustomizablePartType.Body)
			{
				if (customizablePartType != CustomizablePartType.Arms)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.skinPresetDefault.arms.palette = palette;
			}
			else
			{
				this.skinPresetDefault.body.palette = palette;
			}
		}
		SkinChangerGK2 skinChangerGK = this.skinChanger;
		if (skinChangerGK == null)
		{
			return;
		}
		skinChangerGK.ApplyShaderParameters();
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x0004C8DC File Offset: 0x0004AADC
	protected void ApplyCustomLayers()
	{
		if (this.setCustomLayersOnInit)
		{
			if (this.enableLightLayer)
			{
				this.SetLayerWeight(AnimationComponent.Layers.Lighting, 1f);
			}
			if (this.enableBreathLayer)
			{
				this.SetLayerWeight(AnimationComponent.Layers.Breath, 1f);
			}
			if (this.enableOverheadLayer)
			{
				this.SetLayerWeight(AnimationComponent.Layers.Overhead, 1f);
			}
			if (this.enableBackpackLayer)
			{
				this.SetLayerWeight(AnimationComponent.Layers.Backpack, 1f);
			}
			if (this.enableArmorLayer)
			{
				this.SetLayerWeight(AnimationComponent.Layers.Armor, 1f);
			}
			if (this.enableArmorWithSwordLayer)
			{
				this.SetLayerWeight(AnimationComponent.Layers.ArmorWithSword, 1f);
			}
			if (this.enableArmorWithPikeLayer)
			{
				this.SetLayerWeight(AnimationComponent.Layers.ArmorWithPike, 1f);
			}
			if (this.enableArmorWithBowLayer)
			{
				this.SetLayerWeight(AnimationComponent.Layers.ArmorWithBow, 1f);
			}
			if (this.enableArmorNoHelmetLayer)
			{
				this.SetLayerWeight(AnimationComponent.Layers.ArmorNoHelmet, 1f);
			}
		}
	}

	// Token: 0x0400116E RID: 4462
	[SerializeField]
	private bool isNeedSkinChanger = true;

	// Token: 0x0400116F RID: 4463
	[SerializeField]
	[Space]
	private bool setCustomLayersOnInit;

	// Token: 0x04001170 RID: 4464
	[SerializeField]
	private bool enableLightLayer;

	// Token: 0x04001171 RID: 4465
	[SerializeField]
	private bool enableBreathLayer;

	// Token: 0x04001172 RID: 4466
	[SerializeField]
	private bool enableOverheadLayer;

	// Token: 0x04001173 RID: 4467
	[SerializeField]
	private bool enableBackpackLayer;

	// Token: 0x04001174 RID: 4468
	[SerializeField]
	private bool enableArmorLayer;

	// Token: 0x04001175 RID: 4469
	[SerializeField]
	private bool enableArmorWithSwordLayer;

	// Token: 0x04001176 RID: 4470
	[SerializeField]
	private bool enableArmorWithPikeLayer;

	// Token: 0x04001177 RID: 4471
	[SerializeField]
	private bool enableArmorWithBowLayer;

	// Token: 0x04001178 RID: 4472
	[SerializeField]
	private bool enableArmorNoHelmetLayer;

	// Token: 0x04001179 RID: 4473
	[SerializeField]
	[Space]
	private bool autoInitOnAwake;

	// Token: 0x0400117A RID: 4474
	[SerializeField]
	private Direction direction = Direction.Down;

	// Token: 0x0400117B RID: 4475
	[SerializeField]
	protected SkinPresetGK2 skinPresetDefault;

	// Token: 0x0400117C RID: 4476
	[SerializeField]
	[HideInInspector]
	protected ArmorPresetData armorPresetData;

	// Token: 0x0400117D RID: 4477
	[SerializeField]
	[HideInInspector]
	protected int armorColorIndex;

	// Token: 0x0400117E RID: 4478
	[SerializeField]
	private bool useTalkingAnimation;

	// Token: 0x0400117F RID: 4479
	[SerializeField]
	private TalkingHeadPreset talkingHeadPreset;

	// Token: 0x04001180 RID: 4480
	protected SkinChangerGK2 skinChanger;

	// Token: 0x04001181 RID: 4481
	private TalkingHeadPlayer talkingHeadPlayer;

	// Token: 0x02000245 RID: 581
	public enum Layers
	{
		// Token: 0x04001183 RID: 4483
		Default,
		// Token: 0x04001184 RID: 4484
		Lighting,
		// Token: 0x04001185 RID: 4485
		Breath,
		// Token: 0x04001186 RID: 4486
		Overhead,
		// Token: 0x04001187 RID: 4487
		Backpack,
		// Token: 0x04001188 RID: 4488
		OverheadInteracting,
		// Token: 0x04001189 RID: 4489
		WeaponHitBox,
		// Token: 0x0400118A RID: 4490
		Armor,
		// Token: 0x0400118B RID: 4491
		ArmorWithSword,
		// Token: 0x0400118C RID: 4492
		ArmorWithPike,
		// Token: 0x0400118D RID: 4493
		ArmorWithBow,
		// Token: 0x0400118E RID: 4494
		SwordAttack,
		// Token: 0x0400118F RID: 4495
		BowAttack,
		// Token: 0x04001190 RID: 4496
		StanceWalk,
		// Token: 0x04001191 RID: 4497
		Talking,
		// Token: 0x04001192 RID: 4498
		SwordAttackHitbox,
		// Token: 0x04001193 RID: 4499
		Eyes = 17,
		// Token: 0x04001194 RID: 4500
		ArmorNoHelmet
	}
}
