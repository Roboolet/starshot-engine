using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace StarshotEngine.Rendering
{
    public enum ScreenAnchor
    {
        LTop, CTop, RTop,
        LMid, CMid, RMid,
        LBot, CBot, RBot
    }

    public class ScreenspaceScaler : SceneObjectComponent
    {
        SceneCamera camera;

        public float[] targetScale;

        Vector2 anchorPos;
        ScreenAnchor anchor;
        string _screenAnchor;
        public string screenAnchor
        {
            get { return _screenAnchor; }
            set
            {
                _screenAnchor = value;
                SetAnchor(value);
            }
        }

        public void SetAnchor(string anch)
        {
            if (Enum.TryParse(screenAnchor, out ScreenAnchor val))
            {
                SetAnchor(val);
            }
        }

        public void SetAnchor(ScreenAnchor anch)
        {
            anchor = anch;
            switch (anchor)
            {
                case ScreenAnchor.LTop: anchorPos = Vector2.Zero; break;
                case ScreenAnchor.CTop: anchorPos = new Vector2(0.5f, 0f); break;
                case ScreenAnchor.RTop: anchorPos = new Vector2(1f, 0f); break;

                case ScreenAnchor.LMid: anchorPos = new Vector2(0f, 0.5f); break;
                case ScreenAnchor.CMid: anchorPos = new Vector2(0.5f, 0.5f); break;
                case ScreenAnchor.RMid: anchorPos = new Vector2(1f, 0.5f); break;

                case ScreenAnchor.LBot: anchorPos = new Vector2(0f, 1f); break;
                case ScreenAnchor.CBot: anchorPos = new Vector2(0.5f, 1f); break;
                case ScreenAnchor.RBot: anchorPos = Vector2.One; break;
            }
        }

        protected override void OnStart()
        {
            camera = Main.instance.currentScene.camera;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            sceneObject.Position = camera.ScreenToWorldPos(anchorPos.X, anchorPos.Y);
            Debug.WriteLine(sceneObject.Position);
        }
    }
}
