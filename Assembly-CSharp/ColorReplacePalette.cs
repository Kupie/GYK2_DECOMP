using System;
using UnityEngine;

// Token: 0x0200060E RID: 1550
[CreateAssetMenu(fileName = "ColorReplacePalette", menuName = "GK2/Color Replace Palette", order = 1)]
public class ColorReplacePalette : ScriptableObject
{
	// Token: 0x06002991 RID: 10641 RVA: 0x000C3FC0 File Offset: 0x000C21C0
	public void UpdateColorsFromPalette()
	{
		if (this.palette == null || this.isLut)
		{
			this.colors = new Color[0];
			this.colorsNew = new Color[0];
			return;
		}
		if (this.palette.height != 2)
		{
			Debug.LogError(string.Format("Palette \"{0}\" should be of height = 2 (currently: {1}).", this.palette.name, this.palette.height), this.palette);
			return;
		}
		RenderTexture temporary = RenderTexture.GetTemporary(this.palette.width, this.palette.height);
		temporary.filterMode = FilterMode.Point;
		Graphics.Blit(this.palette, temporary);
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = temporary;
		Texture2D texture2D = new Texture2D(this.palette.width, this.palette.height);
		texture2D.filterMode = FilterMode.Point;
		texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = active;
		RenderTexture.ReleaseTemporary(temporary);
		this.colors = new Color[texture2D.width];
		this.colorsNew = new Color[texture2D.width];
		Color[] pixels = texture2D.GetPixels();
		for (int i = 0; i < texture2D.width; i++)
		{
			this.colors[i] = pixels[i + texture2D.width];
			this.colorsNew[i] = pixels[i];
		}
		global::UnityEngine.Object.Destroy(texture2D);
	}

	// Token: 0x06002992 RID: 10642 RVA: 0x000C413C File Offset: 0x000C233C
	public void ApplyToMaterial(Material material)
	{
		if (this.isLut)
		{
			material.DisableKeyword("USE_PALETTE_COLOR_REPLACE");
			material.DisableKeyword("USE_PARAMETER_COLOR_REPLACE");
			if (this.palette == null)
			{
				material.DisableKeyword("USE_LUT_COLOR_REPLACE");
				return;
			}
			material.EnableKeyword("USE_LUT_COLOR_REPLACE");
			material.SetTexture(ColorReplacePalette.idReplaceLut, this.palette);
			return;
		}
		else
		{
			if (this.colors.Length > 20)
			{
				material.EnableKeyword("USE_PALETTE_COLOR_REPLACE");
				material.SetTexture(ColorReplacePalette.idPalette, this.palette);
				return;
			}
			material.DisableKeyword("USE_PALETTE_COLOR_REPLACE");
			material.EnableKeyword("USE_PARAMETER_COLOR_REPLACE");
			for (int i = 0; i < ColorReplacePalette.idColorsA.Length; i++)
			{
				material.SetColor(ColorReplacePalette.idColorsA[i], (i < this.colors.Length) ? this.colors[i] : Color.black);
				material.SetColor(ColorReplacePalette.idColorsB[i], (i < this.colorsNew.Length) ? this.colorsNew[i] : Color.black);
			}
			return;
		}
	}

	// Token: 0x04002289 RID: 8841
	public bool isLut = true;

	// Token: 0x0400228A RID: 8842
	public Texture2D palette;

	// Token: 0x0400228B RID: 8843
	public Color[] colors;

	// Token: 0x0400228C RID: 8844
	public Color[] colorsNew;

	// Token: 0x0400228D RID: 8845
	private static readonly int idPalette = Shader.PropertyToID("_Palette");

	// Token: 0x0400228E RID: 8846
	private static readonly int idReplaceLut = Shader.PropertyToID("_ReplaceLUT");

	// Token: 0x0400228F RID: 8847
	private static readonly int[] idColorsA = new int[]
	{
		Shader.PropertyToID("_RColor1A"),
		Shader.PropertyToID("_RColor2A"),
		Shader.PropertyToID("_RColor3A"),
		Shader.PropertyToID("_RColor4A"),
		Shader.PropertyToID("_RColor5A"),
		Shader.PropertyToID("_RColor6A"),
		Shader.PropertyToID("_RColor7A"),
		Shader.PropertyToID("_RColor8A"),
		Shader.PropertyToID("_RColor9A"),
		Shader.PropertyToID("_RColor10A"),
		Shader.PropertyToID("_RColor11A"),
		Shader.PropertyToID("_RColor12A"),
		Shader.PropertyToID("_RColor13A"),
		Shader.PropertyToID("_RColor14A"),
		Shader.PropertyToID("_RColor15A"),
		Shader.PropertyToID("_RColor16A"),
		Shader.PropertyToID("_RColor17A"),
		Shader.PropertyToID("_RColor18A"),
		Shader.PropertyToID("_RColor19A"),
		Shader.PropertyToID("_RColor20A")
	};

	// Token: 0x04002290 RID: 8848
	private static readonly int[] idColorsB = new int[]
	{
		Shader.PropertyToID("_RColor1B"),
		Shader.PropertyToID("_RColor2B"),
		Shader.PropertyToID("_RColor3B"),
		Shader.PropertyToID("_RColor4B"),
		Shader.PropertyToID("_RColor5B"),
		Shader.PropertyToID("_RColor6B"),
		Shader.PropertyToID("_RColor7B"),
		Shader.PropertyToID("_RColor8B"),
		Shader.PropertyToID("_RColor9B"),
		Shader.PropertyToID("_RColor10B"),
		Shader.PropertyToID("_RColor11B"),
		Shader.PropertyToID("_RColor12B"),
		Shader.PropertyToID("_RColor13B"),
		Shader.PropertyToID("_RColor14B"),
		Shader.PropertyToID("_RColor15B"),
		Shader.PropertyToID("_RColor16B"),
		Shader.PropertyToID("_RColor17B"),
		Shader.PropertyToID("_RColor18B"),
		Shader.PropertyToID("_RColor19B"),
		Shader.PropertyToID("_RColor20B")
	};
}
