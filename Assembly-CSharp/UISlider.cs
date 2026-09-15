using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200083F RID: 2111
public class UISlider : MonoBehaviour
{
	// Token: 0x060035E1 RID: 13793 RVA: 0x00102DDC File Offset: 0x00100FDC
	private void Awake()
	{
		this.increaseButton.onClick.AddListener(delegate
		{
			this.Step(1, true);
		});
		this.decreaseButton.onClick.AddListener(delegate
		{
			this.Step(-1, true);
		});
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.UpdateValue));
	}

	// Token: 0x060035E2 RID: 13794 RVA: 0x00102E40 File Offset: 0x00101040
	public void Initialize(Action<float> onChangedCallback, float currentValue, float step = 10f)
	{
		this.onChangedCallback = onChangedCallback;
		this.step = step;
		this.suppressClickSound = true;
		this.slider.maxValue = 100f / step;
		this.slider.value = currentValue / step;
		this.suppressClickSound = false;
		this.amountLabel.text = currentValue.ToString();
	}

	// Token: 0x060035E3 RID: 13795 RVA: 0x00102E9C File Offset: 0x0010109C
	private void UpdateValue(float value)
	{
		float num = this.slider.value * this.step;
		this.amountLabel.text = num.ToString();
		Action<float> action = this.onChangedCallback;
		if (action != null)
		{
			action(num);
		}
		if (!this.suppressClickSound)
		{
			UISliderClickSound.Play();
		}
	}

	// Token: 0x060035E4 RID: 13796 RVA: 0x00102EED File Offset: 0x001010ED
	private void IncreaseSlider()
	{
		this.Step(1, false);
	}

	// Token: 0x060035E5 RID: 13797 RVA: 0x00102EF7 File Offset: 0x001010F7
	private void DecreaseSlider()
	{
		this.Step(-1, false);
	}

	// Token: 0x060035E6 RID: 13798 RVA: 0x00102F01 File Offset: 0x00101101
	private void Step(int delta, bool suppressClickSound)
	{
		this.suppressClickSound = suppressClickSound;
		this.slider.value += (float)delta;
		this.suppressClickSound = false;
	}

	// Token: 0x060035E7 RID: 13799 RVA: 0x00102F28 File Offset: 0x00101128
	private void Update()
	{
		if (LazyInput.IsGamepadActive && this.gamepadNavigationItem.IsFocused)
		{
			if (UISlider.IsAnyKeyDown(this.decreaseKeys, UISlider.defaultDecreaseKeys))
			{
				this.DecreaseSlider();
			}
			if (UISlider.IsAnyKeyDown(this.increaseKeys, UISlider.defaultIncreaseKeys))
			{
				this.IncreaseSlider();
			}
		}
	}

	// Token: 0x060035E8 RID: 13800 RVA: 0x00102F7C File Offset: 0x0010117C
	private static bool IsAnyKeyDown(IReadOnlyList<GameKey> keys, GameKey[] fallbackKeys)
	{
		IReadOnlyList<GameKey> readOnlyList = ((keys != null && keys.Count > 0) ? keys : fallbackKeys);
		for (int i = 0; i < readOnlyList.Count; i++)
		{
			if (LazyInput.GetKeyDown(readOnlyList[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04002B28 RID: 11048
	[SerializeField]
	private Slider slider;

	// Token: 0x04002B29 RID: 11049
	[SerializeField]
	private Button increaseButton;

	// Token: 0x04002B2A RID: 11050
	[SerializeField]
	private Button decreaseButton;

	// Token: 0x04002B2B RID: 11051
	[SerializeField]
	private TextMeshProUGUI amountLabel;

	// Token: 0x04002B2C RID: 11052
	[SerializeField]
	private List<GameKey> decreaseKeys = new List<GameKey> { GameKey.DecSlider };

	// Token: 0x04002B2D RID: 11053
	[SerializeField]
	private List<GameKey> increaseKeys = new List<GameKey> { GameKey.IncSlider };

	// Token: 0x04002B2E RID: 11054
	private static readonly GameKey[] defaultDecreaseKeys = new GameKey[] { GameKey.DecSlider };

	// Token: 0x04002B2F RID: 11055
	private static readonly GameKey[] defaultIncreaseKeys = new GameKey[] { GameKey.IncSlider };

	// Token: 0x04002B30 RID: 11056
	private Action<float> onChangedCallback;

	// Token: 0x04002B31 RID: 11057
	private float step = 10f;

	// Token: 0x04002B32 RID: 11058
	private bool suppressClickSound;

	// Token: 0x04002B33 RID: 11059
	[SerializeField]
	private GamepadNavigationItem gamepadNavigationItem;
}
