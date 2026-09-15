using System;
using System.Collections.Generic;
using LinqTools;
using UnityEngine;

// Token: 0x0200017C RID: 380
[RequireComponent(typeof(GroundLayoutObjectWithTMP))]
public class DevZone : MonoBehaviour
{
	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06000970 RID: 2416 RVA: 0x000300DF File Offset: 0x0002E2DF
	public List<Wgo> Wgos
	{
		get
		{
			if (this.wgos == null || this.wgos.Count == 0)
			{
				this.wgos = base.GetComponentsInChildren<Wgo>(true).ToList<Wgo>();
			}
			return this.wgos;
		}
	}

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06000971 RID: 2417 RVA: 0x0003010E File Offset: 0x0002E30E
	public List<WorldZone> WorldZones
	{
		get
		{
			if (this.worldZones == null || this.worldZones.Count == 0)
			{
				this.worldZones = base.GetComponentsInChildren<WorldZone>(true).ToList<WorldZone>();
			}
			return this.worldZones;
		}
	}

	// Token: 0x04000B0B RID: 2827
	[SerializeField]
	private List<Wgo> wgos;

	// Token: 0x04000B0C RID: 2828
	private List<WorldZone> worldZones;
}
