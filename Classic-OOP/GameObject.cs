using System.Numerics;
using Raylib_cs;

namespace Classic_OOP
{
    public abstract class GameObject
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Size;
        public float Rotation;
        public Color Color;
        public bool Active = true;

        protected GameObject(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
            Color = Color.White;
        }

        public abstract void Update(float deltaTime);
        public abstract void Draw();
    
        protected void ScreenWrap(int screenWidth, int screenHeight)
        {
            if (Position.X > screenWidth) Position.X = 0;
            else if (Position.X < 0) Position.X = screenWidth;

            if (Position.Y > screenHeight) Position.Y = 0;
            else if (Position.Y < 0) Position.Y = screenHeight;
        }
    }
}