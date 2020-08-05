using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
        [SerializeField] private Material material;
        [SerializeField] private Vector3 startPosition;

        public Material Material => material;

        public Vector3 StartPosition => startPosition;
}