using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez.Sprites;
using Nez;
using Nez.AI.Pathfinding;
using System.Collections.Generic;


namespace MonoGame
{
    public class Enemy : Actor
    {
        public Enemy(Vector2 vector2) : base(vector2)
        {
            startPosition = vector2;
            //Position = startPosition;
            Name = "Enemy";
            
        }

        public override void OnAddedToScene()
        {
            AddComponent(new EnemyMovement(this));
            Scene.Camera.SetPosition(this.Position);
            LoadTexture("Enemy");
        }
    }

    public class EnemyMovement : Movement
    {
        public Actor player;

        public EnemyMovement(Actor actor)
        {
            entity = actor;
            tilePosition = entity.startPosition; // Use startPosition (tile-based) for movement
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            player = (Actor)entity.Scene.FindEntity("Player");
        }
        public List<Point> GetWalls()
        {
            // Making a list of walls 
            List<Point> walls = new List<Point>();

            foreach (var Result in map.tileMap)
            {
                Vector2 position = Result.Key;
                int tileType = Result.Value;

                if (tileType == 0 || tileType == 2 || tileType == 3)
                {

                    walls.Add(new Point((int)position.X, (int)position.Y));
                }
            }

            return walls;
        }

        public override void Controller()
        {
            Vector2 targetPosition = tilePosition;


            // Getting tile position for my points 
            Point entityPosition = new Point((int)(entity.Position.X / 16), (int)(entity.Position.Y / 16));
            Point playerPosition = new Point((int)(player.Position.X / 16), (int)(player.Position.Y / 16));

            // Adding a list of wall that are points 
            List<Point> walls = GetWalls();


            // Making my grind how for looking. this should be from my tile map but this is a quick fix 
            AstarGridGraph grid = new AstarGridGraph(500, 500);

            // Adding my walls for each point in list of walls
            foreach (Point wall in walls)
            {
                grid.Walls.Add(wall);
            }

            // Getting the path for the searched path to and from
            List<Point> path = grid.Search(entityPosition, playerPosition);

            if (path != null && path.Count > 1)
            {
                // Getting the value of point that is a vector  
                Point nextStep = path[1];
                if (nextStep == playerPosition)
                {
                    Debug.Log("Player is next step");
                    return;
                }

                targetPosition = new Vector2(nextStep.X, nextStep.Y);

                InteractOrMove(targetPosition);
            }
            else
            {
                targetPosition = player.Position;

                InteractOrMove(targetPosition);
            }
        }
    }
}
