using System;
using UnityEngine;

// Token: 0x020005E4 RID: 1508
public class PlayerOrderExecutor : IOrderExecutor
{
	// Token: 0x060027DF RID: 10207 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public bool CanAddItem(Item item)
	{
		return true;
	}

	// Token: 0x060027E0 RID: 10208 RVA: 0x000BA0E4 File Offset: 0x000B82E4
	public bool HasItem(Item item)
	{
		if (item.Definition.itemSize == ItemSize.Big)
		{
			Item item2;
			return MainGame.PlayerData.TryGetOverheadItem((Item overhead) => overhead.id == item.id && overhead.Count >= item.Count, out item2) || this.multiInventory.HasItemQuantity(item.id, item.Count);
		}
		return this.multiInventory.HasItemQuantity(item.id, item.Count);
	}

	// Token: 0x060027E1 RID: 10209 RVA: 0x000BA170 File Offset: 0x000B8370
	public void AddItem(Item item)
	{
		PlayerData playerData = MainGame.PlayerData;
		MainGame.Instance.dropSystem.DropItem(new Item(item.id, item.Count), MainGame.PlayerData.currentGameSceneId, playerData.position.Value + new Vector3(playerData.Direction.x, 0f, playerData.Direction.y), null);
	}

	// Token: 0x060027E2 RID: 10210 RVA: 0x000BA1E0 File Offset: 0x000B83E0
	public void RemoveItem(Item item)
	{
		if (item.Definition.itemSize != ItemSize.Big)
		{
			this.multiInventory.RemoveItem(new Item(item.id, item.Count));
			return;
		}
		Item item2;
		if (MainGame.PlayerData.TryGetOverheadItem((Item overhead) => overhead.id == item.id && overhead.Count >= item.Count, out item2))
		{
			MainGame.PlayerData.RemoveOverheadItem(item2);
			return;
		}
		this.multiInventory.RemoveItem(new Item(item.id, item.Count));
	}

	// Token: 0x040021B7 RID: 8631
	private MultiInventory multiInventory = new MultiInventory(MainGame.PlayerData, true);
}
