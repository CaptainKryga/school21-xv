using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
	[SerializeField] private TMPro.TMP_Text playerInfoTypeMove;

	public void UpdatePlayerTypeMove(string type)
	{
		playerInfoTypeMove.text = type;
	}
}
