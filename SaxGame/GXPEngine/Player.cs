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


        private float gravity;
        private float jumpHeight;
        private int jumps;
        private int maxJumps;

        private bool grounded;
        private bool breakDown;
        private bool breakSide;
        private bool fly;


        private float oldX;
        private float slideSpeed;

        ArrayList blocks;

        Sound jumpSound = new Sound("jump.mp3", false, false);

        public Player(float x, float y) : base("square.png")
        {
            this.x = x;
            this.y = y;

            speedX = 0;
            speedY = 0;

            grounded = false;

            gravity = 0.03f;

            slideSpeed = 0.97f;

            SetOrigin(width / 2, height / 2);
            alpha = 0;

            behaviourManager = new BehaviourManager(this);

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

        public void SetParameters(float speed, int jumps, float jumpHeight)
        {
            this.speed = speed;
            this.jumps = jumps;
            this.jumpHeight = jumpHeight;
            maxJumps = jumps;
        }

        public void SetPowers(bool breakDown, bool breakSide, bool fly)
        {
            this.breakDown = breakDown;
            this.breakSide = breakSide;
            this.fly = fly;
        }

        public Behaviour GetBehaviour()
        {
            return behaviourManager.GetCurrentBehaviour();
        }

        public void Update()
        {
            ChangeForm();
            Movement();
            Print();

            //behaviourManager.GetCurrent().GetSprite().rotation++;
        }

        private void ChangeForm()
        {
            if (Input.GetKeyDown(Key.X) && !pressedX)
            {
                behaviourManager.ChangeForm(this);
                pressedX = true;
            }
            if (Input.GetKeyUp(Key.X))
            {
                pressedX = false;
            }
        }

        private void Movement()
        {
            //Wall breaking blocks
            BreakSide wall = null;
            float distW = 0;

            //Ground breaking blocks
            BreakDown ground = null;
            float distG = 0;

            //Depending on block type making it breakable or not
            foreach (Block block in blocks)
            {
                float tempDistance = DistanceTo(block);

                if (block is Platform platform)
                {
                    if (y < platform.y) platform.collider.isTrigger = false;
                    else platform.collider.isTrigger = true;
                }

                if (block is BreakSide breakSide)
                {
                    if (wall == null || (tempDistance < distW && wall != breakSide))
                    {
                        wall = breakSide;
                        distW = tempDistance;
                    }
                }

                if (block is BreakDown breakDown)
                {
                    if (ground == null || (tempDistance < distG && ground != breakDown))
                    {
                        ground = breakDown;
                        distG = tempDistance;
                    }
                }

                /// <Possible improvement>
                /// if ((wall == null || ground == null) || (tempDistance < dist))
                /// {
                ///     if (block is BreakSide breakSide && wall != breakSide)
                ///     {
                ///         wall = breakSide;
                ///         dist = tempDistance;
                ///     
                ///     if (block is BreakDown breakDown && ground != breakDown)
                ///     {
                ///         ground = breakDown;
                ///         dist = tempDistance;
                ///     }
                /// }
                /// if (block is Platform platform)
                /// {
                ///     if (y < platform.y) platform.collider.isTrigger = false;
                ///     else platform.collider.isTrigger = true;
                /// }
                /// </Possible>
            }

            //Break the wall/ground block on hit
            if (wall != null && breakSide)
            {
                if (Input.GetKey(Key.SPACE)) wall.collider.isTrigger = true;
                else wall.collider.isTrigger = false;
                if (HitTest(wall))
                {
                    blocks.Remove(wall);
                    wall.LateDestroy();
                }
            }
            if (ground != null && !grounded && breakDown)
            {
                if (Input.GetKey(Key.SPACE)) ground.collider.isTrigger = true;
                else ground.collider.isTrigger = false;
                if (HitTest(ground))
                {
                    blocks.Remove(ground);
                    ground.LateDestroy();
                }
            }

            //Change X speed
            if (Input.GetKey(Key.A) && !Input.GetKey(Key.D)) speedX = -speed;
            else if (Input.GetKey(Key.D) && !Input.GetKey(Key.A)) speedX = speed;

            //Change Y speed
            speedY += gravity;

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
                jumps = maxJumps;
            }
            if (Input.GetKeyUp(Key.W)) pressedW = false;

            //Actual movement until collision
            if (MoveUntilCollision(speedX, 0) != null) { }

            if (MoveUntilCollision(0, speedY) != null)
            {
                if (speedY > 0) grounded = true;
                speedY = 0;
            }

            //Slide effect calculation
            speedX *= slideSpeed;
            if (Mathf.Abs(x - oldX) < (0.001f)) speedX = 0;
            oldX = x;
        }

        private void Move()
        {
            float tempSpeed = 0;

            oldX = x;

            BreakSide wall = null;
            float wallDist = 0;

            foreach (Block block in blocks)
            {
                if (block is BreakSide breakSide)
                {
                    float tempDistance = DistanceTo(breakSide);
                    if (wall == null || (tempDistance < wallDist && wall != breakSide))
                    {
                        wall = breakSide;
                        wallDist = tempDistance;
                    }
                }
            }

            if (wall != null)
            {
                if (Input.GetKey(Key.SPACE)) wall.collider.isTrigger = true;
                else wall.collider.isTrigger = false;
                if (HitTest(wall))
                {
                    blocks.Remove(wall);
                    wall.LateDestroy();
                }
            }

            if (Input.GetKey(Key.A) && !Input.GetKey(Key.D)) speedX = -speed;
            else if (Input.GetKey(Key.D) && !Input.GetKey(Key.A)) speedX = speed;

            if (MoveUntilCollision(speedX, 0) != null) { }

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
            speedY += gravity;

            BreakDown ground = null;
            float distance = 0;

            if (Input.GetKeyDown(Key.W) && jumps > 0 && !pressedW)
            {
                pressedW = true;
                
                speedY = -2 * jumpHeight;
                grounded = false;
                jumps--;
                //jumpSound.Play();
            } else if (grounded) jumps = 2;

            if (Input.GetKeyUp(Key.W)) pressedW = false;

            foreach (Block block in blocks)
            {
                if (block is Platform platform)
                {
                    if (y < platform.y) platform.collider.isTrigger = false;
                    else platform.collider.isTrigger = true;
                }

                if (block is BreakDown breakDown)
                {
                    float tempDistance = DistanceTo(breakDown);
                    if (ground == null || (tempDistance < distance && ground != breakDown))
                    {
                        ground = breakDown;
                        distance = tempDistance;
                    }
                }
            }

            if (ground != null && !grounded)
            {
                if (Input.GetKey(Key.SPACE)) ground.collider.isTrigger = true;
                else ground.collider.isTrigger = false;
                if (HitTest(ground))
                {
                    blocks.Remove(ground);
                    ground.LateDestroy();
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
