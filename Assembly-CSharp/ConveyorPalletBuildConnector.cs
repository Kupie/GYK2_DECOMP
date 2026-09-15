using System;

// Token: 0x02000609 RID: 1545
public class ConveyorPalletBuildConnector : BuildConnector
{
	// Token: 0x06002987 RID: 10631 RVA: 0x000C3CF0 File Offset: 0x000C1EF0
	public override bool TryConnect(Wgo other)
	{
		ConveyorElementType conveyorType = other.Data.Definition.conveyorType;
		if (conveyorType != ConveyorElementType.Cell)
		{
			if (conveyorType == ConveyorElementType.Splitter)
			{
				base.Parent.Connect(other.Data as ConveyorWgoData, ConveyorConnectionType.Out, Direction.None);
				return false;
			}
			if (conveyorType != ConveyorElementType.UndergroundCell)
			{
				return false;
			}
		}
		base.Parent.Connect(other.Data as ConveyorWgoData, ConveyorConnectionType.Out, Direction.None);
		return false;
	}
}
