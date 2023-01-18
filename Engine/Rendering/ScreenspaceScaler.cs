using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarshotEngine.Rendering
{
    enum ScreenAnchor
    {
        LTop, CTop, RTop,
        LMid, CMid, RMid,
        LBot, CBot, RBot
    }

    public class ScreenspaceScaler : SceneObjectComponent
    {
        public float[] targetScale;
        public float[] offset;

        ScreenAnchor anchor;
        string _screenAnchor;
        public string screenAnchor
        {
            get { return _screenAnchor; }
            set
            {
                _screenAnchor = value;
                if (Enum.TryParse(screenAnchor, out ScreenAnchor val))
                {
                    anchor = val;
                }
            }
        }

        protected override void OnCreate()
        {
           
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
        }
    }
}
