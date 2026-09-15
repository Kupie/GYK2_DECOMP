using System;
using FlowCanvas;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BC1 RID: 3009
	[Name("Get WgoData Pos", 0)]
	[Category("Game/Wgo")]
	[Color("f47dff")]
	public class Flow_GetWgoDataPos : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004E41 RID: 20033 RVA: 0x001710FF File Offset: 0x0016F2FF
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.wgoDataPos = base.AddValueOutput<Vector3>("Pos", delegate
			{
				if (base.GetWgoData() != null)
				{
					return base.GetWgoData().Position;
				}
				return Vector3.zero;
			}, "");
		}

		// Token: 0x04003F69 RID: 16233
		private ValueOutput<Vector3> wgoDataPos;
	}
}
