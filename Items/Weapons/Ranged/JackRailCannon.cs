using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class JackRailcannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Railgun");
			Tooltip.SetDefault($" Shoots a fast high damage ray, but hitting enemies wont't allow you to shoot again for some time\nUses modules for ammo");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 106;
			Item.height = 44;
			Item.rare = 3;
			Item.useTime = 70;
			Item.useAnimation = 70;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 275);
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/Railgun");
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 78;
			Item.crit = 15;
			Item.knockBack = 3.5f;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.Railgun.RailcannonRay>();
			Item.shootSpeed = 8f;
			Item.useAmmo = ModContent.ItemType<Items.Misc.Modules.NeonModule>();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-24, -2);
		}

		public override bool CanUseItem(Player player)
		{
			if (player.HasBuff(ModContent.BuffType<Buffs.Debuffs.InfraRedCorrosion>()))
				return false;
			else
				return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			foreach (Item item in player.inventory)
			{
				if (!player.HasBuff(ModContent.BuffType<Buffs.Debuffs.InfraRedCorrosion>()))
				{
					if (item.type == ModContent.ItemType<Items.Misc.Modules.AmethystModule>())
					{
						type = ModContent.ProjectileType<Projectiles.Ranged.Railgun.CrystalRay>();
					}
					else if (item.type == ModContent.ItemType<Items.Misc.Modules.MaskedPlagueModule>())
					{
						type = ModContent.ProjectileType<Projectiles.Ranged.Railgun.MaskedPlagueRay>();
					}
					else if (item.type == ModContent.ItemType<Items.Misc.Modules.NeonModule>())
					{
						type = ModContent.ProjectileType<Projectiles.Ranged.Railgun.RailcannonRay>();
					}
				}
				else
				{
					type = 0;
				}
			}
		}
	}
}