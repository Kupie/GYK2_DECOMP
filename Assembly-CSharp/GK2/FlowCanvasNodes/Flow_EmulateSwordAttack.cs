using System;
using System.Collections;
using FlowCanvas;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B9A RID: 2970
	[Name("Emulate Sword Attack", 0)]
	[Category("Game/Fighting")]
	[Description("Emulates sword attack animation on a WgoData target.")]
	public class Flow_EmulateSwordAttack : GKCustomFlowNode
	{
		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06004DC0 RID: 19904 RVA: 0x0016F13F File Offset: 0x0016D33F
		public override string name
		{
			get
			{
				return string.Format("Emulate Sword Attack ({0})", this.action);
			}
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x0016F158 File Offset: 0x0016D358
		protected override void RegisterPorts()
		{
			this.targetWgo = base.AddValueInput<WgoData>("Target Wgo", "");
			if (this.action != Flow_EmulateSwordAttack.AttackAction.StopLoop)
			{
				this.delay = base.AddValueInput<float>("Delay Between Attacks", "");
			}
			this.onFinished = base.AddFlowOutput("On Finished", "");
			base.AddFlowInput("In", new FlowHandler(this.Execute), "");
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x0016F1D0 File Offset: 0x0016D3D0
		private void Execute(Flow f)
		{
			WgoData value = this.targetWgo.value;
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
			SwordAttackEmulationComponent swordAttackEmulationComponent = wgoViewGlobal.GetComponentInChildren<SwordAttackEmulationComponent>();
			if (swordAttackEmulationComponent == null)
			{
				swordAttackEmulationComponent = wgoViewGlobal.gameObject.AddComponent<SwordAttackEmulationComponent>();
				AttackComponent componentInChildren = wgoViewGlobal.GetComponentInChildren<AttackComponent>();
				AnimationComponent componentInChildren2 = wgoViewGlobal.GetComponentInChildren<AnimationComponent>();
				swordAttackEmulationComponent.Init(componentInChildren, componentInChildren2);
			}
			switch (this.action)
			{
			case Flow_EmulateSwordAttack.AttackAction.Once:
				swordAttackEmulationComponent.PerformAttack(delegate
				{
					if (this.delay.value > 0f)
					{
						this.StartCoroutine(this.DelayRoutine(this.delay.value, f));
						return;
					}
					f.Call(this.onFinished);
				});
				return;
			case Flow_EmulateSwordAttack.AttackAction.StartLoop:
				swordAttackEmulationComponent.StartLoop(this.delay.value);
				f.Call(this.onFinished);
				return;
			case Flow_EmulateSwordAttack.AttackAction.StopLoop:
				swordAttackEmulationComponent.StopLoop();
				f.Call(this.onFinished);
				return;
			default:
				return;
			}
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x0016F2D8 File Offset: 0x0016D4D8
		private IEnumerator DelayRoutine(float time, Flow f)
		{
			yield return new WaitForSeconds(time);
			f.Call(this.onFinished);
			yield break;
		}

		// Token: 0x04003EC9 RID: 16073
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_EmulateSwordAttack.AttackAction action;

		// Token: 0x04003ECA RID: 16074
		private ValueInput<WgoData> targetWgo;

		// Token: 0x04003ECB RID: 16075
		private ValueInput<float> delay;

		// Token: 0x04003ECC RID: 16076
		private FlowOutput onFinished;

		// Token: 0x02000B9B RID: 2971
		public enum AttackAction
		{
			// Token: 0x04003ECE RID: 16078
			Once,
			// Token: 0x04003ECF RID: 16079
			StartLoop,
			// Token: 0x04003ED0 RID: 16080
			StopLoop
		}
	}
}
