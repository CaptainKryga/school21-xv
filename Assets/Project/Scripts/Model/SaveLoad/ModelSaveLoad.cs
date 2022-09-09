using System;
using Project.Scripts.Model;
using Unity.VisualScripting;
using UnityEditor;
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

	private void Start()
	{
		//пробегаемся по всем итемам и смотрим саймый крайний от него создаём следующий дефолтный скриптейбл обж
		//load defaultScene?
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

	private void PreSaveScene(string save)
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

	private void SaveScene(SettingsScene save)
	{
		save.isSave = true;

		save.stateGame = model.GetStateGame;

		save.items = new ItemData[parentItems.childCount];
		for (int i = 0; i < save.items.Length; i++)
		{
			Item item = parentItems.GetChild(i).GetComponent<Item>();
			save.items[i] = new ItemData(item.defaultName, item.itemName, item.transform.position,
				item.transform.rotation, item.color);
		}

		save.playerData.bodyPosition = player.GetBodyTransform.position;
		save.playerData.bodyRotation = player.GetBodyTransform.rotation;
		
		save.playerData.camPosition = player.GetCamTransform.position;
		save.playerData.camRotation = player.GetCamTransform.rotation;
		
		save.workerData.position = worker.GetTransform.position;
		save.workerData.rotation = worker.GetTransform.rotation;
	}

	public void PreLoadScene(string load)
	{
		if (load == defaultS)
		{
			LoadScene(defaultScene);
		}
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
				Debug.Log("prefab: " + dataBase.defaultPrefabs[x].name + 
						" == load.data: " + load.items[i].defaultName);
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
		
		model.UpdateState(load.stateGame);
	}


	private SettingsScene LoadScene(string sceneName)
	{
		return AssetDatabase.LoadAssetAtPath<SettingsScene>(pathSaveLoad + sceneName + type);
	}

	private void CreateAsset()
	{
		SettingsScene save = ScriptableObject.CreateInstance<SettingsScene>();
		string path = pathSaveLoad + "save";
		AssetDatabase.CreateAsset(save, path + ".asset");
		AssetDatabase.SaveAssets();
	}
	
	private void DestroyAsset(string name)
	{
		AssetDatabase.DeleteAsset(pathSaveLoad + name + ".asset");
		AssetDatabase.SaveAssets();
	}
}
