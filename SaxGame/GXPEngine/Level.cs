using GXPEngine.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMapParser;

namespace GXPEngine
{
    class Level : GameObject
    {
        TiledLoader tiledLoader;

        private Player player;
        private Vector2 playerCheckPoint;

        private ArrayList deadZones = new ArrayList();

        bool active;

        bool mousePressed = false;

        public Level(string filename) : base(true)
        {
            tiledLoader = new TiledLoader(filename);
            Create();
            //Map levelInfo = MapParser.ReadMap(filename);

            //CreateObjects(levelInfo);
            //CreateLevel(levelInfo);

            scale = 2f;
        }

        public bool GetActive()
        {
            return active;
        }

        public void SetActive(bool b)
        {
            active = b;
        }

        public void Update()
        {
            if (player != null) player.Update();
            Scroll();
            PlayerReset();
        }

        private void Scroll()
        {
            ScrollX();
            ScrollY();
            MouseScroll();
        }

        private void ScrollX()
        {
            x = -player.x * scale + game.width/2;

        }

        private void ScrollY()
        {
            y = -player.y * scale + game.height / 2;

        }

        private void MouseScroll()
        {
            if (scale < 4.9f)
            {
                if (Input.GetMouseButtonDown(0) && !mousePressed)
                {
                    mousePressed = true;
                    scale += 0.6f;
                }
            }
            
            if (scale > 0.3f)
            {
                if (Input.GetMouseButtonDown(1) && !mousePressed)
                {
                    mousePressed = true; ;
                    scale -= 0.6f;
                }
            }

            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                mousePressed = false;
            }
        }

        private void PlayerReset()
        {
            // !!! NEED TO ASK HOW TO CREATE DEAD ZONES
            foreach (DeadZone dead in deadZones)
            {
                if (player.HitTest(dead))
                {
                    Console.WriteLine("YAYA");
                    player.x = playerCheckPoint.x;
                    player.y = playerCheckPoint.y;
                }
            }
            if (player.HitTest((GameObject)deadZones[0])) Console.WriteLine(deadZones.Count);
        }

        private void Create()
        {
            tiledLoader.rootObject = this;

            tiledLoader.autoInstance = true;

            tiledLoader.addColliders = false;
            tiledLoader.LoadTileLayers(0);

            tiledLoader.addColliders = true;
            tiledLoader.LoadTileLayers(1);

            tiledLoader.addColliders = false;
            tiledLoader.LoadTileLayers(2);

            tiledLoader.AddManualType("Player", "DeadZone");
            tiledLoader.OnObjectCreated += TiledLoader_OnObjectCreated;
            tiledLoader.LoadObjectGroups(0, 1);
        }

        private void TiledLoader_OnObjectCreated(Sprite sprite, TiledObject obj)
        {
            if (obj.Name == "Player")
            {
                player = new Player(obj.X, obj.Y);
                playerCheckPoint = new Vector2(obj.X, obj.Y);

                AddChild(player);
            }

            else if (obj.Type == "DeadZone")
            {
                deadZones.Add(new DeadZone(obj.X, obj.Y, obj.Width, obj.Height));
            }

            Console.WriteLine(obj.Name);
        }

        /// <Useful stuff for next time>
        /// private void CreateLevel(Map levelInfo)
        /// {
        ///     if (levelInfo.Layers != null || levelInfo.Layers.Length == 0) return;
        ///     Layer main = levelInfo.Layers[0];
        ///     short[,] tileNumbers = main.GetTileArray
        ///     for (int row = 0; row < main.Height; row++)
        ///     {
        ///         for (int col = 0; col < main.Width; col++)
        ///         {
        ///             int tileNumber = tileNumbers[col, row];
        ///             if (tileNumber > 0)
        ///             {
        ///                 TileSet tile = levelInfo.GetTileSet(tileNumber);
        ///                 AnimationSprite tileSprite = new AnimationSprite(tile.Image.FileName, tile.Columns, tile.Rows);
        ///                 tileSprite.SetFrame(tileNumber - tile.FirstGId);
        ///             }
        ///         }
        ///     }
        /// }
        ///
        /// private void CreateObjects(Map levelInfo)
        /// {
        ///     if (levelInfo.ObjectGroups == null || levelInfo.ObjectGroups.Length == 0) return;
        ///     ObjectGroup objects = levelInfo.ObjectGroups[0];
        ///     if (objects.Objects == null || objects.Objects.Length == 0) return;
        ///     foreach (TiledObject obj in objects.Objects)
        ///     {
        ///         switch (obj.Name)
        ///         {
        ///             case "Player":
        ///                 //player = new Player();
        ///                 AddChild(player);
        ///                 Console.WriteLine("SUCCESS");
        ///                 break;
        ///         }
        ///     }
        /// }
        /// 
        /// 
        /// </Useful>
    }

    class LevelManager
    {

    }
}
