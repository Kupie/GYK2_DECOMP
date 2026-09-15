using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B9 RID: 1209
[Serializable]
public class ZombieSystemData
{
	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x0600202F RID: 8239 RVA: 0x0009883C File Offset: 0x00096A3C
	public Dictionary<Guid, ZombieWgoData> Cache
	{
		get
		{
			if (this.cache == null)
			{
				this.cache = new Dictionary<Guid, ZombieWgoData>();
				for (int i = 0; i < this.zombieDrops.Count; i++)
				{
					this.cache.Add(this.zombieDrops[i].UniqueId.Guid, this.zombieDrops[i]);
				}
				for (int j = this.zombieOnSceneWgoIds.Count - 1; j >= 0; j--)
				{
					SGuid sguid = this.zombieOnSceneWgoIds[j];
					ZombieWgoData zombieWgoData = MainGame.WorldData.GetWgoData(sguid) as ZombieWgoData;
					if (zombieWgoData == null)
					{
						this.zombieOnSceneWgoIds.RemoveAt(j);
					}
					else
					{
						this.cache.Add(sguid.Guid, zombieWgoData);
					}
				}
			}
			return this.cache;
		}
	}

	// Token: 0x06002030 RID: 8240 RVA: 0x00098904 File Offset: 0x00096B04
	public void PrepareForGame()
	{
		foreach (ZombieWgoData zombieWgoData in this.zombieDrops)
		{
			zombieWgoData.PrepareForGame();
		}
	}

	// Token: 0x06002031 RID: 8241 RVA: 0x00098954 File Offset: 0x00096B54
	public void ResumeCrafterWorkAfterLoad()
	{
		for (int i = 0; i < this.zombieOnSceneWgoIds.Count; i++)
		{
			ZombieWgoData zombie = this.GetZombie(this.zombieOnSceneWgoIds[i]);
			if (zombie != null)
			{
				zombie.TryResumeCrafterWorkAfterLoad();
			}
		}
	}

	// Token: 0x06002032 RID: 8242 RVA: 0x00098994 File Offset: 0x00096B94
	public ZombieWgoData CreateZombieDrop(string id, Vector3 position, string gameSceneId, Item zombieItem, string collarId = "collar_bronze", string name = null, bool rollSkin = true)
	{
		ZombieWgoData zombieWgoData = new ZombieWgoData(id, position, gameSceneId);
		zombieItem.UniqueId.SetGuid(zombieWgoData.UniqueId);
		this.Cache.Add(zombieWgoData.UniqueId.Guid, zombieWgoData);
		this.zombieDrops.Add(zombieWgoData);
		zombieWgoData.SetZombieItem(zombieItem);
		foreach (Item item in zombieItem.Inventory)
		{
			if (!string.IsNullOrEmpty(item.Definition.bodyLinkedPerk))
			{
				zombieWgoData.AddPerk(item.Definition.bodyLinkedPerk);
			}
		}
		zombieItem.AddItemToInventory(new Item(collarId, 1), false);
		zombieWgoData.equippedCollar.SetGuid(zombieItem.GetItemByType(ItemType.Collar).UniqueId);
		BodyZombieStartItemsSerializedItemProperty bodyZombieStartItemsSerializedItemProperty;
		if (zombieItem.TryGetProperty<BodyZombieStartItemsSerializedItemProperty>(out bodyZombieStartItemsSerializedItemProperty))
		{
			if (!string.IsNullOrEmpty(bodyZombieStartItemsSerializedItemProperty.handsId))
			{
				Item item2 = new Item(bodyZombieStartItemsSerializedItemProperty.handsId, 1);
				zombieItem.AddItemToInventory(item2, false);
				zombieWgoData.equippedHand.SetGuid(item2.UniqueId);
			}
			if (!string.IsNullOrEmpty(bodyZombieStartItemsSerializedItemProperty.armorId))
			{
				Item item3 = new Item(bodyZombieStartItemsSerializedItemProperty.armorId, 1);
				zombieItem.AddItemToInventory(item3, false);
				zombieWgoData.equippedArmor.SetGuid(item3.UniqueId);
			}
			zombieItem.RemoveProperty<BodyZombieStartItemsSerializedItemProperty>();
		}
		else
		{
			Debug.Log("Not found body start items property for zombie item: " + zombieItem.id);
		}
		if (rollSkin)
		{
			ZombieSkinHelper.RollAndApplyZombieSkinToWgoData(zombieWgoData, "zombie_worker");
		}
		return zombieWgoData;
	}

	// Token: 0x06002033 RID: 8243 RVA: 0x00098B20 File Offset: 0x00096D20
	public void PutZombieFromGameSceneToStore(ZombieWgoData zombieWgoData)
	{
		MainGame.WorldData.RemoveWgoDataFromGameScene(zombieWgoData.UniqueId);
		this.zombieOnSceneWgoIds.Remove(zombieWgoData.UniqueId);
		this.zombieDrops.Add(zombieWgoData);
	}

	// Token: 0x06002034 RID: 8244 RVA: 0x00098B50 File Offset: 0x00096D50
	public void PutZombieFromGameSceneToStoreForPlayer(PlayerData playerData, ZombieWgoData zombieWgoData)
	{
		playerData.AddOverheadItem(zombieWgoData.ZombieItem);
		this.PutZombieFromGameSceneToStore(zombieWgoData);
	}

	// Token: 0x06002035 RID: 8245 RVA: 0x00098B68 File Offset: 0x00096D68
	public ZombieWgoData PutZombieFromStoreToGameScene(SGuid uniqueId, string gameSceneId, Vector3 position, Direction direction = Direction.Down)
	{
		ZombieWgoData zombie = this.GetZombie(uniqueId);
		this.zombieOnSceneWgoIds.Add(zombie.UniqueId);
		this.zombieDrops.Remove(zombie);
		zombie.WorldId = gameSceneId;
		zombie.Position = position;
		zombie.direction.Value = direction.ConvertToVector2XZ();
		zombie.SetDefaultAnimState();
		zombie.PrepareForGameBase();
		MainGame.WorldData.AddWgoData(zombie);
		WGODef definition = zombie.Definition;
		if (!string.IsNullOrEmpty((definition != null) ? definition.attachedScript : null))
		{
			WgoDataScriptsManager.CreateScript(zombie, zombie.Definition.attachedScript);
		}
		return zombie;
	}

	// Token: 0x06002036 RID: 8246 RVA: 0x00098BFD File Offset: 0x00096DFD
	public ZombieWgoData PutZombieFromStoreToGameSceneFromPlayer(PlayerData playerData, string gameSceneId, Vector3 position, Direction direction = Direction.Down)
	{
		return this.PutZombieFromStoreToGameSceneFromPlayer(playerData, ZombieSystemData.ResolveOverheadZombieItem(playerData, null), gameSceneId, position, direction);
	}

	// Token: 0x06002037 RID: 8247 RVA: 0x00098C11 File Offset: 0x00096E11
	public ZombieWgoData PutZombieFromStoreToGameSceneFromPlayer(PlayerData playerData, Item zombieItem, string gameSceneId, Vector3 position, Direction direction = Direction.Down)
	{
		zombieItem = ZombieSystemData.ResolveOverheadZombieItem(playerData, zombieItem);
		ZombieWgoData zombieWgoData = this.PutZombieFromStoreToGameScene(zombieItem.UniqueId, gameSceneId, position, direction);
		playerData.RemoveOverheadItem(zombieItem);
		return zombieWgoData;
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x00098C34 File Offset: 0x00096E34
	public ZombieWgoData PutZombieFromStoreToGameSceneAsAssistant(PlayerData playerData, string gameSceneId, Vector3 position, Direction direction = Direction.Down)
	{
		return this.PutZombieFromStoreToGameSceneAsAssistant(playerData, ZombieSystemData.ResolveOverheadZombieItem(playerData, null), gameSceneId, position, direction);
	}

	// Token: 0x06002039 RID: 8249 RVA: 0x00098C48 File Offset: 0x00096E48
	public ZombieWgoData PutZombieFromStoreToGameSceneAsAssistant(PlayerData playerData, Item zombieItem, string gameSceneId, Vector3 position, Direction direction = Direction.Down)
	{
		zombieItem = ZombieSystemData.ResolveOverheadZombieItem(playerData, zombieItem);
		ZombieWgoData zombie = this.GetZombie(zombieItem.UniqueId);
		this.zombieDrops.Remove(zombie);
		this.Cache.Remove(zombie.UniqueId.Guid);
		ZombieWgoData zombieWgoData = zombie.CreateAssistantFromThis(position, gameSceneId, direction);
		MainGame.WorldData.AddWgoData(zombieWgoData);
		this.Cache.Add(zombieWgoData.UniqueId.Guid, zombieWgoData);
		this.zombieOnSceneWgoIds.Add(zombieWgoData.UniqueId);
		playerData.RemoveOverheadItem(zombieItem);
		return zombieWgoData;
	}

	// Token: 0x0600203A RID: 8250 RVA: 0x00098CD6 File Offset: 0x00096ED6
	public ZombieWgoData PutZombieFromStoreToGameSceneAsCommon(PlayerData playerData, string gameSceneId, Vector3 position, Direction direction = Direction.Down)
	{
		return this.PutZombieFromStoreToGameSceneAsCommon(playerData, ZombieSystemData.ResolveOverheadZombieItem(playerData, null), gameSceneId, position, direction);
	}

	// Token: 0x0600203B RID: 8251 RVA: 0x00098CEC File Offset: 0x00096EEC
	public ZombieWgoData PutZombieFromStoreToGameSceneAsCommon(PlayerData playerData, Item zombieItem, string gameSceneId, Vector3 position, Direction direction = Direction.Down)
	{
		zombieItem = ZombieSystemData.ResolveOverheadZombieItem(playerData, zombieItem);
		ZombieWgoData zombie = this.GetZombie(zombieItem.UniqueId);
		if (zombie.id == "zombie")
		{
			return this.PutZombieFromStoreToGameSceneFromPlayer(playerData, zombieItem, gameSceneId, position, direction);
		}
		this.zombieDrops.Remove(zombie);
		this.Cache.Remove(zombie.UniqueId.Guid);
		ZombieWgoData zombieWgoData = zombie.CreateCommonZombieFromThis(position, gameSceneId, direction);
		MainGame.WorldData.AddWgoData(zombieWgoData);
		this.Cache.Add(zombieWgoData.UniqueId.Guid, zombieWgoData);
		this.zombieOnSceneWgoIds.Add(zombieWgoData.UniqueId);
		playerData.RemoveOverheadItem(zombieItem);
		return zombieWgoData;
	}

	// Token: 0x0600203C RID: 8252 RVA: 0x00098D9A File Offset: 0x00096F9A
	public ZombieWgoData PutZombieFromStoreToGameSceneAsGardener(PlayerData playerData, string gameSceneId, Vector3 position, Direction direction = Direction.Down)
	{
		return this.PutZombieFromStoreToGameSceneAsGardener(playerData, ZombieSystemData.ResolveOverheadZombieItem(playerData, null), gameSceneId, position, direction);
	}

	// Token: 0x0600203D RID: 8253 RVA: 0x00098DB0 File Offset: 0x00096FB0
	public ZombieWgoData PutZombieFromStoreToGameSceneAsGardener(PlayerData playerData, Item zombieItem, string gameSceneId, Vector3 position, Direction direction = Direction.Down)
	{
		zombieItem = ZombieSystemData.ResolveOverheadZombieItem(playerData, zombieItem);
		ZombieWgoData zombie = this.GetZombie(zombieItem.UniqueId);
		if (zombie.id == "zombie")
		{
			return this.PutZombieFromStoreToGameSceneFromPlayer(playerData, zombieItem, gameSceneId, position, direction);
		}
		this.zombieDrops.Remove(zombie);
		this.Cache.Remove(zombie.UniqueId.Guid);
		ZombieWgoData zombieWgoData = zombie.CreateCommonZombieFromThis(position, gameSceneId, direction);
		MainGame.WorldData.AddWgoData(zombieWgoData);
		this.Cache.Add(zombieWgoData.UniqueId.Guid, zombieWgoData);
		this.zombieOnSceneWgoIds.Add(zombieWgoData.UniqueId);
		playerData.RemoveOverheadItem(zombieItem);
		return zombieWgoData;
	}

	// Token: 0x0600203E RID: 8254 RVA: 0x00098E60 File Offset: 0x00097060
	private static Item ResolveOverheadZombieItem(PlayerData playerData, Item zombieItem = null)
	{
		if (zombieItem != null && !zombieItem.IsEmpty)
		{
			return zombieItem;
		}
		if (playerData != null)
		{
			Item item2;
			if (playerData.TryGetOverheadItem((Item item) => item.Definition.itemGroupIds.Contains("zombie"), out item2))
			{
				return item2;
			}
		}
		if (playerData == null)
		{
			return null;
		}
		return playerData.overheadItem;
	}

	// Token: 0x0600203F RID: 8255 RVA: 0x00098EB4 File Offset: 0x000970B4
	public ZombieWgoData GetZombie(SGuid uniqueId)
	{
		ZombieWgoData zombieWgoData;
		this.Cache.TryGetValue(uniqueId.Guid, out zombieWgoData);
		return zombieWgoData;
	}

	// Token: 0x06002040 RID: 8256 RVA: 0x00098ED8 File Offset: 0x000970D8
	public void RemoveZombie(SGuid uniqueId)
	{
		for (int i = 0; i < this.zombieDrops.Count; i++)
		{
			if (this.zombieDrops[i].UniqueId == uniqueId)
			{
				this.zombieDrops.RemoveAt(i);
				break;
			}
		}
		this.zombieOnSceneWgoIds.Remove(uniqueId);
		this.Cache.Remove(uniqueId.Guid);
		if (MainGame.WorldData.GetWgoData(uniqueId) != null)
		{
			MainGame.WorldData.RemoveWgoDataFromGameScene(uniqueId);
		}
	}

	// Token: 0x04001CD1 RID: 7377
	public const string DEFAULT_SKIN_ID = "zmb_01_00_worker";

	// Token: 0x04001CD2 RID: 7378
	public List<ZombieWgoData> zombieDrops = new List<ZombieWgoData>();

	// Token: 0x04001CD3 RID: 7379
	public List<SGuid> zombieOnSceneWgoIds = new List<SGuid>();

	// Token: 0x04001CD4 RID: 7380
	private Dictionary<Guid, ZombieWgoData> cache;
}
