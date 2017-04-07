using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Asteroid_Belt_Assault
{
    public static class SoundManager
    {
        private static List<SoundEffect> explosions = new
            List<SoundEffect>();
        private static int explosionCount = 2;

        private static SoundEffect playerShot;
        private static SoundEffect enemyShot;

        private static SoundEffect slave1gun, slave1seismic;

        private static Random rand = new Random();

        public static Microsoft.Xna.Framework.Media.Song titleSong, gameSong;

        public static void Initialize(ContentManager content)
        {
            try
            {
                playerShot = content.Load<SoundEffect>(@"Sounds\Shot1");
                enemyShot = content.Load<SoundEffect>(@"Sounds\Shot2");
                slave1gun = content.Load<SoundEffect>(@"Sounds\Slave1-Guns");
                slave1seismic = content.Load<SoundEffect>(@"Sounds\Slave1-Seismic");

                playerShot = slave1gun;

                for (int x = 1; x <= explosionCount; x++)
                {
                    explosions.Add(
                        content.Load<SoundEffect>(@"Sounds\Explosion" +
                            x.ToString()));
                }
                explosions.Add(slave1gun);
                explosions.Add(slave1seismic);

                titleSong = content.Load<Song>(@"Sounds\Lil Bow Wow - Basketball");
                gameSong = content.Load<Song>(@"Sounds\Space Jam Theme Song");
            }
            catch
            {
                Debug.Write("SoundManager Initialization Failed");
            }
        }

        
        public static void PlaySong(Song titleSong)
        {
            if (titleSong != null)
            {
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(titleSong);
                    MediaPlayer.Volume = 1.0f;
                }
            }
        }

        public static void StopSong()
        {
            MediaPlayer.Stop();
        }

        public static void PlayExplosion(int explosion)
        {
            try
            {
                explosions[explosion % explosions.Count].Play();
            }
            catch
            {
                Debug.Write("PlayExplosion Failed");
            }
        }

        public static void PlayExplosion()
        {
            try
            {
                explosions[rand.Next(1, explosionCount)].Play();
            }
            catch
            {
                Debug.Write("PlayExplosion Failed");
            }
        }

        public static void PlayPlayerShot()
        {
            try
            {
                playerShot.Play();
            }
            catch
            {
                Debug.Write("PlayPlayerShot Failed");
            }
        }

        public static void PlayEnemyShot()
        {
            try
            {
                enemyShot.Play();
            }
            catch
            {
                Debug.Write("PlayEnemyShot Failed");
            }
        }

    }
}