using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200085C RID: 2140
public class HoldRepeatValueChanger
{
	// Token: 0x17000826 RID: 2086
	// (get) Token: 0x060036CB RID: 14027 RVA: 0x0010973F File Offset: 0x0010793F
	public bool IsActive
	{
		get
		{
			return this.currentDirection != 0;
		}
	}

	// Token: 0x060036CC RID: 14028 RVA: 0x0010974C File Offset: 0x0010794C
	public void Press(int direction, Action<int> onDelta)
	{
		if (onDelta == null || direction == 0)
		{
			return;
		}
		int num = ((direction > 0) ? 1 : (-1));
		if (this.currentDirection != num)
		{
			this.Start(num, onDelta);
		}
	}

	// Token: 0x060036CD RID: 14029 RVA: 0x0010977C File Offset: 0x0010797C
	public void Tick(int direction, Action<int> onDelta)
	{
		if (onDelta == null || direction == 0)
		{
			this.Reset();
			return;
		}
		int num = ((direction > 0) ? 1 : (-1));
		if (this.currentDirection != num)
		{
			this.Start(num, onDelta);
			return;
		}
		this.UpdateHold(onDelta);
	}

	// Token: 0x060036CE RID: 14030 RVA: 0x001097B8 File Offset: 0x001079B8
	public void Reset()
	{
		this.holdTimer = 0f;
		this.changeValueTimer = 0f;
		this.speedMode = HoldRepeatValueChanger.SpeedMode.None;
		this.currentDirection = 0;
	}

	// Token: 0x060036CF RID: 14031 RVA: 0x001097E0 File Offset: 0x001079E0
	public static int GetPointerHoldDirection(LazyButton plusButton, LazyButton minusButton)
	{
		bool flag = HoldRepeatValueChanger.IsButtonHeld(plusButton);
		bool flag2 = HoldRepeatValueChanger.IsButtonHeld(minusButton);
		if (flag == flag2)
		{
			return 0;
		}
		if (!flag)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x060036D0 RID: 14032 RVA: 0x00109807 File Offset: 0x00107A07
	public static bool IsKeyHeld(GameKey key)
	{
		return key != null && (LazyInput.GetKey(key) || LazyInput.GetKeyDown(key));
	}

	// Token: 0x060036D1 RID: 14033 RVA: 0x0010981E File Offset: 0x00107A1E
	public static bool IsAnyKeyHeld(GameKey key1, GameKey key2)
	{
		return HoldRepeatValueChanger.IsKeyHeld(key1) || HoldRepeatValueChanger.IsKeyHeld(key2);
	}

	// Token: 0x060036D2 RID: 14034 RVA: 0x00109830 File Offset: 0x00107A30
	public static bool IsShiftHeld()
	{
		return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
	}

	// Token: 0x060036D3 RID: 14035 RVA: 0x0010984C File Offset: 0x00107A4C
	public static int GetAxisHoldDirection(bool vertical)
	{
		float num = (vertical ? LazyInput.GetDirection().y : LazyInput.GetDirection().x);
		if (num > 0.4f)
		{
			return 1;
		}
		if (num < -0.4f)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x060036D4 RID: 14036 RVA: 0x00109888 File Offset: 0x00107A88
	private static bool IsButtonHeld(LazyButton button)
	{
		return button != null && button.isActiveAndEnabled && button.interactable && button.IsPointerHeld;
	}

	// Token: 0x060036D5 RID: 14037 RVA: 0x001098AB File Offset: 0x00107AAB
	private void Start(int sign, Action<int> onDelta)
	{
		this.holdTimer = 0f;
		this.changeValueTimer = 0f;
		this.speedMode = HoldRepeatValueChanger.SpeedMode.None;
		this.currentDirection = sign;
		onDelta(this.GetStartStep() * sign);
	}

	// Token: 0x060036D6 RID: 14038 RVA: 0x001098E0 File Offset: 0x00107AE0
	private void UpdateHold(Action<int> onDelta)
	{
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		if (this.speedMode != HoldRepeatValueChanger.SpeedMode.High)
		{
			this.holdTimer += unscaledDeltaTime;
			if (this.holdTimer.EqualsOrMore(this.HoldHighChangeValueTime, 1E-05f))
			{
				this.speedMode = HoldRepeatValueChanger.SpeedMode.High;
			}
			else if (this.holdTimer.EqualsOrMore(this.HoldMediumChangeValueTime, 1E-05f))
			{
				this.speedMode = HoldRepeatValueChanger.SpeedMode.Medium;
			}
			else if (this.holdTimer.EqualsOrMore(this.HoldLowChangeValueTime, 1E-05f))
			{
				this.speedMode = HoldRepeatValueChanger.SpeedMode.Low;
			}
		}
		this.changeValueTimer += unscaledDeltaTime;
		if (!this.changeValueTimer.EqualsOrMore(this.ChangeValueTime, 1E-05f))
		{
			return;
		}
		int num;
		switch (this.speedMode)
		{
		case HoldRepeatValueChanger.SpeedMode.Low:
			num = this.LowSpeedChangeValue;
			break;
		case HoldRepeatValueChanger.SpeedMode.Medium:
			num = this.MediumSpeedChangeValue;
			break;
		case HoldRepeatValueChanger.SpeedMode.High:
			num = this.HighSpeedChangeValue;
			break;
		default:
			num = 0;
			break;
		}
		int num2 = num;
		if (HoldRepeatValueChanger.IsShiftHeld())
		{
			num2 = Math.Max(num2, this.ShiftStep);
		}
		if (num2 > 0)
		{
			onDelta(num2 * this.currentDirection);
		}
		this.changeValueTimer = 0f;
	}

	// Token: 0x060036D7 RID: 14039 RVA: 0x001099FF File Offset: 0x00107BFF
	private int GetStartStep()
	{
		if (!HoldRepeatValueChanger.IsShiftHeld())
		{
			return this.InitialStep;
		}
		return Math.Max(this.InitialStep, this.ShiftStep);
	}

	// Token: 0x04002BB5 RID: 11189
	public const float AxisDeadZone = 0.4f;

	// Token: 0x04002BB6 RID: 11190
	public float ChangeValueTime = 0.2f;

	// Token: 0x04002BB7 RID: 11191
	public float HoldLowChangeValueTime = 0.5f;

	// Token: 0x04002BB8 RID: 11192
	public float HoldMediumChangeValueTime = 2f;

	// Token: 0x04002BB9 RID: 11193
	public float HoldHighChangeValueTime = 4f;

	// Token: 0x04002BBA RID: 11194
	public int InitialStep = 1;

	// Token: 0x04002BBB RID: 11195
	public int LowSpeedChangeValue = 1;

	// Token: 0x04002BBC RID: 11196
	public int MediumSpeedChangeValue = 5;

	// Token: 0x04002BBD RID: 11197
	public int HighSpeedChangeValue = 10;

	// Token: 0x04002BBE RID: 11198
	public int ShiftStep = 10;

	// Token: 0x04002BBF RID: 11199
	private float changeValueTimer;

	// Token: 0x04002BC0 RID: 11200
	private float holdTimer;

	// Token: 0x04002BC1 RID: 11201
	private HoldRepeatValueChanger.SpeedMode speedMode;

	// Token: 0x04002BC2 RID: 11202
	private int currentDirection;

	// Token: 0x0200085D RID: 2141
	private enum SpeedMode
	{
		// Token: 0x04002BC4 RID: 11204
		None,
		// Token: 0x04002BC5 RID: 11205
		Low,
		// Token: 0x04002BC6 RID: 11206
		Medium,
		// Token: 0x04002BC7 RID: 11207
		High
	}
}
