using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class RCorpRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 56;
			Item.height = 28;
			Item.rare = 3;
			Item.useTime = 7;
            Item.useAnimation = 21;
			Item.reuseDelay = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 40);
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 33;
			Item.crit = 0;
			Item.knockBack = 6f;
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
		public override bool CanUseItem(Player player)
		{
			Rectangle displayPoint = new Rectangle(player.Hitbox.Center.X, player.Hitbox.Center.Y - player.height / 4, 2, 2);

			if (player.altFunctionUse == 2)
			{
				Item.useTime = 90;
				Item.useAnimation = 90;
				Item.useAmmo = AmmoID.Bullet;

				CombatText.NewText(displayPoint, new Color(255, 225, 125), $"Reloaded " + ammo + " Bullets!");
				player.AddBuff(ModContent.BuffType<Buffs.Buff.ReloadBuff>(), 300);
			}
			else if (ammo >= 21f)
			{
				Item.useTime = 40;
				Item.useAnimation = 40;
				Item.reuseDelay = 0;
				Item.useAmmo = AmmoID.None;
			}
			else
			{
				Item.useTime = 7;
				Item.useAnimation = 21;
				Item.reuseDelay = 30;
				Item.useAmmo = AmmoID.Bullet;
			}
			return true;
		}

		float velMult;
		int damageVal;
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 6;

			if (velMult > 33f)
				velMult = 33f;
			else if (velMult > 11f)
				velMult = (float)(damage / 3);
			else if (velMult < 11f)
				velMult = 11f;

			if (player.altFunctionUse == 2)
			{
				ammo = 0f;
				type = 0;
				SoundEngine.PlaySound(SoundID.Research, player.position);
			}
			else
			{
				if (ammo >= 21f)
				{
					if (ammo > 21f)
						ammo = 21f;
					type = 0;
					SoundEngine.PlaySound(SoundID.MenuTick, player.position);
				}
				else
				{
					ammo += 1f;
					SoundEngine.PlaySound(SoundID.Item11, player.position);
				}

				velocity.Normalize();
				velocity *= velMult;
				damageVal = (int)((damage / velMult) * 2);
				if (damageVal < 1)
					damageVal = 1;
				damage = damageVal;
			}
		}
	}
}