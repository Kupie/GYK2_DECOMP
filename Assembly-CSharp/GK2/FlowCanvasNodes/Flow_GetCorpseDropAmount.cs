using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BB0 RID: 2992
	[Name("Get Corpse Drop Amount", 0)]
	[Category("Game")]
	[Color("FFFFFF")]
	public class Flow_GetCorpseDropAmount : GKCustomFlowNode
	{
		// Token: 0x06004E0F RID: 19983 RVA: 0x001706CF File Offset: 0x0016E8CF
		protected override void RegisterPorts()
		{
			this.amount = base.AddValueOutput<int>("amount".CapitalizeFirst(), new ValueHandler<int>(this.GetAmount), "");
		}

		// Token: 0x06004E10 RID: 19984 RVA: 0x001706F8 File Offset: 0x0016E8F8
		private int GetAmount()
		{
			int num = 1;
			if (MainGame.PlayerData.GetRes("nun_many_body_drop", 0f) > 0f)
			{
				ConstDef constDef = ConstDef.Get("donkey_drop_additional");
				if (constDef != null)
				{
					num += ((constDef.IntValue != 0) ? constDef.IntValue : Mathf.RoundToInt(constDef.FloatValue));
				}
			}
			int resInt = MainGame.PlayerData.GetResInt("cur_bodies_count");
			int num2 = (int)MainGame.WorldData.GetWorldZoneDataById("morgue").GetTotalQuality();
			int num3 = Mathf.Max(1, num2 - resInt);
			return Mathf.Min(num, num3);
		}

		// Token: 0x04003F28 RID: 16168
		private ValueOutput<int> amount;
	}
}
