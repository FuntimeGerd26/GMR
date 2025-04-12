using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class RefinedSpazGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 58;
			Item.height = 28;
			Item.rare = 5;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 260);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 52;
			Item.crit = 2;
			Item.knockBack = 2.5f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 4f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-34, -4);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = 20;
				Item.useAnimation = 40;
				Item.reuseDelay = 50;
				Item.shootSpeed = 8f;
				Item.UseSound = SoundID.Item61.WithVolumeScale(-1f);
				Item.useStyle = ItemUseStyleID.Swing;
				Item.noUseGraphic = true;
				Item.useAmmo = AmmoID.None;
			}
			else
			{
				Item.useTime = 20;
				Item.useAnimation = 20;
				Item.reuseDelay = 0;
				Item.shootSpeed = 4f;
				Item.UseSound = SoundID.Item40;
				SoundEngine.PlaySound(SoundID.Item75.WithPitchOffset(0.25f), player.Center);
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noUseGraphic = false;
				Item.useAmmo = AmmoID.Bullet;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 10;

			if (player.altFunctionUse == 2)
			{
				SoundEngine.PlaySound(SoundID.Item7, player.Center);
				SoundEngine.PlaySound(SoundID.Item61.WithPitchOffset(-0.5f).WithVolumeScale(0.5f), player.Center);
				damage = damage * 2;
				type = ProjectileID.Grenade;
			}
			else
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.RefinedSpazmataniumBullet>();
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdHandgun");
			recipe.AddIngredient(ItemID.HallowedBar, 15);
			recipe.AddIngredient(null, "InfraRedBar", 20);
			recipe.AddIngredient(ItemID.SoulofNight, 24);
			recipe.AddIngredient(ItemID.SoulofSight, 18);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
		}
	}
}