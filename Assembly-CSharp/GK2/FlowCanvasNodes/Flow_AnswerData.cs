using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B75 RID: 2933
	[Name("Answer Data", 0)]
	[Category("Game/Dialogue")]
	[Color("40addb")]
	public class Flow_AnswerData : GKCustomFlowNode
	{
		// Token: 0x06004D4C RID: 19788 RVA: 0x0016CA6C File Offset: 0x0016AC6C
		protected override void RegisterPorts()
		{
			this.locks = base.AddValueInput<SmartRes>("locks".CapitalizeFirst(), "");
			this.costs = base.AddValueInput<SmartRes>("costs".CapitalizeFirst(), "");
			this.rewards = base.AddValueInput<SmartRes>("rewards".CapitalizeFirst(), "");
			this.fakeRewards = base.AddValueInput<SmartRes>("fakeRewards".CapitalizeFirst(), "");
			this.dayNumber = base.AddValueInput<string>("dayNumber".CapitalizeFirst(), "");
			this.order = base.AddValueInput<string>("order".CapitalizeFirst(), "");
			this.customHideCondition = base.AddValueInput<bool>("customHideCondition".CapitalizeFirst(), "");
			this.answerData = base.AddValueOutput<AnswerData>("answerData".CapitalizeFirst(), delegate
			{
				AnswerData answerData = new AnswerData();
				ValueInput<SmartRes> valueInput = this.locks;
				if (((valueInput != null) ? valueInput.value : null) != null)
				{
					answerData.lockRes = this.locks.value;
				}
				ValueInput<SmartRes> valueInput2 = this.costs;
				if (((valueInput2 != null) ? valueInput2.value : null) != null)
				{
					answerData.costRes = this.costs.value;
				}
				ValueInput<SmartRes> valueInput3 = this.rewards;
				if (((valueInput3 != null) ? valueInput3.value : null) != null)
				{
					answerData.rewardRes = this.rewards.value;
				}
				ValueInput<SmartRes> valueInput4 = this.fakeRewards;
				if (((valueInput4 != null) ? valueInput4.value : null) != null)
				{
					answerData.fakeRewardRes = this.fakeRewards.value;
				}
				ValueInput<string> valueInput5 = this.dayNumber;
				if (((valueInput5 != null) ? valueInput5.value : null) != null)
				{
					answerData.dayNumber = this.dayNumber.value;
				}
				ValueInput<string> valueInput6 = this.order;
				if (((valueInput6 != null) ? valueInput6.value : null) != null)
				{
					answerData.order = this.order.value;
				}
				ValueInput<bool> valueInput7 = this.customHideCondition;
				bool flag;
				if (valueInput7 == null)
				{
					flag = false;
				}
				else
				{
					bool value = valueInput7.value;
					flag = true;
				}
				if (flag)
				{
					answerData.customHideCondition = this.customHideCondition.value;
				}
				answerData.notAvailable = this.notAvailable;
				return answerData;
			}, "");
		}

		// Token: 0x04003E2D RID: 15917
		[FlowNode.GatherPortsCallbackAttribute]
		public bool notAvailable;

		// Token: 0x04003E2E RID: 15918
		private ValueInput<SmartRes> locks;

		// Token: 0x04003E2F RID: 15919
		private ValueInput<SmartRes> costs;

		// Token: 0x04003E30 RID: 15920
		private ValueInput<SmartRes> rewards;

		// Token: 0x04003E31 RID: 15921
		private ValueInput<SmartRes> fakeRewards;

		// Token: 0x04003E32 RID: 15922
		private ValueInput<string> dayNumber;

		// Token: 0x04003E33 RID: 15923
		private ValueInput<string> order;

		// Token: 0x04003E34 RID: 15924
		private ValueInput<bool> customHideCondition;

		// Token: 0x04003E35 RID: 15925
		private ValueOutput<AnswerData> answerData;
	}
}
