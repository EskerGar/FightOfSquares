using Cubes;
using UnityEngine;

namespace Players
{
    public class HumanPlayer : Player
    {
        public PlayerStatistic PlayerStatistic { get; }

        public HumanPlayer(bool isYourTurn, PlayerSettings settings) : base(isYourTurn, settings)
        {
            PlayerStatistic = new PlayerStatistic();
            NickName = PlayerStatistic.NickName;
        }
        
        protected override void TurnLogic()
        {
            FsPool.SubscribeToClick(ClickOnFreeSpot);
        }
        
            private void ClickOnFreeSpot(Vector3 freeSpotPos)
        {
            if(lastCube != null)
                lastCube.SetPlace(freeSpotPos);
            FsPool.DeactivateFreeSpots(ClickOnFreeSpot);
            CubeList.Add(lastCube);
            Score += lastCube.GetSquare();
            InvokeOnAddScore(Score);
            lastCube = null;
        }
    }
}