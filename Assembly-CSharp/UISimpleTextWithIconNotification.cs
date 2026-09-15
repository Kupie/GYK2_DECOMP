using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200088F RID: 2191
public class UISimpleTextWithIconNotification : UIBaseNotification
{
	// Token: 0x1700085D RID: 2141
	// (get) Token: 0x0600383D RID: 14397 RVA: 0x0010E852 File Offset: 0x0010CA52
	// (set) Token: 0x0600383E RID: 14398 RVA: 0x0010E85A File Offset: 0x0010CA5A
	public string Text { get; set; }

	// Token: 0x1700085E RID: 2142
	// (get) Token: 0x0600383F RID: 14399 RVA: 0x0010E863 File Offset: 0x0010CA63
	// (set) Token: 0x06003840 RID: 14400 RVA: 0x0010E86B File Offset: 0x0010CA6B
	public string LocalizationKey { get; set; }

	// Token: 0x1700085F RID: 2143
	// (get) Token: 0x06003841 RID: 14401 RVA: 0x0010E874 File Offset: 0x0010CA74
	// (set) Token: 0x06003842 RID: 14402 RVA: 0x0010E87C File Offset: 0x0010CA7C
	public string IconId { get; set; }

	// Token: 0x06003843 RID: 14403 RVA: 0x0010E885 File Offset: 0x0010CA85
	public override void Draw()
	{
		this.label.text = this.Text;
		this.iconImage.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.IconId, null);
	}

	// Token: 0x06003844 RID: 14404 RVA: 0x0010E8B4 File Offset: 0x0010CAB4
	public override void ReleaseToPool()
	{
		LazyPooler.ReleaseObject<UISimpleTextWithIconNotification>(this);
	}

	// Token: 0x04002CC6 RID: 11462
	[SerializeField]
	private Image iconImage;

	// Token: 0x04002CC7 RID: 11463
	[SerializeField]
	private TextMeshProUGUI label;
}
