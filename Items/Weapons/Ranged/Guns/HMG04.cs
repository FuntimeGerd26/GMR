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
	public class HMG04 : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 118;
			Item.height = 28;
			Item.rare = 8;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 455);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item40.WithPitchOffset(-0.25f);
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 35;
			Item.crit = -3;
			Item.knockBack = 4f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-24, -6);
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
				Item.useTime = Item.useAnimation = 30;
				Item.UseSound = SoundID.Research;
				Item.useAmmo = AmmoID.None;
				CombatText.NewText(displayPoint, new Color(255, 200, 0), $"Reloaded " + ammo + " Bullets!");
			}
			else if (ammo >= 120f)
			{
				Item.useTime = Item.useAnimation = 10;
				Item.UseSound = SoundID.MenuTick;
				Item.useAmmo = AmmoID.None;
			}
			else
			{
				Item.useTime = Item.useAnimation = (int)(11 - (ammo / 12));
				Item.UseSound = SoundID.Item41;
				Item.useAmmo = AmmoID.Bullet;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 8;
			velocity = (velocity.RotatedByRandom(MathHelper.ToRadians(ammo / 30))) * (0.5f + ((120 - ammo) * 0.025f));

			if (player.altFunctionUse == 2)
			{
				ammo = 0f;
				type = 0;
			}
			else
			{
				if (ammo >= 120f)
				{
					if (ammo > 120f)
						ammo = 120f;
					type = 0;
				}
				else
					ammo += 1f;
			}
		}
	}
}