using System;

// Token: 0x02000743 RID: 1859
public abstract class UniqueCommandHolder
{
	// Token: 0x06003062 RID: 12386
	public abstract Command GetCommand();

	// Token: 0x0400272A RID: 10026
	public bool isQueued;
}
