using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BBC RID: 3004
	[Name("Get Supplies Cost", 0)]
	[Category("Game")]
	public class Flow_GetSuppliesCost : GKCustomFlowNode
	{
		// Token: 0x06004E38 RID: 20024 RVA: 0x00170EFC File Offset: 0x0016F0FC
		protected override void RegisterPorts()
		{
			this.smartRes = base.AddValueOutput<SmartRes>("smartRes".CapitalizeFirst(), delegate
			{
				SmartRes smartRes = new SmartRes
				{
					gameRes = new GameRes()
				};
				int num = 0;
				if (this.suppliesType == Flow_GetSuppliesCost.SuppliesType.SpecialWood)
				{
					switch (this.suppliesCount)
					{
					case Flow_GetSuppliesCost.SuppliesCount.X1:
						num = ConstDef.Get("special_wood_cost_x1").IntValue;
						break;
					case Flow_GetSuppliesCost.SuppliesCount.X10:
						num = ConstDef.Get("special_wood_cost_x10").IntValue;
						break;
					case Flow_GetSuppliesCost.SuppliesCount.X50:
						num = ConstDef.Get("special_wood_cost_x50").IntValue;
						break;
					case Flow_GetSuppliesCost.SuppliesCount.X100:
						num = ConstDef.Get("special_wood_cost_x100").IntValue;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
				else if (this.suppliesType == Flow_GetSuppliesCost.SuppliesType.Steel)
				{
					switch (this.suppliesCount)
					{
					case Flow_GetSuppliesCost.SuppliesCount.X1:
						num = ConstDef.Get("steel_cost_x1").IntValue;
						break;
					case Flow_GetSuppliesCost.SuppliesCount.X10:
						num = ConstDef.Get("steel_cost_x10").IntValue;
						break;
					case Flow_GetSuppliesCost.SuppliesCount.X50:
						num = ConstDef.Get("steel_cost_x50").IntValue;
						break;
					case Flow_GetSuppliesCost.SuppliesCount.X100:
						num = ConstDef.Get("steel_cost_x100").IntValue;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
				smartRes.gameRes.Add(new GameResAtom("money", num));
				return smartRes;
			}, "");
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06004E39 RID: 20025 RVA: 0x00170F28 File Offset: 0x0016F128
		public override string name
		{
			get
			{
				return string.Concat(new string[]
				{
					"Get ",
					this.suppliesType.ToString(),
					" ",
					this.suppliesCount.ToString(),
					" Supplies Cost"
				});
			}
		}

		// Token: 0x04003F5B RID: 16219
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_GetSuppliesCost.SuppliesType suppliesType;

		// Token: 0x04003F5C RID: 16220
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_GetSuppliesCost.SuppliesCount suppliesCount;

		// Token: 0x04003F5D RID: 16221
		private ValueOutput<SmartRes> smartRes;

		// Token: 0x02000BBD RID: 3005
		public enum SuppliesType
		{
			// Token: 0x04003F5F RID: 16223
			SpecialWood,
			// Token: 0x04003F60 RID: 16224
			Steel
		}

		// Token: 0x02000BBE RID: 3006
		public enum SuppliesCount
		{
			// Token: 0x04003F62 RID: 16226
			X1,
			// Token: 0x04003F63 RID: 16227
			X10,
			// Token: 0x04003F64 RID: 16228
			X50,
			// Token: 0x04003F65 RID: 16229
			X100
		}
	}
}
