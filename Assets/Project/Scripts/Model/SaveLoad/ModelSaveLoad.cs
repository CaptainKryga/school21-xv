using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Project.Scripts.Model;
using UnityEngine;

public class ModelSaveLoad : MonoBehaviour
{
	[SerializeField] private ModelController model;
	[SerializeField] private DataBase dataBase;
	
	private const string pathSaveLoad = "Assets/Resources/Save";
	private const string type = ".asset";
	private const string defaultS = "default";

	// [SerializeField] private SettingsScene defaultScene;
	// [SerializeField] private SettingsScene[] saveScenes = new SettingsScene[10];
	
	[SerializeField] private Transform parentItems;
	[SerializeField] private Player player;
	[SerializeField] private Worker worker;

	[SerializeField] private bool isSaveDefaultScene;
	[SerializeField] private bool isLoadDefaultScene;
	
	private string[] lastScanSaveFiles;
	
	public string[] LastScanSaveFiles { get => lastScanSaveFiles; }
	
	private void Awake()
	{
		try
		{
			if (!Directory.Exists(Application.persistentDataPath + "/Save"))
				Directory.CreateDirectory(Application.persistentDataPath + "/Save");
			lastScanSaveFiles = updateArraySaveFiles();
			
			Debug.Log("СОХРАНЕНИЯ РАБОТАЮТ");
		}
		catch (Exception e)
		{
			Debug.Log("СОХРАНЕНИЯ НЕ РАБОТАЮТ");
			Console.WriteLine(e);
			throw;
		}
	}

	private void Update()
	{
		if (isSaveDefaultScene)
		{
			isSaveDefaultScene = false;
			
			PreSaveScene(defaultS);
		}

		if (isLoadDefaultScene)
		{
			isLoadDefaultScene = false;
			
			PreLoadScene(defaultS);
		}
	}

	public void PreSaveScene(string saveFileName)
	{
		lastScanSaveFiles = updateArraySaveFiles();
		
		if (!lastScanSaveFiles.Contains(saveFileName))
		{
			SaveScene(saveFileName);
			return;
		}
		Debug.LogError("НЕВОЗМОЖНО СОХРАНИТЬ ИГРУ, СЦЕНА НЕ НАЙДЕНА");
	}

	private void SaveScene(string saveFileName)
	{
		BinaryFormatter bf2 = new BinaryFormatter(); 
		FileStream file2 = File.Create(Application.persistentDataPath 
									+ "/Save/" + saveFileName);
		
		SaveData save = new SaveData();
		save.saveName = "default";
		save.sceneName = "basic";
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
			Item item = parentItems.GetChild(i).GetComponent<Item>();
			save.itemDefaultName[i] = item.defaultName;
			save.itemName[i] = item.itemName;
			// save.position[i] = item.transform.position;
			save.itemPositionX[i] = item.transform.position.x;
			save.itemPositionY[i] = item.transform.position.y;
			save.itemPositionZ[i] = item.transform.position.z;
			// save.rotation[i] = item.transform.rotation;
			save.itemRotationX[i] = item.transform.rotation.x;
			save.itemRotationY[i] = item.transform.rotation.y;
			save.itemRotationZ[i] = item.transform.rotation.z;
			save.itemRotationW[i] = item.transform.rotation.w;
			// save.color[i] = item.color;
			save.itemColorR[i] = item.color.r;
			save.itemColorG[i] = item.color.g;
			save.itemColorB[i] = item.color.b;
			save.itemColorA[i] = item.color.a;

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
		lastScanSaveFiles = updateArraySaveFiles();

		if (!lastScanSaveFiles.Contains(loadFileName))
		{
			LoadScene(loadFileName);
			return;
		}
		
		Debug.LogError("НЕВОЗМОЖНО ЗАГРУЗИТЬ ИГРУ, СЦЕНА НЕ НАЙДЕНА");
	}

	private void LoadScene(string loadFileName)
	{
		for (int i = 0; i < parentItems.childCount; i++)
		{
			Destroy(parentItems.GetChild(i).transform.gameObject);
		}

		try
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/Save/" + loadFileName, FileMode.Open);
			SaveData load = (SaveData) bf.Deserialize(file);
			file.Close();

			// save.saveName = "default";
			// save.sceneName = "basic";

			// int childCount = parentItems.childCount;
			for (int i = 0; i < load.itemDefaultName.Length; i++)
			{
				for (int x = 0; x < dataBase.defaultPrefabs.Length; x++)
				{
					if (dataBase.defaultPrefabs[x].GetComponent<Item>().defaultName == load.itemDefaultName[i])
					{
						GameObject newItem = Instantiate(dataBase.defaultPrefabs[x]);
						newItem.GetComponent<Item>().defaultName = load.itemDefaultName[i];
						newItem.GetComponent<Item>().itemName = load.itemName[i];
						newItem.transform.position = new Vector3(load.itemPositionX[i], load.itemPositionY[i],
							load.itemPositionZ[i]);
						newItem.transform.rotation = new Quaternion(load.itemRotationX[i], load.itemRotationY[i],
							load.itemRotationZ[i], load.itemRotationW[i]);
						newItem.GetComponent<Item>().SetColor(new Color(load.itemColorR[i], load.itemColorG[i],
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
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}

	private string[] updateArraySaveFiles()
	{
		try
		{
			lastScanSaveFiles = Directory.GetFiles(Application.persistentDataPath + "");
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return lastScanSaveFiles;
	}
}
