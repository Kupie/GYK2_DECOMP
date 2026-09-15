using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000949 RID: 2377
public class MapVirtualCursor : BaseVirtualCursor
{
	// Token: 0x17000976 RID: 2422
	// (get) Token: 0x06003EB2 RID: 16050 RVA: 0x0012B512 File Offset: 0x00129712
	public Vector2 DesiredMovementDelta
	{
		get
		{
			return this.desiredMovementDelta;
		}
	}

	// Token: 0x17000977 RID: 2423
	// (get) Token: 0x06003EB3 RID: 16051 RVA: 0x0012B51A File Offset: 0x0012971A
	public float CurrentFrameSpeed
	{
		get
		{
			return this.GetSpeed();
		}
	}

	// Token: 0x06003EB4 RID: 16052 RVA: 0x00002318 File Offset: 0x00000518
	public override void CalculateBounds()
	{
	}

	// Token: 0x06003EB5 RID: 16053 RVA: 0x0012B522 File Offset: 0x00129722
	public void SetBoundsRectTransform(RectTransform boundsRectTransform)
	{
		this.boundsRectTransform = boundsRectTransform;
	}

	// Token: 0x06003EB6 RID: 16054 RVA: 0x0012B52B File Offset: 0x0012972B
	public void SetPosition(Vector3 position)
	{
		this.ResetMoveAnimationState();
		this.currentPos = position;
		base.transform.position = this.currentPos;
		if (this.cursorRectTransform != null)
		{
			this.cursorRectTransform.position = this.currentPos;
		}
	}

	// Token: 0x06003EB7 RID: 16055 RVA: 0x0012B56C File Offset: 0x0012976C
	public void DoAnimationTo(RectTransform rectTransform)
	{
		if (this.cursorRectTransform == null)
		{
			this.ResetAnimationState();
			return;
		}
		this.previousRectTransform = rectTransform;
		if (this.isMoveTweenActive)
		{
			Tweener tweener = this.moveTween;
			if (tweener != null)
			{
				tweener.Kill(false);
			}
		}
		if (this.isSizeTweenActive)
		{
			Tweener tweener2 = this.sizeTween;
			if (tweener2 != null)
			{
				tweener2.Kill(false);
			}
		}
		Vector3 vector = ((rectTransform != null) ? rectTransform.position : this.currentPos);
		Vector2 vector2 = ((rectTransform != null) ? rectTransform.sizeDelta : this.initialCursorSizeDelta);
		this.moveTween = this.cursorRectTransform.DOMove(vector, this.animationDuration, false).SetEase(this.animationEase).OnComplete(delegate
		{
			this.isMoveTweenActive = false;
			if (rectTransform == null)
			{
				this.previousRectTransform = null;
			}
		});
		this.sizeTween = this.cursorRectTransform.DOSizeDelta(vector2, this.animationDuration, false).SetEase(this.animationEase).OnComplete(delegate
		{
			this.isSizeTweenActive = false;
		});
		this.isMoveTweenActive = true;
		this.isSizeTweenActive = true;
	}

	// Token: 0x06003EB8 RID: 16056 RVA: 0x0012B6A0 File Offset: 0x001298A0
	private void ResetAnimationState()
	{
		Tweener tweener = this.moveTween;
		if (tweener != null)
		{
			tweener.Kill(false);
		}
		Tweener tweener2 = this.sizeTween;
		if (tweener2 != null)
		{
			tweener2.Kill(false);
		}
		this.moveTween = null;
		this.sizeTween = null;
		this.previousRectTransform = null;
		this.isMoveTweenActive = false;
		this.isSizeTweenActive = false;
	}

	// Token: 0x06003EB9 RID: 16057 RVA: 0x0012B6F4 File Offset: 0x001298F4
	private void ResetMoveAnimationState()
	{
		Tweener tweener = this.moveTween;
		if (tweener != null)
		{
			tweener.Kill(false);
		}
		this.moveTween = null;
		this.previousRectTransform = null;
		this.isMoveTweenActive = false;
	}

	// Token: 0x06003EBA RID: 16058 RVA: 0x0012B71D File Offset: 0x0012991D
	protected override float GetSpeed()
	{
		return this.currentSpeed * LazyUI.ScaleFactor * Time.deltaTime;
	}

	// Token: 0x06003EBB RID: 16059 RVA: 0x0012B734 File Offset: 0x00129934
	protected override void Update()
	{
		Vector2 direction = LazyInput.GetDirection();
		base.AccelerateSpeed(direction != Vector2.zero);
		this.desiredMovementDelta = this.GetSpeed() * direction;
		if (this.previousRectTransform != null && this.isMoveTweenActive && (this.previousRectTransform.position - base.transform.position).sqrMagnitude > 0.001f)
		{
			this.moveTween.ChangeEndValue(this.previousRectTransform.position, true);
		}
		Vector2 vector = base.transform.position + this.GetSpeed() * direction;
		Vector2 cursorHalfSizeWorld = this.GetCursorHalfSizeWorld();
		vector = this.ClampWithCursorSize(vector, cursorHalfSizeWorld);
		this.currentPos = new Vector3(vector.x, vector.y, this.zPosition);
		base.transform.position = this.currentPos;
		if (this.cursorRectTransform == null || (this.IsTweening() && this.previousRectTransform != null))
		{
			return;
		}
		this.cursorRectTransform.position = ((this.previousRectTransform != null) ? this.previousRectTransform.position : this.currentPos);
	}

	// Token: 0x06003EBC RID: 16060 RVA: 0x0012B878 File Offset: 0x00129A78
	private bool IsTweening()
	{
		return this.isMoveTweenActive || this.isSizeTweenActive;
	}

	// Token: 0x06003EBD RID: 16061 RVA: 0x0012B88C File Offset: 0x00129A8C
	private Vector2 ClampWithCursorSize(Vector2 targetPosition, Vector2 halfCursorSizeWorld)
	{
		if (this.boundsRectTransform == null)
		{
			return targetPosition;
		}
		ValueTuple<Vector2, Vector2> sides = this.GetSides();
		Vector2 item = sides.Item1;
		Vector2 item2 = sides.Item2;
		float num = item.x + halfCursorSizeWorld.x;
		float num2 = item2.x - halfCursorSizeWorld.x;
		float num3 = item.y + halfCursorSizeWorld.y;
		float num4 = item2.y - halfCursorSizeWorld.y;
		float num5 = ((num <= num2) ? Mathf.Clamp(targetPosition.x, num, num2) : ((item.x + item2.x) * 0.5f));
		float num6 = ((num3 <= num4) ? Mathf.Clamp(targetPosition.y, num3, num4) : ((item.y + item2.y) * 0.5f));
		return new Vector2(num5, num6);
	}

	// Token: 0x06003EBE RID: 16062 RVA: 0x0012B950 File Offset: 0x00129B50
	private Vector2 GetCursorHalfSizeWorld()
	{
		if (this.cursorRectTransform == null)
		{
			return Vector2.zero;
		}
		this.cursorRectTransform.GetWorldCorners(MapVirtualCursor.cornersBuffer);
		float num = Mathf.Abs(MapVirtualCursor.cornersBuffer[3].x - MapVirtualCursor.cornersBuffer[0].x);
		float num2 = Mathf.Abs(MapVirtualCursor.cornersBuffer[1].y - MapVirtualCursor.cornersBuffer[0].y);
		return new Vector2(num * 0.5f, num2 * 0.5f);
	}

	// Token: 0x06003EBF RID: 16063 RVA: 0x0012B9E0 File Offset: 0x00129BE0
	protected override void Awake()
	{
		base.Awake();
		if (this.cursorRectTransform != null)
		{
			this.initialCursorSizeDelta = this.cursorRectTransform.sizeDelta;
		}
		this.currentPos = base.transform.position;
	}

	// Token: 0x06003EC0 RID: 16064 RVA: 0x0012BA18 File Offset: 0x00129C18
	private void OnDrawGizmosSelected()
	{
		if (this.boundsRectTransform == null)
		{
			return;
		}
		Gizmos.color = Color.yellow;
		ValueTuple<Vector2, Vector2> sides = this.GetSides();
		Vector2 item = sides.Item1;
		Vector2 item2 = sides.Item2;
		Vector3 vector = new Vector3((item.x + item2.x) * 0.5f, (item.y + item2.y) * 0.5f, base.transform.position.z);
		Vector3 vector2 = new Vector3(item2.x - item.x, item2.y - item.y, 0.01f);
		Gizmos.DrawWireCube(vector, vector2);
	}

	// Token: 0x06003EC1 RID: 16065 RVA: 0x0012BAB8 File Offset: 0x00129CB8
	[return: TupleElementNames(new string[] { "bottomLeft", "topUp" })]
	private ValueTuple<Vector2, Vector2> GetSides()
	{
		if (this.boundsRectTransform == null)
		{
			return new ValueTuple<Vector2, Vector2>(Vector2.zero, Vector2.zero);
		}
		Vector3[] array = new Vector3[4];
		this.boundsRectTransform.GetWorldCorners(array);
		Vector2 vector = array[0];
		Vector2 vector2 = array[2];
		return new ValueTuple<Vector2, Vector2>(vector, vector2);
	}

	// Token: 0x0400313F RID: 12607
	private static readonly Vector3[] cornersBuffer = new Vector3[4];

	// Token: 0x04003140 RID: 12608
	[SerializeField]
	private RectTransform cursorRectTransform;

	// Token: 0x04003141 RID: 12609
	private Vector2 initialCursorSizeDelta;

	// Token: 0x04003142 RID: 12610
	private Tweener moveTween;

	// Token: 0x04003143 RID: 12611
	private Tweener sizeTween;

	// Token: 0x04003144 RID: 12612
	private float animationDuration = 0.3f;

	// Token: 0x04003145 RID: 12613
	private Ease animationEase = Ease.OutQuart;

	// Token: 0x04003146 RID: 12614
	private RectTransform previousRectTransform;

	// Token: 0x04003147 RID: 12615
	private RectTransform boundsRectTransform;

	// Token: 0x04003148 RID: 12616
	private Vector3 currentPos;

	// Token: 0x04003149 RID: 12617
	private Vector2 desiredMovementDelta;

	// Token: 0x0400314A RID: 12618
	private bool isMoveTweenActive;

	// Token: 0x0400314B RID: 12619
	private bool isSizeTweenActive;
}
