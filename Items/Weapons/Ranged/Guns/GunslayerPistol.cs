using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class GunslayerPistol : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 58;
			Item.height = 38;
			Item.rare = 2;
			Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 40);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item41;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 30;
			Item.crit = 8;
			Item.knockBack = 8f;
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
				Item.useTime = 90;
				Item.useAnimation = 90;
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
			else if (ammo >= 8f)
			{
				Item.UseSound = SoundID.MenuTick;
				Item.useAmmo = AmmoID.None;
			}
			else
			{
				Item.useTime = 40;
				Item.useAnimation = 40;
				Item.UseSound = SoundID.Item41;
				Item.useAmmo = AmmoID.Bullet;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 6;
			if (player.altFunctionUse == 2)
			{
				ammo = 0f;
				type = 0;
				justReloaded = true;
			}
			else
			{
				if (ammo >= 8f)
				{
					if (ammo > 8f)
						ammo = 8f;
					type = 0;
				}
				else
					ammo += 1f;
				justReloaded = false;
			}
		}
	}
}