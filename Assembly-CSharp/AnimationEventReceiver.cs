using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200025B RID: 603
public class AnimationEventReceiver : MonoBehaviour
{
	// Token: 0x06000F3D RID: 3901 RVA: 0x0004EA92 File Offset: 0x0004CC92
	public void CallEvent1()
	{
		UnityEvent unityEvent = this.onEvent1;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000F3E RID: 3902 RVA: 0x0004EAA4 File Offset: 0x0004CCA4
	public void CallEvent2()
	{
		if (this.onEvent2 != null && this.onEvent2.GetPersistentEventCount() > 0)
		{
			this.onEvent2.Invoke();
			return;
		}
		AnimationComponentBase componentInParent = base.GetComponentInParent<AnimationComponentBase>();
		if (componentInParent == null)
		{
			return;
		}
		componentInParent.OnAnimationStepCompleted();
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x0004EAD8 File Offset: 0x0004CCD8
	public void CallEvent3()
	{
		UnityEvent unityEvent = this.onEvent3;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x0004EAEA File Offset: 0x0004CCEA
	public void CallEvent4()
	{
		UnityEvent unityEvent = this.onEvent4;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x0004EAFC File Offset: 0x0004CCFC
	public void CallEvent5()
	{
		UnityEvent unityEvent = this.onEvent5;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000F42 RID: 3906 RVA: 0x0004EB0E File Offset: 0x0004CD0E
	public void CallEvent6()
	{
		UnityEvent unityEvent = this.onEvent6;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x0004EB20 File Offset: 0x0004CD20
	public void CallEvent7()
	{
		UnityEvent unityEvent = this.onEvent7;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x0004EB32 File Offset: 0x0004CD32
	public void CallEvent8()
	{
		UnityEvent unityEvent = this.onEvent8;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x0004EB44 File Offset: 0x0004CD44
	public void CallEvent9()
	{
		UnityEvent unityEvent = this.onEvent9;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x0004EB56 File Offset: 0x0004CD56
	public void CallEvent10()
	{
		UnityEvent unityEvent = this.onEvent10;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x0004EB68 File Offset: 0x0004CD68
	public void CallFishingEvent1()
	{
		UnityEvent unityEvent = this.onFishingEvent1;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x0004EB7A File Offset: 0x0004CD7A
	public void CallFishingEvent2()
	{
		UnityEvent unityEvent = this.onFishingEvent2;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x0004EB8C File Offset: 0x0004CD8C
	public void CallSermonEvent1()
	{
		UnityEvent unityEvent = this.onSermonEvent1;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x04001220 RID: 4640
	[SerializeField]
	private bool isPlayerRelatedAnimationEvents;

	// Token: 0x04001221 RID: 4641
	public UnityEvent onEvent1;

	// Token: 0x04001222 RID: 4642
	public UnityEvent onEvent2;

	// Token: 0x04001223 RID: 4643
	public UnityEvent onEvent3;

	// Token: 0x04001224 RID: 4644
	public UnityEvent onEvent4;

	// Token: 0x04001225 RID: 4645
	public UnityEvent onEvent5;

	// Token: 0x04001226 RID: 4646
	public UnityEvent onEvent6;

	// Token: 0x04001227 RID: 4647
	public UnityEvent onEvent7;

	// Token: 0x04001228 RID: 4648
	public UnityEvent onEvent8;

	// Token: 0x04001229 RID: 4649
	public UnityEvent onEvent9;

	// Token: 0x0400122A RID: 4650
	public UnityEvent onEvent10;

	// Token: 0x0400122B RID: 4651
	[Space]
	public UnityEvent onFishingEvent1;

	// Token: 0x0400122C RID: 4652
	public UnityEvent onFishingEvent2;

	// Token: 0x0400122D RID: 4653
	public UnityEvent onSermonEvent1;
}
