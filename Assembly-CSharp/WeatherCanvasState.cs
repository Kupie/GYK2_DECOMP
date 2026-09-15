using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

// Token: 0x02000B2F RID: 2863
[Name("Weather State", 0)]
[Category("Game")]
public class WeatherCanvasState : ActionTask<WeatherComponent>
{
	// Token: 0x17000B69 RID: 2921
	// (get) Token: 0x06004C30 RID: 19504 RVA: 0x00167833 File Offset: 0x00165A33
	protected override string info
	{
		get
		{
			return string.Format("Weather: {0}", base.agentInfo);
		}
	}

	// Token: 0x06004C31 RID: 19505 RVA: 0x00167845 File Offset: 0x00165A45
	protected override void OnExecute()
	{
		base.agent.FadeIn();
	}

	// Token: 0x06004C32 RID: 19506 RVA: 0x00167852 File Offset: 0x00165A52
	protected override void OnStop()
	{
		base.agent.FadeOut();
	}
}
