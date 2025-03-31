using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez.Sprites;
using Nez;
using Nez.Textures;
using System.Linq;
using Nez.Tweens;
using System.Linq.Expressions;

namespace MonoGame
{
    public class Actor : Entity
    {
        public SpriteRenderer spriteRenderer;
        public Entity TurnManager;
        public bool isTurn;
        public bool WaitForTurn;
        public bool WaitAnimation;

        public Texture2D entityTexture;
        public Vector2 startPosition;

        public Map map;

        public HealthSystem healthSystem; 
        public Actor(Vector2 vector2)
        {
            startPosition = vector2;
            AddComponent(new HealthSystem());
            
        }

        public override void OnAddedToScene()
        {

            map = Scene.EntitiesOfType<Map>().FirstOrDefault();
            //map.addTile(playerTexture, Transform.Position);
        }

        public void LoadTexture(string textureName)
        {
            entityTexture = Scene.Content.Load<Texture2D>(textureName);
            SpriteRenderer tileRenderer = new SpriteRenderer(entityTexture);
            tileRenderer.SetOrigin(Position);
            tileRenderer.SetLayerDepth(0);
            //Debug.Log(startPosition);
            //Debug.Log(startPosition / 16);
            AddComponent(tileRenderer);
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual void StartTurn()
        {
            //Debug.Log("Started turn");
            isTurn = true;
            WaitForTurn = false;
        }

        public virtual void EndTurn()
        {
            //Debug.Log("Ended turn");
            isTurn = false;
            WaitForTurn = true;
        }

        public virtual void UpdateTurn()
        {
            if (WaitAnimation)
            {
                StartTurn();
            }
            else
            {
                EndTurn();
            }
        }

        public virtual void Attack(Actor actor)
        {
            actor.healthSystem.TakeDamage(1);
            EndTurn();
        }

        public void Move(Vector2 targetPosition)
        {
            if (Position != targetPosition)
            {
                Debug.Log(Name);
                WaitAnimation = true;
                Vector2 MoveVector = targetPosition;
                // moving to the move vector, how long the action is. then what to do after its done
                this.TweenPositionTo(MoveVector, 1.20f).SetCompletionHandler(action =>{WaitAnimation = false; UpdateTurn();}).Start();
            }
        }
    }
}
