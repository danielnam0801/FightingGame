using UnityEngine.UIElements;

namespace SelectScene
{

    public class ModeButton
    {
        string player1 = "p1";
        string player2 = "p2";
        string computer = "cp";

        Button _modBtn;
        PlayerType _currentMod;
        PlayerType _linkedPlayer;
        public PlayerType GetCurrentMode => _currentMod;

        public ModeButton(Button button, PlayerType player)
        {
            _linkedPlayer = player;
            _currentMod = player;
            _modBtn = button;
            _modBtn.RegisterCallback<ClickEvent>(SelectMode);
            //init
            PlayerSetting(_linkedPlayer);
        }

        public void SelectMode(ClickEvent evt)
        {
            switch (_currentMod) 
            {
                case PlayerType.player1:
                    CPSetting();
                    break;
                case PlayerType.player2:
                    CPSetting();
                    break;
                case PlayerType.AI:
                    PlayerSetting(_linkedPlayer);
                    break;
            }
        }

        private void PlayerSetting(PlayerType player)
        {
            if(player == PlayerType.player1)
            {
                SetText("1p");
                RemoveAndAddClass(computer, player1);
            }
            else // p2
            {
                SetText("2p");
                RemoveAndAddClass(computer, player2);
            }
            _currentMod = player;
        }

        private void CPSetting()
        {
            _currentMod = PlayerType.AI;
            SetText("cp");

            if (_linkedPlayer == PlayerType.player1)
            {
                RemoveAndAddClass(player1, computer);
            }
            else
            {
                RemoveAndAddClass(player2, computer);
            }
        }

        public void RemoveAndAddClass(string removeclassname, string addclassname)
        {
            _modBtn.RemoveFromClassList(removeclassname);
            _modBtn.AddToClassList(addclassname);
        }

        public void SetText(string text)
        {
            _modBtn.text = text;
        }
    }
}
