using System;

// Token: 0x020005E1 RID: 1505
public interface IOrderExecutor
{
	// Token: 0x060027CF RID: 10191
	bool CanAddItem(Item item);

	// Token: 0x060027D0 RID: 10192
	bool HasItem(Item item);

	// Token: 0x060027D1 RID: 10193
	void AddItem(Item item);

	// Token: 0x060027D2 RID: 10194
	void RemoveItem(Item item);
}
