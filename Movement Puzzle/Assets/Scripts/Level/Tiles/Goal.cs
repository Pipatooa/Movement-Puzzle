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

        public new void ProcessPlayer(ref Player player)
        {
            Debug.Log("goal");
            
            if (!goalUsed)
            {
                player.reachedGoal = true;
                player.gameObject.SetActive(false);

                Events.OnPlayerReachedGoal();

                goalUsed = true;

                Debug.Log("yay");
            }
        }
    }
}