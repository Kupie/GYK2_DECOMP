using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000246 RID: 582
public abstract class AnimationComponentBase : MonoBehaviour
{
	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06000EA5 RID: 3749 RVA: 0x0004C9C4 File Offset: 0x0004ABC4
	// (remove) Token: 0x06000EA6 RID: 3750 RVA: 0x0004C9FC File Offset: 0x0004ABFC
	public event Action<ItemType> OnToolLoopStarted;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06000EA7 RID: 3751 RVA: 0x0004CA34 File Offset: 0x0004AC34
	// (remove) Token: 0x06000EA8 RID: 3752 RVA: 0x0004CA6C File Offset: 0x0004AC6C
	public event Action<ItemType> OnToolLoopFinished;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06000EA9 RID: 3753 RVA: 0x0004CAA4 File Offset: 0x0004ACA4
	// (remove) Token: 0x06000EAA RID: 3754 RVA: 0x0004CADC File Offset: 0x0004ACDC
	public event Action SpearAttackFired;

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06000EAB RID: 3755 RVA: 0x0004CB11 File Offset: 0x0004AD11
	public Animator Animator
	{
		get
		{
			return this.ResolveAnimator();
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06000EAC RID: 3756 RVA: 0x0004CB19 File Offset: 0x0004AD19
	public Vector3 OverheadItemWorldPosition
	{
		get
		{
			if (!(this.dropView != null))
			{
				return base.transform.position;
			}
			return this.dropView.transform.position;
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06000EAD RID: 3757 RVA: 0x0004CB45 File Offset: 0x0004AD45
	public global::AnimationState AnimationState
	{
		get
		{
			return this.animationState;
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06000EAE RID: 3758 RVA: 0x0004CB4D File Offset: 0x0004AD4D
	public SkinPresetGK2 SkinPreset
	{
		get
		{
			return this.skinPreset;
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06000EAF RID: 3759 RVA: 0x0004CB55 File Offset: 0x0004AD55
	public bool HasBlockAnimation
	{
		get
		{
			return this.hasBlockAnimation;
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x0004CB5D File Offset: 0x0004AD5D
	// (set) Token: 0x06000EB1 RID: 3761 RVA: 0x0004CB65 File Offset: 0x0004AD65
	public bool UseAdditionalStepSound
	{
		get
		{
			return this.useAdditionalStepSound;
		}
		set
		{
			this.useAdditionalStepSound = value;
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x0004CB6E File Offset: 0x0004AD6E
	// (set) Token: 0x06000EB3 RID: 3763 RVA: 0x0004CB76 File Offset: 0x0004AD76
	public string AdditionalStepSoundId
	{
		get
		{
			return this.additionalStepSoundId;
		}
		set
		{
			this.additionalStepSoundId = value;
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x0004CB7F File Offset: 0x0004AD7F
	public AnimationEventReceiver AnimationEventReceiver
	{
		get
		{
			return this.animationEventReceiver;
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x0004CB87 File Offset: 0x0004AD87
	private SteppedRotationPreset SteppedRotationPreset
	{
		get
		{
			if (this.steppedRotationPreset == null)
			{
				return BasicNpcSteppedRotationPreset.Instance;
			}
			return this.steppedRotationPreset;
		}
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x0004CBA0 File Offset: 0x0004ADA0
	public virtual void Init(string visualId)
	{
		if (this.skinPreset == null)
		{
			this.skinPreset = SkinPresetGK2.LoadAsset(visualId);
		}
		if (this.skinPreset == null)
		{
			Debug.LogError("Failed to load skin preset: " + visualId);
			return;
		}
		this.InitInternal(this.skinPreset);
	}

	// Token: 0x06000EB7 RID: 3767 RVA: 0x0004CBF2 File Offset: 0x0004ADF2
	public virtual void Init(SkinPresetGK2 skinPreset)
	{
		this.InitInternal(skinPreset);
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x0004CBFB File Offset: 0x0004ADFB
	public void SetSkinPreset(SkinPresetGK2 skinPreset)
	{
		this.skinPreset = skinPreset;
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x0004CC04 File Offset: 0x0004AE04
	public virtual void InitWithSkinOrApplyCurrentSkin(string skinPreset)
	{
		this.Init(skinPreset);
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x0004CC04 File Offset: 0x0004AE04
	public virtual void InitWithSkinOrApplySkin(string skinPreset)
	{
		this.Init(skinPreset);
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x0004CC0D File Offset: 0x0004AE0D
	public void SetState(global::AnimationState animationState)
	{
		this.animationState = animationState;
		this.animator.SetInteger(AnimationComponentBase.idStateAnimator, (int)animationState);
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x0004CC27 File Offset: 0x0004AE27
	public global::AnimationState GetState()
	{
		return (global::AnimationState)this.animator.GetInteger(AnimationComponentBase.idStateAnimator);
	}

	// Token: 0x06000EBD RID: 3773 RVA: 0x0004CC39 File Offset: 0x0004AE39
	public void SetTrigger(string trigger)
	{
		this.animator.SetTrigger(trigger);
	}

	// Token: 0x06000EBE RID: 3774 RVA: 0x0004CC47 File Offset: 0x0004AE47
	public void SetTrigger(int trigger)
	{
		this.animator.SetTrigger(trigger);
	}

	// Token: 0x06000EBF RID: 3775 RVA: 0x0004CC58 File Offset: 0x0004AE58
	public float SetDirection(Vector2 direction)
	{
		float num = this.SteppedRotationPreset.ComputeAngle(direction);
		Animator animator = this.ResolveAnimator();
		if (animator == null || !animator.isActiveAndEnabled)
		{
			return 0f;
		}
		animator.SetFloat(AnimationComponentBase.idDirectionAnimator, num);
		return num;
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x0004CCA0 File Offset: 0x0004AEA0
	public void SetDirection(Direction direction)
	{
		Animator animator = this.ResolveAnimator();
		if (animator == null || !animator.isActiveAndEnabled)
		{
			return;
		}
		animator.SetFloat(AnimationComponentBase.idDirectionAnimator, this.SteppedRotationPreset.GetSteppedDirection(direction));
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x0004CCE0 File Offset: 0x0004AEE0
	public Vector2 GetDirection()
	{
		float @float = this.animator.GetFloat(AnimationComponentBase.idDirectionAnimator);
		return new Vector2(Mathf.Cos(@float * 0.017453292f), Mathf.Sin(@float * 0.017453292f));
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x0004CD1B File Offset: 0x0004AF1B
	public void SetWalkAnimationSpeedMultiplier(float speed)
	{
		this.animator.SetFloat(AnimationComponentBase.walkSpeedMultiplier, speed / 1.5f);
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x0004CD34 File Offset: 0x0004AF34
	public void SetOverheadItem(Item item)
	{
		this.ApplyOverheadItemToView(this.dropView, item);
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x0004CD44 File Offset: 0x0004AF44
	public void SetOverheadItems(IReadOnlyList<Item> items)
	{
		if (items == null || items.Count == 0)
		{
			this.RemoveOverheadItem();
			return;
		}
		this.ApplyOverheadItemToView(this.dropView, items[0]);
		this.EnsureExtraOverheadViews(items.Count - 1);
		AnimationComponentBase.ApplyOverheadStackSorting(this.dropView, 0);
		for (int i = 1; i < items.Count; i++)
		{
			DropViewAtomMesh dropViewAtomMesh = this.extraOverheadDropViews[i - 1];
			dropViewAtomMesh.transform.localPosition = LazyConsts.OVERHEAD_STACK_OFFSET * (float)i;
			dropViewAtomMesh.transform.localRotation = Quaternion.identity;
			dropViewAtomMesh.transform.localScale = Vector3.one;
			this.ApplyOverheadItemToView(dropViewAtomMesh, items[i]);
			AnimationComponentBase.ApplyOverheadStackSorting(dropViewAtomMesh, i);
		}
		for (int j = items.Count - 1; j < this.extraOverheadDropViews.Count; j++)
		{
			if (!(this.extraOverheadDropViews[j] == null))
			{
				this.extraOverheadDropViews[j].Deactivate();
				this.extraOverheadDropViews[j].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x0004CE58 File Offset: 0x0004B058
	public void RemoveOverheadItem()
	{
		if (this.dropView != null)
		{
			this.dropView.Deactivate();
		}
		for (int i = 0; i < this.extraOverheadDropViews.Count; i++)
		{
			if (!(this.extraOverheadDropViews[i] == null))
			{
				this.extraOverheadDropViews[i].Deactivate();
				this.extraOverheadDropViews[i].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x0004CED0 File Offset: 0x0004B0D0
	private void ApplyOverheadItemToView(DropViewAtomMesh view, Item item)
	{
		if (view == null || item == null || item.IsEmpty)
		{
			return;
		}
		view.isOverhead = true;
		if (item.Definition.isLinkedToWgo)
		{
			ZombieWgoData zombie = MainGame.ZombieSystemData.GetZombie(item.UniqueId);
			if (zombie != null)
			{
				SkinPresetGK2 presetForWgoData = ZombieSkinHelper.GetPresetForWgoData(zombie, "zombie_worker");
				if (presetForWgoData != null)
				{
					view.ActivateZombie(presetForWgoData.head.id.ToString("D4"), presetForWgoData.head.palette);
					return;
				}
			}
		}
		view.Activate(item.Definition.iconId);
		view.SetInteractionState(false);
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x0004CF70 File Offset: 0x0004B170
	private static void ApplyOverheadStackSorting(DropViewAtomMesh view, int stackIndex)
	{
		if (view == null)
		{
			return;
		}
		Renderer[] componentsInChildren = view.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].sortingOrder = stackIndex;
		}
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x0004CFA8 File Offset: 0x0004B1A8
	private void EnsureExtraOverheadViews(int count)
	{
		if (this.dropView == null)
		{
			return;
		}
		while (this.extraOverheadDropViews.Count < count)
		{
			DropViewAtomMesh dropViewAtomMesh = global::UnityEngine.Object.Instantiate<DropViewAtomMesh>((this.extraOverheadDropViews.Count > 0) ? this.extraOverheadDropViews[0] : this.dropView, this.dropView.transform.parent);
			dropViewAtomMesh.name = string.Format("{0}_stack_{1}", this.dropView.name, this.extraOverheadDropViews.Count);
			dropViewAtomMesh.transform.SetParent(this.dropView.transform, false);
			dropViewAtomMesh.transform.localRotation = Quaternion.identity;
			dropViewAtomMesh.transform.localScale = Vector3.one;
			dropViewAtomMesh.gameObject.SetActive(false);
			this.extraOverheadDropViews.Add(dropViewAtomMesh);
		}
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x0004D08A File Offset: 0x0004B28A
	public void DisableDropView()
	{
		if (this.dropViewParent != null)
		{
			this.dropViewParent.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x0004D0AB File Offset: 0x0004B2AB
	public void EnableDropView()
	{
		if (this.dropViewParent != null)
		{
			this.dropViewParent.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x0004D0CC File Offset: 0x0004B2CC
	public void SetLayerWeight(int layerIndex, float weight)
	{
		this.animator.SetLayerWeight(layerIndex, weight);
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x0004D0DB File Offset: 0x0004B2DB
	public float GetLayerWeight(int layerIndex)
	{
		return this.animator.GetLayerWeight(layerIndex);
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x0004D0E9 File Offset: 0x0004B2E9
	public void PlaySound(string sound)
	{
		LazyAudio.PlayAtGameObject(sound, base.transform, SpatialType.sound3D, true);
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x0004D0FA File Offset: 0x0004B2FA
	public void PlaySoundIfVoiceOverNotEnabled(string sound)
	{
		if (VoiceOverSettings.IsEnabled)
		{
			return;
		}
		this.PlaySound(sound);
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x0004D10B File Offset: 0x0004B30B
	public void OnUseToolAnimation(ToolComponent toolComponent)
	{
		if (!toolComponent.IsActionActive)
		{
			return;
		}
		this.PlayToolUseSound(toolComponent.ToolInUse.Definition.type);
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x0004D12C File Offset: 0x0004B32C
	public void PlayToolUseSound(ItemType toolType)
	{
		string text = string.Empty;
		switch (toolType)
		{
		case ItemType.Axe:
			text = "tool_axe";
			break;
		case ItemType.Shovel:
			text = "tool_shovel";
			break;
		case ItemType.Pickaxe:
			text = "tool_pickaxe";
			break;
		case ItemType.Hammer:
			text = "tool_hammer";
			break;
		}
		if (text != string.Empty)
		{
			LazyAudio.PlayAtGameObject(text, base.transform, SpatialType.sound3D, true);
		}
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x00002318 File Offset: 0x00000518
	public void OnPlantingAnimation()
	{
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x0004D194 File Offset: 0x0004B394
	public void ShowFishingRope()
	{
		NpcFishingContainer componentInChildren = base.GetComponentInChildren<NpcFishingContainer>(true);
		if (componentInChildren != null)
		{
			componentInChildren.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x0004D1C0 File Offset: 0x0004B3C0
	public void HideFishingRope()
	{
		NpcFishingContainer componentInChildren = base.GetComponentInChildren<NpcFishingContainer>(true);
		if (componentInChildren != null)
		{
			componentInChildren.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x0004D1EA File Offset: 0x0004B3EA
	public void OnFishingStart()
	{
		LazyAudio.PlayAtGameObject("fishing_start", base.transform, SpatialType.sound3D, true);
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x0004D1FF File Offset: 0x0004B3FF
	public void OnFishingCast()
	{
		LazyAudio.PlayAtGameObject("fishing_cast_swoosh", base.transform, SpatialType.sound3D, true);
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x0004D214 File Offset: 0x0004B414
	public virtual void OnSpearAttack()
	{
		LazyAudio.PlayAtGameObject("spear_attack", base.transform, SpatialType.sound3D, true);
		Action spearAttackFired = this.SpearAttackFired;
		if (spearAttackFired == null)
		{
			return;
		}
		spearAttackFired();
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x0004D23C File Offset: 0x0004B43C
	public virtual void OnBowAimStart()
	{
		this.CancelBowAimLoop();
		this.bowSoundHandler = LazyAudio.PlayAtGameObject("bow_aim_start", base.transform, SpatialType.sound3D, true);
		if (this.bowSoundHandler != null)
		{
			SoundHandler soundHandler = this.bowSoundHandler;
			soundHandler.OnSoundPlayed = (Action)Delegate.Combine(soundHandler.OnSoundPlayed, new Action(this.OnBowAimLoopStart));
		}
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x0004D296 File Offset: 0x0004B496
	private void OnBowAimLoopStart()
	{
		this.CancelBowAimLoop();
		if (this == null)
		{
			return;
		}
		this.bowSoundHandler = LazyAudio.PlayAtGameObject("bow_aim_loop", base.transform, SpatialType.sound3D, true);
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x0004D2C0 File Offset: 0x0004B4C0
	public virtual void OnBowAimShot()
	{
		this.CancelBowAimLoop();
		this.bowSoundHandler = LazyAudio.PlayAtGameObject("bow_aim_shot", base.transform, SpatialType.sound3D, true);
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x0004D2E0 File Offset: 0x0004B4E0
	public void OnAllyDeathAnimationStart()
	{
		FightingAgent componentInParent = base.GetComponentInParent<FightingAgent>();
		if (componentInParent != null)
		{
			if (componentInParent.FighterDef.id.StartsWith("npc_town_barracks_mercenary"))
			{
				LazyAudio.PlayAtGameObject("ally_death_human", base.transform, SpatialType.sound3D, true);
				return;
			}
			if (componentInParent.FighterDef.id == "zmb_wild_mob_allie")
			{
				LazyAudio.PlayAtGameObject("ally_death_zombie", base.transform, SpatialType.sound3D, true);
			}
		}
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x0004D352 File Offset: 0x0004B552
	public void CancelBowAimLoop()
	{
		if (this.bowSoundHandler != null)
		{
			this.bowSoundHandler.OnSoundPlayed = null;
			this.bowSoundHandler.Stop();
			this.bowSoundHandler = null;
		}
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x0004D37B File Offset: 0x0004B57B
	public void OnZombieAttack()
	{
		LazyAudio.PlayAtGameObject("zombie_attack", base.transform, SpatialType.sound3D, true);
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x0004D390 File Offset: 0x0004B590
	public void StartZombieIdleSound()
	{
		if (this.idleSoundHandler != null && this.idleSoundHandler.IsActive)
		{
			return;
		}
		this.StopZombieIdleSound();
		this.idleSoundHandler = LazyAudio.PlayAtGameObject("zombie_idle", base.transform, SpatialType.sound3D, false);
		if (this.idleSoundHandler != null)
		{
			SoundHandler soundHandler = this.idleSoundHandler;
			soundHandler.OnSoundPlayed = (Action)Delegate.Combine(soundHandler.OnSoundPlayed, new Action(delegate
			{
				if (this == null)
				{
					this.StopZombieIdleSound();
					return;
				}
				if (this.animationState != global::AnimationState.Idle && this.animationState != global::AnimationState.Walk)
				{
					this.StopZombieIdleSound();
					return;
				}
				this.StartZombieIdleSound();
			}));
		}
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x0004D400 File Offset: 0x0004B600
	public void StopZombieIdleSound()
	{
		if (this.idleSoundHandler != null)
		{
			this.idleSoundHandler.OnSoundPlayed = null;
			this.idleSoundHandler.Stop();
			this.idleSoundHandler = null;
		}
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x0004D429 File Offset: 0x0004B629
	public virtual void HandleLoopStarted(ToolAnimationSMB machineBehaviour)
	{
		Action<ItemType> onToolLoopStarted = this.OnToolLoopStarted;
		if (onToolLoopStarted == null)
		{
			return;
		}
		onToolLoopStarted(machineBehaviour.itemType);
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x0004D441 File Offset: 0x0004B641
	public virtual void HandleLoopFinished(ToolAnimationSMB machineBehaviour)
	{
		Action<ItemType> onToolLoopFinished = this.OnToolLoopFinished;
		if (onToolLoopFinished == null)
		{
			return;
		}
		onToolLoopFinished(machineBehaviour.itemType);
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void OnDeathAnimationFinished()
	{
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x0004D459 File Offset: 0x0004B659
	public virtual void ResetForPool()
	{
		this.OnToolLoopStarted = null;
		this.OnToolLoopFinished = null;
		this.SetState(global::AnimationState.Idle);
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x0004D470 File Offset: 0x0004B670
	protected virtual void OnDisable()
	{
		this.CancelBowAimLoop();
		this.StopZombieIdleSound();
		LazyTimer.Stop(this.soundTimerId);
		if (!this.resetAnimatorOnDisable)
		{
			return;
		}
		if (this.animator != null)
		{
			this.SetState(global::AnimationState.Idle);
		}
		this.TryPlayResetClip();
	}

	// Token: 0x06000EE4 RID: 3812 RVA: 0x0004D4B0 File Offset: 0x0004B6B0
	private AnimationClip ResolveResetClip()
	{
		if (this.animatorResetClip == null)
		{
			return null;
		}
		RuntimeAnimatorController runtimeAnimatorController = ((this.animator != null) ? this.animator.runtimeAnimatorController : null);
		if (this.cachedResolvedResetClip != null && this.cachedResolvedAgainstController == runtimeAnimatorController && this.cachedResolvedAgainstBaseClip == this.animatorResetClip)
		{
			return this.cachedResolvedResetClip;
		}
		this.cachedResolvedResetClip = this.ResolveOverrideOrBase(runtimeAnimatorController);
		this.cachedResolvedAgainstController = runtimeAnimatorController;
		this.cachedResolvedAgainstBaseClip = this.animatorResetClip;
		return this.cachedResolvedResetClip;
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x0004D548 File Offset: 0x0004B748
	private AnimationClip ResolveOverrideOrBase(RuntimeAnimatorController controller)
	{
		AnimatorOverrideController animatorOverrideController = controller as AnimatorOverrideController;
		if (animatorOverrideController != null)
		{
			List<KeyValuePair<AnimationClip, AnimationClip>> list = new List<KeyValuePair<AnimationClip, AnimationClip>>(animatorOverrideController.overridesCount);
			animatorOverrideController.GetOverrides(list);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Key == this.animatorResetClip && list[i].Value != null)
				{
					return list[i].Value;
				}
			}
		}
		return this.animatorResetClip;
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x0004D5CC File Offset: 0x0004B7CC
	public virtual void OnAnimationStepCompleted()
	{
		SurfaceType surfaceType = this.GetSurfaceType();
		if (surfaceType == SurfaceType.None)
		{
			this.PlayAdditionalStepSound();
			return;
		}
		if (surfaceType != this.currentSurfaceType)
		{
			this.currentSurfaceType = surfaceType;
			if (this.currentSurfaceType != SurfaceType.None)
			{
				this.stepSoundId = LazySingletonSO<SurfaceStepSoundSettings>.Instance.GetStepSoundIdForSurfaceType(this.currentSurfaceType);
			}
		}
		if (string.IsNullOrEmpty(this.stepSoundId))
		{
			this.PlayAdditionalStepSound();
			return;
		}
		if (global::UnityEngine.Random.Range(0f, 100f) < (float)LazySingletonSO<SurfaceStepSoundSettings>.Instance.delayChance)
		{
			this.soundTimerId = LazyTimer.AddTimer(global::UnityEngine.Random.Range(LazySingletonSO<SurfaceStepSoundSettings>.Instance.minDelayTime, LazySingletonSO<SurfaceStepSoundSettings>.Instance.maxDelayTime), delegate
			{
				try
				{
					LazyAudio.PlayAtGameObject(this.stepSoundId, base.transform, SpatialType.sound3D, true);
					this.PlayAdditionalStepSound();
				}
				catch (Exception)
				{
				}
			}, null);
			return;
		}
		LazyAudio.PlayAtGameObject(this.stepSoundId, base.transform, SpatialType.sound3D, true);
		this.PlayAdditionalStepSound();
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x0004D694 File Offset: 0x0004B894
	private void PlayAdditionalStepSound()
	{
		if (!this.useAdditionalStepSound || string.IsNullOrEmpty(this.additionalStepSoundId))
		{
			return;
		}
		LazyAudio.PlayAtGameObject(this.additionalStepSoundId, base.transform, SpatialType.sound3D, true);
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x0004D6C0 File Offset: 0x0004B8C0
	private SurfaceType GetSurfaceType()
	{
		float num = 0.01666667f;
		Vector3 vector = base.transform.position + Vector3.up * num;
		Vector3 normalized = Physics.gravity.normalized;
		int num2 = Physics.RaycastNonAlloc(new Ray(vector, normalized), AnimationComponentBase.raycastHitsBuffer, 1f, 2060);
		if (num2 > 0)
		{
			int num3 = int.MaxValue;
			IOrderedSurface orderedSurface = null;
			for (int i = 0; i < num2; i++)
			{
				RaycastHit raycastHit = AnimationComponentBase.raycastHitsBuffer[i];
				IOrderedSurface component = raycastHit.collider.GetComponent<IOrderedSurface>();
				if (component != null && component.Depth < num3)
				{
					num3 = component.Depth;
					orderedSurface = component;
				}
			}
			if (orderedSurface != null)
			{
				return orderedSurface.SurfaceType;
			}
		}
		return SurfaceType.None;
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x0004D77D File Offset: 0x0004B97D
	private Animator ResolveAnimator()
	{
		if (this.animator != null)
		{
			return this.animator;
		}
		if (this.animatorLookupAttempted)
		{
			return null;
		}
		this.animatorLookupAttempted = true;
		this.animator = base.GetComponent<Animator>();
		return this.animator;
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x0004D7B8 File Offset: 0x0004B9B8
	protected virtual void InitInternal(SkinPresetGK2 skinPreset)
	{
		this.ResolveAnimator();
		if (this.dropView != null)
		{
			this.dropView.gameObject.SetActive(false);
		}
		this.animationEventReceiver = base.GetComponent<AnimationEventReceiver>();
		AnimationComponentBase.SteppedRotationPresetType steppedRotationPresetType = this.steppedRotationPresetType;
		if (steppedRotationPresetType == AnimationComponentBase.SteppedRotationPresetType.BasicNpc)
		{
			this.steppedRotationPreset = BasicNpcSteppedRotationPreset.Instance;
			return;
		}
		if (steppedRotationPresetType != AnimationComponentBase.SteppedRotationPresetType.Donkey)
		{
			return;
		}
		this.steppedRotationPreset = DonkeySteppedRotationPreset.Instance;
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x0004D820 File Offset: 0x0004BA20
	public void TryPlayResetClip()
	{
		AnimationClip animationClip = this.ResolveResetClip();
		if (animationClip != null)
		{
			animationClip.SampleAnimation(base.gameObject, 0f);
			return;
		}
		if (this.animator != null)
		{
			this.animator.WriteDefaultValues();
		}
	}

	// Token: 0x04001198 RID: 4504
	private const float RAYCAST_DISTANCE = 1f;

	// Token: 0x04001199 RID: 4505
	protected const string ANIM_STATE_ID = "State";

	// Token: 0x0400119A RID: 4506
	protected const string MOVEMENT_ANIM_DIRECTION_ID = "Direction";

	// Token: 0x0400119B RID: 4507
	protected const string WALK_SPEED = "walk_speed";

	// Token: 0x0400119C RID: 4508
	public static readonly string ATTACK_BLOCK_TRIGGER = "doBlock";

	// Token: 0x0400119D RID: 4509
	public static readonly string RESET_TO_IDLE_TRIGGER = "force_static";

	// Token: 0x0400119E RID: 4510
	public static readonly int idStateAnimator = Animator.StringToHash("State");

	// Token: 0x0400119F RID: 4511
	public static readonly int idDirectionAnimator = Animator.StringToHash("Direction");

	// Token: 0x040011A0 RID: 4512
	private static readonly int walkSpeedMultiplier = Animator.StringToHash("walk_speed");

	// Token: 0x040011A1 RID: 4513
	[SerializeField]
	private AnimationComponentBase.SteppedRotationPresetType steppedRotationPresetType;

	// Token: 0x040011A2 RID: 4514
	[SerializeField]
	protected Animator animator;

	// Token: 0x040011A3 RID: 4515
	[FormerlySerializedAs("playerDropView")]
	[SerializeField]
	[Space]
	protected DropViewAtomMesh dropView;

	// Token: 0x040011A4 RID: 4516
	[FormerlySerializedAs("playerDropViewParent")]
	[SerializeField]
	protected GameObject dropViewParent;

	// Token: 0x040011A5 RID: 4517
	[SerializeField]
	protected bool hasBlockAnimation;

	// Token: 0x040011A6 RID: 4518
	[SerializeField]
	[Space]
	private bool useAdditionalStepSound;

	// Token: 0x040011A7 RID: 4519
	[SerializeField]
	private string additionalStepSoundId;

	// Token: 0x040011A8 RID: 4520
	[SerializeField]
	[Space]
	[Tooltip("Primary reset mechanism. On disable, this clip is sampled at t=0 via AnimationClip.SampleAnimation, applying its curves to the hierarchy and reverting any animation-driven state (IsActive, transforms, material props, etc.) to the values captured when the 'Generate Reset Clip' button was pressed. If the animator uses an AnimatorOverrideController that overrides this clip, the overridden clip is sampled at runtime. When set, supersedes Reset Animator On Disable.")]
	protected AnimationClip animatorResetClip;

	// Token: 0x040011A9 RID: 4521
	[SerializeField]
	[Tooltip("Fallback: when no Animator Reset Clip is assigned and this is enabled, animator.WriteDefaultValues() is called on disable to revert animated properties to their bind-time values. Note: only useful when the prefab-authored state of the animated properties is the desired resting state. Ignored when Animator Reset Clip is assigned.")]
	protected bool resetAnimatorOnDisable;

	// Token: 0x040011AA RID: 4522
	private AnimationClip cachedResolvedResetClip;

	// Token: 0x040011AB RID: 4523
	private RuntimeAnimatorController cachedResolvedAgainstController;

	// Token: 0x040011AC RID: 4524
	private AnimationClip cachedResolvedAgainstBaseClip;

	// Token: 0x040011AD RID: 4525
	private SteppedRotationPreset steppedRotationPreset;

	// Token: 0x040011AE RID: 4526
	protected global::AnimationState animationState;

	// Token: 0x040011AF RID: 4527
	protected SkinPresetGK2 skinPreset;

	// Token: 0x040011B0 RID: 4528
	protected AnimationEventReceiver animationEventReceiver;

	// Token: 0x040011B1 RID: 4529
	private SurfaceType currentSurfaceType;

	// Token: 0x040011B2 RID: 4530
	private string stepSoundId = string.Empty;

	// Token: 0x040011B3 RID: 4531
	private int soundTimerId;

	// Token: 0x040011B4 RID: 4532
	private static RaycastHit[] raycastHitsBuffer = new RaycastHit[8];

	// Token: 0x040011B5 RID: 4533
	private float computedAngle;

	// Token: 0x040011B6 RID: 4534
	private SoundHandler bowSoundHandler;

	// Token: 0x040011B7 RID: 4535
	private SoundHandler idleSoundHandler;

	// Token: 0x040011B8 RID: 4536
	protected bool isInitialized;

	// Token: 0x040011B9 RID: 4537
	private bool animatorLookupAttempted;

	// Token: 0x040011BA RID: 4538
	[NonSerialized]
	private readonly List<DropViewAtomMesh> extraOverheadDropViews = new List<DropViewAtomMesh>();

	// Token: 0x02000247 RID: 583
	public enum SteppedRotationPresetType
	{
		// Token: 0x040011BC RID: 4540
		BasicNpc,
		// Token: 0x040011BD RID: 4541
		Donkey
	}
}
