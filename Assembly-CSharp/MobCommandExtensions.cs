using System;
using UnityEngine;

// Token: 0x02000ACA RID: 2762
public static class MobCommandExtensions
{
	// Token: 0x06004AA0 RID: 19104 RVA: 0x0016048B File Offset: 0x0015E68B
	public static MobCommand ToTarget(this MobCommand mobCommand, ICombatEntity entity)
	{
		mobCommand.TargetEntity = entity;
		return mobCommand;
	}

	// Token: 0x06004AA1 RID: 19105 RVA: 0x00160495 File Offset: 0x0015E695
	public static MobCommand ToPosition(this MobCommand mobCommand, Vector3 position)
	{
		mobCommand.customPosition = position;
		return mobCommand;
	}
}
