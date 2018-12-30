using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

namespace System
{
    public class ItemFactory : MonoBehaviour
    {
        [Header("生成参数")]
        //是否为定点生成
        public bool IsStationary;
        //刷新时间（周期）
        public  int RefreshTime;
        //单次刷新道具数量
        public int OnceRefreshNum;

        [Header("所需预制获取")]
        /*获取生成点控制函数*****
         *****注意，定点时，数组长度为定点数量
         ********非定点时，为最大同时存在数量*******/
        
        public ItemCreator[] CreateArea;
        //获取地图函数（用于非定点生成区域获取）
        public MapController Map;
        

        private readonly Random _random = new Random();
        private int _tempCreateSerial;
        private bool _isfull;
        
        private readonly List<int> _empty = new List<int>();

        
        // Start is called before the first frame update
        void Start()
        {
            
            
            if (IsStationary)
            {
                StartCoroutine(CreateStationary());
            }
            else
            {
                StartCoroutine(CreateRandomly());
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        IEnumerator CreateStationary()
        {
            
            while (true)
            {
                string debugstring= " ";
                _empty.Clear();
                for (int i = 0; i < CreateArea.Length; i++)
                {
                    CreateArea[i].Register(this);
                    if (CreateArea[i].IsEmpty)
                    {
                        _empty.Add(i);
                        debugstring += i + " ";
                    }
                }
                Debug.Log(debugstring+"    "+_empty.Count);
                for (int i = 0; i < OnceRefreshNum; i++)
                {
                    //如果已满就不再生成（防止Do/while死循环)
                    if (_empty.Count>0)
                    {
                        //寻找空生成点
                        _tempCreateSerial = _random.Next(0, _empty.Count);
                        Debug.Log("此次生成点为"+_tempCreateSerial);
                        CreateArea[_empty[_tempCreateSerial]].CreateItem();
                        _empty.RemoveAt(_tempCreateSerial);
                        Debug.Log("剩余空闲槽位数量"+_empty.Count);
                    }
                }
                yield return new WaitForSeconds(RefreshTime);
            } 
        }

        IEnumerator CreateRandomly()
        {
            while (true)
            {
                for (int i = 0; i < CreateArea.Length; i++)
                {
                    if (CreateArea[i].IsEmpty)
                    {
                        _empty.Add(i);
                    }
                }
                for (int i = 0; i < OnceRefreshNum; i++)
                {
                    //如果已满就不再生成（防止Do/while死循环)
                    if (_empty.Count>0)
                    {
                        //寻找空生成点
                        _tempCreateSerial = _random.Next(0, _empty.Count);
                        CreateArea[_empty[_tempCreateSerial]].gameObject.transform.position = Map.GetRelievePoint(5);
                        CreateArea[_empty[_tempCreateSerial]].CreateItem();
                        _empty.RemoveAt(_tempCreateSerial);
                    }
                    else
                    {
                        _tempCreateSerial = _random.Next(0, CreateArea.Length);
                        CreateArea[_empty[_tempCreateSerial]].gameObject.transform.position = Map.GetRelievePoint(5);
                        _empty.RemoveAt(_tempCreateSerial);
                    }
                    
                    
                }
                yield return new WaitForSeconds(RefreshTime);
            }
        }

        IEnumerator BlockRandomCreateTest()
        {
            while (true)
            {
                for (int i = 0; i < CreateArea.Length; i++)
                {
                    CreateArea[i].Register(this);
                    if (CreateArea[i].IsEmpty)
                    {
                        _empty.Add(i);
                    }
                }
                for (int i = 0; i < OnceRefreshNum; i++)
                {
                    //如果已满就不再生成（防止Do/while死循环)
                    if (_empty.Count>0)
                    {
                        //寻找空生成点
                        _tempCreateSerial = _random.Next(0, _empty.Count);
                        GameObject block = Map.GetEmptyBlock();
                        CreateArea[_empty[_tempCreateSerial]].gameObject.transform.parent = block.transform;
                        CreateArea[_empty[_tempCreateSerial]].CreateItem();
                        _empty.RemoveAt(_tempCreateSerial);
                    }
                    else
                    {
                        _tempCreateSerial = _random.Next(0, CreateArea.Length);
                        GameObject block = Map.GetEmptyBlock();
                        CreateArea[_empty[_tempCreateSerial]].gameObject.transform.parent = block.transform;
                        _empty.RemoveAt(_tempCreateSerial);
                    }
                    
                    
                }
                yield return new WaitForSeconds(RefreshTime);
            }
        }
    }
}
