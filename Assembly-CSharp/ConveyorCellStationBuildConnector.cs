using System;
using UnityEngine;

// Token: 0x02000606 RID: 1542
public class ConveyorCellStationBuildConnector : BuildConnector
{
	// Token: 0x06002981 RID: 10625 RVA: 0x00028294 File Offset: 0x00026494
	public override bool TryConnect(Wgo other)
	{
		return false;
	}

	// Token: 0x0400227C RID: 8828
	[SerializeField]
	private Direction direction;
}
