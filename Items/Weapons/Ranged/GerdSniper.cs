using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class GerdSniper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultra-Blue Sniper");
			Tooltip.SetDefault("'Reloading won't be the worst thing'\nHaving this increases ranged weapon speed by 10% but decreases damage by 5%\nUpon hitting enemies shoots 3 energy bullets behind it");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 86;
			Item.height = 48;
			Item.rare = 6;
			Item.useTime = 20;
			Item.useAnimation = 25;
			Item.reuseDelay = 50;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 120);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item40;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 45;
			Item.crit = 4;
			Item.knockBack = 4f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.UltraBlueDart>();
			Item.shootSpeed = 30f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override void UpdateInventory(Player player)
		{
			player.GetAttackSpeed(DamageClass.Ranged) += 0.1f;
			player.GetDamage(DamageClass.Ranged) += 0.05f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, -4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Seed || type == ProjectileID.PoisonDartBlowgun || type == ProjectileID.IchorDart || type == ProjectileID.CrystalDart || type == ProjectileID.CursedDart)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.UltraBlueDart>();
				SoundEngine.PlaySound(SoundID.Item40, player.position);
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CobaltBar, 12);
			recipe.AddIngredient(ItemID.SoulofNight, 25);
			recipe.AddIngredient(ItemID.AdamantiteBar, 16);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.PalladiumBar, 12);
			recipe2.AddIngredient(ItemID.SoulofNight, 25);
			recipe2.AddIngredient(ItemID.TitaniumBar, 16);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ItemID.PalladiumBar, 12);
			recipe3.AddIngredient(ItemID.SoulofNight, 25);
			recipe3.AddIngredient(ItemID.AdamantiteBar, 16);
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.Register();

			Recipe recipe4 = CreateRecipe();
			recipe4.AddIngredient(ItemID.CobaltBar, 12);
			recipe4.AddIngredient(ItemID.SoulofNight, 25);
			recipe4.AddIngredient(ItemID.TitaniumBar, 16);
			recipe4.AddTile(TileID.MythrilAnvil);
			recipe4.Register();
		}
	}
}