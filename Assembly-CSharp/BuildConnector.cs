using System;
using UnityEngine;

// Token: 0x02000602 RID: 1538
public class BuildConnector : MonoBehaviour
{
	// Token: 0x170006A9 RID: 1705
	// (get) Token: 0x06002977 RID: 10615 RVA: 0x000C370F File Offset: 0x000C190F
	public ConveyorConnectionType ConnectionType
	{
		get
		{
			return this.connectionType;
		}
	}

	// Token: 0x170006AA RID: 1706
	// (get) Token: 0x06002978 RID: 10616 RVA: 0x000C3717 File Offset: 0x000C1917
	protected ConveyorComponent Parent
	{
		get
		{
			ConveyorWgoData conveyorWgoData = base.GetComponentInParent<Wgo>().Data as ConveyorWgoData;
			if (conveyorWgoData == null)
			{
				return null;
			}
			return conveyorWgoData.ConveyorComponent;
		}
	}

	// Token: 0x170006AB RID: 1707
	// (get) Token: 0x06002979 RID: 10617 RVA: 0x000C3734 File Offset: 0x000C1934
	public BoxCollider BoxCollider
	{
		get
		{
			if (this.boxCollider == null)
			{
				this.boxCollider = base.GetComponent<BoxCollider>();
			}
			return this.boxCollider;
		}
	}

	// Token: 0x0600297A RID: 10618 RVA: 0x000C3756 File Offset: 0x000C1956
	public void TryDisconnect(ConveyorWgoData conveyorWgoData)
	{
		if (this.Parent == null)
		{
			return;
		}
		this.Parent.Disconnect(conveyorWgoData, this.connectionType);
	}

	// Token: 0x0600297B RID: 10619 RVA: 0x000C3774 File Offset: 0x000C1974
	public virtual bool TryConnect(Wgo conveyorWgo)
	{
		return this.Parent != null && this.Parent.Connect(conveyorWgo.Data as ConveyorWgoData, this.connectionType, Direction.None);
	}

	// Token: 0x04002276 RID: 8822
	[SerializeField]
	private BoxCollider boxCollider;

	// Token: 0x04002277 RID: 8823
	[SerializeField]
	private ConveyorConnectionType connectionType;

	// Token: 0x04002278 RID: 8824
	private ConveyorComponent parent;
}
