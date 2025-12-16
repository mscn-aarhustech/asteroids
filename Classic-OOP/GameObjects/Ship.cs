using System;
using System.Numerics;
using Raylib_cs;

namespace Classic_OOP.GameObjects
{
    public class Ship : GameObject
    {
        private const float RotationSpeed = 400.0f;
        private const float Acceleration = 400.0f;
        private const float Brake = 0.95f;
        private const float Friction = 0.995f;

        public Ship(Vector2 startPos) : base(startPos, Vector2.Zero)
        {
            Size = new Vector2(20, 20);
        }

        public override void Update(float dt)
        {
            Position += Velocity * dt;
            //Velocity *= Friction; // Drag
            ScreenWrap(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        }

        // --- Commands (Called by InputHandler) ---

        public void Rotate(float direction, float dt)
        {
            Rotation += (RotationSpeed * direction) * dt;
        }

        public void ApplyThrust(float dt)
        {
            float radians = Rotation * (MathF.PI / 180.0f);
            Vector2 thrust = new Vector2(MathF.Sin(radians), -MathF.Cos(radians)) * Acceleration * dt;
            Velocity += thrust;
        }

        public void ApplyRetroThrust(float dt)
        {
            Velocity *= Brake;
        }

        public override void Draw()
        {
            Vector2 v1 = Position + RotatePoint(new Vector2(0, -Size.Y), Rotation);
            Vector2 v2 = Position + RotatePoint(new Vector2(-Size.X / 2, Size.Y / 2), Rotation);
            Vector2 v3 = Position + RotatePoint(new Vector2(Size.X / 2, Size.Y / 2), Rotation);

            Raylib.DrawTriangleLines(v1, v2, v3, Color);
        }

        private Vector2 RotatePoint(Vector2 point, float angleDeg)
        {
            float rad = angleDeg * (MathF.PI / 180.0f);
            float cos = MathF.Cos(rad);
            float sin = MathF.Sin(rad);
            return new Vector2(
                point.X * cos - point.Y * sin,
                point.X * sin + point.Y * cos
            );
        }
    }
}