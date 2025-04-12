using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Spears
{
	public class OvercooledSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
			ItemID.Sets.Spears[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.rare = 6;
			Item.width = 90;
			Item.height = 90;
			Item.value = Item.sellPrice(silver: 220);
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = true;
			Item.damage = 50;
			Item.knockBack = 6f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.OvercooledSpear>();
		}

		public override bool? UseItem(Player player)
		{
			if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}

		float combo;
		public override bool CanUseItem(Player player)
		{
			if (combo >= 2f)
			{
				Item.useTime = 10;
				Item.useAnimation = 10;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.OvercooledSpear>();
				combo += 1f;
				return true;
			}
			else if (combo < 2f)
			{
				Item.useTime = 20;
				Item.useAnimation = 20;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.OvercooledSpearSpin>();
				combo += 1f;
				return player.ownedProjectileCounts[Item.shoot] < 1;
			}

			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (combo >= 3f)
				combo = -1f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "Iceberg");
			recipe.AddIngredient(null, "InfraRedBar", 15);
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddIngredient(ItemID.FrostCore);
			recipe.AddIngredient(ItemID.SoulofFright, 6);
			recipe.AddIngredient(ItemID.SoulofSight, 6);
			recipe.AddIngredient(ItemID.SoulofMight, 6);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}