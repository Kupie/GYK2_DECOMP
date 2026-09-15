using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000DA RID: 218
	public class PlaySoundOnEnable : MonoBehaviour
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00014883 File Offset: 0x00012A83
		public string SoundId
		{
			get
			{
				return this.soundId;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0001488B File Offset: 0x00012A8B
		public SpatialType Spatial
		{
			get
			{
				return this.spatial;
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00014893 File Offset: 0x00012A93
		protected virtual void OnEnable()
		{
			this.soundHandler = LazyAudio.PlayAtGameObject(this.soundId, base.transform, this.spatial, true);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x000148B3 File Offset: 0x00012AB3
		protected virtual void OnDisable()
		{
			if (this.stopOnDisable)
			{
				SoundHandler soundHandler = this.soundHandler;
				if (soundHandler == null)
				{
					return;
				}
				soundHandler.Stop();
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000148CE File Offset: 0x00012ACE
		protected virtual void OnDestroy()
		{
			if (this.stopOnDestroy)
			{
				SoundHandler soundHandler = this.soundHandler;
				if (soundHandler == null)
				{
					return;
				}
				soundHandler.Stop();
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000148E9 File Offset: 0x00012AE9
		private void OnDidApplyAnimationProperties()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x000148F0 File Offset: 0x00012AF0
		public void Play()
		{
			LazyAudio.Play(this.soundId);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x000148FE File Offset: 0x00012AFE
		public void PlayAtGameObject()
		{
			LazyAudio.PlayAtGameObject(this.soundId, base.transform, this.spatial, true);
		}

		// Token: 0x0400019E RID: 414
		[SerializeField]
		public string soundId;

		// Token: 0x0400019F RID: 415
		[SerializeField]
		private bool stopOnDisable = true;

		// Token: 0x040001A0 RID: 416
		[SerializeField]
		private bool stopOnDestroy;

		// Token: 0x040001A1 RID: 417
		[SerializeField]
		private SpatialType spatial = SpatialType.sound1D;

		// Token: 0x040001A2 RID: 418
		protected SoundHandler soundHandler;
	}
}
