using System;
using UnityEngine;

namespace Cubes
{
    public interface ICreateFreeSpot
    {
        void Generate(GameObject currentCube, GameObject lastCube);
    }
}