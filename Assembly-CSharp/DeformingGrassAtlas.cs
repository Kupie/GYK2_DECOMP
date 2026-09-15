using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004FC RID: 1276
[CreateAssetMenu(fileName = "DeformingGrassAtlas", menuName = "HP/Deforming Grass Atlas", order = 1)]
public class DeformingGrassAtlas : ScriptableObject
{
	// Token: 0x04001DCC RID: 7628
	public Texture2D texture;

	// Token: 0x04001DCD RID: 7629
	[HideInInspector]
	public List<int> variations = new List<int>();
}
