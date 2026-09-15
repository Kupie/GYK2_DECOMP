using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003A4 RID: 932
public class SSM
{
	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x06001917 RID: 6423 RVA: 0x00077046 File Offset: 0x00075246
	public SSMState CurState
	{
		get
		{
			return this.curState;
		}
	}

	// Token: 0x06001918 RID: 6424 RVA: 0x0007704E File Offset: 0x0007524E
	public SSM(SSMState defaultState)
	{
		this.defaultState = defaultState;
		this.AddState(defaultState);
	}

	// Token: 0x06001919 RID: 6425 RVA: 0x0007707A File Offset: 0x0007527A
	public void AddState(SSMState state)
	{
		if (this.statesByType.TryAdd(state.GetType(), state))
		{
			this.states.Add(state);
			return;
		}
		Debug.LogError(string.Format("Can't add state: {0}", state.GetType()));
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x000770B4 File Offset: 0x000752B4
	public T GetState<T>() where T : SSMState
	{
		SSMState ssmstate;
		if (!this.statesByType.TryGetValue(typeof(T), out ssmstate))
		{
			return default(T);
		}
		return (T)((object)ssmstate);
	}

	// Token: 0x0600191B RID: 6427 RVA: 0x000770EC File Offset: 0x000752EC
	public void ForceEnterState<T>() where T : SSMState
	{
		SSMState ssmstate;
		if (this.statesByType.TryGetValue(typeof(T), out ssmstate))
		{
			this.curState = ssmstate;
			this.HandleActionChanged();
		}
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x00077120 File Offset: 0x00075320
	public void CustomUpdate()
	{
		SSMState ssmstate = null;
		foreach (SSMState ssmstate2 in this.states)
		{
			if (ssmstate2 != this.curState && ssmstate2.CanEnter)
			{
				ssmstate = ssmstate2;
				break;
			}
		}
		if (this.curState == null || !this.curState.IsActive || ssmstate != null)
		{
			this.curState = ssmstate ?? this.defaultState;
		}
		if (this.HandleActionChanged())
		{
			return;
		}
		if (this.curState != null && this.curState.IsActive)
		{
			this.curState.Update();
		}
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x000771D8 File Offset: 0x000753D8
	public void CustomFixedUpdate()
	{
		if (this.HandleActionChanged())
		{
			return;
		}
		if (this.curState != null && this.curState.IsActive)
		{
			this.curState.FixedUpdate();
		}
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x00077204 File Offset: 0x00075404
	private bool HandleActionChanged()
	{
		bool flag = this.prevFStateBase != this.curState;
		if (flag)
		{
			SSMState ssmstate = this.prevFStateBase;
			if (ssmstate != null)
			{
				ssmstate.OnExit();
			}
			if (this.prevFStateBase != null)
			{
				Debug.Log(string.Format("SSM.ExitState: {0}", this.prevFStateBase.GetType()));
			}
			this.prevFStateBase = this.curState;
			this.curState.OnEnter();
			Debug.Log(string.Format("SSM.EnterState: {0}", this.curState.GetType()));
		}
		return flag;
	}

	// Token: 0x04001880 RID: 6272
	private List<SSMState> states = new List<SSMState>();

	// Token: 0x04001881 RID: 6273
	private Dictionary<Type, SSMState> statesByType = new Dictionary<Type, SSMState>();

	// Token: 0x04001882 RID: 6274
	private readonly SSMState defaultState;

	// Token: 0x04001883 RID: 6275
	private SSMState prevFStateBase;

	// Token: 0x04001884 RID: 6276
	private SSMState curState;
}
