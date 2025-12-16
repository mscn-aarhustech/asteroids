using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Classic_OOP.GameObjects
{
    public class Particle : GameObject
    {
        private float _lifeTime = 2.0f;

        public Particle(Vector2 position, Vector2 velocity) : base(position, velocity)
        {
        }

        public override void Draw()
        {
            Raylib.DrawCircleV(Position, 1, Color);
        }

        public override void Update(float dt)
        {
            Position += Velocity * dt;
            ScreenWrap(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

            // Prevent screen wrapping for bullets
            //if (Position.X < 0 || Position.X > Raylib.GetScreenWidth() ||
            //    Position.Y < 0 || Position.Y > Raylib.GetScreenHeight())
            //{
            //    Active = false;
            //}

            _lifeTime -= dt;
            if (_lifeTime <= 0) Active = false;
        }
    }
}
