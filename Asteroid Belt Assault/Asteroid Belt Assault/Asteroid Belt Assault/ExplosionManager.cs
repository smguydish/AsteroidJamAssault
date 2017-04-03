using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Belt_Assault
{
    class ExplosionManager
    {
        private Texture2D explodeSheet;
        private List<Rectangle> explodeRectangles = new List<Rectangle>();

        private Texture2D explodeSheet2;
        private List<Rectangle> explodeRectangles2 = new List<Rectangle>();

        private Texture2D texture;
        private List<Rectangle> pieceRectangles = new List<Rectangle>();
        private Rectangle pointRectangle;

        private int minPieceCount = 3;
        private int maxPieceCount = 6;
        private int minPointCount = 200;
        private int maxPointCount = 300;

        private int durationCount = 90;
        private float explosionMaxSpeed = 60f;

        private float pieceSpeedScale = 6f;
        private int pointSpeedMin = 15;
        private int pointSpeedMax = 90;

        private Color initialColor = new Color(1.0f, 0.3f, 0f) * 0.5f;
        private Color finalColor = new Color(0f, 0f, 0f, 0f);

        Random rand = new Random();

        private List<Particle> ExplosionParticles = new List<Particle>();

        public ExplosionManager(
            Texture2D texture,
            Rectangle initialFrame,
            int pieceCount,
            Rectangle pointRectangle,
            Texture2D explodesheet,
            Texture2D explodesheet2)
        {
            this.texture = texture;
            this.explodeSheet = explodesheet;
            this.explodeSheet2 = explodesheet2;
            for (int x = 0; x < pieceCount; x++)
            {
                pieceRectangles.Add(new Rectangle(
                    initialFrame.X + (initialFrame.Width * x),
                    initialFrame.Y,
                    initialFrame.Width,
                    initialFrame.Height));
            }
            this.pointRectangle = pointRectangle;

            for (int y = 0; y < 6; y++)
                for (int x = 0; x < 5; x++)
                {
                    // 192
                    explodeRectangles.Add(new Rectangle(
                            x * 192, y * 192,
                            192, 192
                        ));
                }

            for (int x = 0; x < 18; x++)
            {
                explodeRectangles2.Add(new Rectangle(
                        x * 50, 0,
                        50, 128
                    ));
            }
        }

        public Vector2 randomDirection(float scale)
        {
            Vector2 direction;
            do
            {
                direction = new Vector2(
                rand.Next(0, 101) - 50,
                rand.Next(0, 101) - 50);
            } while (direction.Length() == 0);
            direction.Normalize();
            direction *= scale;

            return direction;
        }

        public void AddExplosion(Vector2 location, Vector2 momentum)
        {
            AddExplosion(location, momentum, 0);
        }

        public void AddExplosion(Vector2 location, Vector2 momentum, int type) // type: 0 = asteroid, 1 = player, 2 = enemy
        {
            Vector2 pieceLocation = location -
                new Vector2(pieceRectangles[0].Width / 2,
                    pieceRectangles[0].Height / 2);

            int pieces = rand.Next(minPieceCount, maxPieceCount + 1);
            for (int x = 0; x < pieces; x++)
            {
                ExplosionParticles.Add(new Particle(
                    pieceLocation,
                    texture,
                    pieceRectangles[rand.Next(0, pieceRectangles.Count)],
                    randomDirection(pieceSpeedScale) + momentum,
                    Vector2.Zero,
                    explosionMaxSpeed,
                    durationCount,
                    initialColor,
                    finalColor));
            }

            if (type == 1)
            {
                Particle p = new Particle(
                        pieceLocation - new Vector2(192 / 2 - 20, 192 / 2 - 20),
                        explodeSheet,
                        explodeRectangles[0],
                        Vector2.Zero,
                        Vector2.Zero,
                        explosionMaxSpeed,
                        durationCount,
                        Color.White,
                        Color.White);

                p.PlayOnce = true;
                p.FrameTime = .05f;

                for (int i = 1; i < explodeRectangles.Count; i++)
                    p.AddFrame(explodeRectangles[i]);

                ExplosionParticles.Add(
                    p
                    );
            }
            else if (type == 2)
            {
                Particle p = new Particle(
                        pieceLocation - new Vector2(explodeRectangles2[0].Width / 2 - 20, explodeRectangles2[0].Height / 2 - 20),
                        explodeSheet2,
                        explodeRectangles2[0],
                        Vector2.Zero,
                        Vector2.Zero,
                        explosionMaxSpeed,
                        durationCount,
                        Color.White,
                        Color.White);

                p.PlayOnce = true;
                p.FrameTime = .05f;

                for (int i = 1; i < explodeRectangles2.Count; i++)
                    p.AddFrame(explodeRectangles2[i]);

                ExplosionParticles.Add(
                    p
                    );
            }

            int points = rand.Next(minPointCount, maxPointCount + 1);
            for (int x = 0; x < points; x++)
            {
                ExplosionParticles.Add(new Particle(
                    location,
                    texture,
                    pointRectangle,
                    randomDirection((float)rand.Next(
                        pointSpeedMin, pointSpeedMax)) + momentum,
                    Vector2.Zero,
                    explosionMaxSpeed,
                    durationCount,
                    initialColor,
                    finalColor));

            }
            SoundManager.PlayExplosion();
        }

        public void Update(GameTime gameTime)
        {
            for (int x = ExplosionParticles.Count - 1; x >= 0; x--)
            {
                if (ExplosionParticles[x].IsActive)
                {
                    ExplosionParticles[x].Update(gameTime);
                }
                else
                {
                    ExplosionParticles.RemoveAt(x);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in ExplosionParticles)
            {
                particle.Draw(spriteBatch);
            }
        }

    }
}
