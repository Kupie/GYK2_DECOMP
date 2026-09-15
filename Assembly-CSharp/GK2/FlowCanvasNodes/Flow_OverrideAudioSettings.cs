using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BE2 RID: 3042
	[Name("Override Audio Settings", 0)]
	[Category("Game/Audio")]
	[Description("Overrides Music and SFX volumes in GameSettings. Can restore them back to original values.")]
	[Color("f5da42")]
	public class Flow_OverrideAudioSettings : GKCustomFlowNode
	{
		// Token: 0x06004EAD RID: 20141 RVA: 0x00172A64 File Offset: 0x00170C64
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Execute), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (!this.restore)
			{
				this.musicVolume = base.AddValueInput<float>("musicVolume".CapitalizeFirst(), "");
				this.sfxVolume = base.AddValueInput<float>("sfxVolume".CapitalizeFirst(), "");
			}
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x00172AF4 File Offset: 0x00170CF4
		private void Execute(Flow flow)
		{
			GameSettings instance = GameSettings.Instance;
			if (this.restore)
			{
				if (this.originalMusicVolume != null)
				{
					instance.musicVolume = this.originalMusicVolume.Value;
					instance.sfxVolume = this.originalSfxVolume.Value;
					instance.ApplyAudioSettings();
					this.originalMusicVolume = null;
					this.originalSfxVolume = null;
				}
			}
			else
			{
				if (this.originalMusicVolume == null)
				{
					this.originalMusicVolume = new float?(instance.musicVolume);
					this.originalSfxVolume = new float?(instance.sfxVolume);
				}
				instance.musicVolume = this.musicVolume.value;
				instance.sfxVolume = this.sfxVolume.value;
				instance.ApplyAudioSettings();
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06004EAF RID: 20143 RVA: 0x00172BC4 File Offset: 0x00170DC4
		public override string name
		{
			get
			{
				if (!this.restore)
				{
					return "Override Audio Settings";
				}
				return "Restore Audio Settings";
			}
		}

		// Token: 0x04003FD4 RID: 16340
		[FlowNode.GatherPortsCallbackAttribute]
		public bool restore;

		// Token: 0x04003FD5 RID: 16341
		private FlowInput @in;

		// Token: 0x04003FD6 RID: 16342
		private FlowOutput @out;

		// Token: 0x04003FD7 RID: 16343
		private ValueInput<float> musicVolume;

		// Token: 0x04003FD8 RID: 16344
		private ValueInput<float> sfxVolume;

		// Token: 0x04003FD9 RID: 16345
		private float? originalMusicVolume;

		// Token: 0x04003FDA RID: 16346
		private float? originalSfxVolume;
	}
}
