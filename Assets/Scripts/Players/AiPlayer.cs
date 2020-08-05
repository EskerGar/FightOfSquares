using System;
using System.Linq;
using Cubes;
using UnityEngine;
using static Cubes.FsPool;

namespace Players
{
    public class AiPlayer : Player
    {
        private readonly bool _upSide;
        
        public AiPlayer(bool isYourTurn, Vector3 startPos, Material material) : base(isYourTurn, startPos, material)
        {
            _upSide = CheckToLeftBorder();
        }

        protected override void TurnLogic()
        {
            if (_upSide)
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
        

        private bool CheckToLeftBorder()
        {
            return Mathf.Abs(startPos.x) - Mathf.Abs(Borders.LeftBorder) <= 4f;
        }
    }
}