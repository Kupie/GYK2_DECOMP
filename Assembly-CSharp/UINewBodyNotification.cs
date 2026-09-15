using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200088B RID: 2187
public class UINewBodyNotification : UIBaseNotification
{
	// Token: 0x17000858 RID: 2136
	// (get) Token: 0x06003823 RID: 14371 RVA: 0x0010E6A3 File Offset: 0x0010C8A3
	// (set) Token: 0x06003824 RID: 14372 RVA: 0x0010E6AB File Offset: 0x0010C8AB
	public ItemDef ItemDef { get; set; }

	// Token: 0x06003825 RID: 14373 RVA: 0x0010E6B4 File Offset: 0x0010C8B4
	public override void Draw()
	{
		this.iconImage.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.ItemDef.iconId, "i_body");
	}

	// Token: 0x06003826 RID: 14374 RVA: 0x0010E6DB File Offset: 0x0010C8DB
	public override void ReleaseToPool()
	{
		LazyPooler.ReleaseObject<UINewBodyNotification>(this);
	}

	// Token: 0x04002CB9 RID: 11449
	[SerializeField]
	private Image iconImage;
}
