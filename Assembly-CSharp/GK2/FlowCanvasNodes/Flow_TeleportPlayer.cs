using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C1F RID: 3103
	[Name("Teleport Player", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_TeleportPlayer : GKCustomFlowNode
	{
		// Token: 0x06004F7B RID: 20347 RVA: 0x00176978 File Offset: 0x00174B78
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Teleport), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onSceneLoaded = base.AddFlowOutput("onSceneLoaded".CapitalizeFirst(), "");
			if (this.isNeedApplyPreset)
			{
				this.presetNameToApply = base.AddValueInput<string>("presetNameToApply", "");
			}
			Flow_TeleportPlayer.TeleportDestination teleportDestination = this.teleportDestination;
			if (teleportDestination == Flow_TeleportPlayer.TeleportDestination.GDPoint)
			{
				this.gdPointData = base.AddValueInput<GDPointData>("GDPointData", "");
				return;
			}
			if (teleportDestination - Flow_TeleportPlayer.TeleportDestination.Wgo > 1)
			{
				return;
			}
			this.wgoData = base.AddValueInput<WgoData>("WgoData", "");
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x00176A40 File Offset: 0x00174C40
		private void Teleport(Flow flow)
		{
			MainGame.PlayerController.SetControlTakenType(TakenControlType.ByTeleport, false);
			string text = (this.isNeedApplyPreset ? this.presetNameToApply.value : string.Empty);
			switch (this.teleportDestination)
			{
			case Flow_TeleportPlayer.TeleportDestination.GDPoint:
			{
				GDPointData value = this.gdPointData.value;
				if (value != null)
				{
					PlayerController.Teleport(new GDPointTeleportData(value, text, "", delegate
					{
						this.onSceneLoaded.Call(flow);
					}, !this.doFade, 0.3f));
				}
				break;
			}
			case Flow_TeleportPlayer.TeleportDestination.Wgo:
			{
				WgoData value2 = this.wgoData.value;
				if (value2 != null)
				{
					PlayerController.Teleport(new WgoTeleportData(value2.id, text, "", false, delegate
					{
						this.onSceneLoaded.Call(flow);
					}, !this.doFade, 0.3f));
				}
				break;
			}
			case Flow_TeleportPlayer.TeleportDestination.DockPoint:
			{
				WgoData wgoData = base.WgoDataParamOrSelf(this.wgoData);
				if (wgoData != null)
				{
					PlayerController.Teleport(new WgoTeleportData(wgoData.id, text, "", true, delegate
					{
						this.onSceneLoaded.Call(flow);
					}, !this.doFade, 0.3f));
				}
				break;
			}
			}
			this.@out.Call(flow);
		}

		// Token: 0x040040E5 RID: 16613
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_TeleportPlayer.TeleportDestination teleportDestination;

		// Token: 0x040040E6 RID: 16614
		[FlowNode.GatherPortsCallbackAttribute]
		public bool isNeedApplyPreset;

		// Token: 0x040040E7 RID: 16615
		[FlowNode.GatherPortsCallbackAttribute]
		public bool doFade;

		// Token: 0x040040E8 RID: 16616
		private FlowInput @in;

		// Token: 0x040040E9 RID: 16617
		private FlowOutput @out;

		// Token: 0x040040EA RID: 16618
		private FlowOutput onSceneLoaded;

		// Token: 0x040040EB RID: 16619
		private ValueInput<GDPointData> gdPointData;

		// Token: 0x040040EC RID: 16620
		private ValueInput<WgoData> wgoData;

		// Token: 0x040040ED RID: 16621
		private ValueInput<string> presetNameToApply;

		// Token: 0x02000C20 RID: 3104
		public enum TeleportDestination
		{
			// Token: 0x040040EF RID: 16623
			GDPoint,
			// Token: 0x040040F0 RID: 16624
			Wgo,
			// Token: 0x040040F1 RID: 16625
			DockPoint
		}
	}
}
