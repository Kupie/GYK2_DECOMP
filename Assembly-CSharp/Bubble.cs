using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007FC RID: 2044
public class Bubble : MonoBehaviour, ILazyGUIElement
{
	// Token: 0x0600346E RID: 13422 RVA: 0x000FC0A4 File Offset: 0x000FA2A4
	public void Init()
	{
		Bubble.instance = this;
		this.speechBubble.Init();
		this.multiAnswer.Init();
		this.interactingItem.Init();
		MainGame.OnGameStarted = (Action)Delegate.Combine(MainGame.OnGameStarted, new Action(this.OnGameStarted));
	}

	// Token: 0x0600346F RID: 13423 RVA: 0x000FC0F8 File Offset: 0x000FA2F8
	private void OnGameStarted()
	{
		UISpeechBubble.ForceRemoveAll();
		List<UIMultiAnswer> list = base.GetComponentsInChildren<UIMultiAnswer>().ToList<UIMultiAnswer>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != this.multiAnswer)
			{
				global::UnityEngine.Object.Destroy(list[i]);
				list.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06003470 RID: 13424 RVA: 0x000FC152 File Offset: 0x000FA352
	public static void ShowMultiAnswer(List<AnswerVisualData> answers, Transform targetTransform, WgoData dialogParticipant, Action<string> onChosen, Action onDisappeared, bool isOverBlackout = false)
	{
		UIMultiAnswer.ShowAnswers(answers, targetTransform, dialogParticipant, onChosen, onDisappeared, UIBasicBubble.ForceCornerPosition.Auto, false, isOverBlackout);
	}

	// Token: 0x06003471 RID: 13425 RVA: 0x000FC164 File Offset: 0x000FA364
	public static void Talk(PhraseData data)
	{
		if (data.isPlayer)
		{
			data.preset = ((data.speechType == SpeechBubbleType.Talk) ? Bubble.instance.speechBubblePresets[2] : Bubble.instance.speechBubblePresets[3]);
			Debug.Log("#talk# [Player]: " + data.text);
		}
		else
		{
			data.preset = ((data.speechType == SpeechBubbleType.Talk) ? Bubble.instance.speechBubblePresets[0] : Bubble.instance.speechBubblePresets[1]);
			if (data.npcWgoData == null)
			{
				Debug.LogError("Talk error: NPC WGO is null, text = " + data.text);
				data.onFinished();
				return;
			}
			Debug.Log("#talk# [" + data.npcWgoData.id + "]: " + data.text);
		}
		if (data.cornerPosition == UIBasicBubble.ForceCornerPosition.Auto)
		{
			Vector3 targetPos = data.GetTargetPos();
			Direction direction = data.GetDirection();
			if (direction == Direction.Left || direction == Direction.Right)
			{
				UIBasicBubble.ForceCornerPosition forceCornerPosition = Bubble.GetForceCornerPosition(UIBasicBubble.ForceCornerPosition.Auto, targetPos, direction, data.text, data.preset);
				data.cornerPosition = forceCornerPosition;
			}
		}
		UIDialogBubble.ShowMessage(data).GetComponent<RectTransform>().RefreshContentFitter();
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.SpeechSay, data.text);
	}

	// Token: 0x06003472 RID: 13426 RVA: 0x000FC29F File Offset: 0x000FA49F
	public static void SetVisibility(bool isVisible)
	{
		Bubble.instance.gameObject.SetActive(isVisible);
	}

	// Token: 0x06003473 RID: 13427 RVA: 0x000FC2B4 File Offset: 0x000FA4B4
	private static UIBasicBubble.ForceCornerPosition GetForceCornerPosition(UIBasicBubble.ForceCornerPosition forceCornerPosition, Vector3 pos, Direction direction, string textLocale, SpeechBubblePreset preset)
	{
		Bubble.<>c__DisplayClass17_0 CS$<>8__locals1;
		CS$<>8__locals1.pos = pos;
		CS$<>8__locals1.preset = preset;
		CS$<>8__locals1.textLocale = textLocale;
		if (forceCornerPosition == UIBasicBubble.ForceCornerPosition.Auto)
		{
			Bubble.<GetForceCornerPosition>g__CalcScreenPos|17_0(ref CS$<>8__locals1);
			Bubble.<GetForceCornerPosition>g__CalcScreenBounds|17_2(ref CS$<>8__locals1);
			Bubble.<GetForceCornerPosition>g__CalcBounds|17_1(ref CS$<>8__locals1);
			if (direction == Direction.Right && Bubble.<GetForceCornerPosition>g__Left|17_3(ref CS$<>8__locals1))
			{
				forceCornerPosition = (Bubble.<GetForceCornerPosition>g__Up|17_5(ref CS$<>8__locals1) ? UIBasicBubble.ForceCornerPosition.BottomRight : UIBasicBubble.ForceCornerPosition.TopRight);
			}
			else if (direction == Direction.Left && Bubble.<GetForceCornerPosition>g__Right|17_4(ref CS$<>8__locals1))
			{
				forceCornerPosition = (Bubble.<GetForceCornerPosition>g__Up|17_5(ref CS$<>8__locals1) ? UIBasicBubble.ForceCornerPosition.BottomLeft : UIBasicBubble.ForceCornerPosition.TopLeft);
			}
		}
		return forceCornerPosition;
	}

	// Token: 0x06003475 RID: 13429 RVA: 0x000FC32D File Offset: 0x000FA52D
	[CompilerGenerated]
	internal static void <GetForceCornerPosition>g__CalcScreenPos|17_0(ref Bubble.<>c__DisplayClass17_0 A_0)
	{
		A_0.screenPos = CameraSystem.WorldToScreenPoint(A_0.pos);
	}

	// Token: 0x06003476 RID: 13430 RVA: 0x000FC340 File Offset: 0x000FA540
	[CompilerGenerated]
	internal static void <GetForceCornerPosition>g__CalcBounds|17_1(ref Bubble.<>c__DisplayClass17_0 A_0)
	{
		A_0.preset.textStyle.ApplyStyle(Bubble.instance.speechBubbleLabel, false, null, null, null);
		Vector2 vector = LabelSizeCalculator.CalculateFitVector(Bubble.instance.speechBubbleLabel, A_0.preset.highlightedTextColorStyle.TranslateAndColorizeTags(A_0.textLocale), Bubble.instance.settings.preferredWidth);
		A_0.bounds = new Vector2(vector.x, vector.y);
		A_0.bounds.Scale(Vector2.one * LazyUI.ScaleFactor);
		A_0.bounds += new Vector2(Bubble.instance.boundsOffset, Bubble.instance.boundsOffsetY + Mathf.Abs(Bubble.instance.corners[1].rectTransform.localPosition.y)) * LazyUI.ScaleFactor;
	}

	// Token: 0x06003477 RID: 13431 RVA: 0x000FC442 File Offset: 0x000FA642
	[CompilerGenerated]
	internal static void <GetForceCornerPosition>g__CalcScreenBounds|17_2(ref Bubble.<>c__DisplayClass17_0 A_0)
	{
		A_0.screenBounds = LazyUI.GetScreenBounds();
	}

	// Token: 0x06003478 RID: 13432 RVA: 0x000FC44F File Offset: 0x000FA64F
	[CompilerGenerated]
	internal static bool <GetForceCornerPosition>g__Left|17_3(ref Bubble.<>c__DisplayClass17_0 A_0)
	{
		return A_0.screenPos.x - A_0.bounds.x > 0f;
	}

	// Token: 0x06003479 RID: 13433 RVA: 0x000FC46F File Offset: 0x000FA66F
	[CompilerGenerated]
	internal static bool <GetForceCornerPosition>g__Right|17_4(ref Bubble.<>c__DisplayClass17_0 A_0)
	{
		return A_0.screenPos.x + A_0.bounds.x < A_0.screenBounds.max.x;
	}

	// Token: 0x0600347A RID: 13434 RVA: 0x000FC49A File Offset: 0x000FA69A
	[CompilerGenerated]
	internal static bool <GetForceCornerPosition>g__Up|17_5(ref Bubble.<>c__DisplayClass17_0 A_0)
	{
		return A_0.screenPos.y + A_0.bounds.y < A_0.screenBounds.max.y;
	}

	// Token: 0x0600347B RID: 13435 RVA: 0x000FC4C5 File Offset: 0x000FA6C5
	[CompilerGenerated]
	internal static bool <GetForceCornerPosition>g__Down|17_6(ref Bubble.<>c__DisplayClass17_0 A_0)
	{
		return A_0.screenPos.y - A_0.bounds.y > 0f;
	}

	// Token: 0x040029DE RID: 10718
	[SerializeField]
	private float boundsOffset;

	// Token: 0x040029DF RID: 10719
	[SerializeField]
	private float boundsOffsetY;

	// Token: 0x040029E0 RID: 10720
	[SerializeField]
	private TextMeshProUGUI speechBubbleLabel;

	// Token: 0x040029E1 RID: 10721
	[SerializeField]
	protected SpeechBubbleSettings settings;

	// Token: 0x040029E2 RID: 10722
	[SerializeField]
	protected List<UIBubbleCorner> corners;

	// Token: 0x040029E3 RID: 10723
	[SerializeField]
	private UIDialogBubble speechBubble;

	// Token: 0x040029E4 RID: 10724
	[SerializeField]
	private UIMultiAnswer multiAnswer;

	// Token: 0x040029E5 RID: 10725
	[SerializeField]
	private UIInteractingItem interactingItem;

	// Token: 0x040029E6 RID: 10726
	[SerializeField]
	private List<Image> speechBubbleCorners;

	// Token: 0x040029E7 RID: 10727
	[SerializeField]
	private Sprite defaultCornerSprite;

	// Token: 0x040029E8 RID: 10728
	[Space]
	[SerializeField]
	private List<SpeechBubblePreset> speechBubblePresets;

	// Token: 0x040029E9 RID: 10729
	private static Bubble instance;
}
