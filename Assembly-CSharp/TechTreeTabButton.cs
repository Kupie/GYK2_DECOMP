using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000961 RID: 2401
public class TechTreeTabButton : MonoBehaviour
{
	// Token: 0x17000987 RID: 2439
	// (get) Token: 0x06003F47 RID: 16199 RVA: 0x0012F7EF File Offset: 0x0012D9EF
	// (set) Token: 0x06003F48 RID: 16200 RVA: 0x0012F7F7 File Offset: 0x0012D9F7
	public TechTreeTab TechTreeTab { get; private set; }

	// Token: 0x06003F49 RID: 16201 RVA: 0x0012F800 File Offset: 0x0012DA00
	public void Init(TechTreeTab techTreeTab, Sprite sprite, string langToken, Action<TechTreeTabButton> onPressAction)
	{
		this.OnDeselect();
		this.button.onExit.RemoveAllListeners();
		this.button.onExit.AddListener(new UnityAction(this.OnDeselect));
		this.button.onEnter.RemoveAllListeners();
		this.button.onEnter.AddListener(new UnityAction(this.OnSelect));
		this.TechTreeTab = techTreeTab;
		if (this.localizedLabel != null)
		{
			this.localizedLabel.langToken = langToken;
			this.localizedLabel.Localize();
		}
		this.button.onClick.RemoveAllListeners();
		this.button.onClick.AddListener(delegate
		{
			Action<TechTreeTabButton> onPressAction2 = onPressAction;
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

	// Token: 0x06003F4A RID: 16202 RVA: 0x0012F90C File Offset: 0x0012DB0C
	public void SetState(bool active)
	{
		Canvas canvas = LazyUI.GetWindow<CharacterWindow>().Canvas;
		this.frameImageInactive.sprite = this.inactiveFrame;
		this.activeGameObject.SetActive(active);
		this.inactiveGameObject.SetActive(!active);
		this.button.interactable = !active;
		this.canvas.sortingOrder = (active ? (canvas.sortingOrder + 3) : (canvas.sortingOrder + 1));
		this.inactiveOver.SetActive(false);
		this.iconInactive.material = (active ? null : this.inactiveIconMaterial);
	}

	// Token: 0x06003F4B RID: 16203 RVA: 0x0012F9A2 File Offset: 0x0012DBA2
	private void OnDisable()
	{
		if (this.button.interactable)
		{
			this.OnDeselect();
		}
	}

	// Token: 0x06003F4C RID: 16204 RVA: 0x0012F9B7 File Offset: 0x0012DBB7
	private void OnSelect()
	{
		this.iconInactive.material = null;
		this.frameImageInactive.sprite = this.inactiveFrameSelected;
		this.inactiveOver.SetActive(true);
	}

	// Token: 0x06003F4D RID: 16205 RVA: 0x0012F9E2 File Offset: 0x0012DBE2
	private void OnDeselect()
	{
		this.iconInactive.material = this.inactiveIconMaterial;
		this.frameImageInactive.sprite = this.inactiveFrame;
		this.inactiveOver.SetActive(false);
	}

	// Token: 0x040031D3 RID: 12755
	[SerializeField]
	private LazyButton button;

	// Token: 0x040031D4 RID: 12756
	[SerializeField]
	private GameObject activeGameObject;

	// Token: 0x040031D5 RID: 12757
	[SerializeField]
	private GameObject inactiveGameObject;

	// Token: 0x040031D6 RID: 12758
	[SerializeField]
	private Image iconActive;

	// Token: 0x040031D7 RID: 12759
	[SerializeField]
	private Image iconInactive;

	// Token: 0x040031D8 RID: 12760
	[SerializeField]
	private LocalizedLabel localizedLabel;

	// Token: 0x040031D9 RID: 12761
	[SerializeField]
	private Image frameImageInactive;

	// Token: 0x040031DA RID: 12762
	[SerializeField]
	private Sprite inactiveFrame;

	// Token: 0x040031DB RID: 12763
	[SerializeField]
	private Sprite inactiveFrameSelected;

	// Token: 0x040031DC RID: 12764
	[SerializeField]
	private GameObject inactiveOver;

	// Token: 0x040031DD RID: 12765
	[SerializeField]
	private Canvas canvas;

	// Token: 0x040031DE RID: 12766
	[SerializeField]
	private Material inactiveIconMaterial;
}
