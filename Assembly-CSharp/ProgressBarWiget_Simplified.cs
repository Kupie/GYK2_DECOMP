using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007E9 RID: 2025
public class ProgressBarWiget_Simplified : LazyWidget<ProgressBarWidget_SimplifiedData>
{
	// Token: 0x170007D6 RID: 2006
	// (get) Token: 0x06003416 RID: 13334 RVA: 0x000FB3CC File Offset: 0x000F95CC
	public RectTransform GreenFillRect
	{
		get
		{
			if (!(this.greenBarSlider != null))
			{
				return null;
			}
			return this.greenBarSlider.fillRect;
		}
	}

	// Token: 0x06003417 RID: 13335 RVA: 0x000FB3EC File Offset: 0x000F95EC
	public void Apply(int cellCount, int greenValue, int redValue = 0, CraftDef craftDef = null)
	{
		if (this.data != null && this.data.CellCount == cellCount && this.data.GreenValue == greenValue && this.data.RedValue == redValue && this.data.CraftDef == craftDef && base.gameObject.activeSelf)
		{
			return;
		}
		this.Draw(new ProgressBarWidget_SimplifiedData(cellCount, greenValue, redValue, craftDef));
	}

	// Token: 0x06003418 RID: 13336 RVA: 0x000FB458 File Offset: 0x000F9658
	public override void Redraw()
	{
		base.Redraw();
		int num = Mathf.Max(0, this.data.CellCount);
		this.greenBarLayoutElement.preferredWidth = (float)(7 * num);
		int num2 = Mathf.Max(1, num);
		this.greenBarSlider.maxValue = (float)num2;
		this.greenBarSlider.value = (float)Mathf.Clamp(this.data.GreenValue, 0, num);
		this.redBarSlider.maxValue = (float)num2;
		this.redBarSlider.value = (float)Mathf.Clamp(this.data.RedValue, 0, num);
		ProgressCellAtlasSize progressCellAtlasSize;
		if (this.bgImage.TryGetComponent<ProgressCellAtlasSize>(out progressCellAtlasSize))
		{
			progressCellAtlasSize.SetCellCount(num);
		}
		this.RedrawIcons();
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x000FB508 File Offset: 0x000F9708
	private void RedrawIcons()
	{
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		int num5 = -1;
		CraftDef craftDef = this.data.CraftDef;
		if (craftDef != null)
		{
			if (craftDef.isStarCraft)
			{
				num = craftDef.bronzeLevel - 1;
				num2 = craftDef.silverLevel - 1;
				num3 = craftDef.goldLevel - 1;
			}
			else
			{
				AutopsyTypeCraft autopsyTypeCraft = craftDef.autopsyTypeCraft;
				if (autopsyTypeCraft - AutopsyTypeCraft.ExtractOrgan > 1)
				{
					if (autopsyTypeCraft == AutopsyTypeCraft.ChangeOrgan)
					{
						num5 = craftDef.goldLevel - 1;
					}
				}
				else
				{
					num4 = craftDef.silverLevel - 1;
					num5 = craftDef.goldLevel - 1;
				}
			}
		}
		this.ApplyIcon(this.bronzeIconRectTr, num);
		this.ApplyIcon(this.silverIconRectTr, num2);
		this.ApplyIcon(this.goldIconRectTr, num3);
		this.ApplyIcon(this.brokenHeartIconRectTr, num4);
		this.ApplyIcon(this.heartIconRectTr, num5);
	}

	// Token: 0x0600341A RID: 13338 RVA: 0x000FB5D8 File Offset: 0x000F97D8
	private void ApplyIcon(RectTransform icon, int cellNumber)
	{
		if (icon == null)
		{
			return;
		}
		bool flag = cellNumber >= 0 && cellNumber <= this.data.CellCount;
		icon.gameObject.SetActive(flag);
		if (!flag)
		{
			return;
		}
		icon.anchorMin = new Vector2(0f, 0.5f);
		icon.anchorMax = new Vector2(0f, 0.5f);
		icon.pivot = new Vector2(0f, 0.5f);
		icon.anchoredPosition = new Vector2((float)(cellNumber * 7), -0.5f);
	}

	// Token: 0x0600341B RID: 13339 RVA: 0x000FB66B File Offset: 0x000F986B
	protected override void TestDraw()
	{
		this.Draw(new ProgressBarWidget_SimplifiedData(6, 3, 2, null));
	}

	// Token: 0x04002997 RID: 10647
	private const int CellPixelWidth = 7;

	// Token: 0x04002998 RID: 10648
	[SerializeField]
	private Slider greenBarSlider;

	// Token: 0x04002999 RID: 10649
	[SerializeField]
	private LayoutElement greenBarLayoutElement;

	// Token: 0x0400299A RID: 10650
	[SerializeField]
	private Slider redBarSlider;

	// Token: 0x0400299B RID: 10651
	[SerializeField]
	private Image bgImage;

	// Token: 0x0400299C RID: 10652
	[SerializeField]
	private RectTransform bronzeIconRectTr;

	// Token: 0x0400299D RID: 10653
	[SerializeField]
	private RectTransform silverIconRectTr;

	// Token: 0x0400299E RID: 10654
	[SerializeField]
	private RectTransform goldIconRectTr;

	// Token: 0x0400299F RID: 10655
	[SerializeField]
	private RectTransform brokenHeartIconRectTr;

	// Token: 0x040029A0 RID: 10656
	[SerializeField]
	private RectTransform heartIconRectTr;
}
