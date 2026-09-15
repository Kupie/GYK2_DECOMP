using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200094D RID: 2381
public class QuestTreeConnectorElement : MonoBehaviour
{
	// Token: 0x1700097B RID: 2427
	// (get) Token: 0x06003ECF RID: 16079 RVA: 0x0012C14C File Offset: 0x0012A34C
	public RectTransform RectTransform
	{
		get
		{
			return this.image.rectTransform;
		}
	}

	// Token: 0x06003ED0 RID: 16080 RVA: 0x0012C15C File Offset: 0x0012A35C
	public void SetupAsActive(bool background)
	{
		if (background)
		{
			this.image.sprite = this.backgroundTypeSprite;
			this.image.color = this.colorBackground;
			return;
		}
		this.image.sprite = this.foregroundTypeSprite;
		this.image.color = this.foregroundColorActive;
	}

	// Token: 0x06003ED1 RID: 16081 RVA: 0x0012C1B4 File Offset: 0x0012A3B4
	public void SetupAsInactive(bool background)
	{
		if (background)
		{
			this.image.sprite = this.backgroundTypeSprite;
			this.image.color = this.colorBackground;
			return;
		}
		this.image.sprite = this.foregroundTypeSprite;
		this.image.color = this.foregroundColorInactive;
	}

	// Token: 0x0400315A RID: 12634
	public Image image;

	// Token: 0x0400315B RID: 12635
	public Vector2 extraSize;

	// Token: 0x0400315C RID: 12636
	public Sprite backgroundTypeSprite;

	// Token: 0x0400315D RID: 12637
	public Sprite foregroundTypeSprite;

	// Token: 0x0400315E RID: 12638
	public Color colorBackground;

	// Token: 0x0400315F RID: 12639
	public Color foregroundColorActive;

	// Token: 0x04003160 RID: 12640
	public Color foregroundColorInactive;
}
