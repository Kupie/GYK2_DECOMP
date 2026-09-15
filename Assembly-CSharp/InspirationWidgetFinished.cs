using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200091A RID: 2330
public class InspirationWidgetFinished : LazyWidget<InspirationWidgetData>
{
	// Token: 0x17000940 RID: 2368
	// (get) Token: 0x06003D70 RID: 15728 RVA: 0x001259DA File Offset: 0x00123BDA
	public string IdWithoutLevel
	{
		get
		{
			InspirationWidgetData data = this.data;
			if (data == null)
			{
				return null;
			}
			return data.IdWithoutLevel;
		}
	}

	// Token: 0x17000941 RID: 2369
	// (get) Token: 0x06003D71 RID: 15729 RVA: 0x001259ED File Offset: 0x00123BED
	public int Level
	{
		get
		{
			InspirationWidgetData data = this.data;
			if (data == null)
			{
				return 0;
			}
			return data.CurrentLevel;
		}
	}

	// Token: 0x06003D72 RID: 15730 RVA: 0x00125A00 File Offset: 0x00123C00
	public override void Redraw()
	{
		InspirationWidgetData data = this.data;
		if (((data != null) ? data.InspirationDef : null) == null)
		{
			return;
		}
		InspirationDef inspirationDef = this.data.InspirationDef;
		this.idLabel.text = LLBase.L(inspirationDef.id);
		this.descriptionLabel.text = LLBase.L(inspirationDef.id + "_d");
		this.icon.sprite = inspirationDef.Icon;
		this.icon.BlueColorReplace(this.iconOutlineColor);
		int num = this.data.CurrentLevelFrame - 1;
		if (this.framesSprites != null && num >= 0 && num < this.framesSprites.Length)
		{
			this.iconFrame.sprite = this.framesSprites[num];
		}
	}

	// Token: 0x06003D73 RID: 15731 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x0400303F RID: 12351
	[SerializeField]
	private TextMeshProUGUI idLabel;

	// Token: 0x04003040 RID: 12352
	[SerializeField]
	private TextMeshProUGUI descriptionLabel;

	// Token: 0x04003041 RID: 12353
	[SerializeField]
	private Image icon;

	// Token: 0x04003042 RID: 12354
	[SerializeField]
	private Image iconFrame;

	// Token: 0x04003043 RID: 12355
	[SerializeField]
	private Color iconOutlineColor;

	// Token: 0x04003044 RID: 12356
	[SerializeField]
	private Sprite[] framesSprites;
}
