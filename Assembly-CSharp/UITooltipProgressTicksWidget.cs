using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x02000854 RID: 2132
public class UITooltipProgressTicksWidget : LazyWidget<UITooltipProgressTicksWidgetData>
{
	// Token: 0x060036A6 RID: 13990 RVA: 0x00108E44 File Offset: 0x00107044
	public override void Redraw()
	{
		this.talentIcon.Draw(this.data.TalentDef, string.Format("{0}/{1}", this.data.MasteryValue, this.data.MasteryLock));
		int masteryValue = this.data.MasteryValue;
		int masteryLock = this.data.MasteryLock;
		int num = 0;
		float num2;
		if (!this.data.IsStarCraft)
		{
			if (masteryValue < masteryLock)
			{
				num = 0;
				num2 = 0f;
			}
			else
			{
				float num3 = 100f / (float)masteryLock;
				int num4 = masteryValue - masteryLock;
				float num5 = num3 * (float)num4;
				num = 1 + (int)(num5 / 100f);
				num2 = num5 % 100f;
			}
		}
		else
		{
			float num6 = 100f / (float)masteryLock;
			if (masteryValue < masteryLock)
			{
				num2 = num6 * (float)masteryValue;
			}
			else
			{
				float num7 = 100f / (float)masteryLock;
				int num8 = masteryValue - masteryLock;
				float num9 = num7 * (float)num8;
				num = 1 + (int)(num9 / 100f);
				num2 = num9 % 100f;
			}
		}
		bool flag = num > 0 && num2 > 0f;
		num = Mathf.Clamp(num, 0, 4);
		int num10 = num + ((num2 > 0f) ? 1 : 0);
		if (num10 > 0)
		{
			this.progressBarWidget.Apply(num10, num, 0, null);
		}
		else
		{
			this.progressBarWidget.Hide();
		}
		this.chanceLabel.text = string.Format("{0}%", (int)num2);
		this.chanceLabel.gameObject.SetActive(num2 > 0f);
		if (flag)
		{
			this.additionalCellPlusLabel.gameObject.transform.SetSiblingIndex(this.additionalCellPlusLabel.gameObject.transform.parent.transform.childCount - 2);
			this.additionalCellPlusLabel.gameObject.SetActive(true);
			return;
		}
		this.additionalCellPlusLabel.gameObject.SetActive(false);
	}

	// Token: 0x060036A7 RID: 13991 RVA: 0x0010900D File Offset: 0x0010720D
	public override void Hide()
	{
		this.progressBarWidget.Hide();
		base.Hide();
	}

	// Token: 0x060036A8 RID: 13992 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002B99 RID: 11161
	private const int MAX_CELLS_AMOUNT = 4;

	// Token: 0x04002B9A RID: 11162
	[SerializeField]
	private ProgressBarWiget_Simplified progressBarWidget;

	// Token: 0x04002B9B RID: 11163
	[SerializeField]
	private UITalentIcon talentIcon;

	// Token: 0x04002B9C RID: 11164
	[SerializeField]
	private TextMeshProUGUI chanceLabel;

	// Token: 0x04002B9D RID: 11165
	[SerializeField]
	private TextMeshProUGUI additionalCellPlusLabel;
}
