using System.Collections;
using System.Collections.Generic;
using GamePlayer;
using Photon.Pun;
using UnityEngine;

public class MonsterSystem : MonoBehaviour
{
    
    public int Level = 1;
    public MapController MapController;
    public GameObject Monster;
    
    public GameObject _monsterAlive;
    private bool isAlive =false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMonster()
    {
        

        Vector3 createPos = MapController.GetRelievePoint(15);
        Debug.Log(createPos);
        GameObject monsterAlive = PhotonNetwork.InstantiateSceneObject(Monster.name,createPos
            , Quaternion.identity);
        //monsterAlive.GetComponent<Monster>().Level = Level;
        
        
    }

    
}
