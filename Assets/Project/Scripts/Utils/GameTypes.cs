namespace Project.Scripts.Utils
{
	public class GameTypes
	{
		//текущее состояние игры
		public enum Game
		{
			Play,
			Create,
			Change,
			SceneImportExport,
			Animations,
			GameSaveLoad,
			Video,
			Null
		}

		//стейт типа управления и положения камеры
		public enum PlayerMove
		{
			Spectator,
			HumanFirst,
			HumanThird,
			WorkerFirst,
			WorkerThird,
			Null
		}

		public enum Task
		{
			Transfer,
			Craft,
			Cycle
		}

		public enum Item
		{
			None,
			BoxSmall,
			BoxMiddle,
		}

		public enum Place
		{
			Storage,
			Craft,
		}

		public enum Phase
		{
			First,
			Second,
			Third,
			Fourth
		}

		public enum WorkerAnimations
		{
			None,
			Walk,
			WalkItem,
			UsePlace
		}

		public static Item GetItemFromString(string item)
		{
			if (item == Item.BoxSmall.ToString())
			{
				return Item.BoxSmall;
			}
			else if (item == Item.BoxMiddle.ToString())
			{
				return Item.BoxMiddle;
			}
			return Item.None;
		}
		
		public static Task GetTaskFromString(string task)
		{
			if (task == Task.Transfer.ToString())
			{
				return Task.Transfer;
			}
			else if (task == Task.Craft.ToString())
			{
				return Task.Craft;
			}
			return Task.Cycle;
		}
	}
}