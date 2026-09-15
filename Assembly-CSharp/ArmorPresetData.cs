using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200060D RID: 1549
[CreateAssetMenu(fileName = "ArmorPresetData", menuName = "GK2/ArmorPresetData")]
public class ArmorPresetData : ScriptableObject
{
	// Token: 0x0600298F RID: 10639 RVA: 0x000C3FA2 File Offset: 0x000C21A2
	public ColorReplacePalette GetColorReplacePalette(int index)
	{
		return PaletteReplaceHelper.CombinePalettes(this.colorPalettes[index].GetPalettes(), this.sourcePalette);
	}

	// Token: 0x04002286 RID: 8838
	public List<ArmorColorPalette> colorPalettes;

	// Token: 0x04002287 RID: 8839
	public List<CustomizablePartType> affectedPartTypes;

	// Token: 0x04002288 RID: 8840
	public Texture2D sourcePalette;
}
