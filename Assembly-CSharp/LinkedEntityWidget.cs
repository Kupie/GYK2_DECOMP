using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020008C1 RID: 2241
public class LinkedEntityWidget : LazyWidget<LinkedEntityWidgetData>
{
	// Token: 0x170008C6 RID: 2246
	// (get) Token: 0x06003A6F RID: 14959 RVA: 0x001179E2 File Offset: 0x00115BE2
	public LinkedEntityWidgetData WidgetData
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x170008C7 RID: 2247
	// (get) Token: 0x06003A70 RID: 14960 RVA: 0x001179EA File Offset: 0x00115BEA
	public LazyButton Button
	{
		get
		{
			return this.button;
		}
	}

	// Token: 0x06003A71 RID: 14961 RVA: 0x001179F4 File Offset: 0x00115BF4
	public override void Redraw()
	{
		base.Redraw();
		this.icon.sprite = this.data.Icon;
		this.qualityIcon.sprite = this.data.QualityIcon;
		this.qualityIcon.gameObject.SetActive(this.qualityIcon.sprite != null);
		this.label.text = this.data.LabelText;
		if (this.data.LabelCustomStyle != null)
		{
			this.labelStyleComponent.SetTextStyle(this.data.LabelCustomStyle);
			this.labelStyleComponent.ApplyStyle();
		}
		this.fontIcon.text = this.data.FontIcon;
		this.fontIcon.gameObject.SetActive(!string.IsNullOrEmpty(this.data.FontIcon));
		this.label.gameObject.SetActive(!string.IsNullOrEmpty(this.data.LabelText));
		this.icon.gameObject.SetActive(this.icon.sprite != null);
		this.button.onClick.RemoveAllListeners();
		this.background.enabled = this.data.LinkedEntityType != LinkedEntityType.BuildingDef && this.data.LinkedEntityType != LinkedEntityType.PerkDef && this.data.LinkedEntityType != LinkedEntityType.TownBuildingDef;
		if (!this.data.IsInactive)
		{
			this.button.onClick.AddListener(delegate
			{
				if (this.data.OnClicked != null)
				{
					this.data.OnClicked();
					this.selectionFrame.SetActive(false);
					UITooltip.HideImmediately();
				}
			});
		}
		Image image = this.blockerImage;
		if (image == null)
		{
			return;
		}
		image.gameObject.SetActive(this.data.IsInactive);
	}

	// Token: 0x06003A72 RID: 14962 RVA: 0x00117BB0 File Offset: 0x00115DB0
	private void Awake()
	{
		this.button.onEnter.AddListener(new UnityAction(this.OnOver));
		this.button.onExit.AddListener(new UnityAction(this.OnOut));
		this.button.SetCallbacksIntoGamepadNavigationItem();
		this.selectionFrame.SetActive(false);
	}

	// Token: 0x06003A73 RID: 14963 RVA: 0x00117C0C File Offset: 0x00115E0C
	private void OnOver()
	{
		if (this.data == null)
		{
			return;
		}
		if (this.data.IsInactive)
		{
			return;
		}
		if ((this.data.OnClicked != null || LazyInput.IsGamepadActive) && !this.data.NoSelectionFrames && this.selectionFrame != null)
		{
			this.selectionFrame.SetActive(true);
		}
		LinkedEntityWidgetData data = this.data;
		bool flag;
		if (data != null)
		{
			LinkedEntityType linkedEntityType = data.LinkedEntityType;
			if (linkedEntityType == LinkedEntityType.GameRes || linkedEntityType == LinkedEntityType.DayNumber)
			{
				flag = true;
				goto IL_0071;
			}
		}
		flag = false;
		IL_0071:
		if (flag)
		{
			return;
		}
		UITooltip.ShowLinkedEntity(this);
	}

	// Token: 0x06003A74 RID: 14964 RVA: 0x00117C94 File Offset: 0x00115E94
	private void OnOut()
	{
		if (this.selectionFrame != null)
		{
			this.selectionFrame.SetActive(false);
		}
		UITooltip.Hide();
	}

	// Token: 0x06003A75 RID: 14965 RVA: 0x00117CB5 File Offset: 0x00115EB5
	private void OnDisable()
	{
		if (this.selectionFrame != null)
		{
			this.selectionFrame.SetActive(false);
		}
		if (UITooltip.IsTooltipShowingAtTarget(base.transform as RectTransform))
		{
			UITooltip.HideImmediately();
		}
	}

	// Token: 0x06003A76 RID: 14966 RVA: 0x00117CE8 File Offset: 0x00115EE8
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new LinkedEntityWidgetData(GameBalance.Me.GetData<ItemDef>("pickaxe_0"), null));
	}

	// Token: 0x04002E15 RID: 11797
	[SerializeField]
	private LazyButton button;

	// Token: 0x04002E16 RID: 11798
	[SerializeField]
	protected Image background;

	// Token: 0x04002E17 RID: 11799
	[SerializeField]
	private GameObject selectionFrame;

	// Token: 0x04002E18 RID: 11800
	[SerializeField]
	private Image icon;

	// Token: 0x04002E19 RID: 11801
	[SerializeField]
	private Image qualityIcon;

	// Token: 0x04002E1A RID: 11802
	[SerializeField]
	private TextMeshProUGUI fontIcon;

	// Token: 0x04002E1B RID: 11803
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04002E1C RID: 11804
	[SerializeField]
	private TextStyleComponent labelStyleComponent;

	// Token: 0x04002E1D RID: 11805
	[SerializeField]
	private GamepadNavigationItem gamepadNavigationItem;

	// Token: 0x04002E1E RID: 11806
	[SerializeField]
	private Image blockerImage;
}
