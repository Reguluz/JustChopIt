using UnityEngine;

namespace Items
{
    public class Items:MonoBehaviour
    {
        private ItemCreator Owner;

        public void SetOwner(ItemCreator owner)
        {
            Owner = owner;
        }

        public void SetEmpty()
        {
            Owner.IsEmpty = true;
        }
        
        
    }
}