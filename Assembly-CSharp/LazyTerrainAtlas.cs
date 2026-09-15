using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020006AF RID: 1711
[CreateAssetMenu(menuName = "GK2/Lazy Terrain Atlas")]
public class LazyTerrainAtlas : ScriptableObject
{
	// Token: 0x040024AF RID: 9391
	public const int GRID_SIZE = 48;

	// Token: 0x040024B0 RID: 9392
	public LazyAtlas[] atlases;

	// Token: 0x040024B1 RID: 9393
	public Texture2D[] textures;

	// Token: 0x040024B2 RID: 9394
	public List<LazyTerrainSpriteGroup> spriteGroups = new List<LazyTerrainSpriteGroup>();
}
