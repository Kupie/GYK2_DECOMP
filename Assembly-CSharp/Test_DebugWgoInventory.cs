using System;
using UnityEngine;

// Token: 0x020007BC RID: 1980
public class Test_DebugWgoInventory : MonoBehaviour
{
	// Token: 0x060032F2 RID: 13042 RVA: 0x000F5A9F File Offset: 0x000F3C9F
	private void Start()
	{
		this.wgo = base.GetComponentInParent<Wgo>();
	}

	// Token: 0x060032F3 RID: 13043 RVA: 0x000F5AAD File Offset: 0x000F3CAD
	private void AddTestItem()
	{
		this.wgo.Data.Inventory.AddItemToInventory(new Item(this.itemId, 1), null, false);
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x000F5AD3 File Offset: 0x000F3CD3
	private void AddTestGameRes()
	{
		this.wgo.Data.AddGameRes(this.gameResId, 1);
	}

	// Token: 0x040028C2 RID: 10434
	public string gameResId;

	// Token: 0x040028C3 RID: 10435
	public string itemId;

	// Token: 0x040028C4 RID: 10436
	private Wgo wgo;
}
