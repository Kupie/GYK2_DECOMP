using System;

// Token: 0x02000574 RID: 1396
[Serializable]
public class ConveyorConnectionData
{
	// Token: 0x060023CA RID: 9162 RVA: 0x000A7A73 File Offset: 0x000A5C73
	public ConveyorConnectionData(SGuid connectedUniqueId, Direction connectionDirection)
	{
		this.connectedUniqueId = connectedUniqueId;
		this.connectionDirection = connectionDirection;
	}

	// Token: 0x04001FD8 RID: 8152
	public SGuid connectedUniqueId;

	// Token: 0x04001FD9 RID: 8153
	public Direction connectionDirection;
}
