using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020007C8 RID: 1992
[Serializable]
public class AddWgoDataBehaviour : PlayableBehaviour
{
	// Token: 0x0600333C RID: 13116 RVA: 0x000F69F8 File Offset: 0x000F4BF8
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (this.hasFired || info.effectiveWeight <= 0f)
		{
			return;
		}
		Transform transform = playerData as Transform;
		Vector3 vector = ((this.useBindingPosition && transform != null) ? transform.position : this.position);
		string text = this.gameSceneId;
		if (string.IsNullOrEmpty(text))
		{
			if (MainGame.PlayerData != null && !string.IsNullOrEmpty(MainGame.PlayerData.currentGameSceneId))
			{
				text = MainGame.PlayerData.currentGameSceneId;
			}
			else
			{
				text = MainGame.EntrySceneToLoad;
			}
		}
		if (!string.IsNullOrEmpty(this.wgoId))
		{
			WgoData wgoData;
			MainGame.WorldData.AddWgoData(this.wgoId, vector, text, this.customTag, out wgoData, false);
		}
		this.hasFired = true;
	}

	// Token: 0x0600333D RID: 13117 RVA: 0x000F6AAD File Offset: 0x000F4CAD
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		this.hasFired = false;
	}

	// Token: 0x040028F3 RID: 10483
	public string wgoId;

	// Token: 0x040028F4 RID: 10484
	public string customTag;

	// Token: 0x040028F5 RID: 10485
	public bool useBindingPosition = true;

	// Token: 0x040028F6 RID: 10486
	public Vector3 position;

	// Token: 0x040028F7 RID: 10487
	public string gameSceneId;

	// Token: 0x040028F8 RID: 10488
	private bool hasFired;
}
