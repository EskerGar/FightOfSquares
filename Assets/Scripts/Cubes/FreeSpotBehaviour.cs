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

        public void ActivateOutline()
        {
            _outline.enabled = true;
        }

        public void DeActivateOutline()
        {
            _outline.enabled = false;
        }
        
        public void OnTouch()
        {
            _outline.enabled = false;
            OnClick?.Invoke(gameObject.transform.position);
        }
        
    }
}
