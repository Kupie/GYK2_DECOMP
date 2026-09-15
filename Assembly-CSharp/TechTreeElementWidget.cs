using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000959 RID: 2393
public class TechTreeElementWidget : TechTreeElementBaseWidget
{
	// Token: 0x17000984 RID: 2436
	// (get) Token: 0x06003F18 RID: 16152 RVA: 0x0012DDE4 File Offset: 0x0012BFE4
	public List<LinkedEntityWidget> ShownUnlocks
	{
		get
		{
			return this.shownUnlocks;
		}
	}

	// Token: 0x17000985 RID: 2437
	// (get) Token: 0x06003F19 RID: 16153 RVA: 0x0012DDEC File Offset: 0x0012BFEC
	public Image Background
	{
		get
		{
			return this.background;
		}
	}

	// Token: 0x06003F1A RID: 16154 RVA: 0x0012DDF4 File Offset: 0x0012BFF4
	public override void Redraw()
	{
		base.Redraw();
		this.ClearContent();
		this.button.interactable = false;
		this.priceLabel.gameObject.SetActive(true);
		this.content.gameObject.SetActive(true);
		this.hiddenObj.SetActive(false);
		this.visibleObj.SetActive(false);
		this.availableObj.SetActive(false);
		this.unlockedObj.SetActive(false);
		TechState visualTechState = this.data.VisualTechState;
		base.name = string.Format("{0}({1})", this.data.techDef.id, visualTechState);
		for (int i = 0; i < this.headers.Length; i++)
		{
			this.headers[i].text = LLBase.L(this.data.techDef.id);
		}
		string empty = string.Empty;
		switch (visualTechState)
		{
		case TechState.Visible:
			empty = this.data.techDef.GetPriceLabel(this.priceDisabledStyle, this.priceNotEnoughStyle, GameResIconType.TechPointSmall);
			break;
		case TechState.Available:
			empty = this.data.techDef.GetPriceLabel(this.priceNormalStyle, this.priceNotEnoughStyle, GameResIconType.TechPointSmall);
			break;
		}
		this.priceLabel.text = empty;
		for (int j = 0; j < this.data.techDef.linkedEntityWidgetDatas.Count; j++)
		{
			LinkedEntityWidget orCreateObject = TechTreePageWidget.UnlocksPool.GetOrCreateObject<LinkedEntityWidget>();
			orCreateObject.transform.SetParent(this.content.transform);
			this.data.techDef.linkedEntityWidgetDatas[j].NoSelectionFrames = !LazyInput.IsGamepadActive;
			this.data.techDef.linkedEntityWidgetDatas[j].OnClicked = new Action(base.OnClicked);
			orCreateObject.Draw(this.data.techDef.linkedEntityWidgetDatas[j]);
			this.shownUnlocks.Add(orCreateObject);
		}
		switch (visualTechState)
		{
		case TechState.Hidden:
			this.hiddenObj.SetActive(true);
			this.priceLabel.gameObject.SetActive(false);
			this.content.gameObject.SetActive(false);
			break;
		case TechState.Visible:
			this.visibleObj.SetActive(true);
			this.button.interactable = true;
			break;
		case TechState.Available:
			this.availableObj.SetActive(true);
			this.button.interactable = true;
			break;
		case TechState.Unlocked:
			this.button.interactable = true;
			this.unlockedObj.SetActive(true);
			this.priceLabel.gameObject.SetActive(false);
			break;
		}
		for (int k = 0; k < this.shownUnlocks.Count; k++)
		{
			LinkedEntityWidget linkedEntityWidget = this.shownUnlocks[k];
			linkedEntityWidget.transform.localScale = Vector3.one;
			linkedEntityWidget.transform.SetSiblingIndex(k);
		}
	}

	// Token: 0x06003F1B RID: 16155 RVA: 0x0012E0EA File Offset: 0x0012C2EA
	public override void Hide()
	{
		this.ClearContent();
		base.Hide();
	}

	// Token: 0x06003F1C RID: 16156 RVA: 0x0012E0F8 File Offset: 0x0012C2F8
	private void ClearContent()
	{
		foreach (LinkedEntityWidget linkedEntityWidget in this.shownUnlocks)
		{
			TechTreePageWidget.UnlocksPool.ReleaseObject<LinkedEntityWidget>(linkedEntityWidget);
		}
		this.shownUnlocks.Clear();
	}

	// Token: 0x06003F1D RID: 16157 RVA: 0x0012E15C File Offset: 0x0012C35C
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new TechTreeElementWidgetData());
	}

	// Token: 0x040031A3 RID: 12707
	[SerializeField]
	private GameObject content;

	// Token: 0x040031A4 RID: 12708
	[SerializeField]
	private Image background;

	// Token: 0x040031A5 RID: 12709
	[SerializeField]
	private TextStyle priceNormalStyle;

	// Token: 0x040031A6 RID: 12710
	[SerializeField]
	private TextStyle priceNotEnoughStyle;

	// Token: 0x040031A7 RID: 12711
	[SerializeField]
	private TextStyle priceDisabledStyle;

	// Token: 0x040031A8 RID: 12712
	[SerializeField]
	private TextMeshProUGUI priceLabel;

	// Token: 0x040031A9 RID: 12713
	[SerializeField]
	private TextMeshProUGUI[] headers;

	// Token: 0x040031AA RID: 12714
	private List<LinkedEntityWidget> shownUnlocks = new List<LinkedEntityWidget>();
}
