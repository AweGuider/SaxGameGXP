using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class Player : GameObject
    {
        BehaviourManager behaviourManager;
        bool pressedX = false;

        public Player()
        {
            behaviourManager = new BehaviourManager();
            AddChild(behaviourManager.GetCurrent().GetSprite());
        }

        public void Update()
        {
            if (Input.GetKeyDown(Key.X) && !pressedX)
            {
                pressedX = true;
                RemoveChild(behaviourManager.GetCurrent().GetSprite());
                AddChild(behaviourManager.GetNext().GetSprite());
            }
            if (Input.GetKeyUp(Key.X))
            {
                pressedX = false;
            }
        }
    }
}
