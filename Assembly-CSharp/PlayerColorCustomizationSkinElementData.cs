using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000614 RID: 1556
[Serializable]
public class PlayerColorCustomizationSkinElementData
{
	// Token: 0x0600299E RID: 10654 RVA: 0x000C47B1 File Offset: 0x000C29B1
	public Texture2D GetPaletteByIndex(int index)
	{
		if (index >= this.palettes.Count)
		{
			return null;
		}
		return this.palettes[index];
	}

	// Token: 0x040022A1 RID: 8865
	public int skinId;

	// Token: 0x040022A2 RID: 8866
	public List<Texture2D> palettes;
}
