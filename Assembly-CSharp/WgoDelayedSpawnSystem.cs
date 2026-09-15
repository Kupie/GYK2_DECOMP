using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000443 RID: 1091
public class WgoDelayedSpawnSystem : ICustomUpdatable
{
	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x06001CBB RID: 7355 RVA: 0x000866B9 File Offset: 0x000848B9
	private WgoDelayedSpawnSystemData Data
	{
		get
		{
			return this.data ?? MainGame.Instance.GameSave.wgoDelayedSpawnSystemData;
		}
	}

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x06001CBC RID: 7356 RVA: 0x000866D4 File Offset: 0x000848D4
	private float MinSpawnDistance
	{
		get
		{
			if (this.minSpawnDistance < 0f)
			{
				this.minSpawnDistance = GameBalance.Me.GetData<ConstDef>("spawn_distance").FloatValue;
			}
			return this.minSpawnDistance;
		}
	}

	// Token: 0x06001CBD RID: 7357 RVA: 0x00086704 File Offset: 0x00084904
	public void CustomUpdate(float deltaTime)
	{
		if (this.Data == null)
		{
			return;
		}
		List<SpawnDelayedObject> spawnDelayedObjects = this.Data.spawnDelayedObjects;
		for (int i = spawnDelayedObjects.Count - 1; i >= 0; i--)
		{
			if (this.TrySpawn(spawnDelayedObjects[i]))
			{
				spawnDelayedObjects.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001CBE RID: 7358 RVA: 0x0008674F File Offset: 0x0008494F
	public void Add(WgoData wgoData, CraftElementBase craftElement)
	{
		this.Data.spawnDelayedObjects.Add(new SpawnDelayedObject(craftElement as CraftElement, wgoData));
	}

	// Token: 0x06001CBF RID: 7359 RVA: 0x0008676D File Offset: 0x0008496D
	public bool CanSpawn(WgoData wgoData)
	{
		return Vector3.Distance(wgoData.Position, MainGame.PlayerData.position.Value) > this.MinSpawnDistance;
	}

	// Token: 0x06001CC0 RID: 7360 RVA: 0x00086791 File Offset: 0x00084991
	public bool Contains(WgoData wgoData)
	{
		return this.Data.Contains(wgoData);
	}

	// Token: 0x06001CC1 RID: 7361 RVA: 0x000867A0 File Offset: 0x000849A0
	private bool TrySpawn(SpawnDelayedObject spawnDelayedWgo)
	{
		WgoData resolvedWgoData = spawnDelayedWgo.ResolvedWgoData;
		if (resolvedWgoData == null)
		{
			return true;
		}
		if (!this.CanSpawn(resolvedWgoData))
		{
			return false;
		}
		CraftElement craftElement = spawnDelayedWgo.craftElement;
		if (craftElement == null)
		{
			return true;
		}
		craftElement.BindCraftable(resolvedWgoData);
		craftElement.Count = 1;
		resolvedWgoData.CraftComponent.AddCraftNoStart(craftElement);
		resolvedWgoData.OnCraftEnd(craftElement);
		return true;
	}

	// Token: 0x04001AC8 RID: 6856
	private float minSpawnDistance = -1f;

	// Token: 0x04001AC9 RID: 6857
	private WgoDelayedSpawnSystemData data;
}
