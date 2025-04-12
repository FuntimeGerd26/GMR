using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class GunslayerMachinegun : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 124;
			Item.height = 42;
			Item.rare = 4;
			Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 140);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item41;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 20;
			Item.crit = 8;
			Item.knockBack = 2f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 24f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, -4);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		float ammo;
		bool justReloaded;
		public override bool CanUseItem(Player player)
		{
			Rectangle displayPoint = new Rectangle(player.Hitbox.Center.X, player.Hitbox.Center.Y - player.height / 4, 2, 2);

			if (player.altFunctionUse == 2)
			{
				Item.useTime = 40;
				Item.useAnimation = 40;
				Item.UseSound = SoundID.Research;
				Item.useAmmo = AmmoID.None;

				if (justReloaded && ammo == 0f)
				{
					CombatText.NewText(displayPoint, new Color(150, 125, 255), "SHINE!");
					player.AddBuff(ModContent.BuffType<Buffs.Buff.GunslayerMight>(), 300);
				}
				else
					CombatText.NewText(displayPoint, new Color(255, 200, 0), $"Reloaded " + ammo + " Bullets!");
			}
			else if (ammo >= 40f)
			{
				Item.UseSound = SoundID.MenuTick;
				Item.useAmmo = AmmoID.None;
			}
			else
			{
				Item.useTime = 10;
				Item.useAnimation = 10;
				Item.UseSound = SoundID.Item41;
				Item.useAmmo = AmmoID.Bullet;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 2;
			if (player.altFunctionUse == 2)
			{
				ammo = 0f;
				type = 0;
				justReloaded = true;
			}
			else
			{
				if (ammo >= 40f)
				{
					if (ammo > 40f)
						ammo = 40f;
					type = 0;
				}
				else
					ammo += 1f;
				justReloaded = false;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GunslayerPistol");
			recipe.AddIngredient(ItemID.HellstoneBar, 8);
			recipe.AddIngredient(ItemID.SoulofNight, 6);
			recipe.AddIngredient(ItemID.MythrilBar, 15);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "GunslayerPistol");
			recipe2.AddIngredient(ItemID.HellstoneBar, 8);
			recipe2.AddIngredient(ItemID.SoulofNight, 6);
			recipe2.AddIngredient(ItemID.OrichalcumBar, 15);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 6);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}