using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AC9 RID: 2761
public static class ImageExtensions
{
	// Token: 0x06004A9E RID: 19102 RVA: 0x00160430 File Offset: 0x0015E630
	public static void BlueColorReplace(this Image image, Color color)
	{
		Material material = image.material;
		image.material = new Material(material);
		image.material.SetColor(ImageExtensions.tint, color);
		if (!material.name.EndsWith("(Instance)"))
		{
			return;
		}
		global::UnityEngine.Object.Destroy(material);
	}

	// Token: 0x04003A77 RID: 14967
	private static readonly int tint = Shader.PropertyToID("_Color");
}
