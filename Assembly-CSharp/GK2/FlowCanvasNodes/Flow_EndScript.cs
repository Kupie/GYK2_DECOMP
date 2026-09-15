using System;
using FlowCanvas;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BA2 RID: 2978
	[Name("End Script", 0)]
	[Category("Game/Script")]
	[Icon("Assets/GFX/ParadoxNotionCustomIcons/RedCross.png", false, "")]
	[Color("ff5c5c")]
	public class Flow_EndScript : GKCustomFlowNode
	{
		// Token: 0x06004DD9 RID: 19929 RVA: 0x0016F65E File Offset: 0x0016D85E
		protected override void RegisterPorts()
		{
			base.AddFlowInput("In", delegate(Flow f)
			{
				this.TerminateScript();
			}, "");
		}
	}
}
