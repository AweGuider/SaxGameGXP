using GXPEngine.Core;
using System;
using System.Collections;
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
        private bool pressedQ = false;
        private bool pressedW = false;

        private float speedX;
        private float speedY;
        private float speed;

        private float oldX;

        private float gravity;
        private float jumpHeight;
        private int jumps;
        private bool grounded = false;

        private float slideSpeed;

        ArrayList blocks;

        Sound jumpSound = new Sound("jump.mp3", false, false);

        public Player(float x, float y) : base("square.png")
        {
            this.x = x;
            this.y = y;

            speedX = 0;
            speedY = 0.5f;
            speed = 0.5f;

            gravity = 0.1f;
            jumpHeight = 1;
            jumps = 2;

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
            Jump();
            Move();
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

        private void Move()
        {
            float tempSpeed = 0;

            oldX = x;

            BreakSide wall = null;
            float distance = 0;

            foreach (Block block in blocks)
            {
                if (block is BreakSide breakSide)
                {
                    float tempDistance = DistanceTo(breakSide);
                    if (wall == null)
                    {
                        wall = breakSide;
                        distance = tempDistance;
                    }
                    else if (tempDistance < distance && wall != breakSide)
                    {
                        wall = breakSide;
                        distance = tempDistance;
                    }
                }
            }

            if (wall != null)
            {
                if (Input.GetKey(Key.SPACE)) wall.collider.isTrigger = true;
                else wall.collider.isTrigger = false;
            }

            if (Input.GetKey(Key.A) && !Input.GetKey(Key.D))
            {
                speedX = -speed;
            }
            else if (Input.GetKey(Key.D) && !Input.GetKey(Key.A))
            {
                speedX = speed;
            }


            if (wall != null && HitTest(wall))
            {
                blocks.Remove(wall);
                wall.LateDestroy();
            }


            if (MoveUntilCollision(speedX, 0) != null)
            {

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

        public void Jump()
        {
            speedY += gravity * 0.4f;
            
            if (Input.GetKeyDown(Key.W) && jumps > 0 && !pressedW)
            {
                pressedW = true;
                
                speedY = -2 * jumpHeight;
                grounded = false;
                jumps--;
                //jumpSound.Play();
            }
            
            else if (grounded)
            {
                jumps = 2;
            }

            if (Input.GetKeyUp(Key.W)) pressedW = false;

            foreach (Block block in blocks)
            {
                if (block is Platform platform)
                {
                    if (y < platform.y) platform.collider.isTrigger = false;
                    else platform.collider.isTrigger = true;
                }
            }

            if (MoveUntilCollision(0, speedY) != null)
            {
                if ((speedY > 0))
                {
                    grounded = true;
                }
                speedY = 0;

            }

            /// <Previous tries>
            /// bool isPlat = false;
            /// Platform plat = null;
            /// for (int i = 0; i < collisions.Length - 1; i++)
            /// {
            ///     foreach (Block block in blocks)
            ///     {
            ///         if (block is Platform platform)
            ///         {
            ///             if (block == (collisions[i]))
            ///             {
            ///                 Console.WriteLine("HAHAHAHA");
            ///                 plat = platform;
            ///                 isPlat = true;
            ///             }
            ///             else
            ///             {
            ///                 platform.collider.isTrigger = false;
            ///             }
            ///         }
            ///     }
            /// }
            /// if (speedY < 0)
            /// {
            ///     foreach (Block block in blocks)
            ///     {
            ///         if (block is Platform)
            ///         {
            ///             if (HitTest(block))
            ///             {
            ///                 //y += speedY;
            ///             }
            ///         }
            ///     }
            ///     y += speedY;
            /// }
            /// else
            /// {
            ///     if (MoveUntilCollision(0, speedY) != null)
            ///     {
            ///         if (speedY > 0)
            ///         {
            ///             grounded = true;
            ///         }
            ///         speedY = 0;
            ///     }
            /// }
            /// 
            /// </Previous>
        }

        void OnCollision(GameObject gameObject)
        {

            //Console.WriteLine(gameObject.name);
        }


        public void AddBlocksToCheck(ArrayList blocks)
        {
            this.blocks = blocks;
            //Console.WriteLine(this.blocks.Count);
        }

        private void Print()
        {
            if (Input.GetKeyDown(Key.Q) && !pressedQ)
            {
                pressedQ = true;
                Console.WriteLine("width = " + width + "; height = " + height);
                Console.WriteLine("x = " + this.x + "; y = " + this.y);
            }
            if (Input.GetKeyUp(Key.Q))
            {
                pressedQ = false;
            }

            //Console.WriteLine("X = " + x + "; Y = " + y);
        }
    }
}
