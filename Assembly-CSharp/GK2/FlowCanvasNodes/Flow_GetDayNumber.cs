using System;
using FlowCanvas;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BB1 RID: 2993
	[Name("Get Day Number", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_GetDayNumber : GKCustomFlowNode
	{
		// Token: 0x06004E12 RID: 19986 RVA: 0x0017078C File Offset: 0x0016E98C
		protected override void RegisterPorts()
		{
			this.selectedDay = base.AddValueInput<Flow_GetDayNumber.AllDays>("selectedDay", "");
			this.dayNumberOut = base.AddValueOutput<int>("Number", () => MainGame.Instance.GameSave.environmentData.CurrentDayNumber, "");
			this.isSelectedDayToday = base.AddValueOutput<bool>("isSelectedDayToday", () => MainGame.Instance.GameSave.environmentData.CurrentDayNumber == ConstDef.Get(this.selectedDay.value.ToString()).IntValue, "");
		}

		// Token: 0x04003F29 RID: 16169
		private ValueInput<Flow_GetDayNumber.AllDays> selectedDay;

		// Token: 0x04003F2A RID: 16170
		private ValueOutput<int> dayNumberOut;

		// Token: 0x04003F2B RID: 16171
		private ValueOutput<bool> isSelectedDayToday;

		// Token: 0x02000BB2 RID: 2994
		public enum AllDays
		{
			// Token: 0x04003F2D RID: 16173
			day_gluttony,
			// Token: 0x04003F2E RID: 16174
			day_sloth,
			// Token: 0x04003F2F RID: 16175
			day_lust,
			// Token: 0x04003F30 RID: 16176
			day_envy,
			// Token: 0x04003F31 RID: 16177
			day_pride,
			// Token: 0x04003F32 RID: 16178
			day_wrath
		}
	}
}
