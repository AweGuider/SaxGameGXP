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

        public Level(string filename)
        {
            tiledLoader = new TiledLoader(filename);
            Create();
            Map levelInfo = MapParser.ReadMap(filename);
            //CreateLevel(levelInfo);
            SetScaleXY(0.5f);
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

            tiledLoader.LoadTileLayers(0);

            tiledLoader.addColliders = true;

            tiledLoader.LoadTileLayers(1);

            tiledLoader.addColliders = true;

            tiledLoader.LoadTileLayers(2);

            tiledLoader.addColliders = true;
        }
    }

    class LevelManager
    {

    }
}
