using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B95 RID: 2965
	[Name("Drop Zombie", 0)]
	[Category("Game/Item")]
	[Color("FFFFFF")]
	public class Flow_DropZombie : GKCustomFlowNode
	{
		// Token: 0x06004DB7 RID: 19895 RVA: 0x0016ED54 File Offset: 0x0016CF54
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Drop), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.bodyId = base.AddValueInput<string>("bodyId", "");
			switch (this.dropTargetType)
			{
			case Flow_DropZombie.DropTargetType.Player:
				break;
			case Flow_DropZombie.DropTargetType.Wgo:
				this.wgoDataDropOn = base.AddValueInput<WgoData>("wgoDataDropOn", "");
				return;
			case Flow_DropZombie.DropTargetType.GDPoint:
				this.gdPointDataDropOn = base.AddValueInput<GDPointData>("gdPointDataDropOn", "");
				break;
			default:
				return;
			}
		}

		// Token: 0x06004DB8 RID: 19896 RVA: 0x0016EE00 File Offset: 0x0016D000
		private void Drop(Flow flow)
		{
			Vector3 vector = Vector3.zero;
			string text = string.Empty;
			bool flag = false;
			switch (this.dropTargetType)
			{
			case Flow_DropZombie.DropTargetType.Player:
			{
				Vector2 vector2 = MainGame.PlayerData.Direction * 1f;
				vector = MainGame.PlayerController.PlayerData.position.Value + new Vector3(vector2.x, 0f, vector2.y);
				text = MainGame.PlayerData.currentGameSceneId;
				flag = true;
				break;
			}
			case Flow_DropZombie.DropTargetType.Wgo:
			{
				WgoData wgoData;
				if (base.TryGetParamValue<WgoData>(this.wgoDataDropOn, out wgoData))
				{
					vector = wgoData.Position;
					text = wgoData.WorldId;
					flag = true;
				}
				break;
			}
			case Flow_DropZombie.DropTargetType.GDPoint:
			{
				GDPointData gdpointData;
				if (base.TryGetParamValue<GDPointData>(this.gdPointDataDropOn, out gdpointData))
				{
					vector = gdpointData.Position;
					text = gdpointData.GameSceneDataId;
					flag = true;
				}
				break;
			}
			}
			if (flag)
			{
				Item item = GameBalance.Me.GetData<BodyDef>(this.bodyId.value).GenerateItem();
				MainGame.ZombieSystemData.CreateZombieDrop("zombie", vector, text, item, "collar_bronze", null, true);
				MainGame.Instance.dropSystem.DropItem(item, text, vector, null);
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003EB4 RID: 16052
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_DropZombie.DropTargetType dropTargetType = Flow_DropZombie.DropTargetType.Wgo;

		// Token: 0x04003EB5 RID: 16053
		private FlowInput @in;

		// Token: 0x04003EB6 RID: 16054
		private FlowOutput @out;

		// Token: 0x04003EB7 RID: 16055
		private ValueInput<string> bodyId;

		// Token: 0x04003EB8 RID: 16056
		private ValueInput<WgoData> wgoDataDropOn;

		// Token: 0x04003EB9 RID: 16057
		private ValueInput<GDPointData> gdPointDataDropOn;

		// Token: 0x02000B96 RID: 2966
		public enum DropTargetType
		{
			// Token: 0x04003EBB RID: 16059
			Player,
			// Token: 0x04003EBC RID: 16060
			Wgo,
			// Token: 0x04003EBD RID: 16061
			GDPoint
		}
	}
}
