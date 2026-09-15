using System;
using UnityEngine;

// Token: 0x0200060F RID: 1551
[CreateAssetMenu(menuName = "Player Customization", fileName = "CustomizablePart")]
public class CustomizablePart : ScriptableObject
{
	// Token: 0x04002291 RID: 8849
	public int orderIndex;

	// Token: 0x04002292 RID: 8850
	public CustomizablePartType type;

	// Token: 0x04002293 RID: 8851
	public SkinPresetPartGK2 skinPart;
}
