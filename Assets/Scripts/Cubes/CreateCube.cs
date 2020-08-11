using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cubes
{
    public class CreateCube : MonoBehaviour
    {
        [SerializeField] private GameObject cubePrefab;
        
        
        public GameObject Generate()
        {
            var cube = Instantiate(cubePrefab);
            cube.transform.position = transform.position;
            cube.transform.localScale = GenerateSize(cube);
            return cube;
        }

        public Vector3 GenerateSize(GameObject cube)
        {
            var size =  new Vector3(Random.Range(1, 6), Random.Range(1, 6), 1);
            cube.GetComponentInChildren<TextMesh>().text = ((int)(size.x * size.y)).ToString();
            return size;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position, new Vector3(6f, 6f, 1f));
        }
    }

}