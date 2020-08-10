using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cubes
{
    public class CreateFreeSpot: MonoBehaviour
    {
        [SerializeField] private GameObject freeSpotPrefab;
        [SerializeField] private Field field;

        public void Generate(GameObject currentCube, GameObject lastCube)
        {
            var lastCubeSize = lastCube.transform.localScale;
            var currentCubePos = currentCube.transform.position;
            var halfCubeSize = currentCube.transform.localScale / 2;
            var cubeBorders = new CubeBorders(
                new Vector2(currentCubePos.x - halfCubeSize.x, currentCubePos.y - halfCubeSize.y),
                new Vector2(currentCubePos.x + halfCubeSize.x, currentCubePos.y + halfCubeSize.y), 
                lastCubeSize);
            //Horizontal
            CreateSide(
                cubeBorders,
                new IterationVectors(
                   (Vector2)currentCubePos + new Vector2( lastCubeSize.x / 2 - halfCubeSize.x, lastCubeSize.y / 2 + halfCubeSize.y),
                   new Vector2(0, -halfCubeSize.y * 2 - lastCubeSize.y),
                   new Vector2(lastCubeSize.x, 0f)));
            CreateSide(
                cubeBorders,
                new IterationVectors(
                    (Vector2)currentCubePos + new Vector2( -(lastCubeSize.x / 2 - halfCubeSize.x), lastCubeSize.y / 2 + halfCubeSize.y),
                    new Vector2(0, -halfCubeSize.y * 2 - lastCubeSize.y), 
                    new Vector2(-lastCubeSize.x, 0f)));
            //Vertical
            CreateSide(
                cubeBorders,
                new IterationVectors(
                    (Vector2)currentCubePos + new Vector2( -(lastCubeSize.x / 2 + halfCubeSize.x), lastCubeSize.y / 2 - halfCubeSize.y),
                new Vector2(halfCubeSize.x * 2 + lastCubeSize.x, 0f),
                            new Vector2(0f, lastCubeSize.y)));
            CreateSide(
                cubeBorders,
                new IterationVectors(
                    (Vector2)currentCubePos + new Vector2( -(lastCubeSize.x / 2 + halfCubeSize.x), -(lastCubeSize.y / 2 - halfCubeSize.y)),
                    new Vector2(halfCubeSize.x * 2 + lastCubeSize.x, 0f),
                    new Vector2(0f, -lastCubeSize.y)));
        }

        private void CreateSide(CubeBorders borders, IterationVectors vectors)
        {
            var startPosition = vectors.StartPos;
            do
            {
                CreateSpot(startPosition, borders.LastCubeSize);
                var invertStartPos = startPosition + vectors.AddForInvert;
                CreateSpot(invertStartPos, borders.LastCubeSize);
                startPosition += vectors.IterateVector;
            } while (borders.CheckBorders(startPosition));
        }

        private void CreateSpot(Vector3 position, Vector3 size)
        {
            if (!CheckOverlap(position, size) || !FsPool.CheckSpot(position)) return;
            GameObject freeSpot = FsPool.ActivateFreeSpot();
            if( freeSpot == null)
                freeSpot = Instantiate(freeSpotPrefab);
            freeSpot.SetPlace(position);
            freeSpot.transform.localScale = size;
            freeSpot.GetComponent<FreeSpotBehaviour>().Initialize();
            FsPool.AddFreeSpot(freeSpot);
        }
        
        private bool CheckOverlap(Vector3 position, Vector3 size)
        {
            var results = Physics2D.OverlapBoxNonAlloc(position, size, 0, new Collider2D[1], 
                LayerMask.GetMask("CubeLayer"));
            return results <= 0 && field.CheckFieldBorders(position, size);
        }

        private class IterationVectors
        {
            public Vector2 StartPos { get; }
            public Vector2 AddForInvert { get; }
            public Vector2 IterateVector { get; }

            public IterationVectors(Vector2 startPos, Vector2 addForInvert, Vector2 iterateVector)
            {
                StartPos = startPos;
                AddForInvert = addForInvert;
                IterateVector = iterateVector;
            }
        }
    }
}