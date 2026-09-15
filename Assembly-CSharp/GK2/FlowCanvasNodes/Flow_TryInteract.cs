using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C27 RID: 3111
	[Name("Try Interact", 0)]
	[Category("Game/Player")]
	[Description("Attempts to perform the primary interaction with the object currently under the player's focus.")]
	[Color("313c8f")]
	public class Flow_TryInteract : GKCustomFlowNode
	{
		// Token: 0x06004F92 RID: 20370 RVA: 0x001771BC File Offset: 0x001753BC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.TryInteract), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onSuccess = base.AddFlowOutput("onSuccess".CapitalizeFirst(), "");
			this.onFail = base.AddFlowOutput("onFail".CapitalizeFirst(), "");
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x00177244 File Offset: 0x00175444
		private void TryInteract(Flow flow)
		{
			PlayerController playerController = MainGame.PlayerController;
			PlayerInteractionComponent playerInteractionComponent = playerController.PlayerInteractionComponent;
			bool flag = false;
			DropView dropView = playerInteractionComponent.BigDropUnderInteraction;
			if (dropView == null)
			{
				BoxCollider boxCollider;
				if (playerInteractionComponent.TryGetComponent<BoxCollider>(out boxCollider))
				{
					boxCollider.enabled = true;
				}
				dropView = playerInteractionComponent.TryGetClosestInteractionTarget();
				if (boxCollider)
				{
					boxCollider.enabled = false;
				}
			}
			if (dropView != null && dropView.InteractionHandler.HasInteraction())
			{
				dropView.InteractionHandler.Interact();
				flag = true;
			}
			else if (playerInteractionComponent.WgoUnderInteraction != null)
			{
				IWGOInteractionHandler interactionHandler = playerInteractionComponent.WgoUnderInteraction.InteractionHandler;
				WgoData data = playerInteractionComponent.WgoUnderInteraction.Data;
				if (data.Events.Count > 0)
				{
					if (interactionHandler.HasInteraction() || interactionHandler.HasInteraction2())
					{
						if (interactionHandler.HasInteraction())
						{
							flag = data.FireInteractionEvent();
						}
					}
					else
					{
						flag = data.FireInteractionEvent();
					}
				}
				if (!flag && interactionHandler.HasInteraction())
				{
					flag = interactionHandler.Interact(playerController);
				}
			}
			this.@out.Call(flow);
			if (flag)
			{
				this.onSuccess.Call(flow);
				return;
			}
			this.onFail.Call(flow);
		}

		// Token: 0x0400410B RID: 16651
		private FlowInput @in;

		// Token: 0x0400410C RID: 16652
		private FlowOutput @out;

		// Token: 0x0400410D RID: 16653
		private FlowOutput onSuccess;

		// Token: 0x0400410E RID: 16654
		private FlowOutput onFail;
	}
}
