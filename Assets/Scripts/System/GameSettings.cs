using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GamePlayer;
using Photon.Pun;
using UnityEngine;

public class GameSettings : MonoBehaviour
{

    public CharacterType ChosenType;

    public GameParameters GameParameters;

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
        Screen.orientation = ScreenOrientation.AutoRotation;                
        Screen.autorotateToLandscapeLeft = true;                
        Screen.autorotateToLandscapeRight = true;                
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

    }

    private void PlayerSettingInitial()
    {
        ChosenType = CharacterType.Circle;
    }

    
}
