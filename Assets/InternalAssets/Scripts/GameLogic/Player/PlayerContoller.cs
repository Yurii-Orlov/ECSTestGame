using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OrCor_GameName
{

    public class PlayerContoller 
    {
        private readonly GameManager _gameManager;

        public PlayerContoller(GameManager gameManager)
        {
            _gameManager = gameManager;
            Init();
        }

        public void Init()
        {
            Debug.Log("Player inited " + _gameManager.IsGameRuning);
        }

        public class Factory : PlaceholderFactory<PlayerContoller>
        {

        }
    }

}