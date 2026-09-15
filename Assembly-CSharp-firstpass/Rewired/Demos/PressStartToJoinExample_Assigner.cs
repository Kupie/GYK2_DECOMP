using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000104 RID: 260
	[AddComponentMenu("")]
	public class PressStartToJoinExample_Assigner : MonoBehaviour
	{
		// Token: 0x06000CBF RID: 3263 RVA: 0x00024EAC File Offset: 0x000230AC
		public static Player GetRewiredPlayer(int gamePlayerId)
		{
			if (!ReInput.isReady)
			{
				return null;
			}
			if (PressStartToJoinExample_Assigner.instance == null)
			{
				Debug.LogError("Not initialized. Do you have a PressStartToJoinPlayerSelector in your scehe?");
				return null;
			}
			for (int i = 0; i < PressStartToJoinExample_Assigner.instance.playerMap.Count; i++)
			{
				if (PressStartToJoinExample_Assigner.instance.playerMap[i].gamePlayerId == gamePlayerId)
				{
					return ReInput.players.GetPlayer(PressStartToJoinExample_Assigner.instance.playerMap[i].rewiredPlayerId);
				}
			}
			return null;
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00024F2E File Offset: 0x0002312E
		private void Awake()
		{
			this.playerMap = new List<PressStartToJoinExample_Assigner.PlayerMap>();
			PressStartToJoinExample_Assigner.instance = this;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00024F44 File Offset: 0x00023144
		private void Update()
		{
			for (int i = 0; i < ReInput.players.playerCount; i++)
			{
				if (ReInput.players.GetPlayer(i).GetButtonDown("JoinGame"))
				{
					this.AssignNextPlayer(i);
				}
			}
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00024F84 File Offset: 0x00023184
		private void AssignNextPlayer(int rewiredPlayerId)
		{
			if (this.playerMap.Count >= this.maxPlayers)
			{
				Debug.LogError("Max player limit already reached!");
				return;
			}
			int nextGamePlayerId = this.GetNextGamePlayerId();
			this.playerMap.Add(new PressStartToJoinExample_Assigner.PlayerMap(rewiredPlayerId, nextGamePlayerId));
			Player player = ReInput.players.GetPlayer(rewiredPlayerId);
			player.controllers.maps.SetMapsEnabled(false, "Assignment");
			player.controllers.maps.SetMapsEnabled(true, "Default");
			Debug.Log("Added Rewired Player id " + rewiredPlayerId.ToString() + " to game player " + nextGamePlayerId.ToString());
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x00025024 File Offset: 0x00023224
		private int GetNextGamePlayerId()
		{
			int num = this.gamePlayerIdCounter;
			this.gamePlayerIdCounter = num + 1;
			return num;
		}

		// Token: 0x04000687 RID: 1671
		private static PressStartToJoinExample_Assigner instance;

		// Token: 0x04000688 RID: 1672
		public int maxPlayers = 4;

		// Token: 0x04000689 RID: 1673
		private List<PressStartToJoinExample_Assigner.PlayerMap> playerMap;

		// Token: 0x0400068A RID: 1674
		private int gamePlayerIdCounter;

		// Token: 0x02000105 RID: 261
		private class PlayerMap
		{
			// Token: 0x06000CC5 RID: 3269 RVA: 0x00025051 File Offset: 0x00023251
			public PlayerMap(int rewiredPlayerId, int gamePlayerId)
			{
				this.rewiredPlayerId = rewiredPlayerId;
				this.gamePlayerId = gamePlayerId;
			}

			// Token: 0x0400068B RID: 1675
			public int rewiredPlayerId;

			// Token: 0x0400068C RID: 1676
			public int gamePlayerId;
		}
	}
}
