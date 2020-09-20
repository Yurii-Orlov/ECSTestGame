
using System.Collections.Generic;

namespace OrCor_GameName
{
    public class GameStateFactory
    {
        private readonly GameState.Factory _gamePlayFactory;
        private readonly MenuState.Factory _menuStateFactory;

        public GameStateFactory(GameState.Factory gamePlayFactory,
                                MenuState.Factory menuStateFactory)
        {
            _gamePlayFactory = gamePlayFactory;
            _menuStateFactory = menuStateFactory;
        }

        /// <summary>
        /// Creates the requested game state entity
        /// </summary>
        /// <param name="gameState">State we want to create</param>
        /// <returns>The requested game state entity</returns>
        internal GameStateEntity CreateState(Enumerators.GameStateTypes gameState)
        {
            switch (gameState)
            {

                case Enumerators.GameStateTypes.START_GAMEPLAY:
                    return _gamePlayFactory.Create();

                case Enumerators.GameStateTypes.MENU:
                    return _menuStateFactory.Create();

            }

            return null;

        }

    }
}