using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMapParser;

namespace GXPEngine
{
    public class Player : Sprite
    {
        BehaviourManager behaviourManager;

       // Camera camera;

        private bool pressedX = false;
        private bool pressedSpace = false;

        private float speedX;
        private float speedY;

        private float oldX;

        private float gravity;
        private float slideSpeed;


        Sound jumpSound = new Sound("jump.mp3", false, false);

        public Player(float x, float y) : base("square.png")
        {
            this.x = x;
            this.y = y;

            speedX = 0;
            speedY = 0.5f;

            gravity = 0.1f;
            slideSpeed = 0.95f;

            SetOrigin(width / 2, height / 2);
            alpha = 0;

            behaviourManager = new BehaviourManager();

            width = behaviourManager.GetCurrent().GetSprite().width;
            height = behaviourManager.GetCurrent().GetSprite().height;

            AddChild(behaviourManager.GetCurrent().GetSprite());

            /*camera = new Camera((int)x - game.width / 3, (int)y - game.height / 3, game.width, game.height);
            camera.SetScaleXY(0.25f);
            AddChild(camera);*/
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public Behaviour GetBehaviour()
        {
            return this.behaviourManager.GetCurrent();
        }

        public void Update()
        {
            ChangeForm();
            MoveY();
            MoveX();
            Print();

            //behaviourManager.GetCurrent().GetSprite().rotation++;
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
            float tempSpeed = 0;

            oldX = x;

            if (Input.GetKey(Key.A) && !Input.GetKey(Key.D))
            {
                speedX = -1f;
            }
            else if (Input.GetKey(Key.D) && !Input.GetKey(Key.A))
            {
                speedX = 1f;
            }

            if (MoveUntilCollision(speedX, 0) != null)
            {
                //Something later maybe
            }

            speedX *= slideSpeed;

            if (Mathf.Abs(x - oldX) < (0.001f)) speedX = 0;

            /// <Acceleration>
            /// tempSpeed *= speed;
            /// speed += 0.01f;
            /// 
            /// Might be needed to add speed = 0 somewhere down!
            /// </Acceleration>
        }

        public void MoveY()
        {
            speedY += gravity * 0.4f;

            if (MoveUntilCollision(0, speedY) != null)
            {
                speedY = 0;
            }
            if (Input.GetKeyDown(Key.W))
            {
                speedY = -2;
                //Jump();
            }
        }
        private void Jump()
        {
            jumpSound.Play();
        }

        void OnCollision(GameObject gameObject)
        {
            
            //Console.WriteLine(gameObject.name);
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

            //Console.WriteLine("X = " + x + "; Y = " + y);
        }
    }
}
