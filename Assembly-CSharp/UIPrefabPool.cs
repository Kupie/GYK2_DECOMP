using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000862 RID: 2146
[Serializable]
public class UIPrefabPool : MonoBehaviour
{
	// Token: 0x17000827 RID: 2087
	// (get) Token: 0x060036DF RID: 14047 RVA: 0x00109AA9 File Offset: 0x00107CA9
	public MonoBehaviour Prefab
	{
		get
		{
			return this.prefab;
		}
	}

	// Token: 0x060036E0 RID: 14048 RVA: 0x00109AB1 File Offset: 0x00107CB1
	public void Init()
	{
		this.pool = new Pool(this.prefab, base.transform, this.initialPoolSize, this.poolType, false, null);
	}

	// Token: 0x060036E1 RID: 14049 RVA: 0x00109AD8 File Offset: 0x00107CD8
	public T GetOrCreateObject<T>(Transform newParent) where T : MonoBehaviour
	{
		T orCreateObject = this.pool.GetOrCreateObject<T>();
		orCreateObject.transform.SetParent(newParent);
		return orCreateObject;
	}

	// Token: 0x060036E2 RID: 14050 RVA: 0x00109AF6 File Offset: 0x00107CF6
	public void ReleaseObject<T>(T element) where T : MonoBehaviour
	{
		this.pool.ReleaseObject<T>(element);
		element.transform.SetParent(base.transform);
	}

	// Token: 0x04002BCC RID: 11212
	[SerializeField]
	private MonoBehaviour prefab;

	// Token: 0x04002BCD RID: 11213
	[SerializeField]
	private int initialPoolSize = 10;

	// Token: 0x04002BCE RID: 11214
	[SerializeField]
	private Pool.PoolType poolType;

	// Token: 0x04002BCF RID: 11215
	private Pool pool;
}
