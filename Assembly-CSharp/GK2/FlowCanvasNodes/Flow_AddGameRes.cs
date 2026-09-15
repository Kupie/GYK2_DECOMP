using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B6B RID: 2923
	[Name("Add Gameres", 0)]
	[Category("Game/GameRes")]
	[Color("FFFFFF")]
	public class Flow_AddGameRes : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004D2F RID: 19759 RVA: 0x0016C0BC File Offset: 0x0016A2BC
		protected override void RegisterPorts()
		{
			if (!this.addToPlayer)
			{
				base.RegisterPorts();
			}
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ChangeGameRes), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.gameresName = base.AddValueInput<string>("gameresName".CapitalizeFirst(), "");
			this.gameresValue = base.AddValueInput<float>("gameresValue".CapitalizeFirst(), "");
			this.gameResNameOut = base.AddValueOutput<string>("paramName", () => this.gameresName.value, "");
		}

		// Token: 0x06004D30 RID: 19760 RVA: 0x0016C174 File Offset: 0x0016A374
		private void ChangeGameRes(Flow flow)
		{
			if (this.addToPlayer)
			{
				PlayerData playerData = MainGame.PlayerData;
				Flow_AddGameRes.GameResGiveType gameResGiveType = this.giveType;
				if (gameResGiveType != Flow_AddGameRes.GameResGiveType.Set)
				{
					if (gameResGiveType == Flow_AddGameRes.GameResGiveType.Add)
					{
						playerData.AddRes(this.gameresName.value, this.gameresValue.value);
					}
				}
				else
				{
					playerData.SetRes(this.gameresName.value, this.gameresValue.value);
				}
				if (this.gameresName.value == "cur_bodies_count" && playerData.CurrentWorldZoneData != null && playerData.CurrentWorldZoneData.Definition.id == "morgue")
				{
					GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
				}
			}
			else
			{
				WgoData wgoData = base.GetWgoData();
				if (wgoData != null)
				{
					Flow_AddGameRes.GameResGiveType gameResGiveType = this.giveType;
					if (gameResGiveType != Flow_AddGameRes.GameResGiveType.Set)
					{
						if (gameResGiveType == Flow_AddGameRes.GameResGiveType.Add)
						{
							wgoData.AddGameRes(this.gameresName.value, (int)this.gameresValue.value);
						}
					}
					else
					{
						wgoData.SetGameRes(this.gameresName.value, (int)this.gameresValue.value);
					}
					if (this.updateView)
					{
						GameScene.GetWgoViewGlobal(wgoData.UniqueId).UpdateViewAsset();
					}
				}
				else
				{
					Debug.LogError(string.Format("{0}: Tried to {1} GameRes to null WGO", "Flow_AddGameRes", this.giveType));
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06004D31 RID: 19761 RVA: 0x0016C2D0 File Offset: 0x0016A4D0
		public override string name
		{
			get
			{
				return this.giveType.ToString() + " GameRes To " + (this.addToPlayer ? "Player" : "WgoData");
			}
		}

		// Token: 0x04003DFB RID: 15867
		[FlowNode.GatherPortsCallbackAttribute]
		public bool addToPlayer;

		// Token: 0x04003DFC RID: 15868
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_AddGameRes.GameResGiveType giveType;

		// Token: 0x04003DFD RID: 15869
		[FlowNode.GatherPortsCallbackAttribute]
		[ShowIf("addToPlayer", 0)]
		public bool updateView;

		// Token: 0x04003DFE RID: 15870
		private FlowInput @in;

		// Token: 0x04003DFF RID: 15871
		private FlowOutput @out;

		// Token: 0x04003E00 RID: 15872
		private ValueInput<WgoData> wgo;

		// Token: 0x04003E01 RID: 15873
		private ValueInput<string> gameresName;

		// Token: 0x04003E02 RID: 15874
		private ValueInput<float> gameresValue;

		// Token: 0x04003E03 RID: 15875
		private ValueOutput<string> gameResNameOut;

		// Token: 0x02000B6C RID: 2924
		public enum GameResGiveType
		{
			// Token: 0x04003E05 RID: 15877
			Set,
			// Token: 0x04003E06 RID: 15878
			Add
		}
	}
}
