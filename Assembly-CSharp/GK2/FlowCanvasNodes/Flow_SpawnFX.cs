using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C16 RID: 3094
	[Name("Spawn FX", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_SpawnFX : GKCustomFlowNode
	{
		// Token: 0x06004F5B RID: 20315 RVA: 0x00175F90 File Offset: 0x00174190
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SpawnFX), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.fxName = base.AddValueInput<string>("fxName", "");
			this.worldPos = base.AddValueInput<Vector3>("worldPos", "");
		}

		// Token: 0x06004F5C RID: 20316 RVA: 0x0017600B File Offset: 0x0017420B
		private void SpawnFX(Flow flow)
		{
			WorldFX.Spawn(this.worldPos.value, this.fxName.value, null, this.size);
			this.@out.Call(flow);
		}

		// Token: 0x040040BB RID: 16571
		public Vector3 size = Vector3.one;

		// Token: 0x040040BC RID: 16572
		protected FlowInput @in;

		// Token: 0x040040BD RID: 16573
		protected FlowOutput @out;

		// Token: 0x040040BE RID: 16574
		protected ValueInput<string> fxName;

		// Token: 0x040040BF RID: 16575
		protected ValueInput<Vector3> worldPos;
	}
}
