using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class Glaicey : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts Frostburn to enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 50;
			Item.height = 50;
			Item.rare = 1;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 30);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 8;
			Item.crit = 14;
			Item.knockBack = 2f;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			// 60 frames = 1 second
			target.AddBuff(BuffID.Frostburn, 600);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IceBlock, 15);
			recipe.AddIngredient(ItemID.SnowBlock, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}