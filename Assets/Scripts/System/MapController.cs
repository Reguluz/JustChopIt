using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    public enum MapSize{S,L};
    public MapSize Mapsize;
    public Collider RelieveArea;
    public Material GridMaterial;
    private  int _maxpointdata;
    
    public GameObject TerrainAssemble;

    public GameObject _effectCenter;
    private bool _effectOpen = false;
    
    private GameObject[] _blockStatuses;
    

    // Start is called before the first frame update
    private void Start()
    {
        /*Transform[] _blocks = TerrainAssemble.transform.GetComponentsInChildren<Transform>();
        _blockStatuses = new BlockInfo[_blocks.Length];
        for (int i = 0; i < _blocks.Length; i++)
        {
            _blockStatuses[i].Block = _blocks[i].gameObject;
            _blockStatuses[i].isEmpty = true;
        }*/
    }

    void Awake()
    {
        if(Mapsize.Equals(MapSize.S)){
            _maxpointdata = 250;
        }else{
            _maxpointdata = 500;
        }

        EffectSwitch(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_effectOpen)
        {
            GridMaterial.SetVector("_Center",_effectCenter.transform.position);
        }
    }

    public Vector3 GetRelievePoint(int radius = 1){
        //新建复活点
        Vector3 relievepoint;
        //新建碰撞检测数组
        Collider[] results;
        int overtime = 0;//随机上限
        //检测复活点及周围有没有碰撞盒，要求复活点只和复活区域RelieveArea这一个碰撞盒接触（也就是不接触任何碰撞盒而且在复活区域内）
        do
        {
            overtime++;
            relievepoint  = new Vector3(Random.Range(-_maxpointdata,_maxpointdata),0,Random.Range(-_maxpointdata,_maxpointdata));
            results = Physics.OverlapSphere(relievepoint,radius);
            Debug.Log(results.Length);
            if (overtime > 10000)
            {
                Debug.Log("没有符合条件的复活点");
                return Vector3.zero;
            }
        }while(!CheckArray(results));
        
        return relievepoint;
     
    }

    public GameObject GetEmptyBlock()
    {
        int next;
        do
        {
            next = Random.Range(0, _blockStatuses.Length);
        } while (_blockStatuses[next].transform.childCount!=0);
        return _blockStatuses[next];
    }

    //检测是否在出生区域
    private bool CheckArray(Collider[] colliderarray){
        if (colliderarray.Length == 0)
        {
            return false;
        }
        foreach (Collider collider in colliderarray)
        {
            if (!collider.name.Equals(RelieveArea.name))
            {
                return false;
            }
        }
        return true;

        /*
        if(colliderarray.Length==1){
            if(colliderarray[0].name.Equals(RelieveArea.name)){
                return true;
            }else{
                print("not in relievearea and ****" + colliderarray[0]);
                return false;
            }
        }else{
            return false;
        }*/
    }

    public void SetEffectCenter(GameObject obj)
    {
        _effectCenter = obj;
    }

    public void EffectSwitch(bool isOpen)
    {
        if (GridMaterial!=null)
        {
            GridMaterial.SetFloat("_Open",isOpen?1:0);
            _effectOpen = isOpen;
        }
        
    }
    
    
}
