using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace Game_Of_Throws
{
    public class Projectile
    {
        #region Properties

        public Rectangle Hitbox
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }
        
        #endregion

        #region Ctor

        public Projectile(Rectangle rectHitbox, Texture2D txTexture)
        {
            this.Hitbox = rectHitbox;
            this.Texture = txTexture;
        }

        #endregion
    }
    public class ProjectileHandler
    {
        #region Properties

        public ContentManager ContentManager
        {
            get;
            set;
        }

        public List<Projectile> ActiveProjectiles
        {
            get;
            set;
        }
        
        #endregion

        #region Ctor

        public ProjectileHandler(ContentManager cntManager)
        {
            this.ActiveProjectiles = new List<Projectile>();
            this.ContentManager = cntManager;
        }
        
        #endregion

        public void Update(BinaryReader brReader)
        {
            ActiveProjectiles.Clear();

            int amountOfProjectiles = brReader.ReadInt32();

            for (int i = 0; i < amountOfProjectiles; i++)
            {
                int width = brReader.ReadInt32(),
                    height = brReader.ReadInt32(),
                    x = brReader.ReadInt32(),
                    y = brReader.ReadInt32();

                Direction dir = (Direction)brReader.ReadByte();
                string textureName = "arrow" + Enum.GetName(typeof(Direction), dir);

                Projectile prjCurr =
                    new Projectile(new Rectangle(x, y, width, height),
                                   this.ContentManager.Load<Texture2D>("Items/Textures/" + textureName));
                this.ActiveProjectiles.Add(prjCurr);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.CameraMatrix);

            for (int i = 0; i < this.ActiveProjectiles.Count; i++)
            {
                Projectile prjCurr = this.ActiveProjectiles[i];
                spriteBatch.Draw(prjCurr.Texture, prjCurr.Hitbox, Color.White);
            }

            spriteBatch.End();
        }
    }
}
