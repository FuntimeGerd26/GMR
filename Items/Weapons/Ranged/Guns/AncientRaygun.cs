using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class AncientRaygun : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 96;
			Item.height = 26;
			Item.rare = 3;
			Item.useTime = 3;
			Item.useAnimation = 3;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 95);
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/BulletShot2") { Volume = 0.5f, };
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 8;
			Item.crit = 0;
			Item.knockBack = 0.5f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.AncientEnergy>();
			Item.shootSpeed = 24f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-20, -4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 6;
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(2));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AncientRifle");
			recipe.AddIngredient(ItemID.StarCannon);
			recipe.AddIngredient(null, "AncientInfraRedPlating", 30);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 6);
			recipe.AddIngredient(null, "MagmaticShard", 15);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}