using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000925 RID: 2341
public class TalentLevelUpsConnectorElement : MonoBehaviour
{
	// Token: 0x1700094B RID: 2379
	// (get) Token: 0x06003DB4 RID: 15796 RVA: 0x00126D88 File Offset: 0x00124F88
	public RectTransform RectTransform
	{
		get
		{
			return this.image.rectTransform;
		}
	}

	// Token: 0x06003DB5 RID: 15797 RVA: 0x00126D98 File Offset: 0x00124F98
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

	// Token: 0x06003DB6 RID: 15798 RVA: 0x00126DF0 File Offset: 0x00124FF0
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

	// Token: 0x0400307B RID: 12411
	public Image image;

	// Token: 0x0400307C RID: 12412
	public Vector2 extraSize;

	// Token: 0x0400307D RID: 12413
	public Sprite backgroundTypeSprite;

	// Token: 0x0400307E RID: 12414
	public Sprite foregroundTypeSprite;

	// Token: 0x0400307F RID: 12415
	public Color colorBackground;

	// Token: 0x04003080 RID: 12416
	public Color foregroundColorActive;

	// Token: 0x04003081 RID: 12417
	public Color foregroundColorInactive;
}
