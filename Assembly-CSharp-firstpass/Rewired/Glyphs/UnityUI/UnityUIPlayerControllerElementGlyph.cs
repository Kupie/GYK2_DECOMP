using System;
using UnityEngine;

namespace Rewired.Glyphs.UnityUI
{
	// Token: 0x02000073 RID: 115
	[AddComponentMenu("Rewired/Glyphs/Unity UI/Unity UI Player Controller Element Glyph")]
	public class UnityUIPlayerControllerElementGlyph : UnityUIPlayerControllerElementGlyphBase
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0000E678 File Offset: 0x0000C878
		// (set) Token: 0x060005F7 RID: 1527 RVA: 0x0000E680 File Offset: 0x0000C880
		public override int playerId
		{
			get
			{
				return this._playerId;
			}
			set
			{
				this._playerId = value;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0000E689 File Offset: 0x0000C889
		// (set) Token: 0x060005F9 RID: 1529 RVA: 0x0000E6A0 File Offset: 0x0000C8A0
		public override int actionId
		{
			get
			{
				if (!this._actionIdCached)
				{
					this.CacheActionId();
				}
				return this._actionId;
			}
			set
			{
				if (!ReInput.isReady)
				{
					return;
				}
				InputAction action = ReInput.mapping.GetAction(value);
				if (action == null)
				{
					Debug.LogError("Invalid Action id: " + value.ToString());
					return;
				}
				this._actionName = action.name;
				this.CacheActionId();
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x0000E6ED File Offset: 0x0000C8ED
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x0000E6F5 File Offset: 0x0000C8F5
		public string actionName
		{
			get
			{
				return this._actionName;
			}
			set
			{
				this._actionName = value;
				this.CacheActionId();
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0000E704 File Offset: 0x0000C904
		private void CacheActionId()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			InputAction action = ReInput.mapping.GetAction(this._actionName);
			this._actionId = ((action != null) ? action.id : (-1));
			this._actionIdCached = true;
		}

		// Token: 0x0400031D RID: 797
		[Tooltip("The Player id.")]
		[SerializeField]
		private int _playerId;

		// Token: 0x0400031E RID: 798
		[Tooltip("The Action name.")]
		[SerializeField]
		private string _actionName;

		// Token: 0x0400031F RID: 799
		[NonSerialized]
		private int _actionId = -1;

		// Token: 0x04000320 RID: 800
		[NonSerialized]
		private bool _actionIdCached;
	}
}
