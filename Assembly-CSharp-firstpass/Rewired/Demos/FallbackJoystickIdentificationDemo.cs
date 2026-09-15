using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020000FE RID: 254
	[AddComponentMenu("")]
	public class FallbackJoystickIdentificationDemo : MonoBehaviour
	{
		// Token: 0x06000C8F RID: 3215 RVA: 0x00023F2C File Offset: 0x0002212C
		private void Awake()
		{
			if (!ReInput.unityJoystickIdentificationRequired)
			{
				return;
			}
			ReInput.ControllerConnectedEvent += this.JoystickConnected;
			ReInput.ControllerDisconnectedEvent += this.JoystickDisconnected;
			this.IdentifyAllJoysticks();
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00023F5E File Offset: 0x0002215E
		private void JoystickConnected(ControllerStatusChangedEventArgs args)
		{
			this.IdentifyAllJoysticks();
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00023F5E File Offset: 0x0002215E
		private void JoystickDisconnected(ControllerStatusChangedEventArgs args)
		{
			this.IdentifyAllJoysticks();
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00023F68 File Offset: 0x00022168
		public void IdentifyAllJoysticks()
		{
			this.Reset();
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			Joystick[] joysticks = ReInput.controllers.GetJoysticks();
			if (joysticks == null)
			{
				return;
			}
			this.identifyRequired = true;
			this.joysticksToIdentify = new Queue<Joystick>(joysticks);
			this.SetInputDelay();
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x00023FB0 File Offset: 0x000221B0
		private void SetInputDelay()
		{
			this.nextInputAllowedTime = Time.time + 1f;
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x00023FC4 File Offset: 0x000221C4
		private void OnGUI()
		{
			if (!this.identifyRequired)
			{
				return;
			}
			if (this.joysticksToIdentify == null || this.joysticksToIdentify.Count == 0)
			{
				this.Reset();
				return;
			}
			Rect rect = new Rect((float)Screen.width * 0.5f - 125f, (float)Screen.height * 0.5f - 125f, 250f, 250f);
			GUILayout.Window(0, rect, new GUI.WindowFunction(this.DrawDialogWindow), "Joystick Identification Required", Array.Empty<GUILayoutOption>());
			GUI.FocusWindow(0);
			if (Time.time < this.nextInputAllowedTime)
			{
				return;
			}
			if (!ReInput.controllers.SetUnityJoystickIdFromAnyButtonOrAxisPress(this.joysticksToIdentify.Peek().id, 0.8f, false))
			{
				return;
			}
			this.joysticksToIdentify.Dequeue();
			this.SetInputDelay();
			if (this.joysticksToIdentify.Count == 0)
			{
				this.Reset();
			}
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x000240A8 File Offset: 0x000222A8
		private void DrawDialogWindow(int windowId)
		{
			if (!this.identifyRequired)
			{
				return;
			}
			if (this.style == null)
			{
				this.style = new GUIStyle(GUI.skin.label);
				this.style.wordWrap = true;
			}
			GUILayout.Space(15f);
			GUILayout.Label("A joystick has been attached or removed. You will need to identify each joystick by pressing a button on the controller listed below:", this.style, Array.Empty<GUILayoutOption>());
			Joystick joystick = this.joysticksToIdentify.Peek();
			GUILayout.Label("Press any button on \"" + joystick.name + "\" now.", this.style, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Skip", Array.Empty<GUILayoutOption>()))
			{
				this.joysticksToIdentify.Dequeue();
				return;
			}
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0002415A File Offset: 0x0002235A
		private void Reset()
		{
			this.joysticksToIdentify = null;
			this.identifyRequired = false;
		}

		// Token: 0x04000665 RID: 1637
		private const float windowWidth = 250f;

		// Token: 0x04000666 RID: 1638
		private const float windowHeight = 250f;

		// Token: 0x04000667 RID: 1639
		private const float inputDelay = 1f;

		// Token: 0x04000668 RID: 1640
		private bool identifyRequired;

		// Token: 0x04000669 RID: 1641
		private Queue<Joystick> joysticksToIdentify;

		// Token: 0x0400066A RID: 1642
		private float nextInputAllowedTime;

		// Token: 0x0400066B RID: 1643
		private GUIStyle style;
	}
}
