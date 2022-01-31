using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class FastInventory : MonoBehaviour
    {
        [SerializeField] private string[] FastEquipment = new string[3+1];
        [SerializeField] private KeyCode[] _keyCodesFast = new KeyCode[3+1]
            {KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3};
        [SerializeField] private GameObject m_fastSlotPrefab;
        [SerializeField] public InventoryManager m_inventoryManager;
     
        private GameObject[] _fastSlots;
        private Image[] _previewImages;
        private Text[] _amountTexts;
        private Sprite _defaultSprite;

        private void Start()
        {
            if (m_inventoryManager == null)
                m_inventoryManager = FindObjectOfType<InventoryManager>();
            m_inventoryManager.FastInventory = this;
            CreateSlots();
            DisplayVisual(FastEquipment[0], 0);
        }

        private void CreateSlots()
        {
            FastEquipment = new string[_keyCodesFast.Length];
            int n = FastEquipment.Length;
            _fastSlots = new GameObject[n];
            _previewImages = new Image[n];
            _amountTexts = new Text[n];

            _fastSlots[0] = m_fastSlotPrefab;
            for (int i = 1; i < n; i++)
                _fastSlots[i] = Instantiate(m_fastSlotPrefab, transform);
       
            for (int i = 0; i < n; i++)
            {
                _previewImages[i] = _fastSlots[i].transform.GetChild(0).GetComponent<Image>();
                _amountTexts[i] = _fastSlots[i].GetComponentInChildren<Text>();
                _fastSlots[i].GetComponent<DropSlot>().i = i;
                _fastSlots[i].GetComponent<DropSlot>()._fi = this;
            }
            
            _defaultSprite = _previewImages[0].sprite;          
        }
    
        private void Update()
        {
            for (var i = 0; i < _keyCodesFast.Length; i++)
            {
                if (Input.GetKeyDown(_keyCodesFast[i]))
                {
                    m_inventoryManager.UseItem(FastEquipment[i]);
                    DisplayVisual(FastEquipment[i], i);
                }
            }
        }
    
        internal void Drop(int p, string dropName)
        {
            if (dropName.Substring(0, 5) != "Slot_") return;
            string itemName = dropName.Substring(5, dropName.Length - 5);
            FastEquipment[p] = itemName;
            DisplayVisual(FastEquipment[p], p);
        }

        private void DisplayVisual(string itemName, int p)
        {
            if(itemName == null) return;
            int key = m_inventoryManager.CollectionList.keys.IndexOf(itemName);
         
            if ( !m_inventoryManager.ItemsAmount.ContainsKey(itemName)
                 || m_inventoryManager.ItemsAmount[itemName] <= 0)
            {
                _previewImages[p].sprite = _defaultSprite;
                _amountTexts[p].text = "Empty"; 
                FastEquipment[p] = "";
            }
            else
            {
                _previewImages[p].sprite = m_inventoryManager.CollectionList.values[key].uiImage;
                _amountTexts[p].text = m_inventoryManager.ItemsAmount[itemName].ToString();
            }
        }
    }
}