
using System.Collections;
using Project.Scripts.Utils;
using UnityEngine;

namespace Animation
{
	public class Storage : Place
	{
		public StorageCell[] storageCells;
		public GameTypes.Item outputItem;

		private void Start()
		{
			storageCells = transform.GetComponentsInChildren<StorageCell>();
			// StartCoroutine(Sleep());
		}

		IEnumerator Sleep()
		{
			yield return new WaitForSeconds(5);
			for (int x = 0; x < storageCells.Length; x++)
			{
				storageCells[x].gameObject.SetActive(false);
				yield return new WaitForSeconds(1);
			}
			yield break;
		}
	}
}