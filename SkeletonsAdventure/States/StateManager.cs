﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SkeletonsAdventure.States
{
    internal class StateManager()
    {
        public State CurrentState { get; private set; }
        private State _nextState;

        public void ChangeState(State state)
        {
            CurrentState?.StateChangeFromHandler();
            _nextState = state;
            _nextState?.StateChangeToHandler();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentState.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                CurrentState = _nextState;
                _nextState = null;
            }

            CurrentState.Update(gameTime);
            CurrentState.PostUpdate(gameTime);
        }

        public void HandleInput(PlayerIndex playerIndex)
        {
            CurrentState.HandleInput(playerIndex);
        }
    }
}
