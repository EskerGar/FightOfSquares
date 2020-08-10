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

        protected GameObject lastCube;
        
        private List<GameObject> CubeList { get; } = new List<GameObject>();
        private Vector3 _startPos;
        private readonly CreateCube _cubeCreator;
        private readonly CreateFreeSpot _freeSpotCreator;
        private readonly Material _material;
        private readonly int _sideCoef;
        protected readonly bool isUpPlace;
        private Func<Vector3, Vector3, Vector3> _addToStartPos;

        protected Player(bool isYourTurn, PlayerSettings settings)
        {
            IsYourTurn = isYourTurn;
            var spawner = Object.FindObjectOfType<CreateCube>().gameObject;
            _cubeCreator = spawner.GetComponent<CreateCube>();
            _freeSpotCreator = spawner.GetComponent<CreateFreeSpot>();
            _startPos = settings.StartPos;
            _material = settings.Material;
            isUpPlace = settings.IsUpPlace;
            _sideCoef = settings.SideCoef;
        }
        
        public void DoTurn()
        {
            lastCube = _cubeCreator.Generate();
            lastCube.SetMaterial(_material);
            if (CubeList.Count <= 0)
            {
                var halfSize = lastCube.transform.localScale / 2;
                _startPos = new Vector3( _startPos.x +_sideCoef * halfSize.x,  _startPos.y - _sideCoef * halfSize.y);
                lastCube.SetPlace(_startPos);
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