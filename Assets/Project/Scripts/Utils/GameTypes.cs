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
	}
}