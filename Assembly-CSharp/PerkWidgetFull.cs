using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x02000945 RID: 2373
public class PerkWidgetFull : PerkWidget
{
	// Token: 0x17000971 RID: 2417
	// (get) Token: 0x06003E8F RID: 16015 RVA: 0x0012A4A0 File Offset: 0x001286A0
	// (set) Token: 0x06003E90 RID: 16016 RVA: 0x0012A4A8 File Offset: 0x001286A8
	public PerksWidgetSeparator NextItemSeparator { get; set; }

	// Token: 0x06003E91 RID: 16017 RVA: 0x0012A4B4 File Offset: 0x001286B4
	public override void Redraw()
	{
		base.Redraw();
		this.perkName.text = LLBase.L(this.data.PerkData.id);
		this.perkDescription.text = LLBase.L(this.data.PerkData.id + "_d");
		if (this.data.PerkData.Definition.perkType == PerkType.Buff)
		{
			this.durationLabel.rectTransform.anchoredPosition = this.durationPosWhenBuff;
			return;
		}
		this.durationLabel.rectTransform.anchoredPosition = this.durationPosWhenDefault;
	}

	// Token: 0x04003123 RID: 12579
	[SerializeField]
	private TextMeshProUGUI perkName;

	// Token: 0x04003124 RID: 12580
	[SerializeField]
	private TextMeshProUGUI perkDescription;

	// Token: 0x04003125 RID: 12581
	[SerializeField]
	private Vector2 durationPosWhenBuff;

	// Token: 0x04003126 RID: 12582
	[SerializeField]
	private Vector2 durationPosWhenDefault;
}
