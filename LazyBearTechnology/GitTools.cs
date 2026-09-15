using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200001D RID: 29
public static class GitTools
{
	// Token: 0x0600008B RID: 139 RVA: 0x00004320 File Offset: 0x00002520
	public static string RunGitCommand(string gitCommand)
	{
		ProcessStartInfo processStartInfo = new ProcessStartInfo("git", gitCommand)
		{
			CreateNoWindow = true,
			UseShellExecute = false,
			RedirectStandardOutput = true,
			RedirectStandardError = true
		};
		Process process = new Process
		{
			StartInfo = processStartInfo
		};
		try
		{
			process.Start();
		}
		catch (Exception ex)
		{
			global::UnityEngine.Debug.LogError("Git is not set-up correctly, required to be on PATH, and to be a git project.");
			throw ex;
		}
		string text = process.StandardOutput.ReadToEnd();
		string text2 = process.StandardError.ReadToEnd();
		process.WaitForExit();
		process.Close();
		if (text.Contains("fatal") || text == "no-git" || text == "")
		{
			throw new Exception(string.Concat(new string[] { "Command: git ", gitCommand, " Failed\n", text, text2 }));
		}
		if (text2 != "")
		{
			global::UnityEngine.Debug.LogError("Git Error: " + text2);
		}
		return text;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00004428 File Offset: 0x00002628
	public static string GetCurrentCommitShorthash()
	{
		string text = GitTools.RunGitCommand("rev-parse --short --verify HEAD");
		return string.Join("", text.Split(null, StringSplitOptions.RemoveEmptyEntries));
	}
}
