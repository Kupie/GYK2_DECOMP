using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000863 RID: 2147
public class UIPrefabsPooler : MonoBehaviour, ILazyGUIElement
{
	// Token: 0x17000828 RID: 2088
	// (get) Token: 0x060036E4 RID: 14052 RVA: 0x00109B2A File Offset: 0x00107D2A
	public static UIPrefabsPooler Instance
	{
		get
		{
			return UIPrefabsPooler.instance;
		}
	}

	// Token: 0x060036E5 RID: 14053 RVA: 0x00109B34 File Offset: 0x00107D34
	public void Init()
	{
		UIPrefabsPooler.instance = this;
		this.pools = base.GetComponentsInChildren<UIPrefabPool>().ToList<UIPrefabPool>();
		foreach (UIPrefabPool uiprefabPool in this.pools)
		{
			if (!this.poolsCache.TryAdd(uiprefabPool.Prefab.GetType(), uiprefabPool))
			{
				Debug.LogError(string.Format("Error: {0}: pool type [{1}] is already exists", "UIPrefabPool", uiprefabPool.Prefab.GetType()));
			}
			else
			{
				uiprefabPool.Init();
			}
		}
		this.pools.ForEach(delegate(UIPrefabPool item)
		{
			item.Init();
		});
	}

	// Token: 0x060036E6 RID: 14054 RVA: 0x00109C04 File Offset: 0x00107E04
	public T GetElementFromPool<T>(Transform newParent) where T : MonoBehaviour
	{
		UIPrefabPool uiprefabPool;
		if (this.poolsCache.TryGetValue(typeof(T), out uiprefabPool))
		{
			return uiprefabPool.GetOrCreateObject<T>(newParent);
		}
		Debug.LogError(string.Format("No pool found for type [{0}]", typeof(T)));
		return default(T);
	}

	// Token: 0x060036E7 RID: 14055 RVA: 0x00109C54 File Offset: 0x00107E54
	public void ReleaseElementToPool<T>(T element) where T : MonoBehaviour
	{
		Type type = element.GetType();
		UIPrefabPool uiprefabPool;
		if (this.poolsCache.TryGetValue(type, out uiprefabPool))
		{
			uiprefabPool.ReleaseObject<T>(element);
			element.transform.SetParent(uiprefabPool.transform);
			return;
		}
		Debug.LogError(string.Format("No pool found for type [{0}]", type));
	}

	// Token: 0x04002BD0 RID: 11216
	private static UIPrefabsPooler instance;

	// Token: 0x04002BD1 RID: 11217
	private List<UIPrefabPool> pools = new List<UIPrefabPool>();

	// Token: 0x04002BD2 RID: 11218
	private Dictionary<Type, UIPrefabPool> poolsCache = new Dictionary<Type, UIPrefabPool>();
}
