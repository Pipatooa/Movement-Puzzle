using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Goal : BaseTile
    {
        bool goalUsed;

        public Goal() : base()
        {
            objectID = 1;
            traversable = true;
        }

        public override void ProcessPlayer(ref Player player)
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