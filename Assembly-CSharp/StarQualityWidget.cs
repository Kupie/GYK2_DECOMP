using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200097D RID: 2429
public class StarQualityWidget : LazyWidget<StarQualityWidgetData>
{
	// Token: 0x06004036 RID: 16438 RVA: 0x0013366F File Offset: 0x0013186F
	public override void Redraw()
	{
		base.Redraw();
		this.RedrawQualityBar();
	}

	// Token: 0x06004037 RID: 16439 RVA: 0x00133680 File Offset: 0x00131880
	private void RedrawQualityBar()
	{
		CraftParamsData craftParams = this.data.CraftParams;
		this.silverChanceField.text = string.Empty;
		this.goldChanceField.text = string.Empty;
		this.bronzeChanceSlider.value = 0f;
		this.silverChanceSlider.value = 0f;
		this.goldChanceSlider.value = 0f;
		if (craftParams.total <= 1f)
		{
			this.bronzeChanceSlider.value = 1f;
			return;
		}
		if (craftParams.total > 1f && craftParams.total <= 2f)
		{
			float num = (float)Mathf.RoundToInt(craftParams.total * 100f - 100f);
			this.silverChanceField.text = num.ToInvariantCultureString() + "%";
			this.bronzeChanceSlider.value = 1f;
			this.silverChanceSlider.value = num / 100f;
			return;
		}
		if (craftParams.total > 2f && craftParams.total <= 3f)
		{
			float num = (float)Mathf.RoundToInt(craftParams.total * 100f - 200f);
			this.silverChanceField.text = 100.ToString() + "%";
			this.goldChanceField.text = num.ToInvariantCultureString() + "%";
			this.bronzeChanceSlider.value = 1f;
			this.silverChanceSlider.value = 1f;
			this.goldChanceSlider.value = num / 100f;
			return;
		}
		if (craftParams.total > 3f)
		{
			this.silverChanceField.text = 100.ToString() + "%";
			this.goldChanceField.text = 100.ToString() + "%";
			this.bronzeChanceSlider.value = 1f;
			this.silverChanceSlider.value = 1f;
			this.goldChanceSlider.value = 1f;
		}
	}

	// Token: 0x06004038 RID: 16440 RVA: 0x00133899 File Offset: 0x00131A99
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new StarQualityWidgetData(new CraftParamsData("grape_wine", new WgoData("test_oven", Vector3.zero), CraftParamsData.CraftParamsType.Common, -1)));
	}

	// Token: 0x0400326C RID: 12908
	[SerializeField]
	private Slider bronzeChanceSlider;

	// Token: 0x0400326D RID: 12909
	[SerializeField]
	private Slider silverChanceSlider;

	// Token: 0x0400326E RID: 12910
	[SerializeField]
	private Slider goldChanceSlider;

	// Token: 0x0400326F RID: 12911
	[SerializeField]
	private TextMeshProUGUI silverChanceField;

	// Token: 0x04003270 RID: 12912
	[SerializeField]
	private TextMeshProUGUI goldChanceField;
}
