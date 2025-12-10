using System.Numerics;
using Raylib_cs;

namespace Classic_OOP.GameObjects
{
    public enum AsteroidSize { Small, Medium, Large }

    public class Asteroid : GameObject
    {
        public AsteroidSize SizeCategory;
        public float Radius;

        public Asteroid(Vector2 position, Vector2 velocity, AsteroidSize size) : base(position, velocity)
        {
            SizeCategory = size;

            switch (size)
            {
                case AsteroidSize.Large: Radius = 40; break;
                case AsteroidSize.Medium: Radius = 20; break;
                case AsteroidSize.Small: Radius = 10; break;
            }
        }

        public override void Update(float dt)
        {
            Position += Velocity * dt;
            Rotation += 30 * dt;
            ScreenWrap(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        }

        public override void Draw()
        {
            Raylib.DrawPolyLines(Position, 6, Radius, Rotation, Color);
        }
    }
}