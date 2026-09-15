using System;
using UnityEngine;

// Token: 0x02000762 RID: 1890
[Serializable]
public class NetworkPlayer
{
	// Token: 0x0600311A RID: 12570 RVA: 0x00021B94 File Offset: 0x0001FD94
	public NetworkPlayer()
	{
	}

	// Token: 0x0600311B RID: 12571 RVA: 0x000E9485 File Offset: 0x000E7685
	public NetworkPlayer(int clientId, PlayerData playerData)
	{
		this.playerData = playerData;
		this.clientId = clientId;
	}

	// Token: 0x0600311C RID: 12572 RVA: 0x000E949C File Offset: 0x000E769C
	public void SubscribeToPlayerDataChanges(PlayerPhysicalBody playerBody)
	{
		this.playerData.position.ValueChanged += delegate(Vector3 newPosition)
		{
			playerBody.MoveByPosition(newPosition, this.playerData.Direction, true);
		};
		this.playerData.AddDirectionListener(delegate(Vector2 direction)
		{
			playerBody.PlayerView.PlayerAnimation.SetDirection(direction);
		});
		this.playerData.charState.ValueChanged += delegate(global::AnimationState state)
		{
			playerBody.PlayerView.PlayerAnimation.SetState(state);
		};
	}

	// Token: 0x04002772 RID: 10098
	public PlayerData playerData;

	// Token: 0x04002773 RID: 10099
	public int clientId;
}
