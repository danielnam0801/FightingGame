using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] GameObject aiModel;
    [SerializeField] GameObject playerModel;
    [SerializeField] Transform p1SpawnPoint;
    [SerializeField] Transform p2SpawnPoint;

    PlayerInfo player1Info;
    PlayerInfo player2Info;

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        player1Info = DataManager<PlayerInfo>.LoadData(Core.Path.Player1Path);
        player2Info = DataManager<PlayerInfo>.LoadData(Core.Path.Player2Path);

        SetPlayer();
    }

    private void SetPlayer()
    {
        GameObject p1 = (player1Info.mode == PlayerType.AI)
            ? Instantiate(aiModel, p1SpawnPoint.position, Quaternion.Euler(0, 90, 0))
            : Instantiate(playerModel, p1SpawnPoint.position, Quaternion.Euler(0, 90, 0));

        GameObject p2 = (player2Info.mode == PlayerType.AI)
            ? Instantiate(aiModel, p2SpawnPoint.position, Quaternion.Euler(0, -90, 0))
            : Instantiate(playerModel, p2SpawnPoint.position, Quaternion.Euler(0, -90, 0));

        p1.GetComponent<PlayerSetting>().SetVisual(player1Info.character.material);
        p2.GetComponent<PlayerSetting>().SetVisual(player2Info.character.material);

    }

    private void OnDisable()
    {
        player1Info = null;
        player2Info = null;
    }
}
