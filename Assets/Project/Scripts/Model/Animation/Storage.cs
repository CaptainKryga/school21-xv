
namespace Animation
{
	public class Storage : Place
	{
		public StorageCell[] storageCells;

		private int id = 0;
		
		private void Start()
		{
			storageCells = transform.GetComponentsInChildren<StorageCell>();
		}

		public void GetOneItem()
		{
			storageCells[id].gameObject.SetActive(false);
			id++;
			if (id >= storageCells.Length)
			{
				id = 0;
				for (int x = 0; x < storageCells.Length; x++)
				{
					storageCells[x].gameObject.SetActive(true);
				}
			}
		}
	}
}