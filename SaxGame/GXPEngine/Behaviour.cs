using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class Behaviour
    {
        private Sprite sprite;
        private string name;

        private float speed;
        private int jumpAmount;
        private float jumpHeight;

        private bool breakDown;
        private bool breakSide;
        private bool fly;

        public Behaviour(string name, string imagePath, float speed, int jumps, float height, bool breakDown = false, bool breakSide = false, bool fly = false)
        {
            this.name = name;
            this.speed = speed;
            jumpAmount = jumps;
            jumpHeight = height;
            this.breakDown = breakDown;
            this.breakSide = breakSide;
            this.fly = fly;

            sprite = new Sprite(imagePath, false, false);
            sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
        }

        public float GetSpeed()
        {
            return speed;
        }

        public int GetJumpAmount()
        {
            return jumpAmount;
        }

        public float GetJumpHeight()
        {
            return jumpHeight;
        }
        public bool GetBreakDownPower()
        {
            return breakDown;
        }

        public bool GetBreakSidePower()
        {
            return breakSide;
        }

        public bool GetFlyPower()
        {
            return fly;
        }

        public Sprite GetSprite()
        {
            return sprite;
        }

        public override string ToString()
        {
            return name.ToString();
        }

    }
    
    class Triangle : Behaviour
    {
        public Triangle(string name, string path, float speed, int jumps, float jumpHeight) : base(name, path, speed, jumps, jumpHeight, false, true)
        {

        }
    }

    class Circle : Behaviour
    {
        public Circle(string name, string path, float speed, int jumps, float jumpHeight) : base(name, path, speed, jumps, jumpHeight, false, false, true)
        {

        }
    }

    class Box : Behaviour
    {
        public Box(string name, string path, float speed, int jumps, float jumpHeight) : base(name, path, speed, jumps, jumpHeight, true)
        {

        }
    }

    class BehaviourManager
    {
        ArrayList behaviours;

        Behaviour current;

        private int index;
        public BehaviourManager(Player player)
        {
            behaviours = new ArrayList
            {
                new Box("box", "square.png", 0.25f, 2, 0.75f),
                new Circle("circle", "circle.png", 0.5f, 1, 1.25f),
                new Triangle("triangle", "triangle.png", 0.75f, 1, 1)
            };
            index = 0;
            current = GetCurrentBehaviour();

            player.width = current.GetSprite().width;
            player.height = current.GetSprite().height;
            player.AddChild(current.GetSprite());
            player.SetParameters(current.GetSpeed(), current.GetJumpAmount(), current.GetJumpHeight());
        }

        public Behaviour GetCurrentBehaviour()
        {
            current = (Behaviour)behaviours[index];
            return current;
        }


        private Behaviour GetNextBehaviour()
        {
            index++;
            if (index >= behaviours.Count) index = 0;
            Console.WriteLine(behaviours[index].ToString());
            return GetCurrentBehaviour();
        }

        public void ChangeForm(Player player)
        {
            player.RemoveChild(current.GetSprite());
            player.AddChild(GetNextBehaviour().GetSprite());
            player.SetParameters(current.GetSpeed(), current.GetJumpAmount(), current.GetJumpHeight());
            player.SetPowers(current.GetBreakDownPower(), current.GetBreakSidePower(), current.GetFlyPower());
        }
    }
}
