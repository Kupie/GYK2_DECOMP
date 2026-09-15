using System;
using System.Collections.Generic;
using Cinemachine;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000803 RID: 2051
public class UIDialogBubble : UISpeechBubble
{
	// Token: 0x06003487 RID: 13447 RVA: 0x000FC9FD File Offset: 0x000FABFD
	public override void Init()
	{
		base.Init();
		UISpeechBubble.instance = this;
		Canvas component = base.GetComponent<Canvas>();
		component.overrideSorting = true;
		component.sortingOrder = 350;
	}

	// Token: 0x06003488 RID: 13448 RVA: 0x000FCA22 File Offset: 0x000FAC22
	public void OnDestroy()
	{
		CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.UpdatePos));
	}

	// Token: 0x06003489 RID: 13449 RVA: 0x000FCA3C File Offset: 0x000FAC3C
	public static UIDialogBubble ShowMessage(PhraseData data)
	{
		int num = (data.isPlayer ? MainGame.PlayerController.GetHashCode() : data.npcWgoData.GetHashCode());
		Vector3 vector = data.GetTargetPos();
		vector = CameraSystem.WorldToScreenPoint(vector);
		VoiceID voiceID;
		if (data.isPlayer)
		{
			voiceID = VoiceID.Feather;
		}
		else if (data.npcWgoData.Definition.voiceId == null || data.npcWgoData.Definition.voiceId == VoiceID.None)
		{
			voiceID = VoiceID.Feather;
		}
		else
		{
			voiceID = data.npcWgoData.Definition.voiceId;
		}
		UIDialogBubble uidialogBubble = UISpeechBubble.ShowMessage(num, data.text, vector, data.preset, voiceID, data.onFinished, null, null, null, data.cornerPosition, data.fixedShowTimeValue, true, default(Vector3)) as UIDialogBubble;
		if (uidialogBubble == null)
		{
			return null;
		}
		uidialogBubble.phraseData = data;
		uidialogBubble.hasVoiceOver = LazyAudio.VoiceOverPlayer.HasVoiceOver && VoiceOverSettings.IsEnabled;
		uidialogBubble.hasPortraitAnimator = false;
		CinemachineCore.CameraUpdatedEvent.AddListener(new UnityAction<CinemachineBrain>(uidialogBubble.UpdatePos));
		if (!uidialogBubble.phraseData.isPlayer)
		{
			uidialogBubble.DrawNPCRelatedStuff();
			Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(data.npcWgoData.UniqueId);
			if (wgoViewGlobal)
			{
				UIDialogBubble uidialogBubble2 = uidialogBubble;
				WgoPart mainWgoPart = wgoViewGlobal.MainWgoPart;
				uidialogBubble2.animationComponent = ((mainWgoPart != null) ? mainWgoPart.AnimationComponent : null) as AnimationComponent;
			}
		}
		else
		{
			uidialogBubble.animationComponent = MainGame.PlayerController.PhysicalBody.PlayerView.PlayerAnimation;
		}
		Canvas component = uidialogBubble.GetComponent<Canvas>();
		component.overrideSorting = true;
		component.sortingOrder = (data.isOverBlackout ? 900 : 350);
		uidialogBubble.EnsureTextFits();
		LayoutRebuilder.ForceRebuildLayoutImmediate(uidialogBubble.RootTransform);
		uidialogBubble.SyncCornerFadeMask();
		return uidialogBubble;
	}

	// Token: 0x0600348A RID: 13450 RVA: 0x000FCBFC File Offset: 0x000FADFC
	private void UpdatePos(CinemachineBrain brain)
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		Vector3 targetPos = this.phraseData.GetTargetPos();
		this.UpdatePositionAndCorner(CameraSystem.WorldToScreenPoint(targetPos), this.phraseData.cornerPosition);
	}

	// Token: 0x0600348B RID: 13451 RVA: 0x000FCC3C File Offset: 0x000FAE3C
	protected override void Update()
	{
		base.Update();
		if (!this.hasPortraitAnimator && !this.animationComponent)
		{
			return;
		}
		bool flag = (this.hasVoiceOver ? LazyAudio.VoiceOverPlayer.IsPlayingByLoud(base.VoiceId, base.VoiceOverLocalKey) : LazySpeechEngine.Instance.IsSpeechPlaying(base.VoiceId));
		if (this.hasPortraitAnimator)
		{
			this.portraitAnimator.SetBool(UIDialogBubble.isSpeakingPortrait, flag);
			return;
		}
		if (this.animationComponent.UseTalkingAnimation)
		{
			float num = (flag ? 1f : 0f);
			if (!this.animationComponent.GetLayerWeight(AnimationComponent.Layers.Talking).EqualsTo(num, 1E-05f))
			{
				this.animationComponent.SetLayerWeight(AnimationComponent.Layers.Talking, num);
				return;
			}
		}
		else if (this.animationComponent.HasTalkingHeadFrames)
		{
			if (flag)
			{
				this.animationComponent.StartTalkingHead();
				return;
			}
			this.animationComponent.PauseTalkingHead();
		}
	}

	// Token: 0x0600348C RID: 13452 RVA: 0x000FCD20 File Offset: 0x000FAF20
	private void OnDisable()
	{
		if (!this.animationComponent)
		{
			return;
		}
		if (this.animationComponent.UseTalkingAnimation)
		{
			this.animationComponent.SetLayerWeight(AnimationComponent.Layers.Talking, 0f);
		}
		this.animationComponent.StopTalkingHead();
		this.animationComponent = null;
	}

	// Token: 0x0600348D RID: 13453 RVA: 0x000FCD6C File Offset: 0x000FAF6C
	protected override void OnCornerChanged()
	{
		base.OnCornerChanged();
		this.SyncCornerFadeMask();
	}

	// Token: 0x0600348E RID: 13454 RVA: 0x000FCD7A File Offset: 0x000FAF7A
	private void OnEnable()
	{
		this.EnsureCornerFadeMask();
		this.SyncCornerFadeMask();
	}

	// Token: 0x0600348F RID: 13455 RVA: 0x000FCD88 File Offset: 0x000FAF88
	private void EnsureCornerFadeMask()
	{
		if (this.cornerFadeMask == null)
		{
			this.cornerFadeMask = SpeechBubbleCornerFadeMask.Ensure(this.background, this.corners, base.gameObject);
		}
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x000FCDB5 File Offset: 0x000FAFB5
	private void SyncCornerFadeMask()
	{
		this.EnsureCornerFadeMask();
		this.cornerFadeMask.Sync(this.currentCorner);
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x000FCDD0 File Offset: 0x000FAFD0
	private void DrawNPCRelatedStuff()
	{
		if (this.currentCorner != null && this.currentCorner.customText != null)
		{
			this.currentCorner.customText.text = this.phraseData.npcWgoData.id;
		}
		if (!this.phraseData.npcWgoData.Definition.usePortraitInDialogues)
		{
			this.portraitImage.transform.parent.gameObject.SetActive(false);
			return;
		}
		this.portraitImage.transform.parent.gameObject.SetActive(true);
		string speakerId = this.phraseData.npcWgoData.id;
		UIDialogBubble.PortraitControllerEntry portraitControllerEntry = this.portraitControllers.Find((UIDialogBubble.PortraitControllerEntry e) => e.speakerId == speakerId);
		if (((portraitControllerEntry != null) ? portraitControllerEntry.controller : null) != null)
		{
			this.hasPortraitAnimator = true;
			this.portraitAnimator.runtimeAnimatorController = portraitControllerEntry.controller;
			this.portraitAnimator.SetBool(UIDialogBubble.isSpeakingPortrait, false);
			return;
		}
		this.portraitAnimator.runtimeAnimatorController = null;
		this.portraitImage.sprite = this.phraseData.npcWgoData.Definition.Portrait;
	}

	// Token: 0x04002A05 RID: 10757
	private static readonly int isSpeakingPortrait = Animator.StringToHash("IsSpeaking");

	// Token: 0x04002A06 RID: 10758
	[SerializeField]
	private Image portraitImage;

	// Token: 0x04002A07 RID: 10759
	[SerializeField]
	private Animator portraitAnimator;

	// Token: 0x04002A08 RID: 10760
	[SerializeField]
	private List<UIDialogBubble.PortraitControllerEntry> portraitControllers = new List<UIDialogBubble.PortraitControllerEntry>();

	// Token: 0x04002A09 RID: 10761
	private PhraseData phraseData;

	// Token: 0x04002A0A RID: 10762
	private List<UIDialogBubble> dialogBubbles;

	// Token: 0x04002A0B RID: 10763
	private bool hasVoiceOver;

	// Token: 0x04002A0C RID: 10764
	private bool hasPortraitAnimator;

	// Token: 0x04002A0D RID: 10765
	private SpeechBubbleCornerFadeMask cornerFadeMask;

	// Token: 0x04002A0E RID: 10766
	public AnimationComponent animationComponent;

	// Token: 0x02000804 RID: 2052
	[Serializable]
	private class PortraitControllerEntry
	{
		// Token: 0x04002A0F RID: 10767
		public string speakerId;

		// Token: 0x04002A10 RID: 10768
		public RuntimeAnimatorController controller;
	}
}
