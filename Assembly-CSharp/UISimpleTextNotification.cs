using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x0200088E RID: 2190
public class UISimpleTextNotification : UIBaseNotification
{
	// Token: 0x1700085B RID: 2139
	// (get) Token: 0x06003836 RID: 14390 RVA: 0x0010E815 File Offset: 0x0010CA15
	// (set) Token: 0x06003837 RID: 14391 RVA: 0x0010E81D File Offset: 0x0010CA1D
	public string Text { get; set; }

	// Token: 0x1700085C RID: 2140
	// (get) Token: 0x06003838 RID: 14392 RVA: 0x0010E826 File Offset: 0x0010CA26
	// (set) Token: 0x06003839 RID: 14393 RVA: 0x0010E82E File Offset: 0x0010CA2E
	public string LocalizationKey { get; set; }

	// Token: 0x0600383A RID: 14394 RVA: 0x0010E837 File Offset: 0x0010CA37
	public override void Draw()
	{
		this.label.text = this.Text;
	}

	// Token: 0x0600383B RID: 14395 RVA: 0x0010E84A File Offset: 0x0010CA4A
	public override void ReleaseToPool()
	{
		LazyPooler.ReleaseObject<UISimpleTextNotification>(this);
	}

	// Token: 0x04002CC3 RID: 11459
	[SerializeField]
	private TextMeshProUGUI label;
}
