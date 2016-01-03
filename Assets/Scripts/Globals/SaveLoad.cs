using UnityEngine;
using System.Collections;
using System.IO;

public class SaveLoad : MonoBehaviour {

	public string savePath = "/save";
	public bool ignoreSaveInEditor = false;

	public void SaveGame()
	{
		if (Application.isEditor && ignoreSaveInEditor)
		{
			return;
		}

		if (string.IsNullOrEmpty(Application.dataPath + savePath) || Globals.Instance == null)
		{
			return;
		}

		StreamWriter saveFile;
		if (!File.Exists(savePath))
		{
			saveFile = File.CreateText(Application.dataPath + savePath);
		}
		else
		{
			saveFile = new StreamWriter(Application.dataPath + savePath);
		}

		int saveData = 0;
		for (int i = 0; i < Globals.Instance.levelsCompleted.Length; i++)
		{
			if (Globals.Instance.levelsCompleted[i])
			{
				saveData += (int)Mathf.Pow(2, i);
			}
		}

		saveFile.WriteLine("" + saveData);
		saveFile.Close();
	}

	public void LoadGame()
	{
		if (Application.isEditor && ignoreSaveInEditor)
		{
			return;
		}

		if (string.IsNullOrEmpty(savePath) || !File.Exists(Application.dataPath + savePath) || Globals.Instance == null)
		{
			return;
		}

		StreamReader loadFile = File.OpenText(Application.dataPath + savePath);

		string progressLine = loadFile.ReadLine();
		int loadData = int.Parse(progressLine);
		int checkBit = 1;

		for (int i = 0; i < Globals.Instance.levelsCompleted.Length; i++)
		{
			Globals.Instance.levelsCompleted[i] = ((loadData & checkBit) > 0);
			checkBit = checkBit << 1;
		}
		
		loadFile.Close();
	}

	//TODO call save game when level is completed and load when game is started
}
