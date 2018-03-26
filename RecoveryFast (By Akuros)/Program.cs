using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;
using Ensage.Common.Menu;
using SharpDX;

namespace RecoveryFast
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    internal static class Program
    {
        private static bool _loaded;
        private static readonly Menu Menu = new Menu("RecoveryFast (By Akuros)", "cb", true, "courier_return_stash_items", true).SetFontColor(Color.YellowGreen);
        private static Hero _globalTarget;
        private static readonly List<string> Items = new List<string>
        {
            "item_belt_of_strength",
            "item_ethereal_blade",
            "item_ultimate_scepter",
            "item_aether_lens",
            "item_octarine_core",
            "item_mystic_Staff",
            "item_cyclone",
            "item_sheepstick",
            "item_veil_of_discord",
            "item_point_booster",
            "item_energy_booster",
            "item_vitality_booster",
            "item_null_talisman",
            "item_power_treads",
            "item_glimmer_cape",
            "item_necronomicon",
            "item_necronomicon_2",
            "item_necronomicon_3",
            "item_orchid",
            "item_lotus_orb",
            "item_soul_booster",
            "item_bloodstone",
            "item_sphere",
            "item_ultimate_orb",
            "item_meteor_hammer",
            "item_staff_of_wizardry",
            "item_blade_of_alacrity",
            "item_ogre_axe",
            "item_magic_wand",
            "item_bloodthorn",
            "item_oblivion_Staff",
            "item_dagon_5",
            "item_dagon_4",
            "item_dagon_3",
            "item_dagon_2",
            "item_dagon",
            "item_magic_stick",
            "item_band_of_elvenskin",
            "item_mantle",
            "item_heart",
            "item_guardian_greaves",
            "item_ghost",
            "item_reaver",
            "item_robe",
            "item_rod_of_atos",
            "item_ancient_janggo",

        };
        public static Dictionary<string, int>itemsInOrder = new Dictionary<string, int>();
        private static Dictionary<Item,ItemSlot> ItemSlots = new Dictionary<Item,ItemSlot>();

        public static int Akuros { get; private set; }

        private static void Main()
        {
            Game.OnUpdate+=Game_OnUpdate;
            Drawing.OnDraw+=Drawing_OnDraw;

            var dict=new Dictionary<string,bool>
            {
                {Items[0],true},
                {Items[1],true},
                {Items[2],true},
                {Items[3],true},
                {Items[4],true},
                {Items[5],true},
                {Items[6],true},
                {Items[7],true},
                {Items[8],true},
                {Items[9],true},
                {Items[10],true},
                {Items[11],true},
                {Items[12],true},
                {Items[13],true},
                {Items[14],true},
                {Items[15],true},
                {Items[16],true},
                {Items[17],true},
                {Items[18],true},
                {Items[19],true},
                {Items[20],true},
                {Items[21],true},
                {Items[22],true},
                {Items[23],true},
                {Items[24],true},
                {Items[25],true},
                {Items[26],true},
                {Items[27],true},
                {Items[28],true},
                {Items[29],true},
                {Items[30],true},
                {Items[31],true},
                {Items[32],true},
                {Items[33],true},
                {Items[34],true},
                {Items[35],true},
                {Items[36],true},
                {Items[37],true},
                {Items[38],true},
                {Items[39],true},
                {Items[40],true},
                {Items[41],true},
                {Items[42],true},
                {Items[43],true},
                {Items[44],true},
                {Items[45],true},


            };

            Menu.AddItem(new MenuItem("Enabled", "Enabled").SetValue(true));
            Menu.AddItem(new MenuItem("Drop all", "Drop all").SetValue(new KeyBind('X', KeyBindType.Press)));
            Menu.AddItem(new MenuItem("Collect all", "Collect all").SetValue(new KeyBind('C', KeyBindType.Press)));
            Menu.AddItem(new MenuItem("Items suport", "Items Suport:").SetValue(new AbilityToggler(dict)));
            

            Menu.AddToMainMenu();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (!_loaded) return;
            if (_globalTarget == null || !_globalTarget.IsAlive) return;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            var me = ObjectManager.LocalHero;

            if (!_loaded)
            {
                if (!Game.IsInGame || me == null)
                {
                    return;
                }
                _loaded = true;
               
                Game.PrintMessage(
                    "<font face='Comic Sans MS, cursive'><font color='#00aaff'>" + Menu.DisplayName + " By Akuros" );
                Console.WriteLine("By Akuros");


            }

            if (!Game.IsInGame || me == null)
            {
                _loaded = false;
                return;
            }
            if (!Menu.Item("Enabled").GetValue<bool>()||!me.IsAlive || Game.IsPaused) return;

            if (Menu.Item("Drop all").GetValue<KeyBind>().Active)
            {
                if (Utils.SleepCheck("stop"))
                {   me.Stop();
                    Utils.Sleep(250, "stop");  
                }

                StashItem(me);
                return;
            }

            if (Menu.Item("Collect all").GetValue<KeyBind>().Active)
            {
                if (Utils.SleepCheck("stop"))
                {   me.Stop();
                    Utils.Sleep(250, "stop");  
                }

                GetItem(me);
            }

        }

        public static void GetItem(Hero me){
            if (Utils.SleepCheck("pickitems"))
            {   
                var droppedItems =
                ObjectManager.GetEntities<PhysicalItem>().Where(x => x.Distance2D(me) < 250).Reverse().ToList();
                foreach (var s in droppedItems)
                {   
                    me.PickUpItem(s, true); 
                }      

                var currentItemOrder =
                    me.Inventory.Items.Where(
                        x =>
                            (Items.Contains(x.Name) || x.Name.IndexOf("dagon")!=-1)&& Menu.Item("Items suport").GetValue<AbilityToggler>().IsEnabled(x.Name) &&
                            Utils.SleepCheck(x.Name)).ToList();

                foreach (var item in currentItemOrder){
                    if(Items.Contains(item.Name) && ItemSlots.ContainsKey(item)){
                        item.MoveItem(ItemSlots[item]);
                    }
                }
                itemsInOrder = new Dictionary<string, int>();
                ItemSlots = new Dictionary<Item,ItemSlot>();
                Utils.Sleep(200, "pickitems");

            }
        }


        public static void StashItem(Hero me){
            if (Utils.SleepCheck("dropitems"))
            {   
                for (var i = 0; i < 6; i++) {
                    var currentSlot = (ItemSlot) i;
                    var currentItem = me.Inventory.GetItem(currentSlot);

                    if (currentItem == null || !Items.Contains(currentItem.Name)) continue;
                    ItemSlots.Add(currentItem,currentSlot);
                    me.DropItem(currentItem, me.NetworkPosition, true);
                }
                Utils.Sleep(200, "dropitems");
            }
        }
    }
}
