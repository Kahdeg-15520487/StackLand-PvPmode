using BerryLoaderNS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PvPMod
{
    public class CustomMenu : MonoBehaviour
    {
        private RectTransform serverScreen;
        private CustomButton menuButton;

        public void Start()
        {
            var log = BerryLoader.L;
            log.LogInfo("initing screen..");

            serverScreen = MenuAPI.CreateScreen("Connect to a server");
            var parent = serverScreen.GetChild(0).GetChild(1);
            var settlementButton = MenuAPI.CreateButton(parent, "test", () =>
            {
                log.LogInfo("button pressed");
            });
            settlementButton.GetComponent<Image>().sprite = Sprite.Create(Datas.Textures["image_button"], new Rect(0, 0, 1024, 1024), Vector2.zero);
            MenuAPI.CreateButton(parent, "Back", GameCanvas.instance.MainMenuScreen);

            menuButton = MenuAPI.CreateButton(GameCanvas.instance.MainMenuScreen.GetChild(0).GetChild(5), "Play on a server", serverScreen);
            menuButton.transform.SetSiblingIndex(2);
        }
    }
}
