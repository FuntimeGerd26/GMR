using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class GerdRocketLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultra-Blue Rocket Launcher");
			Tooltip.SetDefault("'It's easier modifying things than making them'\nIncreases ranged damage by 8% and increases ranged knockback while in the inventory\nOn contact with an enemy the projectile will explode\nUses rockets as ammo");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 92;
			Item.height = 48;
			Item.rare = 7;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.reuseDelay = 70;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 240);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item61;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 60;
			Item.crit = 4;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.UltraBlueRocket>();
			Item.shootSpeed = 24f;
			Item.useAmmo = AmmoID.Rocket;
		}

		public override void UpdateInventory(Player player)
		{
			player.GetKnockback(DamageClass.Ranged) += 1f;
			player.GetDamage(DamageClass.Ranged) += 0.8f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, -2);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			type = ModContent.ProjectileType<Projectiles.Ranged.UltraBlueRocket>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdSniper");
			recipe.AddIngredient(ItemID.GrenadeLauncher);
            recipe.AddIngredient(ItemID.Wire, 50);
			recipe.AddIngredient(ItemID.IllegalGunParts);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
		}
	}
}