using System;
using System.Collections.Generic;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BA6 RID: 2982
	[Name("Find GD Point Data", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_FindGdPoint : GKCustomFlowNode
	{
		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x06004DE5 RID: 19941 RVA: 0x0016F96C File Offset: 0x0016DB6C
		public override string name
		{
			get
			{
				return "Find GD Point" + ((!this.getView) ? " Data" : "") + (this.getList ? " List" : "") + (this.findByCustomTag ? " By Tag" : "");
			}
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x0016F9C0 File Offset: 0x0016DBC0
		protected override void RegisterPorts()
		{
			if (!this.findByCustomTag)
			{
				this.gdPointId = base.AddValueInput<string>("id", "");
			}
			else
			{
				this.gdPointCustomTag = base.AddValueInput<string>("customTag", "");
			}
			if (!this.getList)
			{
				this.gdPointData = base.AddValueOutput<GDPointData>("gdPointData", new ValueHandler<GDPointData>(this.FindGdPointData), "");
				if (this.getView)
				{
					this.gdPoint = base.AddValueOutput<GDPoint>("gdPoint", new ValueHandler<GDPoint>(this.FindGdPoint), "");
				}
			}
			else
			{
				this.gdPointDataList = base.AddValueOutput<List<GDPointData>>("gdPointDataList", new ValueHandler<List<GDPointData>>(this.FindGdPointsData), "");
				if (this.getView)
				{
					this.gdPointList = base.AddValueOutput<List<GDPoint>>("gdPointList", new ValueHandler<List<GDPoint>>(this.FindGdPoints), "");
				}
			}
			if (!this.getList)
			{
				this.gdPointPosition = base.AddValueOutput<Vector3>("pos", delegate
				{
					if (this.foundGdPointData != null)
					{
						return this.foundGdPointData.Position;
					}
					this.FindGdPointData();
					if (this.foundGdPointData == null)
					{
						return default(Vector3);
					}
					return this.foundGdPointData.Position;
				}, "");
			}
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x0016FAD0 File Offset: 0x0016DCD0
		private GDPointData FindGdPointData()
		{
			this.foundGdPointData = ((!this.findByCustomTag) ? MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.gdPointId.value) : MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataByCustomTag(this.gdPointCustomTag.value));
			if (this.foundGdPointData == null)
			{
				Debug.LogError("[Flow_FindGdPoint]: not found GDPointData by " + this.GetIdOrCustomTagLog());
			}
			return this.foundGdPointData;
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x0016FB58 File Offset: 0x0016DD58
		private GDPoint FindGdPoint()
		{
			foreach (GameScene gameScene in LazySingleton<GameSceneManager>.Instance.LoadedGameScenes)
			{
				foreach (GDPoint gdpoint in gameScene.SceneGdPoints)
				{
					if (!this.findByCustomTag)
					{
						if (gdpoint.Id == this.gdPointId.value)
						{
							return gdpoint;
						}
						if (gdpoint.CustomTag == this.gdPointCustomTag.value)
						{
							return gdpoint;
						}
					}
				}
			}
			Debug.LogError("[Flow_FindGdPoint]: not found GDPoint by " + this.GetIdOrCustomTagLog());
			return null;
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x0016FC1C File Offset: 0x0016DE1C
		private List<GDPointData> FindGdPointsData()
		{
			this.foundGdPointDataList = ((!this.findByCustomTag) ? MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointsDataById(this.gdPointId.value) : MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointsDataByCustomTag(this.gdPointCustomTag.value));
			if (this.foundGdPointDataList.Count == 0)
			{
				Debug.LogError("[Flow_FindGdPoint]: not found GDPointsData by " + this.GetIdOrCustomTagLog());
			}
			return this.foundGdPointDataList;
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x0016FCAC File Offset: 0x0016DEAC
		private List<GDPoint> FindGdPoints()
		{
			List<GDPoint> list = new List<GDPoint>();
			foreach (GameScene gameScene in LazySingleton<GameSceneManager>.Instance.LoadedGameScenes)
			{
				foreach (GDPoint gdpoint in gameScene.SceneGdPoints)
				{
					if (!this.findByCustomTag)
					{
						if (gdpoint.Id == this.gdPointId.value)
						{
							list.Add(gdpoint);
						}
					}
					else if (gdpoint.CustomTag == this.gdPointCustomTag.value)
					{
						list.Add(gdpoint);
					}
				}
			}
			if (list.Count == 0)
			{
				Debug.LogError("[Flow_FindGdPoint]: not found GDPoints by " + this.GetIdOrCustomTagLog());
			}
			return list;
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x0016FD84 File Offset: 0x0016DF84
		private string GetIdOrCustomTagLog()
		{
			if (!this.findByCustomTag)
			{
				return "ID: [" + this.gdPointId.value + "]";
			}
			return "customTag: [" + this.gdPointCustomTag.value + "]";
		}

		// Token: 0x04003EF4 RID: 16116
		[FlowNode.GatherPortsCallbackAttribute]
		public bool getList;

		// Token: 0x04003EF5 RID: 16117
		[FlowNode.GatherPortsCallbackAttribute]
		public bool getView;

		// Token: 0x04003EF6 RID: 16118
		[FlowNode.GatherPortsCallbackAttribute]
		public bool findByCustomTag;

		// Token: 0x04003EF7 RID: 16119
		private ValueInput<string> gdPointId;

		// Token: 0x04003EF8 RID: 16120
		private ValueInput<string> gdPointCustomTag;

		// Token: 0x04003EF9 RID: 16121
		private ValueOutput<GDPointData> gdPointData;

		// Token: 0x04003EFA RID: 16122
		private ValueOutput<GDPoint> gdPoint;

		// Token: 0x04003EFB RID: 16123
		private ValueOutput<List<GDPointData>> gdPointDataList;

		// Token: 0x04003EFC RID: 16124
		private ValueOutput<List<GDPoint>> gdPointList;

		// Token: 0x04003EFD RID: 16125
		private ValueOutput<Vector3> gdPointPosition;

		// Token: 0x04003EFE RID: 16126
		private GDPointData foundGdPointData;

		// Token: 0x04003EFF RID: 16127
		private GDPoint foundGdPoint;

		// Token: 0x04003F00 RID: 16128
		private List<GDPointData> foundGdPointDataList = new List<GDPointData>();

		// Token: 0x04003F01 RID: 16129
		private List<GDPoint> foundGdPointList = new List<GDPoint>();
	}
}
