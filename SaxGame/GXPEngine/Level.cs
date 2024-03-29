﻿using GXPEngine.Core;
using GXPEngine.OpenGL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMapParser;

namespace GXPEngine
{
    public class Level : GameObject
    {
        TiledLoader tiledLoader;

        protected Player player;
        private Vector2 playerCheckPoint;

        private ArrayList deadZones = new ArrayList();
        private ArrayList blocks = new ArrayList();

        bool active;
        float newScroll;

        public Level(string filename) : base(true)
        {
            tiledLoader = new TiledLoader(filename);
            Create();

            scale = 2f;

            /// <Alternative>
            /// Map levelInfo = MapParser.ReadMap(filename);
            /// CreateObjects(levelInfo);
            /// CreateLevel(levelInfo);
            /// </Alternative>
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
            x = -player.x * scale + game.width / 2;
            if (x <= -2720) x = -2720;
            if (x >= 0) x = 0;

        }

        private void ScrollY()
        {
            Console.WriteLine("Player Y: " + player.y + "; Level Y: " + y + ";");

            y = -player.y * scale + game.height / 2;
            if (y <= -370) y = -370;
            if (y >= 0) y = 0;
        }

        private void MouseScroll()
        {
            newScroll = GL.glfwGetMouseWheel();
            if (newScroll > 5) newScroll = 5;
            if (newScroll < -3) newScroll = -3;
            if (newScroll > -4 && newScroll < 6 && newScroll != 0) scale = 2 + 0.6f * newScroll;
            if (newScroll == 0) scale = 2;
        }

        public bool PlayerReset()
        {
            foreach (DeadZone dead in deadZones)
            {
                if (player.HitTest(dead))
                {
                    player.x = playerCheckPoint.x;
                    player.y = playerCheckPoint.y;
                    return true;
                }
            }
            return false;
        }

        private void Create()
        {
            tiledLoader.rootObject = this;

            tiledLoader.autoInstance = true;

            tiledLoader.addColliders = false;
            tiledLoader.LoadTileLayers(0);

            tiledLoader.addColliders = true;
            tiledLoader.LoadTileLayers(1);

            LoadSpecialBlocks();

            tiledLoader.AddManualType("Player");
            tiledLoader.OnObjectCreated += TiledLoader_OnObjectCreated;
            tiledLoader.LoadObjectGroups(1, 2);

            tiledLoader.addColliders = false;
            tiledLoader.LoadTileLayers(3);

            player.AddBlocksToCheck(blocks);
        }

        private void TiledLoader_OnObjectCreated(Sprite sprite, TiledObject obj)
        {
            if (obj.Name == "Player")
            {
                player = new Player(obj.X, obj.Y);
                playerCheckPoint = new Vector2(obj.X, obj.Y);

                AddChild(player);
            }

            if (obj.Type == "DeadZone")
            {
                deadZones.Add(sprite as DeadZone);
            }

            Console.WriteLine(obj.Name);
        }

        private void LoadSpecialBlocks()
        {
            short[,] tileNumbers = tiledLoader.map.Layers[2].GetTileArray();
            for (int row = 0; row < tiledLoader.map.Layers[2].Height; row++)
            {
                for (int col = 0; col < tiledLoader.map.Layers[2].Width; col++)
                {
                    int tileNumber = tileNumbers[col, row];
                    if (tileNumber > 0)
                    {
                        TileSet tile = tiledLoader.map.GetTileSet(tileNumber);

                        //Console.WriteLine(tileNumber);
                        //Console.WriteLine(tile.Image.FileName);

                        switch (tileNumber)
                        {
                            case 1061: case 1062:
                                Platform platform = new Platform(tile.Image.FileName, tile.Columns, tile.Rows);
                                platform.SetXY(col * tile.TileWidth, row * tile.TileHeight);
                                platform.SetFrame(tileNumber - tile.FirstGId);
                                blocks.Add(platform);
                                AddChild(platform);
                                break;
                            case 1171: case 1172: case 1204: case 1205: case 1303: case 1304: case 1404:
                                BreakSide breakSide = new BreakSide(tile.Image.FileName, tile.Columns, tile.Rows);
                                //breakSide.SetOrigin(tile.TileWidth / 2, tile.TileHeight / 2);
                                breakSide.SetXY(col * tile.TileWidth, row * tile.TileHeight);
                                breakSide.SetFrame(tileNumber - tile.FirstGId);
                                blocks.Add(breakSide);
                                AddChild(breakSide);
                                break;
                            case 1179: case 1180: case 1212: case 1213: case 1311: case 1312: case 1412:
                                BreakDown breakDown = new BreakDown(tile.Image.FileName, tile.Columns, tile.Rows);
                                breakDown.SetXY(col * tile.TileWidth, row * tile.TileHeight);
                                breakDown.SetFrame(tileNumber - tile.FirstGId);
                                blocks.Add(breakDown);
                                AddChild(breakDown);
                                break;
                            default:
                                break;

                        }
                    }
                }
            }
        }

        /// <Useful stuff for next time>
        /// private void CreateLevel(Map levelInfo)
        /// {
        ///     if (levelInfo.Layers != null || levelInfo.Layers.Length == 0) return;
        ///     Layer main = levelInfo.Layers[0];
        ///     short[,] tileNumbers = main.GetTileArray();
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

    public class LevelManager : GameObject
    {
        Level active;

        List<Level> levels;
        List<string> levelPaths;
        public LevelManager(List<string> levelPaths)
        {
            this.levelPaths = levelPaths;
            levels = new List<Level>();
            levels.Add(new Level(levelPaths[0]));
            active = levels[0];
            AddChild(active);
            if (this.levelPaths.Count() > 0) levelPaths.RemoveAt(0);
            Console.WriteLine(levels.Count);
        }

        public void Update()
        {
            if (active != null) active.Update();
            if (Input.GetKeyUp(Key.TWO)) UploadLevel();
        }

        private void UploadLevel()
        {
            if (levelPaths.Count() < 1) return;
            levels.Add(new Level(levelPaths[0]));
            DestroyLevel();
            active = levels.Last();
            AddChild(active);
            levelPaths.RemoveAt(0);
        }

        private void LoadLevel(int n)
        {
            DestroyLevel();
            active = levels[n - 1];
        }

        private void DestroyLevel()
        {
            List<GameObject> children = GetChildren();
            for (int i = children.Count - 1; i > 0; i--)
            {
                children[i].LateDestroy();
            }
        }
    }
}
