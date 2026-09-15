using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BAF RID: 2991
	[Name("Get Const Definition Value", 0)]
	[Category("Player")]
	public class Flow_GetConstDefinitionValue : GKCustomFlowNode
	{
		// Token: 0x06004E09 RID: 19977 RVA: 0x00170584 File Offset: 0x0016E784
		protected override void RegisterPorts()
		{
			this.definitionId = base.AddValueInput<string>("definitionId".CapitalizeFirst(), "");
			switch (this.constType)
			{
			case ConstDef.ConstType.@bool:
				this.boolValue = base.AddValueOutput<bool>("boolValue".CapitalizeFirst(), () => ConstDef.Get(this.definitionId.value).BoolValue, "");
				return;
			case ConstDef.ConstType.@int:
				this.intValue = base.AddValueOutput<int>("intValue".CapitalizeFirst(), () => ConstDef.Get(this.definitionId.value).IntValue, "");
				return;
			case ConstDef.ConstType.@float:
				this.floatValue = base.AddValueOutput<float>("floatValue".CapitalizeFirst(), () => ConstDef.Get(this.definitionId.value).FloatValue, "");
				return;
			case ConstDef.ConstType.@string:
				this.stringValue = base.AddValueOutput<string>("stringValue".CapitalizeFirst(), () => ConstDef.Get(this.definitionId.value).StringValue, "");
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x04003F22 RID: 16162
		[FlowNode.GatherPortsCallbackAttribute]
		public ConstDef.ConstType constType;

		// Token: 0x04003F23 RID: 16163
		private ValueInput<string> definitionId;

		// Token: 0x04003F24 RID: 16164
		private ValueOutput<bool> boolValue;

		// Token: 0x04003F25 RID: 16165
		private ValueOutput<int> intValue;

		// Token: 0x04003F26 RID: 16166
		private ValueOutput<float> floatValue;

		// Token: 0x04003F27 RID: 16167
		private ValueOutput<string> stringValue;
	}
}
