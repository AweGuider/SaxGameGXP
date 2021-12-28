using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class Player
    {
        BehaviourManager behaviourManager;

        private Behaviour curBehave;
        public Sprite sprite;
        public Player()
        {
            behaviourManager = new BehaviourManager();
            curBehave = behaviourManager.GetCurrent();
            sprite = curBehave.GetSprite();
        }

        public void Update()
        {
            if (Input.GetKeyDown(Key.X))
            {
                behaviourManager.SetNext(this);
            }
        }

        public Sprite GetSprite()
        {
            return this.sprite;
        }

        public void SetNextSprite(Behaviour behaviour)
        {
            curBehave = behaviour;
            sprite = curBehave.GetSprite();
        }
    }
}
