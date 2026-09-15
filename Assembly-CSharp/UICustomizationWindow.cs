using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020009A0 RID: 2464
public class UICustomizationWindow : LazyWindow<UICustomizationWindowData>
{
	// Token: 0x140000C1 RID: 193
	// (add) Token: 0x060041DC RID: 16860 RVA: 0x00139A2C File Offset: 0x00137C2C
	// (remove) Token: 0x060041DD RID: 16861 RVA: 0x00139A64 File Offset: 0x00137C64
	public event Action<PlayerCustomizationData> OnCustomizationApplied;

	// Token: 0x140000C2 RID: 194
	// (add) Token: 0x060041DE RID: 16862 RVA: 0x00139A9C File Offset: 0x00137C9C
	// (remove) Token: 0x060041DF RID: 16863 RVA: 0x00139AD4 File Offset: 0x00137CD4
	public event Action OnCustomizationCanceled;

	// Token: 0x060041E0 RID: 16864 RVA: 0x00139B0C File Offset: 0x00137D0C
	public override void Init()
	{
		base.Init();
		this.newCustomizationData = new PlayerCustomizationData();
		this.allParts = Addressables.LoadAssetsAsync<CustomizablePart>("player_skins", null).WaitForCompletion().ToList<CustomizablePart>();
	}

	// Token: 0x060041E1 RID: 16865 RVA: 0x00139B48 File Offset: 0x00137D48
	public override void Open(UICustomizationWindowData data)
	{
		base.Open(data);
		MainGame.PlayerController.View.CustomizationCharacter.gameObject.SetActive(true);
		this.newCustomizationData = PlayerCustomizationData.Copy(data.CurrentData);
		PlayerSkinHelper.ApplySkin(this.newCustomizationData, true);
		PlayerSkinHelper.ApplyPlayerColorsByData(this.newCustomizationData, true);
		this.playerCamera.gameObject.SetActive(true);
		this.InitPartSwitchButton(this.hairSwitcher, new List<CustomizablePartType> { CustomizablePartType.Hair });
		this.InitPartSwitchButton(this.beardSwitcher, new List<CustomizablePartType> { CustomizablePartType.Beard });
		this.InitPartSwitchButton(this.bodySwitcher, new List<CustomizablePartType>
		{
			CustomizablePartType.Body,
			CustomizablePartType.Arms
		});
		this.UpdateColorSwitchers();
		PlayerSkinHelper.ApplySkin(this.newCustomizationData, true);
		PlayerSkinHelper.ApplyPlayerColorsByData(this.newCustomizationData, true);
		this.btnData = new UIDialogWindowData.ButtonData(new Action(this.OnApplyPressed), LLBase.L("ui_apply"), null, true, GameKey.Select, "");
		this.applyButton.Draw(this.btnData);
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x060041E2 RID: 16866 RVA: 0x00139C70 File Offset: 0x00137E70
	public override void Close()
	{
		base.Close();
		MainGame.PlayerController.View.CustomizationCharacter.gameObject.SetActive(false);
		base.Hide();
		this.playerCamera.gameObject.SetActive(false);
	}

	// Token: 0x060041E3 RID: 16867 RVA: 0x00139CA9 File Offset: 0x00137EA9
	private void UpdateColorSwitchers()
	{
		this.InitColorSwitchButton(this.hedColorSwitch, PlayerColorCustomizationType.Hed);
		this.InitColorSwitchButton(this.bdy1ColorSwitch, PlayerColorCustomizationType.Bdy1);
		this.InitColorSwitchButton(this.bdy2ColorSwitch, PlayerColorCustomizationType.Bdy2);
		this.InitColorSwitchButton(this.bdy3ColorSwitch, PlayerColorCustomizationType.Bdy3);
	}

	// Token: 0x060041E4 RID: 16868 RVA: 0x00139CE0 File Offset: 0x00137EE0
	private int GetIndexOfPart(List<CustomizablePart> list, CustomizablePartType type)
	{
		string partId = this.newCustomizationData.GetCustomizationPartId(type);
		int num = list.FindIndex((CustomizablePart e) => e.name == partId);
		if (num != -1)
		{
			return num;
		}
		return 0;
	}

	// Token: 0x060041E5 RID: 16869 RVA: 0x00139D1F File Offset: 0x00137F1F
	private void SetCustomizationPart(PlayerCustomizationPartData newPartData)
	{
		this.SetCustomizationParts(new PlayerCustomizationPartData[] { newPartData });
	}

	// Token: 0x060041E6 RID: 16870 RVA: 0x00139D34 File Offset: 0x00137F34
	private void SetCustomizationParts(IEnumerable<PlayerCustomizationPartData> newPartsData)
	{
		using (IEnumerator<PlayerCustomizationPartData> enumerator = newPartsData.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PlayerCustomizationPartData newPartData = enumerator.Current;
				this.newCustomizationData.customizationPartsData.RemoveAll((PlayerCustomizationPartData x) => x.type == newPartData.type);
				this.newCustomizationData.customizationPartsData.Add(newPartData);
				Debug.Log(string.Format("#customize# newPartData.id :[{0}] newPartData.type:[{1}]", newPartData.id, newPartData.type));
			}
		}
		PlayerSkinHelper.ApplySkin(this.newCustomizationData, true);
		this.UpdateColorSwitchers();
		PlayerSkinHelper.ApplyPlayerColorsByData(this.newCustomizationData, true);
	}

	// Token: 0x060041E7 RID: 16871 RVA: 0x00139DFC File Offset: 0x00137FFC
	public void SetColorPaletteByIndex(int index, PlayerColorCustomizationType type)
	{
		PlayerColorCustomizationElementData playerColorCustomizationElementData = PlayerSkinHelper.CharacterCustomizationData.customizationElements.Find((PlayerColorCustomizationElementData e) => e.playerColorCustomizationType == type);
		this.newCustomizationData.SetColorCustomizationIndexForType(playerColorCustomizationElementData.playerColorCustomizationType, index);
		Debug.Log(string.Format("#customize# apply index:[{0}] for color type:[{1}]", index, type));
		PlayerSkinHelper.ApplyPlayerColors(PlayerSkinHelper.GetColorReplacementPalette(this.newCustomizationData), PlayerSkinHelper.CharacterCustomizationData.affectedPartTypes, true);
	}

	// Token: 0x060041E8 RID: 16872 RVA: 0x00139E80 File Offset: 0x00138080
	private Texture2D GetTextureForCustomizationPart(int index, PlayerColorCustomizationType type)
	{
		PlayerColorCustomizationElementData playerColorCustomizationElementData = PlayerSkinHelper.CharacterCustomizationData.customizationElements.Find((PlayerColorCustomizationElementData e) => e.playerColorCustomizationType == type);
		int partSkinIdForColorCustomizationType = this.newCustomizationData.GetPartSkinIdForColorCustomizationType(type);
		return playerColorCustomizationElementData.GetSkinElement(partSkinIdForColorCustomizationType).palettes[index];
	}

	// Token: 0x060041E9 RID: 16873 RVA: 0x00139ED8 File Offset: 0x001380D8
	private void OnApplyPressed()
	{
		this.data.CurrentData = this.newCustomizationData;
		Action<PlayerCustomizationData> onCustomizationApplied = this.OnCustomizationApplied;
		if (onCustomizationApplied != null)
		{
			onCustomizationApplied(this.newCustomizationData);
		}
		this.Close();
	}

	// Token: 0x060041EA RID: 16874 RVA: 0x00139F08 File Offset: 0x00138108
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, () => false);
		return gameKeyDelegates;
	}

	// Token: 0x060041EB RID: 16875 RVA: 0x00139F3A File Offset: 0x0013813A
	private void OnBackPressed()
	{
		Action onCustomizationCanceled = this.OnCustomizationCanceled;
		if (onCustomizationCanceled != null)
		{
			onCustomizationCanceled();
		}
		this.Close();
	}

	// Token: 0x060041EC RID: 16876 RVA: 0x00139F54 File Offset: 0x00138154
	private void InitColorSwitchButton(UICharacterOptionSwitcher switchButton, PlayerColorCustomizationType type)
	{
		PlayerColorCustomizationElementData playerColorCustomizationElementData = PlayerSkinHelper.CharacterCustomizationData.customizationElements.Find((PlayerColorCustomizationElementData e) => e.playerColorCustomizationType == type);
		int partSkinIdForColorCustomizationType = this.newCustomizationData.GetPartSkinIdForColorCustomizationType(type);
		PlayerColorCustomizationSkinElementData skinElementData = ((playerColorCustomizationElementData != null) ? playerColorCustomizationElementData.GetSkinElement(partSkinIdForColorCustomizationType) : null);
		List<int> unlockedIndices = new List<int>(this.newCustomizationData.GetUnlockedColorIndices(type, partSkinIdForColorCustomizationType, PlayerSkinHelper.CharacterCustomizationData));
		bool flag = skinElementData != null && skinElementData.palettes.Count != 0;
		if (flag)
		{
			unlockedIndices.RemoveAll((int index) => index < 0 || index >= skinElementData.palettes.Count);
		}
		switchButton.IsInteractable = flag && unlockedIndices.Count > 1;
		if (flag && unlockedIndices.Count > 0)
		{
			string[] array = new string[unlockedIndices.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = string.Format("{0}/{1}", i + 1, array.Length);
			}
			int colorCustomizationIndexByType = this.newCustomizationData.GetColorCustomizationIndexByType(type);
			int num = unlockedIndices.IndexOf(colorCustomizationIndexByType);
			if (num < 0)
			{
				num = 0;
				this.newCustomizationData.SetColorCustomizationIndexForType(type, unlockedIndices[num]);
			}
			switchButton.SetColorImage(this.GetTextureForCustomizationPart(unlockedIndices[num], type));
			switchButton.Initialize(delegate(int fieldIndex)
			{
				int num2 = unlockedIndices[fieldIndex];
				this.SetColorPaletteByIndex(num2, type);
				switchButton.SetColorImage(this.GetTextureForCustomizationPart(num2, type));
			}, array, num, false);
			return;
		}
		switchButton.SetSprite(this.nonInteractableSwitcherImage);
	}

	// Token: 0x060041ED RID: 16877 RVA: 0x0013A130 File Offset: 0x00138330
	private void InitPartSwitchButton(UICharacterOptionSwitcher switchButton, List<CustomizablePartType> types)
	{
		List<CustomizablePart> unlockedCustomizablePartsForType = this.GetUnlockedCustomizablePartsForType(types[0]);
		switchButton.IsInteractable = unlockedCustomizablePartsForType.Count > 1;
		if (unlockedCustomizablePartsForType.Count == 0)
		{
			switchButton.SetSprite(this.nonInteractableSwitcherImage);
			return;
		}
		List<string> list = new List<string>();
		for (int i = 0; i < unlockedCustomizablePartsForType.Count; i++)
		{
			list.Add(string.Format("{0}/{1}", i + 1, unlockedCustomizablePartsForType.Count));
		}
		switchButton.Initialize(delegate(int s)
		{
			CustomizablePartType customizablePartType = types[0];
			string name = this.GetUnlockedCustomizablePartsForType(customizablePartType)[s].name;
			List<PlayerCustomizationPartData> list2 = new List<PlayerCustomizationPartData>();
			foreach (CustomizablePartType customizablePartType2 in types)
			{
				list2.Add(new PlayerCustomizationPartData
				{
					id = UICustomizationWindow.GetPairedCustomizationPartId(name, customizablePartType, customizablePartType2),
					type = customizablePartType2
				});
			}
			this.SetCustomizationParts(list2);
		}, list.ToArray(), this.GetIndexOfPart(unlockedCustomizablePartsForType, types[0]), false);
	}

	// Token: 0x060041EE RID: 16878 RVA: 0x0013A1EE File Offset: 0x001383EE
	private static string GetPairedCustomizationPartId(string primaryId, CustomizablePartType primaryType, CustomizablePartType targetType)
	{
		if (primaryType == CustomizablePartType.Body && targetType == CustomizablePartType.Arms)
		{
			return primaryId.Replace("bdy_", "arm_");
		}
		return primaryId;
	}

	// Token: 0x060041EF RID: 16879 RVA: 0x0013A20C File Offset: 0x0013840C
	private List<CustomizablePart> GetUnlockedCustomizablePartsForType(CustomizablePartType type)
	{
		List<string> unlockedIds = this.newCustomizationData.GetUnlockedPartIds(type);
		return (from p in this.GetAllCustomizablePartsForType(type).FindAll((CustomizablePart p) => unlockedIds.Contains(p.name))
			orderby p.orderIndex
			select p).ToList<CustomizablePart>();
	}

	// Token: 0x060041F0 RID: 16880 RVA: 0x0013A274 File Offset: 0x00138474
	private List<CustomizablePart> GetAllCustomizablePartsForType(CustomizablePartType type)
	{
		List<CustomizablePart> list;
		if (!this.cachedPartsLists.TryGetValue(type, out list))
		{
			list = this.allParts.FindAll((CustomizablePart p) => p.type == type);
			this.cachedPartsLists.Add(type, list);
		}
		return list;
	}

	// Token: 0x060041F1 RID: 16881 RVA: 0x0013A2D0 File Offset: 0x001384D0
	protected override void PrintTips()
	{
		this.lazyButtonTips.Print(new LazyGameKeyTip[]
		{
			new LazyGameKeyTip(GameKey.PrevSubTab, "tip_prev", true, true, true),
			new LazyGameKeyTip(GameKey.NextSubTab, "tip_next", true, true, true),
			LazyGameKeyTip.Back(true, true, true)
		});
	}

	// Token: 0x060041F2 RID: 16882 RVA: 0x0013A324 File Offset: 0x00138524
	protected override void TestDraw()
	{
		this.Open(new UICustomizationWindowData
		{
			CurrentData = PlayerCustomizationData.Copy(PlayerSkinHelper.playerStandardCustomizationData)
		});
	}

	// Token: 0x04003374 RID: 13172
	[SerializeField]
	private UIDialogWindowButton applyButton;

	// Token: 0x04003375 RID: 13173
	[SerializeField]
	private Camera playerCamera;

	// Token: 0x04003376 RID: 13174
	[SerializeField]
	private UICharacterOptionSwitcher hairSwitcher;

	// Token: 0x04003377 RID: 13175
	[SerializeField]
	private UICharacterOptionSwitcher beardSwitcher;

	// Token: 0x04003378 RID: 13176
	[SerializeField]
	private UICharacterOptionSwitcher bodySwitcher;

	// Token: 0x04003379 RID: 13177
	[SerializeField]
	private UICharacterOptionSwitcher hedColorSwitch;

	// Token: 0x0400337A RID: 13178
	[SerializeField]
	private UICharacterOptionSwitcher bdy1ColorSwitch;

	// Token: 0x0400337B RID: 13179
	[SerializeField]
	private UICharacterOptionSwitcher bdy2ColorSwitch;

	// Token: 0x0400337C RID: 13180
	[SerializeField]
	private UICharacterOptionSwitcher bdy3ColorSwitch;

	// Token: 0x0400337D RID: 13181
	[SerializeField]
	private Sprite nonInteractableSwitcherImage;

	// Token: 0x0400337E RID: 13182
	public PlayerCustomizationData newCustomizationData;

	// Token: 0x0400337F RID: 13183
	private List<CustomizablePart> allParts;

	// Token: 0x04003380 RID: 13184
	private Dictionary<CustomizablePartType, List<CustomizablePart>> cachedPartsLists = new Dictionary<CustomizablePartType, List<CustomizablePart>>();

	// Token: 0x04003381 RID: 13185
	private UIDialogWindowData.ButtonData btnData;
}
