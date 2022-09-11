using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Project.Scripts.Model;
using UnityEngine;

public class ModelSaveLoad : MonoBehaviour
{
	[SerializeField] private ModelController model;
	[SerializeField] private DataBase dataBase;

	[SerializeField] private WindowSaveLoad wSaveLoad;
	
	private string format = ".save";
	
	[SerializeField] private Transform parentItems;
	[SerializeField] private Player player;
	[SerializeField] private Worker worker;

	private string pathToSaveDirectory;
	
	private string[] lastScanSaveFiles;

	public string[] LastScanSaveFiles
	{
		get
		{
			UpdateArraySaveFiles();
			return lastScanSaveFiles;
		}
	}
	
	private void Awake()
	{
		pathToSaveDirectory = Application.persistentDataPath + "/Save/";
		
		try
		{
			if (!Directory.Exists(pathToSaveDirectory))
				Directory.CreateDirectory(pathToSaveDirectory);
			lastScanSaveFiles = UpdateArraySaveFiles();
			
			Debug.Log("СОХРАНЕНИЯ РАБОТАЮТ");
		}
		catch (Exception e)
		{
			Debug.Log("СОХРАНЕНИЯ НЕ РАБОТАЮТ");
			Console.WriteLine(e);
			throw;
		}
	}

	public void PreSaveScene(string saveFileName)
	{
		UpdateArraySaveFiles();
		
		SaveScene(saveFileName);
		
		wSaveLoad.UpdateContent();
	}

	private void SaveScene(string saveFileName)
	{
		BinaryFormatter bf2 = new BinaryFormatter(); 
		FileStream file2 = File.Create(pathToSaveDirectory + saveFileName + format);
		
		SaveData save = new SaveData();
		save.sceneName = saveFileName;
		save.stateGame = model.GetStateGame;
		
		int childCount = parentItems.childCount;
		save.itemDefaultName = new string[childCount];
		save.itemName = new string[childCount];
		// save.position = new Vector3[childCount];
		save.itemPositionX = new float[childCount];
		save.itemPositionY = new float[childCount];
		save.itemPositionZ = new float[childCount];
		// save.rotation = new Quaternion[childCount];
		save.itemRotationX = new float[childCount];
		save.itemRotationY = new float[childCount];
		save.itemRotationZ = new float[childCount];
		save.itemRotationW = new float[childCount];
		// save.color = new Color[childCount];
		save.itemColorR = new float[childCount];
		save.itemColorG = new float[childCount];
		save.itemColorB = new float[childCount];
		save.itemColorA = new float[childCount];
		for (int i = 0; i < save.itemDefaultName.Length; i++)
		{
			Dynamic @dynamic = parentItems.GetChild(i).GetComponent<Dynamic>();
			save.itemDefaultName[i] = dynamic.defaultName;
			save.itemName[i] = dynamic.itemName;
			// save.position[i] = item.transform.position;
			save.itemPositionX[i] = dynamic.transform.position.x;
			save.itemPositionY[i] = dynamic.transform.position.y;
			save.itemPositionZ[i] = dynamic.transform.position.z;
			// save.rotation[i] = item.transform.rotation;
			save.itemRotationX[i] = dynamic.transform.rotation.x;
			save.itemRotationY[i] = dynamic.transform.rotation.y;
			save.itemRotationZ[i] = dynamic.transform.rotation.z;
			save.itemRotationW[i] = dynamic.transform.rotation.w;
			// save.color[i] = item.color;
			save.itemColorR[i] = dynamic.color.r;
			save.itemColorG[i] = dynamic.color.g;
			save.itemColorB[i] = dynamic.color.b;
			save.itemColorA[i] = dynamic.color.a;

		}
		
		// save.playerBodyPosition = player.GetBodyTransform.position;
		save.playerBodyPositionX = player.GetBodyTransform.position.x;
		save.playerBodyPositionY = player.GetBodyTransform.position.y;
		save.playerBodyPositionZ = player.GetBodyTransform.position.z;
		// save.playerBodyRotation = player.GetBodyTransform.rotation;
		save.playerBodyRotationX = player.GetBodyTransform.rotation.x;
		save.playerBodyRotationY = player.GetBodyTransform.rotation.y;
		save.playerBodyRotationZ = player.GetBodyTransform.rotation.z;
		save.playerBodyRotationW = player.GetBodyTransform.rotation.w;
		// save.playerCamPosition = player.GetCamTransform.position;
		save.playerCamPositionX = player.GetCamTransform.position.x;
		save.playerCamPositionY = player.GetCamTransform.position.y;
		save.playerCamPositionZ = player.GetCamTransform.position.z;
		// save.playerCamRotation = player.GetCamTransform.rotation;
		save.playerCamRotationX = player.GetCamTransform.rotation.x;
		save.playerCamRotationY = player.GetCamTransform.rotation.y;
		save.playerCamRotationZ = player.GetCamTransform.rotation.z;
		save.playerCamRotationW = player.GetCamTransform.rotation.w;
		
		save.playerMoveState = player.GetState;
		
		// save.workerBodyPosition = worker.GetTransform.position;
		save.workerBodyPositionX = worker.GetTransform.position.x;
		save.workerBodyPositionY = worker.GetTransform.position.y;
		save.workerBodyPositionZ = worker.GetTransform.position.z;
		// save.workerBodyRotation = worker.GetTransform.rotation;
		save.workerBodyRotationX = worker.GetTransform.rotation.x;
		save.workerBodyRotationY = worker.GetTransform.rotation.y;
		save.workerBodyRotationZ = worker.GetTransform.rotation.z;
		save.workerBodyRotationW = worker.GetTransform.rotation.w;
			
		bf2.Serialize(file2, save);
		file2.Close();
		
		Debug.Log("Game data saved!");
	}

	public void PreLoadScene(string loadFileName)
	{
		lastScanSaveFiles = UpdateArraySaveFiles();

		if (lastScanSaveFiles.Contains(loadFileName))
		{
			LoadScene(loadFileName);
			return;
		}
		
		Debug.LogError("НЕВОЗМОЖНО ЗАГРУЗИТЬ ИГРУ, СЦЕНА НЕ НАЙДЕНА");
	}

	private void LoadScene(string loadFileName)
	{
		Dynamic[] saveItems = parentItems.GetComponentsInChildren<Dynamic>();

		try
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(pathToSaveDirectory + loadFileName + format, FileMode.Open);
			SaveData load = (SaveData) bf.Deserialize(file);
			file.Close();

			// save.saveName = "default";
			// save.sceneName = "basic";

			model.SceneName = load.sceneName;
			
			// int childCount = parentItems.childCount;
			for (int i = 0; i < load.itemDefaultName.Length; i++)
			{
				for (int x = 0; x < dataBase.defaultPrefabs.Length; x++)
				{
					if (dataBase.defaultPrefabs[x].GetComponent<Dynamic>().defaultName == load.itemDefaultName[i])
					{
						GameObject newItem = Instantiate(dataBase.defaultPrefabs[x]);
						newItem.GetComponent<Dynamic>().defaultName = load.itemDefaultName[i];
						newItem.GetComponent<Dynamic>().itemName = load.itemName[i];
						newItem.transform.position = new Vector3(load.itemPositionX[i], load.itemPositionY[i],
							load.itemPositionZ[i]);
						newItem.transform.rotation = new Quaternion(load.itemRotationX[i], load.itemRotationY[i],
							load.itemRotationZ[i], load.itemRotationW[i]);
						newItem.GetComponent<Dynamic>().InitColor(new Color(load.itemColorR[i], load.itemColorG[i],
							load.itemColorB[i], load.itemColorA[i]));
						newItem.transform.SetParent(parentItems);
						break;
					}
				}
			}
			// save.playerBodyPosition = player.GetBodyTransform.position;
			player.GetBodyTransform.position = new Vector3(load.playerBodyPositionX, load.playerBodyPositionY,
				load.playerBodyPositionZ);
			// save.playerBodyRotation = player.GetBodyTransform.rotation;
			player.GetBodyTransform.rotation = new Quaternion(load.playerBodyRotationX, load.playerBodyRotationY, 
				load.playerBodyRotationZ, load.playerBodyRotationW);
			// save.playerCamPosition = player.GetCamTransform.position;
			player.GetCamTransform.position = new Vector3(load.playerCamPositionX, load.playerCamPositionY,
				load.playerCamPositionZ);
			// save.playerCamRotation = player.GetCamTransform.rotation;
			player.GetCamTransform.rotation = new Quaternion(load.playerCamRotationX, load.playerCamRotationY, 
				load.playerCamRotationZ, load.playerCamRotationW);

			// save.workerBodyPosition = worker.GetTransform.position;
			worker.GetTransform.position = new Vector3(load.workerBodyPositionX, load.workerBodyPositionY,
				load.workerBodyPositionZ);
			// save.workerBodyRotation = worker.GetTransform.rotation;
			worker.GetTransform.rotation = new Quaternion(load.workerBodyRotationX, load.workerBodyRotationY, 
				load.workerBodyRotationZ, load.workerBodyRotationW);

			model.UpdateGameState(load.stateGame);
			model.UpdatePlayerState(load.playerMoveState);
			
			Debug.Log("Game data loaded!");
			
			for (int i = 0; i < saveItems.Length; i++)
			{
				Destroy(saveItems[i].gameObject);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			Debug.Log("БИТЫЙ ФАЙЛ ВЫБЕРИТЕ ДРУГОЙ!!!");
			throw;
		}
	}

	private string[] UpdateArraySaveFiles()
	{
		try
		{
			List<string> temp = new List<string>();
			lastScanSaveFiles = Directory.GetFiles(pathToSaveDirectory);
			for (int i = 0; i < lastScanSaveFiles.Length; i++)
			{
				if (lastScanSaveFiles[i].Substring(lastScanSaveFiles[i].Length - 5) == format)
				{
					string res = lastScanSaveFiles[i].Substring(pathToSaveDirectory.Length);
					res = res.Substring(0, res.Length - 5);
					temp.Add(res);
				}
			}

			lastScanSaveFiles = temp.ToArray();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return lastScanSaveFiles;
	}

	public bool Rename(string oldName, string newName)
	{
		if (lastScanSaveFiles.Contains(newName))
		{
			Debug.LogError("НЕВОЗМОЖНО ПЕРЕИМЕНОВАТЬ ИГРУ, ЭТО ИМЯ ЗАНЯТО");
			return false;
		}

		if (!lastScanSaveFiles.Contains(oldName))
		{
			Debug.LogError("НЕВОЗМОЖНО ПЕРЕИМЕНОВАТЬ ИГРУ, СОХРАНИТЕ ЕЁ ВНАЧАЛЕ ИЛИ ЗАГРУЗИТЕ СТАРУЮ");
			Debug.LogError("тут можно бахнуть сразу сохранение что думаешь???");
			return false;
		}
		
		try
		{
			File.Move(pathToSaveDirectory + oldName + format, 
				pathToSaveDirectory + newName + format);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return true;
	}
}
