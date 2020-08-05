using UnityEngine;

namespace Cubes
{
    public class CubeBorders
    {
        private readonly Vector2 _minBorder;
        private readonly Vector2 _maxBorder;
        public Vector3 LastCubeSize { get; }

        public CubeBorders(Vector2 minBorder, Vector2 maxBorder, Vector3 lastCubeSize)
        {
            _minBorder = minBorder;
            _maxBorder = maxBorder;
            LastCubeSize = lastCubeSize;
        }

        public bool CheckBorders(Vector2 startPos)
        {
            var newCubeBorderMax = startPos + (Vector2)LastCubeSize / 2;
            var newCubeBorderMin = startPos - (Vector2)LastCubeSize / 2;
            return ComparisonBorders(newCubeBorderMin, newCubeBorderMax);
        }

        private bool ComparisonBorders(Vector2 cubeBorderMin, Vector2 cubeBorderMax)
        {
            return (cubeBorderMin.x >= _minBorder.x && cubeBorderMax.x <= _maxBorder.x) || 
                   (cubeBorderMin.y >= _minBorder.y && cubeBorderMax.y <= _maxBorder.y);
        }
        
        
    }
}