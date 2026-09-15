using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BC9 RID: 3017
	[Name("Go To Sleep", 0)]
	[Category("Game/Environment")]
	public class Flow_GoToSleep : GKCustomFlowNode
	{
		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06004E5E RID: 20062 RVA: 0x001716B5 File Offset: 0x0016F8B5
		public bool IsWgoDataConnected
		{
			get
			{
				return this.wgoData != null && this.wgoData.isConnected;
			}
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x001716CC File Offset: 0x0016F8CC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.GoToSleep), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onFinished = base.AddFlowOutput("onFinished".CapitalizeFirst(), "");
			if (!this.sleepWithMaxEnergy)
			{
				this.onFinishedIfNoNeedToSleep = base.AddFlowOutput("onFinishedIfNoNeedToSleep".CapitalizeFirst(), "");
			}
			this.wgoData = base.AddValueInput<WgoData>("WgoData", "");
		}

		// Token: 0x06004E60 RID: 20064 RVA: 0x00171770 File Offset: 0x0016F970
		private void GoToSleep(Flow flow)
		{
			MainGame.PlayerData.energySystem.StartSleeping(delegate
			{
				this.onFinished.Call(flow);
			}, delegate
			{
				this.onFinishedIfNoNeedToSleep.Call(flow);
			}, this.sleepWithoutSavingGame, this.sleepWithMaxEnergy, this.ResolveAnimType(), this.sleepDuration);
			this.@out.Call(flow);
		}

		// Token: 0x06004E61 RID: 20065 RVA: 0x001717E4 File Offset: 0x0016F9E4
		private SleepAnimType ResolveAnimType()
		{
			if (!this.IsWgoDataConnected)
			{
				return this.animType;
			}
			WgoData value = this.wgoData.value;
			if (value == null)
			{
				Debug.LogError("Flow_GoToSleep: WgoData is connected but null");
				return this.animType;
			}
			if (value.GetGameResInt("bed_upgrade") < 1)
			{
				return SleepAnimType.StrawBed;
			}
			return SleepAnimType.RegularBed;
		}

		// Token: 0x04003F81 RID: 16257
		private const string BED_UPGRADE_GAME_RES = "bed_upgrade";

		// Token: 0x04003F82 RID: 16258
		[FlowNode.GatherPortsCallbackAttribute]
		public bool sleepWithoutSavingGame;

		// Token: 0x04003F83 RID: 16259
		[FlowNode.GatherPortsCallbackAttribute]
		public bool sleepWithMaxEnergy;

		// Token: 0x04003F84 RID: 16260
		[FlowNode.GatherPortsCallbackAttribute]
		[ShowIf("IsWgoDataConnected", 0)]
		public SleepAnimType animType;

		// Token: 0x04003F85 RID: 16261
		[FlowNode.GatherPortsCallbackAttribute]
		public float sleepDuration = 1.5f;

		// Token: 0x04003F86 RID: 16262
		private FlowInput @in;

		// Token: 0x04003F87 RID: 16263
		private FlowOutput @out;

		// Token: 0x04003F88 RID: 16264
		private FlowOutput onFinished;

		// Token: 0x04003F89 RID: 16265
		private FlowOutput onFinishedIfNoNeedToSleep;

		// Token: 0x04003F8A RID: 16266
		private ValueInput<WgoData> wgoData;
	}
}
