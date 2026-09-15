using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054B RID: 1355
[ExecuteAlways]
public class GDPoint : MonoBehaviour
{
	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x060022CF RID: 8911 RVA: 0x000A30B7 File Offset: 0x000A12B7
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x060022D0 RID: 8912 RVA: 0x000A30BF File Offset: 0x000A12BF
	public string CustomTag
	{
		get
		{
			return this.customTag;
		}
	}

	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x060022D1 RID: 8913 RVA: 0x000A30C7 File Offset: 0x000A12C7
	public Direction Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x060022D2 RID: 8914 RVA: 0x000A30CF File Offset: 0x000A12CF
	public bool IsTransitPoint
	{
		get
		{
			return this.isTransitPoint;
		}
	}

	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x060022D3 RID: 8915 RVA: 0x000A30D7 File Offset: 0x000A12D7
	public List<GDPoint> NextGdPoints
	{
		get
		{
			return this.nextGdPoints;
		}
	}

	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x060022D4 RID: 8916 RVA: 0x000A30DF File Offset: 0x000A12DF
	public string TransitToGdPointId
	{
		get
		{
			return this.transitToGdPointId;
		}
	}

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x060022D5 RID: 8917 RVA: 0x000A30E7 File Offset: 0x000A12E7
	public string WorldIdToTransit
	{
		get
		{
			return this.worldIdToTransit;
		}
	}

	// Token: 0x060022D6 RID: 8918 RVA: 0x000A30EF File Offset: 0x000A12EF
	public void Init(GDPointData gdPointData)
	{
		this.gdPointData = gdPointData;
		this.gdPointData.OnActiveStateChanged += this.OnActiveStateChanged;
		this.OnActiveStateChanged(gdPointData.Enabled);
	}

	// Token: 0x060022D7 RID: 8919 RVA: 0x000A311C File Offset: 0x000A131C
	public void OnActiveStateChanged(bool state)
	{
		if (this == null)
		{
			Debug.LogWarning("GDPoint.OnActiveStateChanged called but 'this' is null");
			return;
		}
		base.gameObject.SetActive(state);
		if (state && !base.gameObject.activeInHierarchy)
		{
			string text = "GDPoint [";
			string text2 = this.id;
			string text3 = "] SetActive(true) called but activeInHierarchy is still false. Parent object is inactive: ";
			Transform parent = base.transform.parent;
			Debug.LogWarning(text + text2 + text3 + ((parent != null) ? parent.name : null));
		}
	}

	// Token: 0x060022D8 RID: 8920 RVA: 0x000A318A File Offset: 0x000A138A
	public void OnDestroy()
	{
		if (this.gdPointData != null && Application.isPlaying)
		{
			this.gdPointData.OnActiveStateChanged -= this.OnActiveStateChanged;
		}
	}

	// Token: 0x04001F6C RID: 8044
	[SerializeField]
	private string id;

	// Token: 0x04001F6D RID: 8045
	[SerializeField]
	private string customTag;

	// Token: 0x04001F6E RID: 8046
	[SerializeField]
	private Direction direction = Direction.Down;

	// Token: 0x04001F6F RID: 8047
	[SerializeReference]
	private List<GDPoint> nextGdPoints = new List<GDPoint>();

	// Token: 0x04001F70 RID: 8048
	[SerializeField]
	private bool isTransitPoint;

	// Token: 0x04001F71 RID: 8049
	[SerializeField]
	private string transitToGdPointId;

	// Token: 0x04001F72 RID: 8050
	[SerializeField]
	private string worldIdToTransit;

	// Token: 0x04001F73 RID: 8051
	private GDPointData gdPointData;
}
