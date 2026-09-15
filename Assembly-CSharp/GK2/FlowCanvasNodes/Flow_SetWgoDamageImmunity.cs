using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C04 RID: 3076
	[Name("Set Wgo Damage Immunity", 0)]
	[Category("Game/WGO")]
	[Color("313c8f")]
	public class Flow_SetWgoDamageImmunity : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004F25 RID: 20261 RVA: 0x00175304 File Offset: 0x00173504
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetDamageImmunityState), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.hasDamageImmunity = base.AddValueInput<bool>("hasDamageImmunity", "");
			if (!this.isPlayer)
			{
				base.RegisterPorts();
			}
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x00175378 File Offset: 0x00173578
		private void SetDamageImmunityState(Flow flow)
		{
			if (this.isPlayer)
			{
				MainGame.PlayerData.hpComponent.IsImmuneToDamage = this.hasDamageImmunity.value;
			}
			else
			{
				WgoData wgoData = base.GetWgoData();
				if (wgoData != null)
				{
					wgoData.HpComponent.IsImmuneToDamage = this.hasDamageImmunity.value;
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06004F27 RID: 20263 RVA: 0x001753D5 File Offset: 0x001735D5
		public override string name
		{
			get
			{
				return "Set " + (this.isPlayer ? "Player" : "Wgo") + " Damage Immunity";
			}
		}

		// Token: 0x04004079 RID: 16505
		[FlowNode.GatherPortsCallbackAttribute]
		public bool isPlayer;

		// Token: 0x0400407A RID: 16506
		private FlowInput @in;

		// Token: 0x0400407B RID: 16507
		private FlowOutput @out;

		// Token: 0x0400407C RID: 16508
		private ValueInput<bool> hasDamageImmunity;
	}
}
