using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000A9A RID: 2714
public class ConstructorPart : MonoBehaviour, IChunkableObject
{
	// Token: 0x17000B24 RID: 2852
	// (get) Token: 0x06004991 RID: 18833 RVA: 0x0015B942 File Offset: 0x00159B42
	public bool HasChildPath
	{
		get
		{
			return this.constructorPartChildData != null && !string.IsNullOrEmpty(this.constructorPartChildData.pathToObject);
		}
	}

	// Token: 0x06004992 RID: 18834 RVA: 0x0015B964 File Offset: 0x00159B64
	public void SetLutTexture(Texture2D texture)
	{
		this.constructorPartChildData.lut = texture;
		Object3D componentInChildren;
		if (!base.TryGetComponent<Object3D>(out componentInChildren))
		{
			componentInChildren = base.GetComponentInChildren<Object3D>();
			if (componentInChildren == null)
			{
				return;
			}
		}
		foreach (Object3DMesh object3DMesh in componentInChildren.Object3DMeshes)
		{
			object3DMesh.SetLutTexture(texture);
		}
	}

	// Token: 0x06004993 RID: 18835 RVA: 0x0015B9DC File Offset: 0x00159BDC
	public BurstableBounds GetChunkableData()
	{
		return this.constructorPartChildData.chunkBounds.GetBounds();
	}

	// Token: 0x17000B25 RID: 2853
	// (get) Token: 0x06004994 RID: 18836 RVA: 0x0015B9EE File Offset: 0x00159BEE
	// (set) Token: 0x06004995 RID: 18837 RVA: 0x0015B9F6 File Offset: 0x00159BF6
	public MultiFlagOR<ChunkingIgnoreType> IgnoreMultiFlag { get; set; }

	// Token: 0x17000B26 RID: 2854
	// (get) Token: 0x06004996 RID: 18838 RVA: 0x00028294 File Offset: 0x00026494
	public bool IgnoreChunkVisibility
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004997 RID: 18839 RVA: 0x000337EF File Offset: 0x000319EF
	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		this.DrawChunkGizmos();
	}

	// Token: 0x06004998 RID: 18840 RVA: 0x0015BA00 File Offset: 0x00159C00
	public void UpdateChunkVisibility(bool isVisible)
	{
		if (this.isVisible == isVisible)
		{
			return;
		}
		this.isVisible = isVisible;
		if (isVisible)
		{
			this.constructorPartChildData.view = ConstructorPartPool.Get(this.constructorPartChildData.pathToObject);
			if (this.constructorPartChildData.view == null)
			{
				this.isVisible = false;
				return;
			}
			this.constructorPartChildData.view.transform.SetParent(base.transform);
			this.constructorPartChildData.view.transform.localScale = this.constructorPartChildData.localScale;
			this.constructorPartChildData.view.transform.rotation = this.constructorPartChildData.rotation;
			this.constructorPartChildData.view.transform.localPosition = this.constructorPartChildData.localPosition;
			if (!(this.constructorPartChildData.lut != null))
			{
				return;
			}
			Object3D componentInChildren;
			if (!base.TryGetComponent<Object3D>(out componentInChildren))
			{
				componentInChildren = base.GetComponentInChildren<Object3D>();
				if (componentInChildren == null)
				{
					return;
				}
			}
			using (List<Object3DMesh>.Enumerator enumerator = componentInChildren.Object3DMeshes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Object3DMesh object3DMesh = enumerator.Current;
					object3DMesh.SetLutTexture(this.constructorPartChildData.lut);
				}
				return;
			}
		}
		if (this.constructorPartChildData.view != null)
		{
			ConstructorPartPool.Release(this.constructorPartChildData.pathToObject, this.constructorPartChildData.view);
			this.constructorPartChildData.view = null;
		}
	}

	// Token: 0x04003965 RID: 14693
	[HideInInspector]
	public ConstructorPartChildData constructorPartChildData = new ConstructorPartChildData();

	// Token: 0x04003966 RID: 14694
	private bool isVisible;
}
