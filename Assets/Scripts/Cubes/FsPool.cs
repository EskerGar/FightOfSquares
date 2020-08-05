using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cubes
{
    public static class FsPool
    {
        public static List<GameObject> FreeSpotsList { get; } = new List<GameObject>();
        private static List<GameObject> DeactivateFreeSpotsList { get; } = new List<GameObject>();

        public static void DeactivateFreeSpots(Action<Vector3> onClick)
        {
            foreach (var fs in FreeSpotsList)
            {
                fs.GetComponent<FreeSpotBehaviour>().UnsubscribeFromClick(onClick);
                fs.SetActive(false);
                DeactivateFreeSpotsList.Add(fs);
            }

            FreeSpotsList.Clear();
        }

        public static void AddFreeSpot(GameObject freeSpot)
        {
            FreeSpotsList.Add(freeSpot);
        }

        public static GameObject ActivateFreeSpot()
        {
            if (DeactivateFreeSpotsList.Count <= 0) return null;
            var freeSpot = DeactivateFreeSpotsList[0];
            DeactivateFreeSpotsList.RemoveAt(0);
            freeSpot.SetActive(true);
            return freeSpot;
        }

        public static bool CheckSpot(Vector3 position)
        {
            return FreeSpotsList.All(freeSpot => !freeSpot.transform.position.Equals(position));
        }

        public static void SubscribeToClick(Action<Vector3> onClick)
        {
            foreach (var freeSpot in FreeSpotsList)
            {
                freeSpot.GetComponent<FreeSpotBehaviour>().SubscribeToClick(onClick);
            }
        }
    }
}