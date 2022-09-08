namespace Project.Scripts.Utils
{
	public class GameTypes
	{
		//текущее состояние игры
		public enum Game
		{
			Create,
			SceneImportExport,
			Play,
			Pause,
			GameSaveLoad,
			Change
		}

		//стейт типа управления и положения камеры
		public enum PlayerCam
		{
			Spectator,
			HumanFirst,
			HumanThird,
			WorkerFirst,
			WorkerThird
		}
	}
}