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
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 162;
			Item.height = 50;
			Item.rare = 9;
			Item.useTime = 80;
			Item.useAnimation = 80;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 355);
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/Railgun");
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 600;
			Item.crit = 96;
			Item.knockBack = 3.5f;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.Railgun.RailcannonRay>();
			Item.shootSpeed = 12f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-64, -4);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		int Mode;
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Mode = Mode >= 2 ? 0 : Mode + 1;
				Item.UseSound = SoundID.Research;

				Rectangle displayPoint = new Rectangle(player.Hitbox.Center.X, player.Hitbox.Center.Y - player.height / 4, 2, 2);
				if (Mode == 0) // Piercing Gatorade
				{
					CombatText.NewText(displayPoint, Color.HotPink, "Pierce");
				}
				else if (Mode == 1) // The Homing Laser:tm:
				{
					CombatText.NewText(displayPoint, Color.Purple, "Chase");
				}
				else if (Mode == 2) // Baja Blast
				{
					CombatText.NewText(displayPoint, Color.Lime, "Explode");
				}
			}
			else
			{
				if (player.HasBuff(ModContent.BuffType<Buffs.Debuffs.InfraRedCorrosion>()))
					return false;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 6;

			if (player.altFunctionUse == 2)
				type = 0;
			else
			{
				if (!player.HasBuff(ModContent.BuffType<Buffs.Debuffs.InfraRedCorrosion>()))
				{
					if (Mode == 0)
					{
						type = ModContent.ProjectileType<Projectiles.Ranged.Railgun.RailcannonEnergy>();
					}
					else if (Mode == 1)
					{
						type = ModContent.ProjectileType<Projectiles.Ranged.Railgun.CrystalEnergy>();
					}
					else if (Mode == 2)
					{
						type = ModContent.ProjectileType<Projectiles.Ranged.Railgun.MaskedPlagueEnergy>();
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
			recipe.AddIngredient(null, "JackRailgun");
			recipe.AddIngredient(null, "JackRailcannon");
			recipe.AddIngredient(null, "AmethystModule");
			recipe.AddIngredient(null, "MaskedPlagueModule");
			recipe.AddIngredient(null, "NeonModule");
			recipe.AddIngredient(null, "InfraRedBar", 40);
			recipe.AddIngredient(ItemID.SoulofLight, 16);
			recipe.AddIngredient(ItemID.BeetleHusk, 20);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddIngredient(null, "SpecialUpgradeCrystal");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}