using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez.Sprites;
using Nez;
using Nez.AI.Pathfinding;
using Nez.Textures;
using System.Linq;
using Nez.AI.UtilityAI;
using System.IO;
using System.Collections.Generic;
using System;

namespace MonoGame
{
    public class Spider: Actor
    {
        public Spider(Vector2 vector2) : base(vector2)
        {
            startPosition = vector2;
            //Position = startPosition;
            Name = "Spider";

        }
        public override void OnAddedToScene()
        {
            AddComponent(new RangerMovement(this));
            LoadTexture("Spider");
        }
    }

    public class RangerMovement: EnemyMovement
    {
        public RangerMovement(Actor actor) : base(actor)
        {
            entity = actor;
            tilePosition = entity.startPosition;
        }
        public override void Controller()
        {
            Point entityPosition = new Point((int)(entity.Position.X / 16), (int)(entity.Position.Y / 16));
            Point playerPosition = new Point((int)(player.Position.X / 16), (int)(player.Position.Y / 16));

            List<Point> walls = GetWalls();
            AstarGridGraph grid = new AstarGridGraph(500, 500);


            //If the enemy x is the same as the players
            if (entityPosition.X == playerPosition.X)
            {
                //Doing a for loop to check if we would hit a wall or not
                for (int y = entityPosition.Y + 1; y < playerPosition.Y; y++)
                {
                    //If a wall is in signt before player return
                    if (walls.Contains(new Point(entityPosition.X, y)))
                    {
                        return;
                    }
                }
                for (int y = entityPosition.Y - 1; y > playerPosition.Y; y--)
                {
                    if (walls.Contains(new Point(entityPosition.X, y)))
                    {
                        return;
                    }
                }
                //If we never hit a wall they attack
                //player.spriteRenderer.Color = Color.Red;
                Debug.Log("I see the player");
            }
            else if (entityPosition.Y == playerPosition.Y)
            {

                for (int x = entityPosition.X + 1; x < playerPosition.X; x++)
                {
                    if (walls.Contains(new Point(x, entityPosition.Y)))
                    {
                        return;
                    }
                }
                for (int x = entityPosition.X - 1; x > playerPosition.X; x--)
                {
                    if (walls.Contains(new Point(x, entityPosition.Y)))
                    {
                        return;
                    }
                }
                //If we never hit a wall they attack
                player.spriteRenderer.Color = Color.Red;
                //player.healthSystem.TakeDamage(1);
                Debug.Log("I see the player");
            }
            entity.EndTurn();
        }
    }
}
