using BerryLoaderNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CoopDungeon.Source
{
    public class DungeonPortal : CardData
    {
        public int MaxCapacity = 5;

        public float TravelTime = 5f;

        public bool InPrompt;

        public override bool DetermineCanHaveCardsWhenIsRoot => true;

        public bool IsPreparingToGo
        {
            get
            {
                if (MyGameCard.TimerRunning)
                {
                    return MyGameCard.TimerActionId == GetActionId("Preparing");
                }
                return false;
            }
        }

        public bool InPreparing => InPrompt;

        public override bool CanBeDragged
        {
            get
            {
                if (IsPreparingToGo)
                {
                    return false;
                }
                return true;
            }
        }
        public override bool CanBePushedBy(CardData otherCard)
        {
            return false;
        }
        protected override bool CanHaveCard(CardData otherCard)
        {
            return otherCard.MyCardType == CardType.Humans;
        }
        public override bool CanHaveCardsWhileHasStatus()
        {
            return true;
        }

        protected override void Awake()
        {
            //this.frames =

            base.Awake();
        }

        public override void Clicked()
        {
            var L = Plugin.L;

            var ov = this.MyGameCard.gameObject.GetComponentInChildren<ModOverride>();
            ov.Color = new Color(0, 0, 0, 1);
            ov.Color2 = new Color(0, 0, 0, 1);
            ov.IconColor = new Color(0, 0, 0, 1);

            //var renderer = this.MyGameCard.CardRenderer;
            //var mpb = new MaterialPropertyBlock();
            //renderer.GetPropertyBlock(mpb, 2);
            //this.MyCardType = (CardType)9;
            //for (int i = 0; i < renderer.materials.Length; i++)
            //{
            //    var material = renderer.materials[i];
            //    //if (material.name.Contains("CardFront"))
            //    {
            //        L.LogInfo(material.name);
            //        L.LogInfo($"\t{material.shader.name}");
            //        var shader = material.shader;
            //        mpb.SetColor("_Color", Color.white);
            //        mpb.SetColor("_Color2", Color.white);
            //        mpb.SetColor("_IconColor", Color.white);
            //        renderer.SetPropertyBlock(mpb, 2);
            //        for (int j = 0; j < shader.GetPropertyCount(); j++)
            //        {
            //            var propName = shader.GetPropertyName(j);
            //            switch (shader.GetPropertyType(j))
            //            {
            //                case UnityEngine.Rendering.ShaderPropertyType.Color:
            //                    L.LogInfo($"\t{propName} : {material.GetColor(propName)} : {mpb.GetColor(propName)}");
            //                    break;
            //                case UnityEngine.Rendering.ShaderPropertyType.Vector:
            //                    L.LogInfo($"\t{propName} : {material.GetVector(propName)} : {mpb.GetVector(propName)}");
            //                    break;
            //                case UnityEngine.Rendering.ShaderPropertyType.Float:
            //                    L.LogInfo($"\t{propName} : {material.GetFloat(propName)} : {mpb.GetFloat(propName)}");
            //                    break;
            //                case UnityEngine.Rendering.ShaderPropertyType.Texture:
            //                    L.LogInfo($"\t{propName} : {material.GetTexture(propName)} : {mpb.GetTexture(propName)}");
            //                    break;
            //            }
            //        }
            //        L.LogInfo("=====");
            //    }
            //}

            //var mr = this.MyGameCard.gameObject.GetComponentInChildren<MeshRenderer>();
            ////mr.materials = mr.materials.ToList().Take(2).ToArray();
            //var mpb = new MaterialPropertyBlock();
            //mr.GetPropertyBlock(mpb, 2);
            //mpb.Clear();
            //mr.SetPropertyBlock(mpb, 2);
            //for (int i = 0; i < mr.materials.Length; i++)
            //{
            //    var material = mr.materials[i];
            //    //if (material.name.Contains("CardFront"))
            //    {
            //        L.LogInfo(material.name);
            //        L.LogInfo($"\t{material.shader.name}");
            //        var shader = material.shader;
            //        material.SetColor("_Color", new Color(0, 0, 0, 0));
            //        material.SetColor("_Color2", new Color(0, 0, 0, 0));
            //        material.SetColor("_IconColor", new Color(0, 0, 0, 0));
            //        material.SetFloat("_Foil", 0f);
            //        material.SetFloat("_SparkleIntensity", 0f);
            //        for (int j = 0; j < shader.GetPropertyCount(); j++)
            //        {
            //            var propName = shader.GetPropertyName(j);
            //            switch (shader.GetPropertyType(j))
            //            {
            //                case UnityEngine.Rendering.ShaderPropertyType.Color:
            //                    L.LogInfo($"\t{propName} : {material.GetColor(propName)}");
            //                    break;
            //                case UnityEngine.Rendering.ShaderPropertyType.Vector:
            //                    L.LogInfo($"\t{propName} : {material.GetVector(propName)}");
            //                    break;
            //                case UnityEngine.Rendering.ShaderPropertyType.Float:
            //                    L.LogInfo($"\t{propName} : {material.GetFloat(propName)}");
            //                    break;
            //                case UnityEngine.Rendering.ShaderPropertyType.Texture:
            //                    L.LogInfo($"\t{propName} : {material.GetTexture(propName)}");
            //                    break;
            //            }
            //        }
            //        L.LogInfo("=====");
            //    }
            //}

            base.Clicked();
        }

        public override void UpdateCard()
        {
            if (!TransitionScreen.InTransition && !WorldManager.instance.InAnimation)
            {
                int num = ChildrenMatchingPredicateCount((CardData x) => x is Villager);
                if (num > 0)
                {
                    int cardCount = WorldManager.instance.GetCardCount((CardData x) => x is Villager);
                    int requiredFoodCount = WorldManager.instance.GetRequiredFoodCount();
                    if (WorldManager.instance.GetFoodCount() < requiredFoodCount)
                    {
                        MyGameCard.Child.RemoveFromParent();
                        CoopDungeonModalScreen.instance.NotEnoughFoodToGoToDungeonPrompt();
                    }
                    else if (WorldManager.instance.CurrentBoard.Id == "main" && num == cardCount)
                    {
                        MyGameCard.CancelTimer(GetActionId("Preparing"));
                        CoopDungeonModalScreen.instance.OneVillagerNeedsToStayPrompt();
                        RemoveLastVillager();
                    }
                    else
                    {
                        MyGameCard.StartTimer(TravelTime, Preparing, SokLoc.Translate("card_dungeon_portal_status"), GetActionId("Preparing"));
                    }
                }
                else
                {
                    MyGameCard.CancelTimer(GetActionId("Preparing"));
                }
            }
            base.UpdateCard();
        }

        private void RemoveLastVillager()
        {
            List<GameCard> allCardsInStack = MyGameCard.GetAllCardsInStack();
            for (int num = allCardsInStack.Count - 1; num >= 0; num--)
            {
                if (allCardsInStack[num].CardData is Villager)
                {
                    allCardsInStack.RemoveAt(num);
                    break;
                }
            }
            WorldManager.instance.Restack(allCardsInStack);
        }

        [TimedAction("Preparing")]
        public void Preparing()
        {
            if (!TransitionScreen.InTransition && !WorldManager.instance.InAnimation)
            {
                CoopDungeonModalScreen.instance.ChangeLocationPrompt(GoAway, Stay);
            }
        }

        private void Stay()
        {
            RestackChildrenMatchingPredicate((CardData c) => c is Villager);
        }

        private void GoAway()
        {
            EndOfMonthParameters endOfMonthParameters = new EndOfMonthParameters();
            endOfMonthParameters.SkipSpecialEvents = true;
            endOfMonthParameters.EndOfMonthText = SokLoc.Translate("coop_dungeon_label_enter_dungeon_full");
            endOfMonthParameters.SkipEndConfirmation = true;
            InPrompt = true;
            //RemoveStacksFromAllBoats();
            endOfMonthParameters.OnDone = delegate
            {
                InPrompt = false;
                GameCanvas.instance.SetScreen(GameCanvas.instance.EndOfMonthScreen);
                string id = ((!(WorldManager.instance.CurrentBoard.Id == "main")) ? "main" : "dungeon");
                GameBoard targetBoard = WorldManager.instance.GetBoardWithId(id);
                WorldManager.instance.GoToBoard(targetBoard, delegate
                {
                    GameCanvas.instance.SetScreen(GameCanvas.instance.GameScreen);
                    WorldManager.instance.SendToBoard(MyGameCard, targetBoard, new Vector2(0.4f, 0.5f));
                    RestackChildrenMatchingPredicate((CardData v) => v is Villager);
                });
            };
            WorldManager.instance.ForceEndOfMoon(endOfMonthParameters);
        }
    }
}
