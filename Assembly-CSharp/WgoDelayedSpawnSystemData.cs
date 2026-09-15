using System;
using System.Collections.Generic;

// Token: 0x02000444 RID: 1092
[Serializable]
public class WgoDelayedSpawnSystemData
{
	// Token: 0x06001CC3 RID: 7363 RVA: 0x00086808 File Offset: 0x00084A08
	public bool Contains(WgoData wgoData)
	{
		if (wgoData == null)
		{
			return false;
		}
		SGuid uniqueId = wgoData.UniqueId;
		for (int i = 0; i < this.spawnDelayedObjects.Count; i++)
		{
			SpawnDelayedObject spawnDelayedObject = this.spawnDelayedObjects[i];
			if (spawnDelayedObject != null)
			{
				if (!SGuid.IsNullOrEmpty(spawnDelayedObject.wgoUniqueId) && spawnDelayedObject.wgoUniqueId == uniqueId)
				{
					return true;
				}
				if (spawnDelayedObject.wgoData != null && spawnDelayedObject.wgoData.UniqueId == uniqueId)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x04001ACA RID: 6858
	public List<SpawnDelayedObject> spawnDelayedObjects = new List<SpawnDelayedObject>();
}
