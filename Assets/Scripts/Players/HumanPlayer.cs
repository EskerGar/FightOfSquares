using Cubes;
using UnityEngine;

namespace Players
{
    public class HumanPlayer : Player
    {
        public HumanPlayer(bool isYourTurn, Vector3 startPos, Material material) : base(isYourTurn, startPos, material)
        {
        }

        protected override void TurnLogic()
        {
            FsPool.SubscribeToClick(ClickOnFreeSpot);
        }

            


            private void ClickOnFreeSpot(Vector3 freeSpotPos)
        {
            //if(lastCube != null)
                lastCube.SetPlace(freeSpotPos);
            FsPool.DeactivateFreeSpots(ClickOnFreeSpot);
            //lastCube = null;
            IsYourTurn = false;
        }
    }
}