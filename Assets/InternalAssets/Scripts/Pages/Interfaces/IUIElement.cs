using UnityEngine;

namespace OrCor_GameName
{
    public interface IUIElement
    {
        GameObject SelfPage { get; set; }

        void Show();
        void Hide();
    }
}