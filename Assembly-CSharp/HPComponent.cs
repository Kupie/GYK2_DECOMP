using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200035B RID: 859
[Serializable]
public class HPComponent : IComponent
{
	// Token: 0x14000035 RID: 53
	// (add) Token: 0x060016B8 RID: 5816 RVA: 0x0006D1AC File Offset: 0x0006B3AC
	// (remove) Token: 0x060016B9 RID: 5817 RVA: 0x0006D1E4 File Offset: 0x0006B3E4
	public event Action OnFirstDamageDealt;

	// Token: 0x14000036 RID: 54
	// (add) Token: 0x060016BA RID: 5818 RVA: 0x0006D21C File Offset: 0x0006B41C
	// (remove) Token: 0x060016BB RID: 5819 RVA: 0x0006D254 File Offset: 0x0006B454
	public event Action OnFullHpRestored;

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x060016BC RID: 5820 RVA: 0x0006D28C File Offset: 0x0006B48C
	// (remove) Token: 0x060016BD RID: 5821 RVA: 0x0006D2C4 File Offset: 0x0006B4C4
	public event Action<HPComponent> OnHpChanged;

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x060016BE RID: 5822 RVA: 0x0006D2F9 File Offset: 0x0006B4F9
	// (set) Token: 0x060016BF RID: 5823 RVA: 0x0006D301 File Offset: 0x0006B501
	public bool IsImmuneToDamage { get; set; }

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x060016C0 RID: 5824 RVA: 0x0006D30A File Offset: 0x0006B50A
	// (set) Token: 0x060016C1 RID: 5825 RVA: 0x0006D312 File Offset: 0x0006B512
	public int Hp
	{
		get
		{
			return this.hp;
		}
		set
		{
			if (this.isDeathDelayed)
			{
				return;
			}
			this.prevHp = this.hp;
			this.hp = value;
			if (this.hp < 0)
			{
				this.hp = 0;
			}
			if (this.hp == 0)
			{
				this.HandleHp0();
			}
		}
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x060016C2 RID: 5826 RVA: 0x0006D34E File Offset: 0x0006B54E
	public int MaxHpValue
	{
		get
		{
			return this.maxHpValue;
		}
	}

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x060016C3 RID: 5827 RVA: 0x0006D356 File Offset: 0x0006B556
	public bool WasDamagedAtLeastOnce
	{
		get
		{
			return this.wasDamagedAtLeastOnce;
		}
	}

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x060016C4 RID: 5828 RVA: 0x0006D35E File Offset: 0x0006B55E
	public bool HasFullHp
	{
		get
		{
			return this.hp == this.maxHpValue;
		}
	}

	// Token: 0x060016C5 RID: 5829 RVA: 0x00021B94 File Offset: 0x0001FD94
	public HPComponent()
	{
	}

	// Token: 0x060016C6 RID: 5830 RVA: 0x0006D36E File Offset: 0x0006B56E
	public HPComponent(int maxHpValue)
	{
		this.hp = maxHpValue;
		this.prevHp = maxHpValue;
		this.maxHpValue = maxHpValue;
	}

	// Token: 0x060016C7 RID: 5831 RVA: 0x0006D38B File Offset: 0x0006B58B
	public HPComponent(int maxHpValue, int startHpValue)
	{
		this.hp = startHpValue;
		this.prevHp = startHpValue;
		this.maxHpValue = maxHpValue;
	}

	// Token: 0x060016C8 RID: 5832 RVA: 0x0006D3A8 File Offset: 0x0006B5A8
	public void Init(Action onDelayedDeadAction = null, Action hp0Action = null)
	{
		this.onDelayedDeadAction = onDelayedDeadAction;
		this.hp0Action = hp0Action;
		this.prevHp = this.hp;
		if (this.isDeathDelayed)
		{
			this.ScheduleCompleteDelayedDeathAfterWorldReady();
		}
	}

	// Token: 0x060016C9 RID: 5833 RVA: 0x0006D3D4 File Offset: 0x0006B5D4
	public void ApplyDamage(int damage)
	{
		if (this.IsImmuneToDamage)
		{
			return;
		}
		if (this.maxHpValue == -1)
		{
			return;
		}
		if (this.isDeathDelayed)
		{
			return;
		}
		this.prevHp = this.hp;
		this.hp -= damage;
		this.hp = Mathf.Clamp(this.hp, 0, this.maxHpValue);
		if (!this.wasDamagedAtLeastOnce && this.hp != this.prevHp)
		{
			this.wasDamagedAtLeastOnce = true;
			Action onFirstDamageDealt = this.OnFirstDamageDealt;
			if (onFirstDamageDealt != null)
			{
				onFirstDamageDealt();
			}
		}
		if (this.hp != this.prevHp)
		{
			Action<HPComponent> onHpChanged = this.OnHpChanged;
			if (onHpChanged != null)
			{
				onHpChanged(this);
			}
		}
		if (this.hp == 0)
		{
			this.HandleHp0();
		}
	}

	// Token: 0x060016CA RID: 5834 RVA: 0x0006D48C File Offset: 0x0006B68C
	public void AddHp(int value)
	{
		if (this.maxHpValue == -1)
		{
			return;
		}
		if (this.isDeathDelayed)
		{
			return;
		}
		this.prevHp = this.hp;
		this.hp += value;
		this.hp = Mathf.Clamp(this.hp, 0, this.maxHpValue);
		if (this.hp != this.prevHp)
		{
			Action<HPComponent> onHpChanged = this.OnHpChanged;
			if (onHpChanged == null)
			{
				return;
			}
			onHpChanged(this);
		}
	}

	// Token: 0x060016CB RID: 5835 RVA: 0x0006D500 File Offset: 0x0006B700
	public void RestoreFullHp()
	{
		this.prevHp = this.hp;
		this.hp = this.maxHpValue;
		this.wasDamagedAtLeastOnce = false;
		this.isDeathDelayed = false;
		Action<HPComponent> onHpChanged = this.OnHpChanged;
		if (onHpChanged != null)
		{
			onHpChanged(this);
		}
		Action onFullHpRestored = this.OnFullHpRestored;
		if (onFullHpRestored == null)
		{
			return;
		}
		onFullHpRestored();
	}

	// Token: 0x060016CC RID: 5836 RVA: 0x0006D555 File Offset: 0x0006B755
	public void SetCustomHpValue(int newHp, bool overrideMaxHpValue = true)
	{
		this.hp = newHp;
		this.prevHp = newHp;
		if (overrideMaxHpValue)
		{
			this.maxHpValue = newHp;
		}
	}

	// Token: 0x060016CD RID: 5837 RVA: 0x0006D56F File Offset: 0x0006B76F
	private void HandleHp0()
	{
		Action action = this.hp0Action;
		if (action != null)
		{
			action();
		}
		this.RunDelayedDeath();
	}

	// Token: 0x060016CE RID: 5838 RVA: 0x0006D588 File Offset: 0x0006B788
	private void HandleDeath()
	{
		Action action = this.onDelayedDeadAction;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x060016CF RID: 5839 RVA: 0x0006D59A File Offset: 0x0006B79A
	private void RunDelayedDeath()
	{
		this.isDeathDelayed = true;
		LazyTimer.AddTimer(0.25f, delegate
		{
			this.DoDeath();
		}, null);
	}

	// Token: 0x060016D0 RID: 5840 RVA: 0x0006D5BB File Offset: 0x0006B7BB
	private void DoDeath()
	{
		if (!this.isDeathDelayed)
		{
			return;
		}
		this.isDeathDelayed = false;
		this.HandleDeath();
	}

	// Token: 0x060016D1 RID: 5841 RVA: 0x0006D5D4 File Offset: 0x0006B7D4
	private void ScheduleCompleteDelayedDeathAfterWorldReady()
	{
		GameScene gameScene;
		if (MainGame.Instance != null && MainGame.PlayerController.TryGetCurrentGameScene(out gameScene))
		{
			this.DoDeath();
			return;
		}
		MainGame.OnGameStarted = (Action)Delegate.Combine(MainGame.OnGameStarted, new Action(this.CompleteDelayedDeathOnGameStarted));
	}

	// Token: 0x060016D2 RID: 5842 RVA: 0x0006D623 File Offset: 0x0006B823
	private void CompleteDelayedDeathOnGameStarted()
	{
		MainGame.OnGameStarted = (Action)Delegate.Remove(MainGame.OnGameStarted, new Action(this.CompleteDelayedDeathOnGameStarted));
		if (this.isDeathDelayed)
		{
			this.DoDeath();
		}
	}

	// Token: 0x040016ED RID: 5869
	private const float DEATH_DELAY_TIME = 0.25f;

	// Token: 0x040016F1 RID: 5873
	private Action onDelayedDeadAction;

	// Token: 0x040016F2 RID: 5874
	private Action hp0Action;

	// Token: 0x040016F3 RID: 5875
	[SerializeField]
	private int maxHpValue;

	// Token: 0x040016F4 RID: 5876
	[SerializeField]
	private int hp;

	// Token: 0x040016F5 RID: 5877
	[NonSerialized]
	public int prevHp;

	// Token: 0x040016F6 RID: 5878
	public bool wasDamagedAtLeastOnce;

	// Token: 0x040016F7 RID: 5879
	public bool isDeathDelayed;

	// Token: 0x040016F8 RID: 5880
	public bool firstDamageWasMax;
}
