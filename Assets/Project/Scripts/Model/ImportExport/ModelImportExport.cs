using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Animation;
using Project.Scripts.Model.Animation;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.Model.ImportExport
{
	public class ModelImportExport : MonoBehaviour
	{
		[SerializeField] private ModelController model;
		[SerializeField] private ModelAnimation modelAnimation;
		[SerializeField] private DataBase dataBase;
		[SerializeField] private WindowImportExport wImportExport;

		private string format = ".xv";
		[SerializeField] private Transform parentItems;

		[SerializeField] private Player player;
		[SerializeField] private Worker worker;

		private string pathToImportExportDirectory;

		private string[] lastScanSaveFiles;

		public string[] LastScanSaveFiles
		{
			get
			{
				UpdateArraySaveFiles();
				return lastScanSaveFiles;
			}
		}
		public string PathToImportExportDirectory { get => pathToImportExportDirectory; }

		private void Awake()
		{
			pathToImportExportDirectory = Application.persistentDataPath + "/ImportExport/";

			try
			{
				if (!Directory.Exists(pathToImportExportDirectory))
					Directory.CreateDirectory(pathToImportExportDirectory);
				lastScanSaveFiles = UpdateArraySaveFiles();

				Debug.Log("IMPORT EXPORT РАБОТАЮТ");
			}
			catch (Exception e)
			{
				Debug.Log("IMPORT EXPORT НЕ РАБОТАЮТ");
				Console.WriteLine(e);
				throw;
			}
		}

		public void PreExportScene(string saveFileName)
		{
			// UpdateArraySaveFiles();

			ExportScene(saveFileName);

			// wImportExport.UpdateContent();
		}

		private void ExportScene(string saveFileName)
		{
			BinaryFormatter bf2 = new BinaryFormatter();
			// FileStream file2 = File.Create(pathToImportExportDirectory + saveFileName + format);
			FileStream file2 = File.Create(saveFileName);

			SaveData save = new SaveData();
			save.sceneName = saveFileName;
			save.stateGame = model.GetStateGame;

			int childCount = parentItems.childCount;
			save.itemDefaultName = new string[childCount];
			save.itemName = new string[childCount];
			save.id = new int[childCount];
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
				save.id[i] = dynamic.LocalId;
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
			
			ContentTask[] tasks = modelAnimation.GetActualTasks();
			if (tasks == null)
				save.lenght = 0;
			else
				save.lenght = tasks.Length;
			if (save.lenght > 0)
			{
				save.lenght = tasks.Length;
				save.taskName = new string[tasks.Length];
				save.taskType = new GameTypes.Task[tasks.Length];
				save.taskDescription = new string[tasks.Length];
				save.taskPlaceAId = new int[tasks.Length];
				save.taskPlaceBId = new int[tasks.Length];
				save.taskItem = new GameTypes.Item[tasks.Length];
				save.taskSpeed = new float[tasks.Length];
				save.taskIterations = new int[tasks.Length];
				save.taskNowIterations = new int[tasks.Length];
				save.taskParentTask = new int[tasks.Length];
				save.taskChildTask = new int[tasks.Length];

				for (int i = 0; i < tasks.Length; i++)
				{
					save.taskName[i] = tasks[i].TextNameTask;
					save.taskType[i] = tasks[i].Type;
					save.taskDescription[i] = tasks[i].Description;
					save.taskPlaceAId[i] = tasks[i].PlaceA ? tasks[i].PlaceA.LocalId : -1;
					save.taskPlaceBId[i] = tasks[i].PlaceB ? tasks[i].PlaceB.LocalId : -1;
					Debug.Log(" >>>> PlaceA: " + tasks[i].PlaceA + " | " + (tasks[i].PlaceA ? tasks[i].PlaceA.LocalId : -1));
					save.taskSpeed[i] = tasks[i].Speed;
					save.taskIterations[i] = tasks[i].Iterations;
					save.taskNowIterations[i] = tasks[i].NowIterations;
					save.taskParentTask[i] = GetIdTAsk(tasks[i].ParentTask, tasks);
					save.taskChildTask[i] = GetIdTAsk(tasks[i].ChildTask, tasks);
				}
			}

			bf2.Serialize(file2, save);
			file2.Close();

			Debug.Log("Game data exported!");
		}

		public void PreImportScene(string loadFileName)
		{
			StartCoroutine(ImportScene(loadFileName));
		}

		IEnumerator ImportScene(string loadFileName)
		{
			Dynamic[] saveItems = parentItems.GetComponentsInChildren<Dynamic>();
			for (int i = 0; i < saveItems.Length; i++)
			{
				Destroy(saveItems[i].gameObject);
			}

			yield return new WaitForSeconds(0.1f);
			
			try
			{
				BinaryFormatter bf = new BinaryFormatter();
				// FileStream file = File.Open(pathToImportExportDirectory + loadFileName + format, FileMode.Open);
				FileStream file = File.Open(loadFileName, FileMode.Open);
				SaveData load = (SaveData) bf.Deserialize(file);
				file.Close();

				for (int i = 0; i < load.itemDefaultName.Length; i++)
				{
					for (int x = 0; x < dataBase.defaultPrefabs.Length; x++)
					{
						if (dataBase.defaultPrefabs[x].GetComponent<Dynamic>().defaultName == load.itemDefaultName[i])
						{
							GameObject newItem = Instantiate(dataBase.defaultPrefabs[x]);
							newItem.GetComponent<Dynamic>().defaultName = load.itemDefaultName[i];
							newItem.GetComponent<Dynamic>().itemName = load.itemName[i];
							newItem.GetComponent<Dynamic>().LocalId = load.id[i];
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

				player.GetBodyTransform.position = player.PlayerBodyStartPosition;
				player.GetBodyTransform.rotation = player.PlayerBodyStartRotation;
				player.GetCamTransform.position = player.PlayerCamStartPosition;
				player.GetCamTransform.rotation = player.PlayerCamStartRotation;

				worker.GetTransform.position = worker.WorkerBodyStartPosition;
				worker.GetTransform.rotation = worker.WorkerBodyStartRotation;

				if (load.lenght > 0)
				{
					ContentTask[] tasks = new ContentTask[load.taskType.Length];
					for (int i = 0; i < tasks.Length; i++)
					{
						Debug.Log(i);
						tasks[i] = Instantiate(load.taskParentTask[i] != -1 ? 
								modelAnimation.WindowAnimation.PrefabSubEndTask :
								modelAnimation.WindowAnimation.PrefabContentTask,
							modelAnimation.WindowAnimation.ParentContent).GetComponent<ContentTask>();
					
						tasks[i].InitSaveLoad(load.taskName[i], load.taskDescription[i], load.taskType[i],
							load.taskItem[i], load.taskSpeed[i], load.taskIterations[i], load.taskNowIterations[i]);
					}

					Place[] places = modelAnimation.GetAllPlaces();
			
					for (int i = 0; i < tasks.Length; i++)
					{
						tasks[i].InitSaveLoadSecond(GetPlace(load.taskPlaceAId[i], places), 
							GetPlace(load.taskPlaceBId[i], places), 
							(load.taskParentTask[i] == -1 ? null : tasks[load.taskParentTask[i]]), 
							(load.taskChildTask[i] == -1 ? null : tasks[load.taskChildTask[i]]));

						tasks[i].InitButtons(modelAnimation.UpdatePositionTasks, modelAnimation.DestroyTask);
					}

					modelAnimation.SetActualTasks(tasks);
				}
				
				
				model.UpdateGameState(GameTypes.Game.SceneImportExport);
				model.UpdatePlayerState(GameTypes.PlayerMove.Spectator);

				Debug.Log("Game data imported!");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Debug.LogError("БИТЫЙ ФАЙЛ ВЫБЕРИТЕ ДРУГОЙ!!!");
				throw;
			}
			
			yield break;
		}
		
		private int GetIdTAsk(ContentTask task, ContentTask[] tasks)
		{
			for (int i = 0; i < tasks.Length; i++)
			{
				if (task == tasks[i])
					return i;
			}

			return -1;
		}	
	
		private Place GetPlace(int placeId, Place[] places)
		{
			for (int i = 0; i < places.Length; i++)
			{
				Debug.Log("place: " + places[i] + ", id: " + placeId + ", localId: " + places[i].LocalId);
				if (placeId == places[i].LocalId)
					return places[i];
			}

			return null;
		}

		private string[] UpdateArraySaveFiles()
		{
			try
			{
				List<string> temp = new List<string>();
				lastScanSaveFiles = Directory.GetFiles(pathToImportExportDirectory);
				for (int i = 0; i < lastScanSaveFiles.Length; i++)
				{
					if (lastScanSaveFiles[i].Substring(lastScanSaveFiles[i].Length - 3) == format)
					{
						string res = lastScanSaveFiles[i].Substring(pathToImportExportDirectory.Length);
						res = res.Substring(0, res.Length - 3);
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
				Debug.LogError("НЕВОЗМОЖНО ПЕРЕИМЕНОВАТЬ СЦЕНУ, ЭТО ИМЯ ЗАНЯТО");
				return false;
			}

			if (!lastScanSaveFiles.Contains(oldName))
			{
				Debug.LogError("НЕВОЗМОЖНО ПЕРЕИМЕНОВАТЬ СЦЕНУ, ЭКСПОРТИРУЙТЕ ЕЁ ВНАЧАЛЕ ИЛИ ИМПОРТИРУЙТЕ СТАРУЮ");
				Debug.LogError("тут можно бахнуть сразу сохранение что думаешь???");
				return false;
			}

			try
			{
				File.Move(pathToImportExportDirectory + oldName + format,
					pathToImportExportDirectory + newName + format);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}

			return true;
		}
	}
}