using System;
using System.Collections.Generic;
using System.Numerics;
using Classic_OOP.GameObjects;
using Raylib_cs;

namespace Classic_OOP
{
    public class Game
    {
        private Ship _ship;
        private readonly List<Asteroid> _asteroids;
        private readonly List<Bullet> _bullets;
        private readonly Random _rnd;
        private readonly InputHandler _input; // Reference to the controller

        private const int ScreenWidth = 800;
        private const int ScreenHeight = 800;

        // Expose state safely to InputHandler
        public bool IsGameOver { get; private set; }

        public Game()
        {
            _asteroids = new List<Asteroid>();
            _bullets = new List<Bullet>();
            _rnd = new Random();
            _input = new InputHandler(); // Initialize Handler
        }

        public void Run()
        {
            Raylib.InitWindow(ScreenWidth, ScreenHeight, "Asteroids - OOP");
            Raylib.SetTargetFPS(60);

            ResetGame();

            while (!Raylib.WindowShouldClose())
            {
                float dt = Raylib.GetFrameTime();

                // 1. Process Input
                _input.HandleInput(this, _ship, dt);

                // 2. Update Logic
                Update(dt);

                // 3. Draw
                Draw();
            }

            Raylib.CloseWindow();
        }

        // Made public so InputHandler can call it
        public void Restart()
        {
            ResetGame();
        }

        private void ResetGame()
        {
            _ship = new Ship(new Vector2(ScreenWidth / 2, ScreenHeight / 2));
            _asteroids.Clear();
            _bullets.Clear();
            IsGameOver = false;

            for (int i = 0; i < 5; i++)
            {
                SpawnAsteroid(AsteroidSize.Large, GetRandomEdgePosition());
            }
        }

        private void Update(float dt)
        {
            if (IsGameOver) return;

            // Note: Ship.Update only handles physics now
            _ship.Update(dt);

            foreach (var asteroid in _asteroids) asteroid.Update(dt);
            foreach (var bullet in _bullets) bullet.Update(dt);

            CheckCollisions();

            _asteroids.RemoveAll(a => !a.Active);
            _bullets.RemoveAll(b => !b.Active);
        }

        // Made public (or internal) so InputHandler can trigger shooting
        public void SpawnBullet()
        {
            // Prevent shooting if ship is dead or game is over
            if (IsGameOver) return;

            float radians = _ship.Rotation * (MathF.PI / 180.0f);
            Vector2 direction = new Vector2(MathF.Sin(radians), -MathF.Cos(radians));
            Vector2 velocity = direction * 500.0f;

            _bullets.Add(new Bullet(_ship.Position + direction * 20, velocity));
        }

        // ... (Remaining methods: Draw, SpawnAsteroid, CheckCollisions, etc. are unchanged) ...

        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            if (IsGameOver)
            {
                Raylib.DrawText("GAME OVER", ScreenWidth / 2 - 100, ScreenHeight / 2, 40, Color.Red);
                Raylib.DrawText("Press ENTER to Restart", ScreenWidth / 2 - 120, ScreenHeight / 2 + 50, 20, Color.Gray);
            }
            else
            {
                _ship.Draw();
                foreach (var asteroid in _asteroids) asteroid.Draw();
                foreach (var bullet in _bullets) bullet.Draw();
                Raylib.DrawText($"Asteroids: {_asteroids.Count}", 10, 10, 20, Color.Green);
            }

            Raylib.EndDrawing();
        }

        private void SpawnAsteroid(AsteroidSize size, Vector2 pos)
        {
            float speed = _rnd.Next(50, 150);
            float angle = _rnd.Next(0, 360) * (MathF.PI / 180.0f);
            Vector2 vel = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed;
            _asteroids.Add(new Asteroid(pos, vel, size));
        }

        private void CheckCollisions()
        {
            // ... (Same collision logic as previous) ...
            foreach (var bullet in _bullets)
            {
                foreach (var asteroid in _asteroids)
                {
                    if (!bullet.Active || !asteroid.Active) continue;
                    float dist = Vector2.Distance(bullet.Position, asteroid.Position);
                    if (dist < asteroid.Radius)
                    {
                        bullet.Active = false;
                        asteroid.Active = false;
                        HandleAsteroidSplit(asteroid);
                        break;
                    }
                }
            }

            foreach (var asteroid in _asteroids)
            {
                if (!asteroid.Active) continue;
                float dist = Vector2.Distance(_ship.Position, asteroid.Position);
                if (dist < asteroid.Radius + 15) IsGameOver = true;
            }
        }

        private void HandleAsteroidSplit(Asteroid parent)
        {
            if (parent.SizeCategory == AsteroidSize.Large)
            {
                SpawnAsteroid(AsteroidSize.Medium, parent.Position);
                SpawnAsteroid(AsteroidSize.Medium, parent.Position);
            }
            else if (parent.SizeCategory == AsteroidSize.Medium)
            {
                SpawnAsteroid(AsteroidSize.Small, parent.Position);
                SpawnAsteroid(AsteroidSize.Small, parent.Position);
            }
        }

        private Vector2 GetRandomEdgePosition()
        {
            if (_rnd.Next(0, 2) == 0)
                return new Vector2(_rnd.Next(0, ScreenWidth), _rnd.Next(0, 2) == 0 ? 0 : ScreenHeight);
            else
                return new Vector2(_rnd.Next(0, 2) == 0 ? 0 : ScreenWidth, _rnd.Next(0, ScreenHeight));
        }
    }
}