
using UnityEngine.Assertions;


public enum HackGame {
    round_ball,
    puzzle,
    monkey,
    globe,
    disk
}

static class GamesMethodsExtensions {
    public static string prefabPath(this HackGame gameType) {
        var result = "Games/";
        switch (gameType) {            
            case HackGame.round_ball:
                result += "RBGame";
                break;
            case HackGame.puzzle:
                result += "PuzzleGame";
                break;
            case HackGame.monkey:
                result += "MonkeyGame";
                break;
            case HackGame.globe:
                result += "GlobeGame";
                break;
            case HackGame.disk:
                result += "DiskGame";
                break;            
            default:
                Assert.IsTrue(false, "prefab path for hack game '" + gameType.ToString() + "' is not defined");
                break;
        }

        return result;
    }
}
