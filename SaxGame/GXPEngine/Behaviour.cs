using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class Behaviour
    {
        protected Sprite sprite;
        protected string name;
        protected Behaviour(string name)
        {
            this.name = name;
        }

        public Sprite GetSprite()
        {
            return this.sprite;
        }

        public override string ToString()
        {
            return this.name.ToString();
        }
    }
    
    class Triangle : Behaviour
    {
        public Triangle(string name) : base(name)
        {
            base.sprite = new Sprite("triangle.png");
        }
    }

    class Circle : Behaviour
    {
        public Circle(string name) : base(name)
        {
            base.sprite = new Sprite("circle.png");
        }
    }

    class Box : Behaviour
    {
        public Box(string name) : base(name)
        {
            base.sprite = new Sprite("square.png");
        }
    }

    class BehaviourManager
    {
        ArrayList behaviours;

        Behaviour current;

        int index;
        public BehaviourManager()
        {
            this.behaviours = new ArrayList
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
            return (Behaviour) behaviours[index];
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
