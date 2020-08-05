using UnityEngine;
using static Borders;

namespace Cubes
{
    public class Overlap
    {

        public bool CheckOverlap(Vector3 position, Vector3 size)
        {
            var results = Physics2D.OverlapBoxNonAlloc(position, size, 0, new Collider2D[1], LayerMask.GetMask("CubeLayer"));
            return results <= 0; //&& CheckFieldBorders(position, size);
        }

        private bool CheckFieldBorders(Vector3 position, Vector3 size)
        {
            var halfSize = size / 2;
            return position.x + halfSize.x <= RightBorder && position.x - halfSize.x >= LeftBorder && position.y + halfSize.y <= UpBorder &&
                   position.y - halfSize.y >= DownBorder;
        }
    }
}