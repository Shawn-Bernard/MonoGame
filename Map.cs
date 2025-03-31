using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.AI.Pathfinding;
using Nez.Sprites;
using Nez.Textures;
using Nez.Tiled;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonoGame
{
    public class Map : Entity
    {

        //public static Map map;

        TurnBasedSystem turnBasedSystem;

        System.Random rng = new();

        private string path = $"{Environment.CurrentDirectory}/../../../Maps/";

        public Dictionary<Vector2, int> tileMap;

        int rngX;

        int rngY;

        private List<string> Maps = new List<string>();

        private List<Item> ItemsInScene = new List<Item>();

        //AstarGridGraph grid;

        Texture2D wallTexture, groundTexture, exitTexture;

        public Map()
        {
            //map = this;
            Name = "Map";
            AddPreMadeMaps();
        }

        public override void OnAddedToScene()
        {
            
            //Debug.Log($"{Environment.CurrentDirectory}/../../../Maps/");

            turnBasedSystem = Scene.FindComponentOfType<TurnBasedSystem>();

            wallTexture = Scene.Content.Load<Texture2D>("Wall");
            groundTexture = Scene.Content.Load<Texture2D>("Ground");
            exitTexture = Scene.Content.Load<Texture2D>("Exit");

            MapStyle();

            AddListToScene();
        }
        public void ReloadMap()
        {
            foreach (var actor in turnBasedSystem.Actors.ToList())
            {
                turnBasedSystem.RemoveActor(actor);
            }

            turnBasedSystem.Actors.Clear();

            tileMap.Clear();

            // Remove sprite components
            RemoveAllComponents();

            OnAddedToScene();
        }

        private void MapStyle()
        {
            int mapRPNG = rng.Next(0, 2);

            //Picking which map style to load
            if (mapRPNG == 0)
            {
                tileMap = InitializeMap();
            }
            else
            {
                tileMap = TextMap(path + PickRandomMap());
            }
            loadMap();
        }

        private Dictionary<Vector2, int> InitializeMap()
        {
            Dictionary<Vector2, int> MapGen = new Dictionary<Vector2, int>();
            rngX = rng.Next(15, 30);
            rngY = rng.Next(15, 30);

            //grid = new AstarGridGraph(rngX, rngY);

            //Step 1 initializing map
            for (int x = 0; x < rngX; x++)
            {
                for (int y = 0; y < rngY; y++)
                {
                    //Adding my base map with only floors
                    MapGen.Add(new Vector2(x, y), 1);
                }
            }

            //Step 2: placing walls
            for (int x = 0; x < rngX; x++)
            {
                for (int y = 0; y < rngY; y++)
                {
                    //If statement that checks arounf the borders
                    if (x == 0 || y == 0 || x == rngX - 1 || y == rngY - 1)
                    {
                        MapGen[new Vector2(x, y)] = 0;
                    }
                }
            }

            //Step 3: Place clusters
            //Making a amount of clusters equal to the max map count
            int clusterCount = MapGen.Count;
            for (int i = 0; i < clusterCount; i++)
            {
                //Picking my start x & y for the to start clusters
                int clusterX = rng.Next(1, rngX - 2);
                int clusterY = rng.Next(1, rngY - 2);

                //Picking a random width & height for my clusters
                int clusterWidth = rng.Next(2, 4);
                int clusterHeight = rng.Next(2, 4);

                //Making a bool to check if i can place
                bool canPlace = true;

                //Checking a negative space so we can look around the cluster to see if we can place 
                //The reason being check back 1 space (-1) then forward space (width + 1)
                for (int x = -1; x < clusterWidth + 1; x++)
                {
                    for (int y = -1; y < clusterHeight + 1; y++)
                    {
                        Vector2 checkPosition = new Vector2(clusterX + x, clusterY + y);
                        //If my map has the key position & the value of 0 can place is false
                        if (MapGen.ContainsKey(checkPosition) && MapGen[checkPosition] == 0)
                        {
                            canPlace = false;
                        }
                    }
                }

                //If we can place doing a for loop with the cluster width / height
                if (canPlace)
                {
                    for (int x = 0; x < clusterWidth; x++)
                    {
                        for (int y = 0; y < clusterHeight; y++)
                        {
                            Vector2 ClusterPosition = new Vector2(clusterX + x, clusterY + y);

                            MapGen[ClusterPosition] = 0;
                        }
                    }
                }
            }

            bool playerPlaced = false;

            //Running a while loop untill my player is placed
            while (!playerPlaced)
            {
                //Picking a random stop within my map
                int playerX = rng.Next(1, rngX - 2);
                int playerY = rng.Next(1, rngY - 2);
                Vector2 checkPosition = new Vector2(playerX, playerY);

                // if my map 
                if (MapGen[checkPosition] == 1)
                {
                    Player player = new Player(checkPosition);
                    turnBasedSystem.AddActor(player);
                    playerPlaced = true;
                }
            }

            int EnemyCount = rng.Next(2, 4);

            for (int EnemyPlaced = 0; EnemyPlaced < EnemyCount; EnemyPlaced++)
            {
                bool enemyPlaced = false;
                while (!enemyPlaced)
                {
                    int enemyX = rng.Next(2, rngX - 2);
                    int enemyY = rng.Next(2, rngY - 2);
                    Vector2 checkPosition = new Vector2(enemyX, enemyY);

                    if (MapGen.ContainsKey(checkPosition) && MapGen[checkPosition] == 1)
                    {
                        Enemy enemy = new Enemy(checkPosition);
                        turnBasedSystem.AddActor(enemy);
                        enemyPlaced = true;
                    }
                }
            }

            int GhostCount = rng.Next(1, 3);

            for (int EnemyPlaced = 0; EnemyPlaced < EnemyCount; EnemyPlaced++)
            {
                bool GhostPlaced = false;
                while (!GhostPlaced)
                {
                    int enemyX = rng.Next(2, rngX - 2);
                    int enemyY = rng.Next(2, rngY - 2);
                    Vector2 checkPosition = new Vector2(enemyX, enemyY);

                    if (MapGen.ContainsKey(checkPosition) && MapGen[checkPosition] == 1)
                    {
                        Ghost ghost = new Ghost(checkPosition);
                        turnBasedSystem.AddActor(ghost);
                        GhostPlaced = true;
                    }
                }
            }


            int SpiderCount = rng.Next(1, 2);

            for (int EnemyPlaced = 0; EnemyPlaced < EnemyCount; EnemyPlaced++)
            {
                bool SpiderPlaced = false;
                while (!SpiderPlaced)
                {
                    int enemyX = rng.Next(2, rngX - 2);
                    int enemyY = rng.Next(2, rngY - 2);
                    Vector2 checkPosition = new Vector2(enemyX, enemyY);

                    if (MapGen.ContainsKey(checkPosition) && MapGen[checkPosition] == 1)
                    {
                        Spider spider = new Spider(checkPosition);
                        turnBasedSystem.AddActor(spider);
                        SpiderPlaced = true;
                    }
                }
            }



            bool exitPlaced = false;
            while (!exitPlaced)
            {
                // Picking a random start X & Y for the door placement
                int doorX = rng.Next(1, rngX - 2);
                int doorY = rng.Next(1, rngY - 2);

                Vector2 checkPosition = new Vector2(doorX, doorY);

                if (MapGen.ContainsKey(checkPosition) && MapGen[checkPosition] == 1)
                {
                    MapGen[checkPosition] = 2;
                    exitPlaced = true;
                }
            }
            return MapGen;
        }


        // Adding my list of strings for my map
        private void AddPreMadeMaps()
        {
            Maps.Add("Level_1.txt");
            Maps.Add("Level_2.txt");
            Maps.Add("Level_3.txt");
        }

        public int checkTile(Vector2 checkedPosition)
        {
            if (tileMap.ContainsKey(checkedPosition))
            {
                return tileMap[checkedPosition];// Return the tile value if it there
            }
            else
            {
                return -1; // Return a default value 
            }
        }

        //Picks a random map from the list of maps 
        private string PickRandomMap()
        {
            rng = new System.Random();
            int index = rng.Next(Maps.Count);
            return Maps[index];
        }
        private Dictionary<Vector2, int> TextMap(string filepath)
        {
            Dictionary<Vector2, int> result = new Dictionary<Vector2, int>();
            StreamReader reader = new StreamReader(filepath);
            int y = 0;
            string line;

            //This will give line the value untill the reader is done reading the text file
            while ((line = reader.ReadLine()) != null)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    Vector2 tilePosition = new Vector2(x,y); // Correct y indexing
                    char tile = line[x];
                    switch (tile)
                    {
                        case '#':
                            result[tilePosition] = 0; // Walls
                            break;
                        case '-':
                            result[tilePosition] = 1; // Floor
                            break;
                        case '*':
                            result[tilePosition] = 2; // Exit
                            break;
                        case '@':
                            result[tilePosition] = 1; // Player
                            Player player = new Player(tilePosition);
                            turnBasedSystem.AddActor(player);
                            break;
                        case '!':
                            result[tilePosition] = 1; // Enemy
                            Enemy enemy = new Enemy(tilePosition);
                            turnBasedSystem.AddActor(enemy);
                            break;
                        case 'O':
                            result[tilePosition] = 1; // Ghost
                            Ghost ghost = new Ghost(tilePosition);
                            turnBasedSystem.AddActor(ghost);
                            break;
                        case '.':
                            result[tilePosition] = 1; // Spider
                            Spider spider = new Spider(tilePosition);
                            turnBasedSystem.AddActor(spider);
                            break;
                        case '+':
                            result[tilePosition] = 1; // Health
                            HealingPotion potion = new HealingPotion();
                            potion.tilePosition = tilePosition;
                            ItemsInScene.Add(potion);
                            break;
                        case 'F':
                            result[tilePosition] = 1; // FireBall
                            ScrollOfFireball fireBall = new ScrollOfFireball();
                            fireBall.tilePosition = tilePosition;
                            ItemsInScene.Add(fireBall);
                            break;
                        case 'L':
                            result[tilePosition] = 1; // Lightninhg 
                            ScrollOfLightning lightning = new ScrollOfLightning();
                            lightning.tilePosition = tilePosition;
                            ItemsInScene.Add(lightning);
                            break;
                    }
                }
                y++;
            }
            return result;
        }
        public void loadMap()
        {
            //The result is the return from Text Map
            foreach (var Result in tileMap)
            {
                Vector2 tilePosition = new Vector2(Result.Key.X,Result.Key.Y);
                //Position = tilePosition;
                switch (Result.Value)
                {
                    case 0:
                        addTile(wallTexture, tilePosition);
                        //spriteRenderer.SetTexture(wallTexture).SetOrigin(Position);
                        break;
                    case 1:
                        addTile(groundTexture, tilePosition);
                        break;
                    case 2:
                        addTile(exitTexture, tilePosition);
                        break;
                }
            }
        }
        public void addTile(Texture2D texture, Vector2 position)
        {
            SpriteRenderer tileRenderer = new SpriteRenderer(texture);
            tileRenderer.SetOrigin(-position * 16);
            tileRenderer.SetLayerDepth(1);
            //Debug.Log(tileRenderer.Sprite);
            //Debug.Log(Scale);
            AddComponent(tileRenderer);
        }

        private void AddListToScene()
        {
            foreach (Actor actor in turnBasedSystem.Actors)
            {
                Scene.AddEntity(actor);
                Debug.Log(actor.Name);
            }

            foreach (Item item in ItemsInScene)
            {
                Scene.AddEntity(item);
            }
        }
    }
}
