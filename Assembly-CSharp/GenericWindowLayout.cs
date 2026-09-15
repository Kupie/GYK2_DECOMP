using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009DC RID: 2524
public class GenericWindowLayout : MonoBehaviour
{
	// Token: 0x06004397 RID: 17303 RVA: 0x001419A0 File Offset: 0x0013FBA0
	public void UpdateSize(bool isSmall)
	{
		Image[] array;
		if (isSmall)
		{
			this.header.gameObject.SetActive(false);
			this.maskRectTransform.offsetMin = this.maskOffsetMinWithoutHeader;
			this.maskRectTransform.offsetMax = this.maskOffsetMaxWithoutHeader;
			this.windowLayoutGroup.padding.top = this.layoutGroupTopPaddingWithoutHeader;
			array = this.backgroundImages;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].sprite = this.backgroundSpriteSmall;
			}
			return;
		}
		this.header.gameObject.SetActive(true);
		this.maskRectTransform.offsetMin = this.maskOffsetMinWithHeader;
		this.maskRectTransform.offsetMax = this.maskOffsetMaxWithHeader;
		this.windowLayoutGroup.padding.top = this.layoutGroupTopPaddingWithHeader;
		array = this.backgroundImages;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sprite = this.backgroundSpriteBig;
		}
	}

	// Token: 0x040034B2 RID: 13490
	[SerializeField]
	private GameObject header;

	// Token: 0x040034B3 RID: 13491
	[SerializeField]
	private RectTransform maskRectTransform;

	// Token: 0x040034B4 RID: 13492
	[SerializeField]
	private Vector2 maskOffsetMinWithHeader = new Vector2(13f, 13f);

	// Token: 0x040034B5 RID: 13493
	[SerializeField]
	private Vector2 maskOffsetMaxWithHeader = new Vector2(13f, 36f);

	// Token: 0x040034B6 RID: 13494
	[SerializeField]
	private Vector2 maskOffsetMinWithoutHeader = new Vector2(13f, 13f);

	// Token: 0x040034B7 RID: 13495
	[SerializeField]
	private Vector2 maskOffsetMaxWithoutHeader = new Vector2(13f, 13f);

	// Token: 0x040034B8 RID: 13496
	[SerializeField]
	private VerticalLayoutGroup windowLayoutGroup;

	// Token: 0x040034B9 RID: 13497
	[SerializeField]
	private int layoutGroupTopPaddingWithHeader = 47;

	// Token: 0x040034BA RID: 13498
	[SerializeField]
	private int layoutGroupTopPaddingWithoutHeader = 24;

	// Token: 0x040034BB RID: 13499
	[SerializeField]
	private Image[] backgroundImages;

	// Token: 0x040034BC RID: 13500
	[SerializeField]
	private Sprite backgroundSpriteBig;

	// Token: 0x040034BD RID: 13501
	[SerializeField]
	private Sprite backgroundSpriteSmall;
}
