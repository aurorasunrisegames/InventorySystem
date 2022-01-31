using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class DragDrop : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        private RectTransform _rt;
        private Vector2 _startPos;
    
        void Start()
        {
            _rt = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPos = _rt.anchoredPosition;
        }
    
        public void OnDrag(PointerEventData eventData)
        {
            _rt.anchoredPosition += eventData.delta;
        }
    
        public void OnEndDrag(PointerEventData eventData)
        {
            _rt.anchoredPosition = _startPos;
        }
    }
}