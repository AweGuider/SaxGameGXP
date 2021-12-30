using GXPEngine.Core;
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
        bool pressedSpace = false;

        float speedX;
        float speedY;

        int width;
        int height;

        public Player(float x, float y, int width, int height)
        {
            this.x = x;
            this.y = y;

            this.speedX = 1;
            this.speedY = 1;

            behaviourManager = new BehaviourManager();

            this.width = behaviourManager.GetCurrent().GetSprite().width;
            this.height = behaviourManager.GetCurrent().GetSprite().height;

            AddChild(behaviourManager.GetCurrent().GetSprite());
        }

        public int GetWidth()
        {
            return this.width;
        }

        public int GetHeight()
        {
            return this.height;
        }

        public void Update()
        {
            ChangeForm();
            Print();
            Move();
        }

        private void ChangeForm()
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

        private void Print()
        {
            if (Input.GetKeyDown(Key.SPACE) && !pressedSpace)
            {
                pressedSpace = true;
                Console.WriteLine("width = " + width + "; height = " + height);
            }
            if (Input.GetKeyUp(Key.SPACE))
            {
                pressedSpace = false;
            }
        }

        private void Move()
        {
            if (Input.GetKey(Key.A))
            {
                this.x -= speedX;
            }
            
            else if (Input.GetKey(Key.D))
            {
                this.x += speedX;
            }

            if (Input.GetKey(Key.W))
            {
                this.y -= speedY;
            }
        }
    }
}
