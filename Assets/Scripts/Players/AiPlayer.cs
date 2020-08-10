using System;
using System.Linq;
using Cubes;
using UnityEngine;
using static Cubes.FsPool;

namespace Players
{
    public class AiPlayer : Player
    {

        public AiPlayer(bool isYourTurn, PlayerSettings settings) : base(isYourTurn, settings)
        {
        }

        protected override void TurnLogic()
        {
            if (isUpPlace)
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
            //lastCube = null;
            IsYourTurn = false;
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