using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SoftMasking.Samples
{
	// Token: 0x0200000F RID: 15
	public class SoftMaskSampleChooser : MonoBehaviour
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00002C3C File Offset: 0x00000E3C
		public void Start()
		{
			string activeSceneName = SceneManager.GetActiveScene().name;
			int num = this.dropdown.options.FindIndex((Dropdown.OptionData x) => x.text == activeSceneName);
			if (num >= 0)
			{
				this.dropdown.value = num;
				this.dropdown.onValueChanged.AddListener(new UnityAction<int>(this.Choose));
				return;
			}
			this.Fallback(activeSceneName);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002CB8 File Offset: 0x00000EB8
		private void Fallback(string activeSceneName)
		{
			this.dropdown.gameObject.SetActive(false);
			this.fallbackLabel.gameObject.SetActive(true);
			this.fallbackLabel.text = activeSceneName;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002CE8 File Offset: 0x00000EE8
		public void Choose(int sampleIndex)
		{
			SceneManager.LoadScene(this.dropdown.options[sampleIndex].text);
		}

		// Token: 0x0400003F RID: 63
		public Dropdown dropdown;

		// Token: 0x04000040 RID: 64
		public Text fallbackLabel;
	}
}
