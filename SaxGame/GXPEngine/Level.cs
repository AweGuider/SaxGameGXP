using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMapParser;

namespace GXPEngine
{
    class Level : GameObject
    {
        TiledLoader tiledLoader;

        bool active;

        Player player;

        public Level(string filename)
        {
            tiledLoader = new TiledLoader(filename);
            Create();
            Map levelInfo = MapParser.ReadMap(filename);

            //CreateObjects(levelInfo);
            //CreateLevel(levelInfo);
        }

        public void Update()
        {
            if (player != null) player.Update();
        }

        public bool GetActive()
        {
            return active;
        }

        public void SetActive(bool b)
        {
            active = b;
        }

        private void CreateLevel(Map levelInfo)
        {
            if (levelInfo.Layers != null || levelInfo.Layers.Length == 0) return;
            Layer main = levelInfo.Layers[0];
            short[,] tileNumbers = main.GetTileArray();

            for (int row = 0; row < main.Height; row++)
            {
                for (int col = 0; col < main.Width; col++)
                {
                    int tileNumber = tileNumbers[col, row];
                    if (tileNumber > 0)
                    {
                        //IDK WHAT TO DO HERE!
                    }
                }
            }
        }

        private void Create()
        {
            tiledLoader.autoInstance = true;

            tiledLoader.LoadTileLayers(1);

            tiledLoader.LoadObjectGroups(0);
        }

        private void CreateObjects(Map levelInfo)
        {
            if (levelInfo.ObjectGroups == null || levelInfo.ObjectGroups.Length == 0) return;

            ObjectGroup objects = levelInfo.ObjectGroups[0];

            if (objects.Objects == null || objects.Objects.Length == 0) return;

            foreach (TiledObject obj in objects.Objects)
            {
                switch (obj.Name)
                {
                    case "Player":
                        player = new Player(obj.X, obj.Y);
                        AddChild(player);
                        Console.WriteLine("SUCCESS");
                        break;
                }
            }
        }
    }

    class LevelManager
    {

    }
}
