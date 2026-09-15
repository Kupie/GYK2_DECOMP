using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000885 RID: 2181
public class UICustomizationUnlockedNotification : UIBaseNotification
{
	// Token: 0x17000850 RID: 2128
	// (get) Token: 0x060037FF RID: 14335 RVA: 0x0010E1FF File Offset: 0x0010C3FF
	// (set) Token: 0x06003800 RID: 14336 RVA: 0x0010E207 File Offset: 0x0010C407
	public Item SourceItem { get; set; }

	// Token: 0x06003801 RID: 14337 RVA: 0x0010E210 File Offset: 0x0010C410
	public override void Draw()
	{
		this.itemIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.SourceItem.Definition.iconId, null);
		this.itemIcon.BlueColorReplace(this.toReplace);
		this.label.text = LLBase.L("tech_customization") + " " + LLBase.L(this.SourceItem.id);
		LazyAudio.PlayAndForget("unlock");
	}

	// Token: 0x06003802 RID: 14338 RVA: 0x0010E28D File Offset: 0x0010C48D
	public override void ReleaseToPool()
	{
		LazyPooler.ReleaseObject<UICustomizationUnlockedNotification>(this);
	}

	// Token: 0x04002C98 RID: 11416
	[SerializeField]
	private Image itemIcon;

	// Token: 0x04002C99 RID: 11417
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04002C9A RID: 11418
	[SerializeField]
	private Color toReplace = new Color(1f, 1f, 1f, 0f);
}
