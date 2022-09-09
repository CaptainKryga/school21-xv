using System;
using Project.Scripts.Model;
using UnityEditor;
using UnityEngine;

public class ModelSaveLoad : MonoBehaviour
{
	private const string pathSaveLoad = "Assets/Resources/Save";
	private const string type = ".asset";

	[SerializeField] private SettingsScene standardScene;
	[SerializeField] private SettingsScene[] saveScenes = new SettingsScene[10];
	
	[SerializeField] private Transform parentItems;
	[SerializeField] private Player player;
	[SerializeField] private Worker worker;

	private void Start()
	{
		//пробегаемся по всем итемам и смотрим саймый крайний от него создаём следующий дефолтный скриптейбл обж

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
