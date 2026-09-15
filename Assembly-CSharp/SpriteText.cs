using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000549 RID: 1353
public class SpriteText : MonoBehaviour
{
	// Token: 0x060022CC RID: 8908 RVA: 0x000A2FC0 File Offset: 0x000A11C0
	public void SetText(string txt)
	{
		foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>(true))
		{
			if (spriteRenderer != this.sprite)
			{
				global::UnityEngine.Object.Destroy(spriteRenderer.gameObject);
			}
		}
		this.sprite.gameObject.SetActive(false);
		float num = 0f;
		for (int j = txt.Length - 1; j >= 0; j--)
		{
			char c = txt[j];
			SpriteRenderer spriteRenderer2 = global::UnityEngine.Object.Instantiate<SpriteRenderer>(this.sprite, this.sprite.transform.parent);
			spriteRenderer2.gameObject.SetActive(true);
			spriteRenderer2.transform.localPosition = new Vector3(num, 0f);
			num -= this.padding;
			spriteRenderer2.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("sprfont_" + c.ToString(), null);
		}
	}

	// Token: 0x04001F69 RID: 8041
	private const string SPRITE_PREFIX = "sprfont_";

	// Token: 0x04001F6A RID: 8042
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x04001F6B RID: 8043
	[SerializeField]
	private float padding = 0.104166664f;
}
