using System;
using Project.Scripts.Utils;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Model.CreateChange
{
	public class ModelChange : MonoBehaviour
	{
		[SerializeField] private ControllerGame game;
		[SerializeField] private ModelController model;

		[SerializeField] private WindowCreateChange wCreateChange;
		
		[SerializeField] private DataBase dataBase;
		[SerializeField] private Transform parentItems;
		
		private Item nowSelectedItem;
		private Rigidbody rigidbody;
		private bool isMove;
		private ItemCreate itemCreate;
		[SerializeField] private Material correct, incorrect;

		private Vector3 savePosition;
		private Quaternion saveRotation;

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
			if (!nowSelectedItem || !isMove)
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
			if (key != KeyCode.LeftShift || !nowSelectedItem)
				return;

			isMove = true;

			if (isMove && !itemCreate)
			{
				itemCreate = nowSelectedItem.AddComponent<ItemCreate>();
				itemCreate.Init(correct, incorrect);
			}
		}

		private void ReceiveMouse(KeyCode key)
		{
			if (model.GetStateGame != GameTypes.Game.Change)
				return;

			if (key == KeyCode.Mouse0)
			{
				if (itemCreate)
				{
					Destroy(itemCreate);
					nowSelectedItem = null;
					isMove = false;
					wCreateChange.SetItemName("null");
				}
				else
				{
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit, 100))
					{
						// Debug.Log("hit " + hit.transform.name);
					}

					if (hit.collider && hit.collider.GetComponent<Item>())
					{
						nowSelectedItem = hit.collider.GetComponent<Item>();
						rigidbody = nowSelectedItem.GetComponent<Rigidbody>();

						wCreateChange.SetItemName(nowSelectedItem.itemName);

						savePosition = nowSelectedItem.transform.position;
						saveRotation = nowSelectedItem.transform.rotation;
					}
				}
			}
			else if (key == KeyCode.Mouse1)
			{
				nowSelectedItem.transform.position = savePosition;
				nowSelectedItem.transform.rotation = saveRotation;

				Destroy(itemCreate);
				nowSelectedItem = null;
				
				// Destroy(nowSelectedItem.transform.gameObject);
				isMove = false;
				wCreateChange.SetItemName("null");
			}
		}

		private void ReceiveMouseScroll(float scroll)
		{
			if (!nowSelectedItem || !isMove)
				return;
			
			rigidbody.MoveRotation(nowSelectedItem.transform.rotation * Quaternion.Euler(Vector3.up * scroll * 100));
			nowSelectedItem.transform.Rotate(Vector3.up * scroll * 100);
		}

		public string GetSceneName()
		{
			return model.SceneName;
		}

		public void RenameNowSelectedItemName(string newName)
		{
			if (nowSelectedItem)
			{
				nowSelectedItem.itemName = newName;
			}
		}

		public void DeleteNowSelectedItem()
		{
			if (nowSelectedItem)
			{
				Destroy(nowSelectedItem.gameObject);
				wCreateChange.SetItemName("null");
			}
		}
		
		public void SetColorNowSelectedItem(Color color)
		{
			if (nowSelectedItem)
			{
				nowSelectedItem.SetColor(color);
			}
		}
	}
}