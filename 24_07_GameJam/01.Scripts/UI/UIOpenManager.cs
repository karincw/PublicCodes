using karin;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Karin
{

    public class UIOpenManager : MonoSingleton<UIOpenManager>
    {
        public List<UIOpener> LeftOpener, RightOpener, TopOpener, BottomOpener;
        [SerializeField] private TableScroller _scroller;

        private void Update()
        {
            if (((Keyboard.current.tabKey.wasReleasedThisFrame || Keyboard.current.iKey.wasReleasedThisFrame) &&
                (SceneManager.GetActiveScene().name == "Shop" && NPC.Instance.existNpc) || 
                (SceneManager.GetActiveScene() != null && SceneManager.GetActiveScene().name == "AnotherWorld")))
            {
                LeftOpener[0].ActiveSwitch();
            }
        }

        public void DefenceAll()
        {

        }
        public void DefenceOpenAll()
        {
            OpenDefencer(null, Position.Left, true);
            OpenDefencer(null, Position.Right, true);
        }

        public void OpenDefencer(UIOpener opener, Position position, bool state = false)
        {
            List<UIOpener> openers = null;
            switch (position)
            {
                case Position.Left:
                    openers = LeftOpener;
                    break;
                case Position.Right:
                    openers = RightOpener;
                    break;
                case Position.Top:
                    openers = TopOpener;
                    break;
                case Position.Bottom:
                    openers = BottomOpener;
                    break;
            }

            openers.ForEach(o =>
            {
                if (o != opener)
                {
                    o.BtnInteract(state);
                }
            });

        }

        public void ListSetup(Position position)
        {
            List<UIOpener> openers = null;
            switch (position)
            {
                case Position.Left:
                    openers = LeftOpener;
                    break;
                case Position.Right:
                    openers = RightOpener;
                    break;
                case Position.Top:
                    openers = TopOpener;
                    break;
                case Position.Bottom:
                    openers = BottomOpener;
                    break;
            }

            openers.ForEach(o =>
            {
                o.BtnInteract(true);
            });

        }

        public void CloseAll()
        {
            LeftOpener.ForEach(t => t.CloseAll());
            RightOpener.ForEach(t => t.CloseAll());
            TopOpener.ForEach(t => t.CloseAll());
            BottomOpener.ForEach(t => t.CloseAll());
            _scroller?.SetOriginPos();

        }

    }
}