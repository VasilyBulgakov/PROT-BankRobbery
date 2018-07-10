using UnityEngine;


namespace GamesManagement
{
	class GamesManager : MonoBehaviour
	{
		public enum GameNames
		{
			GlobeGame,
			PuzzleGame,
			Rotation,
			RBGame,
			FAGame
		}
		public static string GAMES_PATH = "Games/";	

		private void Awake() {
			if (FindObjectsOfType<GamesManager>().Length > 1) {
					Destroy(gameObject);
			}
		}
		
		public void launchPuzzle(Transform point)
		{
			Debug.Log("Loading at " + point.position);

			loadGame("PuzzleGame", new GameState(point));
		}

		public void launchDisk(Transform point)
		{
			Debug.Log("Loading at " + point.position);

			loadGame("DiskGame", new GameState(point));
		}


		public static GameObject loadGame(string name, GameState state)
		{
			GameObject prefab = Resources.Load( GAMES_PATH + name ) as GameObject;
			if(prefab != null)
			{
				GameObject obj = GameObject.Instantiate(prefab);
				obj.name = name;
				obj.GetComponent<GameScene>().SetState(state);
			}			
			return null;
		}
		public static void unloadGame(string name){
			GameObject.Destroy(GameObject.Find(name));
		}
	}
}