using System;

// Token: 0x020005E8 RID: 1512
public class ZombieConveyorTransporterOrderExecutor : IOrderExecutor
{
	// Token: 0x060027ED RID: 10221 RVA: 0x000BA3C4 File Offset: 0x000B85C4
	public ZombieConveyorTransporterOrderExecutor(ZombieWgoData zombieWgoData)
	{
		this.zombieWgoData = zombieWgoData;
	}

	// Token: 0x060027EE RID: 10222 RVA: 0x000BA3D3 File Offset: 0x000B85D3
	public bool CanAddItem(Item item)
	{
		return this.zombieWgoData.ConveyorTransporterPortableItem.IsEmpty;
	}

	// Token: 0x060027EF RID: 10223 RVA: 0x000BA3E5 File Offset: 0x000B85E5
	public bool HasItem(Item item)
	{
		return this.zombieWgoData.ConveyorTransporterPortableItem != null && !this.zombieWgoData.ConveyorTransporterPortableItem.IsEmpty;
	}

	// Token: 0x060027F0 RID: 10224 RVA: 0x000BA409 File Offset: 0x000B8609
	public void AddItem(Item item)
	{
		this.zombieWgoData.ConveyorTransporterPortableItem = new Item(item.id, item.Count);
	}

	// Token: 0x060027F1 RID: 10225 RVA: 0x000BA428 File Offset: 0x000B8628
	public void RemoveItem(Item item)
	{
		this.zombieWgoData.ConveyorTransporterPortableItem.Count -= item.Count;
		if (this.zombieWgoData.ConveyorTransporterPortableItem.Count < 0)
		{
			this.zombieWgoData.ConveyorTransporterPortableItem = Item.Empty;
			return;
		}
		this.zombieWgoData.ConveyorTransporterPortableItem = this.zombieWgoData.ConveyorTransporterPortableItem;
	}

	// Token: 0x040021BB RID: 8635
	private ZombieWgoData zombieWgoData;
}
