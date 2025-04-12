using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class StarMadness : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.rare = 3;
			Item.width = 42;
			Item.height = 42;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 120);
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.autoReuse = true; 
			Item.damage = 15;
			Item.knockBack = 8f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.CoolSwords.NewSwords.StarMadness>();
			Item.shootSpeed = 10f;
		}

		public override bool MeleePrefix()
		{
			return true;
		}

		public override bool? UseItem(Player player)
		{
			Item.FixSwing(player);
			return null;
		}

		private float combo;
		public override bool CanUseItem(Player player)
		{
			if (combo >= 2f)
			{
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.StarMadnessThrow>();
				Item.shootSpeed = 18f;
				SoundEngine.PlaySound(SoundID.Item7.WithPitchOffset(-1f), player.Center);
			}
			else
			{
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.CoolSwords.NewSwords.StarMadness>();
				Item.shootSpeed = 10f;
			}

			Item.damage = 15 + (int)(player.statLife / 50 + 1);
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (combo >= 2f)
				combo = -1f;
			combo += 1f;

			float numberProjectiles = 2;
			float rotation = MathHelper.ToRadians(10f);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
				Projectile.NewProjectile(player.GetSource_FromThis(), position, perturbedSpeed, type, (int)(damage * 0.75f), knockback, player.whoAmI);
			}
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.GoldBar, 10);
			recipe.AddIngredient(ItemID.FallenStar, 15);
			recipe.AddIngredient(null, "MagmaticShard", 15);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.PlatinumBar, 10);
			recipe2.AddIngredient(ItemID.FallenStar, 15);
			recipe2.AddIngredient(null, "MagmaticShard", 15);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}