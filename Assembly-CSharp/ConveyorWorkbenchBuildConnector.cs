using System;
using UnityEngine;

// Token: 0x0200060B RID: 1547
public class ConveyorWorkbenchBuildConnector : BuildConnector
{
	// Token: 0x0600298B RID: 10635 RVA: 0x000C3EF8 File Offset: 0x000C20F8
	public override bool TryConnect(Wgo other)
	{
		ConveyorElementType conveyorType = other.Data.Definition.conveyorType;
		if (conveyorType != ConveyorElementType.Cell)
		{
			if (conveyorType == ConveyorElementType.Splitter)
			{
				base.Parent.Connect(other.Data as ConveyorWgoData, base.ConnectionType, this.direction);
				return false;
			}
			if (conveyorType != ConveyorElementType.UndergroundCell)
			{
				return false;
			}
		}
		base.Parent.Connect(other.Data as ConveyorWgoData, base.ConnectionType, this.direction);
		return false;
	}

	// Token: 0x04002281 RID: 8833
	[SerializeField]
	private Direction direction;
}
