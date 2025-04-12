using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class HatredGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 28;
			Item.rare = 1;
			Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 80);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 18;
			Item.crit = 4;
			Item.knockBack = 6f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
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
				Item.useTime = 50;
				Item.useAnimation = 50;
				Item.UseSound = SoundID.Research;
				Item.useAmmo = AmmoID.None;

				CombatText.NewText(displayPoint, new Color(255, 225, 125), $"Reloaded " + ammo + " Bullets!");
			}
			else if (ammo >= 16f)
			{
				Item.UseSound = SoundID.MenuTick;
				Item.useAmmo = AmmoID.None;
			}
			else
			{
				Item.useTime = 20;
				Item.useAnimation = 20;
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
			}
			else
			{
				if (ammo >= 16f)
				{
					if (ammo > 16f)
						ammo = 16f;
					type = 0;
				}
				else
					ammo += 1f;
			}
		}
	}
}