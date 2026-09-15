using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C23 RID: 3107
	[Name("Time Controller", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_TimeController : GKCustomFlowNode
	{
		// Token: 0x06004F85 RID: 20357 RVA: 0x00176BFC File Offset: 0x00174DFC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ControlTime), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (this.timeControlType != Flow_TimeController.TimeControlType.Resume && this.timeControlType != Flow_TimeController.TimeControlType.Pause && this.timeControlType != Flow_TimeController.TimeControlType.ResumeFromData)
			{
				this.isStartGameTime = base.AddValueInput<bool>("isStartGameTime".CapitalizeFirst(), "");
				this.time = base.AddValueInput<float>("time".CapitalizeFirst() + " [0;1]", "");
			}
		}

		// Token: 0x06004F86 RID: 20358 RVA: 0x00176CA8 File Offset: 0x00174EA8
		protected void ControlTime(Flow flow)
		{
			EnvironmentEngine environmentEngine = MainGame.Instance.GameSave.environmentData.EnvironmentEngine;
			switch (this.timeControlType)
			{
			case Flow_TimeController.TimeControlType.Pause:
				environmentEngine.IsPaused = true;
				break;
			case Flow_TimeController.TimeControlType.Resume:
				environmentEngine.IsPaused = false;
				break;
			case Flow_TimeController.TimeControlType.Set:
				environmentEngine.SetTimeOfDay(Mathf.Clamp(this.time.value, 0f, 1f));
				if (this.isStartGameTime.value)
				{
					MainGame.Instance.GameSave.gameLogicSystemData.PrepareForNewGame();
				}
				break;
			case Flow_TimeController.TimeControlType.SetAndPause:
				environmentEngine.SetTimeOfDay(Mathf.Clamp(this.time.value, 0f, 1f));
				environmentEngine.IsPaused = true;
				if (this.isStartGameTime.value)
				{
					MainGame.Instance.GameSave.gameLogicSystemData.PrepareForNewGame();
				}
				break;
			case Flow_TimeController.TimeControlType.SetAndResume:
				environmentEngine.SetTimeOfDay(Mathf.Clamp(this.time.value, 0f, 1f));
				environmentEngine.IsPaused = false;
				if (this.isStartGameTime.value)
				{
					MainGame.Instance.GameSave.gameLogicSystemData.PrepareForNewGame();
				}
				break;
			case Flow_TimeController.TimeControlType.SetFake:
				environmentEngine.SetTimeOfDayFake(Mathf.Clamp(this.time.value, 0f, 1f));
				break;
			case Flow_TimeController.TimeControlType.ResumeFromData:
				environmentEngine.ResumeTimeOfDayFromData();
				break;
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06004F87 RID: 20359 RVA: 0x00176E24 File Offset: 0x00175024
		public override string name
		{
			get
			{
				string text;
				switch (this.timeControlType)
				{
				case Flow_TimeController.TimeControlType.Pause:
					text = " \n<color=#fc1403>Pause</color>";
					break;
				case Flow_TimeController.TimeControlType.Resume:
					text = " \n<color=#03fc41>Resume</color>";
					break;
				case Flow_TimeController.TimeControlType.Set:
					text = " \n<color=#dbdbdb>Set</color>";
					break;
				case Flow_TimeController.TimeControlType.SetAndPause:
					text = " \n<color=#fc1403>Set & Pause</color>";
					break;
				case Flow_TimeController.TimeControlType.SetAndResume:
					text = " \n<color=#03fc41>Set & Resume</color>";
					break;
				default:
					text = base.name;
					break;
				}
				string text2 = text;
				return base.name + text2;
			}
		}

		// Token: 0x040040F6 RID: 16630
		protected FlowInput @in;

		// Token: 0x040040F7 RID: 16631
		protected FlowOutput @out;

		// Token: 0x040040F8 RID: 16632
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_TimeController.TimeControlType timeControlType;

		// Token: 0x040040F9 RID: 16633
		private ValueInput<float> time;

		// Token: 0x040040FA RID: 16634
		private ValueInput<bool> isStartGameTime;

		// Token: 0x02000C24 RID: 3108
		public enum TimeControlType
		{
			// Token: 0x040040FC RID: 16636
			Pause,
			// Token: 0x040040FD RID: 16637
			Resume,
			// Token: 0x040040FE RID: 16638
			Set,
			// Token: 0x040040FF RID: 16639
			SetAndPause,
			// Token: 0x04004100 RID: 16640
			SetAndResume,
			// Token: 0x04004101 RID: 16641
			SetFake,
			// Token: 0x04004102 RID: 16642
			ResumeFromData
		}
	}
}
