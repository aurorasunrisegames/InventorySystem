using UnityEngine;

namespace Inventory
{
   [CreateAssetMenu(fileName = "new Item", menuName = "Collectable/Item")]
   public class ItemSO : ScriptableObject
   {
      public Sprite uiImage;
      public string description;
      public GameObject prefab;
      public string itemName = "";
      public AudioClip pickUpSound, useSound;
   }
}
