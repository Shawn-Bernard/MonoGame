using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez.Sprites;
using Nez;

namespace MonoGame
{
    public class Movement : Component, IUpdatable
    {
        public Actor entity;
        public Vector2 tilePosition; // This needs to be updated to map grind
        public Map map;


        // This is basically the start method
        public override void OnAddedToEntity()
        {
            map = (Map)Entity.Scene.FindEntity("Map");
            //tilePosition = entity.startPosition / 16;
            entity.Position = tilePosition * 16;
            //Debug.Log("entity.startPosition");
            //Debug.Log(entity.startPosition);
            //Debug.Log("entity.Position");
            //Debug.Log(entity.Position);
            //Debug.Log("tilePosition");
            //Debug.Log(tilePosition);
        }

        // Component, IUpdatable lets me use the update method
        public void Update()
        {
            if (entity.isTurn && !entity.WaitAnimation)
            {
                Controller();
            }

        }

        public virtual void Controller()
        {
            entity.Scene.Camera.SetPosition(entity.Position);

            Vector2 move = tilePosition;

            if (Input.IsKeyPressed(Keys.W))
            {
                //Debug.Log("Moving Up");
                move.Y -= 1;
                //Debug.Log(move);
            }
            if (Input.IsKeyPressed(Keys.S))
            {
                //Debug.Log("Moving Down");
                move.Y += 1;
                //Debug.Log(move);
            }
            if (Input.IsKeyPressed(Keys.A))
            {
                //Debug.Log("Moving Left");
                move.X -= 1;
                //Debug.Log(move);
            }
            if (Input.IsKeyPressed(Keys.D))
            {
                //Debug.Log("Moving Right");
                move.X += 1;
                //Debug.Log(move);
            }
            if (move != tilePosition)
            InteractOrMove(move);
        }

        public virtual void InteractOrMove(Vector2 targetPosition)
        {
            // Get a return from the target position
            int tile = map.checkTile(targetPosition);

            //Switch case for the tile int
            switch (tile)
            {
                case 0: // Wall
                    break;
                case 1: // Ground
                    //Debug.Log("Ground");
                    tilePosition = targetPosition;
                    break;
                case 2: // Exit
                    map.ReloadMap();
                    break;
                case 3: // Player
                    Debug.Log("Player!!!!!!!!!!!!!!!");
                    tilePosition = targetPosition;
                    break;
                case 4: // Enemy
                    Debug.Log("Enemy!!!!!!!!!!!!!!!");
                    tilePosition = targetPosition;
                    break;
            }
            entity.Move( tilePosition * 16);
        }
    }
}
