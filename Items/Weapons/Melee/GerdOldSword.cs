using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class GerdOldSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Survival Blade");
			Tooltip.SetDefault("Having this in your inventory increases melee speed and all damage by 5%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 56;
			Item.height = 56;
			Item.rare = 2;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 75);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 20;
			Item.crit = 4;
			Item.knockBack = 2f;
		}

		public override void UpdateInventory(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetDamage(DamageClass.Generic) += 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdDagger");
			recipe.AddIngredient(ItemID.GoldBar, 12);
			recipe.AddIngredient(ItemID.Amber, 7);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "GerdDagger");
			recipe2.AddIngredient(ItemID.PlatinumBar, 12);
			recipe2.AddIngredient(ItemID.Amber, 7);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}