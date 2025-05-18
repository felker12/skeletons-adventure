using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Animations;
using System;
using System.Collections.Generic;

namespace SkeletonsAdventure.Entities
{
    public class AnimatedSprite : Sprite
    {
        protected Dictionary<AnimationKey, SpriteAnimation> _animations;


        //Get/Set
        protected bool IsAnimating { get; set; }
        public AnimationKey CurrentAnimation { get; set; }

        //Constructors
        public AnimatedSprite() : base()
        {
            SetFrames(3, 32, 54, 0, 64); //This is the default which is used by the skeleton spritesheet
        }

        public void SetFrames(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset)
        {
            _animations = [];
            CloneAnimations(CreateAnimations(frameCount, frameWidth, frameHeight, xOffset, yOffset));
            UpdateFrame();
        }

        protected void UpdateFrame()
        {
            Frame = _animations[CurrentAnimation].CurrentFrameRect;
        }

        protected void CloneAnimations(Dictionary<AnimationKey, SpriteAnimation> animation)
        {
            foreach (AnimationKey key in animation.Keys)
                _animations.Add(key, (SpriteAnimation)animation[key].Clone());
        }

        //Methods
        public override void Update(GameTime gameTime)
        {
            if (IsAnimating)
                _animations[CurrentAnimation].Update(gameTime);

            UpdateFrame();
            UpdateCurrentAnimation();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        protected void UpdateCurrentAnimation()
        {
            Vector2 motion = CalculateReducedMotion(Motion);

            //TODO Add frames for angles
            if (motion != Vector2.Zero)
            {
                //TODO   
                //Add a check for the angle of the motion vector to determine which animation to use



                IsAnimating = true;
            }
            else
                IsAnimating = false;
        }

        private Dictionary<AnimationKey, SpriteAnimation> CreateAnimations(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset)
        {
            Dictionary<AnimationKey, SpriteAnimation> _Animations = [];
            Width = frameWidth;
            Height = frameHeight;

            SpriteAnimation animation = new(frameCount, frameWidth, frameHeight, 0, 0);
            _Animations.Add(AnimationKey.Down, animation);
            animation = new(frameCount, frameWidth, frameHeight, xOffset, yOffset);
            _Animations.Add(AnimationKey.Left, animation);
            animation = new(frameCount, frameWidth, frameHeight, xOffset * 2, yOffset* 2);
            _Animations.Add(AnimationKey.Right, animation);
            animation = new(frameCount, frameWidth, frameHeight, xOffset * 3 , yOffset * 3);
            _Animations.Add(AnimationKey.Up, animation);

            return _Animations;
        }

        public static Vector2 CalculateReducedMotion(Vector2 Motion)
        {
            Vector2 motion = Vector2.Zero;

            if (Motion != Vector2.Zero)
            {
                if (Motion.X != 0 && Motion.Y != 0)
                {
                    if (Math.Abs(Motion.X) > Math.Abs(Motion.Y))
                    {
                        if (Math.Abs(Motion.X) > 1)
                            motion = new(Motion.X / Math.Abs(Motion.X), Motion.Y / Math.Abs(Motion.X));
                        else
                            motion = Motion;
                    }
                    else
                    {
                        if (Math.Abs(Motion.Y) > 1)
                            motion = new(Motion.X / Math.Abs(Motion.Y), Motion.Y / Math.Abs(Motion.Y));
                        else
                            motion = Motion;
                    }
                }
                else if (Motion.X != 0 && Motion.Y == 0)
                {
                    if (Math.Abs(Motion.X) > 1)
                        motion = new(Motion.X / Math.Abs(Motion.X), Motion.Y);
                    else
                        motion = Motion;
                }
                else if (Motion.X == 0 && Motion.Y != 0)
                {
                    if (Math.Abs(Motion.Y) > 1)
                        motion = new(Motion.X, Motion.Y / Math.Abs(Motion.Y));
                    else motion = Motion;
                }
            }

            return motion;
        }
    }
}

