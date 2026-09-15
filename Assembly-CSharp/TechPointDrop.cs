using System;
using System.Collections;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020001A1 RID: 417
public class TechPointDrop : MonoBehaviour
{
	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06000A8D RID: 2701 RVA: 0x000355A5 File Offset: 0x000337A5
	public Rigidbody RigidBody
	{
		get
		{
			return this.rigidBody;
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06000A8E RID: 2702 RVA: 0x000355AD File Offset: 0x000337AD
	public TechPointDropData Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x000355B8 File Offset: 0x000337B8
	public static TechPointDrop Spawn(TechPointDropData techPointDropData, Transform parent)
	{
		if (TechPointDrop.pool == null)
		{
			TechPointDrop.pool = LazyPooler.CreatePool<TechPointDrop>(Addressables.LoadAssetAsync<GameObject>("Assets/AddressableAssets/Drops/TechPointDrop.prefab").WaitForCompletion().GetComponent<TechPointDrop>(), 0, Pool.PoolType.ImmediateActivation, true, false, null);
		}
		TechPointDrop orCreateObject = TechPointDrop.pool.GetOrCreateObject<TechPointDrop>();
		orCreateObject.Init(techPointDropData);
		orCreateObject.transform.SetParent(parent);
		orCreateObject.transform.position = techPointDropData.pos;
		orCreateObject.animator.SetInteger(TechPointDrop.color, (int)techPointDropData.type);
		orCreateObject.RigidBody.linearVelocity = Vector3.zero;
		orCreateObject.RigidBody.angularVelocity = Vector3.zero;
		return orCreateObject;
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x00035658 File Offset: 0x00033858
	public static void CollectAllToPlayer(Transform target, float duration)
	{
		for (int i = TechPointDrop.activeDrops.Count - 1; i >= 0; i--)
		{
			TechPointDrop techPointDrop = TechPointDrop.activeDrops[i];
			if (techPointDrop != null)
			{
				techPointDrop.MoveToCollectorTimed(target, duration);
			}
		}
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x0003569C File Offset: 0x0003389C
	public static bool IsTracked(TechPointDropData dropData)
	{
		if (dropData == null)
		{
			return false;
		}
		for (int i = 0; i < TechPointDrop.activeDrops.Count; i++)
		{
			TechPointDrop techPointDrop = TechPointDrop.activeDrops[i];
			if (techPointDrop != null && techPointDrop.data == dropData)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x000356E4 File Offset: 0x000338E4
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, this.collectRadius);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(base.transform.position, this.magnetRadius);
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x00035734 File Offset: 0x00033934
	private void Init(TechPointDropData techPointDropData)
	{
		this.StopTimedMove();
		this.data = techPointDropData;
		this.techPointName = TechDef.FlyingReses[(int)techPointDropData.type];
		this.coll.enabled = true;
		this.collectDelay = this.delay;
		this.curTime = 0f;
		this.tf = base.transform;
		this.player = MainGame.PlayerController.transform;
		this.isInitialized = true;
		this.isTimedCollecting = false;
		this.rigidBody.useGravity = false;
		this.rigidBody.isKinematic = false;
		this.RegisterActive();
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x000357D0 File Offset: 0x000339D0
	private void FixedUpdate()
	{
		if (!this.isInitialized || this.isTimedCollecting)
		{
			return;
		}
		if (MainGame.IsGamePaused)
		{
			return;
		}
		this.data.pos = this.rigidBody.position;
		this.curTime += Time.fixedDeltaTime;
		if (this.curTime < this.collectDelay)
		{
			return;
		}
		Vector3 vector = this.player.position - this.tf.position;
		float sqrMagnitude = vector.XZ().sqrMagnitude;
		if (sqrMagnitude > this.magnetRadius)
		{
			this.coll.isTrigger = false;
			return;
		}
		this.coll.isTrigger = true;
		if (sqrMagnitude < this.collectRadius * this.collectRadius)
		{
			this.Collect();
			return;
		}
		Vector3 linearVelocity = this.rigidBody.linearVelocity;
		if (linearVelocity.sqrMagnitude > 0.0001f && vector.sqrMagnitude > 0.0001f)
		{
			this.rigidBody.linearVelocity = Vector3.RotateTowards(linearVelocity, vector, this.magnetTurnSpeed * 0.017453292f * Time.fixedDeltaTime, 0f);
		}
		this.rigidBody.AddForce(vector * (this.magnetForce * 1.2f));
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x00035900 File Offset: 0x00033B00
	public void MoveToCollectorTimed(Transform target, float duration)
	{
		if (!this.isInitialized || this.data == null || this.isTimedCollecting)
		{
			return;
		}
		this.StopTimedMove();
		this.isTimedCollecting = true;
		this.coll.enabled = false;
		this.rigidBody.linearVelocity = Vector3.zero;
		this.rigidBody.angularVelocity = Vector3.zero;
		this.rigidBody.isKinematic = true;
		if (target == null || duration <= 0f)
		{
			this.Collect();
			return;
		}
		this.moveToCollectorCoroutine = base.StartCoroutine(this.MoveToCollectorTimedCoroutine(target, duration));
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x00035997 File Offset: 0x00033B97
	private IEnumerator MoveToCollectorTimedCoroutine(Transform target, float duration)
	{
		Vector3 startPosition = ((this.rigidBody != null) ? this.rigidBody.position : base.transform.position);
		float elapsed = 0f;
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		while (elapsed < duration)
		{
			if (!this.isInitialized || this.data == null)
			{
				yield break;
			}
			if (target == null)
			{
				this.moveToCollectorCoroutine = null;
				this.Collect();
				yield break;
			}
			if (!MainGame.IsGamePaused)
			{
				elapsed += Time.deltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				this.SetWorldPosition(Vector3.Lerp(startPosition, target.position, num * num));
			}
			yield return wait;
		}
		if (!this.isInitialized || this.data == null)
		{
			yield break;
		}
		if (target != null)
		{
			this.SetWorldPosition(target.position);
		}
		this.moveToCollectorCoroutine = null;
		this.Collect();
		yield break;
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x000359B4 File Offset: 0x00033BB4
	private void SetWorldPosition(Vector3 position)
	{
		base.transform.position = position;
		if (this.rigidBody != null)
		{
			this.rigidBody.position = position;
		}
		if (this.data != null)
		{
			this.data.pos = position;
		}
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x000359F0 File Offset: 0x00033BF0
	private void Collect()
	{
		if (!this.isInitialized || this.data == null)
		{
			return;
		}
		this.StopTimedMove();
		this.UnregisterActive();
		this.isInitialized = false;
		this.coll.enabled = false;
		DebugDraw.DrawCross(FlyingTechPoint.Drop(MainGame.PlayerController.transform.position, this.techPointName, null, -1).transform.position, 1f, Color.yellow, 2f, true);
		this.PlaySound();
		MainGame.Instance.GameSave.WorldData.GetGameSceneDataById(this.data.worldId).RemoveTechPointDrop(this.data);
		this.collectDelay = 1f;
		this.curTime = 0f;
		this.data = null;
		TechPointDrop.pool.ReleaseObject<TechPointDrop>(this);
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x00035AC0 File Offset: 0x00033CC0
	private void PlaySound()
	{
		if (this.data.type != TechPointsSpawner.Type.H)
		{
			LazyAudio.Play("tech_point_collect");
			return;
		}
		int num;
		FlyingTechPoint.FlyingTechPointsCountByName.TryGetValue("happiness", out num);
		if (MainGame.Instance.GameSave.townSystem.Quality > 0)
		{
			LazyAudio.Play((MainGame.PlayerData.GetResInt("happiness") + num > MainGame.Instance.GameSave.townSystem.Quality) ? "tech_point_collect_failed" : "tech_point_collect");
			return;
		}
		LazyAudio.Play("tech_point_collect");
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x00035B55 File Offset: 0x00033D55
	private void RegisterActive()
	{
		if (!TechPointDrop.activeDrops.Contains(this))
		{
			TechPointDrop.activeDrops.Add(this);
		}
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x00035B6F File Offset: 0x00033D6F
	private void UnregisterActive()
	{
		TechPointDrop.activeDrops.Remove(this);
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x00035B7D File Offset: 0x00033D7D
	private void StopTimedMove()
	{
		if (this.moveToCollectorCoroutine != null)
		{
			base.StopCoroutine(this.moveToCollectorCoroutine);
			this.moveToCollectorCoroutine = null;
		}
		this.isTimedCollecting = false;
		if (this.rigidBody != null)
		{
			this.rigidBody.isKinematic = false;
		}
	}

	// Token: 0x04000BFB RID: 3067
	private static Pool pool;

	// Token: 0x04000BFC RID: 3068
	private static readonly int color = Animator.StringToHash("color");

	// Token: 0x04000BFD RID: 3069
	private static readonly List<TechPointDrop> activeDrops = new List<TechPointDrop>();

	// Token: 0x04000BFE RID: 3070
	[SerializeField]
	private float delay = 0.7f;

	// Token: 0x04000BFF RID: 3071
	[SerializeField]
	private float magnetRadius = 200f;

	// Token: 0x04000C00 RID: 3072
	[SerializeField]
	private Rigidbody rigidBody;

	// Token: 0x04000C01 RID: 3073
	[SerializeField]
	private Animator animator;

	// Token: 0x04000C02 RID: 3074
	[SerializeField]
	private Collider coll;

	// Token: 0x04000C03 RID: 3075
	[SerializeField]
	private float collectRadius = 0.25f;

	// Token: 0x04000C04 RID: 3076
	[SerializeField]
	private float magnetForce = 35f;

	// Token: 0x04000C05 RID: 3077
	[SerializeField]
	private float magnetTurnSpeed = 720f;

	// Token: 0x04000C06 RID: 3078
	private Transform tf;

	// Token: 0x04000C07 RID: 3079
	private Transform player;

	// Token: 0x04000C08 RID: 3080
	private TechPointDropData data;

	// Token: 0x04000C09 RID: 3081
	private float collectDelay = 1f;

	// Token: 0x04000C0A RID: 3082
	private float curTime;

	// Token: 0x04000C0B RID: 3083
	private string techPointName;

	// Token: 0x04000C0C RID: 3084
	private bool isInitialized;

	// Token: 0x04000C0D RID: 3085
	private bool isTimedCollecting;

	// Token: 0x04000C0E RID: 3086
	private Coroutine moveToCollectorCoroutine;
}
