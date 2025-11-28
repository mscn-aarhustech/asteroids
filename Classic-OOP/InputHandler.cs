using Raylib_cs;

namespace Classic_OOP
{
    public class InputHandler
    {
        // We pass the Game and Ship instances to the handler so it can command them
        public void HandleInput(Game game, Ship ship, float dt)
        {
            // --- Global Game Controls ---
            if (game.IsGameOver)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                    game.Restart();
                }
                return; // Stop processing ship inputs if game is over
            }

            // --- Ship Movement Controls ---
            if (ship != null)
            {
                // Rotation
                if (Raylib.IsKeyDown(KeyboardKey.Left))
                {
                    ship.Rotate(-1, dt); // -1 for left
                }
                if (Raylib.IsKeyDown(KeyboardKey.Right))
                {
                    ship.Rotate(1, dt);  // 1 for right
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

                // Shooting (Command sent to Game, as Game manages the bullet list)
                if (Raylib.IsKeyPressed(KeyboardKey.Space))
                {
                    game.SpawnBullet();
                }
            }
        }
    }
}