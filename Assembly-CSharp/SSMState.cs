using System;

// Token: 0x020003B2 RID: 946
public abstract class SSMState
{
	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x06001981 RID: 6529 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool CanEnter
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x06001982 RID: 6530 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public virtual bool IsActive
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001983 RID: 6531 RVA: 0x00078B08 File Offset: 0x00076D08
	public SSMState(PlayerController playerController)
	{
		this.playerController = playerController;
	}

	// Token: 0x06001984 RID: 6532 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void Update()
	{
	}

	// Token: 0x06001985 RID: 6533 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void FixedUpdate()
	{
	}

	// Token: 0x06001986 RID: 6534 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void OnEnter()
	{
	}

	// Token: 0x06001987 RID: 6535 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void OnExit()
	{
	}

	// Token: 0x040018CE RID: 6350
	protected PlayerController playerController;
}
