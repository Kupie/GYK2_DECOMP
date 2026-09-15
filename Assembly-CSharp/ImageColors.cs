using System;
using UnityEngine;

// Token: 0x02000826 RID: 2086
[CreateAssetMenu(fileName = "NewImageColors", menuName = "UI/Image Colors", order = 1)]
public class ImageColors : ScriptableObject
{
	// Token: 0x170007F9 RID: 2041
	// (get) Token: 0x0600356D RID: 13677 RVA: 0x00101058 File Offset: 0x000FF258
	public Color NormalColor
	{
		get
		{
			return this.normalColor;
		}
	}

	// Token: 0x170007FA RID: 2042
	// (get) Token: 0x0600356E RID: 13678 RVA: 0x00101060 File Offset: 0x000FF260
	public Color HighlightedColorMouse
	{
		get
		{
			return this.highlightedColorMouse;
		}
	}

	// Token: 0x04002ADF RID: 10975
	[SerializeField]
	private Color normalColor;

	// Token: 0x04002AE0 RID: 10976
	[SerializeField]
	private Color highlightedColorMouse;
}
