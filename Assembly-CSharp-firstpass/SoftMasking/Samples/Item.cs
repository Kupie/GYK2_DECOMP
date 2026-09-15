using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoftMasking.Samples
{
	// Token: 0x02000007 RID: 7
	public class Item : MonoBehaviour
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002248 File Offset: 0x00000448
		public void Set(string name, Sprite sprite, Color color, float health, float damage)
		{
			if (this.image)
			{
				this.image.sprite = sprite;
				this.image.color = color;
			}
			if (this.title)
			{
				this.title.text = name;
			}
			if (this.description)
			{
				this.description.text = "The short description of " + name;
			}
			if (this.healthBar)
			{
				this.healthBar.anchorMax = new Vector2(health, 1f);
			}
			if (this.damageBar)
			{
				this.damageBar.anchorMax = new Vector2(damage, 1f);
			}
		}

		// Token: 0x0400000D RID: 13
		public Image image;

		// Token: 0x0400000E RID: 14
		public Text title;

		// Token: 0x0400000F RID: 15
		public Text description;

		// Token: 0x04000010 RID: 16
		public RectTransform healthBar;

		// Token: 0x04000011 RID: 17
		public RectTransform damageBar;
	}
}
