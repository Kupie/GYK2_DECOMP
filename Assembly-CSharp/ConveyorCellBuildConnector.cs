using System;
using UnityEngine;

// Token: 0x02000604 RID: 1540
public class ConveyorCellBuildConnector : BuildConnector
{
	// Token: 0x0600297E RID: 10622 RVA: 0x000C37B0 File Offset: 0x000C19B0
	public override bool TryConnect(Wgo other)
	{
		Vector3 vector = base.transform.position - base.GetComponentInParent<Wgo>().transform.position;
		switch (other.Data.Definition.conveyorType)
		{
		case ConveyorElementType.Cell:
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
		case ConveyorElementType.UndergroundCell:
		{
			float num = Mathf.Abs(Vector3.Angle(other.gameObject.GetComponentInChildren<BuildConnector>().transform.position - other.transform.position, vector));
			if (num < 180f)
			{
				base.Parent.Connect(other.Data as ConveyorWgoData, ConveyorConnectionType.Out, this.direction);
			}
			break;
		}
		case ConveyorElementType.StationCell:
			if (Mathf.Abs(Vector3.Angle(other.gameObject.GetComponentInChildren<BuildConnector>().transform.position - other.transform.position, vector)).Equals(180f))
			{
				base.Parent.Connect(other.Data as ConveyorWgoData, ConveyorConnectionType.Out, this.direction);
			}
			break;
		}
		return false;
	}

	// Token: 0x0400227A RID: 8826
	[SerializeField]
	private Direction direction;
}
