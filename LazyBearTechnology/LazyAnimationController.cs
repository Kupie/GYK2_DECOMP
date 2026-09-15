using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000C3 RID: 195
	[RequireComponent(typeof(Animator))]
	public class LazyAnimationController : MonoBehaviour
	{
		// Token: 0x060002D8 RID: 728 RVA: 0x0000EC38 File Offset: 0x0000CE38
		private void Awake()
		{
			this.animator = base.GetComponent<Animator>();
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000EC48 File Offset: 0x0000CE48
		private void OnEnable()
		{
			foreach (AnimationEventData animationEventData in this.animationEvents)
			{
				animationEventData.PlanStartPlayingTime();
			}
			this.TryStartEventFromStartTriggerList();
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000ECA0 File Offset: 0x0000CEA0
		private void TryStartEventFromStartTriggerList()
		{
			if (this.onStartTriggerList.Count != 0)
			{
				string random = this.onStartTriggerList.GetRandom<string>();
				this.ForceStartEvent(random);
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000ECCD File Offset: 0x0000CECD
		private void Update()
		{
			this.CheckIsIdleState();
			if (this.idleState)
			{
				this.minIdleTimeCounter += Time.deltaTime;
				this.UpdateEventTimers();
				this.CheckAnimationEventsToFire();
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000ECFC File Offset: 0x0000CEFC
		private void CheckIsIdleState()
		{
			AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
			bool flag = false;
			for (int i = 0; i < this.idleAnimStateNames.Count; i++)
			{
				if (currentAnimatorStateInfo.IsName(this.idleAnimStateNames[i]))
				{
					flag = true;
					break;
				}
			}
			if (flag && !this.idleState)
			{
				this.minIdleTimeCounter = 0f;
				Action onTriggerToIdleReturned = this.OnTriggerToIdleReturned;
				if (onTriggerToIdleReturned != null)
				{
					onTriggerToIdleReturned();
				}
				this.OnTriggerToIdleReturned = null;
				this.idleState = true;
				return;
			}
			if (!flag && this.idleState)
			{
				this.idleState = false;
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000ED90 File Offset: 0x0000CF90
		private void UpdateEventTimers()
		{
			float deltaTime = Time.deltaTime;
			for (int i = 0; i < this.animationEvents.Count; i++)
			{
				this.animationEvents[i].nextTimeToPlay -= deltaTime;
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000EDD4 File Offset: 0x0000CFD4
		private void CheckAnimationEventsToFire()
		{
			if (!this.idleState || this.minIdleTimeCounter < this.minIdleTime)
			{
				return;
			}
			this.possibleEventsToFire.Clear();
			for (int i = 0; i < this.animationEvents.Count; i++)
			{
				if (this.animationEvents[i].nextTimeToPlay <= 0f)
				{
					this.possibleEventsToFire.Add(this.animationEvents[i]);
				}
			}
			if (this.possibleEventsToFire.Count > 0)
			{
				if (this.possibleEventsToFire.Count == 1)
				{
					this.FireEvent(this.possibleEventsToFire[0]);
					return;
				}
				AnimationEventData animationEventData = this.possibleEventsToFire[0];
				float num = animationEventData.planningTime;
				for (int j = 1; j < this.possibleEventsToFire.Count; j++)
				{
					if (this.possibleEventsToFire[j].planningTime < num)
					{
						animationEventData = this.possibleEventsToFire[j];
						num = animationEventData.planningTime;
					}
				}
				this.FireEvent(animationEventData);
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000EED0 File Offset: 0x0000D0D0
		private void FireEvent(AnimationEventData animationEvent)
		{
			animationEvent.PlanStartPlayingTime();
			if (this.resetTriggersBeforeFireNewTrigger)
			{
				for (int i = 0; i < animationEvent.triggers.Count; i++)
				{
					this.animator.ResetTrigger(animationEvent.triggers[i].triggerName);
				}
			}
			this.animator.SetTrigger(animationEvent.PickTrigger());
			this.idleState = false;
			this.minIdleTimeCounter = 0f;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000EF40 File Offset: 0x0000D140
		public void ForceStartEvent(string eventId)
		{
			AnimationEventData animationEventData = this.animationEvents.Find((AnimationEventData x) => x.id == eventId);
			if (animationEventData == null)
			{
				Debug.LogError("AnimationData for event id " + eventId + " not defined.");
				return;
			}
			this.FireEvent(animationEventData);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000EF97 File Offset: 0x0000D197
		public void SetTrigger(string trigger, Action callback, Action<string> onEnterClip = null)
		{
			this.animator.SetTrigger(trigger);
			this.OnTriggerToIdleReturned = callback;
			this.onEnterClip = onEnterClip;
			this.idleState = false;
			this.minIdleTimeCounter = 0f;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000EFC5 File Offset: 0x0000D1C5
		public void InvokeClipEnter(string clipName)
		{
			Action<string> action = this.onEnterClip;
			if (action == null)
			{
				return;
			}
			action(clipName);
		}

		// Token: 0x040000CA RID: 202
		[SerializeField]
		private float minIdleTime;

		// Token: 0x040000CB RID: 203
		[SerializeField]
		private List<string> idleAnimStateNames;

		// Token: 0x040000CC RID: 204
		[SerializeField]
		private List<AnimationEventData> animationEvents;

		// Token: 0x040000CD RID: 205
		[SerializeField]
		private bool resetTriggersBeforeFireNewTrigger;

		// Token: 0x040000CE RID: 206
		[SerializeField]
		private List<string> onStartTriggerList;

		// Token: 0x040000CF RID: 207
		private Animator animator;

		// Token: 0x040000D0 RID: 208
		private bool idleState;

		// Token: 0x040000D1 RID: 209
		private float minIdleTimeCounter;

		// Token: 0x040000D2 RID: 210
		private List<AnimationEventData> possibleEventsToFire = new List<AnimationEventData>();

		// Token: 0x040000D3 RID: 211
		private Action OnTriggerToIdleReturned;

		// Token: 0x040000D4 RID: 212
		private Action<string> onEnterClip;
	}
}
