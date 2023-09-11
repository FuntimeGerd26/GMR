using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class HatredGun : ModItem
	{
		public int Charge;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hateful Gun");
			Tooltip.SetDefault("Holding down Right-Click changes the gun mode\nFrom semi-auto to manual (with increased damage and piercing)");
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 28;
			Item.rare = 2;
			Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 80);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 10;
			Item.crit = 4;
			Item.knockBack = 1.5f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, 0);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Charge = Charge == 1 ? 0 : 1;
				Item.useTime = 30;
				Item.useAnimation = 30;
				Item.UseSound = SoundID.Research;
			}
			else if (Charge == 0)
			{
				Item.damage = 10;
				Item.useTime = 20;
				Item.useAnimation = 20;
				Item.UseSound = SoundID.Item11;
			}
			else
			{
				Item.damage = 48;
				Item.useTime = 50;
				Item.useAnimation = 50;
				Item.UseSound = SoundID.Item41;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				type = 0;
			}
			else
			{
				if (Charge == 1)
				{
					int p = Projectile.NewProjectile(Item.GetSource_FromThis(), position, velocity, type, damage, knockback, player.whoAmI);
					Main.projectile[p].penetrate += 2;
				}
			}
		}
	}
}