using System;
using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class SaveData
{ 
	//scene
	// public bool isSave;
	public string sceneName;
	public GameTypes.Game stateGame;
	//item default in resources unity name
	public string[] itemDefaultName;
	//custom item name in game
	public string[] itemName;
	public int[] id;
	// public Vector3[] position;
	public float[] itemPositionX;
	public float[] itemPositionY;
	public float[] itemPositionZ;
	// public Quaternion[] rotation;
	public float[] itemRotationX;
	public float[] itemRotationY;
	public float[] itemRotationZ;
	public float[] itemRotationW;
	// public Color[] color;
	public float[] itemColorR;
	public float[] itemColorG;
	public float[] itemColorB;
	public float[] itemColorA;
	//player
	// public Vector3 playerBodyPosition;
	public float playerBodyPositionX;
	public float playerBodyPositionY;
	public float playerBodyPositionZ;
	// public Quaternion playerBodyRotation;
	public float playerBodyRotationX;
	public float playerBodyRotationY;
	public float playerBodyRotationZ;
	public float playerBodyRotationW;
	// public Vector3 playerCamPosition;
	public float playerCamPositionX;
	public float playerCamPositionY;
	public float playerCamPositionZ;
	// public Quaternion playerCamRotation;
	public float playerCamRotationX;
	public float playerCamRotationY;
	public float playerCamRotationZ;
	public float playerCamRotationW;
	public GameTypes.PlayerMove playerMoveState;
	//worker
	// public Vector3 workerBodyPosition;
	public float workerBodyPositionX;
	public float workerBodyPositionY;
	public float workerBodyPositionZ;
	// public Quaternion workerBodyRotation;
	public float workerBodyRotationX;
	public float workerBodyRotationY;
	public float workerBodyRotationZ;
	public float workerBodyRotationW;
	
	//task's
	public int lenght;
	public string[] taskName;
	public GameTypes.Task[] taskType;
	public string[] taskDescription;
	public int[] taskPlaceAId;
	public int[] taskPlaceBId;
	public GameTypes.Item[] taskItem;
	public float[] taskSpeed;
	public int[] taskIterations;
	public int[] taskNowIterations;
	public int[] taskParentTask;
	public int[] taskChildTask;
}
