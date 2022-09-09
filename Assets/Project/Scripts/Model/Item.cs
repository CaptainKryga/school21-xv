using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public string defaultName;
	public string itemName;
	public Color color;

	public void SetColor(Color color)
	{
		MeshRenderer[] mrs = this.gameObject.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < mrs.Length; i++)
		{
			mrs[i].material.color = color;
		}

		this.color = color;
	}
}
