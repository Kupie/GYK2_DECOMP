using System;
using UnityEngine;

// Token: 0x020002E3 RID: 739
public abstract class GoToDestinationModifier
{
	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06001357 RID: 4951 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public virtual bool IsValid
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06001358 RID: 4952 RVA: 0x0005E810 File Offset: 0x0005CA10
	public virtual float CustomDestinationOffset
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x06001359 RID: 4953 RVA: 0x0005E817 File Offset: 0x0005CA17
	// (set) Token: 0x0600135A RID: 4954 RVA: 0x0005E81F File Offset: 0x0005CA1F
	private protected FightingAgent Agent { protected get; private set; }

	// Token: 0x1700034F RID: 847
	// (get) Token: 0x0600135B RID: 4955 RVA: 0x0005E828 File Offset: 0x0005CA28
	protected Wgo Wgo
	{
		get
		{
			return this.Agent.Wgo;
		}
	}

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x0600135C RID: 4956 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool StopWhenCrowded
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x0600135D RID: 4957 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool IsStucked
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x0600135E RID: 4958 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool ShouldAnchorOnArrival
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x0005E835 File Offset: 0x0005CA35
	public virtual void Init(FightingAgent agent)
	{
		this.Agent = agent;
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void OnStart()
	{
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void OnUpdate(float deltaTime)
	{
	}

	// Token: 0x06001362 RID: 4962
	public abstract bool TryGetTargetPositionForPathfinding(out ICombatEntity combatEntity, out Vector3 position);

	// Token: 0x06001363 RID: 4963
	public abstract Vector3 GetCurrentTargetPosition();

	// Token: 0x06001364 RID: 4964 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void OnReachedDestination()
	{
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void OnFinish()
	{
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x0005E840 File Offset: 0x0005CA40
	public virtual GoToDestinationModifierDebugInfo GetDebugInfo()
	{
		GoToDestinationModifierDebugInfo goToDestinationModifierDebugInfo = default(GoToDestinationModifierDebugInfo);
		goToDestinationModifierDebugInfo.ModifierType = base.GetType().Name;
		goToDestinationModifierDebugInfo.IsValid = this.IsValid;
		goToDestinationModifierDebugInfo.IsStucked = this.IsStucked;
		goToDestinationModifierDebugInfo.ShouldAnchorOnArrival = this.ShouldAnchorOnArrival;
		goToDestinationModifierDebugInfo.CurrentTargetPosition = ((this.Agent != null) ? this.GetCurrentTargetPosition() : default(Vector3));
		FightingAgent agent = this.Agent;
		ICombatEntity combatEntity;
		if (agent == null)
		{
			combatEntity = null;
		}
		else
		{
			MobCommand mobCommand = agent.MobCommand;
			combatEntity = ((mobCommand != null) ? mobCommand.TargetEntity : null);
		}
		goToDestinationModifierDebugInfo.TargetWgo = GoToDestinationModifierDebugInfo.ResolveTargetWgoView(combatEntity, null);
		return goToDestinationModifierDebugInfo;
	}
}
