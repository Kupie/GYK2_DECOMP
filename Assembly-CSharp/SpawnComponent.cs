using System;
using UnityEngine;

// Token: 0x020003BB RID: 955
[DisallowMultipleComponent]
[RequireComponent(typeof(Wgo))]
public class SpawnComponent : MonoBehaviour
{
	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x060019AB RID: 6571 RVA: 0x000791ED File Offset: 0x000773ED
	// (set) Token: 0x060019AC RID: 6572 RVA: 0x000791FF File Offset: 0x000773FF
	public SpawnConfiguration SpawnConfiguration
	{
		get
		{
			return this.Wgo.MainWgoPart.SpawnConfiguration;
		}
		set
		{
			this.Wgo.MainWgoPart.SpawnConfiguration = value;
		}
	}

	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x060019AD RID: 6573 RVA: 0x00079212 File Offset: 0x00077412
	public Wgo Wgo
	{
		get
		{
			if (this.wgo == null)
			{
				this.wgo = base.GetComponent<Wgo>();
			}
			return this.wgo;
		}
	}

	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x060019AE RID: 6574 RVA: 0x00079234 File Offset: 0x00077434
	public SpawnWgoComponent SpawnWgoComponent
	{
		get
		{
			return this.Wgo.Data.SpawnWGOComponent;
		}
	}

	// Token: 0x040018F6 RID: 6390
	private Wgo wgo;
}
