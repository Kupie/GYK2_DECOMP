using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020001DB RID: 475
[Serializable]
public class BodyDef : BalanceBaseObject
{
	// Token: 0x06000C0A RID: 3082 RVA: 0x0003CAFC File Offset: 0x0003ACFC
	public Item GenerateItem()
	{
		Item item = new Item(this.linkedBodyItemId, 1);
		foreach (string text in this.parts)
		{
			item.AddItemToInventory(new Item(text, 1), false);
		}
		foreach (string text2 in this.pocket)
		{
			item.AddItemToInventory(new Item(text2, 1), false);
		}
		foreach (string text3 in this.burialReward)
		{
			item.AddItemToInventory(new Item(text3, 1), false);
		}
		BodyZombieSkinSerializedItemProperty bodyZombieSkinSerializedItemProperty = new BodyZombieSkinSerializedItemProperty();
		ValueTuple<int, int, string, string> valueTuple = ZombieSkinHelper.RollZombie("zombie_worker");
		bodyZombieSkinSerializedItemProperty.body = valueTuple.Item1;
		bodyZombieSkinSerializedItemProperty.head = valueTuple.Item2;
		bodyZombieSkinSerializedItemProperty.headLut = valueTuple.Item4;
		item.AddProperty<BodyZombieSkinSerializedItemProperty>(bodyZombieSkinSerializedItemProperty);
		item.AddProperty<BodyZombieStartItemsSerializedItemProperty>(new BodyZombieStartItemsSerializedItemProperty
		{
			armorId = this.armorId,
			handsId = this.handsId
		});
		Debug.Log(string.Format("Created body :[{0}] skin body:[{1}] head:[{2}] headLut:[{3}]", new object[] { this.id, bodyZombieSkinSerializedItemProperty.body, bodyZombieSkinSerializedItemProperty.head, bodyZombieSkinSerializedItemProperty.headLut }));
		return item;
	}

	// Token: 0x04000D2E RID: 3374
	[AutoParse("body_item")]
	public string linkedBodyItemId;

	// Token: 0x04000D2F RID: 3375
	[AutoParse("tier")]
	public int tier;

	// Token: 0x04000D30 RID: 3376
	[AutoParse("body_parts")]
	public List<string> parts = new List<string>();

	// Token: 0x04000D31 RID: 3377
	[AutoParse("pockets")]
	public List<string> pocket = new List<string>();

	// Token: 0x04000D32 RID: 3378
	[AutoParse("burial_reward")]
	public List<string> burialReward = new List<string>();

	// Token: 0x04000D33 RID: 3379
	[AutoParse("armor_id")]
	public string armorId;

	// Token: 0x04000D34 RID: 3380
	[AutoParse("hands_id")]
	public string handsId;
}
