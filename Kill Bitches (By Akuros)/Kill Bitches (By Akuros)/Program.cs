using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;
using Ensage.Common.Menu;
using System.Windows.Input;
using SharpDX;

namespace Kill_Bitches
{
    internal static class Program
    {
        private static readonly Menu Menu = new Menu("Kill Bitches (By Akuros)", "cb", true, "item_courier", true).SetFontColor(Color.OrangeRed);
        private static bool _loaded;
        private static void Main()

        {
            Menu.AddItem(new MenuItem("Kill", "kill and follow, if it's within our reach").SetValue(new KeyBind('0', KeyBindType.Toggle, false)).SetTooltip("auto AA enemy Couriers"));
           
            Menu.AddToMainMenu();

            Game.OnUpdate += Game_OnUpdate;

        }

        private static void On_Load(object sender, EventArgs e)
        {
            if (!_loaded)
            {
                if (!Game.IsInGame)
                {
                    return;
                }
                _loaded = true;
            }

            if (!Game.IsInGame)
            {
                _loaded = false;
                return;
            }

        }



        private static void Game_OnUpdate(EventArgs args)
        {

            if (!Utils.SleepCheck("rate"))
                return;




            var me = ObjectManager.LocalHero;
            var range = me.AttackRange;
            var couriers = ObjectManager.GetEntities<Courier>().Where(x => x.IsAlive && x.Team != me.Team);

            var enemies = ObjectManager.GetEntities<Hero>().Where(x => x.IsAlive && !x.IsIllusion && x.Team != me.Team).ToList();



     
            var aa = me.MinimumDamage  ;
       


            if (!_loaded)
            {
                if (!Game.IsInGame || me == null || !me.IsAlive)
                {
                    return;
                }
                _loaded = true;
            }

            if (!Game.IsInGame || me == null)
            {
                _loaded = false;
                return;
            }
            if (Game.IsPaused) return;
            foreach (var enemy in enemies)
                foreach (var courier in couriers)

                    if (me.Distance2D(courier) <= range && Menu.Item("Kill").GetValue<KeyBind>().Active )
                    {
                        me.Attack(courier);
                    }
        }
    }
}
    



