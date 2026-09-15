using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B8D RID: 2957
	[Name("Disable UI (HUD)", 0)]
	[Category("Game/UI")]
	public class Flow_DisableUI : GKCustomFlowNode
	{
		// Token: 0x06004D9F RID: 19871 RVA: 0x0016E144 File Offset: 0x0016C344
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoFade), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.enable = base.AddValueInput<bool>("enable?", "");
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x0016E1AC File Offset: 0x0016C3AC
		private void DoFade(Flow flow)
		{
			GUIElements.Instance.SetVisibilityState(this.enable.value);
			if (this.toggleInfoWidget)
			{
				UIInfoWidget.IsDisabled = !this.enable.value;
				if (UIInfoWidget.IsDisabled)
				{
					UIInfoWidget[] array = global::UnityEngine.Object.FindObjectsByType<UIInfoWidget>(FindObjectsInactive.Include, FindObjectsSortMode.None);
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Hide();
					}
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06004DA1 RID: 19873 RVA: 0x0016E21A File Offset: 0x0016C41A
		public override string name
		{
			get
			{
				if (this.enable.value)
				{
					return "Enable UI (HUD)";
				}
				return "Disable UI (HUD)";
			}
		}

		// Token: 0x04003E8D RID: 16013
		[FlowNode.GatherPortsCallbackAttribute]
		public bool toggleInfoWidget;

		// Token: 0x04003E8E RID: 16014
		private FlowInput @in;

		// Token: 0x04003E8F RID: 16015
		private FlowOutput @out;

		// Token: 0x04003E90 RID: 16016
		private ValueInput<bool> enable;
	}
}
