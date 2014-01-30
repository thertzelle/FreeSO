﻿/*The contents of this file are subject to the Mozilla Public License Version 1.1
(the "License"); you may not use this file except in compliance with the
License. You may obtain a copy of the License at http://www.mozilla.org/MPL/

Software distributed under the License is distributed on an "AS IS" basis,
WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
the specific language governing rights and limitations under the License.

The Original Code is the TSOClient.

The Initial Developer of the Original Code is
Mats 'Afr0' Vederhus. All Rights Reserved.

Contributor(s): ______________________________________.
*/

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TSOClient.Code;

namespace TSOClient.ThreeD
{
    /// <summary>
    /// Manages ThreeDScene instances.
    /// </summary>
    public class SceneManager
    {
        private List<ThreeDAbstract> m_Scenes = new List<ThreeDAbstract>();

        private Game m_Game;

        private Matrix m_WorldMatrix, m_ViewMatrix, m_ProjectionMatrix;

        public List<ThreeDAbstract> Scenes
        {
            get { return m_Scenes; }
        }

        /// <summary>
        /// The graphicsdevice that is part of the game instance.
        /// Used when calling XNA's graphic functions.
        /// </summary>
        public GraphicsDevice Device
        {
            get { return m_Game.GraphicsDevice; }
        }

        /// <summary>
        /// A worldmatrix, used to display 3D objects (sims).
        /// Initialized in the ScreenManager's constructor.
        /// </summary>
        public Matrix WorldMatrix
        {
            get { return m_WorldMatrix; }
            set { m_WorldMatrix = value; }
        }

        /// <summary>
        /// A viewmatrix, used to display 3D objects (sims).
        /// Initialized in the ScreenManager's constructor.
        /// </summary>
        public Matrix ViewMatrix
        {
            get { return m_ViewMatrix; }
            set { m_WorldMatrix = value; }
        }

        /// <summary>
        /// A projectionmatrix, used to display 3D objects (sims).
        /// Initialized in the ScreenManager's constructor.
        /// </summary>
        public Matrix ProjectionMatrix
        {
            get { return m_ProjectionMatrix; }
            set { m_ProjectionMatrix = value; }
        }

        public SceneManager(Game G)
        {
            m_Game = G;

            Device.RenderState.CullMode = CullMode.None;

            m_WorldMatrix = Matrix.Identity;
            m_ViewMatrix = Matrix.CreateLookAt(Vector3.Backward * 5, Vector3.Zero, Vector3.Right);

            m_ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 4.0f,
                    (float)Device.PresentationParameters.BackBufferWidth /
                    (float)Device.PresentationParameters.BackBufferHeight,
                    1.0f, 10000.0f);

            Device.DeviceReset += new EventHandler(Device_DeviceReset);
        }

        private void Device_DeviceReset(object sender, EventArgs e)
        {
            GameFacade.Game.Content.Unload();
            Reload();

            for (int i = 0; i < m_Scenes.Count; i++)
                m_Scenes[i].DeviceReset(Device);

            Device.RenderState.CullMode = CullMode.None;

            m_WorldMatrix = Matrix.Identity;
            m_ViewMatrix = Matrix.CreateLookAt(Vector3.Backward * 5, Vector3.Zero, Vector3.Right);

            m_ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 4.0f,
                    (float)Device.PresentationParameters.BackBufferWidth /
                    (float)Device.PresentationParameters.BackBufferHeight,
                    1.0f, 10000.0f);
        }

        /// <summary>
        /// TODO: This should probably be moved elsewhere...
        /// </summary>
        private void Reload()
        {
            GameFacade.MainFont = new TSOClient.Code.UI.Framework.Font();
            GameFacade.MainFont.AddSize(10, GameFacade.Game.Content.Load<SpriteFont>("Fonts/ProjectDollhouse_10px"));
            GameFacade.MainFont.AddSize(12, GameFacade.Game.Content.Load<SpriteFont>("Fonts/ProjectDollhouse_12px"));
            GameFacade.MainFont.AddSize(14, GameFacade.Game.Content.Load<SpriteFont>("Fonts/ProjectDollhouse_14px"));
            GameFacade.MainFont.AddSize(16, GameFacade.Game.Content.Load<SpriteFont>("Fonts/ProjectDollhouse_16px"));
        }

        public List<ThreeDAbstract> ExternalScenes = new List<ThreeDAbstract>();

        /// <summary>
        /// Adds a ThreeDScene instance to the scene manager but the scene manager will not render
        /// this scene. It is only added so it can be included in the debug panel
        /// </summary>
        /// <param name="Scene"></param>
        public void AddExternalScene(ThreeDAbstract Scene)
        {
            ExternalScenes.Add(Scene);
        }

        /// <summary>
        /// Adds a ThreeDScene instance to this SceneManager instance's list of scenes.
        /// </summary>
        /// <param name="Scene">The ThreeDScene instance to add.</param>
        public void AddScene(ThreeDAbstract Scene)
        {
            m_Scenes.Add(Scene);
        }

        public void AddSceneAtStart(ThreeDAbstract Scene)
        {
            m_Scenes.Insert(0, Scene);
        }

        /// <summary>
        /// Removes a scene from this SceneManager instances' list of scenes.
        /// </summary>
        /// <param name="Scene">The ThreeDScene instance to remove.</param>
        public void RemoveScene(ThreeDAbstract Scene)
        {
            m_Scenes.Remove(Scene);
        }

        public void Update(GameTime Time)
        {
            for(int i = 0; i < m_Scenes.Count; i++)
            {
                m_Scenes[i].Update(Time);
            }
        }

        public void Draw()
        {
            var device = GameFacade.GraphicsDevice;

            for (int i = 0; i < m_Scenes.Count; i++)
            {
                m_Scenes[i].Draw(device);
            }
        }
    }
}
