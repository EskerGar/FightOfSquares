using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
        [SerializeField] private Material material;
        
        public Vector3 StartPos { get; private set; }
        public int SideCoef { get; private set; }
        public bool IsUpPlace { get; private set; }

        public void SetSettings(Vector3 startPos, int sideCoef, bool isUpPlace)
        {
                StartPos = startPos;
                SideCoef = sideCoef;
                IsUpPlace = isUpPlace;
        }

        public Material Material => material;
        
}