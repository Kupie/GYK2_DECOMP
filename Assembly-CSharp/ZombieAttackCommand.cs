using System;

// Token: 0x020002DD RID: 733
public abstract class ZombieAttackCommand : MobCommand
{
	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06001321 RID: 4897 RVA: 0x0005E141 File Offset: 0x0005C341
	public bool IsAttackAnimPlaying
	{
		get
		{
			return this.isAttackAnimPlaying;
		}
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x0005E149 File Offset: 0x0005C349
	protected ZombieAttackCommand(MobCommand.CommandType type)
		: base(type)
	{
	}

	// Token: 0x06001323 RID: 4899 RVA: 0x0005E154 File Offset: 0x0005C354
	public override void Init(FightingAgent agent)
	{
		base.Init(agent);
		this.pauseTime = agent.FighterDef.atkPause.EvaluateFloat(agent.Wgo);
		if (agent.PreviousCommand == null)
		{
			this.applyPause = false;
		}
		else
		{
			this.applyPause = agent.PreviousCommand is ZombieAttackCommand;
		}
		this.pauseTimeAccumulated = 0f;
	}

	// Token: 0x06001324 RID: 4900 RVA: 0x00002318 File Offset: 0x00000518
	public override void OnStart()
	{
	}

	// Token: 0x06001325 RID: 4901 RVA: 0x0005E1B4 File Offset: 0x0005C3B4
	public override void OnUpdate(float deltaTime)
	{
		base.SetDirectionToTarget();
	}

	// Token: 0x06001326 RID: 4902 RVA: 0x0005E1BC File Offset: 0x0005C3BC
	public override void OnFinish()
	{
		this.agent.RVO_Locked = this.agent.IsAnchoredAtDockPoint;
		this.pauseTimeAccumulated = 0f;
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x0005E1DF File Offset: 0x0005C3DF
	public ZombieAttackCommand WithCustomStopCondition(Func<bool> condition)
	{
		this.customStopCondition = condition;
		return this;
	}

	// Token: 0x06001328 RID: 4904 RVA: 0x0005E1E9 File Offset: 0x0005C3E9
	public ZombieAttackCommand WithCustomOnStartAction(Action action)
	{
		this.onStartAction = action;
		return this;
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x0005E1F3 File Offset: 0x0005C3F3
	public ZombieAttackCommand WithCustomOnStopAction(Action action)
	{
		this.onStopAction = action;
		return this;
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x0005E200 File Offset: 0x0005C400
	protected bool ShouldPause(float deltaTime)
	{
		if (!this.applyPause)
		{
			return false;
		}
		this.pauseTimeAccumulated += deltaTime;
		return this.pauseTime.EqualsOrMore(0f, 1E-05f) && this.pauseTimeAccumulated < this.pauseTime;
	}

	// Token: 0x04001491 RID: 5265
	protected Func<bool> customStopCondition;

	// Token: 0x04001492 RID: 5266
	protected Action onStartAction;

	// Token: 0x04001493 RID: 5267
	protected Action onStopAction;

	// Token: 0x04001494 RID: 5268
	protected bool applyPause;

	// Token: 0x04001495 RID: 5269
	protected float pauseTime;

	// Token: 0x04001496 RID: 5270
	protected float pauseTimeAccumulated;

	// Token: 0x04001497 RID: 5271
	protected bool isAttackAnimPlaying;
}
