using System;
using cakeslice;
using UnityEngine;

namespace Cubes
{
    public class FreeSpotBehaviour : MonoBehaviour
    {
        private Outline _outline;
        private event Action<Vector3> OnClick ;
        
        public void Initialize()
        {
            _outline = GetComponent<Outline>();
            _outline.enabled = false;
        }

        public void SubscribeToClick(Action<Vector3> onClick)
        {
            OnClick += onClick;
        }

        public void UnsubscribeFromClick(Action<Vector3> onClick)
        {
            OnClick -= onClick;
        }

        private void OnMouseEnter()
        {
            _outline.enabled = true;
        }

        private void OnMouseExit()
        {
            _outline.enabled = false;
        }

        private void OnMouseDown()
        {
            OnClick?.Invoke(gameObject.transform.position);
        }
    }
}
