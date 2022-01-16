using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class Behaviour
    {
        protected Sprite sprite;
        protected string name;

        private int jumpAmount;
        private float jumpHeight;
        private float speed;

        public Behaviour(string name, string imagePath)
        {
            this.name = name;
            sprite = new Sprite(imagePath, false, false);
            sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
        }

        public Sprite GetSprite()
        {
            return sprite;
        }

        public override string ToString()
        {
            return name.ToString();
        }

        public void SetSpeed(float speed)
        {
            this.speed = (speed == 0) ? 1 : speed;
        }

        public void SetJumpAmount(int jumps)
        {
            jumpAmount = (jumps == 0) ? 1 : jumps;
        }

        public void SetJumpHeight(float height)
        {
            jumpHeight = (height == 0) ? 1 : height;
        }
    }
    
    class Triangle : Behaviour
    {
        public Triangle(string name) : base(name, "triangle.png")
        {
            SetJumpAmount(0);
            SetSpeed(1.5f);
            SetJumpHeight(0);
        }
    }

    class Circle : Behaviour
    {
        public Circle(string name) : base(name, "circle.png")
        {
            SetJumpAmount(0);
            SetSpeed(0);
            SetJumpHeight(1.5f);
        }
    }

    class Box : Behaviour
    {
        public Box(string name) : base(name, "square.png")
        {
            SetJumpAmount(2);
            SetSpeed(0.5f);
            SetJumpHeight(0);
        }
    }

    class BehaviourManager
    {
        ArrayList behaviours;

        Behaviour current;

        int index;
        public BehaviourManager()
        {
            behaviours = new ArrayList
            {
                new Box("box"),
                new Circle("circle"),
                new Triangle("triangle")
            };
            index = 0;
            current = GetCurrent();
        }

        public Behaviour GetCurrent()
        {
            current = (Behaviour)behaviours[index];
            return current;
        }


        public Behaviour GetNext()
        {
            index++;
            if (index >= behaviours.Count) index = 0;
            Console.WriteLine(behaviours[index].ToString());
            return (Behaviour)behaviours[index];
        }
    }
}
