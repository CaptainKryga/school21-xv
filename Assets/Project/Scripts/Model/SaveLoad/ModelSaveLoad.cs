using System;
using System.IO;
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

	[SerializeField] private SettingsScene defaultScene;
	[SerializeField] private SettingsScene[] saveScenes = new SettingsScene[10];
	
	[SerializeField] private Transform parentItems;
	[SerializeField] private Player player;
	[SerializeField] private Worker worker;

	[SerializeField] private bool isSaveDefaultScene;
	[SerializeField] private bool isLoadDefaultScene;
	
	public SettingsScene[] GetSaveScenes { get => saveScenes; }

	private string[] saveNames =
		{"save0", "save1", "save2", "save3", "save4", "save5", "save6", "save7", "save8", "save9"};
	
	private void Start()
	{
		try
		{
			if (!Directory.Exists(Application.persistentDataPath + "/Save"))
				Directory.CreateDirectory(Application.persistentDataPath + "/Save");
			
			string[] allfiles = Directory.GetFiles(Application.persistentDataPath + "");
			for (int x = 0; x < saveNames.Length; x++)
			{
				bool isCreate = false;
				for (int y = 0; y < allfiles.Length; y++)
				{
					if (saveNames[x] == allfiles[y])
					{
						isCreate = true;
						break;
					}
				}

				if (!isCreate)
				{
					// BinaryFormatter bf = new BinaryFormatter(); 
					// FileStream file = File.Create(Application.persistentDataPath 
												// + "/Save/" + saveNames[x]);
					// SettingsScene data = new SettingsScene();
					// data.isSave = true;
					// data.stateGame = model.GetStateGame;
					// data.items = new ItemData[parentItems.childCount];
					// for (int i = 0; i < data.items.Length; i++)
					// {
						// Item item = parentItems.GetChild(i).GetComponent<Item>();
						// data.items[i] = new ItemData(item.defaultName, item.itemName, item.transform.position,
							// item.transform.rotation, item.color);
					// }
					// data.playerData.bodyPosition = player.GetBodyTransform.position;
					// data.playerData.bodyRotation = player.GetBodyTransform.rotation;
					// data.playerData.camPosition = player.GetCamTransform.position;
					// data.playerData.camRotation = player.GetCamTransform.rotation;
					// data.workerData.position = worker.GetTransform.position;
					// data.workerData.rotation = worker.GetTransform.rotation;
					// data.playerData.state = player.GetState;
					
					// bf.Serialize(file, data);
					// file.Close();
				}
					
			}

		}
		catch (Exception e)
		{
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

	public void PreSaveScene(string save)
	{
		if (save == defaultS)
		{
			SaveScene(defaultScene);
			return;
		}

		for (int i = 0; i < saveScenes.Length; i++)
		{
			if (save == saveScenes[i].sceneName)
			{
				SaveScene(saveScenes[i]);
				return;
			}
		}

		Debug.LogError("НЕВОЗМОЖНО СОХРАНИТЬ ИГРУ, СЦЕНА НЕ НАЙДЕНА");
	}

	private void SaveScene(SettingsScene save2)
	{
		BinaryFormatter bf2 = new BinaryFormatter(); 
		FileStream file2 = File.Create(Application.persistentDataPath 
									+ "/Save/ddfs");
		
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
		Debug.Log("Game data saved!");
	}

	public void PreLoadScene(string load)
	{
		if (load == defaultS)
		{
			LoadScene(defaultScene);
			return;
		}
		
		for (int i = 0; i < saveScenes.Length; i++)
		{
			if (load == saveScenes[i].sceneName)
			{
				LoadScene(saveScenes[i]);
				return;
			}
		}
		
		Debug.LogError("НЕВОЗМОЖНО ЗАГРУЗИТЬ ИГРУ, СЦЕНА НЕ НАЙДЕНА");
	}

	private void LoadScene(SettingsScene load)
	{
		for (int i = 0; i < parentItems.childCount; i++)
		{
			Destroy(parentItems.GetChild(i).transform.gameObject);
		}
		
		for (int i = 0; i < load.items.Length; i++)
		{
			for (int x = 0; x < dataBase.defaultPrefabs.Length; x++)
			{
				if (dataBase.defaultPrefabs[x].GetComponent<Item>().defaultName == load.items[i].defaultName)
				{
					GameObject newItem = Instantiate(dataBase.defaultPrefabs[x]);
					newItem.GetComponent<Item>().defaultName = load.items[i].defaultName;
					newItem.GetComponent<Item>().itemName = load.items[i].itemName;
					newItem.GetComponent<Item>().SetColor(load.items[i].color);
					newItem.transform.position = load.items[i].position;
					newItem.transform.rotation = load.items[i].rotation;
					newItem.transform.SetParent(parentItems);
					
					break;
				}
			}
		}

		player.GetBodyTransform.position = load.playerData.bodyPosition;
		player.GetBodyTransform.rotation = load.playerData.bodyRotation;
		
		player.GetCamTransform.position = load.playerData.camPosition;
		player.GetCamTransform.rotation = load.playerData.camRotation;
		
		worker.GetTransform.position = load.workerData.position;
		worker.GetTransform.rotation = load.workerData.rotation;
		
		model.UpdateGameState(load.stateGame);
		model.UpdatePlayerState(load.playerData.state);
	}


	// private SettingsScene LoadScene(string sceneName)
	// {
	// 	return AssetDatabase.LoadAssetAtPath<SettingsScene>(pathSaveLoad + sceneName + type);
	// }
	//
	// private void CreateAsset()
	// {
	// 	SettingsScene save = ScriptableObject.CreateInstance<SettingsScene>();
	// 	string path = pathSaveLoad + "save";
	// 	AssetDatabase.CreateAsset(save, path + ".asset");
	// 	AssetDatabase.SaveAssets();
	// }
	//
	// private void DestroyAsset(string name)
	// {
	// 	AssetDatabase.DeleteAsset(pathSaveLoad + name + ".asset");
	// 	AssetDatabase.SaveAssets();
	// }
}
