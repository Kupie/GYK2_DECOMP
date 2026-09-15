using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B77 RID: 2935
	[Name("Body Roll", 0)]
	[Category("Game")]
	public class Flow_Body : GKCustomFlowNode
	{
		// Token: 0x06004D53 RID: 19795 RVA: 0x0016CD4C File Offset: 0x0016AF4C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAction), "");
			this.success = base.AddFlowOutput("success".CapitalizeFirst(), "");
			this.fail = base.AddFlowOutput("fail".CapitalizeFirst(), "");
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x0016CDB8 File Offset: 0x0016AFB8
		private void DoAction(Flow flow)
		{
			Debug.Log("Body Roll");
			int resInt = MainGame.PlayerData.GetResInt("cur_bodies_count");
			int num = (int)MainGame.WorldData.GetWorldZoneDataById("morgue").GetTotalQuality();
			if (resInt >= num)
			{
				MainGame.PlayerData.AddRes("donkey_body_drop_chance", this.ChanceIncreaseAfterSkip());
				Debug.Log("Skipped body drop, because morgue is full");
				this.fail.Call(flow);
				return;
			}
			if (MainGame.PlayerData.GetRes("donkey_body_drop_chance", 0f) >= (float)global::UnityEngine.Random.Range(0, 100))
			{
				MainGame.PlayerData.AddRes("donkey_body_drop_chance", this.ChanceReduceAfterSuccess());
				Debug.Log("Started body drop");
				this.success.Call(flow);
				return;
			}
			MainGame.PlayerData.AddRes("donkey_body_drop_chance", this.ChanceIncreaseAfterFail());
			Debug.Log("Skipped body drop, because roll failed");
			this.fail.Call(flow);
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x0016CE99 File Offset: 0x0016B099
		private float ChanceReduceAfterSuccess()
		{
			if (MainGame.PlayerData.GetResInt("nun_many_body_drop") == 1)
			{
				return ConstDef.Get("nun_many_drop_chance_reduce_after_success").FloatValue * -1f;
			}
			return ConstDef.Get("donkey_drop_chance_reduce_after_success").FloatValue * -1f;
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x0016CED8 File Offset: 0x0016B0D8
		private float ChanceIncreaseAfterFail()
		{
			if (MainGame.PlayerData.GetResInt("nun_many_body_drop") == 1)
			{
				return ConstDef.Get("nun_many_drop_chance_increase_after_fail").FloatValue;
			}
			return ConstDef.Get("donkey_drop_chance_increase_after_fail").FloatValue;
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x0016CF0B File Offset: 0x0016B10B
		private float ChanceIncreaseAfterSkip()
		{
			if (MainGame.PlayerData.GetResInt("nun_many_body_drop") == 1)
			{
				return ConstDef.Get("nun_many_drop_chance_increase_after_skip").FloatValue;
			}
			return ConstDef.Get("donkey_drop_chance_increase_after_skip").FloatValue;
		}

		// Token: 0x04003E39 RID: 15929
		private FlowInput @in;

		// Token: 0x04003E3A RID: 15930
		private FlowOutput success;

		// Token: 0x04003E3B RID: 15931
		private FlowOutput fail;
	}
}
