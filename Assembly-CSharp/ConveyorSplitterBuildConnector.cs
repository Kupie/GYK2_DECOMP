using System;
using UnityEngine;

// Token: 0x0200060A RID: 1546
public class ConveyorSplitterBuildConnector : BuildConnector
{
	// Token: 0x06002989 RID: 10633 RVA: 0x000C3D50 File Offset: 0x000C1F50
	public override bool TryConnect(Wgo other)
	{
		if (base.ConnectionType != ConveyorConnectionType.Out)
		{
			return false;
		}
		Vector3 vector = base.transform.position - base.GetComponentInParent<Wgo>().transform.position;
		switch (other.Data.Definition.conveyorType)
		{
		case ConveyorElementType.Cell:
		case ConveyorElementType.UndergroundCell:
		{
			float num = Mathf.Abs(Vector3.Angle(other.gameObject.GetComponentInChildren<BuildConnector>().transform.position - other.transform.position, vector));
			if (num < 180f)
			{
				base.Parent.Connect(other.Data as ConveyorWgoData, ConveyorConnectionType.Out, this.direction);
			}
			break;
		}
		case ConveyorElementType.Chest:
			base.Parent.Connect(other.Data as ConveyorWgoData, ConveyorConnectionType.Out, this.direction);
			break;
		case ConveyorElementType.Splitter:
			foreach (BuildConnector buildConnector in other.gameObject.GetComponentsInChildren<BuildConnector>())
			{
				if (buildConnector.gameObject.activeSelf && buildConnector.ConnectionType == ConveyorConnectionType.In)
				{
					float num = Mathf.Abs(Vector3.Angle(buildConnector.transform.position - other.transform.position, vector));
					if (num.EqualsTo(180f, 1E-05f) || num.EqualsTo(0f, 1E-05f))
					{
						base.Parent.Connect(other.Data as ConveyorWgoData, ConveyorConnectionType.Out, this.direction);
					}
				}
			}
			break;
		}
		return false;
	}

	// Token: 0x04002280 RID: 8832
	[SerializeField]
	private Direction direction;
}
