using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BF8 RID: 3064
	[Name("Set Drop Position", 0)]
	[Category("Game/Item")]
	[Description("Sets the position of a dropped item. Optionally disables its physics collision.")]
	[Color("FFFFFF")]
	public class Flow_SetDropPosition : GKCustomFlowNode
	{
		// Token: 0x06004EFD RID: 20221 RVA: 0x001746F8 File Offset: 0x001728F8
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetPosition), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.item = base.AddValueInput<Item>("item", "");
			if (this.useGdPoint)
			{
				this.gdPoint = base.AddValueInput<GDPointData>("gdPoint", "");
			}
			else
			{
				this.position = base.AddValueInput<Vector3>("position", "");
			}
			this.disableCollision = base.AddValueInput<bool>("disableCollision", "");
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x001747AC File Offset: 0x001729AC
		private void SetPosition(Flow flow)
		{
			Item value = this.item.value;
			if (value != null && !value.IsEmpty)
			{
				Vector3 vector = (this.useGdPoint ? ((this.gdPoint.value != null) ? this.gdPoint.value.Position : Vector3.zero) : this.position.value);
				DropData dropData = null;
				DropView dropView = null;
				foreach (GameSceneData gameSceneData in MainGame.Instance.GameSave.worldData.gameSceneDataList)
				{
					foreach (DropData dropData2 in gameSceneData.droppedItems)
					{
						if (dropData2.UniqueId == value.UniqueId)
						{
							dropData = dropData2;
							break;
						}
					}
					if (dropData != null)
					{
						break;
					}
				}
				if (dropData != null)
				{
					dropData.Position = vector;
					foreach (GameScene gameScene in LazySingleton<GameSceneManager>.Instance.LoadedGameScenes)
					{
						dropView = gameScene.GetDropView(value);
						if (dropView != null)
						{
							break;
						}
					}
					if (dropView != null)
					{
						if (this.disableCollision.value)
						{
							dropView.SetNonPhysicState(true);
						}
						dropView.transform.position = vector;
						Rigidbody rigidbody;
						if (dropView.TryGetComponent<Rigidbody>(out rigidbody))
						{
							rigidbody.position = vector;
							rigidbody.linearVelocity = Vector3.zero;
							rigidbody.angularVelocity = Vector3.zero;
						}
					}
					else
					{
						Debug.LogWarning(string.Format("[Flow_SetDropPosition] DropView not found for item {0}. Drop might not be spawned yet.", value.UniqueId));
					}
				}
				else
				{
					Debug.LogWarning(string.Format("[Flow_SetDropPosition] DropData not found for item {0}", value.UniqueId));
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06004EFF RID: 20223 RVA: 0x001749A8 File Offset: 0x00172BA8
		public override string name
		{
			get
			{
				return "Set Drop Position" + (this.useGdPoint ? " (GDPoint)" : "");
			}
		}

		// Token: 0x04004042 RID: 16450
		[FlowNode.GatherPortsCallbackAttribute]
		public bool useGdPoint;

		// Token: 0x04004043 RID: 16451
		private FlowInput @in;

		// Token: 0x04004044 RID: 16452
		private FlowOutput @out;

		// Token: 0x04004045 RID: 16453
		private ValueInput<Item> item;

		// Token: 0x04004046 RID: 16454
		private new ValueInput<Vector3> position;

		// Token: 0x04004047 RID: 16455
		private ValueInput<GDPointData> gdPoint;

		// Token: 0x04004048 RID: 16456
		private ValueInput<bool> disableCollision;
	}
}
