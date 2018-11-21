using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public enum MapSize{S,L};
    public MapSize Mapsize;
    public Collider RelieveArea;
    public Material GridMaterial;
    private  int _maxpointdata;
    // Start is called before the first frame update
    void Awake()
    {
        if(Mapsize.Equals(MapSize.S)){
            _maxpointdata = 250;
        }else{
            _maxpointdata = 500;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetRelievePoint(int radius = 1){
        //新建复活点
        Vector3 relievepoint;
        //新建碰撞检测数组
        Collider[] results;
        
        //检测复活点及周围有没有碰撞盒，要求复活点只和复活区域RelieveArea这一个碰撞盒接触（也就是不接触任何碰撞盒而且在复活区域内）
        do{
            relievepoint  = new Vector3(Random.Range(-_maxpointdata,_maxpointdata),0,Random.Range(-_maxpointdata,_maxpointdata));
            results = Physics.OverlapSphere(relievepoint,radius);
        }while(!CheckArray(results));
        
        return relievepoint;
     
    }

    //检测是否在出生区域
    private bool CheckArray(Collider[] colliderarray){
        if(colliderarray.Length==1){
            if(colliderarray[0].name.Equals(RelieveArea.name)){
                return true;
            }else{
                print("not in relievearea and ****" + colliderarray[0]);
                return false;
            }
        }else{
            return false;
        }
    }

    public void GridEffect(Vector3 position, int type)
    {
        GridMaterial.SetVector("_Center",position);
        GridMaterial.SetFloat("_Open",type);
    }
    
    
}
