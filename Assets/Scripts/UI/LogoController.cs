using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Networks;
using Photon.Pun;
using Photon.Realtime;

public class LogoController : MonoBehaviour
{
    public GameObject MatchingUI;
    public GameObject Logo;
    public GameObject TapMesh;

    public RetroAesthetics.RetroCameraEffect RetroCameraEffect;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        RetroCameraEffect.FadeIn(3);
        MatchingUI.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterGame(){
        RetroCameraEffect.FadeOut(0.5f);
        Invoke("ChangeUiToMatch",0.5f);
    }

    public void ChangeUiToMatch(){
        Logo.SetActive(false);
        TapMesh.SetActive(false);
        MatchingUI.SetActive(true);
        if(PhotonNetwork.IsConnectedAndReady){
            MatchingUI.GetComponent<MatchingNetConnect>().ConnectInit();
        }
        RetroCameraEffect.randomGlitches = false;
        RetroCameraEffect.useDisplacementWaves = false;
        RetroCameraEffect.useChromaticAberration = false;
        RetroCameraEffect.FadeIn(0.5f);
    }

    public void ChangeUiToRoom(){
        Logo.SetActive(false);
        TapMesh.SetActive(false);
        RetroCameraEffect.randomGlitches = false;
        RetroCameraEffect.useDisplacementWaves = false;
        RetroCameraEffect.useChromaticAberration = false;
        RetroCameraEffect.FadeIn(0.5f);
    }
}
