using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Melee
{
	public class DualSlashBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"'Blade mode'\nGrants 'Cutting Edge', 'Rapid Healing' and 'Wrath' buffs when hitting an enemy\nInflicts Venom on enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; //Count of items to research
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 78;
			Item.height = 86;
			Item.rare = 8;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 325);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 88;
			Item.noMelee = true;
			Item.crit = 8;
			Item.knockBack = 6f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.DualSlashBladeSwing>();
			Item.shootSpeed = 6f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity * 0, type, damage,
				knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
			Projectile.NewProjectile(source, position, velocity * 0, ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.DualSlashCutterSwing>(), 0,
				knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "DualSlashCutter");
			recipe.AddIngredient(ItemID.ChlorophyteBar, 24);
			recipe.AddIngredient(ItemID.Ectoplasm, 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "DualBlasterShooter");
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}