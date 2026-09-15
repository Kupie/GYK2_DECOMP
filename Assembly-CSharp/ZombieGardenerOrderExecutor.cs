using System;

// Token: 0x020005EA RID: 1514
public class ZombieGardenerOrderExecutor : IOrderExecutor
{
	// Token: 0x0600280A RID: 10250 RVA: 0x000BAA5A File Offset: 0x000B8C5A
	public ZombieGardenerOrderExecutor(ZombieWgoData zombieWgoData)
	{
		this.zombieWgoData = zombieWgoData;
	}

	// Token: 0x0600280B RID: 10251 RVA: 0x000BAA69 File Offset: 0x000B8C69
	public bool CanAddItem(Item item)
	{
		return new Inventory(this.zombieWgoData.ZombieItem).CanAddItemToInventory(item);
	}

	// Token: 0x0600280C RID: 10252 RVA: 0x000BAA84 File Offset: 0x000B8C84
	public bool HasItem(Item item)
	{
		Inventory inventory = new Inventory(this.zombieWgoData.ZombieItem);
		MultiInventory multiInventory = new MultiInventory(this.zombieWgoData.WorldZoneData, null, false);
		return inventory.Data.HasItemQuantityInInventory(item.id, item.Count) || multiInventory.HasItemQuantity(item.id, item.Count);
	}

	// Token: 0x0600280D RID: 10253 RVA: 0x000BAAE0 File Offset: 0x000B8CE0
	public void AddItem(Item item)
	{
		new Inventory(this.zombieWgoData.ZombieItem).AddItemToInventory(item, null, false);
	}

	// Token: 0x0600280E RID: 10254 RVA: 0x000BAAFB File Offset: 0x000B8CFB
	public void RemoveItem(Item item)
	{
		new Inventory(this.zombieWgoData.ZombieItem).RemoveItemById(item.id, item.Count, null, null, false);
	}

	// Token: 0x040021C3 RID: 8643
	public ZombieWgoData zombieWgoData;
}
