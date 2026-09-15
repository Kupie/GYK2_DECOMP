using System;
using UnityEngine;

// Token: 0x0200073A RID: 1850
[Command]
[Serializable]
public class CharMoveCommand : Command
{
	// Token: 0x0600303E RID: 12350 RVA: 0x000E7928 File Offset: 0x000E5B28
	public override void Execute(ulong senderClientId, GameSave gameSave)
	{
		if (this.playerId == (int)LazyNetwork.NetworkManager.MyId)
		{
			return;
		}
		NetworkPlayer networkPlayer;
		gameSave.GetClient((int)senderClientId, out networkPlayer, true);
		networkPlayer.playerData.position.Value = this.position;
		networkPlayer.playerData.Direction = this.direction;
		networkPlayer.playerData.charState.Value = this.animState;
	}

	// Token: 0x04002716 RID: 10006
	[CommandField]
	public int playerId;

	// Token: 0x04002717 RID: 10007
	[CommandField]
	public Vector3 position;

	// Token: 0x04002718 RID: 10008
	[CommandField]
	public Vector2 direction;

	// Token: 0x04002719 RID: 10009
	[CommandField]
	public global::AnimationState animState;
}
