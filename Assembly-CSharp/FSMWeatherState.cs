using System;
using System.Collections.Generic;
using System.Linq;
using FlowCanvas;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using UnityEngine;

// Token: 0x02000B2C RID: 2860
[Name("Weather State", 0)]
public class FSMWeatherState : FSMState
{
	// Token: 0x17000B68 RID: 2920
	// (get) Token: 0x06004C22 RID: 19490 RVA: 0x0016762E File Offset: 0x0016582E
	public override string name
	{
		get
		{
			if (!(this.weather != null))
			{
				return "NULL Weather";
			}
			return this.weather.name;
		}
	}

	// Token: 0x06004C23 RID: 19491 RVA: 0x00167650 File Offset: 0x00165850
	private FSMWeatherState.WeatherStateExit GetRandomExit()
	{
		if (this.exits.Count == 0)
		{
			return null;
		}
		if (this.exits.Count == 1)
		{
			return this.exits[0];
		}
		int num = this.exits.Sum((FSMWeatherState.WeatherStateExit e) => e.w);
		int num2 = global::UnityEngine.Random.Range(0, num - 1);
		num = 0;
		foreach (FSMWeatherState.WeatherStateExit weatherStateExit in this.exits)
		{
			if (weatherStateExit.w != 0)
			{
				num += weatherStateExit.w;
				if (num2 <= num)
				{
					return weatherStateExit;
				}
			}
		}
		return null;
	}

	// Token: 0x06004C24 RID: 19492 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	protected override bool CanConnectFromSource(Node sourceNode)
	{
		return true;
	}

	// Token: 0x06004C25 RID: 19493 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	protected override bool CanConnectToTarget(Node targetNode)
	{
		return true;
	}

	// Token: 0x06004C26 RID: 19494 RVA: 0x0016771C File Offset: 0x0016591C
	protected override void OnEnter()
	{
		if (this.onEnterFlowScript != null)
		{
			GlobalScriptsManager.RunFlowScript(this.onEnterFlowScript, null, FlowScriptLoadMode.DeserializeOnInit);
		}
		if (this.weather == null)
		{
			this.ProcessTransition();
			return;
		}
		this.weather.FadeIn();
	}

	// Token: 0x06004C27 RID: 19495 RVA: 0x00167759 File Offset: 0x00165959
	protected override void OnExit()
	{
		if (this.onExitFlowScript != null)
		{
			GlobalScriptsManager.RunFlowScript(this.onExitFlowScript, null, FlowScriptLoadMode.DeserializeOnInit);
		}
		WeatherComponent weatherComponent = this.weather;
		if (weatherComponent == null)
		{
			return;
		}
		weatherComponent.FadeOut();
	}

	// Token: 0x06004C28 RID: 19496 RVA: 0x00167788 File Offset: 0x00165988
	private void ProcessTransition()
	{
		WeatherComponent weatherComponent = this.weather;
		if (weatherComponent != null)
		{
			weatherComponent.FadeOut();
		}
		FSMWeatherState.WeatherStateExit randomExit = this.GetRandomExit();
		WeatherStateFSMConnection weatherStateFSMConnection = ((randomExit != null) ? randomExit.connection : null);
		if (weatherStateFSMConnection != null)
		{
			base.FSM.EnterState((FSMState)weatherStateFSMConnection.targetNode, weatherStateFSMConnection.transitionCallMode);
			weatherStateFSMConnection.status = Status.Success;
			return;
		}
		base.FSM.EnterState(this, FSM.TransitionCallMode.Normal);
	}

	// Token: 0x06004C29 RID: 19497 RVA: 0x001677EF File Offset: 0x001659EF
	protected override void OnInit()
	{
		base.OnInit();
		base.transitionEvaluation = FSMState.TransitionEvaluationMode.CheckManually;
	}

	// Token: 0x06004C2A RID: 19498 RVA: 0x001677FE File Offset: 0x001659FE
	public void FinishWeatherState()
	{
		base.Finish();
		this.ProcessTransition();
	}

	// Token: 0x04003D49 RID: 15689
	public WeatherComponent weather;

	// Token: 0x04003D4A RID: 15690
	public FlowScript onEnterFlowScript;

	// Token: 0x04003D4B RID: 15691
	public FlowScript onExitFlowScript;

	// Token: 0x04003D4C RID: 15692
	public readonly List<FSMWeatherState.WeatherStateExit> exits = new List<FSMWeatherState.WeatherStateExit>();

	// Token: 0x02000B2D RID: 2861
	[Serializable]
	public class WeatherStateExit
	{
		// Token: 0x04003D4D RID: 15693
		public int w;

		// Token: 0x04003D4E RID: 15694
		public WeatherStateFSMConnection connection;
	}
}
