using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class BMFG : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bio-Mass Force Gun"); // Big Motherfucking Gun :trollface:
			Tooltip.SetDefault($" Shoots a fast high damage energy ray, but hitting enemies wont't allow you to shoot again for some time\nUses modules for ammo");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 120;
			Item.height = 46;
			Item.rare = 9;
			Item.useTime = 80;
			Item.useAnimation = 80;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 355);
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/Railgun");
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 145;
			Item.crit = 25;
			Item.knockBack = 3.5f;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.Railgun.RailcannonRay>();
			Item.shootSpeed = 8f;
			Item.useAmmo = ModContent.ItemType<Items.Misc.Modules.NeonModule>();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-32, -4);
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
					if (item.type == ModContent.ItemType<Items.Misc.Modules.NeonModule>())
					{
						type = ModContent.ProjectileType<Projectiles.Ranged.Railgun.RailcannonEnergy>();
					}
					else if (item.type == ModContent.ItemType<Items.Misc.Modules.AmethystModule>())
					{
						type = ModContent.ProjectileType<Projectiles.Ranged.Railgun.CrystalEnergy>();
					}
					else if (item.type == ModContent.ItemType<Items.Misc.Modules.MaskedPlagueModule>())
					{
						type = ModContent.ProjectileType<Projectiles.Ranged.Railgun.MaskedPlagueEnergy>();
					}
				}
				else
				{
					type = 0;
				}
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "JackRailgun");
			recipe.AddIngredient(null, "JackRailcannon");
			recipe.AddIngredient(ItemID.SoulofLight, 16);
			recipe.AddIngredient(ItemID.HallowedBar, 26);
			recipe.AddIngredient(ItemID.BeetleHusk, 20);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddIngredient(null, "SpecialUpgradeCrystal");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}