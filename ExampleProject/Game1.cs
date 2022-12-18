using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarshotEngine;

namespace ExampleProject
{
    public class Game1 : Main 
    {

        protected override void LoadContent()
        {
            base.LoadContent();
            LoadScene("GameplayScene");
        }
    }
}
