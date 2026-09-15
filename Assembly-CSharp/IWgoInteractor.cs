using System;
using UnityEngine;

// Token: 0x02000674 RID: 1652
public interface IWgoInteractor
{
	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x06002B93 RID: 11155
	Vector3 Position { get; }

	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x06002B94 RID: 11156
	Inventory Inventory { get; }
}
