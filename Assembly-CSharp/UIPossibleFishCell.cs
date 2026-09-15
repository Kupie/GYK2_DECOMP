using System;
using UnityEngine;

// Token: 0x020009D7 RID: 2519
public class UIPossibleFishCell : MonoBehaviour
{
	// Token: 0x0600434C RID: 17228 RVA: 0x0013FD30 File Offset: 0x0013DF30
	public void Draw(Item item)
	{
		Debug.Log("UIPossibleFishCell Draw: " + item.id, base.gameObject);
		this.locked.SetActive(false);
		this.inactive.SetActive(false);
		this.itemCell.Draw(item, false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
	}

	// Token: 0x0600434D RID: 17229 RVA: 0x0013FD88 File Offset: 0x0013DF88
	public void DrawInactive(Item item)
	{
		Debug.Log("UIPossibleFishCell DrawInactive: " + item.id, base.gameObject);
		this.locked.SetActive(false);
		this.inactive.SetActive(true);
		this.itemCell.Draw(item, false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
	}

	// Token: 0x0600434E RID: 17230 RVA: 0x0013FDDF File Offset: 0x0013DFDF
	public void DrawLocked()
	{
		Debug.Log("UIPossibleFishCell DrawLocked", base.gameObject);
		this.locked.SetActive(true);
		this.inactive.SetActive(false);
		this.itemCell.DrawEmpty(true, true, false);
	}

	// Token: 0x0600434F RID: 17231 RVA: 0x0013FE17 File Offset: 0x0013E017
	public void DrawLockedInactive()
	{
		Debug.Log("UIPossibleFishCell DrawLockedInactive", base.gameObject);
		this.locked.SetActive(true);
		this.inactive.SetActive(true);
		this.itemCell.DrawEmpty(true, true, false);
	}

	// Token: 0x04003473 RID: 13427
	public UIItemCell itemCell;

	// Token: 0x04003474 RID: 13428
	public GameObject locked;

	// Token: 0x04003475 RID: 13429
	public GameObject inactive;
}
