using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BFF RID: 3071
	[Name("Set Player Armor", 0)]
	[Category("Game/Player")]
	[Color("313c8f")]
	public class Flow_SetPlayerArmorState : GKCustomFlowNode
	{
		// Token: 0x06004F13 RID: 20243 RVA: 0x00174EE0 File Offset: 0x001730E0
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetArmorState), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x00174F30 File Offset: 0x00173130
		private void SetArmorState(Flow flow)
		{
			if (this.isEnabled)
			{
				PlayerController playerController = MainGame.PlayerController;
				bool flag = true;
				bool flag2 = this.withHelmet;
				playerController.SetArmorView(flag, null, flag2);
				if (this.withSword)
				{
					MainGame.PlayerController.AttackComponent.EquipWeapon(ItemType.Sword);
				}
			}
			else
			{
				MainGame.PlayerController.AttackComponent.UnequipWeapon();
				if (this.withArmor)
				{
					MainGame.PlayerController.SetArmorView(false, null, true);
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x06004F15 RID: 20245 RVA: 0x00174FB4 File Offset: 0x001731B4
		public override string name
		{
			get
			{
				if (!this.isEnabled)
				{
					return "Unequip Sword" + (this.withArmor ? " and Armor" : "");
				}
				return "Equip Armor" + (this.withHelmet ? "" : " (no helmet)") + (this.withSword ? " and Sword" : "");
			}
		}

		// Token: 0x04004065 RID: 16485
		[FlowNode.GatherPortsCallbackAttribute]
		public bool isEnabled;

		// Token: 0x04004066 RID: 16486
		[FlowNode.GatherPortsCallbackAttribute]
		[ShowIf("isEnabled", 0)]
		public bool withArmor;

		// Token: 0x04004067 RID: 16487
		[FlowNode.GatherPortsCallbackAttribute]
		[ShowIf("isEnabled", 1)]
		public bool withSword;

		// Token: 0x04004068 RID: 16488
		[FlowNode.GatherPortsCallbackAttribute]
		[ShowIf("isEnabled", 1)]
		public bool withHelmet = true;

		// Token: 0x04004069 RID: 16489
		private FlowInput @in;

		// Token: 0x0400406A RID: 16490
		private FlowOutput @out;
	}
}
