using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cubes
{
    public class CreateCube : MonoBehaviour, ICreateCube
    {
        [SerializeField] private GameObject cubePrefab;
        
        
        public GameObject Generate()
        {
            var cube = Instantiate(cubePrefab);
            cube.transform.position = transform.position;
            cube.transform.localScale = GenerateSize();
            return cube;
        }

        private Vector3 GenerateSize()
        {
            return new Vector3(Random.Range(1, 6), Random.Range(1, 6), 1);
        }
    }
}