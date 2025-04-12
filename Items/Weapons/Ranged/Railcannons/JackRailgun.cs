using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Railcannons
{
	public class JackRailgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Railgun");
			Tooltip.SetDefault($" Shoots a fast high damage ray, but hitting enemies wont't allow you to shoot again for some time\nUses modules for ammo");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 28;
			Item.rare = 5;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 275);
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/Railgun");
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 350;
			Item.crit = 46;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.Railgun.RailcannonRay>();
			Item.shootSpeed = 8f;
			Item.useAmmo = ModContent.ItemType<Items.Misc.Modules.NeonModule>();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-20, -2);
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
			position.Y = position.Y - 6;

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
					SoundEngine.PlaySound(GMR.GetSounds("Items/Ranged/railgunVariant", 2, 1f, 0f, 0.75f), player.Center);
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
			recipe.AddIngredient(ItemID.HallowedBar, 16);
			recipe.AddIngredient(ItemID.SoulofNight, 28);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddIngredient(null, "AncientInfraRedPlating", 30);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 8);
			recipe.AddIngredient(null, "InfraRedBar", 32);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}