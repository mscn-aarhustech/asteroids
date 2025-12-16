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
        private readonly List<Particle> _particles;
        private readonly Random _rnd;
        private readonly InputHandler _input;

        private const int ScreenWidth = 800;
        private const int ScreenHeight = 800;

        public bool IsGameOver { get; private set; }

        public Game()
        {
            _asteroids = new List<Asteroid>();
            _bullets = new List<Bullet>();
            _particles = new List<Particle>();
            _rnd = new Random();
            _input = new InputHandler();
        }

        public void Run()
        {
            Raylib.InitWindow(ScreenWidth, ScreenHeight, "Asteroids - OOP");
            Raylib.SetTargetFPS(60);

            ResetGame();

            while (!Raylib.WindowShouldClose())
            {
                float dt = Raylib.GetFrameTime();

                _input.HandleInput(this, _ship, dt);

                Update(dt);

                Draw();
            }

            Raylib.CloseWindow();
        }

        public void Restart()
        {
            ResetGame();
        }

        private void ResetGame()
        {
            _ship = new Ship(new Vector2(ScreenWidth / 2, ScreenHeight / 2));
            _asteroids.Clear();
            _bullets.Clear();
            _particles.Clear();

            IsGameOver = false;

            for (int i = 0; i < 5; i++)
            {
                SpawnAsteroid(AsteroidSize.Large, GetRandomEdgePosition(), new Vector2());
            }
        }

        private void Update(float dt)
        {
            if (IsGameOver) return;

            _ship.Update(dt);

            foreach (var asteroid in _asteroids) asteroid.Update(dt);
            foreach (var bullet in _bullets) bullet.Update(dt);
            foreach (var particle in _particles) particle.Update(dt);

            CheckCollisions();

            _asteroids.RemoveAll(a => !a.Active);
            _bullets.RemoveAll(b => !b.Active);
            _particles.RemoveAll(p => !p.Active);
        }

        public void SpawnBullet()
        {
            if (IsGameOver) return;

            float radians = _ship.Rotation * (MathF.PI / 180.0f);
            Vector2 direction = new Vector2(MathF.Sin(radians), -MathF.Cos(radians));
            Vector2 velocity = direction * 500.0f;

            _bullets.Add(new Bullet(_ship.Position + direction * 20, _ship.Velocity + velocity));
        }

        public void SpawnParticles(Vector2 position, Vector2 velocity, int count)
        {
            for (int i = 0; i < count; i++)
            {
                float angle = _rnd.Next(0, 360) * (MathF.PI / 180.0f);
                float speed = _rnd.Next(5, 75);
                Vector2 vel = velocity + new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed;
                _particles.Add(new Particle(position, vel));
            }
        }

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
                foreach (var particle in _particles) particle.Draw();
                Raylib.DrawText($"Asteroids: {_asteroids.Count}", 10, 10, 20, Color.Green);
            }

            Raylib.EndDrawing();
        }

        private void SpawnAsteroid(AsteroidSize size, Vector2 pos, Vector2 vel)
        {
            float speed = _rnd.Next(20, 70);
            float angle = _rnd.Next(0, 360) * (MathF.PI / 180.0f);
            float angularVelocity = _rnd.Next(-50, 50);
            Vector2 velocity = vel + new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed;
            _asteroids.Add(new Asteroid(pos, velocity, angularVelocity, size));
        }

        private void CheckCollisions()
        {
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
                SpawnAsteroid(AsteroidSize.Medium, parent.Position, parent.Velocity);
                SpawnAsteroid(AsteroidSize.Medium, parent.Position, parent.Velocity);
                SpawnParticles(parent.Position, parent.Velocity, 10);
            }
            else if (parent.SizeCategory == AsteroidSize.Medium)
            {
                SpawnAsteroid(AsteroidSize.Small, parent.Position, parent.Velocity);
                SpawnAsteroid(AsteroidSize.Small, parent.Position, parent.Velocity);
                SpawnParticles(parent.Position, parent.Velocity, 10);
            }
            else if (parent.SizeCategory == AsteroidSize.Small)
            {
                SpawnParticles(parent.Position, parent.Velocity, 10);
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