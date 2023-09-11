using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Accessories
{
	public class NajaBladeCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Volcano Charm");
			Tooltip.SetDefault($"Increases damage and attack speed by 8%\nMakes lightnings fall from the sky when taking damage" +
			$"\nUsing any weapon that's not ranged will shoot a fireball that explodes dealing damage on a large area");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 52;
			Item.value = Item.sellPrice(silver: 185);
			Item.rare = 3;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.08f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.08f;
			player.GPlayer().Thunderblade = Item;
			if (ClientConfig.Instance.NajaFireball)
			{
				player.GPlayer().NajaCharm = Item;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ThunderbladeCharm");
			recipe.AddIngredient(null, "NajaCharm");
			recipe.AddIngredient(ItemID.HellstoneBar, 25);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}