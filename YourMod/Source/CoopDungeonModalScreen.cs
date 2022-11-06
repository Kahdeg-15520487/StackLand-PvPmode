using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CoopDungeon.Source
{
    public class CoopDungeonModalScreen
    {
        public static CoopDungeonModalScreen instance = new CoopDungeonModalScreen();

        public void NotEnoughFoodToGoToDungeonPrompt()
        {
            ModalScreen.instance.Clear();
            ModalScreen.instance.SetTexts(SokLoc.Translate("coop_dungeon_label_enter_dungeon_full"), SokLoc.Translate("coop_dungeon_label_not_enough_food_before_entering_dungeon"));
            ModalScreen.instance.AddOption(SokLoc.Translate("label_okay"), delegate
            {
                var opb = new MaterialPropertyBlock(); try
                {
                    WorldManager.instance.Boards[0].gameObject.GetComponent<MeshRenderer>().GetPropertyBlock(opb);
                    Plugin.L.LogInfo($"{opb is null}");
                    var texture = opb.GetTexture(0);
                    Plugin.L.LogInfo($"{texture is null}");
                    //texture
                    Plugin.L.LogInfo(texture?.name);
                }
                catch (Exception ex)
                {
                    Plugin.L.LogError(ex.ToString());
                }
                CloseModal();
            });
            OpenModal();
        }

        public void OneVillagerNeedsToStayPrompt()
        {
            ModalScreen.instance.Clear();
            ModalScreen.instance.SetTexts(SokLoc.Translate("coop_dungeon_label_enter_dungeon_full"), SokLoc.Translate("label_one_villager_needs_to_stay"));
            ModalScreen.instance.AddOption(SokLoc.Translate("label_okay"), delegate
            {
                CloseModal();
            });
            OpenModal();
        }

        public void ChangeLocationPrompt(Action onYes, Action onNo)
        {
            string termId = ((!(WorldManager.instance.CurrentBoard.Id == "main")) ? "coop_dungeon_label_return_to_mainland_prompt" : "coop_dungeon_label_go_to_dungeon_prompt");
            ModalScreen.instance.Clear();
            ModalScreen.instance.SetTexts(SokLoc.Translate("coop_dungeon_label_enter_dungeon_full"), SokLoc.Translate(termId));
            ModalScreen.instance.AddOption(SokLoc.Translate("label_yes"), delegate
            {
                CloseModal();
                onYes();
            });
            ModalScreen.instance.AddOption(SokLoc.Translate("label_no"), delegate
            {
                CloseModal();
                onNo();
            });
            OpenModal();
        }

        private void OpenModal()
        {
            GameCanvas.instance.OpenModal();
        }

        private void CloseModal()
        {
            GameCanvas.instance.CloseModal();
        }
    }
}
