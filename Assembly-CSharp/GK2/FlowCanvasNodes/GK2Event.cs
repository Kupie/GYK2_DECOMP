using System;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C2D RID: 3117
	[Name("GK2 Event", 0)]
	[Category("Events/Custom")]
	[Description("Add event from DialogData through FlowDialog tool")]
	public class GK2Event : CustomEvent
	{
		// Token: 0x04004123 RID: 16675
		private string buttonText = "Build event";

		// Token: 0x04004124 RID: 16676
		[SerializeField]
		private bool addControlNodes = true;
	}
}
