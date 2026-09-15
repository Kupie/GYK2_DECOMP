using System;
using FlowCanvas;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BB8 RID: 3000
	[Name("Get Insanity", 0)]
	[Category("Game/GameRes")]
	public class Flow_GetInsanity : GKCustomFlowNode
	{
		// Token: 0x06004E2B RID: 20011 RVA: 0x00170CF2 File Offset: 0x0016EEF2
		protected override void RegisterPorts()
		{
			this.gameResValueOut = base.AddValueOutput<float>("value", delegate
			{
				if (this.getMaxValue)
				{
					return PlayerInsanityGameResSystem.GetSystem().Max;
				}
				return base.PlayerData.GetRes("insanity", 0f);
			}, "");
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06004E2C RID: 20012 RVA: 0x00170D16 File Offset: 0x0016EF16
		public override string name
		{
			get
			{
				return "Get " + (this.getMaxValue ? "Max" : "Current") + " Insanity";
			}
		}

		// Token: 0x04003F52 RID: 16210
		[FlowNode.GatherPortsCallbackAttribute]
		public bool getMaxValue;

		// Token: 0x04003F53 RID: 16211
		private ValueOutput<float> gameResValueOut;
	}
}
