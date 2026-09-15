using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// Token: 0x02000A8F RID: 2703
[DefaultExecutionOrder(1)]
[RequireComponent(typeof(NavmeshCut))]
[ExecuteAlways]
public class NavMeshCutPlannerMesh : CustomNavMeshCutBakeSourceBase
{
	// Token: 0x06004972 RID: 18802 RVA: 0x0015AF9C File Offset: 0x0015919C
	private new void Awake()
	{
		base.Awake();
		this.TryResolveReferences();
		this.SyncNavMeshCut(int.MinValue);
	}

	// Token: 0x06004973 RID: 18803 RVA: 0x0015AFB5 File Offset: 0x001591B5
	private void OnEnable()
	{
		this.TryResolveReferences();
		this.TrySubscribe();
		this.SyncNavMeshCut(int.MinValue);
	}

	// Token: 0x06004974 RID: 18804 RVA: 0x0015AFCE File Offset: 0x001591CE
	private void OnDisable()
	{
		this.TryUnsubscribe();
	}

	// Token: 0x06004975 RID: 18805 RVA: 0x0015AFD6 File Offset: 0x001591D6
	private void Update()
	{
		if (this.subscribedData == null)
		{
			this.TryResolveReferences();
			this.TrySubscribe();
		}
	}

	// Token: 0x06004976 RID: 18806 RVA: 0x0015AFEC File Offset: 0x001591EC
	private void TryResolveReferences()
	{
		if (this.navmeshCut == null)
		{
			base.TryGetComponent<NavmeshCut>(out this.navmeshCut);
		}
		if (this.wgoPart == null)
		{
			this.wgoPart = base.GetComponentInParent<WgoPart>(true);
		}
	}

	// Token: 0x06004977 RID: 18807 RVA: 0x0015B024 File Offset: 0x00159224
	private void TrySubscribe()
	{
		WgoPartData wgoPartData;
		if ((wgoPartData = this.subscribedData) == null)
		{
			WgoPart wgoPart = this.wgoPart;
			wgoPartData = ((wgoPart != null) ? wgoPart.WgoPartData : null);
		}
		WgoPartData wgoPartData2 = wgoPartData;
		if (wgoPartData2 == null)
		{
			return;
		}
		if (this.subscribedData == wgoPartData2)
		{
			return;
		}
		this.TryUnsubscribe();
		this.subscribedData = wgoPartData2;
		this.subscribedData.OnStateChange += this.HandleWgoPartStateChanged;
	}

	// Token: 0x06004978 RID: 18808 RVA: 0x0015B080 File Offset: 0x00159280
	private void TryUnsubscribe()
	{
		if (this.subscribedData == null)
		{
			return;
		}
		this.subscribedData.OnStateChange -= this.HandleWgoPartStateChanged;
		this.subscribedData = null;
	}

	// Token: 0x06004979 RID: 18809 RVA: 0x0015B0A9 File Offset: 0x001592A9
	private void HandleWgoPartStateChanged(string variationId, int rotationIndex)
	{
		this.SyncNavMeshCut(WgoPartData.GetStateHash(variationId, rotationIndex));
	}

	// Token: 0x0600497A RID: 18810 RVA: 0x0015B0B8 File Offset: 0x001592B8
	private void SyncNavMeshCut(int stateHash = -2147483648)
	{
		if (this.navmeshCut == null)
		{
			return;
		}
		this.navmeshCut.type = NavmeshCut.MeshType.CustomMesh;
		this.navmeshCut.center = Vector3.zero;
		this.navmeshCut.meshScale = 1f;
		this.navmeshCut.enabled = !this.dontUseCut;
		if (!this.navmeshCut.enabled)
		{
			this.navmeshCut.mesh = null;
			return;
		}
		WgoPartData wgoPartData;
		if ((wgoPartData = this.subscribedData) == null)
		{
			WgoPart wgoPart = this.wgoPart;
			wgoPartData = ((wgoPart != null) ? wgoPart.WgoPartData : null);
		}
		WgoPartData wgoPartData2 = wgoPartData;
		if (((wgoPartData2 != null) ? wgoPartData2.BakedData : null) == null)
		{
			this.navmeshCut.mesh = null;
			return;
		}
		int num = ((stateHash == int.MinValue) ? wgoPartData2.GetStateHash() : stateHash);
		WgoPartBakedData.PlannerMeshData plannerMeshData;
		if (!wgoPartData2.BakedData.TryGetPlannerMeshData(num, out plannerMeshData))
		{
			this.navmeshCut.mesh = null;
			return;
		}
		Mesh mesh;
		if (!this.cachedMeshesByHash.TryGetValue(num, out mesh) || mesh == null)
		{
			WgoPartBakedData.PlannerMeshData plannerMeshData2 = plannerMeshData;
			string text = "PlannerMesh_{0}_{1}";
			WgoPart wgoPart2 = this.wgoPart;
			if (!WgoPartBakedData.TryCreateMesh(plannerMeshData2, string.Format(text, ((wgoPart2 != null) ? wgoPart2.Id : null) ?? base.gameObject.name, num), out mesh))
			{
				this.navmeshCut.mesh = null;
				return;
			}
			this.cachedMeshesByHash[num] = mesh;
		}
		this.navmeshCut.mesh = mesh;
	}

	// Token: 0x0600497B RID: 18811 RVA: 0x0015B20F File Offset: 0x0015940F
	public override void OnCustomNavMeshCutSpawn(WgoData wgoData, WgoPartData wgoPartData)
	{
		base.OnCustomNavMeshCutSpawn(wgoData, wgoPartData);
		this.subscribedData = wgoPartData;
		this.TryResolveReferences();
		this.TrySubscribe();
		this.SyncNavMeshCut(int.MinValue);
	}

	// Token: 0x0400394C RID: 14668
	[SerializeField]
	private NavmeshCut navmeshCut;

	// Token: 0x0400394D RID: 14669
	[SerializeField]
	private WgoPart wgoPart;

	// Token: 0x0400394E RID: 14670
	public bool dontUseCut;

	// Token: 0x0400394F RID: 14671
	private readonly Dictionary<int, Mesh> cachedMeshesByHash = new Dictionary<int, Mesh>();

	// Token: 0x04003950 RID: 14672
	private WgoPartData subscribedData;
}
