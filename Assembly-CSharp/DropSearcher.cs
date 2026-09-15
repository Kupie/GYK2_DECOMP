using System;
using UnityEngine;

// Token: 0x02000372 RID: 882
public class DropSearcher : MonoBehaviour
{
	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x06001768 RID: 5992 RVA: 0x0006F32B File Offset: 0x0006D52B
	public Transform CollectorObjectTransform
	{
		get
		{
			return this.collectorObjectTransform;
		}
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x0006F334 File Offset: 0x0006D534
	public void OnTriggerEnter(Collider other)
	{
		TriggerColliderComponentLinker triggerColliderComponentLinker;
		if (other.TryGetComponent<TriggerColliderComponentLinker>(out triggerColliderComponentLinker))
		{
			this.TryFindAndMoveDropToCollector(triggerColliderComponentLinker);
		}
	}

	// Token: 0x0600176A RID: 5994 RVA: 0x0006F354 File Offset: 0x0006D554
	public void OnTriggerStay(Collider other)
	{
		TriggerColliderComponentLinker triggerColliderComponentLinker;
		if (other.TryGetComponent<TriggerColliderComponentLinker>(out triggerColliderComponentLinker))
		{
			this.TryFindAndMoveDropToCollector(triggerColliderComponentLinker);
		}
	}

	// Token: 0x0600176B RID: 5995 RVA: 0x0006F374 File Offset: 0x0006D574
	private void TryFindAndMoveDropToCollector(TriggerColliderComponentLinker linker)
	{
		DropView dropView = linker.Component as DropView;
		if (!DropCollector.CanCollectDrop(dropView))
		{
			return;
		}
		if (dropView.Data.IsResDrop || MainGame.PlayerData.inventory.CanAddItemToInventory(dropView.Data.Item.id, 1))
		{
			dropView.TryMoveToCollector(this.collectorObjectTransform);
		}
	}

	// Token: 0x0400174C RID: 5964
	[SerializeField]
	private Transform collectorObjectTransform;
}
