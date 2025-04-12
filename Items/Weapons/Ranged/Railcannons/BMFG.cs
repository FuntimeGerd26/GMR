using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Railcannons
{
	public class BMFG : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bio-Mass Force Gun"); // Big Motherfucking Gun :trollface:

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 162;
			Item.height = 50;
			Item.rare = 9;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 355);
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
			return new Vector2(-60, -2);
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
					CombatText.NewText(displayPoint, Color.HotPink, "Single");
				}
				else if (Mode == 1) // The Homing Projectiles
				{
					CombatText.NewText(displayPoint, Color.HotPink, "Burst");
				}
				else if (Mode == 2) // Baja Blast
				{
					CombatText.NewText(displayPoint, Color.HotPink, "Split");
				}

				Item.useTime = 20;
				Item.useAnimation = 20;
				Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/Railgun");
			}
			else
			{

				if (player.HasBuff(ModContent.BuffType<Buffs.Debuffs.InfraRedCorrosion>()) && Mode != 1)
				{
					return false;
				}
				else if (Mode == 0)
				{
					Item.damage = 500;
					Item.useTime = 60;
					Item.useAnimation = 60;
					Item.reuseDelay = 0;
					Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/Railgun");
				}
				else if (Mode == 1)
				{
					Item.damage = 100;
					Item.useTime = 4;
					Item.useAnimation = 12;
					Item.reuseDelay = 30;
					Item.UseSound = SoundID.Item20;
				}
				else if (Mode == 2)
				{
					Item.damage = 600;
					Item.useTime = 60;
					Item.useAnimation = 60;
					Item.reuseDelay = 0;
					Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/Railgun");
				}
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 4;

			if (player.altFunctionUse == 2)
				type = 0;
			else
			{
				if (Mode == 0 && !player.HasBuff(ModContent.BuffType<Buffs.Debuffs.InfraRedCorrosion>()))
				{
					type = ModContent.ProjectileType<Projectiles.Ranged.Railgun.BMFGRay>();
					SoundEngine.PlaySound(GMR.GetSounds("Items/Ranged/railgunVariant", 2, 1f, 0f, 0.75f), player.Center);
				}
				else if (Mode == 1)
				{
					type = ModContent.ProjectileType<Projectiles.Ranged.Railgun.BMFGEnergy>();
					SoundEngine.PlaySound(SoundID.Item42, player.Center);
					SoundEngine.PlaySound(SoundID.Item41, player.Center);
				}
				else if (Mode == 2 && !player.HasBuff(ModContent.BuffType<Buffs.Debuffs.InfraRedCorrosion>()))
				{
					type = ModContent.ProjectileType<Projectiles.Ranged.Railgun.BMFGBeam>();
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
			recipe.AddIngredient(null, "AncientInfraRedPlating", 60);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 20);
			recipe.AddIngredient(ItemID.SoulofLight, 16);
			recipe.AddIngredient(null, "MagmaticShard", 20);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddIngredient(null, "SpecialUpgradeCrystal");
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}