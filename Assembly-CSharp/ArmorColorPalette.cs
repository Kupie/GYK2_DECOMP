using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200060C RID: 1548
[CreateAssetMenu(fileName = "ArmorColorPalette", menuName = "GK2/ArmorColorPalette")]
public class ArmorColorPalette : ScriptableObject
{
	// Token: 0x0600298D RID: 10637 RVA: 0x000C3F6B File Offset: 0x000C216B
	public List<Texture2D> GetPalettes()
	{
		return new List<Texture2D> { this.goldPalette, this.steelePalette, this.bluePalette, this.redPalette };
	}

	// Token: 0x04002282 RID: 8834
	public Texture2D goldPalette;

	// Token: 0x04002283 RID: 8835
	public Texture2D steelePalette;

	// Token: 0x04002284 RID: 8836
	public Texture2D bluePalette;

	// Token: 0x04002285 RID: 8837
	public Texture2D redPalette;
}
