using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace GMR.GerdUI.EnchantmentToggles
{
    public class ToggleEnchantmentUI : UIState
    {
        private DragablePanel panel;
        private UIList toggleList;
        private UIScrollbar scrollBar;

        public override void OnInitialize()
        {
            UIHoverImageButton button = new(ModContent.Request<Texture2D>("GMR/GerdUI/EnchantmentToggles/ConfigureEnchants"), "Effect Toggler (Gerd's Lab)");
            button.OnLeftClick += (a, b) => ToggleConfig();
            button.SetRectangle(572, 310, 26, 26);
            Append(button);

            scrollBar = new();
            scrollBar.SetRectangle(340, 0, 20, 480);
            scrollBar.OverflowHidden = true;

            toggleList = new()
            {
                new EffectToggle("MayDress", "Amalgamate", "Dreaming/Awakened Dress Saws"),
                new EffectToggle("GoldenEmpire", "Amalgamate", "Active Golden Empire"),
                new EffectToggle("MultipleProjectile", "Amalgamate", "Multiply Projectiles"),
                new EffectToggle("AlloybloodDagger", "Alloyblood", "Alloyblood Dagger"),
                new EffectToggle("AlloybloodOrbitingProjectiles", "Alloyblood", "BL Fujoshi Orbitals"),
                new EffectToggle("AluminiumShuriken", "Aluminium", "Aluminium Shuriken"),
                new EffectToggle("BoostFireball", "Boost", "Naja Charm Fireball"),
                new EffectToggle("BoostSet", "Boost", "Boost Set Bonus"),
                new EffectToggle("IcePrincessShuriken", "IcePrincess", "Ice Princess Shuriken"),
                new EffectToggle("ArmRocket", "Magnum", "Charged Arm Rocket"),
                new EffectToggle("MaskedPlagueCloak", "MaskedPlague", "Masked Plague Cloak"),
                new EffectToggle("SandwaveKnife", "Sandwave", "Sandwave Knife"),
            };
            toggleList.SetRectangle(0, 0, 360, 500);
            toggleList.SetScrollbar(scrollBar);

            panel = new(toggleList, scrollBar);
            panel.SetRectangle(600, 36, 380, 500);

            panel.Append(toggleList);
            panel.Append(scrollBar);
        }

        private void ToggleConfig()
        {
            if (HasChild(panel))
            {
                RemoveChild(panel);
            }
            else
            {
                Append(panel);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Main.playerInventory)
            {
                ToggleEnchantmentSystem.Instance.Hide();
                if (HasChild(panel))
                {
                    RemoveChild(panel);
                }
                return;
            }
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }

    public class ToggleEnchantmentSystem : ModSystem
    {
        internal ToggleEnchantmentUI ToggleUI;
        private UserInterface toggleInterface;
        public static ToggleEnchantmentSystem Instance;

        public override void Load()
        {
            if (Main.dedServ) return;
            Instance = this;
            ToggleUI = new();
            ToggleUI.Activate();
            toggleInterface = new UserInterface();
            toggleInterface.SetState(ToggleUI);
        }

        public override void Unload()
        {
            Instance = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (Main.playerInventory) Show();
            toggleInterface?.Update(gameTime);
        }

        public void Show()
        {
            toggleInterface.SetState(ToggleUI);
        }
        public void Hide()
        {
            toggleInterface.SetState(null);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Gerd's Lab: Show Enchantment Toggles Button",
                    delegate
                    {
                        toggleInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
