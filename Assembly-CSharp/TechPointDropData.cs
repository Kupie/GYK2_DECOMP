using System;
using UnityEngine;

// Token: 0x020001A3 RID: 419
[Serializable]
public class TechPointDropData
{
	// Token: 0x06000AA5 RID: 2725 RVA: 0x00021B94 File Offset: 0x0001FD94
	public TechPointDropData()
	{
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x00035D9F File Offset: 0x00033F9F
	public TechPointDropData(Vector3 pos, TechPointsSpawner.Type type, string worldId)
	{
		this.pos = pos;
		this.type = type;
		this.worldId = worldId;
	}

	// Token: 0x04000C17 RID: 3095
	public Vector3 pos;

	// Token: 0x04000C18 RID: 3096
	public TechPointsSpawner.Type type;

	// Token: 0x04000C19 RID: 3097
	public string worldId;
}
