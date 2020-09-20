using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrCor_GameName
{

    public interface IShop 
    {
        /// <summary>
        /// First init shop on app start
        /// </summary>
        void InitShop();
        /// <summary>
        /// Update shop and view when open shop popup
        /// </summary>
        void UpdateShop();
    }

}