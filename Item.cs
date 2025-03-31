using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez.Sprites;
using Nez;
using Nez.Systems;

namespace MonoGame
{
    public abstract class Item: Entity
    {
        public Player Owner;

        private Texture2D Texture;

        public Vector2 tilePosition;


        public override void OnAddedToScene()
        {
            Owner = (Player)Scene.FindEntity("Player");
        }
        public void LoadTexture(string textureName)
        {
            //Position = tilePosition;
            Texture = Owner.Scene.Content.Load<Texture2D>(textureName);
            SpriteRenderer tileRenderer = new SpriteRenderer(Texture);
            tileRenderer.SetOrigin(-tilePosition * 16);
            tileRenderer.SetLayerDepth(1);
            //Debug.Log(startPosition);
            //Debug.Log(startPosition / 16);
            AddComponent(tileRenderer);
        }

        public virtual void Use()
        {

        }

        public virtual void Use(Actor actor)
        {

        }
    }

    public class HealingPotion : Item
    {
        int HealAmount = 1;

        private Texture2D Health;

        public HealingPotion()
        {
            Name = "Healing Potion"; 
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();
            LoadTexture("Heal");
        }


        public override void Use()
        {
            Owner.healthSystem.Heal(HealAmount);
            Owner.EndTurn();
        }
    }
    public class ScrollOfFireball : Item
    {
        int DamageAmount = 2;

        private Texture2D FireBall;
        public ScrollOfFireball()
        {
            Name = "Scroll of Fireball";
        }
        public override void OnAddedToScene()
        {
            base.OnAddedToScene();
            LoadTexture("Fireball");
        }

        public override void Use(Actor actor)
        {
            actor.healthSystem.TakeDamage(DamageAmount);
            Owner.EndTurn(); 
        }
    }

    public class ScrollOfLightning: Item
    {
        int DamageAmount = 2;

        private Texture2D Lightning;
        public ScrollOfLightning()
        {
            Name = "Scroll of Lightining";
        }
        public override void OnAddedToScene()
        {
            base.OnAddedToScene();
            LoadTexture("Lightning");
        }

        public override void Use(Actor actor)
        {
            actor.healthSystem.TakeDamage(DamageAmount);
            Owner.EndTurn();
        }
    }

}
