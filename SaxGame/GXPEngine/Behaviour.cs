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
        protected String name;
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
        public Triangle(string name)
        {
            base.sprite = new Sprite("triangle.png");
            base.name = name;
        }
    }

    class Circle : Behaviour
    {
        public Circle(string name)
        {
            base.sprite = new Sprite("circle.png");
            base.name = name;
        }
    }

    class Box : Behaviour
    {
        public Box(string name)
        {
            base.sprite = new Sprite("square.png");
            base.name = name;
        }
    }

    class BehaviourManager
    {
        ArrayList behaviours;
        int current;
        public BehaviourManager()
        {
            this.behaviours = new ArrayList
            {
                new Box("box"),
                new Circle("circle"),
                new Triangle("triangle")
            };
            current = 0;
        }

        public Behaviour GetCurrent()
        {
            return (Behaviour) behaviours[current];
        }

        public void SetNext(Player player)
        {
            if (current + 1 > behaviours.Count) current = -1;
            player.SetNextSprite((Behaviour) behaviours[++current]);
        }
    }
}
