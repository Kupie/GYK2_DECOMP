using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B97 RID: 2967
	[Name("Emulate Bow Attack", 0)]
	[Category("Game/Fighting")]
	[Description("Emulates bow attack animation on a WgoData target using random targets list.")]
	public class Flow_EmulateBowAttack : GKCustomFlowNode
	{
		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06004DBA RID: 19898 RVA: 0x0016EF3F File Offset: 0x0016D13F
		public override string name
		{
			get
			{
				return string.Format("Emulate Bow Attack ({0})", this.action);
			}
		}

		// Token: 0x06004DBB RID: 19899 RVA: 0x0016EF58 File Offset: 0x0016D158
		protected override void RegisterPorts()
		{
			this.attackerWgo = base.AddValueInput<WgoData>("Attacker Wgo", "");
			this.targets = base.AddValueInput<List<WgoData>>("Targets", "");
			if (this.action != Flow_EmulateBowAttack.AttackAction.StopLoop)
			{
				this.delay = base.AddValueInput<float>("Delay Between Attacks", "");
			}
			this.onFinished = base.AddFlowOutput("On Finished", "");
			base.AddFlowInput("In", new FlowHandler(this.Execute), "");
		}

		// Token: 0x06004DBC RID: 19900 RVA: 0x0016EFE4 File Offset: 0x0016D1E4
		private void Execute(Flow f)
		{
			WgoData value = this.attackerWgo.value;
			if (value == null)
			{
				f.Call(this.onFinished);
				return;
			}
			Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(value.UniqueId);
			if (wgoViewGlobal == null)
			{
				f.Call(this.onFinished);
				return;
			}
			BowAttackEmulationComponent bowAttackEmulationComponent = wgoViewGlobal.GetComponentInChildren<BowAttackEmulationComponent>();
			if (bowAttackEmulationComponent == null)
			{
				bowAttackEmulationComponent = wgoViewGlobal.gameObject.AddComponent<BowAttackEmulationComponent>();
			}
			AttackComponent componentInChildren = wgoViewGlobal.GetComponentInChildren<AttackComponent>();
			AnimationComponent componentInChildren2 = wgoViewGlobal.GetComponentInChildren<AnimationComponent>();
			if (componentInChildren == null || componentInChildren2 == null)
			{
				Debug.LogError("AttackComponent or AnimationComponent not found");
				return;
			}
			bowAttackEmulationComponent.Init(componentInChildren, componentInChildren2);
			bowAttackEmulationComponent.SetTargets(this.targets.value);
			switch (this.action)
			{
			case Flow_EmulateBowAttack.AttackAction.Once:
				bowAttackEmulationComponent.PerformAttack(this.delay.value, delegate
				{
					f.Call(this.onFinished);
				});
				return;
			case Flow_EmulateBowAttack.AttackAction.StartLoop:
				bowAttackEmulationComponent.StartLoop(this.delay.value);
				f.Call(this.onFinished);
				return;
			case Flow_EmulateBowAttack.AttackAction.StopLoop:
				bowAttackEmulationComponent.StopLoop();
				f.Call(this.onFinished);
				return;
			default:
				return;
			}
		}

		// Token: 0x04003EBE RID: 16062
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_EmulateBowAttack.AttackAction action;

		// Token: 0x04003EBF RID: 16063
		private ValueInput<WgoData> attackerWgo;

		// Token: 0x04003EC0 RID: 16064
		private ValueInput<List<WgoData>> targets;

		// Token: 0x04003EC1 RID: 16065
		private ValueInput<float> delay;

		// Token: 0x04003EC2 RID: 16066
		private FlowOutput onFinished;

		// Token: 0x02000B98 RID: 2968
		public enum AttackAction
		{
			// Token: 0x04003EC4 RID: 16068
			Once,
			// Token: 0x04003EC5 RID: 16069
			StartLoop,
			// Token: 0x04003EC6 RID: 16070
			StopLoop
		}
	}
}
