﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MapLibrary;

namespace MonoGameWindowsStarter
{
    enum EnemyAnimationState
    {
        Idle,
        MovingLeft,
        MovingRight,
        MovingUp,
        MovingDown
    }
    public class Enemy : IBoundable
    {
        const int FRAME_RATE = 300;
        int currentFrame = 0;
        TimeSpan animationTimer;

        EnemyAnimationState animationState = EnemyAnimationState.Idle;

        BoundingRectangle bounds;

        Sprite sprite;

        Vector2 position;
        Vector2 velocity;

        int movement;       // 0 for vertical 1 for horizontal
        int moveLength;

        public BoundingRectangle Bounds => bounds;

        public Enemy(BoundingRectangle bounds, Sprite sprite, Vector2 speed, int movingDirection, int moveLength)
        {
            this.bounds = bounds;
            this.sprite = sprite;
            this.position = new Vector2(bounds.X, bounds.Y);
            this.velocity = speed;
            velocity.Normalize();
            this.movement = movingDirection;
            this.moveLength = moveLength;

            animationState = EnemyAnimationState.MovingLeft;
        }

        public void Update(GameTime gameTime)
        {
            if (movement == 0)
            {
                bounds.X = position.X;
                bounds.Y = position.Y;

                if (velocity.Y > 0)
                {
                    animationState = EnemyAnimationState.MovingDown;
                }
                else
                {
                    animationState = EnemyAnimationState.MovingUp;
                }

                float oldPos = position.Y;
                position.Y += (float)gameTime.ElapsedGameTime.TotalMilliseconds * velocity.Y / 2;

                if (position.Y > (oldPos + moveLength) | position.Y < (oldPos - moveLength))
                {
                    oldPos = position.Y;
                    velocity.Y *= -1;
                }
            }
            else if  (movement == 1)
            {
                bounds.X = position.X;
                bounds.Y = position.Y;

                if (velocity.X > 0)
                {
                    animationState = EnemyAnimationState.MovingRight;
                }
                else
                {
                    animationState = EnemyAnimationState.MovingLeft;
                }
                
                float oldPos = position.X;
                position.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds * velocity.X / 2;

                if (position.X > oldPos + moveLength | position.X < oldPos - moveLength)
                {
                    oldPos = position.X;
                    velocity.X *= -1;
                }
            }

            switch (animationState)
            {
                case EnemyAnimationState.Idle:
                    currentFrame = 2;
                    animationTimer = new TimeSpan(0);
                    break;

                case EnemyAnimationState.MovingLeft:
                    animationTimer += gameTime.ElapsedGameTime;
                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 4;
                    break;

                case EnemyAnimationState.MovingRight:
                    animationTimer += gameTime.ElapsedGameTime;
                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 6;
                    break;
                case EnemyAnimationState.MovingDown:
                    animationTimer += gameTime.ElapsedGameTime;
                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE) + 2;
                    break;
                case EnemyAnimationState.MovingUp:
                    animationTimer += gameTime.ElapsedGameTime;
                    if (animationTimer.TotalMilliseconds > FRAME_RATE * 2)
                    {
                        animationTimer = new TimeSpan(0);
                    }
                    currentFrame = (int)Math.Floor(animationTimer.TotalMilliseconds / FRAME_RATE);
                    break;

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
#if VISUAL_DEBUG
            VisualDebugging.DrawRectangle(spriteBatch, bounds, Color.Green);
#endif
            sprite.Draw(spriteBatch, position/*new Vector2(bounds.X * sprite.Width, bounds.Y)*/, Color.White);
        }
    }
}
