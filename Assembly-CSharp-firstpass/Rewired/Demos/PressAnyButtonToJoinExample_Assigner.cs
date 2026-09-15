using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000102 RID: 258
	[AddComponentMenu("")]
	public class PressAnyButtonToJoinExample_Assigner : MonoBehaviour
	{
		// Token: 0x06000CB4 RID: 3252 RVA: 0x00024C4F File Offset: 0x00022E4F
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.AssignJoysticksToPlayers();
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00024C60 File Offset: 0x00022E60
		private void AssignJoysticksToPlayers()
		{
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				Joystick joystick = joysticks[i];
				if (!ReInput.controllers.IsControllerAssigned(joystick.type, joystick.id) && joystick.GetAnyButtonDown())
				{
					Player player = this.FindPlayerWithoutJoystick();
					if (player == null)
					{
						return;
					}
					player.controllers.AddController(joystick, false);
				}
			}
			if (this.DoAllPlayersHaveJoysticks())
			{
				ReInput.configuration.autoAssignJoysticks = true;
				base.enabled = false;
			}
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x00024CE4 File Offset: 0x00022EE4
		private Player FindPlayerWithoutJoystick()
		{
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i].controllers.joystickCount <= 0)
				{
					return players[i];
				}
			}
			return null;
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00024D2A File Offset: 0x00022F2A
		private bool DoAllPlayersHaveJoysticks()
		{
			return this.FindPlayerWithoutJoystick() == null;
		}
	}
}
