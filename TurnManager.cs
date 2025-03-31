using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez;
using Nez.BitmapFonts;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame
{
    public class TurnManager : Entity
    {
        public TurnBasedSystem turnSystem;

        public TextComponent textComponent;

        public TurnManager()
        {
            AddComponent(new TurnBasedSystem());
            turnSystem = GetComponent<TurnBasedSystem>();
        }
        public override void OnAddedToScene()
        {

            
        }

    }
}
