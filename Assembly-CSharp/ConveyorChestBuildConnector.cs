using System;
using UnityEngine;

// Token: 0x02000608 RID: 1544
public class ConveyorChestBuildConnector : BuildConnector
{
	// Token: 0x06002985 RID: 10629 RVA: 0x000C3B74 File Offset: 0x000C1D74
	public override bool TryConnect(Wgo other)
	{
		Vector3 vector = base.transform.position - base.GetComponentInParent<Wgo>().transform.position;
		ConveyorElementType conveyorType = other.Data.Definition.conveyorType;
		float num;
		if (conveyorType != ConveyorElementType.Cell)
		{
			if (conveyorType == ConveyorElementType.Splitter)
			{
				foreach (BuildConnector buildConnector in other.gameObject.GetComponentsInChildren<BuildConnector>())
				{
					if (buildConnector.gameObject.activeSelf && buildConnector.ConnectionType == ConveyorConnectionType.In)
					{
						num = Mathf.Abs(Vector3.Angle(buildConnector.transform.position - other.transform.position, vector));
						if (num.EqualsTo(180f, 1E-05f) || num.EqualsTo(0f, 1E-05f))
						{
							base.Parent.Connect(other.Data as ConveyorWgoData, ConveyorConnectionType.Out, this.direction, this.slotIndex);
						}
					}
				}
				return false;
			}
			if (conveyorType != ConveyorElementType.UndergroundCell)
			{
				return false;
			}
		}
		num = Mathf.Abs(Vector3.Angle(other.gameObject.GetComponentInChildren<BuildConnector>().transform.position - other.transform.position, vector));
		if (num < 180f)
		{
			base.Parent.Connect(other.Data as ConveyorWgoData, ConveyorConnectionType.Out, this.direction, this.slotIndex);
		}
		return false;
	}

	// Token: 0x0400227E RID: 8830
	[SerializeField]
	private Direction direction;

	// Token: 0x0400227F RID: 8831
	[SerializeField]
	private int slotIndex = -1;
}
