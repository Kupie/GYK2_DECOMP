using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020008FC RID: 2300
public class UIGameBindingElement : MonoBehaviour
{
	// Token: 0x06003C2D RID: 15405 RVA: 0x0011F8B0 File Offset: 0x0011DAB0
	private void Update()
	{
		if (this.isForGamepad)
		{
			return;
		}
		if (this.isChanging)
		{
			foreach (object obj in Enum.GetValues(typeof(KeyCode)))
			{
				KeyCode keyCode = (KeyCode)obj;
				if (Input.GetKeyDown(keyCode))
				{
					if (this.IsForbiddenKey(keyCode))
					{
						break;
					}
					this.isChanging = false;
					this.SubmitChanges(keyCode);
					break;
				}
			}
		}
	}

	// Token: 0x06003C2E RID: 15406 RVA: 0x0011F940 File Offset: 0x0011DB40
	private void SubmitChanges(KeyCode newKey)
	{
		if (newKey == KeyCode.None)
		{
			Debug.LogError("Cannot assign keykode!");
		}
		this.keyBinding.keyCode = newKey;
		this.keyLabel.text = this.GetTextForKeyCode(newKey);
		this.recordObject.gameObject.SetActive(false);
		Action<UIGameBindingElement> action = this.onSubmitBinding;
		if (action == null)
		{
			return;
		}
		action(this);
	}

	// Token: 0x06003C2F RID: 15407 RVA: 0x0011F99C File Offset: 0x0011DB9C
	private bool IsForbiddenKey(KeyCode key)
	{
		return (key >= KeyCode.F1 && key <= KeyCode.F12) || (key == KeyCode.LeftWindows || key == KeyCode.RightWindows) || (key == KeyCode.LeftMeta || key == KeyCode.RightMeta) || key == KeyCode.Menu || key == KeyCode.Print || key == KeyCode.ScrollLock || key == KeyCode.Break || key == KeyCode.Pause || key == KeyCode.Numlock || key == KeyCode.Tilde || key == KeyCode.BackQuote || key == KeyCode.Mouse0 || key == KeyCode.Escape;
	}

	// Token: 0x06003C30 RID: 15408 RVA: 0x0011FA38 File Offset: 0x0011DC38
	public void InitializedForGamepad(GamepadBinding gamepadBinding)
	{
		this.layoutElement.minHeight = 30f;
		this.actionLabel.text = LLBase.L(gamepadBinding.localeId);
		this.keyLabel.text = ControllerIconLibrary.GetIconId(gamepadBinding.gameKey, null, true);
		this.button.gameObject.SetActive(false);
		this.recordObject.gameObject.SetActive(false);
		this.isForGamepad = true;
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x0011FAAC File Offset: 0x0011DCAC
	public void Initialize(KeyBinding binding, Action<UIGameBindingElement> onSubmitBinding, Action onChangingBinding)
	{
		this.layoutElement.minHeight = 20f;
		this.isForGamepad = false;
		this.keyBinding = binding;
		this.actionLabel.text = LLBase.L(binding.localeId);
		this.keyLabel.text = this.GetTextForKeyCode(this.keyBinding.keyCode);
		if (binding.keyCode == KeyCode.Escape || binding.keyCode == KeyCode.Mouse0)
		{
			this.button.gameObject.SetActive(false);
			return;
		}
		this.onSubmitBinding = onSubmitBinding;
		this.onChangingBinding = onChangingBinding;
		this.button.onClick.RemoveAllListeners();
		this.button.onClick.AddListener(new UnityAction(this.ChangeBinding));
		this.button.gameObject.SetActive(true);
		this.recordObject.gameObject.SetActive(false);
	}

	// Token: 0x06003C32 RID: 15410 RVA: 0x0011FB8E File Offset: 0x0011DD8E
	public void UpdateKeyLabel()
	{
		this.keyLabel.text = this.GetTextForKeyCode(this.keyBinding.keyCode);
	}

	// Token: 0x06003C33 RID: 15411 RVA: 0x0011FBAC File Offset: 0x0011DDAC
	public void ChangeIsAvailable(bool value)
	{
		this.button.interactable = value;
	}

	// Token: 0x06003C34 RID: 15412 RVA: 0x0011FBBA File Offset: 0x0011DDBA
	private void ChangeBinding()
	{
		this.recordObject.gameObject.SetActive(true);
		this.isChanging = true;
		this.keyLabel.text = "...";
		Action action = this.onChangingBinding;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06003C35 RID: 15413 RVA: 0x0011FBF4 File Offset: 0x0011DDF4
	private string GetTextForKeyCode(KeyCode keyCode)
	{
		return LazySingletonSO<ControllerIconLibrary>.Instance.GetKeycodeString(keyCode);
	}

	// Token: 0x04002F53 RID: 12115
	[SerializeField]
	private TextMeshProUGUI actionLabel;

	// Token: 0x04002F54 RID: 12116
	[SerializeField]
	private TextMeshProUGUI keyLabel;

	// Token: 0x04002F55 RID: 12117
	[SerializeField]
	private GameObject recordObject;

	// Token: 0x04002F56 RID: 12118
	[SerializeField]
	private LazyButton button;

	// Token: 0x04002F57 RID: 12119
	[SerializeField]
	private LayoutElement layoutElement;

	// Token: 0x04002F58 RID: 12120
	public KeyBinding keyBinding;

	// Token: 0x04002F59 RID: 12121
	private Action<UIGameBindingElement> onSubmitBinding;

	// Token: 0x04002F5A RID: 12122
	private Action onChangingBinding;

	// Token: 0x04002F5B RID: 12123
	private bool isChanging;

	// Token: 0x04002F5C RID: 12124
	private bool isForGamepad;
}
