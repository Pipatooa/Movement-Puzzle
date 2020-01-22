using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Goal : Tile
    {
        bool goalUsed;

        public Goal() : base()
        {
            objectID = 1;
            traversable = true;
        }

        public new void ProcessPlayer(ref Player player)
        {
            if (!goalUsed)
            {
                player.reachedGoal = true;
                player.gameObject.SetActive(false);

                Events.OnPlayerReachedGoal();

                goalUsed = true;
            }
        }
    }
}