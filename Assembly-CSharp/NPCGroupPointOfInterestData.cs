using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020001B0 RID: 432
[Serializable]
public class NPCGroupPointOfInterestData
{
	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x00036C10 File Offset: 0x00034E10
	public List<SGuid> Wgos
	{
		get
		{
			return this.wgos;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x00036C18 File Offset: 0x00034E18
	public string Id
	{
		get
		{
			return this.groupId;
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x00036C20 File Offset: 0x00034E20
	public NPCGroupPointOfInterestConfiguration Configuration
	{
		get
		{
			return LazySingletonSO<NPCLifeSimulatorConfiguration>.Instance.GetGroupById(this.groupId);
		}
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x00036C32 File Offset: 0x00034E32
	public NPCGroupPointOfInterestData(NPCGroupPointOfInterestConfiguration configuration)
	{
		this.groupId = configuration.Id;
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x00036C51 File Offset: 0x00034E51
	public void AddWgoToGroup(WgoData wgoData)
	{
		this.wgos.Add(wgoData.UniqueId);
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x00036C64 File Offset: 0x00034E64
	public void RemoveWgoFromGroup(WgoData wgoData)
	{
		this.wgos.Remove(wgoData.UniqueId);
	}

	// Token: 0x04000C54 RID: 3156
	[SerializeField]
	private string groupId;

	// Token: 0x04000C55 RID: 3157
	[SerializeField]
	private List<SGuid> wgos = new List<SGuid>();
}
