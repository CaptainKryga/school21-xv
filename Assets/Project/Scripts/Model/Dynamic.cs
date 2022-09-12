using System;
using System.Collections.Generic;
using Project.Scripts.Utils;
using UnityEngine;

//REFACTOR MATERIALSSSSSSSS
public class Dynamic : MonoBehaviour
{
	private Material[] materials;
	public string defaultName;
	public string itemName;
	public Color color;

	private void Start()
	{
		MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
		List<Material> mats = new List<Material>();
		for (int x = 0; x < mrs.Length; x++)
		{
			for (int y = 0; y < mrs[x].materials.Length; y++)
			{
				mats.Add(mrs[x].materials[y]);
			}
		}
		materials = mats.ToArray();
	}

	public void InitColor(Color color)
	{
		if (color == Color.white)
			return;

		SetColor(color);
	}
	
	public void SetColor(Color color)
	{
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].color = color;
		}

		this.color = color;
	}
}
