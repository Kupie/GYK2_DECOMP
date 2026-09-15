using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020001A4 RID: 420
public class TechPointsSpawner : MonoBehaviour
{
	// Token: 0x06000AA7 RID: 2727 RVA: 0x00035DBC File Offset: 0x00033FBC
	public static void CreateSpawner(Vector3 pos, int r, int g, int b, int h)
	{
		if (TechPointsSpawner.pool == null)
		{
			TechPointsSpawner.pool = LazyPooler.CreatePool<TechPointsSpawner>(Addressables.LoadAssetAsync<GameObject>("Assets/AddressableAssets/Drops/TechPointsSpawner.prefab").WaitForCompletion().GetComponent<TechPointsSpawner>(), 0, Pool.PoolType.ImmediateActivation, true, false, null);
		}
		TechPointsSpawner orCreateObject = TechPointsSpawner.pool.GetOrCreateObject<TechPointsSpawner>();
		orCreateObject.transform.position = pos;
		orCreateObject.Spawn(MainGame.PlayerController.CurrentGameScene.Id, r, g, b, h);
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x00035E28 File Offset: 0x00034028
	public static void FlushPendingAsWorldDrops()
	{
		for (int i = TechPointsSpawner.activeSpawners.Count - 1; i >= 0; i--)
		{
			TechPointsSpawner techPointsSpawner = TechPointsSpawner.activeSpawners[i];
			if (techPointsSpawner != null)
			{
				techPointsSpawner.FlushRemainingAsWorldDrops();
			}
		}
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x00035E68 File Offset: 0x00034068
	private void Spawn(string worldId, int r, int g, int b, int h)
	{
		base.transform.SetParent(MainGame.PlayerController.CurrentGameScene.transform);
		this.worldId = worldId;
		this.needSpawn[0] = r;
		this.needSpawn[1] = g;
		this.needSpawn[2] = b;
		this.needSpawn[3] = h;
		this.dt = 0f;
		this.spawning = true;
		if (!TechPointsSpawner.activeSpawners.Contains(this))
		{
			TechPointsSpawner.activeSpawners.Add(this);
		}
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x00035EE8 File Offset: 0x000340E8
	private void Update()
	{
		if (!this.spawning)
		{
			return;
		}
		if (MainGame.IsGamePaused)
		{
			return;
		}
		this.dt += Time.deltaTime;
		if (this.dt > this.period)
		{
			this.dt -= this.period;
			int num = global::UnityEngine.Random.Range(0, 3);
			if (this.needSpawn[num] == 0)
			{
				if (++num == 4)
				{
					num = 0;
				}
				if (this.needSpawn[num] == 0 && ++num == 4)
				{
					num = 0;
				}
			}
			if (this.needSpawn[0] + this.needSpawn[1] + this.needSpawn[2] + this.needSpawn[3] == 0)
			{
				this.OnDoneSpawning();
				return;
			}
			if (this.needSpawn[num] != 0)
			{
				this.needSpawn[num]--;
				this.SpawnTechPoint((TechPointsSpawner.Type)num, true);
			}
		}
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x00035FBC File Offset: 0x000341BC
	private void SpawnTechPoint(TechPointsSpawner.Type type, bool applyImpulse)
	{
		TechPointDropData techPointDropData = new TechPointDropData(base.transform.position, type, this.worldId);
		TechPointDrop techPointDrop = TechPointDrop.Spawn(techPointDropData, base.transform.parent);
		MainGame.Instance.GameSave.WorldData.GetGameSceneDataById(techPointDropData.worldId).AddTechPointDrop(techPointDropData);
		if (!applyImpulse)
		{
			return;
		}
		Vector3 vector = MainGame.PlayerController.MovablePosition - base.transform.position;
		float num = Mathf.Atan2(vector.z, vector.x) * 57.29578f;
		float num2 = global::UnityEngine.Random.Range(this.force.x, this.force.y);
		float num3 = global::UnityEngine.Random.Range(num - this.deltaAngle, num + this.deltaAngle);
		Vector3 vector2 = new Vector3(Mathf.Cos(num3 * 0.017453292f), 0f, Mathf.Sin(num3 * 0.017453292f)) * num2;
		techPointDrop.RigidBody.AddForce(vector2, ForceMode.Impulse);
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x000360BC File Offset: 0x000342BC
	private void FlushRemainingAsWorldDrops()
	{
		if (!this.spawning)
		{
			return;
		}
		for (int i = 0; i < this.needSpawn.Length; i++)
		{
			int num = this.needSpawn[i];
			this.needSpawn[i] = 0;
			for (int j = 0; j < num; j++)
			{
				this.SpawnTechPoint((TechPointsSpawner.Type)i, false);
			}
		}
		this.OnDoneSpawning();
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x00036114 File Offset: 0x00034314
	private void OnDoneSpawning()
	{
		this.spawning = false;
		TechPointsSpawner.activeSpawners.Remove(this);
		for (int i = 0; i < this.needSpawn.Length; i++)
		{
			this.needSpawn[i] = 0;
		}
		this.dt = 0f;
		if (this.releaseAfterSpawn)
		{
			TechPointsSpawner.pool.ReleaseObject<TechPointsSpawner>(this);
		}
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x0003616E File Offset: 0x0003436E
	private void TestSpawn()
	{
		this.Spawn(MainGame.PlayerData.currentGameSceneId, this.testR, this.testG, this.testB, this.testH);
	}

	// Token: 0x04000C1A RID: 3098
	private static Pool pool;

	// Token: 0x04000C1B RID: 3099
	[SerializeField]
	private float period = 0.1f;

	// Token: 0x04000C1C RID: 3100
	[SerializeField]
	private Vector2 force;

	// Token: 0x04000C1D RID: 3101
	[Range(0f, 359f)]
	[SerializeField]
	private float deltaAngle;

	// Token: 0x04000C1E RID: 3102
	[SerializeField]
	private bool releaseAfterSpawn = true;

	// Token: 0x04000C1F RID: 3103
	private int testR = 1;

	// Token: 0x04000C20 RID: 3104
	private int testG = 1;

	// Token: 0x04000C21 RID: 3105
	private int testB = 1;

	// Token: 0x04000C22 RID: 3106
	private int testH = 1;

	// Token: 0x04000C23 RID: 3107
	private int[] needSpawn = new int[4];

	// Token: 0x04000C24 RID: 3108
	private bool spawning;

	// Token: 0x04000C25 RID: 3109
	private float dt;

	// Token: 0x04000C26 RID: 3110
	private string worldId;

	// Token: 0x04000C27 RID: 3111
	private static readonly List<TechPointsSpawner> activeSpawners = new List<TechPointsSpawner>();

	// Token: 0x020001A5 RID: 421
	public enum Type
	{
		// Token: 0x04000C29 RID: 3113
		R,
		// Token: 0x04000C2A RID: 3114
		G,
		// Token: 0x04000C2B RID: 3115
		B,
		// Token: 0x04000C2C RID: 3116
		H
	}
}
