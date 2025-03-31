using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez.Sprites;
using Nez;
using Nez.AI.Pathfinding;
using System.Collections.Generic;

namespace MonoGame
{
    public class InventorySystem : Component, IUpdatable
    {
        public Player Owner;

        public InventorySystem(Actor player)
        {
            Owner = (Player)player;
        }

        public void Update()
        {
            if (Owner.isTurn && Owner.WaitAnimation)
            {
                InventoryController();
            }
        }

        public void AddItem(Item item)
        {
            if (Owner.Inventory.Count > 5)
            {
                Debug.Log("No room for item");
                return;
            }
            else
            {
                Owner.Inventory.Add(item);
            }
        }

        private void InventoryController()
        {
            if (Input.IsKeyPressed(Keys.D1))
            {
                UseItem(0);
            }
            if (Input.IsKeyPressed(Keys.D2))
            {
                UseItem(1);
            }
            if (Input.IsKeyPressed(Keys.D3))
            {
                UseItem(2);
            }
            if (Input.IsKeyPressed(Keys.D4))
            {
                UseItem(3);
            }
            if (Input.IsKeyPressed(Keys.D5))
            {
                UseItem(4);
            }
        }

        private void UseItem(int keyNum)
        {
            if (Owner.Inventory.Count > keyNum)
            {
                Owner.Inventory[keyNum].Use();
                Owner.Inventory.RemoveAt(keyNum);
            }
            else
            {
                Debug.Log("No item is here");
            }
        }
    }

}
