using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C25 RID: 3109
	[Name("Toggle Nearest Wgo Work Event", 0)]
	[Category("Game/Wgo")]
	[Color("f47dff")]
	[global::ParadoxNotion.Design.Icon("Dialogue", false, "")]
	public class Flow_ToggleNearestWgoWorkEvent : GKCustomFlowNode
	{
		// Token: 0x06004F89 RID: 20361 RVA: 0x00176E94 File Offset: 0x00175094
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Toggle), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			base.AddValueOutput<WgoData>("WgoData", new ValueHandler<WgoData>(this.GetLastTarget), "");
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x00176F00 File Offset: 0x00175100
		private void Toggle(Flow flow)
		{
			WgoData lastTarget = this.GetLastTarget();
			if (lastTarget != null && Flow_ToggleNearestWgoWorkEvent.HasWorkEvent(lastTarget))
			{
				lastTarget.RemoveInteractionEvent("work");
				this.lastTargetUniqueId = null;
			}
			else
			{
				WgoData wgoData = this.FindNearestWgo();
				if (wgoData == null)
				{
					Debug.LogWarning("Flow_ToggleNearestWgoWorkEvent: no WGO found near the player");
				}
				else
				{
					wgoData.AddInteractionEvent("work", true);
					this.lastTargetUniqueId = wgoData.UniqueId;
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x06004F8B RID: 20363 RVA: 0x00176F6D File Offset: 0x0017516D
		private WgoData GetLastTarget()
		{
			if (!(this.lastTargetUniqueId == null))
			{
				return base.WorldData.GetWgoData(this.lastTargetUniqueId);
			}
			return null;
		}

		// Token: 0x06004F8C RID: 20364 RVA: 0x00176F90 File Offset: 0x00175190
		private WgoData FindNearestWgo()
		{
			if (base.PlayerData == null || base.WorldData == null)
			{
				return null;
			}
			Vector3 value = base.PlayerData.position.Value;
			string currentGameSceneId = base.PlayerData.currentGameSceneId;
			WgoData wgoData = null;
			float num = float.MaxValue;
			foreach (WgoData wgoData2 in base.WorldData.Cache.wgoDataByUidCache.Values)
			{
				if (wgoData2 != null && !wgoData2.IsHidden && !wgoData2.isTempObject && (string.IsNullOrEmpty(currentGameSceneId) || !(wgoData2.WorldId != currentGameSceneId)))
				{
					float sqrMagnitude = (wgoData2.Position - value).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						wgoData = wgoData2;
					}
				}
			}
			return wgoData;
		}

		// Token: 0x06004F8D RID: 20365 RVA: 0x00177078 File Offset: 0x00175278
		private static bool HasWorkEvent(WgoData wgoData)
		{
			using (List<InteractionEvent>.Enumerator enumerator = wgoData.Events.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.str == "work")
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04004103 RID: 16643
		private const string WorkEventId = "work";

		// Token: 0x04004104 RID: 16644
		private FlowInput @in;

		// Token: 0x04004105 RID: 16645
		private FlowOutput @out;

		// Token: 0x04004106 RID: 16646
		private SGuid lastTargetUniqueId;
	}
}
