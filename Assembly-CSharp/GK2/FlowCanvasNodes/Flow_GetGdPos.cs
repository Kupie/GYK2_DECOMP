using System;
using FlowCanvas;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BB6 RID: 2998
	[Name("Get GD Point Data Position", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_GetGdPos : GKCustomFlowNode
	{
		// Token: 0x06004E25 RID: 20005 RVA: 0x00170BDC File Offset: 0x0016EDDC
		protected override void RegisterPorts()
		{
			this.gdPointDataValIn = base.AddValueInput<GDPointData>("GDPointData", "");
			switch (this.infoType)
			{
			case Flow_GetGdPos.InfoType.Position:
				this.posValOut = base.AddValueOutput<Vector3>("Pos", delegate
				{
					GDPointData value = this.gdPointDataValIn.value;
					if (value == null)
					{
						return Vector3.zero;
					}
					return value.Position;
				}, "");
				return;
			case Flow_GetGdPos.InfoType.Direction:
				this.direction = base.AddValueOutput<Direction>("Direction", () => this.gdPointDataValIn.value.Direction, "");
				return;
			case Flow_GetGdPos.InfoType.Id:
				this.gdPointId = base.AddValueOutput<string>("Id", () => this.gdPointDataValIn.value.Id, "");
				return;
			default:
				return;
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06004E26 RID: 20006 RVA: 0x00170C81 File Offset: 0x0016EE81
		public override string name
		{
			get
			{
				return "Get GD Point Data" + ((this.infoType == Flow_GetGdPos.InfoType.Id) ? " Id" : ((this.infoType == Flow_GetGdPos.InfoType.Direction) ? " Direction" : " Pos"));
			}
		}

		// Token: 0x04003F49 RID: 16201
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_GetGdPos.InfoType infoType;

		// Token: 0x04003F4A RID: 16202
		private ValueInput<GDPointData> gdPointDataValIn;

		// Token: 0x04003F4B RID: 16203
		private ValueOutput<Vector3> posValOut;

		// Token: 0x04003F4C RID: 16204
		private ValueOutput<Direction> direction;

		// Token: 0x04003F4D RID: 16205
		private ValueOutput<string> gdPointId;

		// Token: 0x02000BB7 RID: 2999
		public enum InfoType
		{
			// Token: 0x04003F4F RID: 16207
			Position,
			// Token: 0x04003F50 RID: 16208
			Direction,
			// Token: 0x04003F51 RID: 16209
			Id
		}
	}
}
