using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000990 RID: 2448
public class UICraftRequirementWidget : LazyWidget<UICraftRequirementWidgetData>
{
	// Token: 0x06004141 RID: 16705 RVA: 0x001373DC File Offset: 0x001355DC
	public override void Redraw()
	{
		base.Redraw();
		this.requirementId.text = this.data.IconId.FontIcon();
		this.requirement.text = this.data.RequirementValue;
		this.requirement.gameObject.SetActive(!string.IsNullOrEmpty(this.data.RequirementValue));
		if (this.data.IsRequirement)
		{
			this.requirementTextStyle.SetTextStyle(this.data.IsEnough ? this.enoughTextStyle : this.notEnoughTextStyle);
			return;
		}
		this.requirementTextStyle.SetTextStyle(this.commonTextStyle);
	}

	// Token: 0x06004142 RID: 16706 RVA: 0x00137488 File Offset: 0x00135688
	public void OnOver()
	{
		this.isHovered = true;
		this.ShowUITooltip();
	}

	// Token: 0x06004143 RID: 16707 RVA: 0x00137497 File Offset: 0x00135697
	public void OnOut()
	{
		this.isHovered = false;
		this.HideUITooltip(false);
	}

	// Token: 0x06004144 RID: 16708 RVA: 0x001374A7 File Offset: 0x001356A7
	private void OnDisable()
	{
		if (this.isHovered && UITooltip.IsTooltipShowingAtTarget(base.transform as RectTransform))
		{
			this.HideUITooltip(true);
		}
	}

	// Token: 0x06004145 RID: 16709 RVA: 0x001374CA File Offset: 0x001356CA
	public void ShowUITooltip()
	{
		this.isHovered = true;
		UITooltip.ShowCraftRequirementDescription(this, this.data.Id, this.data.CraftDef, this.data.LinkedActivePerks, this.data.ToolForWork);
	}

	// Token: 0x06004146 RID: 16710 RVA: 0x00137505 File Offset: 0x00135705
	public void HideUITooltip(bool immediately = false)
	{
		this.isHovered = false;
		if (immediately)
		{
			UITooltip.HideImmediately();
			return;
		}
		UITooltip.Hide();
	}

	// Token: 0x06004147 RID: 16711 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003310 RID: 13072
	[SerializeField]
	private TextMeshProUGUI requirementId;

	// Token: 0x04003311 RID: 13073
	[SerializeField]
	private TextMeshProUGUI requirement;

	// Token: 0x04003312 RID: 13074
	[SerializeField]
	private TextStyleComponent requirementTextStyle;

	// Token: 0x04003313 RID: 13075
	[SerializeField]
	private TextStyle commonTextStyle;

	// Token: 0x04003314 RID: 13076
	[SerializeField]
	private TextStyle enoughTextStyle;

	// Token: 0x04003315 RID: 13077
	[SerializeField]
	private TextStyle notEnoughTextStyle;

	// Token: 0x04003316 RID: 13078
	private bool isHovered;
}
