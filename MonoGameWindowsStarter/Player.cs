using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace MonoGameWindowsStarter
{
    enum PlayerAnimationState
    {
        Idle,
        MovingLeft,
        MovingRight,
        MovingUp,
        MovingDown,
        Dead
    }

    public class Player
    {
        const int FRAME_RATE = 300;

        Sprite[] frames;
        int currentFrame = 0;

        PlayerAnimationState animationState = PlayerAnimationState.Idle;

        int speed = 3;

        TimeSpan animationTimer;
        SpriteEffects spriteEffects = SpriteEffects.None;

        Color color = Color.White;

        Vector2 origin = new Vector2(15, 33);
        public Vector2 Position = new Vector2(50, 100);
        public BoundingRectangle Bounds => new BoundingRectangle(Position - 1.8f * origin, 34, 34);

        /// <summary>
        /// Constructs a new player
        /// </summary>
        /// <param name="frames">The sprite frames associated with the player</param>
        public Player(IEnumerable<Sprite> frames)
        {
            this.frames = frames.ToArray();
            animationState = PlayerAnimationState.MovingLeft;
        }

        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();


            // Horizontal movement
            if (keyboard.IsKeyDown(Keys.Left))
            {
                animationState = PlayerAnimationState.MovingLeft;
                Position.X -= speed;
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                animationState = PlayerAnimationState.MovingRight;
                Position.X += speed;
            }
            else if (keyboard.IsKeyDown(Keys.Up))
            {
                animationState = PlayerAnimationState.MovingUp;
                Position.Y -= speed;
            }
            else if (keyboard.IsKeyDown(Keys.Down))
            {
                animationState = PlayerAnimationState.MovingDown;
                Position.Y += speed;
            }
            else
            {
                animationState = PlayerAnimationState.Idle;
            }

            // Apply animations
            switch (animationState)
            {
                case PlayerAnimationState.Idle:
                    currentFrame = 2;
                    animationTimer = new TimeSpan(0);
                    break;

                case PlayerAnimationState.MovingLeft:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;
                    
                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 4;
                    break;

                case PlayerAnimationState.MovingRight:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;
                    
                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 6;
                    break;
                case PlayerAnimationState.MovingDown:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;

                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 2;
                    break;
                case PlayerAnimationState.MovingUp:
                    animationTimer += gameTime.ElapsedGameTime;
                    spriteEffects = SpriteEffects.None;

                    if (animationTimer.TotalMilliseconds > FRAME_RATE *2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE);
                    break;

            }
        }

        public void CheckForEnemyCollision(IEnumerable<IBoundable> enemies)
        {
            foreach (Enemy enemy in enemies)
            {
                if (Bounds.CollidesWith(enemy.Bounds))
                {
                    animationState = PlayerAnimationState.Idle;
                }
            }
        }

        /// <summary>
        /// Render the player sprite.  Should be invoked between 
        /// SpriteBatch.Begin() and SpriteBatch.End()
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
#if VISUAL_DEBUG 
            VisualDebugging.DrawRectangle(spriteBatch, Bounds, Color.Red);
#endif
            frames[currentFrame].Draw(spriteBatch, Position, color, 0, origin, 2, spriteEffects, 1);
        }

    }
}
