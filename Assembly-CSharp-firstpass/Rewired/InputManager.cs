using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rewired
{
	// Token: 0x0200001F RID: 31
	[AddComponentMenu("Rewired/Input Manager")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class InputManager : InputManager_Base
	{
		// Token: 0x06000255 RID: 597 RVA: 0x000033EE File Offset: 0x000015EE
		protected override void OnInitialized()
		{
			this.SubscribeEvents();
		}

		// Token: 0x06000256 RID: 598 RVA: 0x000033F6 File Offset: 0x000015F6
		protected override void OnDeinitialized()
		{
			this.UnsubscribeEvents();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00003400 File Offset: 0x00001600
		protected override void DetectPlatform()
		{
			this.scriptingBackend = ScriptingBackend.Mono;
			this.scriptingAPILevel = ScriptingAPILevel.Net20;
			this.editorPlatform = EditorPlatform.None;
			this.platform = Platform.Unknown;
			this.webplayerPlatform = WebplayerPlatform.None;
			this.isEditor = false;
			if (SystemInfo.deviceName == null)
			{
				string empty = string.Empty;
			}
			if (SystemInfo.deviceModel == null)
			{
				string empty2 = string.Empty;
			}
			this.platform = Platform.Windows;
			this.scriptingBackend = ScriptingBackend.Mono;
			this.scriptingAPILevel = ScriptingAPILevel.Net46;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00003466 File Offset: 0x00001666
		protected override void CheckRecompile()
		{
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00003468 File Offset: 0x00001668
		protected override IExternalTools GetExternalTools()
		{
			return new ExternalTools();
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000346F File Offset: 0x0000166F
		private bool CheckDeviceName(string searchPattern, string deviceName, string deviceModel)
		{
			return Regex.IsMatch(deviceName, searchPattern, RegexOptions.IgnoreCase) || Regex.IsMatch(deviceModel, searchPattern, RegexOptions.IgnoreCase);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00003485 File Offset: 0x00001685
		private void SubscribeEvents()
		{
			this.UnsubscribeEvents();
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000349E File Offset: 0x0000169E
		private void UnsubscribeEvents()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000034B1 File Offset: 0x000016B1
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			base.OnSceneLoaded();
		}

		// Token: 0x040001DB RID: 475
		private bool ignoreRecompile;
	}
}
