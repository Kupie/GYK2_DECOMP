using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x02000143 RID: 323
	public class UISpeechBubble : UIBasicBubble
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x000215C4 File Offset: 0x0001F7C4
		public string TextToDisplay
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x000215CC File Offset: 0x0001F7CC
		protected VoiceID VoiceId
		{
			get
			{
				return this.voiceId;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x000215D4 File Offset: 0x0001F7D4
		protected string VoiceOverLocalKey
		{
			get
			{
				return this.voiceOverLocalKey;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x000215DC File Offset: 0x0001F7DC
		public LazySpeechEngine.VoiceData VoiceData
		{
			get
			{
				if (this.voiceData == null)
				{
					this.voiceData = LazySpeechEngine.Instance.GetVoiceData(this.voiceId);
				}
				return this.voiceData;
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00021602 File Offset: 0x0001F802
		private void Awake()
		{
			if (this.fontDefault == null)
			{
				this.fontDefault = this.textField.font;
				this.materialPresetDefault = this.textField.fontSharedMaterial;
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00021634 File Offset: 0x0001F834
		public virtual void Init()
		{
			if (UISpeechBubble.instance != null)
			{
				return;
			}
			UISpeechBubble.cachedCornerAnchoredPositions = new Vector2[this.corners.Count];
			for (int i = 0; i < this.corners.Count; i++)
			{
				UISpeechBubble.cachedCornerAnchoredPositions[i] = this.corners[i].rectTransform.anchoredPosition;
			}
			UISpeechBubble.cachedCornerImagesAnchoredPositions = new Vector2[this.cornerImages.Count];
			for (int j = 0; j < this.cornerImages.Count; j++)
			{
				UISpeechBubble.cachedCornerImagesAnchoredPositions[j] = this.cornerImages[j].rectTransform.anchoredPosition;
			}
			base.gameObject.SetActive(false);
			UISpeechBubble.instance = this;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x000216FC File Offset: 0x0001F8FC
		private void ApplyPreset(SpeechBubblePreset preset)
		{
			if (preset == null)
			{
				Debug.LogError("preset for SpeechBubbleType is null.");
				return;
			}
			this.SetColor(preset.backgroundColor);
			this.SetSprites(preset);
			preset.textStyle.ApplyStyle(this.textField, false, null, null, null);
			this.usingPreset = preset;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00021760 File Offset: 0x0001F960
		private void SetColor(Color color)
		{
			this.background.color = color;
			foreach (Image image in this.cornerImages)
			{
				image.color = color;
			}
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x000217C0 File Offset: 0x0001F9C0
		private void SetSprites(SpeechBubblePreset preset)
		{
			if (preset.backgroundSprite != null)
			{
				this.background.sprite = preset.backgroundSprite;
			}
			for (int i = 0; i < this.cornerImages.Count; i++)
			{
				Image image = this.cornerImages[i];
				UIBubbleCorner uibubbleCorner = this.corners[i];
				if (preset.cornerSprite != null)
				{
					image.sprite = preset.cornerSprite;
					image.SetNativeSize();
					image.enabled = true;
				}
				else
				{
					image.enabled = false;
				}
				Vector2 vector = UISpeechBubble.cachedCornerImagesAnchoredPositions[i];
				Vector2 vector2 = UISpeechBubble.cachedCornerAnchoredPositions[i];
				if (i < preset.spriteOffset.Length)
				{
					vector += preset.spriteOffset[i];
				}
				if (i < preset.cornerOffset.Length)
				{
					vector2 += preset.cornerOffset[i];
				}
				image.rectTransform.anchoredPosition = vector;
				uibubbleCorner.rectTransform.anchoredPosition = vector2;
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x000218BC File Offset: 0x0001FABC
		protected virtual void Update()
		{
			if (this.disableBubbleCounter > 0)
			{
				int num = this.disableBubbleCounter - 1;
				this.disableBubbleCounter = num;
				if (num <= 0)
				{
					if (this.useDisappearAnimation)
					{
						this.canvasGroup.DOFade(0f, this.settings.fadeTime).OnComplete(new TweenCallback(this.DoDisableBubble));
						this.TryRemoveBubbleFromActiveBubbles();
						return;
					}
					this.DoDisableBubble();
					this.TryRemoveBubbleFromActiveBubbles();
				}
				return;
			}
			if (this.bubblePauseCounter > 0f)
			{
				this.bubblePauseCounter -= Time.deltaTime;
				if (this.bubblePauseCounter <= 0f)
				{
					this.StartAppearAnimation();
				}
				return;
			}
			if (UISpeechBubble.isPaused)
			{
				return;
			}
			if (this.canSkipBubble)
			{
				this.CheckSkip();
			}
			this.textAnimator.CustomUpdate();
			this.CheckBubbleTime();
			if (this.textAnimator.IsAnimating && this.textHasLetters && this.playSound && (!LazyAudio.VoiceOverPlayer.HasVoiceOver || !VoiceOverSettings.IsEnabled) && this.voiceId != VoiceID.None)
			{
				float num2 = this.textAnimator.GetRemainingTime() + this.settings.voiceAdditionalAverageTime;
				LazySpeechEngine.Instance.Play(this.voiceId, num2);
			}
			if (this.playSound && this.VoiceData != null && this.VoiceData.useOneSampleOncePerCue)
			{
				this.playSound = false;
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00021A18 File Offset: 0x0001FC18
		private void WaitBeforeBubbleAppear(float time)
		{
			this.bubblePauseCounter = time;
			this.canvasGroup.alpha = 0f;
			this.textAnimator.StartTime += time;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00021A44 File Offset: 0x0001FC44
		private void CheckSkip()
		{
			bool flag = Input.GetMouseButtonDown(0);
			if (UISpeechBubble.customDialogSkipCondition != null)
			{
				flag = flag || UISpeechBubble.customDialogSkipCondition();
			}
			int num = 0;
			while (num < this.keysToSkip.Count && !flag)
			{
				if (LazyInput.GetKeyDown(this.keysToSkip[num]))
				{
					flag = true;
				}
				num++;
			}
			if (flag)
			{
				LazyInput.ClearAllKeysDown();
				if (this.textAnimator.IsAnimating)
				{
					this.textAnimator.Complete();
					return;
				}
				this.bubbleShowTime = -1f;
			}
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00021ACC File Offset: 0x0001FCCC
		private void CheckBubbleTime()
		{
			if (!this.textAnimator.IsAnimating)
			{
				if (this.bubbleShowTime > 0f)
				{
					this.bubbleShowTime -= Time.deltaTime;
				}
				Action action = this.onTextAnimationEnd;
				if (action != null)
				{
					action();
				}
				this.onTextAnimationEnd = null;
				if (this.disappearing || this.bubbleShowTime > 0f)
				{
					return;
				}
				this.disappearing = true;
				this.DisableBubble(true);
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00021B44 File Offset: 0x0001FD44
		protected virtual void LateUpdate()
		{
			if (this.onForceHideCondition != null && this.onForceHideCondition())
			{
				this.ForceHide();
				return;
			}
			if (this.onUpdatePosition != null)
			{
				this.UpdatePositionAndCorner(this.onUpdatePosition(), this.forcedCornerPosition);
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00021B91 File Offset: 0x0001FD91
		private void StartAppearAnimation()
		{
			this.canvasGroup.alpha = 0f;
			this.canvasGroup.DOFade(1f, this.settings.fadeTime);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00021BC0 File Offset: 0x0001FDC0
		protected virtual void DisableBubble(bool animated = false)
		{
			this.useDisappearAnimation = animated;
			this.playSound = false;
			if (UISpeechBubble.canReuseBubbles)
			{
				if (this.disableBubbleCounter == -1)
				{
					this.disableBubbleCounter = 2;
				}
			}
			else
			{
				this.DoDisableBubble();
				this.TryRemoveBubbleFromActiveBubbles();
			}
			LazyAudio.VoiceOverPlayer.Stop();
			this.onDisappearedForReusingBubbles = null;
			Action action = this.onDisappeared;
			this.onDisappeared = null;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00021C28 File Offset: 0x0001FE28
		private void TryRemoveBubbleFromActiveBubbles()
		{
			if (UISpeechBubble.activeBubbles.ContainsKey(this.speakerId) && UISpeechBubble.activeBubbles[this.speakerId] == this)
			{
				UISpeechBubble.activeBubbles.Remove(this.speakerId);
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00021C65 File Offset: 0x0001FE65
		private void DoDisableBubble()
		{
			this.canvasGroup.DOKill(false);
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00021C7F File Offset: 0x0001FE7F
		private void ForceHide()
		{
			this.DisableBubble(false);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00021C88 File Offset: 0x0001FE88
		private void ShowMessage(string localKey, UIBasicBubble.ForceCornerPosition forceCornerPosition = UIBasicBubble.ForceCornerPosition.Auto, bool useAppearAnimation = true, float fixedShowTime = 0f)
		{
			this.textField.text = string.Empty;
			this.forcedCornerPosition = forceCornerPosition;
			this.voiceOverLocalKey = localKey;
			this.text = this.usingPreset.highlightedTextColorStyle.TranslateAndColorizeTags(localKey);
			this.textHasLetters = this.text.Any(new Func<char, bool>(char.IsLetter));
			this.textAnimator.ShowMessage(this.textField, this.text, this.settings.letterAnimAppearTime);
			if (VoiceOverPlayer.IsMuted(localKey))
			{
				this.playSound = false;
			}
			else
			{
				LazyAudio.VoiceOverPlayer.Play(localKey, this.voiceId);
			}
			if (LazyAudio.VoiceOverPlayer.HasVoiceOver)
			{
				this.bubbleShowTime = this.CalculateVoiceOverBubbleShowTime() + 1f;
			}
			else if (fixedShowTime > 0f)
			{
				this.bubbleShowTime = fixedShowTime;
			}
			else
			{
				this.bubbleShowTime = this.settings.CalculateBubbleShowingTime(this.text);
			}
			this.textField.RecalculateClipping();
			this.UpdatePositionAndCorner(this.uiTargetPosition, this.forcedCornerPosition);
			base.gameObject.SetActive(true);
			this.EnsureTextFits();
			if (useAppearAnimation)
			{
				if (UISpeechBubble.activeBubbles.Count > 1)
				{
					this.WaitBeforeBubbleAppear(this.settings.fadeTime);
					return;
				}
				this.StartAppearAnimation();
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00021DD4 File Offset: 0x0001FFD4
		private float CalculateVoiceOverBubbleShowTime()
		{
			float clipLength = LazyAudio.VoiceOverPlayer.ClipLength;
			if (!UISpeechBubble.IsCurrentLanguageMatchingVoiceOver())
			{
				return clipLength;
			}
			float num = Mathf.Max(0f, this.textAnimator.GetRemainingTime());
			return Mathf.Max(0f, clipLength - VoiceOverSettings.AdditionalClipLength - num);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00021E20 File Offset: 0x00020020
		private static bool IsCurrentLanguageMatchingVoiceOver()
		{
			string text = (string.IsNullOrEmpty(VoiceOverSettings.LanguageId) ? "en" : VoiceOverSettings.LanguageId);
			return string.Equals(LLBase.CurrentLang, text, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00021E52 File Offset: 0x00020052
		protected virtual void EnsureTextFits()
		{
			this.textLayout.preferredWidth = LabelSizeCalculator.CalculateFitWidth(this.textField, this.text, this.settings.preferredWidth);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00021E7C File Offset: 0x0002007C
		public static UISpeechBubble ShowMessage(int speakerId, string localKey, Vector2 uiPosition, SpeechBubblePreset bubblePreset, VoiceID voiceId = null, Action onDisappeared = null, Action onTextAnimationEnd = null, Func<Vector2> onUpdatePosition = null, Func<bool> onForceHideCondition = null, UIBasicBubble.ForceCornerPosition forceCornerPosition = UIBasicBubble.ForceCornerPosition.Auto, float fixedShowTime = 0f, bool canSkipBubble = true, Vector3 localScale = default(Vector3))
		{
			return UISpeechBubble.ShowMessage(speakerId, localKey, uiPosition, bubblePreset, onDisappeared, onTextAnimationEnd, onUpdatePosition, onForceHideCondition, forceCornerPosition, fixedShowTime, canSkipBubble, delegate(UISpeechBubble bubble)
			{
				bubble.voiceId = voiceId ?? VoiceID.None;
			}, false, localScale);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00021EC0 File Offset: 0x000200C0
		private static UISpeechBubble ShowMessage(int speakerId, string localKey, Vector2 uiPosition, SpeechBubblePreset bubblePreset, Action onDisappeared = null, Action onTextAnimationEnd = null, Func<Vector2> onUpdatePosition = null, Func<bool> onForceHideCondition = null, UIBasicBubble.ForceCornerPosition forceCornerPosition = UIBasicBubble.ForceCornerPosition.Auto, float fixedShowTime = 0f, bool canSkipBubble = true, Action<UISpeechBubble> setVoiceParameters = null, bool forceCreateNewBubbleInsteadReuse = false, Vector3 localScale = default(Vector3))
		{
			if (UISpeechBubble.instance == null)
			{
				Debug.LogError("SpeechBubble called but not initialized.");
				return null;
			}
			bool flag = UISpeechBubble.activeBubbles.ContainsKey(speakerId);
			if (flag && (forceCreateNewBubbleInsteadReuse || !UISpeechBubble.canReuseBubbles))
			{
				UISpeechBubble.activeBubbles[speakerId].ForceHide();
				UISpeechBubble.activeBubbles.Remove(speakerId);
				flag = false;
			}
			UISpeechBubble uispeechBubble;
			if (flag)
			{
				uispeechBubble = UISpeechBubble.activeBubbles[speakerId];
				Action action = uispeechBubble.onDisappearedForReusingBubbles;
				if (action != null)
				{
					action();
				}
				uispeechBubble.disableBubbleCounter = -1;
				uispeechBubble.disappearing = false;
				uispeechBubble.playSound = true;
				uispeechBubble.voiceData = null;
			}
			else
			{
				uispeechBubble = UISpeechBubble.instance.Copy(null, true, "");
				UISpeechBubble.activeBubbles.Add(speakerId, uispeechBubble);
			}
			if (localScale != default(Vector3))
			{
				uispeechBubble.RootTransform.localScale = localScale;
			}
			uispeechBubble.uiTargetPosition = uiPosition;
			uispeechBubble.onDisappeared = onDisappeared;
			uispeechBubble.onDisappearedForReusingBubbles = onDisappeared;
			uispeechBubble.onTextAnimationEnd = onTextAnimationEnd;
			uispeechBubble.onUpdatePosition = onUpdatePosition;
			uispeechBubble.onForceHideCondition = onForceHideCondition;
			uispeechBubble.speakerId = speakerId;
			if (setVoiceParameters != null)
			{
				setVoiceParameters(uispeechBubble);
			}
			uispeechBubble.canSkipBubble = canSkipBubble;
			uispeechBubble.ApplyPreset(bubblePreset);
			uispeechBubble.ShowMessage(localKey, forceCornerPosition, !flag, fixedShowTime);
			return uispeechBubble;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00021FFC File Offset: 0x000201FC
		public static void ForceHideAll()
		{
			for (int i = UISpeechBubble.activeBubbles.Count - 1; i >= 0; i--)
			{
				UISpeechBubble.activeBubbles.ElementAt(i).Value.ForceHide();
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00022038 File Offset: 0x00020238
		public static void ForceRemoveAll()
		{
			for (int i = UISpeechBubble.activeBubbles.Count - 1; i >= 0; i--)
			{
				UISpeechBubble value = UISpeechBubble.activeBubbles.ElementAt(i).Value;
				value.DoDisableBubble();
				value.TryRemoveBubbleFromActiveBubbles();
			}
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0002207A File Offset: 0x0002027A
		protected override void OnCornerChanged()
		{
			base.OnCornerChanged();
			this.CheckBackgroundFlip();
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00022088 File Offset: 0x00020288
		protected virtual void CheckBackgroundFlip()
		{
			if (this.flipBackgroundHorizontally != UISpeechBubble.FlipHorizontallySetting.None)
			{
				int num = ((this.pickedCorner == UIBasicBubble.BubbleCornerDirection.RightDown || this.pickedCorner == UIBasicBubble.BubbleCornerDirection.RightDown) ? this.GetRightCornerSign() : (-this.GetRightCornerSign()));
				if (this.flipBackgroundHorizontally == UISpeechBubble.FlipHorizontallySetting.ForLeft)
				{
					num = -num;
				}
				this.background.transform.localScale = new Vector3((float)num, 1f, 1f);
			}
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000220EC File Offset: 0x000202EC
		protected virtual int GetRightCornerSign()
		{
			return -1;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x000220EF File Offset: 0x000202EF
		public static void SetCustomDialogSkipCondition(Func<bool> customDialogSkipCondition)
		{
			UISpeechBubble.customDialogSkipCondition = customDialogSkipCondition;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000220F7 File Offset: 0x000202F7
		public static void SetPause(bool isPaused)
		{
			UISpeechBubble.isPaused = isPaused;
		}

		// Token: 0x040003C7 RID: 967
		private const float VOICE_OVER_ADDITIONAL_SHOW_TIME_IN_SECONDS = 1f;

		// Token: 0x040003C8 RID: 968
		public static bool canReuseBubbles = true;

		// Token: 0x040003C9 RID: 969
		[SerializeField]
		protected SpeechBubbleSettings settings;

		// Token: 0x040003CA RID: 970
		[Space]
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x040003CB RID: 971
		[SerializeField]
		private TextMeshProUGUI textField;

		// Token: 0x040003CC RID: 972
		[SerializeField]
		protected LayoutElement textLayout;

		// Token: 0x040003CD RID: 973
		[SerializeField]
		protected Image background;

		// Token: 0x040003CE RID: 974
		[SerializeField]
		private List<Image> cornerImages;

		// Token: 0x040003CF RID: 975
		[SerializeField]
		private List<GameKey> keysToSkip = new List<GameKey>();

		// Token: 0x040003D0 RID: 976
		protected TextAnimator textAnimator = new TextAnimator();

		// Token: 0x040003D1 RID: 977
		private Action onTextAnimationEnd;

		// Token: 0x040003D2 RID: 978
		private Action onDisappeared;

		// Token: 0x040003D3 RID: 979
		private Action onDisappearedForReusingBubbles;

		// Token: 0x040003D4 RID: 980
		private Func<Vector2> onUpdatePosition;

		// Token: 0x040003D5 RID: 981
		private Func<bool> onForceHideCondition;

		// Token: 0x040003D6 RID: 982
		private Vector2 uiTargetPosition;

		// Token: 0x040003D7 RID: 983
		private int disableBubbleCounter = -1;

		// Token: 0x040003D8 RID: 984
		private bool useDisappearAnimation = true;

		// Token: 0x040003D9 RID: 985
		private bool canSkipBubble = true;

		// Token: 0x040003DA RID: 986
		private float bubblePauseCounter;

		// Token: 0x040003DB RID: 987
		[SerializeField]
		private UISpeechBubble.FlipHorizontallySetting flipBackgroundHorizontally;

		// Token: 0x040003DC RID: 988
		private static Dictionary<int, UISpeechBubble> activeBubbles = new Dictionary<int, UISpeechBubble>();

		// Token: 0x040003DD RID: 989
		protected static UISpeechBubble instance;

		// Token: 0x040003DE RID: 990
		private float bubbleShowTime;

		// Token: 0x040003DF RID: 991
		private bool disappearing;

		// Token: 0x040003E0 RID: 992
		private bool textHasLetters;

		// Token: 0x040003E1 RID: 993
		private bool playSound = true;

		// Token: 0x040003E2 RID: 994
		private string text;

		// Token: 0x040003E3 RID: 995
		private int lastTextIndex;

		// Token: 0x040003E4 RID: 996
		private int speakerId;

		// Token: 0x040003E5 RID: 997
		private SpeechBubblePreset usingPreset;

		// Token: 0x040003E6 RID: 998
		private TMP_FontAsset fontDefault;

		// Token: 0x040003E7 RID: 999
		private Material materialPresetDefault;

		// Token: 0x040003E8 RID: 1000
		private static bool isPaused;

		// Token: 0x040003E9 RID: 1001
		private static Func<bool> customDialogSkipCondition = null;

		// Token: 0x040003EA RID: 1002
		private static Vector2[] cachedCornerAnchoredPositions;

		// Token: 0x040003EB RID: 1003
		private static Vector2[] cachedCornerImagesAnchoredPositions;

		// Token: 0x040003EC RID: 1004
		private VoiceID voiceId;

		// Token: 0x040003ED RID: 1005
		private string voiceOverLocalKey;

		// Token: 0x040003EE RID: 1006
		private LazySpeechEngine.VoiceData voiceData;

		// Token: 0x020001F1 RID: 497
		[Serializable]
		private enum FlipHorizontallySetting
		{
			// Token: 0x040006A1 RID: 1697
			None,
			// Token: 0x040006A2 RID: 1698
			ForLeft,
			// Token: 0x040006A3 RID: 1699
			ForRight
		}
	}
}
