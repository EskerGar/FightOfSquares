﻿using System;
using System.Collections.Generic;
using Cubes;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Players
{
    public abstract class Player
    {
        public bool IsYourTurn { get; set; }
        public int Score { get; protected set; }

        public string NickName { get; protected set; }
        public event Action<int> OnAddScore; 

        protected GameObject lastCube;
        
        protected List<GameObject> CubeList { get; } = new List<GameObject>();
        private Vector3 _startPos;
        private readonly CreateCube _cubeCreator;
        private readonly CreateFreeSpot _freeSpotCreator;
        private readonly Material _material;
        private readonly int _sideCoef;
        private Func<Vector3, Vector3, Vector3> _addToStartPos;

        protected Player(bool isYourTurn, PlayerSettings settings)
        {
            IsYourTurn = isYourTurn;
            var spawner = Object.FindObjectOfType<CreateCube>().gameObject;
            _cubeCreator = spawner.GetComponent<CreateCube>();
            _freeSpotCreator = spawner.GetComponent<CreateFreeSpot>();
            _startPos = settings.StartPos;
            _material = settings.Material;
            _sideCoef = settings.SideCoef;
        }
        
        public void DoTurn()
        {
            if (lastCube != null && CubeList.Count > 1)
            {
                lastCube.SetActive(true);
                lastCube.transform.localScale = _cubeCreator.GenerateSize(lastCube);
            }
            else
            {
             lastCube = _cubeCreator.Generate();
            }
            lastCube.SetMaterial(_material);
            if (CubeList.Count <= 0)
            {
                var halfSize = lastCube.transform.localScale / 2;
                _startPos = new Vector3( _startPos.x +_sideCoef * halfSize.x,  _startPos.y - _sideCoef * halfSize.y);
                lastCube.SetPlace(_startPos);
                CubeList.Add(lastCube);
                Score += lastCube.GetSquare();
                InvokeOnAddScore(Score);
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
        }

        public void ControlLastCube()
        {
            if(lastCube != null && CubeList.Count > 1)
                lastCube.SetActive(false);
        }

        public void OffSquareCubes()
        {
            foreach (var cube in CubeList)
            {
                cube.transform.Find("Square").gameObject.SetActive(false);
            }
        }
        
        protected abstract void TurnLogic();

        protected void InvokeOnAddScore(int score)
        {
            OnAddScore?.Invoke(score);
        }
    }
}