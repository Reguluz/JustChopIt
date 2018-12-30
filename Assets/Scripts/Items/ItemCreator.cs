using System;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

namespace Items
{
    public class ItemCreator : MonoBehaviour
    {
        //public BlockInfo ParentPosition;
        public ItemFactory Owner;
        public bool IsEmpty;

        public GameObject[] Itemlists;
        
        private readonly Random _random = new Random();
        private int _temp;
        // Start is called before the first frame update

        public void CreateItem(Vector3 position = default(Vector3))
        {
            /*if (true)
            {
                transform.position = ParentPosition.Block.transform.position;
            }*/
            IsEmpty = false;
            if (position == default(Vector3))
            {
                position = transform.position;
            }
            _temp = _random.Next(0, Itemlists.Length);
            if (PhotonNetwork.IsMasterClient)
            {
                GameObject newItem = PhotonNetwork.InstantiateSceneObject(Itemlists[_temp].name,
                    new Vector3(position.x, position.y, position.z), Quaternion.identity);
                newItem.GetComponent<Items>().SetOwner(this);
            }
            
        }

        public void SetEmpty()
        {
            IsEmpty = true;
        }

        public void Register(ItemFactory owner)
        {
            Owner = owner;
        }

        private void AddEmpty()
        {
            
        }
    }
}
