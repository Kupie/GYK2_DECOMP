using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000865 RID: 2149
public class UISwitchButton : MonoBehaviour
{
	// Token: 0x17000829 RID: 2089
	// (get) Token: 0x060036EC RID: 14060 RVA: 0x00109CDD File Offset: 0x00107EDD
	// (set) Token: 0x060036ED RID: 14061 RVA: 0x00109CE5 File Offset: 0x00107EE5
	public bool IsInteractable
	{
		get
		{
			return this.isInteractable;
		}
		set
		{
			this.isInteractable = value;
			this.RefreshNavigationButtons();
		}
	}

	// Token: 0x1700082A RID: 2090
	// (get) Token: 0x060036EE RID: 14062 RVA: 0x00109CF4 File Offset: 0x00107EF4
	public int CurrentFieldIndex
	{
		get
		{
			return this.currentFieldIndex;
		}
	}

	// Token: 0x1700082B RID: 2091
	// (get) Token: 0x060036EF RID: 14063 RVA: 0x00109CFC File Offset: 0x00107EFC
	public string CurrentFieldData
	{
		get
		{
			return this.fields[this.currentFieldIndex];
		}
	}

	// Token: 0x060036F0 RID: 14064 RVA: 0x00109D0B File Offset: 0x00107F0B
	private void Awake()
	{
		this.increaseButton.onClick.AddListener(new UnityAction(this.IncreaseSlider));
		this.decreaseButton.onClick.AddListener(new UnityAction(this.DecreaseSlider));
	}

	// Token: 0x060036F1 RID: 14065 RVA: 0x00109D48 File Offset: 0x00107F48
	public void Initialize(Action<int> onChangedCallback, string[] fields, int currentFieldIndex = 0, string header = "", GameKey[] decreaseKeys = null, GameKey[] increaseKeys = null, bool loopNavigation = true)
	{
		this.onChangedCallback = onChangedCallback;
		this.fields = fields;
		this.decreaseKeys = ((decreaseKeys != null && decreaseKeys.Length != 0) ? decreaseKeys : UISwitchButton.defaultDecreaseKeys);
		this.increaseKeys = ((increaseKeys != null && increaseKeys.Length != 0) ? increaseKeys : UISwitchButton.defaultIncreaseKeys);
		this.loopNavigation = loopNavigation;
		this.UpdateField(currentFieldIndex, false);
		if (!string.IsNullOrEmpty(header) && this.headerLabel != null)
		{
			this.headerLabel.text = header;
		}
	}

	// Token: 0x060036F2 RID: 14066 RVA: 0x00109DC7 File Offset: 0x00107FC7
	public void UpdateField(int currentFieldIndex, bool fireCallback = true)
	{
		if (currentFieldIndex >= this.fields.Length)
		{
			currentFieldIndex = this.fields.Length - 1;
		}
		else if (currentFieldIndex < 0)
		{
			currentFieldIndex = 0;
		}
		this.currentFieldIndex = currentFieldIndex;
		this.Apply(fireCallback);
		this.RefreshNavigationButtons();
	}

	// Token: 0x060036F3 RID: 14067 RVA: 0x00109E00 File Offset: 0x00108000
	private void IncreaseSlider()
	{
		int num = this.currentFieldIndex + 1;
		if (num == this.fields.Length)
		{
			if (!this.loopNavigation)
			{
				return;
			}
			num = 0;
		}
		this.currentFieldIndex = num;
		this.Apply(true);
		this.RefreshNavigationButtons();
	}

	// Token: 0x060036F4 RID: 14068 RVA: 0x00109E40 File Offset: 0x00108040
	private void DecreaseSlider()
	{
		int num = this.currentFieldIndex - 1;
		if (num < 0)
		{
			if (!this.loopNavigation)
			{
				return;
			}
			num = this.fields.Length - 1;
		}
		this.currentFieldIndex = num;
		this.Apply(true);
		this.RefreshNavigationButtons();
	}

	// Token: 0x060036F5 RID: 14069 RVA: 0x00109E82 File Offset: 0x00108082
	private void Apply(bool fireOnChanged = true)
	{
		this.amountLabel.text = this.fields[this.currentFieldIndex];
		if (fireOnChanged)
		{
			Action<int> action = this.onChangedCallback;
			if (action == null)
			{
				return;
			}
			action(this.currentFieldIndex);
		}
	}

	// Token: 0x060036F6 RID: 14070 RVA: 0x00109EB8 File Offset: 0x001080B8
	private void RefreshNavigationButtons()
	{
		bool flag = this.fields != null && this.fields.Length != 0;
		this.decreaseButton.interactable = this.isInteractable && flag && (this.loopNavigation || this.currentFieldIndex > 0);
		this.increaseButton.interactable = this.isInteractable && flag && (this.loopNavigation || this.currentFieldIndex < this.fields.Length - 1);
	}

	// Token: 0x060036F7 RID: 14071 RVA: 0x00109F3C File Offset: 0x0010813C
	public void ReinitLabels(string[] fields)
	{
		this.fields = fields;
		this.amountLabel.text = fields[this.currentFieldIndex];
		this.RefreshNavigationButtons();
	}

	// Token: 0x060036F8 RID: 14072 RVA: 0x00109F5E File Offset: 0x0010815E
	public void SetCustomLabelText(string text)
	{
		this.amountLabel.text = text;
	}

	// Token: 0x060036F9 RID: 14073 RVA: 0x00109F6C File Offset: 0x0010816C
	private void Update()
	{
		if (!this.IsInteractable)
		{
			return;
		}
		if (!LazyInput.IsGamepadActive || !this.gamepadNavigationItem.IsFocused)
		{
			return;
		}
		if (UISwitchButton.IsAnyKeyDown(this.decreaseKeys))
		{
			this.DecreaseSlider();
		}
		if (UISwitchButton.IsAnyKeyDown(this.increaseKeys))
		{
			this.IncreaseSlider();
		}
	}

	// Token: 0x060036FA RID: 14074 RVA: 0x00109FC0 File Offset: 0x001081C0
	private static bool IsAnyKeyDown(IReadOnlyList<GameKey> keys)
	{
		for (int i = 0; i < keys.Count; i++)
		{
			if (LazyInput.GetKeyDown(keys[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04002BD5 RID: 11221
	[SerializeField]
	private LazyButton increaseButton;

	// Token: 0x04002BD6 RID: 11222
	[SerializeField]
	private LazyButton decreaseButton;

	// Token: 0x04002BD7 RID: 11223
	[SerializeField]
	private TextMeshProUGUI amountLabel;

	// Token: 0x04002BD8 RID: 11224
	[SerializeField]
	private TextMeshProUGUI headerLabel;

	// Token: 0x04002BD9 RID: 11225
	[Space]
	[SerializeField]
	private GamepadNavigationItem gamepadNavigationItem;

	// Token: 0x04002BDA RID: 11226
	private static readonly GameKey[] defaultDecreaseKeys = new GameKey[] { GameKey.DecSlider };

	// Token: 0x04002BDB RID: 11227
	private static readonly GameKey[] defaultIncreaseKeys = new GameKey[] { GameKey.IncSlider };

	// Token: 0x04002BDC RID: 11228
	private IReadOnlyList<GameKey> decreaseKeys = UISwitchButton.defaultDecreaseKeys;

	// Token: 0x04002BDD RID: 11229
	private IReadOnlyList<GameKey> increaseKeys = UISwitchButton.defaultIncreaseKeys;

	// Token: 0x04002BDE RID: 11230
	private int currentFieldIndex;

	// Token: 0x04002BDF RID: 11231
	private string[] fields;

	// Token: 0x04002BE0 RID: 11232
	private Action<int> onChangedCallback;

	// Token: 0x04002BE1 RID: 11233
	private bool isInteractable = true;

	// Token: 0x04002BE2 RID: 11234
	private bool loopNavigation = true;
}
