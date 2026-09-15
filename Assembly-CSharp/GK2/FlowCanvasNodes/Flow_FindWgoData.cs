using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BA7 RID: 2983
	[Name("Find WgoData", 0)]
	[Category("Game/Wgo")]
	[Color("f47dff")]
	public class Flow_FindWgoData : GKCustomFlowNode
	{
		// Token: 0x06004DEE RID: 19950 RVA: 0x0016FE2C File Offset: 0x0016E02C
		protected override void RegisterPorts()
		{
			if (!this.getList)
			{
				this.wgoId = base.AddValueInput<string>("wgoId", "");
				this.wgoData = base.AddValueOutput<WgoData>("wgoData", new ValueHandler<WgoData>(this.TryGetWgoData), "");
			}
			else
			{
				this.wgoDataList = base.AddValueOutput<List<WgoData>>("wgoDataList", new ValueHandler<List<WgoData>>(this.TryGetWgoDataList), "");
			}
			this.customTag = base.AddValueInput<string>("customTag", "");
			if (this.displayFoundStateOuput)
			{
				this.wasFound = base.AddValueOutput<bool>("wasFound", new ValueHandler<bool>(this.WasFound), "");
			}
		}

		// Token: 0x06004DEF RID: 19951 RVA: 0x0016FEE0 File Offset: 0x0016E0E0
		private bool WasFound()
		{
			if (!this.getList)
			{
				WgoData wgoDataByCustomTag = MainGame.Instance.GameSave.worldData.GetWgoData(this.wgoId.value);
				if (wgoDataByCustomTag == null)
				{
					wgoDataByCustomTag = MainGame.Instance.GameSave.worldData.GetWgoDataByCustomTag(this.customTag.value);
				}
				return wgoDataByCustomTag != null;
			}
			List<WgoData> wgoDataListByCustomTag = MainGame.Instance.GameSave.worldData.GetWgoDataListByCustomTag(this.customTag.value);
			return wgoDataListByCustomTag != null && wgoDataListByCustomTag.Count > 0;
		}

		// Token: 0x06004DF0 RID: 19952 RVA: 0x0016FF6C File Offset: 0x0016E16C
		private WgoData TryGetWgoData()
		{
			WgoData wgoDataByCustomTag = MainGame.Instance.GameSave.worldData.GetWgoData(this.wgoId.value);
			if (wgoDataByCustomTag == null)
			{
				wgoDataByCustomTag = MainGame.Instance.GameSave.worldData.GetWgoDataByCustomTag(this.customTag.value);
				if (wgoDataByCustomTag == null)
				{
					Debug.LogError("Flow_FindWgoData: WGO [" + this.wgoId.value + "] is null");
				}
			}
			return wgoDataByCustomTag;
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x0016FFE0 File Offset: 0x0016E1E0
		private List<WgoData> TryGetWgoDataList()
		{
			List<WgoData> wgoDataListByCustomTag = MainGame.Instance.GameSave.worldData.GetWgoDataListByCustomTag(this.customTag.value);
			if (wgoDataListByCustomTag == null)
			{
				Debug.LogError("Flow_FindWgoData: WGO [" + this.wgoId.value + "] is null");
			}
			return wgoDataListByCustomTag;
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x06004DF2 RID: 19954 RVA: 0x0017002E File Offset: 0x0016E22E
		public override string name
		{
			get
			{
				return "Find WgoData" + (this.getList ? " List By Tag" : "");
			}
		}

		// Token: 0x04003F02 RID: 16130
		[FlowNode.GatherPortsCallbackAttribute]
		public bool getList;

		// Token: 0x04003F03 RID: 16131
		[FlowNode.GatherPortsCallbackAttribute]
		public bool displayFoundStateOuput;

		// Token: 0x04003F04 RID: 16132
		private ValueInput<string> wgoId;

		// Token: 0x04003F05 RID: 16133
		private ValueInput<string> customTag;

		// Token: 0x04003F06 RID: 16134
		private ValueOutput<WgoData> wgoData;

		// Token: 0x04003F07 RID: 16135
		private ValueOutput<bool> wasFound;

		// Token: 0x04003F08 RID: 16136
		private ValueOutput<List<WgoData>> wgoDataList;
	}
}
