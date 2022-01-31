using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private ItemsListSO m_collection;
        [SerializeField] private int m_count;
        [SerializeField] private float m_delaySec = 0.1f;
        
        void Start()
        {
            StartCoroutine(DelaySpawn());
            ItemsListSO col;
            try
            {
                col = FindObjectOfType<InventoryManager>().CollectionList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            if (col != null)
                m_collection = col;
        } 

        private void Spawn()
        {
            int r = Random.Range(0, m_collection.values.Count);
            GameObject g = Instantiate(m_collection.values[r].prefab);
            g.name = m_collection.values[r].itemName;
            g.tag = "Collectable";
            g.transform.position = new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5));
        }

        private IEnumerator DelaySpawn()
        {
            for (int i = 0; i < m_count; i++)
            {
                Spawn();
                yield return new WaitForSeconds(m_delaySec);
            }
            yield return null;
        }
    }
}