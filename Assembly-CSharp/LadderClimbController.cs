using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020001AC RID: 428
public class LadderClimbController : MonoBehaviour
{
	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0003656E File Offset: 0x0003476E
	public bool AutoLeaveLadderEnabled
	{
		get
		{
			return this.autoLeaveLadderEnabled;
		}
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x00036576 File Offset: 0x00034776
	public bool CanUse
	{
		get
		{
			return this.ladderUnderInteraction != null;
		}
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x00036584 File Offset: 0x00034784
	// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x0003658C File Offset: 0x0003478C
	public Ladder LadderUnderInteraction
	{
		get
		{
			return this.ladderUnderInteraction;
		}
		set
		{
			this.ladderUnderInteraction = value;
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x00036595 File Offset: 0x00034795
	public Ladder LadderUnderUse
	{
		get
		{
			return this.ladderUnderUse;
		}
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0003659D File Offset: 0x0003479D
	public bool IsClimbActive
	{
		get
		{
			return this.isClimbActive;
		}
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06000ACB RID: 2763 RVA: 0x000365A8 File Offset: 0x000347A8
	public bool CanLeaveLadder
	{
		get
		{
			Vector3 position = this.rigidbodyToMove.position;
			return this.ladderUnderUse.TopPart.IsInLeaveRange(position) || this.ladderUnderUse.BotPart.IsInLeaveRange(position);
		}
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x000365E7 File Offset: 0x000347E7
	public void Init(AnimationComponentBase animationComponent)
	{
		this.animationComponent = animationComponent;
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x000365F0 File Offset: 0x000347F0
	public void StartClimb(float initPos, Func<bool> climbUpControl, Func<bool> climbDownControl, Ladder ladder, Rigidbody rigidbodyToMove)
	{
		if (this.isClimbActive)
		{
			Debug.Log("Climb is already active");
			return;
		}
		this.progress = initPos;
		this.climbUpControl = climbUpControl;
		this.climbDownControl = climbDownControl;
		this.stateNameHashed = this.animationComponent.Animator.GetLayerName(this.animationLayerIndex) + ".Climb";
		this.isValid = climbUpControl != null && climbDownControl != null;
		this.ladderUnderUse = ladder;
		this.rigidbodyToMove = rigidbodyToMove;
		this.SetPosOnClimbStart();
		this.isClimbActive = true;
		LazyAudio.PlayAndForget("ladder_climb_start");
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x00036684 File Offset: 0x00034884
	public void StopClimb()
	{
		this.isClimbActive = false;
		this.SetPosOnClimbEnd();
		this.progress = 0f;
		this.isValid = false;
		this.climbUpControl = null;
		this.climbDownControl = null;
		this.ladderUnderUse = null;
		this.rigidbodyToMove = null;
		LazyAudio.PlayAndForget("ladder_climb_finish");
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x000366D8 File Offset: 0x000348D8
	private void Update()
	{
		if (!this.isValid || !this.isClimbActive)
		{
			return;
		}
		if (MainGame.IsGamePaused)
		{
			return;
		}
		bool flag = this.climbUpControl();
		bool flag2 = this.climbDownControl();
		Vector3 vector = this.rigidbodyToMove.position;
		bool flag3 = false;
		if (flag || flag2)
		{
			LadderEdgePart ladderEdgePart = (flag ? this.ladderUnderUse.TopPart : this.ladderUnderUse.BotPart);
			int num = (flag ? 1 : (-1));
			int num2 = (this.IsAbleToMoveToTarget(this.rigidbodyToMove.position, ladderEdgePart.StartPoint.position) ? 1 : 0);
			if (num2 == 1)
			{
				vector += (ladderEdgePart.StartPoint.position - this.rigidbodyToMove.position).normalized * (Time.unscaledDeltaTime * this.climbMovementSpeed * this.speedMultiplier);
			}
			this.progress += Time.unscaledDeltaTime * this.climbAnimSpeed * this.speedMultiplier * (float)num2 * (float)num;
			flag3 = this.autoLeaveLadderEnabled && num2 == 0;
		}
		Vector3 position = this.ladderUnderUse.BotPart.StartPoint.position;
		Vector3 position2 = this.ladderUnderUse.TopPart.StartPoint.position;
		this.rigidbodyToMove.MovePosition(new Vector3(Mathf.Clamp(vector.x, position.x, position2.x), Mathf.Clamp(vector.y, position.y, position2.y), Mathf.Clamp(vector.z, position.z, position2.z)));
		this.animationComponent.Animator.Play(this.stateNameHashed, this.animationLayerIndex, this.progress);
		if (!flag3)
		{
			return;
		}
		this.StopClimb();
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x000368B0 File Offset: 0x00034AB0
	private void OnTriggerEnter(Collider other)
	{
		LadderEdgePart ladderEdgePart;
		if (other.TryGetComponent<LadderEdgePart>(out ladderEdgePart))
		{
			if (!ladderEdgePart.Ladder)
			{
				return;
			}
			this.ladderUnderInteraction = ladderEdgePart.Ladder;
		}
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x000368E4 File Offset: 0x00034AE4
	private void OnTriggerExit(Collider other)
	{
		LadderEdgePart ladderEdgePart;
		if (other.TryGetComponent<LadderEdgePart>(out ladderEdgePart))
		{
			if (!ladderEdgePart.Ladder)
			{
				return;
			}
			if (ladderEdgePart.Ladder == this.ladderUnderInteraction)
			{
				this.ladderUnderInteraction = null;
			}
		}
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x00036924 File Offset: 0x00034B24
	private bool IsAbleToMoveToTarget(Vector3 currentPos, Vector3 target)
	{
		return this.ladderUnderUse && !(currentPos - target).magnitude.EqualsTo(0f, 0.001f);
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x00036964 File Offset: 0x00034B64
	private void SetPosOnClimbStart()
	{
		LadderEdgePart nearestLadderPart = this.ladderUnderUse.GetNearestLadderPart(this.rigidbodyToMove.position);
		if (nearestLadderPart == null)
		{
			return;
		}
		this.rigidbodyToMove.position = nearestLadderPart.StartPoint.position;
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x000369A8 File Offset: 0x00034BA8
	private void SetPosOnClimbEnd()
	{
		LadderEdgePart nearestLadderPart = this.ladderUnderUse.GetNearestLadderPart(this.rigidbodyToMove.position);
		if (nearestLadderPart == null)
		{
			return;
		}
		this.rigidbodyToMove.position = nearestLadderPart.TpPoint.position;
		this.animationComponent.SetDirection(Direction.Up);
	}

	// Token: 0x04000C38 RID: 3128
	private const string climbAnimName = "Climb";

	// Token: 0x04000C39 RID: 3129
	[SerializeField]
	[Range(0.5f, 5f)]
	private float speedMultiplier = 1f;

	// Token: 0x04000C3A RID: 3130
	[SerializeField]
	[Range(0.1f, 5f)]
	private float climbMovementSpeed = 1f;

	// Token: 0x04000C3B RID: 3131
	[SerializeField]
	[Range(0.5f, 5f)]
	private float climbAnimSpeed = 1f;

	// Token: 0x04000C3C RID: 3132
	[SerializeField]
	private bool autoLeaveLadderEnabled;

	// Token: 0x04000C3D RID: 3133
	private AnimationComponentBase animationComponent;

	// Token: 0x04000C3E RID: 3134
	private float progress;

	// Token: 0x04000C3F RID: 3135
	private int animationLayerIndex;

	// Token: 0x04000C40 RID: 3136
	private Func<bool> climbUpControl;

	// Token: 0x04000C41 RID: 3137
	private Func<bool> climbDownControl;

	// Token: 0x04000C42 RID: 3138
	private bool isValid;

	// Token: 0x04000C43 RID: 3139
	private string stateNameHashed;

	// Token: 0x04000C44 RID: 3140
	private Ladder ladderUnderUse;

	// Token: 0x04000C45 RID: 3141
	private Ladder ladderUnderInteraction;

	// Token: 0x04000C46 RID: 3142
	private Rigidbody rigidbodyToMove;

	// Token: 0x04000C47 RID: 3143
	private bool isClimbActive;
}
