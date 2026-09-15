using System;
using System.Collections.Generic;
using Rewired.ControllerExtensions;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020000FA RID: 250
	[AddComponentMenu("")]
	public class DualShock4SpecialFeaturesExample : MonoBehaviour
	{
		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x00023706 File Offset: 0x00021906
		private Player player
		{
			get
			{
				return ReInput.players.GetPlayer(this.playerId);
			}
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00023718 File Offset: 0x00021918
		private void Awake()
		{
			this.InitializeTouchObjects();
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00023720 File Offset: 0x00021920
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			IDualShock4Extension firstDS = this.GetFirstDS4(this.player);
			if (firstDS != null)
			{
				base.transform.rotation = firstDS.GetOrientation();
				this.HandleTouchpad(firstDS);
				Vector3 accelerometerValue = firstDS.GetAccelerometerValue();
				this.accelerometerTransform.LookAt(this.accelerometerTransform.position + accelerometerValue);
			}
			if (this.player.GetButtonDown("CycleLight"))
			{
				this.SetRandomLightColor();
			}
			if (this.player.GetButtonDown("ResetOrientation"))
			{
				this.ResetOrientation();
			}
			if (this.player.GetButtonDown("ToggleLightFlash"))
			{
				if (this.isFlashing)
				{
					this.StopLightFlash();
				}
				else
				{
					this.StartLightFlash();
				}
				this.isFlashing = !this.isFlashing;
			}
			if (this.player.GetButtonDown("VibrateLeft"))
			{
				firstDS.SetVibration(0, 1f, 1f);
			}
			if (this.player.GetButtonDown("VibrateRight"))
			{
				firstDS.SetVibration(1, 1f, 1f);
			}
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00023830 File Offset: 0x00021A30
		private void OnGUI()
		{
			if (this.textStyle == null)
			{
				this.textStyle = new GUIStyle(GUI.skin.label);
				this.textStyle.fontSize = 20;
				this.textStyle.wordWrap = true;
			}
			if (this.GetFirstDS4(this.player) == null)
			{
				return;
			}
			GUILayout.BeginArea(new Rect(200f, 100f, (float)Screen.width - 400f, (float)Screen.height - 200f));
			GUILayout.Label("Rotate the Dual Shock 4 to see the model rotate in sync.", this.textStyle, Array.Empty<GUILayoutOption>());
			GUILayout.Label("Touch the touchpad to see them appear on the model.", this.textStyle, Array.Empty<GUILayoutOption>());
			ActionElementMap actionElementMap = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "ResetOrientation", true);
			if (actionElementMap != null)
			{
				GUILayout.Label("Press " + actionElementMap.elementIdentifierName + " to reset the orientation. Hold the gamepad facing the screen with sticks pointing up and press the button.", this.textStyle, Array.Empty<GUILayoutOption>());
			}
			actionElementMap = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "CycleLight", true);
			if (actionElementMap != null)
			{
				GUILayout.Label("Press " + actionElementMap.elementIdentifierName + " to change the light color.", this.textStyle, Array.Empty<GUILayoutOption>());
			}
			actionElementMap = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "ToggleLightFlash", true);
			if (actionElementMap != null)
			{
				GUILayout.Label("Press " + actionElementMap.elementIdentifierName + " to start or stop the light flashing.", this.textStyle, Array.Empty<GUILayoutOption>());
			}
			actionElementMap = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "VibrateLeft", true);
			if (actionElementMap != null)
			{
				GUILayout.Label("Press " + actionElementMap.elementIdentifierName + " vibrate the left motor.", this.textStyle, Array.Empty<GUILayoutOption>());
			}
			actionElementMap = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "VibrateRight", true);
			if (actionElementMap != null)
			{
				GUILayout.Label("Press " + actionElementMap.elementIdentifierName + " vibrate the right motor.", this.textStyle, Array.Empty<GUILayoutOption>());
			}
			GUILayout.EndArea();
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00023A38 File Offset: 0x00021C38
		private void ResetOrientation()
		{
			IDualShock4Extension firstDS = this.GetFirstDS4(this.player);
			if (firstDS != null)
			{
				firstDS.ResetOrientation();
			}
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00023A5C File Offset: 0x00021C5C
		private void SetRandomLightColor()
		{
			IDualShock4Extension firstDS = this.GetFirstDS4(this.player);
			if (firstDS != null)
			{
				Color color = new Color(global::UnityEngine.Random.Range(0f, 1f), global::UnityEngine.Random.Range(0f, 1f), global::UnityEngine.Random.Range(0f, 1f), 1f);
				firstDS.SetLightColor(color);
				this.lightObject.GetComponent<MeshRenderer>().material.color = color;
			}
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00023AD0 File Offset: 0x00021CD0
		private void StartLightFlash()
		{
			DualShock4Extension dualShock4Extension = this.GetFirstDS4(this.player) as DualShock4Extension;
			if (dualShock4Extension != null)
			{
				dualShock4Extension.SetLightFlash(0.5f, 0.5f);
			}
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00023B04 File Offset: 0x00021D04
		private void StopLightFlash()
		{
			DualShock4Extension dualShock4Extension = this.GetFirstDS4(this.player) as DualShock4Extension;
			if (dualShock4Extension != null)
			{
				dualShock4Extension.StopLightFlash();
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00023B2C File Offset: 0x00021D2C
		private IDualShock4Extension GetFirstDS4(Player player)
		{
			foreach (Joystick joystick in player.controllers.Joysticks)
			{
				IDualShock4Extension extension = joystick.GetExtension<IDualShock4Extension>();
				if (extension != null)
				{
					return extension;
				}
			}
			return null;
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00023B88 File Offset: 0x00021D88
		private void InitializeTouchObjects()
		{
			this.touches = new List<DualShock4SpecialFeaturesExample.Touch>(2);
			this.unusedTouches = new Queue<DualShock4SpecialFeaturesExample.Touch>(2);
			for (int i = 0; i < 2; i++)
			{
				DualShock4SpecialFeaturesExample.Touch touch = new DualShock4SpecialFeaturesExample.Touch();
				touch.go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				touch.go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				touch.go.transform.SetParent(this.touchpadTransform, true);
				touch.go.GetComponent<MeshRenderer>().material.color = ((i == 0) ? Color.red : Color.green);
				touch.go.SetActive(false);
				this.unusedTouches.Enqueue(touch);
			}
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x00023C48 File Offset: 0x00021E48
		private void HandleTouchpad(IDualShock4Extension ds4)
		{
			for (int i = this.touches.Count - 1; i >= 0; i--)
			{
				DualShock4SpecialFeaturesExample.Touch touch = this.touches[i];
				if (!ds4.IsTouchingByTouchId(touch.touchId))
				{
					touch.go.SetActive(false);
					this.unusedTouches.Enqueue(touch);
					this.touches.RemoveAt(i);
				}
			}
			for (int j = 0; j < ds4.maxTouches; j++)
			{
				if (ds4.IsTouching(j))
				{
					int touchId = ds4.GetTouchId(j);
					DualShock4SpecialFeaturesExample.Touch touch2 = this.touches.Find((DualShock4SpecialFeaturesExample.Touch x) => x.touchId == touchId);
					if (touch2 == null)
					{
						touch2 = this.unusedTouches.Dequeue();
						this.touches.Add(touch2);
					}
					touch2.touchId = touchId;
					touch2.go.SetActive(true);
					Vector2 vector;
					ds4.GetTouchPosition(j, out vector);
					touch2.go.transform.localPosition = new Vector3(vector.x - 0.5f, 0.5f + touch2.go.transform.localScale.y * 0.5f, vector.y - 0.5f);
				}
			}
		}

		// Token: 0x04000650 RID: 1616
		private const int maxTouches = 2;

		// Token: 0x04000651 RID: 1617
		public int playerId;

		// Token: 0x04000652 RID: 1618
		public Transform touchpadTransform;

		// Token: 0x04000653 RID: 1619
		public GameObject lightObject;

		// Token: 0x04000654 RID: 1620
		public Transform accelerometerTransform;

		// Token: 0x04000655 RID: 1621
		private List<DualShock4SpecialFeaturesExample.Touch> touches;

		// Token: 0x04000656 RID: 1622
		private Queue<DualShock4SpecialFeaturesExample.Touch> unusedTouches;

		// Token: 0x04000657 RID: 1623
		private bool isFlashing;

		// Token: 0x04000658 RID: 1624
		private GUIStyle textStyle;

		// Token: 0x020000FB RID: 251
		private class Touch
		{
			// Token: 0x04000659 RID: 1625
			public GameObject go;

			// Token: 0x0400065A RID: 1626
			public int touchId = -1;
		}
	}
}
