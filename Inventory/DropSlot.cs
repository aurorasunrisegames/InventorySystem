using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class DropSlot : MonoBehaviour, IDropHandler
    {
        public int i; 
        public FastInventory _fi;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                string dropName = eventData.pointerDrag.gameObject.name; 
                _fi.Drop(i, dropName); 
            }
        }
    }
}