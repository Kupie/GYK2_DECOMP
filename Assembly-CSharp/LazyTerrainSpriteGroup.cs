using System;
using System.Collections.Generic;

// Token: 0x020006B1 RID: 1713
[Serializable]
public class LazyTerrainSpriteGroup
{
	// Token: 0x06002DB3 RID: 11699 RVA: 0x000DAE04 File Offset: 0x000D9004
	public LazyTerrainSpriteDefinition GetDefaultSprite()
	{
		if (this.defaultSprite == null)
		{
			foreach (LazyTerrainSpriteDefinition lazyTerrainSpriteDefinition in this.sprites)
			{
				if (lazyTerrainSpriteDefinition.type == "C")
				{
					this.defaultSprite = lazyTerrainSpriteDefinition;
					return lazyTerrainSpriteDefinition;
				}
			}
			this.defaultSprite = this.sprites[0];
		}
		return this.defaultSprite;
	}

	// Token: 0x040024BA RID: 9402
	public string name;

	// Token: 0x040024BB RID: 9403
	public List<LazyTerrainSpriteDefinition> sprites = new List<LazyTerrainSpriteDefinition>();

	// Token: 0x040024BC RID: 9404
	[NonSerialized]
	private LazyTerrainSpriteDefinition defaultSprite;
}
