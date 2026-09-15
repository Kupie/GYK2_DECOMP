using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020000FF RID: 255
	[AddComponentMenu("")]
	public class PlayerMouseSpriteExample : MonoBehaviour
	{
		// Token: 0x06000C98 RID: 3224 RVA: 0x0002416C File Offset: 0x0002236C
		private void Awake()
		{
			this.pointer = global::UnityEngine.Object.Instantiate<GameObject>(this.pointerPrefab);
			this.pointer.transform.localScale = new Vector3(this.spriteScale, this.spriteScale, this.spriteScale);
			if (this.hideHardwarePointer)
			{
				Cursor.visible = false;
			}
			this.mouse = PlayerMouse.Factory.Create();
			this.mouse.playerId = this.playerId;
			this.mouse.xAxis.actionName = this.horizontalAction;
			this.mouse.yAxis.actionName = this.verticalAction;
			this.mouse.wheel.yAxis.actionName = this.wheelAction;
			this.mouse.leftButton.actionName = this.leftButtonAction;
			this.mouse.rightButton.actionName = this.rightButtonAction;
			this.mouse.middleButton.actionName = this.middleButtonAction;
			this.mouse.pointerSpeed = 1f;
			this.mouse.wheel.yAxis.repeatRate = 5f;
			this.mouse.screenPosition = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
			this.mouse.ScreenPositionChangedEvent += this.OnScreenPositionChanged;
			this.OnScreenPositionChanged(this.mouse.screenPosition);
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x000242E0 File Offset: 0x000224E0
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.pointer.transform.Rotate(Vector3.forward, this.mouse.wheel.yAxis.value * 20f);
			if (this.mouse.leftButton.justPressed)
			{
				this.CreateClickEffect(new Color(0f, 1f, 0f, 1f));
			}
			if (this.mouse.rightButton.justPressed)
			{
				this.CreateClickEffect(new Color(1f, 0f, 0f, 1f));
			}
			if (this.mouse.middleButton.justPressed)
			{
				this.CreateClickEffect(new Color(1f, 1f, 0f, 1f));
			}
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x000243B8 File Offset: 0x000225B8
		private void OnDestroy()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.mouse.ScreenPositionChangedEvent -= this.OnScreenPositionChanged;
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x000243DC File Offset: 0x000225DC
		private void CreateClickEffect(Color color)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.clickEffectPrefab);
			gameObject.transform.localScale = new Vector3(this.spriteScale, this.spriteScale, this.spriteScale);
			gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(this.mouse.screenPosition.x, this.mouse.screenPosition.y, this.distanceFromCamera));
			gameObject.GetComponentInChildren<SpriteRenderer>().color = color;
			global::UnityEngine.Object.Destroy(gameObject, 0.5f);
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0002446C File Offset: 0x0002266C
		private void OnScreenPositionChanged(Vector2 position)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, this.distanceFromCamera));
			this.pointer.transform.position = vector;
		}

		// Token: 0x0400066C RID: 1644
		[Tooltip("The Player that will control the mouse")]
		public int playerId;

		// Token: 0x0400066D RID: 1645
		[Tooltip("The Rewired Action used for the mouse horizontal axis.")]
		public string horizontalAction = "MouseX";

		// Token: 0x0400066E RID: 1646
		[Tooltip("The Rewired Action used for the mouse vertical axis.")]
		public string verticalAction = "MouseY";

		// Token: 0x0400066F RID: 1647
		[Tooltip("The Rewired Action used for the mouse wheel axis.")]
		public string wheelAction = "MouseWheel";

		// Token: 0x04000670 RID: 1648
		[Tooltip("The Rewired Action used for the mouse left button.")]
		public string leftButtonAction = "MouseLeftButton";

		// Token: 0x04000671 RID: 1649
		[Tooltip("The Rewired Action used for the mouse right button.")]
		public string rightButtonAction = "MouseRightButton";

		// Token: 0x04000672 RID: 1650
		[Tooltip("The Rewired Action used for the mouse middle button.")]
		public string middleButtonAction = "MouseMiddleButton";

		// Token: 0x04000673 RID: 1651
		[Tooltip("The distance from the camera that the pointer will be drawn.")]
		public float distanceFromCamera = 1f;

		// Token: 0x04000674 RID: 1652
		[Tooltip("The scale of the sprite pointer.")]
		public float spriteScale = 0.05f;

		// Token: 0x04000675 RID: 1653
		[Tooltip("The pointer prefab.")]
		public GameObject pointerPrefab;

		// Token: 0x04000676 RID: 1654
		[Tooltip("The click effect prefab.")]
		public GameObject clickEffectPrefab;

		// Token: 0x04000677 RID: 1655
		[Tooltip("Should the hardware pointer be hidden?")]
		public bool hideHardwarePointer = true;

		// Token: 0x04000678 RID: 1656
		[NonSerialized]
		private GameObject pointer;

		// Token: 0x04000679 RID: 1657
		[NonSerialized]
		private PlayerMouse mouse;
	}
}
