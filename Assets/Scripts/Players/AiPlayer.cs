using System;
using System.Linq;
using Cubes;
using UnityEngine;
using static Cubes.FsPool;

namespace Players
{
    public class AiPlayer : Player
    {
        private readonly bool _isUpPlace;

        public AiPlayer(bool isYourTurn, PlayerSettings settings) : base(isYourTurn, settings)
        {
            _isUpPlace = settings.IsUpPlace;
        }

        protected override void TurnLogic()
        {
            if(FsPool.FreeSpotsList.Count <= 0) return;
            if (_isUpPlace)
                lastCube.SetPlace(FindRightSpot(
                    (freeSpot, rightFreeSpot) =>
                    {
                        var freeSpotPos = freeSpot.transform.position;
                        var rightSpotPos = rightFreeSpot.transform.position;
                        return freeSpotPos.x > rightSpotPos.x &&
                               freeSpotPos.y > rightSpotPos.y;
                    }).transform.position);
            else
                lastCube.SetPlace(FindRightSpot(
                    (freeSpot, rightFreeSpot) =>
                    {
                        var freeSpotPos = freeSpot.transform.position;
                        var rightSpotPos = rightFreeSpot.transform.position;
                        return freeSpotPos.x < rightSpotPos.x &&
                               freeSpotPos.y < rightSpotPos.y;
                    }).transform.position);
            CubeList.Add(lastCube);
            Score += lastCube.GetSquare();
            InvokeOnAddScore(Score);
            lastCube = null;
            DeactivateFreeSpots(null);
        }

        private GameObject FindRightSpot(Func<GameObject, GameObject, bool> condition)
        {
            var rightFreeSpot = FreeSpotsList[0]; 
            foreach (var freeSpot in FreeSpotsList.Where(cube => condition(cube, rightFreeSpot)))
            {
                rightFreeSpot = freeSpot;
            }

            return rightFreeSpot;
        }
    }
}