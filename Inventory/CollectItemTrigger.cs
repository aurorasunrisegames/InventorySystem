using UnityEngine;

namespace Inventory
{
    public class CollectItemTrigger : MonoBehaviour
    {
        [SerializeField] private InventoryManager m_inventoryManager;
    
        void Start()
        {
            if (m_inventoryManager == null)
                m_inventoryManager = FindObjectOfType<InventoryManager>();
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Collectable"))
            {
                if (m_inventoryManager.AddItem(other.name))
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}