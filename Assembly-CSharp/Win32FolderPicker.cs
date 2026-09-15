using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020007B4 RID: 1972
internal static class Win32FolderPicker
{
	// Token: 0x060032CB RID: 13003 RVA: 0x000F57D8 File Offset: 0x000F39D8
	public static string PickFolder(string title, string startDirectory)
	{
		Guid guid = new Guid("DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7");
		Guid guid2 = new Guid("D57C7288-D4AD-4768-BE02-9D969532D960");
		IntPtr intPtr;
		int num = Win32FolderPicker.CoCreateInstance(ref guid, IntPtr.Zero, 1U, ref guid2, out intPtr);
		if (num != 0 || intPtr == IntPtr.Zero)
		{
			Debug.LogWarning(string.Format("[SteamWorkshopCreator] Failed to create folder dialog ({0:X8}).", num));
			return "";
		}
		string text;
		try
		{
			Win32FolderPicker.SetOptions(intPtr, 2152U);
			if (!string.IsNullOrEmpty(title))
			{
				Win32FolderPicker.SetTitle(intPtr, title);
			}
			if (!string.IsNullOrEmpty(startDirectory))
			{
				Win32FolderPicker.TrySetFolder(intPtr, startDirectory);
			}
			num = Win32FolderPicker.Show(intPtr, Win32FolderPicker.GetActiveWindow());
			IntPtr intPtr2;
			if (num == -2147023673 || num != 0)
			{
				text = "";
			}
			else if (Win32FolderPicker.GetResult(intPtr, out intPtr2) != 0 || intPtr2 == IntPtr.Zero)
			{
				text = "";
			}
			else
			{
				try
				{
					IntPtr intPtr3;
					if (Win32FolderPicker.GetDisplayName(intPtr2, -2147123200, out intPtr3) != 0 || intPtr3 == IntPtr.Zero)
					{
						text = "";
					}
					else
					{
						string text2 = Marshal.PtrToStringUni(intPtr3);
						Marshal.FreeCoTaskMem(intPtr3);
						text = text2 ?? "";
					}
				}
				finally
				{
					Marshal.Release(intPtr2);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("[SteamWorkshopCreator] Folder dialog failed: " + ex.Message);
			text = "";
		}
		finally
		{
			Marshal.Release(intPtr);
		}
		return text;
	}

	// Token: 0x060032CC RID: 13004 RVA: 0x000F5948 File Offset: 0x000F3B48
	private static void TrySetFolder(IntPtr dialog, string startDirectory)
	{
		Guid guid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE");
		IntPtr intPtr;
		if (Win32FolderPicker.SHCreateItemFromParsingName(startDirectory, IntPtr.Zero, ref guid, out intPtr) != 0 || intPtr == IntPtr.Zero)
		{
			return;
		}
		Win32FolderPicker.SetFolder(dialog, intPtr);
		Marshal.Release(intPtr);
	}

	// Token: 0x060032CD RID: 13005 RVA: 0x000F598E File Offset: 0x000F3B8E
	private static IntPtr VTable(IntPtr com, int index)
	{
		return Marshal.ReadIntPtr(Marshal.ReadIntPtr(com), index * IntPtr.Size);
	}

	// Token: 0x060032CE RID: 13006 RVA: 0x000F59A2 File Offset: 0x000F3BA2
	private static int Show(IntPtr dialog, IntPtr hwnd)
	{
		return Marshal.GetDelegateForFunctionPointer<Win32FolderPicker.ShowDelegate>(Win32FolderPicker.VTable(dialog, 3))(dialog, hwnd);
	}

	// Token: 0x060032CF RID: 13007 RVA: 0x000F59B7 File Offset: 0x000F3BB7
	private static void SetOptions(IntPtr dialog, uint fos)
	{
		Marshal.GetDelegateForFunctionPointer<Win32FolderPicker.SetOptionsDelegate>(Win32FolderPicker.VTable(dialog, 9))(dialog, fos);
	}

	// Token: 0x060032D0 RID: 13008 RVA: 0x000F59CE File Offset: 0x000F3BCE
	private static void SetFolder(IntPtr dialog, IntPtr folder)
	{
		Marshal.GetDelegateForFunctionPointer<Win32FolderPicker.SetFolderDelegate>(Win32FolderPicker.VTable(dialog, 12))(dialog, folder);
	}

	// Token: 0x060032D1 RID: 13009 RVA: 0x000F59E5 File Offset: 0x000F3BE5
	private static void SetTitle(IntPtr dialog, string title)
	{
		Marshal.GetDelegateForFunctionPointer<Win32FolderPicker.SetTitleDelegate>(Win32FolderPicker.VTable(dialog, 17))(dialog, title);
	}

	// Token: 0x060032D2 RID: 13010 RVA: 0x000F59FC File Offset: 0x000F3BFC
	private static int GetResult(IntPtr dialog, out IntPtr item)
	{
		return Marshal.GetDelegateForFunctionPointer<Win32FolderPicker.GetResultDelegate>(Win32FolderPicker.VTable(dialog, 20))(dialog, out item);
	}

	// Token: 0x060032D3 RID: 13011 RVA: 0x000F5A12 File Offset: 0x000F3C12
	private static int GetDisplayName(IntPtr item, int sigdn, out IntPtr name)
	{
		return Marshal.GetDelegateForFunctionPointer<Win32FolderPicker.GetDisplayNameDelegate>(Win32FolderPicker.VTable(item, 5))(item, sigdn, out name);
	}

	// Token: 0x060032D4 RID: 13012
	[DllImport("ole32.dll")]
	private static extern int CoCreateInstance(ref Guid rclsid, IntPtr pUnkOuter, uint dwClsContext, ref Guid riid, out IntPtr ppv);

	// Token: 0x060032D5 RID: 13013
	[DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
	private static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IntPtr pbc, ref Guid riid, out IntPtr ppv);

	// Token: 0x060032D6 RID: 13014
	[DllImport("user32.dll")]
	private static extern IntPtr GetActiveWindow();

	// Token: 0x040028BB RID: 10427
	private const uint ClsctxInprocServer = 1U;

	// Token: 0x040028BC RID: 10428
	private const int SigdnFilesyspath = -2147123200;

	// Token: 0x040028BD RID: 10429
	private const uint FosNoChangeDir = 8U;

	// Token: 0x040028BE RID: 10430
	private const uint FosPickFolders = 32U;

	// Token: 0x040028BF RID: 10431
	private const uint FosForceFileSystem = 64U;

	// Token: 0x040028C0 RID: 10432
	private const uint FosPathMustExist = 2048U;

	// Token: 0x040028C1 RID: 10433
	private const int ErrorCancelled = -2147023673;

	// Token: 0x020007B5 RID: 1973
	// (Invoke) Token: 0x060032D8 RID: 13016
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate int ShowDelegate(IntPtr thisPtr, IntPtr hwnd);

	// Token: 0x020007B6 RID: 1974
	// (Invoke) Token: 0x060032DC RID: 13020
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate int SetOptionsDelegate(IntPtr thisPtr, uint fos);

	// Token: 0x020007B7 RID: 1975
	// (Invoke) Token: 0x060032E0 RID: 13024
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate int SetFolderDelegate(IntPtr thisPtr, IntPtr psi);

	// Token: 0x020007B8 RID: 1976
	// (Invoke) Token: 0x060032E4 RID: 13028
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate int SetTitleDelegate(IntPtr thisPtr, [MarshalAs(UnmanagedType.LPWStr)] string title);

	// Token: 0x020007B9 RID: 1977
	// (Invoke) Token: 0x060032E8 RID: 13032
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate int GetResultDelegate(IntPtr thisPtr, out IntPtr item);

	// Token: 0x020007BA RID: 1978
	// (Invoke) Token: 0x060032EC RID: 13036
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	private delegate int GetDisplayNameDelegate(IntPtr thisPtr, int sigdn, out IntPtr name);
}
