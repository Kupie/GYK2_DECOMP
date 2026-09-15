using System;
using LazyBearTechnology;

// Token: 0x02000529 RID: 1321
public class SyncedSpriteManager : LazySingleton<SyncedSpriteManager>
{
	// Token: 0x06002204 RID: 8708 RVA: 0x0009FD7D File Offset: 0x0009DF7D
	private void Update()
	{
		SyncedSprite.UpdateAllSyncedSprites();
	}
}
