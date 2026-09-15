using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B73 RID: 2931
	[Name("Add Wgo As Fighting Decoy", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_AddWgoAsFightingDecoy : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004D44 RID: 19780 RVA: 0x0016C824 File Offset: 0x0016AA24
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoDecoyManipulationLogic), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.customPriority = base.AddValueInput<LazyConsts.Fighting.TargetAttackPriority>("customPriority", "");
			this.customPriority.SetDefaultAndSerializedValue(LazyConsts.Fighting.TargetAttackPriority.High);
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x0016C8A0 File Offset: 0x0016AAA0
		private void DoDecoyManipulationLogic(Flow flow)
		{
			WgoData wgoData = base.GetWgoData();
			Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal((wgoData != null) ? wgoData.UniqueId : null);
			if (wgoViewGlobal != null)
			{
				if (this.toAdd)
				{
					LazySingleton<FightingGameController>.Instance.AddWgoAsCustomDecoy(wgoViewGlobal, false, this.customPriority.value);
				}
				else
				{
					LazySingleton<FightingGameController>.Instance.RemoveWgFromCustomDecoy(wgoViewGlobal);
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06004D46 RID: 19782 RVA: 0x0016C906 File Offset: 0x0016AB06
		public override string name
		{
			get
			{
				if (!this.toAdd)
				{
					return "Remove Wgo From Fighting Decoy";
				}
				return "Add Wgo As Fighting Decoy";
			}
		}

		// Token: 0x04003E24 RID: 15908
		[FlowNode.GatherPortsCallbackAttribute]
		public bool toAdd;

		// Token: 0x04003E25 RID: 15909
		private FlowInput @in;

		// Token: 0x04003E26 RID: 15910
		private FlowOutput @out;

		// Token: 0x04003E27 RID: 15911
		private ValueInput<LazyConsts.Fighting.TargetAttackPriority> customPriority;
	}
}
