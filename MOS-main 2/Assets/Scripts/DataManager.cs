using System.IO;
using UnityEngine;
using System;
public static class DataManager
{
	private static string basePath = Application.dataPath;
	public static FullTrialRun TrialRun = new FullTrialRun();
	public static Trial RecordingTrial;

	public static void ResetTrialRun()
	{
		TrialRun = new FullTrialRun();		
	}

	public static void StartRecordingTrial()
	{
		RecordingTrial = new Trial();
	}

    public static void AddTrialToCurrentRun(Trial trial)
	{
		trial.TrialID = TrialRun.Trials.Count + 1;
		TrialRun.Trials.Add(trial);
		SaveDataWithOverwrite();
	}

	public static void SaveDataWithOverwrite()
	{
		TrialRun.ProlificID = PlayerPrefs.GetString("ProlificID");
		string runData = JsonUtility.ToJson(TrialRun);
		string fileName = $"/Run {TrialRun.GUID}.json";
		string fullPath = basePath + fileName;
		ConfirmFileExists(fullPath);
		File.WriteAllLines(fullPath, new string[] { runData });
	}

	private static void ConfirmFileExists(string fullPath)
	{
		if (!File.Exists(fullPath))
		{
			FileStream file = File.Create(fullPath);
			file.Close();
		}
	}
}
