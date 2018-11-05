using Account;
using GamePlayer;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace System
{
    public class GameSettings : MonoBehaviour
    {

        public Hashtable      PlayerProperty = new Hashtable();
        
        [Header("PlayerSetting")]
        public CharacterType  Chosentype;
        public bool IsReady;
        

        [Header("RoomSetting")] 
        private GameParameters _gameParameters;
        public Dropdown MapSelector;
        public Dropdown ModeSelector;
        public Dropdown PlayerSelector;
        public Dropdown TargetSelector;
    
        

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            SystemSettings();
            PlayerSettingInitial();
        }

        private void SystemSettings()
        {
            //设置屏幕正方向在Home键右边
            Screen.orientation = ScreenOrientation.LandscapeRight;
        
            //设置屏幕自动旋转， 并置支持的方向
            Screen.orientation                    = ScreenOrientation.AutoRotation;                
            Screen.autorotateToLandscapeLeft      = true;                
            Screen.autorotateToLandscapeRight     = true;                
            Screen.autorotateToPortrait           = false;
            Screen.autorotateToPortraitUpsideDown = false;

        }

        public void PlayerSettingInitial()
        {
            SetReady(false);
            SetCharacter(CharacterType.Circle);
            SetNickName();
        }

        public void SetReady(bool isReady)
        {
            IsReady = isReady;
            if (PlayerProperty.ContainsKey("IsReady"))
            {
                PlayerProperty["IsReady"] = IsReady;
            }
            else
            {
                PlayerProperty.Add("IsReady", IsReady);
            }
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperty);
        }

        public void SetCharacter(CharacterType type)
        {
            Chosentype = type;
            if (PlayerProperty.ContainsKey("Character"))
            {
                PlayerProperty["Character"] = Chosentype;
            }
            else
            {
                PlayerProperty.Add("Character", Chosentype);
            }
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperty);
        }

        public void SetNickName()
        {
            PhotonNetwork.LocalPlayer.NickName = AccountInfo.Nickname ??("游客" +PhotonNetwork.LocalPlayer.ActorNumber); //如果有昵称则为昵称，如果没有昵称则为游客+房间内序号
        }

        public void SetRoomSetting()
        {
            _gameParameters.MapSerial = MapSelector.value;
            _gameParameters.ModeSerial = ModeSelector.value;
            _gameParameters.MaxPlayer = PlayerSelector.value;
            _gameParameters.TargetKilling = TargetSelector.value;
        }

        public GameParameters GetRoomSetting()
        {
            return _gameParameters;
        }
        
    }
}
