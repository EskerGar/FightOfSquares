using System;
using System.Collections.Generic;
using Cubes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Players
{
    public abstract class Player
    {
        public bool IsYourTurn { get; set; }
        public int Score { get; private set; }
        protected List<GameObject> CubeList { get; } = new List<GameObject>();

        protected readonly Vector3 startPos;
        private readonly CreateCube _cubeCreator;
        private readonly CreateFreeSpot _freeSpotCreator;
        private readonly Material _material;
        protected GameObject lastCube;

        protected Player(bool isYourTurn, Vector3 startPos, Material material)
        {
            IsYourTurn = isYourTurn;
            var spawner = Object.FindObjectOfType<CreateCube>().gameObject;
            _cubeCreator = spawner.GetComponent<CreateCube>();
            _freeSpotCreator = spawner.GetComponent<CreateFreeSpot>();
            this.startPos = startPos;
            _material = material;
        }
        
        public void DoTurn()
        {
            lastCube = _cubeCreator.Generate();
            lastCube.SetMaterial(_material);
            if (CubeList.Count <= 0)
            {
                lastCube.SetPlace(startPos);
                FsPool.DeactivateFreeSpots(null);
                IsYourTurn = false;
            }
            else
            {
                foreach (var cube in CubeList)
                {
                    _freeSpotCreator.Generate(cube, lastCube);
                }
                TurnLogic();
            }
            Score += lastCube.GetSquare();
            CubeList.Add(lastCube);
        }
        
        
        protected abstract void TurnLogic();
    }
}