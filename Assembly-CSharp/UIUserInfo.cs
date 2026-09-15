using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009F9 RID: 2553
public class UIUserInfo : MonoBehaviour
{
	// Token: 0x060044D5 RID: 17621 RVA: 0x00146340 File Offset: 0x00144540
	public void Init(string id, string ip)
	{
		this.userId.text = "User Id: " + id;
		this.userIp.text = ip;
	}

	// Token: 0x060044D6 RID: 17622 RVA: 0x00146364 File Offset: 0x00144564
	public void Activate()
	{
		this.bgImage.color = Color.white;
	}

	// Token: 0x060044D7 RID: 17623 RVA: 0x00146376 File Offset: 0x00144576
	public void Deactivate()
	{
		this.bgImage.color = Color.grey;
	}

	// Token: 0x040035B9 RID: 13753
	[SerializeField]
	private TextMeshProUGUI userId;

	// Token: 0x040035BA RID: 13754
	[SerializeField]
	private TextMeshProUGUI userIp;

	// Token: 0x040035BB RID: 13755
	[SerializeField]
	private Image bgImage;
}
