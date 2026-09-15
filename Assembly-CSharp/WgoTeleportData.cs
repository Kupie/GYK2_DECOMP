using System;
using UnityEngine;

// Token: 0x020003BA RID: 954
public class WgoTeleportData : TeleportDataBase
{
	// Token: 0x060019A5 RID: 6565 RVA: 0x000790EF File Offset: 0x000772EF
	public WgoTeleportData(string wgoIdTeleportTo, string environmentPreset = "outdoor", string soundOnTeleport = "", bool teleportToDockPoint = false, Action onGameSceneLoaded = null, bool donNotFade = false, float delayInFade = 0.3f)
		: base(environmentPreset, soundOnTeleport, onGameSceneLoaded, donNotFade, delayInFade)
	{
		this.wgoIdTeleportTo = wgoIdTeleportTo;
		this.teleportToDockPoint = teleportToDockPoint;
	}

	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x060019A6 RID: 6566 RVA: 0x0007910E File Offset: 0x0007730E
	public WgoData WgoData
	{
		get
		{
			if (this.wgoData == null)
			{
				MainGame.Instance.GameSave.worldData.TryGetWgoData(this.wgoIdTeleportTo, out this.wgoData, out this.gameSceneData);
			}
			return this.wgoData;
		}
	}

	// Token: 0x060019A7 RID: 6567 RVA: 0x00079145 File Offset: 0x00077345
	public override string GetDestinationId()
	{
		return this.wgoIdTeleportTo;
	}

	// Token: 0x060019A8 RID: 6568 RVA: 0x0007914D File Offset: 0x0007734D
	public override GameSceneData GetDestinationSceneData()
	{
		if (this.gameSceneData == null)
		{
			MainGame.Instance.GameSave.worldData.TryGetWgoData(this.wgoIdTeleportTo, out this.wgoData, out this.gameSceneData);
		}
		return this.gameSceneData;
	}

	// Token: 0x060019A9 RID: 6569 RVA: 0x00079184 File Offset: 0x00077384
	public override Vector3 GetPosition()
	{
		if (this.WgoData == null)
		{
			return default(Vector3);
		}
		if (!this.teleportToDockPoint)
		{
			return this.wgoData.GetTeleportPointPosition();
		}
		return this.wgoData.GetFirstDockPointDataWorldPosition();
	}

	// Token: 0x060019AA RID: 6570 RVA: 0x000791C2 File Offset: 0x000773C2
	public override bool CanTeleport(out string error)
	{
		error = string.Empty;
		if (this.WgoData == null)
		{
			error = "Can't teleport to " + this.wgoIdTeleportTo + ", wgoData not found";
			return false;
		}
		return true;
	}

	// Token: 0x040018F2 RID: 6386
	public string wgoIdTeleportTo;

	// Token: 0x040018F3 RID: 6387
	public bool teleportToDockPoint;

	// Token: 0x040018F4 RID: 6388
	private WgoData wgoData;

	// Token: 0x040018F5 RID: 6389
	private GameSceneData gameSceneData;
}
