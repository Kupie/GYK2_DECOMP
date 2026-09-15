using System;

// Token: 0x02000744 RID: 1860
public abstract class UniqueCommandHolderDataWrapper<T> : UniqueCommandHolder
{
	// Token: 0x06003064 RID: 12388
	public abstract void RegisterData(T data);

	// Token: 0x06003065 RID: 12389
	public abstract void UnregisterData();
}
