using System.Numerics;
using Raylib_cs;

namespace Classic_OOP
{
    public class Bullet : GameObject
    {
        private float _lifeTime = 2.0f;

        public Bullet(Vector2 position, Vector2 velocity) : base(position, velocity)
        {
        }

        public override void Update(float dt)
        {
            Position += Velocity * dt;
            _lifeTime -= dt;

            if (_lifeTime <= 0) Active = false;

            if (Position.X < 0 || Position.X > Raylib.GetScreenWidth() ||
                Position.Y < 0 || Position.Y > Raylib.GetScreenHeight())
            {
                Active = false;
            }
        }

        public override void Draw()
        {
            Raylib.DrawCircleV(Position, 2, Color);
        }
    }
}