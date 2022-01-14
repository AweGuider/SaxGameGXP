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
        public Behaviour(string name, string imagePath)
        {
            this.name = name;
            sprite = new Sprite(imagePath, false, false);
            sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
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
        public Triangle(string name) : base(name, "triangle.png")
        {

        }
    }

    class Circle : Behaviour
    {
        public Circle(string name) : base(name, "circle.png")
        {

        }
    }

    class Box : Behaviour
    {
        public Box(string name) : base(name, "square.png")
        {

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
