using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
   [CreateAssetMenu(fileName = "0_ItemList", menuName = "Collectable/ItemList")]
   public class ItemsListSO : ScriptableObject
   {
      public List<string> keys = new List<string>();
      public List<ItemSO> values = new List<ItemSO>();

      public void Refresh()
      {
         if (values.Count > 0)
         {
            foreach (var value in values)
            {
               if (value == null)
               {
                  values.Remove(value);
                  Refresh();
                  break;
               }
            }
            
            keys.Clear();
            foreach (var value in values)
            {
               keys.Add(value.itemName);
            }
         }
      }
   }
}