using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame
{
    public class Player : Actor
    {
        public List<Item> Inventory = new List<Item>();

        public InventorySystem inventorySystem;
        public Player(Vector2 vector2) : base(vector2)
        {
            startPosition = vector2;
            //Position = startPosition;
            Name = "Player";
            AddComponent(new PlayerMovement(this));
            AddComponent(new InventorySystem(this));
        }

        public override void OnAddedToScene()
        {
            Scene.Camera.SetPosition(this.Position);
            LoadTexture("Player");

            inventorySystem = GetComponent<InventorySystem>();
        }
    }

    public class PlayerMovement: Movement
    {
        public PlayerMovement(Actor actor)
        {
            entity = actor;

            tilePosition = entity.startPosition;
        }
    }
}
