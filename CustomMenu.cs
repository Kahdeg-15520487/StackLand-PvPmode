using BerryLoaderNS;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace YourMod
{
    public class CustomMenu : MonoBehaviour
    {
        private CustomButton menuButton;

        public void Start()
        {
            var log = BerryLoader.L;
            log.LogInfo("initing screen..");

            menuButton = MenuAPI.CreateButton(GameCanvas.instance.MainMenuScreen.GetChild(0).GetChild(5), "Mod Options", () =>
            {
                log.LogInfo("menu button clicked..");
            });
            menuButton.transform.SetSiblingIndex(2);
        }
    }
}
