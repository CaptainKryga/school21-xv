using UnityEngine;

public class Item : MonoBehaviour
{
	public string defaultName;
	public string itemName;
	public Color color;
	
	public void InitColor(Color color)
	{
		if (color == Color.white)
			return;
		
		SetColor(color);
	}
	
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
