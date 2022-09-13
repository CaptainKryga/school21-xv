using System.Collections.Generic;
using UnityEngine;

//REFACTOR MATERIALSSSSSSSS
public class Dynamic : MonoBehaviour
{
	[SerializeField] private Transform point;
	[SerializeField] protected UnityEngine.Animation anim;
	
	private Material[] materials;
	public string defaultName;
	public string itemName;
	public Color color;

	private static int id;
	[SerializeField] private int localId;
	
	public int LocalId
	{
		get => localId;
		set
		{
			if (id < value)
				id = value + 1;
			localId = value;
		}
	}

	public Transform Point { get => point; }
	
	private void Awake()
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

	public void Init()
	{
		localId = id++;
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
