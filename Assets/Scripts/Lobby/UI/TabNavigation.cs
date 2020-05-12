using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace NBPChess.Lobby.UI
{

    public class TabNavigation : MonoBehaviour
    {
        public enum NavPriority
        {
            Down, Right
        }
        public Selectable initialSelected;
        public NavPriority forwardPriority;
        private Selectable curSelected;
        private bool tabActive = false;

        public void Update()
        {
            UpdateCurSelected();
            UpdateTabInput();
        }
        private void UpdateCurSelected()
        {
            GameObject selectedGO = EventSystem.current.currentSelectedGameObject;
            tabActive = selectedGO != null && (curSelected = selectedGO.GetComponent<Selectable>()) != null;
        }
        private void UpdateTabInput()
        {
            if (tabActive && Input.GetKeyDown(KeyCode.Tab))
            {
                bool goingBackwards = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                if (!goingBackwards)
                {
                    Selectable tmp;
                    if (forwardPriority == NavPriority.Down)
                    {
                        if ((tmp = curSelected.FindSelectableOnDown()) != null || (tmp = curSelected.FindSelectableOnRight()) != null)
                        {
                            curSelected = tmp;
                        }
                    } else if (forwardPriority == NavPriority.Right)
                    {
                        if ((tmp = curSelected.FindSelectableOnRight()) != null || (tmp = curSelected.FindSelectableOnDown()) != null)
                        {
                            curSelected = tmp;
                        }
                    }
                } else
                {
                    Selectable tmp;
                    if (forwardPriority == NavPriority.Down)
                    {
                        if ((tmp = curSelected.FindSelectableOnUp()) != null || (tmp = curSelected.FindSelectableOnLeft()) != null)
                        {
                            curSelected = tmp;
                        }
                    }
                    else if (forwardPriority == NavPriority.Right)
                    {
                        if ((tmp = curSelected.FindSelectableOnLeft()) != null || (tmp = curSelected.FindSelectableOnUp()) != null)
                        {
                            curSelected = tmp;
                        }
                    }
                }
                curSelected.Select();
            }
        }
    }
}

