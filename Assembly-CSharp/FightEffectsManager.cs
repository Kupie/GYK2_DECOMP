using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020002BB RID: 699
public class FightEffectsManager : MonoBehaviour
{
	// Token: 0x060011E3 RID: 4579 RVA: 0x00059510 File Offset: 0x00057710
	public void SpawnBloodPaddle(Vector3 position, Direction orientation)
	{
		int num = Physics.RaycastNonAlloc(new Ray(position + Vector3.up, Vector3.down), this.hits, 2f, 257, QueryTriggerInteraction.Collide);
		for (int i = 0; i < num; i++)
		{
			RaycastHit raycastHit = this.hits[i];
			Collider collider = raycastHit.collider;
			if (collider == null || collider.gameObject.layer != 8)
			{
				Collider collider2 = raycastHit.collider;
				if ((collider2 != null) ? collider2.GetComponentInParent<BloodPaddle>() : null)
				{
					return;
				}
			}
		}
		BloodPaddle orCreateObject = this.bloodPaddlePool.GetOrCreateObject<BloodPaddle>();
		orCreateObject.SpawnDecal(position, orientation, "");
		this.activePaddles.Add(orCreateObject);
	}

	// Token: 0x060011E4 RID: 4580 RVA: 0x000595C0 File Offset: 0x000577C0
	private void Awake()
	{
		this.bloodPaddlePool = new Pool(this.bloodPaddlePrefab, this.bloodPaddlesParent, 5, Pool.PoolType.ImmediateActivation, false, null);
		this.bloodPaddlePrefab.gameObject.SetActive(false);
		this.bloodDecalsCollection.sortedDecals = this.sortedDecals;
		this.bonesDecalsCollection.sortedDecals = this.sortedDecals;
		this.gutsDecalsCollection.sortedDecals = this.sortedDecals;
		this.bloodDecalsCollection.SetLayerBand(DecalLayerBand.Blood);
		this.bonesDecalsCollection.SetLayerBand(DecalLayerBand.Gore);
		this.gutsDecalsCollection.SetLayerBand(DecalLayerBand.Guts);
	}

	// Token: 0x060011E5 RID: 4581 RVA: 0x00059650 File Offset: 0x00057850
	private void Update()
	{
		if (MainGame.IsGamePaused)
		{
			return;
		}
		this.bloodDecalsCollection.UpdateLifeTime(Time.deltaTime);
		this.bonesDecalsCollection.UpdateLifeTime(Time.deltaTime);
		this.gutsDecalsCollection.UpdateLifeTime(Time.deltaTime);
		for (int i = 0; i < this.activePaddles.Count; i++)
		{
			BloodPaddle bloodPaddle = this.activePaddles[i];
			bloodPaddle.Lifetime += Time.deltaTime;
			if (bloodPaddle.Lifetime > bloodPaddle.TimeToLive)
			{
				this.bloodPaddlePool.ReleaseObject<BloodPaddle>(bloodPaddle);
				this.activePaddles.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x040013BD RID: 5053
	[Header("Decals")]
	public GroundDecalsCollection bloodDecalsCollection;

	// Token: 0x040013BE RID: 5054
	public GroundDecalsCollection bonesDecalsCollection;

	// Token: 0x040013BF RID: 5055
	public GroundDecalsCollection gutsDecalsCollection;

	// Token: 0x040013C0 RID: 5056
	[Header("Paddles")]
	public BloodPaddle bloodPaddlePrefab;

	// Token: 0x040013C1 RID: 5057
	public Transform bloodPaddlesParent;

	// Token: 0x040013C2 RID: 5058
	private Pool bloodPaddlePool;

	// Token: 0x040013C3 RID: 5059
	private List<BloodPaddle> activePaddles = new List<BloodPaddle>();

	// Token: 0x040013C4 RID: 5060
	private RaycastHit[] hits = new RaycastHit[10];

	// Token: 0x040013C5 RID: 5061
	private SortedDecals sortedDecals = new SortedDecals();
}
