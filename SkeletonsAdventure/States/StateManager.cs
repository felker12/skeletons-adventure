using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkeletonsAdventure.States
{
    public class StateManager()
    {
        public State CurrentState { get; private set; }
        private State _nextState;

        public void ChangeState(State state)
        {
            _nextState = state;
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
    }
}
