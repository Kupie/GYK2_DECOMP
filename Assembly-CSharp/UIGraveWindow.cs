using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009E3 RID: 2531
public class UIGraveWindow : LazyWindow<UIGraveWindowData>
{
	// Token: 0x060043DF RID: 17375 RVA: 0x001429BC File Offset: 0x00140BBC
	public override void Init()
	{
		base.Init();
		this.skullPrefab.gameObject.SetActive(false);
		UIMouseTooltip.Attach(this.graveQualityValue.transform.parent.gameObject, "tt_grave_1", null, true, true, new UIMouseTooltipEdges(10f, 10f, 9f, 0f), default(Vector2), null);
	}

	// Token: 0x060043E0 RID: 17376 RVA: 0x00142A26 File Offset: 0x00140C26
	public override void Open(UIGraveWindowData data)
	{
		base.Open(data);
		data.WgoData.TrySetWorker(MainGame.PlayerController, null);
		this.TryStartTombstoneItemCellSelectionBlinking();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x060043E1 RID: 17377 RVA: 0x00142A5C File Offset: 0x00140C5C
	public override void Redraw()
	{
		base.Redraw();
		this.tombstoneWidget.Draw(this.data.TombstoneWidgetData);
		this.fenceWidget.Draw(this.data.FenceWidgetData);
		if (!this.data.CorpseWidgetData.IsEmpty)
		{
			this.corpseWIdget.Draw(this.data.CorpseWidgetData);
		}
		else
		{
			this.corpseWIdget.Hide();
		}
		int num = Math.Min(this.data.CorpseWidgetData.RedSkulls, 11);
		this.DrawQuality(num, QualityType.RedSkull, this.skullsCorpseParent, this.skullsCorpseParent2);
		int num2 = Math.Min(this.data.CorpseWidgetData.WhiteSkulls, 11 - num);
		this.DrawQuality(num2, QualityType.WhiteSkull, this.skullsCorpseParent, this.skullsCorpseParent2);
		int num3 = this.data.TombstoneWidgetData.Quality + this.data.FenceWidgetData.Quality;
		this.DrawQuality(Math.Min(num3, 11), QualityType.Wreath, this.skullsElementsParent, this.skullsElementsParent2);
		this.DrawCorpseSkullsOverflow(this.data.CorpseWidgetData.RedSkulls, this.data.CorpseWidgetData.WhiteSkulls);
		this.sliderValue2.transform.parent.gameObject.SetActive(false);
		this.DrawSkullsSliders(this.data.Quality);
		if (num3 > 11)
		{
			this.sliderValue2.transform.parent.gameObject.SetActive(true);
			this.sliderValue2.text = string.Format("+{0}", num3 - 11 - 1);
		}
		this.DrawQuality();
	}

	// Token: 0x060043E2 RID: 17378 RVA: 0x00142BFF File Offset: 0x00140DFF
	private void TryStartTombstoneItemCellSelectionBlinking()
	{
		if (MainGame.PlayerData.openedGraveWindowOnce)
		{
			return;
		}
		MainGame.PlayerData.openedGraveWindowOnce = true;
		this.tombstoneWidget.StartItemCellSelectionBlinking();
	}

	// Token: 0x060043E3 RID: 17379 RVA: 0x00142C24 File Offset: 0x00140E24
	public override void Hide()
	{
		base.Hide();
		foreach (TextMeshProUGUI textMeshProUGUI in this.skullElements)
		{
			textMeshProUGUI.gameObject.SetActive(false);
			this.skullPrefabsPool.Add(textMeshProUGUI);
		}
		this.skullElements.Clear();
		this.corpseWIdget.Hide();
	}

	// Token: 0x060043E4 RID: 17380 RVA: 0x00142CA4 File Offset: 0x00140EA4
	public override void Close()
	{
		PlayerController playerController = this.data.WgoData.Worker as PlayerController;
		if (playerController != null && playerController == MainGame.PlayerController)
		{
			this.data.WgoData.ClearWorker();
		}
		base.Close();
	}

	// Token: 0x060043E5 RID: 17381 RVA: 0x00142CED File Offset: 0x00140EED
	private void DrawQuality()
	{
		this.DrawQuality(this.data.Quality);
	}

	// Token: 0x060043E6 RID: 17382 RVA: 0x00142D00 File Offset: 0x00140F00
	private void DrawQuality(float quality)
	{
		this.graveQualityValue.text = quality.ToInvariantCultureString();
		this.DrawTopBg(quality);
		if (quality >= 0f)
		{
			this.graveStatusIcon.text = "wskull".FontIcon();
			this.whiteStyle.ApplyStyle(this.graveQualityValue, false, null, null, null);
			return;
		}
		this.graveStatusIcon.text = "wrskull".FontIcon();
		this.redStyle.ApplyStyle(this.graveQualityValue, false, null, null, null);
	}

	// Token: 0x060043E7 RID: 17383 RVA: 0x00142DB4 File Offset: 0x00140FB4
	private void DrawTopBg(float quality)
	{
		if (this.topBg == null)
		{
			return;
		}
		if (quality > 0f)
		{
			this.topBg.sprite = this.topBgWhite;
			this.topBg.gameObject.SetActive(true);
			return;
		}
		if (quality < 0f)
		{
			this.topBg.sprite = this.topBgRed;
			this.topBg.gameObject.SetActive(true);
			return;
		}
		this.topBg.gameObject.SetActive(false);
	}

	// Token: 0x060043E8 RID: 17384 RVA: 0x00142E38 File Offset: 0x00141038
	private void DrawSkullsSliders(float quality)
	{
		int num = Mathf.Clamp(this.data.CorpseWidgetData.RedSkulls, 0, 11);
		this.DrawSkullsSliders(quality, num);
	}

	// Token: 0x060043E9 RID: 17385 RVA: 0x00142E66 File Offset: 0x00141066
	private void DrawSkullsSliders(float quality, int redSkulls)
	{
		this.DrawRedSkullsSlider(quality, redSkulls);
		this.DrawWhiteSkullsSlider(quality, redSkulls);
	}

	// Token: 0x060043EA RID: 17386 RVA: 0x00142E78 File Offset: 0x00141078
	private void DrawRedSkullsSlider(float quality, int redSkulls)
	{
		if (this.skullsSliderRed == null)
		{
			return;
		}
		this.skullsSliderRed.value = this.ApplySliderValueTopOffset(this.skullsSliderRed, (float)redSkulls / 11f, this.skullsSliderRedTopOffset);
		this.skullsSliderRed.gameObject.SetActive(quality < 0f && this.skullsSliderRed.value > 0f);
	}

	// Token: 0x060043EB RID: 17387 RVA: 0x00142EE8 File Offset: 0x001410E8
	private void DrawWhiteSkullsSlider(float quality, int redSkulls)
	{
		if (this.skullsSliderWhite == null)
		{
			return;
		}
		float num = (float)redSkulls / 11f;
		float num2 = 1f - num;
		if (quality <= 0f || num2 <= 0f)
		{
			this.skullsSliderWhite.gameObject.SetActive(false);
			return;
		}
		this.SetSliderRectZone(this.skullsSliderWhite, num, 1f, this.skullsSliderWhiteBottomOffset, 0f);
		this.skullsSliderWhite.value = this.ApplySliderValueTopOffset(this.skullsSliderWhite, Mathf.Clamp01(quality / (float)(11 - redSkulls)), this.skullsSliderWhiteTopOffset);
		this.skullsSliderWhite.gameObject.SetActive(this.skullsSliderWhite.value > 0f);
	}

	// Token: 0x060043EC RID: 17388 RVA: 0x00142FA0 File Offset: 0x001411A0
	private float ApplySliderValueTopOffset(Slider slider, float value, float topOffset)
	{
		if (Mathf.Approximately(topOffset, 0f))
		{
			return value;
		}
		RectTransform rectTransform = ((slider.fillRect != null) ? (slider.fillRect.parent as RectTransform) : null);
		if (rectTransform == null || rectTransform.rect.height <= 0f)
		{
			return value;
		}
		return Mathf.Clamp01(value - topOffset / rectTransform.rect.height);
	}

	// Token: 0x060043ED RID: 17389 RVA: 0x00143018 File Offset: 0x00141218
	private void SetSliderRectZone(Slider slider, float anchorMinY, float anchorMaxY, float bottomOffset, float topOffset)
	{
		RectTransform rectTransform = slider.transform as RectTransform;
		if (rectTransform == null)
		{
			return;
		}
		Vector2 anchorMin = rectTransform.anchorMin;
		Vector2 anchorMax = rectTransform.anchorMax;
		anchorMin.y = anchorMinY;
		anchorMax.y = anchorMaxY;
		rectTransform.anchorMin = anchorMin;
		rectTransform.anchorMax = anchorMax;
		Vector2 offsetMin = rectTransform.offsetMin;
		Vector2 offsetMax = rectTransform.offsetMax;
		offsetMin.y = bottomOffset;
		offsetMax.y = -topOffset;
		rectTransform.offsetMin = offsetMin;
		rectTransform.offsetMax = offsetMax;
	}

	// Token: 0x060043EE RID: 17390 RVA: 0x00143098 File Offset: 0x00141298
	private void DrawCorpseSkullsOverflow(int redSkulls, int whiteSkulls)
	{
		if (this.sliderValue1 == null || this.sliderValue1.transform.parent == null)
		{
			return;
		}
		int num = redSkulls + whiteSkulls;
		this.sliderValue1.transform.parent.gameObject.SetActive(false);
		if (num > 11)
		{
			this.sliderValue1.transform.parent.gameObject.SetActive(true);
			this.sliderValue1.text = string.Format("+{0}", num - 11 - 1);
		}
	}

	// Token: 0x060043EF RID: 17391 RVA: 0x0014312C File Offset: 0x0014132C
	private void Editor_RedrawQualityPreview()
	{
		if (Application.isPlaying)
		{
			return;
		}
		int num = Mathf.Clamp(this.editorPreviewRedSkulls, 0, 11);
		int num2 = Mathf.Max(this.editorPreviewWhiteSkulls, 0);
		this.DrawQuality(this.editorPreviewQuality);
		this.DrawCorpseSkullsOverflow(num, num2);
		this.DrawSkullsSliders(this.editorPreviewQuality, num);
	}

	// Token: 0x060043F0 RID: 17392 RVA: 0x00143180 File Offset: 0x00141380
	private void DrawQuality(int count, QualityType qualityType, Transform parent1, Transform parent2)
	{
		for (int i = 0; i < count; i++)
		{
			TextMeshProUGUI textMeshProUGUI;
			TextMeshProUGUI textMeshProUGUI2;
			if (i > this.skullPrefabsPool.Count - 1)
			{
				textMeshProUGUI = this.skullPrefab.Copy(null, false, "");
				textMeshProUGUI2 = this.skullPrefab.Copy(null, false, "");
			}
			else
			{
				textMeshProUGUI = this.skullPrefabsPool.PopLast<TextMeshProUGUI>();
				textMeshProUGUI2 = this.skullPrefabsPool.PopLast<TextMeshProUGUI>();
			}
			string text = string.Empty;
			switch (qualityType)
			{
			case QualityType.RedSkull:
				text = "rskull".FontIcon();
				textMeshProUGUI.transform.SetParent(parent2);
				textMeshProUGUI2.transform.SetParent(parent1);
				break;
			case QualityType.WhiteSkull:
				text = "skull".FontIcon();
				textMeshProUGUI.transform.SetParent(parent1);
				textMeshProUGUI2.transform.SetParent(parent2);
				break;
			case QualityType.Wreath:
				if (this.data.CorpseWidgetData.RedSkulls > i)
				{
					text = "wr_red".FontIcon();
					textMeshProUGUI.transform.SetParent(parent2);
					textMeshProUGUI2.transform.SetParent(parent1);
				}
				else
				{
					text = "wr".FontIcon();
					textMeshProUGUI.transform.SetParent(parent1);
					textMeshProUGUI2.transform.SetParent(parent2);
				}
				break;
			}
			textMeshProUGUI.text = text;
			textMeshProUGUI.gameObject.SetActive(true);
			textMeshProUGUI.transform.SetAsLastSibling();
			this.skullElements.Add(textMeshProUGUI);
			UIMouseTooltip.Attach(textMeshProUGUI.gameObject, "tt_grave_4", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
			textMeshProUGUI2.text = string.Empty;
			textMeshProUGUI2.gameObject.SetActive(true);
			textMeshProUGUI2.transform.SetAsLastSibling();
			this.skullElements.Add(textMeshProUGUI2);
			UIMouseTooltip.Attach(textMeshProUGUI2.gameObject, "tt_grave_4", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
		}
	}

	// Token: 0x060043F1 RID: 17393 RVA: 0x0014335F File Offset: 0x0014155F
	private bool OnExhumeButtonPressed()
	{
		this.corpseWIdget.OnButtonPressed();
		return true;
	}

	// Token: 0x060043F2 RID: 17394 RVA: 0x0014336D File Offset: 0x0014156D
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.ExtractBody, new Func<bool>(this.OnExhumeButtonPressed));
		return gameKeyDelegates;
	}

	// Token: 0x060043F3 RID: 17395 RVA: 0x0014338C File Offset: 0x0014158C
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		list.Add(LazyGameKeyTip.Select(true, true, true));
		if (this.closeButton)
		{
			list.Add(LazyGameKeyTip.Back(true, true, true));
		}
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x060043F4 RID: 17396 RVA: 0x001433DC File Offset: 0x001415DC
	[LazyUITest]
	protected override void TestDraw()
	{
		WgoData wgoData = new WgoData("grave_body", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		wgoData.Inventory.AddItemToInventory(GameBalance.Me.GetData<BodyDef>("body_0_1").GenerateItem(), null, false);
		Wgo wgo = Wgo.Spawn(wgoData, MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		LazyUI.GetWindow<UIGraveWindow>().Open(new UIGraveWindowData(wgo));
	}

	// Token: 0x040034F2 RID: 13554
	private const int MAX_AMOUNT_OF_SKULLS = 11;

	// Token: 0x040034F3 RID: 13555
	[SerializeField]
	private TextMeshProUGUI graveStatusIcon;

	// Token: 0x040034F4 RID: 13556
	[SerializeField]
	private TextMeshProUGUI graveQualityValue;

	// Token: 0x040034F5 RID: 13557
	[SerializeField]
	private TextMeshProUGUI sliderValue1;

	// Token: 0x040034F6 RID: 13558
	[SerializeField]
	private TextMeshProUGUI sliderValue2;

	// Token: 0x040034F7 RID: 13559
	[SerializeField]
	private TextStyle redStyle;

	// Token: 0x040034F8 RID: 13560
	[SerializeField]
	private TextStyle whiteStyle;

	// Token: 0x040034F9 RID: 13561
	[SerializeField]
	private Transform skullsCorpseParent;

	// Token: 0x040034FA RID: 13562
	[SerializeField]
	private Transform skullsElementsParent;

	// Token: 0x040034FB RID: 13563
	[SerializeField]
	private Transform skullsCorpseParent2;

	// Token: 0x040034FC RID: 13564
	[SerializeField]
	private Transform skullsElementsParent2;

	// Token: 0x040034FD RID: 13565
	[SerializeField]
	private TextMeshProUGUI skullPrefab;

	// Token: 0x040034FE RID: 13566
	[SerializeField]
	private Slider skullsSliderRed;

	// Token: 0x040034FF RID: 13567
	[SerializeField]
	private Slider skullsSliderWhite;

	// Token: 0x04003500 RID: 13568
	[SerializeField]
	private float skullsSliderRedTopOffset;

	// Token: 0x04003501 RID: 13569
	[SerializeField]
	private float skullsSliderWhiteBottomOffset;

	// Token: 0x04003502 RID: 13570
	[SerializeField]
	private float skullsSliderWhiteTopOffset;

	// Token: 0x04003503 RID: 13571
	[SerializeField]
	private Image topBg;

	// Token: 0x04003504 RID: 13572
	[SerializeField]
	private Sprite topBgRed;

	// Token: 0x04003505 RID: 13573
	[SerializeField]
	private Sprite topBgWhite;

	// Token: 0x04003506 RID: 13574
	[SerializeField]
	private float editorPreviewQuality;

	// Token: 0x04003507 RID: 13575
	[SerializeField]
	private int editorPreviewRedSkulls;

	// Token: 0x04003508 RID: 13576
	[SerializeField]
	private int editorPreviewWhiteSkulls;

	// Token: 0x04003509 RID: 13577
	[SerializeField]
	private UIGraveElementWidget tombstoneWidget;

	// Token: 0x0400350A RID: 13578
	[SerializeField]
	private UIGraveElementWidget fenceWidget;

	// Token: 0x0400350B RID: 13579
	[SerializeField]
	private UICorpseWidget corpseWIdget;

	// Token: 0x0400350C RID: 13580
	private List<TextMeshProUGUI> skullPrefabsPool = new List<TextMeshProUGUI>();

	// Token: 0x0400350D RID: 13581
	private List<TextMeshProUGUI> skullElements = new List<TextMeshProUGUI>();
}
