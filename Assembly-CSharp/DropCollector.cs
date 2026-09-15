using System;
using UnityEngine;

// Token: 0x02000371 RID: 881
public class DropCollector : MonoBehaviour
{
	// Token: 0x06001762 RID: 5986 RVA: 0x0006F248 File Offset: 0x0006D448
	public void OnTriggerEnter(Collider other)
	{
		TriggerColliderComponentLinker triggerColliderComponentLinker;
		if (other.TryGetComponent<TriggerColliderComponentLinker>(out triggerColliderComponentLinker))
		{
			DropView dropView = triggerColliderComponentLinker.Component as DropView;
			this.TryCollectDrop(dropView);
		}
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x0006F274 File Offset: 0x0006D474
	public void OnTriggerStay(Collider other)
	{
		TriggerColliderComponentLinker triggerColliderComponentLinker;
		if (other.TryGetComponent<TriggerColliderComponentLinker>(out triggerColliderComponentLinker))
		{
			DropView dropView = triggerColliderComponentLinker.Component as DropView;
			this.TryCollectDrop(dropView);
		}
	}

	// Token: 0x06001764 RID: 5988 RVA: 0x0006F2A0 File Offset: 0x0006D4A0
	public static bool CanCollectDrop(DropView dropView)
	{
		return !(dropView == null) && dropView.Data != null && !dropView.IsDespawning && !dropView.IsCollectDelayed && !dropView.IsTimedCollecting && !dropView.Data.IsRemoving && dropView.Data.DropType != DropType.WgoData && dropView.Data.Size != ItemSize.Big;
	}

	// Token: 0x06001765 RID: 5989 RVA: 0x0006F30C File Offset: 0x0006D50C
	private void TryCollectDrop(DropView dropView)
	{
		if (!DropCollector.CanCollectDrop(dropView))
		{
			return;
		}
		this.CollectDrop(dropView);
	}

	// Token: 0x06001766 RID: 5990 RVA: 0x0006F31E File Offset: 0x0006D51E
	private void CollectDrop(DropView dropView)
	{
		MainGame.PlayerData.CollectDrop(dropView);
	}
}
