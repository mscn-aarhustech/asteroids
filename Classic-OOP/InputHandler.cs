using Classic_OOP.GameObjects;
using Raylib_cs;

namespace Classic_OOP
{
    public class InputHandler
    {
        public void HandleInput(Game game, Ship ship, float dt)
        {
            if (game.IsGameOver)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                    game.Restart();
                }
                return;
            }

            if (ship != null)
            {
                // Rotation
                if (Raylib.IsKeyDown(KeyboardKey.Left))
                {
                    ship.Rotate(-1, dt);
                }
                if (Raylib.IsKeyDown(KeyboardKey.Right))
                {
                    ship.Rotate(1, dt);
                }

                // Thrust
                if (Raylib.IsKeyDown(KeyboardKey.Up))
                {
                    ship.ApplyThrust(dt);
                }
                if (Raylib.IsKeyDown(KeyboardKey.Down))
                {
                    ship.ApplyRetroThrust(dt);
                }

                // Shooting
                if (Raylib.IsKeyPressed(KeyboardKey.Space))
                {
                    game.SpawnBullet();
                }
            }
        }
    }
}