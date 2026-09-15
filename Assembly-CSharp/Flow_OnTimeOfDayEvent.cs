using System;
using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion;
using ParadoxNotion.Design;

// Token: 0x020004CD RID: 1229
[Name("On Time Of Day", 8)]
[Description("Automatically calls when time reaches timeToCall")]
public class Flow_OnTimeOfDayEvent : EventNode
{
	// Token: 0x0600207E RID: 8318 RVA: 0x00099C07 File Offset: 0x00097E07
	protected override void RegisterPorts()
	{
		this.onTimeOfDay = base.AddFlowOutput("onTimeOfDay".CapitalizeFirst(), "");
		this.timeToCall = base.AddValueInput<float>("timeToCall", "");
	}

	// Token: 0x0600207F RID: 8319 RVA: 0x00099C3A File Offset: 0x00097E3A
	public override void OnGraphStarted()
	{
		EnvironmentEngine.OnTimeOfDayChangedEvent += this.CheckCall;
		this.called = EnvironmentEngine.Instance.timeOfDay >= this.timeToCall.value;
	}

	// Token: 0x06002080 RID: 8320 RVA: 0x00099C6D File Offset: 0x00097E6D
	public override void OnGraphStoped()
	{
		EnvironmentEngine.OnTimeOfDayChangedEvent -= this.CheckCall;
	}

	// Token: 0x06002081 RID: 8321 RVA: 0x00099C80 File Offset: 0x00097E80
	private void CheckCall(float currentTime, bool isFake)
	{
		if (isFake)
		{
			return;
		}
		if (currentTime < this.timeToCall.value)
		{
			this.called = false;
		}
		if (!this.called && currentTime >= this.timeToCall.value)
		{
			this.onTimeOfDay.Call(default(Flow));
			this.called = true;
		}
	}

	// Token: 0x04001D1E RID: 7454
	private ValueInput<float> timeToCall;

	// Token: 0x04001D1F RID: 7455
	private FlowOutput onTimeOfDay;

	// Token: 0x04001D20 RID: 7456
	private bool called;
}
