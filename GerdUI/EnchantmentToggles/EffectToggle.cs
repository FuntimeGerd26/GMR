using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace GMR.GerdUI.EnchantmentToggles
{
    public class EffectToggle : UIElement
    {
        public string IndexEffect;
        UIImageButton button;

        public EffectToggle(string effectIndex, string EnchantmentTexturePath, string text)
        {
            IndexEffect = effectIndex;

            UIPanel panel = new();
            panel.SetRectangle(0, 0, 336, 70);
            Append(panel);

            button = new(ModContent.Request<Texture2D>("Terraria/Images/UI/Wires_1"));
            button.SetRectangle(0, 0, 40, 40);
            button.Top.Set(-19, 0.5f);
            button.OnLeftClick += (a, b) => ToggleEffect();
            panel.Append(button);

            var enchantTexture = ModContent.Request<Texture2D>(EnchantmentTexturePath, ReLogic.Content.AssetRequestMode.ImmediateLoad);
            UIImage enchantmentImage = new(enchantTexture);
            enchantmentImage.Width.Set(enchantTexture.Width(), 0f);
            enchantmentImage.Height.Set(enchantTexture.Height(), 0f);
            enchantmentImage.Left.Set(2 - enchantTexture.Width() / 2, 0.5f);
            enchantmentImage.Top.Set(2 - enchantTexture.Height() / 2, 0.5f);
            button.Append(enchantmentImage);

            UIText effectName = new(text);
            effectName.SetRectangle(50, 0, 0, 0);
            effectName.Top.Set(-6, 0.5f);
            panel.Append(effectName);

            Width.Set(336, 0f);
            Height.Set(60, 0f);
        }

        private void ToggleEffect()
        {
            var gmrPlayer = Main.LocalPlayer.GPlayer();
            gmrPlayer.EnchantToggles[IndexEffect] = !gmrPlayer.EnchantToggles[IndexEffect];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var EnchantmentOutline = GMR.Instance.Assets.Request<Texture2D>("GerdUI/EnchantmentToggles/EnchantmentOutline_On", ReLogic.Content.AssetRequestMode.ImmediateLoad);
            var EnchantmentOutlineOff = GMR.Instance.Assets.Request<Texture2D>("GerdUI/EnchantmentToggles/EnchantmentOutline_Off", ReLogic.Content.AssetRequestMode.ImmediateLoad);

            var gmrPlayer = Main.LocalPlayer.GPlayer();
            if (gmrPlayer.EnchantToggles[IndexEffect])
            {
                button.SetImage(EnchantmentOutline);
            }
            else
            {
                button.SetImage(EnchantmentOutlineOff);
            }
        }
    }
}
