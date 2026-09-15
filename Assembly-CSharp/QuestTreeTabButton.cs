using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000952 RID: 2386
public class QuestTreeTabButton : MonoBehaviour
{
	// Token: 0x1700097D RID: 2429
	// (get) Token: 0x06003EF2 RID: 16114 RVA: 0x0012D1E5 File Offset: 0x0012B3E5
	// (set) Token: 0x06003EF3 RID: 16115 RVA: 0x0012D1ED File Offset: 0x0012B3ED
	public string Tab { get; private set; }

	// Token: 0x06003EF4 RID: 16116 RVA: 0x0012D1F8 File Offset: 0x0012B3F8
	public void Init(string tab, Sprite sprite, string langToken, Action<QuestTreeTabButton> onPressAction)
	{
		this.OnDeselect();
		this.button.onExit.RemoveAllListeners();
		this.button.onExit.AddListener(new UnityAction(this.OnDeselect));
		this.button.onEnter.RemoveAllListeners();
		this.button.onEnter.AddListener(new UnityAction(this.OnSelect));
		this.Tab = tab;
		this.localizedLabel.langToken = langToken;
		this.localizedLabel.Localize();
		this.button.onClick.RemoveAllListeners();
		this.button.onClick.AddListener(delegate
		{
			Action<QuestTreeTabButton> onPressAction2 = onPressAction;
			if (onPressAction2 == null)
			{
				return;
			}
			onPressAction2(this);
		});
		this.button.onClick.AddListener(new UnityAction(this.OnDeselect));
		this.iconActive.sprite = sprite;
		this.iconInactive.sprite = sprite;
	}

	// Token: 0x06003EF5 RID: 16117 RVA: 0x0012D2F6 File Offset: 0x0012B4F6
	public void SetState(bool active)
	{
		this.iconActive.BlueColorReplace(this.activeColor);
		this.activeGameObject.SetActive(active);
		this.inactiveGameObject.SetActive(!active);
		this.button.interactable = !active;
	}

	// Token: 0x06003EF6 RID: 16118 RVA: 0x0012D333 File Offset: 0x0012B533
	private void OnDisable()
	{
		this.OnDeselect();
	}

	// Token: 0x06003EF7 RID: 16119 RVA: 0x0012D33B File Offset: 0x0012B53B
	private void OnSelect()
	{
		this.iconInactive.BlueColorReplace(this.inactiveColorSelected);
	}

	// Token: 0x06003EF8 RID: 16120 RVA: 0x0012D34E File Offset: 0x0012B54E
	private void OnDeselect()
	{
		this.iconInactive.BlueColorReplace(this.inactiveColor);
	}

	// Token: 0x0400317D RID: 12669
	[SerializeField]
	private LazyButton button;

	// Token: 0x0400317E RID: 12670
	[SerializeField]
	private GameObject activeGameObject;

	// Token: 0x0400317F RID: 12671
	[SerializeField]
	private GameObject inactiveGameObject;

	// Token: 0x04003180 RID: 12672
	[SerializeField]
	private Image iconActive;

	// Token: 0x04003181 RID: 12673
	[SerializeField]
	private Image iconInactive;

	// Token: 0x04003182 RID: 12674
	[SerializeField]
	private LocalizedLabel localizedLabel;

	// Token: 0x04003183 RID: 12675
	[SerializeField]
	private Color activeColor;

	// Token: 0x04003184 RID: 12676
	[SerializeField]
	private Color inactiveColor;

	// Token: 0x04003185 RID: 12677
	[SerializeField]
	private Color inactiveColorSelected;
}
