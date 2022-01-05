using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMapParser;

namespace GXPEngine
{
    public class Player : GameObject
    {
        BehaviourManager behaviourManager;

        Camera camera;

        private bool pressedX = false;
        private bool pressedSpace = false;

        private float speedX;
        private float speedY;

        private int width;
        private int height;

        private float gravity;

        public Player(float x, float y, TiledObject obj = null) : base(true)
        {
            /*this.x = obj.X;
            this.y = obj.Y;*/

            /*this.x = x;
            this.y = y;*/

            this.speedX = 1;
            this.speedY = 1;

            this.gravity = 0.25f;

            SetScaleXY(0.25f);

            behaviourManager = new BehaviourManager();

            this.width = behaviourManager.GetCurrent().GetSprite().width;
            this.height = behaviourManager.GetCurrent().GetSprite().height;

            AddChild(behaviourManager.GetCurrent().GetSprite());

            camera = new Camera(this.GetWidth(), this.GetHeight(), width, height);
            this.AddChild(camera);
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
            //MoveY();
            MoveX();
            Print();
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

        private void MoveX()
        {
            if (Input.GetKey(Key.A))
            {
                speedX -= 1f;
            }
            
            else if (Input.GetKey(Key.D))
            {
                speedX += 1f;
            }
            x += speedX * gravity;
            speedX *= 0.9f;
        }

        public void MoveY()
        {
            speedY += gravity / 2;
            y += speedY;
            /*if (MoveUntilCollision(0, speedY) != null)
            {
                speedY = 0;
            }*/

            if (y >= 600 - height * 4)
            {
                y = 600 - height * 4;
                speedY = 0;
            }

            if (Input.GetKeyDown(Key.W))
            {
                speedY = -6;
            }
        }

        void OnCollision(GameObject gameObject)
        {
            if (gameObject is Level)
            {
                Console.WriteLine("HOOLYMOLLY");
            }
        }

        private void Print()
        {
            if (Input.GetKeyDown(Key.SPACE) && !pressedSpace)
            {
                pressedSpace = true;
                Console.WriteLine("width = " + width + "; height = " + height);
                Console.WriteLine("x = " + this.x + "; y = " + this.y);
            }
            if (Input.GetKeyUp(Key.SPACE))
            {
                pressedSpace = false;
            }
        }
    }
}
