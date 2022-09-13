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
		
		private Dynamic nowSelectedDynamic;
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
			if (!nowSelectedDynamic || !isMove)
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
			if (key != KeyCode.LeftShift || !nowSelectedDynamic)
				return;

			isMove = true;

			if (isMove && !itemCreate)
			{
				itemCreate = nowSelectedDynamic.AddComponent<ItemCreate>();
				itemCreate.Init(correct, incorrect);
			}
		}

		private void ReceiveMouse(KeyCode key)
		{
			if (model.GetStateGame != GameTypes.Game.Change)
				return;
			
			if (key == KeyCode.Mouse1)
			{
				nowSelectedDynamic.transform.position = savePosition;
				nowSelectedDynamic.transform.rotation = saveRotation;

				Destroy(itemCreate);
				nowSelectedDynamic = null;
				
				// Destroy(nowSelectedItem.transform.gameObject);
				isMove = false;
				wCreateChange.SetItemName("null");
			}
		}

		public void OnClick_LCM()
		{
			if (model.GetStateGame != GameTypes.Game.Change)
				return;
			
			if (itemCreate)
			{
				Destroy(itemCreate);
				nowSelectedDynamic = null;
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

				if (hit.collider && hit.collider.GetComponent<Dynamic>())
				{
					nowSelectedDynamic = hit.collider.GetComponent<Dynamic>();
					rigidbody = nowSelectedDynamic.GetComponent<Rigidbody>();

					wCreateChange.SetItemName(nowSelectedDynamic.itemName);

					savePosition = nowSelectedDynamic.transform.position;
					saveRotation = nowSelectedDynamic.transform.rotation;
				}
			}
		}

		private void ReceiveMouseScroll(float scroll)
		{
			if (!nowSelectedDynamic || !isMove)
				return;
			
			rigidbody.MoveRotation(nowSelectedDynamic.transform.rotation * Quaternion.Euler(Vector3.up * scroll * 100));
			nowSelectedDynamic.transform.Rotate(Vector3.up * scroll * 100);
		}

		public string GetSceneName()
		{
			return model.SceneName;
		}		
		
		public void SetSceneName(string scene)
		{
			model.SceneName = scene;
		}

		public void RenameNowSelectedItemName(string newName)
		{
			if (nowSelectedDynamic)
			{
				nowSelectedDynamic.itemName = newName;
			}
		}

		public void DeleteNowSelectedItem()
		{
			if (nowSelectedDynamic)
			{
				Destroy(nowSelectedDynamic.gameObject);
				wCreateChange.SetItemName("null");
			}
		}
		
		public void SetColorNowSelectedItem(Color color)
		{
			if (nowSelectedDynamic)
			{
				nowSelectedDynamic.SetColor(color);
			}
		}
	}
}