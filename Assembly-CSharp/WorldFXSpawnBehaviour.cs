using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020007CA RID: 1994
[Serializable]
public class WorldFXSpawnBehaviour : PlayableBehaviour
{
	// Token: 0x06003342 RID: 13122 RVA: 0x000F6AEC File Offset: 0x000F4CEC
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		Transform transform = playerData as Transform;
		if (transform == null)
		{
			return;
		}
		if (!this.hasSpawned && info.effectiveWeight > 0f)
		{
			WorldFX.Spawn(transform, this.id, null, default(Vector3));
			this.hasSpawned = true;
		}
	}

	// Token: 0x06003343 RID: 13123 RVA: 0x000F6B3E File Offset: 0x000F4D3E
	public override void OnPlayableDestroy(Playable playable)
	{
		this.hasSpawned = false;
	}

	// Token: 0x040028FA RID: 10490
	public string id;

	// Token: 0x040028FB RID: 10491
	public bool attachToTarget;

	// Token: 0x040028FC RID: 10492
	private bool hasSpawned;
}
