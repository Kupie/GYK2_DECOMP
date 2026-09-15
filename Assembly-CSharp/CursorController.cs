using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000816 RID: 2070
public class CursorController : LazySingleton<CursorController>, ICursorChanger
{
	// Token: 0x06003516 RID: 13590 RVA: 0x000FFBEA File Offset: 0x000FDDEA
	protected override void Awake()
	{
		base.Awake();
		if (LazySingleton<CursorController>.Instance != this)
		{
			return;
		}
		this.Init();
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x000FFC06 File Offset: 0x000FDE06
	private void Start()
	{
		if (!this.isInitialized && LazySingleton<CursorController>.Instance == this)
		{
			this.Init();
		}
	}

	// Token: 0x06003518 RID: 13592 RVA: 0x000FFC23 File Offset: 0x000FDE23
	private void OnDestroy()
	{
		GameSettings.OnResolutionChanged -= this.OnResolutionChanged;
		this.ClearScaledCursorCache();
	}

	// Token: 0x06003519 RID: 13593 RVA: 0x000FFC3C File Offset: 0x000FDE3C
	private void OnResolutionChanged(IntVector2 _)
	{
		if (!this.isInitialized || GameSettings.Instance.cursorMode == GameCursorMode.Hardware)
		{
			return;
		}
		if (CursorController.GetSoftwareCursorScaleHundredths() == this.appliedSoftwareCursorScaleHundredths)
		{
			return;
		}
		this.UpdateState();
	}

	// Token: 0x0600351A RID: 13594 RVA: 0x000FFC67 File Offset: 0x000FDE67
	private void Update()
	{
		this.AutoHideCursor();
	}

	// Token: 0x0600351B RID: 13595 RVA: 0x000FFC70 File Offset: 0x000FDE70
	private void AutoHideCursor()
	{
		if (LazyInput.IsGamepadActive || MainGame.Instance == null)
		{
			return;
		}
		Vector2 vector = Input.mousePosition;
		float magnitude = (this.lastMousePos - vector).magnitude;
		this.lastMousePos = vector;
		bool flag = true;
		if (magnitude > 0.1f)
		{
			this.lastMoveTime = Time.time;
		}
		else if (Time.time - this.lastMoveTime > this.mouseAutohideTime)
		{
			flag = false;
		}
		if (MainGame.Instance.gameState != MainGame.GameState.InGame || LazyWindowsStackController.ActiveWindow != null)
		{
			flag = true;
			this.lastMoveTime = Time.time;
		}
		Cursor.visible = flag;
	}

	// Token: 0x0600351C RID: 13596 RVA: 0x000FFD11 File Offset: 0x000FDF11
	public void Init()
	{
		if (LazySingleton<CursorController>.Instance != this)
		{
			return;
		}
		if (this.isInitialized)
		{
			return;
		}
		CursorController.AddCursorState(CursorType.Default, this);
		this.isInitialized = true;
		GameSettings.OnResolutionChanged += this.OnResolutionChanged;
	}

	// Token: 0x0600351D RID: 13597 RVA: 0x000FFD49 File Offset: 0x000FDF49
	public static void AddCursorState(CursorType type, ICursorChanger changer)
	{
		LazySingleton<CursorController>.Instance.states.Add(new CursorState(type, changer));
		LazySingleton<CursorController>.Instance.UpdateState();
	}

	// Token: 0x0600351E RID: 13598 RVA: 0x000FFD6C File Offset: 0x000FDF6C
	public static void RemoveAllWithType(CursorType cursorType)
	{
		for (int i = LazySingleton<CursorController>.Instance.states.Count - 1; i >= 0; i--)
		{
			CursorState cursorState = LazySingleton<CursorController>.Instance.states[i];
			if (cursorState != null && cursorState.type == cursorType)
			{
				CursorController.RemoveCursorState(cursorState.changer);
			}
		}
	}

	// Token: 0x0600351F RID: 13599 RVA: 0x000FFDC0 File Offset: 0x000FDFC0
	public static void RemoveCursorState(ICursorChanger changer)
	{
		CursorState stateByChanger = CursorController.GetStateByChanger(changer);
		if (stateByChanger == null)
		{
			return;
		}
		LazySingleton<CursorController>.Instance.states.Remove(stateByChanger);
		LazySingleton<CursorController>.Instance.UpdateState();
	}

	// Token: 0x06003520 RID: 13600 RVA: 0x000FFDF3 File Offset: 0x000FDFF3
	public static void UpdateCursorState()
	{
		LazySingleton<CursorController>.Instance.UpdateState();
	}

	// Token: 0x06003521 RID: 13601 RVA: 0x000FFDFF File Offset: 0x000FDFFF
	public static void ChangeCursorVisibleState(bool visible)
	{
		Cursor.visible = visible;
		if (visible)
		{
			LazySingleton<CursorController>.Instance.ResetCursorState(false);
		}
	}

	// Token: 0x06003522 RID: 13602 RVA: 0x000FFE18 File Offset: 0x000FE018
	private void UpdateState()
	{
		if (this.states == null || this.states.Count == 0)
		{
			return;
		}
		List<CursorState> list = this.states;
		CursorConfiguration configByType = this.GetConfigByType(list[list.Count - 1].type);
		CursorMode cursorMode = ((GameSettings.Instance.cursorMode == GameCursorMode.Hardware) ? CursorMode.Auto : CursorMode.ForceSoftware);
		Texture2D texture2D = configByType.sprite;
		Vector2 vector = configByType.hotSpot;
		if (cursorMode == CursorMode.ForceSoftware)
		{
			int softwareCursorScaleHundredths = CursorController.GetSoftwareCursorScaleHundredths();
			this.appliedSoftwareCursorScaleHundredths = softwareCursorScaleHundredths;
			float num = (float)softwareCursorScaleHundredths / 100f;
			texture2D = this.GetScaledCursor(texture2D, softwareCursorScaleHundredths, num);
			vector *= num;
		}
		Cursor.SetCursor(texture2D, vector, cursorMode);
	}

	// Token: 0x06003523 RID: 13603 RVA: 0x000FFEAE File Offset: 0x000FE0AE
	private static int GetSoftwareCursorScaleHundredths()
	{
		if (GameSettings.Instance.cursorMode != GameCursorMode.Software150)
		{
			return 100;
		}
		return 150;
	}

	// Token: 0x06003524 RID: 13604 RVA: 0x000FFEC8 File Offset: 0x000FE0C8
	private Texture2D GetScaledCursor(Texture2D source, int scaleHundredths, float scale)
	{
		if (source == null || scaleHundredths <= 100)
		{
			return source;
		}
		ValueTuple<Texture2D, int> valueTuple = new ValueTuple<Texture2D, int>(source, scaleHundredths);
		Texture2D texture2D;
		if (this.scaledCursorCache.TryGetValue(valueTuple, out texture2D) && texture2D != null)
		{
			return texture2D;
		}
		int num = Mathf.RoundToInt((float)source.width * scale);
		int num2 = Mathf.RoundToInt((float)source.height * scale);
		Texture2D texture2D2 = new Texture2D(num, num2, TextureFormat.RGBA32, false)
		{
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Clamp,
			hideFlags = HideFlags.HideAndDontSave,
			name = string.Format("{0}_x{1}", source.name, scaleHundredths)
		};
		Color32[] pixels = source.GetPixels32();
		Color32[] array = new Color32[num * num2];
		int width = source.width;
		int height = source.height;
		for (int i = 0; i < num2; i++)
		{
			int num3 = Mathf.Min(height - 1, Mathf.FloorToInt((float)i / scale)) * width;
			int num4 = i * num;
			for (int j = 0; j < num; j++)
			{
				array[num4 + j] = pixels[num3 + Mathf.Min(width - 1, Mathf.FloorToInt((float)j / scale))];
			}
		}
		texture2D2.SetPixels32(array);
		texture2D2.Apply(false, false);
		this.scaledCursorCache[valueTuple] = texture2D2;
		return texture2D2;
	}

	// Token: 0x06003525 RID: 13605 RVA: 0x00100010 File Offset: 0x000FE210
	private void ClearScaledCursorCache()
	{
		foreach (Texture2D texture2D in this.scaledCursorCache.Values)
		{
			if (texture2D != null)
			{
				global::UnityEngine.Object.Destroy(texture2D);
			}
		}
		this.scaledCursorCache.Clear();
	}

	// Token: 0x06003526 RID: 13606 RVA: 0x0010007C File Offset: 0x000FE27C
	private void ResetCursorState(bool ignoreInvisible)
	{
		if (this.states.Count > 1)
		{
			CursorState cursorState = null;
			if (!ignoreInvisible)
			{
				cursorState = this.states.Find((CursorState s) => s.type == CursorType.Invisible);
			}
			LazySingleton<CursorController>.Instance.states.Clear();
			CursorController.AddCursorState(CursorType.Default, this);
			if (!ignoreInvisible && cursorState != null)
			{
				CursorController.AddCursorState(CursorType.Invisible, cursorState.changer);
			}
		}
	}

	// Token: 0x06003527 RID: 13607 RVA: 0x001000F0 File Offset: 0x000FE2F0
	private static CursorState GetStateByChanger(ICursorChanger changer)
	{
		try
		{
			for (int i = 0; i < LazySingleton<CursorController>.Instance.states.Count; i++)
			{
				CursorState cursorState = LazySingleton<CursorController>.Instance.states[i];
				if (cursorState != null && cursorState.changer == changer)
				{
					return cursorState;
				}
			}
		}
		catch (Exception)
		{
			return null;
		}
		return null;
	}

	// Token: 0x06003528 RID: 13608 RVA: 0x00100154 File Offset: 0x000FE354
	private CursorConfiguration GetConfigByType(CursorType type)
	{
		return this.cursorConfigurations.Find((CursorConfiguration c) => c.type == type);
	}

	// Token: 0x06003529 RID: 13609 RVA: 0x00100185 File Offset: 0x000FE385
	public static bool TryGetCursorConfiguration(CursorType type, out CursorConfiguration configuration)
	{
		configuration = null;
		if (LazySingleton<CursorController>.Instance == null)
		{
			return false;
		}
		configuration = LazySingleton<CursorController>.Instance.GetConfigByType(type);
		return configuration != null && configuration.sprite != null;
	}

	// Token: 0x0600352A RID: 13610 RVA: 0x001001B9 File Offset: 0x000FE3B9
	public static float GetSoftwareCursorScale()
	{
		if (GameSettings.Instance == null)
		{
			return 1f;
		}
		if (GameSettings.Instance.cursorMode != GameCursorMode.Software150)
		{
			return 1f;
		}
		return 1.5f;
	}

	// Token: 0x04002A90 RID: 10896
	private const bool IS_CONSOLE_BUILD = false;

	// Token: 0x04002A91 RID: 10897
	[SerializeField]
	private List<CursorConfiguration> cursorConfigurations = new List<CursorConfiguration>();

	// Token: 0x04002A92 RID: 10898
	[SerializeField]
	[ReadOnly]
	private List<CursorState> states = new List<CursorState>();

	// Token: 0x04002A93 RID: 10899
	private bool isInitialized;

	// Token: 0x04002A94 RID: 10900
	[SerializeField]
	private float mouseAutohideTime = 4f;

	// Token: 0x04002A95 RID: 10901
	private Vector2 lastMousePos = Vector2.zero;

	// Token: 0x04002A96 RID: 10902
	private float lastMoveTime;

	// Token: 0x04002A97 RID: 10903
	[TupleElementNames(new string[] { "source", "scaleHundredths" })]
	private readonly Dictionary<ValueTuple<Texture2D, int>, Texture2D> scaledCursorCache = new Dictionary<ValueTuple<Texture2D, int>, Texture2D>();

	// Token: 0x04002A98 RID: 10904
	private int appliedSoftwareCursorScaleHundredths;
}
