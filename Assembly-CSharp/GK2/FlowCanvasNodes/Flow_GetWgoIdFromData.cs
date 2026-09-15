using System;
using FlowCanvas;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BC2 RID: 3010
	[Name("Get WGO Id From Data", 0)]
	[Category("Game/Wgo")]
	[Color("f47dff")]
	public class Flow_GetWgoIdFromData : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004E44 RID: 20036 RVA: 0x00171144 File Offset: 0x0016F344
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.wgoId = base.AddValueOutput<string>("wgoId", () => base.GetWgoData().id, "");
			this.wgoCustomTag = base.AddValueOutput<string>("wgoCustomTag", () => base.GetWgoData().CustomTag, "");
		}

		// Token: 0x04003F6A RID: 16234
		private new ValueOutput<string> wgoId;

		// Token: 0x04003F6B RID: 16235
		private ValueOutput<string> wgoCustomTag;
	}
}
