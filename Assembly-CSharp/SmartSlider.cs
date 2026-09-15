using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020009EE RID: 2542
public class SmartSlider : MonoBehaviour
{
	// Token: 0x17000A6F RID: 2671
	// (get) Token: 0x06004460 RID: 17504 RVA: 0x001448BF File Offset: 0x00142ABF
	public int Value
	{
		get
		{
			return this.min + Mathf.RoundToInt(this.slider.value);
		}
	}

	// Token: 0x06004461 RID: 17505 RVA: 0x001448D8 File Offset: 0x00142AD8
	public void Init()
	{
		this.slider.onValueChanged.AddListener(delegate(float _)
		{
			this.OnSliderChanged();
		});
		this.inputField.onValueChanged.AddListener(delegate(string _)
		{
			this.OnInputFieldChanged();
		});
		this.inputField.onSubmit.AddListener(delegate(string _)
		{
			this.OnInputFieldSubmit();
		});
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnInputFieldEndEdit));
	}

	// Token: 0x06004462 RID: 17506 RVA: 0x00144958 File Offset: 0x00142B58
	public void Open(int value, int min, int max, Action<int> onValueChanged, bool inputFieldEnabled = false, bool gameKeysEnabled = false, int stepForGameKeys = 1, int snapStep = 1)
	{
		this.min = min;
		this.max = max;
		this.snapStep = ((snapStep > 1 && max >= snapStep) ? snapStep : 1);
		this.prevValue = -9999;
		this.holdRepeat.Reset();
		this.skipSnap = false;
		this.slider.minValue = 0f;
		this.slider.maxValue = (float)(max - min);
		this.slider.wholeNumbers = true;
		if (this.minCounter != null)
		{
			this.minCounter.text = this.min.ToString();
		}
		if (this.maxCounter != null)
		{
			this.maxCounter.text = this.max.ToString();
		}
		this.onValueChanged = onValueChanged;
		Selectable selectable = this.inputField;
		this.inputFieldEnabled = inputFieldEnabled;
		selectable.interactable = inputFieldEnabled;
		this.gameKeysEnabled = gameKeysEnabled;
		this.stepForGameKeys = ((this.snapStep > 1) ? this.snapStep : ((stepForGameKeys != 0) ? stepForGameKeys : 1));
		if (this.snapStep > 1)
		{
			value = this.SnapToNearest(value);
		}
		this.SetValue(value, false);
		this.inputField.SetTextWithoutNotify(this.Value.ToString());
		this.inputField.textComponent.textWrappingMode = TextWrappingModes.NoWrap;
	}

	// Token: 0x06004463 RID: 17507 RVA: 0x00144AA4 File Offset: 0x00142CA4
	private void SetValue(int newValue, bool invokeCallback = true)
	{
		newValue = Math.Clamp(newValue, this.min, this.max);
		if (this.snapStep > 1 && !this.skipSnap)
		{
			newValue = this.SnapToNearest(newValue);
		}
		this.slider.SetValueWithoutNotify((float)(newValue - this.min));
		this.UpdateInputFieldFromValue();
		this.prevValue = this.Value;
		if (invokeCallback && this.onValueChanged != null)
		{
			this.onValueChanged(this.Value);
		}
	}

	// Token: 0x06004464 RID: 17508 RVA: 0x00144B24 File Offset: 0x00142D24
	public void OnSliderChanged()
	{
		this.skipSnap = false;
		if (this.snapStep > 1)
		{
			int num = this.min + Mathf.RoundToInt(this.slider.value);
			int num2 = this.SnapToNearest(num);
			if (num2 != num)
			{
				this.slider.SetValueWithoutNotify((float)(num2 - this.min));
			}
		}
		int value = this.Value;
		if (this.prevValue == value)
		{
			return;
		}
		this.prevValue = value;
		this.UpdateInputFieldFromValue();
		UISliderClickSound.Play();
		if (this.onValueChanged != null)
		{
			this.onValueChanged(value);
		}
	}

	// Token: 0x06004465 RID: 17509 RVA: 0x00144BB0 File Offset: 0x00142DB0
	public void OnInputFieldChanged()
	{
		if (!this.inputFieldEnabled)
		{
			return;
		}
		int num = 0;
		if (!int.TryParse(this.inputField.text, out num))
		{
			return;
		}
		if (num < this.min)
		{
			num = this.min;
		}
		if (num > this.max)
		{
			num = this.max;
		}
		this.skipSnap = true;
		if (num != this.Value)
		{
			this.SetValue(num, true);
		}
		if (!this.inputField.isFocused)
		{
			this.inputField.SetTextWithoutNotify(num.ToString());
			this.inputField.textComponent.textWrappingMode = TextWrappingModes.NoWrap;
		}
	}

	// Token: 0x06004466 RID: 17510 RVA: 0x00144C45 File Offset: 0x00142E45
	public void OnInputFieldSubmit()
	{
		if (!this.inputFieldEnabled)
		{
			return;
		}
		this.inputField.DeactivateInputField(false);
	}

	// Token: 0x06004467 RID: 17511 RVA: 0x00144C5C File Offset: 0x00142E5C
	private void OnInputFieldEndEdit(string _)
	{
		this.ApplyInputFieldValue();
	}

	// Token: 0x06004468 RID: 17512 RVA: 0x00144C64 File Offset: 0x00142E64
	private void ApplyInputFieldValue()
	{
		if (!this.inputFieldEnabled || this.inputField.isFocused)
		{
			return;
		}
		int num;
		if (!int.TryParse(this.inputField.text, out num))
		{
			num = this.min;
		}
		num = Math.Clamp(num, this.min, this.max);
		this.skipSnap = true;
		this.SetValue(num, true);
		this.inputField.SetTextWithoutNotify(this.Value.ToString());
		this.inputField.textComponent.textWrappingMode = TextWrappingModes.NoWrap;
	}

	// Token: 0x06004469 RID: 17513 RVA: 0x00144CF0 File Offset: 0x00142EF0
	private void Update()
	{
		if (this.inputField.isFocused)
		{
			this.holdRepeat.Reset();
			return;
		}
		int num = HoldRepeatValueChanger.GetPointerHoldDirection(this.plusBtn, this.minusBtn);
		if (num == 0 && this.gameKeysEnabled)
		{
			num = this.GetInputDirection();
		}
		this.ApplyHoldSettings();
		this.holdRepeat.Tick(num, delegate(int delta)
		{
			this.skipSnap = false;
			this.ChangeValue(delta, !this.IsButtonHoldActive());
		});
		if (num != 0 || !this.gameKeysEnabled)
		{
			return;
		}
		if (LazyInput.GetKeyDown(GameKey.ItemCountWindow_Max) || LazyInput.GetKeyDown(GameKey.MaxSlider))
		{
			this.skipSnap = false;
			this.SetValue((this.snapStep > 1) ? this.GetLastSnappedValue() : this.max, true);
			return;
		}
		if (LazyInput.GetKeyDown(GameKey.ItemCountWindow_Min) || LazyInput.GetKeyDown(GameKey.MinSlider))
		{
			this.skipSnap = false;
			this.SetValue((this.snapStep > 1) ? this.GetFirstSnappedValue() : this.min, true);
		}
	}

	// Token: 0x0600446A RID: 17514 RVA: 0x00144DDF File Offset: 0x00142FDF
	private bool IsButtonHoldActive()
	{
		return HoldRepeatValueChanger.GetPointerHoldDirection(this.plusBtn, this.minusBtn) != 0;
	}

	// Token: 0x0600446B RID: 17515 RVA: 0x00144DF8 File Offset: 0x00142FF8
	private void ApplyHoldSettings()
	{
		this.holdRepeat.ChangeValueTime = this.changeValueTime;
		this.holdRepeat.HoldLowChangeValueTime = this.holdLowChangeValueTime;
		this.holdRepeat.HoldMediumChangeValueTime = this.holdMediumChangeValueTime;
		this.holdRepeat.HoldHighChangeValueTime = this.holdHighChangeValueTime;
		this.holdRepeat.InitialStep = this.stepForGameKeys;
		this.holdRepeat.LowSpeedChangeValue = this.lowSpeedChangeValue;
		this.holdRepeat.MediumSpeedChangeValue = this.mediumSpeedChangeValue;
		this.holdRepeat.HighSpeedChangeValue = this.highSpeedChangeValue;
	}

	// Token: 0x0600446C RID: 17516 RVA: 0x00144E90 File Offset: 0x00143090
	private int GetInputDirection()
	{
		bool flag = LazyInput.GetKey(GameKey.ItemCountWindow_Increase) || LazyInput.GetKeyDown(GameKey.ItemCountWindow_Increase) || LazyInput.GetKey(GameKey.IncSlider) || LazyInput.GetKeyDown(GameKey.IncSlider) || LazyInput.GetKey(GameKey.Right) || LazyInput.GetKeyDown(GameKey.Right) || LazyInput.GetKey(GameKey.DpadRight) || LazyInput.GetKeyDown(GameKey.DpadRight);
		bool flag2 = LazyInput.GetKey(GameKey.ItemCountWindow_Decrease) || LazyInput.GetKeyDown(GameKey.ItemCountWindow_Decrease) || LazyInput.GetKey(GameKey.DecSlider) || LazyInput.GetKeyDown(GameKey.DecSlider) || LazyInput.GetKey(GameKey.Left) || LazyInput.GetKeyDown(GameKey.Left) || LazyInput.GetKey(GameKey.DpadLeft) || LazyInput.GetKeyDown(GameKey.DpadLeft);
		float x = LazyInput.GetDirection().x;
		if (x > 0.4f)
		{
			flag = true;
		}
		else if (x < -0.4f)
		{
			flag2 = true;
		}
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

	// Token: 0x0600446D RID: 17517 RVA: 0x00144F90 File Offset: 0x00143190
	private void ChangeValue(int delta, bool playClickSound = true)
	{
		int value = this.Value;
		int num = ((this.snapStep > 1) ? this.GetSteppedValue(this.Value, delta) : (this.Value + delta));
		this.SetValue(num, true);
		if (playClickSound && this.Value != value)
		{
			UISliderClickSound.Play();
		}
	}

	// Token: 0x0600446E RID: 17518 RVA: 0x00144FE0 File Offset: 0x001431E0
	private void UpdateInputFieldFromValue()
	{
		if (this.inputField.isFocused)
		{
			return;
		}
		string text = this.Value.ToString();
		if (this.inputField.text != text)
		{
			this.inputField.SetTextWithoutNotify(text);
			this.inputField.textComponent.textWrappingMode = TextWrappingModes.NoWrap;
		}
	}

	// Token: 0x0600446F RID: 17519 RVA: 0x0014503C File Offset: 0x0014323C
	private int GetSteppedValue(int current, int delta)
	{
		if (delta == 0)
		{
			return current;
		}
		int num;
		if (delta > 0)
		{
			num = ((current % this.snapStep == 0) ? (current + this.snapStep) : (this.FloorToStep(current) + this.snapStep));
			if (Math.Abs(delta) > this.snapStep)
			{
				num = this.CeilToStep(current + delta);
			}
		}
		else
		{
			num = ((current % this.snapStep == 0) ? (current - this.snapStep) : this.FloorToStep(current));
			if (Math.Abs(delta) > this.snapStep)
			{
				num = this.FloorToStep(current + delta);
			}
		}
		int lastSnappedValue = this.GetLastSnappedValue();
		int firstSnappedValue = this.GetFirstSnappedValue();
		if (num > this.max)
		{
			if (lastSnappedValue <= current)
			{
				return current;
			}
			return lastSnappedValue;
		}
		else
		{
			if (num >= this.min)
			{
				return num;
			}
			if (firstSnappedValue >= current)
			{
				return current;
			}
			return firstSnappedValue;
		}
	}

	// Token: 0x06004470 RID: 17520 RVA: 0x001450F4 File Offset: 0x001432F4
	private int SnapToNearest(int value)
	{
		int num = this.FloorToStep(value);
		int num2 = num + this.snapStep;
		int num3 = ((value - num < num2 - value) ? num : num2);
		int lastSnappedValue = this.GetLastSnappedValue();
		int firstSnappedValue = this.GetFirstSnappedValue();
		if (num3 > this.max)
		{
			return lastSnappedValue;
		}
		if (num3 < this.min)
		{
			return firstSnappedValue;
		}
		if (num3 > lastSnappedValue)
		{
			return lastSnappedValue;
		}
		if (num3 < firstSnappedValue)
		{
			return firstSnappedValue;
		}
		return num3;
	}

	// Token: 0x06004471 RID: 17521 RVA: 0x00145154 File Offset: 0x00143354
	private int GetFirstSnappedValue()
	{
		int num = this.CeilToStep(this.min);
		if (num > this.max)
		{
			return this.min;
		}
		return num;
	}

	// Token: 0x06004472 RID: 17522 RVA: 0x00145180 File Offset: 0x00143380
	private int GetLastSnappedValue()
	{
		int num = this.FloorToStep(this.max);
		if (num < this.min)
		{
			return this.max;
		}
		return num;
	}

	// Token: 0x06004473 RID: 17523 RVA: 0x001451AB File Offset: 0x001433AB
	private int FloorToStep(int value)
	{
		return value / this.snapStep * this.snapStep;
	}

	// Token: 0x06004474 RID: 17524 RVA: 0x001451BC File Offset: 0x001433BC
	private int CeilToStep(int value)
	{
		int num = this.FloorToStep(value);
		if (num != value)
		{
			return num + this.snapStep;
		}
		return value;
	}

	// Token: 0x0400354F RID: 13647
	public const int GAMEPAD_FAST_SLIDER_CHANGE_VALUE = 10;

	// Token: 0x04003550 RID: 13648
	[SerializeField]
	private TextMeshProUGUI minCounter;

	// Token: 0x04003551 RID: 13649
	[SerializeField]
	private TextMeshProUGUI maxCounter;

	// Token: 0x04003552 RID: 13650
	[SerializeField]
	private TMP_InputField inputField;

	// Token: 0x04003553 RID: 13651
	[SerializeField]
	private Slider slider;

	// Token: 0x04003554 RID: 13652
	[SerializeField]
	private LazyButton minusBtn;

	// Token: 0x04003555 RID: 13653
	[SerializeField]
	private LazyButton plusBtn;

	// Token: 0x04003556 RID: 13654
	[SerializeField]
	[Space]
	private float changeValueTime = 0.2f;

	// Token: 0x04003557 RID: 13655
	[SerializeField]
	private float holdLowChangeValueTime = 0.5f;

	// Token: 0x04003558 RID: 13656
	[SerializeField]
	private float holdMediumChangeValueTime = 2f;

	// Token: 0x04003559 RID: 13657
	[SerializeField]
	private float holdHighChangeValueTime = 4f;

	// Token: 0x0400355A RID: 13658
	[SerializeField]
	[Space]
	private int lowSpeedChangeValue = 1;

	// Token: 0x0400355B RID: 13659
	[SerializeField]
	private int mediumSpeedChangeValue = 5;

	// Token: 0x0400355C RID: 13660
	[SerializeField]
	private int highSpeedChangeValue = 10;

	// Token: 0x0400355D RID: 13661
	private int min;

	// Token: 0x0400355E RID: 13662
	private int max;

	// Token: 0x0400355F RID: 13663
	private int stepForGameKeys = 1;

	// Token: 0x04003560 RID: 13664
	private int snapStep = 1;

	// Token: 0x04003561 RID: 13665
	private int prevValue;

	// Token: 0x04003562 RID: 13666
	private bool inputFieldEnabled;

	// Token: 0x04003563 RID: 13667
	private bool gameKeysEnabled;

	// Token: 0x04003564 RID: 13668
	private bool skipSnap;

	// Token: 0x04003565 RID: 13669
	private readonly HoldRepeatValueChanger holdRepeat = new HoldRepeatValueChanger();

	// Token: 0x04003566 RID: 13670
	private Action<int> onValueChanged;
}
