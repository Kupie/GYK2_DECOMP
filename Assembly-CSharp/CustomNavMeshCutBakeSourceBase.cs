using System;
using UnityEngine;

// Token: 0x02000A8A RID: 2698
public abstract class CustomNavMeshCutBakeSourceBase : MonoBehaviour, ICustomNavMeshCut
{
	// Token: 0x17000B22 RID: 2850
	// (get) Token: 0x06004967 RID: 18791 RVA: 0x0015AD60 File Offset: 0x00158F60
	public bool Bakable
	{
		get
		{
			return this.bakable;
		}
	}

	// Token: 0x17000B23 RID: 2851
	// (get) Token: 0x06004968 RID: 18792 RVA: 0x0015AD68 File Offset: 0x00158F68
	public Transform BakeTransform
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x06004969 RID: 18793 RVA: 0x0015AD70 File Offset: 0x00158F70
	protected virtual void Awake()
	{
		if (this.bakable && Application.isPlaying)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600496A RID: 18794 RVA: 0x0015AD8D File Offset: 0x00158F8D
	public virtual void OnCustomNavMeshCutSpawn(WgoData wgoData, WgoPartData wgoPartData)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x04003940 RID: 14656
	[SerializeField]
	private bool bakable;
}
