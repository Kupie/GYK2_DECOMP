using System;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x020002DB RID: 731
[Serializable]
public abstract class MobCommand
{
	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06001313 RID: 4883 RVA: 0x0005E075 File Offset: 0x0005C275
	// (set) Token: 0x06001314 RID: 4884 RVA: 0x0005E07D File Offset: 0x0005C27D
	[CanBeNull]
	public ICombatEntity TargetEntity { get; set; }

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06001315 RID: 4885 RVA: 0x0005E086 File Offset: 0x0005C286
	public Vector3 Position
	{
		get
		{
			ICombatEntity targetEntity = this.TargetEntity;
			if (targetEntity == null)
			{
				return this.customPosition;
			}
			return targetEntity.CombatEntityPosition;
		}
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06001316 RID: 4886 RVA: 0x0005E0A0 File Offset: 0x0005C2A0
	public virtual Vector2 DirectionToTarget
	{
		get
		{
			return (this.Position - this.Wgo.Data.Position).XZ2().normalized;
		}
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06001317 RID: 4887 RVA: 0x0005E0D5 File Offset: 0x0005C2D5
	public Wgo Wgo
	{
		get
		{
			return this.agent.Wgo;
		}
	}

	// Token: 0x06001318 RID: 4888 RVA: 0x0005E0E2 File Offset: 0x0005C2E2
	protected MobCommand(MobCommand.CommandType type)
	{
		this.commandType = type;
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x0005E0F1 File Offset: 0x0005C2F1
	public virtual void Init(FightingAgent agent)
	{
		this.agent = agent;
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x0005E0FA File Offset: 0x0005C2FA
	public virtual bool IsTheSameCommand(MobCommand other)
	{
		return this.TargetEntity == other.TargetEntity || other.TargetEntity == null;
	}

	// Token: 0x0600131B RID: 4891
	public abstract void OnStart();

	// Token: 0x0600131C RID: 4892
	public abstract void OnUpdate(float deltaTime);

	// Token: 0x0600131D RID: 4893
	public abstract void OnFinish();

	// Token: 0x0600131E RID: 4894 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void CompensatePause(float pausedFor)
	{
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x0005E115 File Offset: 0x0005C315
	protected void SetFacingDirection(Vector2 direction, bool instant = false)
	{
		FightingAgent fightingAgent = this.agent;
		if (fightingAgent == null)
		{
			return;
		}
		fightingAgent.SetFacingDirection(direction, instant);
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x0005E129 File Offset: 0x0005C329
	protected void SetDirectionToTarget()
	{
		if (this.TargetEntity == null)
		{
			return;
		}
		this.SetFacingDirection(this.DirectionToTarget, false);
	}

	// Token: 0x04001484 RID: 5252
	public MobCommand.CommandType commandType;

	// Token: 0x04001486 RID: 5254
	public Vector3 customPosition;

	// Token: 0x04001487 RID: 5255
	protected FightingAgent agent;

	// Token: 0x020002DC RID: 732
	public enum CommandType
	{
		// Token: 0x04001489 RID: 5257
		None,
		// Token: 0x0400148A RID: 5258
		GoTo,
		// Token: 0x0400148B RID: 5259
		ZombieMeleeAttack,
		// Token: 0x0400148C RID: 5260
		ZombieBowAttack,
		// Token: 0x0400148D RID: 5261
		ZombiePikeAttack,
		// Token: 0x0400148E RID: 5262
		ZombieSpitAttack,
		// Token: 0x0400148F RID: 5263
		ZombieJump,
		// Token: 0x04001490 RID: 5264
		FlagCapture
	}
}
