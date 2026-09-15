using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008CC RID: 2252
public class UIToolIcon : MonoBehaviour
{
	// Token: 0x06003AD1 RID: 15057 RVA: 0x00118D50 File Offset: 0x00116F50
	public void Draw(ItemType itemType, bool isEquipped, TalentDef talentDef = null)
	{
		string text = (isEquipped ? "equipped" : "not_equipped");
		this.horizontalLayoutGroup = base.GetComponentInParent<HorizontalLayoutGroup>();
		if (this.horizontalLayoutGroup != null)
		{
			this.cachedSpacing = this.horizontalLayoutGroup.spacing;
			this.horizontalLayoutGroup.spacing = 0f;
		}
		this.toolIconImage.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("icon_" + itemType.ToString().ToLower() + "_" + text, null);
		this.toolIconImage.SetNativeSize();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06003AD2 RID: 15058 RVA: 0x00118DF8 File Offset: 0x00116FF8
	public void Hide()
	{
		base.gameObject.SetActive(false);
		if (this.cachedSpacing > -1f && this.horizontalLayoutGroup != null)
		{
			this.horizontalLayoutGroup.spacing = this.cachedSpacing;
			this.cachedSpacing = -1f;
			this.horizontalLayoutGroup = null;
		}
	}

	// Token: 0x04002E6F RID: 11887
	[SerializeField]
	private Image toolIconImage;

	// Token: 0x04002E70 RID: 11888
	private float cachedSpacing = -1f;

	// Token: 0x04002E71 RID: 11889
	private HorizontalLayoutGroup horizontalLayoutGroup;
}
