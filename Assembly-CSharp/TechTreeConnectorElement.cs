using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000955 RID: 2389
public class TechTreeConnectorElement : MonoBehaviour
{
	// Token: 0x17000982 RID: 2434
	// (get) Token: 0x06003F06 RID: 16134 RVA: 0x0012DBD2 File Offset: 0x0012BDD2
	public RectTransform RectTransform
	{
		get
		{
			return this.image.rectTransform;
		}
	}

	// Token: 0x06003F07 RID: 16135 RVA: 0x0012DBE0 File Offset: 0x0012BDE0
	public void SetupAsActive(bool background, bool useRepWidgetSource = false)
	{
		if (background)
		{
			this.image.sprite = this.GetBackgroundSprite(useRepWidgetSource);
			this.image.color = this.colorBackground;
			return;
		}
		this.image.sprite = this.GetForegroundSprite(useRepWidgetSource);
		this.image.color = this.foregroundColorActive;
	}

	// Token: 0x06003F08 RID: 16136 RVA: 0x0012DC38 File Offset: 0x0012BE38
	public void SetupAsInactive(bool background, bool useRepWidgetSource = false)
	{
		if (background)
		{
			this.image.sprite = this.GetBackgroundSprite(useRepWidgetSource);
			this.image.color = this.colorBackground;
			return;
		}
		this.image.sprite = this.GetForegroundSprite(useRepWidgetSource);
		this.image.color = this.foregroundColorInactive;
	}

	// Token: 0x06003F09 RID: 16137 RVA: 0x0012DC8F File Offset: 0x0012BE8F
	private Sprite GetBackgroundSprite(bool useRepWidgetSource)
	{
		if (!useRepWidgetSource || !(this.backgroundTypeSpriteRepWidgetSource != null))
		{
			return this.backgroundTypeSprite;
		}
		return this.backgroundTypeSpriteRepWidgetSource;
	}

	// Token: 0x06003F0A RID: 16138 RVA: 0x0012DCAF File Offset: 0x0012BEAF
	private Sprite GetForegroundSprite(bool useRepWidgetSource)
	{
		if (!useRepWidgetSource || !(this.foregroundTypeSpriteRepWidgetSource != null))
		{
			return this.foregroundTypeSprite;
		}
		return this.foregroundTypeSpriteRepWidgetSource;
	}

	// Token: 0x04003193 RID: 12691
	public Image image;

	// Token: 0x04003194 RID: 12692
	public Vector2 extraSize;

	// Token: 0x04003195 RID: 12693
	public Sprite backgroundTypeSprite;

	// Token: 0x04003196 RID: 12694
	public Sprite foregroundTypeSprite;

	// Token: 0x04003197 RID: 12695
	public Sprite backgroundTypeSpriteRepWidgetSource;

	// Token: 0x04003198 RID: 12696
	public Sprite foregroundTypeSpriteRepWidgetSource;

	// Token: 0x04003199 RID: 12697
	public Color colorBackground;

	// Token: 0x0400319A RID: 12698
	public Color foregroundColorActive;

	// Token: 0x0400319B RID: 12699
	public Color foregroundColorInactive;
}
