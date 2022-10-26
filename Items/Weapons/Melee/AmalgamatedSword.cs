using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class AmalgamatedSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Enemies will start doubting whenever to attack you or not, loosing their focus'\nInflicts thoughtful to enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 66;
			Item.height = 70;
			Item.rare = 6;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 70);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 70;
			Item.crit = 4;
			Item.knockBack = 6f;
			Item.scale = 2f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.AmalgamatedSword>();
			Item.shootSpeed = 6f;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			// 60 frames = 1 second
			target.AddBuff(BuffID.Frostburn, 900);
			target.AddBuff(BuffID.Weak, 600);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "SpazSword");
			recipe.AddIngredient(null, "NeonBlade");
			recipe.AddIngredient(null, "VoidSword");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}