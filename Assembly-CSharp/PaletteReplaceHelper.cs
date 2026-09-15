using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000611 RID: 1553
public static class PaletteReplaceHelper
{
	// Token: 0x06002996 RID: 10646 RVA: 0x000C44B8 File Offset: 0x000C26B8
	public static ColorReplacePalette CombinePalettes(List<Texture2D> palettes, Texture2D sourcePalette)
	{
		ColorReplacePalette colorReplacePalette = ScriptableObject.CreateInstance<ColorReplacePalette>();
		Texture2D texture2D = new Texture2D(sourcePalette.width, 2, sourcePalette.format, false);
		texture2D.wrapMode = TextureWrapMode.Clamp;
		texture2D.filterMode = FilterMode.Point;
		colorReplacePalette.palette = texture2D;
		int width = sourcePalette.width;
		int num = 0;
		for (int i = 0; i < palettes.Count; i++)
		{
			num += palettes[i].width;
		}
		if (width != num)
		{
			Debug.LogError(string.Format("Incorrect palettesWidth! source:[{0}] all others:[{1}]", width, num));
			return colorReplacePalette;
		}
		Color[] array = new Color[texture2D.width * texture2D.height];
		colorReplacePalette.colors = new Color[texture2D.width];
		colorReplacePalette.colorsNew = new Color[texture2D.width];
		for (int j = 0; j < texture2D.width; j++)
		{
			colorReplacePalette.colors[j] = (array[texture2D.width + j] = sourcePalette.GetPixel(j, 0));
		}
		int num2 = 0;
		for (int k = 0; k < palettes.Count; k++)
		{
			for (int l = 0; l < palettes[k].width; l++)
			{
				colorReplacePalette.colorsNew[num2] = (array[num2] = palettes[k].GetPixel(l, 0));
				num2++;
			}
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return colorReplacePalette;
	}
}
