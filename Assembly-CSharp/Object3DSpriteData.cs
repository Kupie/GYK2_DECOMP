using System;
using UnityEngine;

// Token: 0x02000519 RID: 1305
[Serializable]
public class Object3DSpriteData
{
	// Token: 0x060021B8 RID: 8632 RVA: 0x0009E8FB File Offset: 0x0009CAFB
	public Object3DSpriteData(GameObject obj, Sprite sprite)
	{
		this.obj = obj;
		this.sprite = sprite;
	}

	// Token: 0x04001E4F RID: 7759
	public GameObject obj;

	// Token: 0x04001E50 RID: 7760
	public Sprite sprite;
}
