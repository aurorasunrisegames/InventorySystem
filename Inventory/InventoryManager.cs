using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
     public class InventoryManager : MonoBehaviour
     {
          public FastInventory FastInventory { get; set; }
          public ItemsListSO CollectionList => m_collection;
          [SerializeField] private ItemsListSO m_collection;
          
          [SerializeField] private AudioClip m_isFullSound;
          [SerializeField] private GameObject m_isFullLabel;
          [SerializeField] private Sprite m_emptySprite;
          [SerializeField] private RectTransform m_slotPrefab;
          [SerializeField] private RectTransform m_slotsContainer;
          [Range(0.7f, 1f)][SerializeField] private float m_spacingMultiplier = 0.9f;
          [SerializeField] private AudioSource m_audioSource;
     
          private Vector2 _slotSize; 
          private Vector2 _sizeDelta;
     
          private LinkedList<string> ll_itemsNames      = new LinkedList<string>(); 
          private Dictionary<string, int> d_itemsAmount = new Dictionary<string, int>();
          private List<Image> l_slotImages              = new List<Image>();
          private List<Text> l_slotAmountText           = new List<Text>();
          private List<Button> l_slotButtons            = new List<Button>();

          public Dictionary<string, int> ItemsAmount => d_itemsAmount;
          
          private void Start()
          { 
               if (m_audioSource == null)
               {
                    m_audioSource = gameObject.AddComponent<AudioSource>();
                    m_audioSource.playOnAwake = false;
                    m_audioSource.loop = false;
               }
               
               var rect = m_slotsContainer.rect;
               _sizeDelta = new Vector2(rect.width, rect.height);
               _slotSize = new Vector2(_sizeDelta.x / 6, _sizeDelta.y/2);
               m_slotPrefab.sizeDelta = _slotSize;
               CreateSlots(); 
          }
     
          private void CreateSlots()
          {
               Vector2 startPoint = m_slotsContainer.rect.center + _slotSize/2;
               for (int y = 0; y < 2; y++)
               {
                    for (int x = 0; x < 6; x++)
                    {
                         RectTransform rt = Instantiate(m_slotPrefab, m_slotPrefab.parent);
                         rt.anchoredPosition = startPoint + new Vector2(x * _slotSize.x, y * _slotSize.y);
                         rt.localScale *= m_spacingMultiplier;
                         l_slotButtons.Add(rt.GetComponent<Button>() );
                         l_slotImages.Add(rt.transform.GetChild(0).GetComponent<Image>() );
                         l_slotAmountText.Add(rt.GetComponentInChildren<Text>() );
                    }
               }
               
               m_slotPrefab.gameObject.SetActive(false);
          }
     
          public void DisplayVisual()
          {
               for (int p = 6; p < 12; p++)
               {
                    l_slotButtons[p].gameObject.SetActive(ll_itemsNames.Count >= 6);
               }
 
               int i = 0;
               foreach (var itemName in ll_itemsNames)
               {
                    int index = m_collection.keys.IndexOf(itemName);
                    l_slotImages[i].sprite = m_collection.values[index].uiImage;
                    l_slotAmountText[i].text = d_itemsAmount[itemName].ToString();
                    l_slotButtons[i].onClick.RemoveAllListeners();
                    l_slotButtons[i].onClick.AddListener(() => UseItem(itemName));
                    l_slotButtons[i].name = "Slot_" + itemName;
                    i++;
               }
          
               if (i < l_slotButtons.Count)
               {
                    for (int other = i; other < l_slotButtons.Count; other++)
                    {
                         l_slotButtons[other].name = "Empty button";
                         l_slotButtons[other].onClick.RemoveAllListeners();
                         l_slotImages[other].sprite = m_emptySprite;
                         l_slotAmountText[other].text = "";
                    }
               }
          }
  
          public bool AddItem(string itemName)
          {
               if (m_collection.keys.Contains(itemName))
               {
                    if (ll_itemsNames.Count >= 12)
                    {
                         StartCoroutine(IsFullCoroutine());
                         return false;
                    }
                    
                    if (!ll_itemsNames.Contains(itemName))
                    {
                         ll_itemsNames.AddLast(itemName);
                         d_itemsAmount.Add(itemName,0);
                    }
                    
                    d_itemsAmount[itemName]++;
                    DisplayVisual();
                    int index = m_collection.keys.IndexOf(itemName);
                    m_audioSource.clip = m_collection.values[index].pickUpSound;
                    m_audioSource.Play();
                    return true;
               }
               return false;
          }

          public void UseItem(string itemName)
          {
               if (m_collection.keys.Contains(itemName))
               {
                    if (ll_itemsNames.Contains(itemName))
                    {
                         d_itemsAmount[itemName]--;
                         if (d_itemsAmount[itemName] <= 0)
                         {  
                              ll_itemsNames.Remove(itemName);
                              d_itemsAmount.Remove(itemName);
                         }
                    }
                    
                    DisplayVisual();
                    int index = m_collection.keys.IndexOf(itemName);
                    m_audioSource.clip = m_collection.values[index].useSound;
                    m_audioSource.Play();
               }
          }
     
          private IEnumerator IsFullCoroutine()
          {
               m_isFullLabel.SetActive(true);
               m_audioSource.clip = m_isFullSound;
               m_audioSource.Play();
               yield return new WaitForSeconds(1);
               m_isFullLabel.SetActive(false);
               yield return new WaitForSeconds(1);
          }
     }
}