using System;
using UnityEngine;

// Token: 0x02000AB3 RID: 2739
[Serializable]
public class SkinPresetPartGK2
{
	// Token: 0x040039C2 RID: 14786
	[PreviewFormatInteger("0000")]
	public int id;

	// Token: 0x040039C3 RID: 14787
	public Color color = Color.white;

	// Token: 0x040039C4 RID: 14788
	public float hue;

	// Token: 0x040039C5 RID: 14789
	public float saturation = 1f;

	// Token: 0x040039C6 RID: 14790
	public float velocity = 1f;

	// Token: 0x040039C7 RID: 14791
	public Texture2D palette;

	// Token: 0x040039C8 RID: 14792
	public ColorReplaceType colorReplaceType;
}
