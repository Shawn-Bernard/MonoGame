using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez.Sprites;
using Nez;
using Nez.Textures;
namespace MonoGame
{
    public class Game1 : Core
    {

        public Game1() : base()
        {
            //_graphics = new GraphicsDeviceManager(this);
            //Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            //Test

            // Making a new scene with default renderer for background
            Scene scene = Scene.CreateWithDefaultRenderer(Color.Gray);
            scene.ClearColor = Color.Black;

            TurnManager turnManager = new TurnManager();



            scene.AddEntity(turnManager);
            Map map = new Map();

            
            scene.AddEntity(map);
            Scene = scene;
        }

        // Player Movement Component

    }
}