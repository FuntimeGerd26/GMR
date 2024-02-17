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
			Tooltip.SetDefault("Shoots swords that inflict Frostburn to enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
		}

		public override void SetDefaults()
		{
			Item.width = 56;
			Item.height = 56;
			Item.rare = 1;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 125);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 18;
			Item.crit = 0;
			Item.knockBack = 6f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.Glaicey>();
			Item.shootSpeed = 18f;
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Frostburn, 600);
		}

		int ShootBlade;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (ShootBlade == 0)
				ShootBlade = 1;
			if (ShootBlade >= 1 || ShootBlade <= -1)
				ShootBlade *= -1;

			Projectile.NewProjectile(source, position, velocity * 0, type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
			if (ShootBlade == 1)
				Projectile.NewProjectile(source, player.MountedCenter, velocity * 0.5f, ModContent.ProjectileType<Projectiles.Melee.Glaicy>(), damage * 2, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IceBlock, 30);
			recipe.AddIngredient(ItemID.SnowBlock, 40);
			recipe.AddRecipeGroup("GMR:AnyGem", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}