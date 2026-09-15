using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000A79 RID: 2681
[Serializable]
public class VendorOrderData : ObjectLinkedToDefinition<VendorOrderDef>
{
	// Token: 0x17000AFB RID: 2811
	// (get) Token: 0x060048C9 RID: 18633 RVA: 0x00158B9B File Offset: 0x00156D9B
	public SGuid Guid
	{
		get
		{
			return this.guid;
		}
	}

	// Token: 0x17000AFC RID: 2812
	// (get) Token: 0x060048CA RID: 18634 RVA: 0x00158BA3 File Offset: 0x00156DA3
	// (set) Token: 0x060048CB RID: 18635 RVA: 0x00158BAB File Offset: 0x00156DAB
	public int Count
	{
		get
		{
			return this.count;
		}
		set
		{
			this.count = value;
		}
	}

	// Token: 0x17000AFD RID: 2813
	// (get) Token: 0x060048CC RID: 18636 RVA: 0x00158BB4 File Offset: 0x00156DB4
	public bool IsFinishedOnce
	{
		get
		{
			return this.isFinishedOnce;
		}
	}

	// Token: 0x060048CD RID: 18637 RVA: 0x00158BBC File Offset: 0x00156DBC
	public VendorOrderData(string id)
		: base(id)
	{
		this.guid = new SGuid();
	}

	// Token: 0x17000AFE RID: 2814
	// (get) Token: 0x060048CE RID: 18638 RVA: 0x00158BD0 File Offset: 0x00156DD0
	// (set) Token: 0x060048CF RID: 18639 RVA: 0x00158BD8 File Offset: 0x00156DD8
	public VendorOrderState State
	{
		get
		{
			return this.state;
		}
		set
		{
			this.state = value;
			if (this.state == VendorOrderState.Finished)
			{
				this.isFinishedOnce = true;
			}
		}
	}

	// Token: 0x17000AFF RID: 2815
	// (get) Token: 0x060048D0 RID: 18640 RVA: 0x00158BF1 File Offset: 0x00156DF1
	// (set) Token: 0x060048D1 RID: 18641 RVA: 0x00158BF9 File Offset: 0x00156DF9
	public int Tier
	{
		get
		{
			return this.tier;
		}
		set
		{
			this.tier = value;
		}
	}

	// Token: 0x040038C4 RID: 14532
	[SerializeField]
	private VendorOrderState state;

	// Token: 0x040038C5 RID: 14533
	[SerializeField]
	private int tier;

	// Token: 0x040038C6 RID: 14534
	[SerializeField]
	private SGuid guid;

	// Token: 0x040038C7 RID: 14535
	[SerializeField]
	private int count;

	// Token: 0x040038C8 RID: 14536
	[SerializeField]
	private bool isFinishedOnce;
}
