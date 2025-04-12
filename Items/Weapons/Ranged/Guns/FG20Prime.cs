using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class FG20Prime : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 114;
			Item.height = 50;
			Item.rare = 9;
			Item.useTime = 4;
			Item.useAnimation = 4;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 635);
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 68;
			Item.crit = 4;
			Item.knockBack = 6f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-30, -8);
		}
		public override bool CanUseItem(Player player)
		{
			SoundEngine.PlaySound(SoundID.Item41.WithPitchOffset(0.35f), player.Center);
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 6;
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(4f));

			if (type == ProjectileID.Bullet)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.PrimeBullet>();
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "PrimeEnhancementModule");
			recipe.AddIngredient(null, "PrimePlating", 4);
			recipe.AddIngredient(null, "InfraRedBar", 40);
			recipe.AddIngredient(ItemID.SoulofFright, 20);
			recipe.AddIngredient(ItemID.FragmentVortex, 12);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
		}
	}
}