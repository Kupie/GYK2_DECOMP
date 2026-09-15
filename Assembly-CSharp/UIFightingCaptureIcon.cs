using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009C2 RID: 2498
public class UIFightingCaptureIcon : MonoBehaviour, IPoolable
{
	// Token: 0x17000A11 RID: 2577
	// (get) Token: 0x06004280 RID: 17024 RVA: 0x0013B994 File Offset: 0x00139B94
	public FightingCapturePoint CapturePoint
	{
		get
		{
			return this.capturePoint;
		}
	}

	// Token: 0x06004281 RID: 17025 RVA: 0x0013B99C File Offset: 0x00139B9C
	public void Draw(FightingCapturePoint capturePoint)
	{
		if (this.isSubscribed)
		{
			this.Unsubscribe();
		}
		this.capturePoint = capturePoint;
		this.Subscribe();
		this.DrawCurrentPoint();
	}

	// Token: 0x06004282 RID: 17026 RVA: 0x0013B9C0 File Offset: 0x00139BC0
	private void DrawCurrentPoint()
	{
		if (this.capturePoint.OwnedByTeam == LazyConsts.Fighting.TeamType.Player)
		{
			if (this.capturePoint.isBasePoint)
			{
				this.image.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("icon-flag_main-blue", null);
			}
			else
			{
				this.image.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("icon-flag_secondary-blue", null);
			}
		}
		else if (this.capturePoint.isBasePoint)
		{
			this.image.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("icon-flag_main-red", null);
		}
		else
		{
			this.image.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("icon-flag_secondary-red", null);
		}
		this.image.SetNativeSize();
	}

	// Token: 0x06004283 RID: 17027 RVA: 0x0013BA71 File Offset: 0x00139C71
	private void HandleStateChanged(FightingCapturePoint capturePoint)
	{
		this.DrawCurrentPoint();
	}

	// Token: 0x06004284 RID: 17028 RVA: 0x0013BA79 File Offset: 0x00139C79
	private void Subscribe()
	{
		if (this.isSubscribed)
		{
			return;
		}
		if (this.capturePoint != null)
		{
			this.capturePoint.OnCapturedByTeam += this.HandleStateChanged;
			this.isSubscribed = true;
		}
	}

	// Token: 0x06004285 RID: 17029 RVA: 0x0013BAB0 File Offset: 0x00139CB0
	private void Unsubscribe()
	{
		if (this.isSubscribed && this.capturePoint != null)
		{
			this.capturePoint.OnCapturedByTeam -= this.HandleStateChanged;
		}
		this.isSubscribed = false;
	}

	// Token: 0x06004286 RID: 17030 RVA: 0x0013BAE6 File Offset: 0x00139CE6
	public void OnPoolableObjReleased()
	{
		this.Unsubscribe();
	}

	// Token: 0x040033D7 RID: 13271
	[SerializeField]
	private Image image;

	// Token: 0x040033D8 RID: 13272
	private bool isSubscribed;

	// Token: 0x040033D9 RID: 13273
	private FightingCapturePoint capturePoint;
}
