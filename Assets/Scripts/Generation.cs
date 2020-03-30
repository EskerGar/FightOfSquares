using UnityEngine;

public class Generation: MonoBehaviour
{
    public void GenerationCube()
    {
        Vector3 scale = new Vector3(Random.Range(1,6) * 10, Random.Range(1, 6) * 10, .1f);
        GameManager.Instance.CubeBehaviour.CreateCube(scale);
    }

}
