using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.UIElements;
using System;
using CustomUI;

namespace SelectScene
{
    public class SelectUI : MonoBehaviour
    {
        [SerializeField] CharacterListSO _characterListSO;
        [SerializeField] VisualTreeAsset _slotAsset;

        UIDocument _uiDocument;

        #region VisualElements

        VisualElement _LeftPanel;
        VisualElement _RightPanel;
        VisualElement _SlotPanel;
        VisualElement _ReadyPanel;
    
        Button _modBtnleft;
        Button _modBtnright;
        Button _returnBtn;

        Label _VSElement;
        Label _timeElement;

        #endregion

        List<Slot> _characters = new List<Slot>();
        //List<MapSlot> _maps = new List<MapSlot>();

        Selector player1;
        Selector player2;
        TimeLabel timeLabel;
        ModeButton modeBtn1p;
        ModeButton modeBtn2p;

        bool isSelectAll = false;
    
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            var root = _uiDocument.rootVisualElement;

            #region Elements
            _LeftPanel = root.Q<VisualElement>("Middle--left");
            _RightPanel = root.Q<VisualElement>("Middle--right");
            _SlotPanel = root.Q<VisualElement>("SlotPanel");
            _ReadyPanel = root.Q<VisualElement>("ReadyPanel");
            _VSElement = root.Q<Label>("VSLabel");
            _timeElement = root.Q<Label>("TimeLabel");
            _returnBtn = root.Q<Button>("ReturnBtn");
            _modBtnleft = root.Q<Button>("1pBtn");
            _modBtnright = root.Q<Button>("2pBtn");
            #endregion

            MakeCharacterSlot();

            #region CreateSelector
            InputKey p1 = new InputKey();
            p1.upKey = KeyCode.W;
            p1.leftKey = KeyCode.A;
            p1.downKey = KeyCode.S;
            p1.rightKey = KeyCode.D;
            p1.selectKey = KeyCode.Space;
        
            InputKey p2 = new InputKey();
            p2.upKey = KeyCode.UpArrow;
            p2.leftKey = KeyCode.LeftArrow;
            p2.downKey = KeyCode.DownArrow;
            p2.rightKey = KeyCode.RightArrow;
            p2.selectKey = KeyCode.Return;

            player1 = new Selector(PlayerType.player1, p1, _characters);
            player2 = new Selector(PlayerType.player2, p2, _characters);
            #endregion
            //Slot 클릭이벤트 구현해야함  

            timeLabel = new TimeLabel(this, _timeElement, 90f);
            modeBtn1p = new ModeButton(_modBtnleft, PlayerType.player1);
            modeBtn2p = new ModeButton(_modBtnright, PlayerType.player2);

            _returnBtn.RegisterCallback<ClickEvent>((evt) =>
            {
                GoIntro();
            });
    
        }

        private void Update()
        {
            if(player1.isSelect && player2.isSelect)
            {
                isSelectAll = true;
                _ReadyPanel.RemoveFromClassList("off");
            }
            else
            {
                isSelectAll = false;
                _ReadyPanel.AddToClassList("off");
            }

            if(isSelectAll)
            {
                if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                {
                    GoToGameScene();
                }
            }

            player1?.Update();
            player2?.Update();
            timeLabel.Update();
        }

        private void MakeCharacterSlot()
        {
            List<Character> list = _characterListSO.GetCharacterList();
        
            for (int i = 0; i < list.Count; i++)
            {
                VisualElement element = _slotAsset.Instantiate().Q("Slot");
                Slot slot;
                if(i == list.Count - 1) //항상 마지막은 랜덤 슬롯
                {
                    Sprite image = list[i].GetSprite();
                    element.style.backgroundImage = SetImage(image);
                    slot = new RandomSlot(this, element, SetImage(image), i);
                }
                else
                {
                    RenderTexture image = list[i].GetRenderTexture();
                    element.style.backgroundImage = SetImage(image);
                    slot = new CharacterSlot(this, element, SetImage(image), i);
                }
                _characters.Add(slot);
                _SlotPanel.Add(element);
            }
        }

        private StyleBackground SetImage(RenderTexture texture)
        {
            return new StyleBackground(Background.FromRenderTexture(texture));
        }

        private StyleBackground SetImage(Sprite sprite)
        {
            return new StyleBackground(Background.FromSprite(sprite));
        }

        public void SetLeftPanelImage(Slot slot)
        {
            _LeftPanel.style.backgroundImage = slot.Image;
        }

        public void SetRightPanelImage(Slot slot)
        {
            _RightPanel.style.backgroundImage = slot.Image;
        }

        private void GoToGameScene()
        {
            //GameScene으로 이동
            //현재 선택된 정보들을 전부 넘겨줘야함
            // Player1,2 캐릭터, AI 여부
            // 랜덤 맵

            int p1idx;
            int p2idx;

            /// 
            /// 랜덤슬롯인지 확인해주는 곳
            ///
            if(_characters[player1.GetCurSlotIdx] as RandomSlot != null)
                p1idx = UnityEngine.Random.Range(0, _characters.Count - 2); // 맨 마지막 idx는 randomslot이기에 빼줌
            else
                p1idx = player1.GetCurSlotIdx;
            if(_characters[player2.GetCurSlotIdx] as RandomSlot != null)
                p2idx = UnityEngine.Random.Range(0, _characters.Count - 2); // 맨 마지막 idx는 randomslot이기에 빼줌
            else
                p2idx = player2.GetCurSlotIdx;
            ///
            ///
            ///

            PlayerInfo p1 = new PlayerInfo
            {
                character = _characterListSO.List[p1idx],
                mode = modeBtn1p.GetCurrentMode,
                Path = Path.Player1Path
            };
            PlayerInfo p2 = new PlayerInfo
            {
                character = _characterListSO.List[p2idx],
                mode = modeBtn2p.GetCurrentMode,
                Path = Path.Player2Path
            };

            DataManager<PlayerInfo>.SaveData(p1);
            DataManager<PlayerInfo>.SaveData(p2);
            timeLabel.Stop = true;

            SceneManager.Instance.LoadGameScene();

            Debug.Log(DataManager<PlayerInfo>.LoadData(Path.Player1Path).character.material);
            Debug.Log(DataManager<PlayerInfo>.LoadData(Path.Player2Path).character.material);
        }

        public void GoIntro()
        {
            SceneManager.Instance.LoadIntroScene();
            Debug.Log("GoIntro");
        }
    }
}
