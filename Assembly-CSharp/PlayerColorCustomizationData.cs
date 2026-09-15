using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000612 RID: 1554
[CreateAssetMenu(fileName = "PlayerColorCustomizationData", menuName = "GK2/PlayerColorCustomizationData")]
public class PlayerColorCustomizationData : ScriptableObject
{
	// Token: 0x0400229A RID: 8858
	public List<PlayerColorCustomizationElementData> customizationElements;

	// Token: 0x0400229B RID: 8859
	public List<CustomizablePartType> affectedPartTypes;

	// Token: 0x0400229C RID: 8860
	public Texture2D sourcePalette;

	// Token: 0x0400229D RID: 8861
	public ArmorPresetData armorPresetData;
}
