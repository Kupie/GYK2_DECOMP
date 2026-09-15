using System;
using FlowCanvas;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BBA RID: 3002
	[Name("Get Player Pos", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_GetPlayerPos : GKCustomFlowNode
	{
		// Token: 0x06004E33 RID: 20019 RVA: 0x00170EA8 File Offset: 0x0016F0A8
		protected override void RegisterPorts()
		{
			this.posValOut = base.AddValueOutput<Vector3>("Pos", () => MainGame.PlayerController.transform.position, "");
		}

		// Token: 0x04003F58 RID: 16216
		private ValueOutput<Vector3> posValOut;
	}
}
