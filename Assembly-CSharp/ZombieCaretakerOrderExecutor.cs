using System;

// Token: 0x020005E7 RID: 1511
public class ZombieCaretakerOrderExecutor : IOrderExecutor
{
	// Token: 0x060027E8 RID: 10216 RVA: 0x000BA2FD File Offset: 0x000B84FD
	public ZombieCaretakerOrderExecutor(ZombieWgoData zombieWgoData)
	{
		this.zombieWgoData = zombieWgoData;
	}

	// Token: 0x060027E9 RID: 10217 RVA: 0x000BA30C File Offset: 0x000B850C
	public bool CanAddItem(Item item)
	{
		return this.zombieWgoData.CaretakerPortableItem.IsEmpty;
	}

	// Token: 0x060027EA RID: 10218 RVA: 0x000BA31E File Offset: 0x000B851E
	public bool HasItem(Item item)
	{
		return this.zombieWgoData.CaretakerPortableItem != null && !this.zombieWgoData.CaretakerPortableItem.IsEmpty;
	}

	// Token: 0x060027EB RID: 10219 RVA: 0x000BA342 File Offset: 0x000B8542
	public void AddItem(Item item)
	{
		this.zombieWgoData.CaretakerPortableItem = new Item(item.id, item.Count);
	}

	// Token: 0x060027EC RID: 10220 RVA: 0x000BA360 File Offset: 0x000B8560
	public void RemoveItem(Item item)
	{
		this.zombieWgoData.CaretakerPortableItem.Count -= item.Count;
		if (this.zombieWgoData.CaretakerPortableItem.Count < 0)
		{
			this.zombieWgoData.CaretakerPortableItem = Item.Empty;
			return;
		}
		this.zombieWgoData.CaretakerPortableItem = this.zombieWgoData.CaretakerPortableItem;
	}

	// Token: 0x040021BA RID: 8634
	private ZombieWgoData zombieWgoData;
}
