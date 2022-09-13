
namespace Animation
{
	public class Storage : Place
	{
		public StorageCell[] storageCells;
		private int i;
		
		private void Start()
		{
			storageCells = transform.GetComponentsInChildren<StorageCell>();
		}

		public void GetOneItem()
		{
			storageCells[i].gameObject.SetActive(false);
			i++;
			if (i >= storageCells.Length)
			{
				i = 0;
				for (int x = 0; x < storageCells.Length; x++)
				{
					storageCells[x].gameObject.SetActive(true);
				}
			}
		}
	}
}