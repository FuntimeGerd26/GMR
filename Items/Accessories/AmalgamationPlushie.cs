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
	public class AmalgamationPlushie : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amalgamion Plushie");
			Tooltip.SetDefault("'7'\nIncreases invincibility frames by 4 seconds\nWeapons have a chance to shoot 5 projectiles and shoot an aditional special projectile\nIncreases ranged and magic damage by 14%\nConverts wooden and fire arrows turn into 3 Jack Shards");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 70;
			Item.value = Item.sellPrice(silver: 180);
			Item.rare = 7;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Magic) += 0.14f;
			player.GetDamage(DamageClass.Ranged) += 0.14f;
			player.GPlayer().AmalgamPlush = Item;
			player.GPlayer().AmalgamInmune = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "DevPlushie");
			recipe.AddIngredient(null, "JackExpert");
			recipe.AddIngredient(ItemID.SpectreBar, 7);
			recipe.AddIngredient(3783, 2); //Forbidden Fragment
			recipe.AddIngredient(ItemID.SoulofNight, 14);
			recipe.AddIngredient(ItemID.SoulofFright, 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}