using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class VersionShow : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        text.text = PhotonNetwork.GameVersion;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
