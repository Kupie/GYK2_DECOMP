using System;
using System.Collections.Generic;
using DG.Tweening;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020002BD RID: 701
public class GroundDecal : MonoBehaviour, IFightDecal
{
	// Token: 0x170002ED RID: 749
	// (get) Token: 0x060011F6 RID: 4598 RVA: 0x00059C81 File Offset: 0x00057E81
	// (set) Token: 0x060011F7 RID: 4599 RVA: 0x00059C89 File Offset: 0x00057E89
	public float TimeToLive { get; private set; }

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x060011F8 RID: 4600 RVA: 0x00059C92 File Offset: 0x00057E92
	// (set) Token: 0x060011F9 RID: 4601 RVA: 0x00059C9A File Offset: 0x00057E9A
	public float Lifetime { get; set; }

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x060011FA RID: 4602 RVA: 0x00059CA3 File Offset: 0x00057EA3
	// (set) Token: 0x060011FB RID: 4603 RVA: 0x00059CAB File Offset: 0x00057EAB
	internal int CellX { get; private set; }

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x060011FC RID: 4604 RVA: 0x00059CB4 File Offset: 0x00057EB4
	// (set) Token: 0x060011FD RID: 4605 RVA: 0x00059CBC File Offset: 0x00057EBC
	internal int CellZ { get; private set; }

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x060011FE RID: 4606 RVA: 0x00059CC5 File Offset: 0x00057EC5
	// (set) Token: 0x060011FF RID: 4607 RVA: 0x00059CCD File Offset: 0x00057ECD
	internal int DepthLayer { get; private set; } = -1;

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06001200 RID: 4608 RVA: 0x00059CD6 File Offset: 0x00057ED6
	// (set) Token: 0x06001201 RID: 4609 RVA: 0x00059CDE File Offset: 0x00057EDE
	internal DecalLayerBand LayerBand { get; private set; }

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06001202 RID: 4610 RVA: 0x00059CE7 File Offset: 0x00057EE7
	// (set) Token: 0x06001203 RID: 4611 RVA: 0x00059CEF File Offset: 0x00057EEF
	internal GroundDecalsCollection OwnerCollection { get; private set; }

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06001204 RID: 4612 RVA: 0x00059CF8 File Offset: 0x00057EF8
	// (set) Token: 0x06001205 RID: 4613 RVA: 0x00059D00 File Offset: 0x00057F00
	internal Vector3 GroundPosition { get; private set; }

	// Token: 0x06001206 RID: 4614 RVA: 0x00059D09 File Offset: 0x00057F09
	internal void SetPlacementMetadata(int cellX, int cellZ, int depthLayer, DecalLayerBand layerBand, GroundDecalsCollection owner, Vector3 groundPosition)
	{
		this.CellX = cellX;
		this.CellZ = cellZ;
		this.DepthLayer = depthLayer;
		this.LayerBand = layerBand;
		this.OwnerCollection = owner;
		this.GroundPosition = groundPosition;
	}

	// Token: 0x06001207 RID: 4615 RVA: 0x00059D38 File Offset: 0x00057F38
	internal void ClearPlacementMetadata()
	{
		this.CellX = 0;
		this.CellZ = 0;
		this.DepthLayer = -1;
		this.LayerBand = DecalLayerBand.Blood;
		this.OwnerCollection = null;
		this.GroundPosition = Vector3.zero;
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06001208 RID: 4616 RVA: 0x00059D68 File Offset: 0x00057F68
	public int SortingGroup
	{
		get
		{
			return this.sortingGroup;
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06001209 RID: 4617 RVA: 0x00059D70 File Offset: 0x00057F70
	public float GroupYOffset
	{
		get
		{
			return this.groupYOffset;
		}
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x00059D78 File Offset: 0x00057F78
	public IFightDecal SpawnDecal(Vector3 position, Direction orientation, string customDeathEffectId = "")
	{
		base.transform.position = position;
		this.spawnPosition = position;
		this.Rotate(orientation);
		if (!string.IsNullOrEmpty(customDeathEffectId))
		{
			this.ActivateDecalByName(customDeathEffectId);
		}
		else
		{
			this.ActivateRandomDecal();
		}
		this.Lifetime = 0f;
		if (this.animateY)
		{
			float animProgress = 0f;
			DOTween.To(() => animProgress, delegate(float x)
			{
				animProgress = x;
				this.transform.position = this.spawnPosition + Vector3.up * (this.scaleCurve.Evaluate(animProgress) * this.maxYOffset);
			}, this.animDuration, this.animDuration);
		}
		return this;
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x00059E0C File Offset: 0x0005800C
	private void Rotate(Direction direction)
	{
		if (direction == Direction.None)
		{
			return;
		}
		direction.ConvertToVector2XZ();
		base.transform.rotation = Quaternion.FromToRotation(Vector3.forward, direction.ConvertToVector3());
		switch (direction)
		{
		case Direction.Right:
		case Direction.Left:
			base.transform.localScale = new Vector3(1.25f, 1f, 1f);
			return;
		case Direction.Up:
		case Direction.Down:
			base.transform.localScale = Vector3.one;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x00059E88 File Offset: 0x00058088
	private void ActivateDecalByName(string customDeathEffectId)
	{
		if (string.IsNullOrEmpty(customDeathEffectId))
		{
			return;
		}
		GameObject gameObject = this.decalObjs.Find((GameObject item) => item.name == customDeathEffectId);
		if (gameObject)
		{
			gameObject.SetActive(true);
		}
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x00059ED8 File Offset: 0x000580D8
	private void ActivateRandomDecal()
	{
		this.decalObjs.ForEach(delegate(GameObject item)
		{
			item.SetActive(false);
		});
		int num = global::UnityEngine.Random.Range(0, this.decalObjs.Count);
		this.decalObjs[num].SetActive(true);
	}

	// Token: 0x0600120E RID: 4622 RVA: 0x00059F33 File Offset: 0x00058133
	private void Awake()
	{
		this.TimeToLive = LazySingletonSO<GlobalResources>.Instance.fighting.decalsLifeTime;
	}

	// Token: 0x040013CF RID: 5071
	private const float X_SCALE = 1.25f;

	// Token: 0x040013D0 RID: 5072
	private const float Z_SCALE = 1f;

	// Token: 0x040013D1 RID: 5073
	public List<GameObject> decalObjs = new List<GameObject>();

	// Token: 0x040013D2 RID: 5074
	public bool animateY;

	// Token: 0x040013D3 RID: 5075
	public float maxYOffset = 0.5f;

	// Token: 0x040013D4 RID: 5076
	public float animDuration = 1f;

	// Token: 0x040013D5 RID: 5077
	public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

	// Token: 0x040013D6 RID: 5078
	public int sortingGroup;

	// Token: 0x040013D7 RID: 5079
	public float groupYOffset;

	// Token: 0x040013D8 RID: 5080
	internal const int INVALID_DEPTH_LAYER = -1;

	// Token: 0x040013D9 RID: 5081
	private Vector3 spawnPosition;
}
