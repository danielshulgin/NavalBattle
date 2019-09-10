using UnityEngine;
using NavalBattle;

public class GameController : MonoBehaviour
{
    public CanvasGroup gameOverCanvasGroup;
    public BattlefieldConroller battlefieldConroller;
    public BuildController buildController;
    public Settings settings;
    public Player firstPlayer;
    public Player secondPlayer;
    //TODO Player

    public Mode mode { get; private set; }

    private void Start()
    {
        battlefieldConroller.settings = settings;
        buildController.settings = settings;
        firstPlayer = new Player();
        secondPlayer = new Player();
        mode = Mode.FirstPlayerBuild;
    }

    public void EndOfTurn()
    {
        switch (mode)
        {
            case Mode.FirstPlayerBuild:
                EndFirstPlayerBuild();
                break;
            case Mode.SecondPlayerBuild:
                EndBuildMode();
                mode = Mode.FirstPlayerTurn;
                break;
            case Mode.FirstPlayerTurn:
                mode = Mode.SecondPlayerTurn;
                battlefieldConroller.UpdateVizual(Mode.SecondPlayerTurn);
                battlefieldConroller.fired = false;
                break;
            case Mode.SecondPlayerTurn:
                mode = Mode.FirstPlayerTurn;
                battlefieldConroller.UpdateVizual(Mode.FirstPlayerTurn);
                battlefieldConroller.fired = false;
                break;
        }
    }

    private void EndFirstPlayerBuild()
    {
        battlefieldConroller.firstMap = buildController.map;
        buildController.ResetMap();
        mode = Mode.SecondPlayerBuild;
    }

    private void EndBuildMode()
    {
        battlefieldConroller.secondMap = buildController.map;
        buildController.DeleteMap();
        battlefieldConroller.InstantiateBattleField();
        battlefieldConroller.firstMap.GameOver += SubscribeOnGameOver;
        battlefieldConroller.secondMap.GameOver += SubscribeOnGameOver;
    }

    public void SubscribeOnGameOver(Player player)
    {
        gameOverCanvasGroup.alpha = 1f;
        gameOverCanvasGroup.blocksRaycasts = true;

    }

    public void Restart()
    {
        mode = Mode.FirstPlayerBuild;
        battlefieldConroller.DeleteBattleField();
        buildController.Start();
        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.blocksRaycasts = false;
    }
}
