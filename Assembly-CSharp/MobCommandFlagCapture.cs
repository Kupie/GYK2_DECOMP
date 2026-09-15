using System;
using UnityEngine;

// Token: 0x020002EA RID: 746
public class MobCommandFlagCapture : MobCommand
{
	// Token: 0x17000360 RID: 864
	// (get) Token: 0x0600139F RID: 5023 RVA: 0x0005F5F9 File Offset: 0x0005D7F9
	public FightingCapturePoint CapturePoint
	{
		get
		{
			return this.capturePoint;
		}
	}

	// Token: 0x060013A0 RID: 5024 RVA: 0x0005F601 File Offset: 0x0005D801
	public MobCommandFlagCapture(FightingCapturePoint capturePoint)
		: base(MobCommand.CommandType.FlagCapture)
	{
		this.capturePoint = capturePoint;
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x0005F614 File Offset: 0x0005D814
	public override bool IsTheSameCommand(MobCommand other)
	{
		MobCommandFlagCapture mobCommandFlagCapture = other as MobCommandFlagCapture;
		return mobCommandFlagCapture != null && mobCommandFlagCapture.capturePoint == this.capturePoint;
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x0005F640 File Offset: 0x0005D840
	public override void OnStart()
	{
		this.agent.RichAI.SetPath(null, true);
		this.agent.RVO_Locked = false;
		this.agent.RVO_Enabled = true;
		this.agent.RvoStopAt(base.Wgo.Data.Position);
		this.agent.IsAnchoredAtDockPoint = false;
		this.agent.SetNavmeshCutActive(false);
		Vector3 vector;
		bool flag;
		this.hasReservedSlot = this.capturePoint != null && this.capturePoint.TryReserveSlot(base.Wgo.Data.UniqueId, out vector, out flag, false);
		this.agent.RichAI.simulateMovement = false;
		this.FaceCapturePoint();
		WgoPart mainWgoPart = base.Wgo.MainWgoPart;
		if (mainWgoPart == null)
		{
			return;
		}
		AnimationComponentBase animationComponent = mainWgoPart.AnimationComponent;
		if (animationComponent == null)
		{
			return;
		}
		animationComponent.SetState(global::AnimationState.FlagCapture);
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x0005F71C File Offset: 0x0005D91C
	public override void OnUpdate(float deltaTime)
	{
		if (this.capturePoint == null)
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		if (!this.capturePoint.IsOnCapturePoint(base.Wgo.Data.Position))
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		if (this.customStopCondition != null && this.customStopCondition())
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		this.FaceCapturePoint();
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x0005F798 File Offset: 0x0005D998
	public override void OnFinish()
	{
		if (this.hasReservedSlot && this.capturePoint != null)
		{
			this.capturePoint.ReleaseSlot(base.Wgo.Data.UniqueId);
		}
		FightingAgent agent = this.agent;
		if (((agent != null) ? agent.RichAI : null) != null)
		{
			this.agent.RichAI.simulateMovement = true;
		}
		WgoPart mainWgoPart = base.Wgo.MainWgoPart;
		if (mainWgoPart == null)
		{
			return;
		}
		AnimationComponentBase animationComponent = mainWgoPart.AnimationComponent;
		if (animationComponent == null)
		{
			return;
		}
		animationComponent.SetState(global::AnimationState.Idle);
	}

	// Token: 0x060013A5 RID: 5029 RVA: 0x0005F821 File Offset: 0x0005DA21
	public MobCommandFlagCapture WithCustomStopCondition(Func<bool> condition)
	{
		this.customStopCondition = condition;
		return this;
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x0005F82C File Offset: 0x0005DA2C
	private void FaceCapturePoint()
	{
		if (this.capturePoint == null)
		{
			return;
		}
		Vector2 vector = (this.capturePoint.transform.position - base.Wgo.Data.Position).XZ2();
		if (vector.sqrMagnitude > 0.0001f)
		{
			base.SetFacingDirection(vector, false);
		}
	}

	// Token: 0x040014CF RID: 5327
	private readonly FightingCapturePoint capturePoint;

	// Token: 0x040014D0 RID: 5328
	private Func<bool> customStopCondition;

	// Token: 0x040014D1 RID: 5329
	private bool hasReservedSlot;
}
