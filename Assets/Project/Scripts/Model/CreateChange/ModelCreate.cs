using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Model.CreateChange
{
	public class ModelCreate : MonoBehaviour
	{
		[SerializeField] private ControllerGame game;
		[SerializeField] private ModelController model;
		[SerializeField] private DataBase dataBase;
		[SerializeField] private Transform parentItems;
		private int selectedId = -1;
		private GameObject nowCreateGO;
		private Rigidbody rigidbody;
		
		public int SelectedId { get => selectedId; set => selectedId = value; }

		private void OnEnable()
		{
			game.Keyboard_Action += ReceiveKeyboard;
			game.Mouse_Action += ReceiveMouse;
			game.MouseScroll_Action += ReceiveMouseScroll;
		}

		private void OnDisable()
		{
			game.Keyboard_Action -= ReceiveKeyboard;
			game.Mouse_Action -= ReceiveMouse;
			game.MouseScroll_Action -= ReceiveMouseScroll;
		}

		private void Update()
		{
			if (!nowCreateGO)
				return;
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100))
			{
				Debug.Log("hit " + hit.transform.name);
			}
			
			if (hit.collider)
				rigidbody.MovePosition(hit.point);
		}

		private void ReceiveKeyboard(KeyCode key)
		{
			if (key != KeyCode.LeftShift || selectedId == -1)
				return;

			if (nowCreateGO)
			{
				Destroy(nowCreateGO.transform.gameObject);
				nowCreateGO = null;
			}

			nowCreateGO = Instantiate(dataBase.defaultPrefabs[selectedId], parentItems);
			nowCreateGO.AddComponent<ItemCreate>();
			rigidbody = nowCreateGO.GetComponent<Rigidbody>();
		}

		private void ReceiveMouse(KeyCode key)
		{
			if (!nowCreateGO)
				return;
			
			if (key == KeyCode.Mouse0)
			{
				//standing
				if (nowCreateGO.GetComponent<ItemCreate>().Standing())
				{
					Destroy(nowCreateGO.GetComponent<ItemCreate>());
					nowCreateGO = null;
				}
			}
			else if (key == KeyCode.Mouse1)
			{
				//destroy
				Destroy(nowCreateGO.transform.gameObject);
				nowCreateGO = null;
			}
		}

		private void ReceiveMouseScroll(float scroll)
		{
			if (!nowCreateGO)
				return;
			
			rigidbody.MoveRotation(nowCreateGO.transform.rotation * Quaternion.Euler(Vector3.up * scroll * 100));
			nowCreateGO.transform.Rotate(Vector3.up * scroll * 100);
		}
	}
}