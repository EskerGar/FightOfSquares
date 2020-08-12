using UnityEngine;

namespace Cubes
{
    public static class ExtensionsMethods
    {
        public static void SetPlace(this GameObject cube, Vector3 newPos)
        {
            cube.transform.position = newPos;
        }
        
        public static void SetMaterial(this GameObject cube, Material material)
        {
            var meshRender = cube.GetComponent<MeshRenderer>();
            if(meshRender != null)
                meshRender.material = material;
        }

        public static int GetSquare(this GameObject cube)
        {
            var localScale = cube.transform.localScale;
            return (int) (localScale.x * localScale.y);
        }

        public static void SetParametrs(this GameObject obj, Vector3 position, Vector3 scale)
        {
            obj.SetPlace(position);
            obj.transform.localScale = scale;
            obj.transform.Rotate(new Vector3(0, 0, 90));
        }
    }
}