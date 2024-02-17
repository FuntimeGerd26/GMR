using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;
using GMR;
using GMR.Items.CustomStuff;
using GMR.Items.Accessories.SoulsContent.Enchantments.Forces;

namespace GMR.Items.Accessories.SoulsContent.Enchantments.Forces.Souls
{
	public class VioletsSoul : ModItem
	{
		private static readonly Color[] itemNameCycleColors = {
			new Color(205, 205, 205),
			new Color(135, 70, 205),
			new Color(75, 75, 75),
		};

		public bool flip;
		public int Timer;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Violet's Soul");
			Tooltip.SetDefault("'I am still not writing the whole tooltip for this'" +
				"\nGreatly increases damage, attack speed, knockback, movement speed, health, life regeneration, mana, damage reduction and aggro" +
				$"\nSpawnrate is increased to x5" +
				$"\nIncreases ranged velocity, you randomly shoot rockets, when hit you release crystal shards and fireballs" +
				$"\nGives the player buffs, increases luck, below 20% health you will be healed, grants the effects of the following: Star Veil, Shiny Stone, Honey Comb, Frozen Turtle Shell, Mana Flower, Celestial Cuffs" +
				$"\nEvery 2 seconds shoots a projectile that homes, attacking has a chance to shoot 5 of the projectile you shot and summons orbiting projectiles if not hidden" +
				$"\nAttacks shoot extra projectiles, hiding this accessory distables most of them");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.width = 78;
			Item.height = 90;
			Item.value = Item.sellPrice(silver: 500);
			Item.damage = 200;
			Item.DamageType = DamageClass.Generic;
			Item.crit = 14;
			Item.knockBack = 18f;
			Item.rare = 10;
			Item.accessory = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			// Halu
			player.aggro *= 500;
			player.GPlayer().Halu = Item;

			#region Force of Utilities
			player.GetDamage(DamageClass.Generic) += 0.30f;
			player.GetKnockback(DamageClass.Generic) += 0.30f;
			player.GetArmorPenetration(DamageClass.Generic) += 0.30f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.30f;
			player.endurance += 0.15f;
			player.moveSpeed += 0.45f;
			player.statLifeMax2 += player.statLifeMax / 5;
			player.maxMinions += 2;

			// Magnum
			player.GPlayer().MagnumSet = true;
			if (!hideVisual)
				player.GPlayer().ChargedArm = Item;

			// Masked Plague
			if (!hideVisual)
				player.GPlayer().MaskedPlagueCloak = Item;

			// Amethyst Golem
			player.GPlayer().AmethystSet = true;

			// Aluminium
			if (!hideVisual)
			{
				player.GPlayer().AlumArmor = Item;
				player.GPlayer().AluminiumCharm = Item;
			}

			// Armored Bull
			player.GPlayer().BullSet = true;
			#endregion


			#region Force of References
			// Amalgamation
			player.statLifeMax2 += (int)(player.statLifeMax / 10);
			player.lifeRegen += (int)(player.lifeRegen / 10);
			player.luckMaximumCap += player.luckMaximumCap / 5;
			player.bulletDamage += 0.20f;
			player.hasPaladinShield = true;

			// Memer
			player.honeyCombItem = Item;
			if (player.statLife <= player.statLifeMax2 * 0.5)
				player.AddBuff(BuffID.IceBarrier, 5, true);
			player.manaFlower = true;
			player.manaMagnet = true;
			player.maxFallSpeed += 1.5f;
			player.meleeScaleGlove = true;
			player.shinyStone = true;
			player.starCloakItem = Item;
			player.starCloakItem_starVeilOverrideItem = Item;
			player.statDefense += 10;

			player.GetJumpState<MemerEnchJump>().Enable();

			// Chaos Angel
			if (player.statLife < player.statLifeMax2 / 5 && !player.HasBuff(ModContent.BuffType<Buffs.Debuffs.PainfullyHealed>()) && !Main.dedServ)
			{
				player.lifeRegen += (int)(player.lifeRegen * 0.2);
				player.statLife += player.statLifeMax2 / 10;
				player.AddBuff(ModContent.BuffType<Buffs.Debuffs.PainfullyHealed>(), 2400);
			}
			else if (player.HasBuff(ModContent.BuffType<Buffs.Debuffs.PainfullyHealed>()) && (player.lifeRegen > 0 || player.lifeRegen == 0))
			{
				player.lifeRegen = 0;
			}

			// Amalgamate
			player.AddBuff(ModContent.BuffType<Buffs.Buff.PlagueRegen>(), 2);
			player.maxTurrets += 1;
			player.GPlayer().AwakeMayDress = true;
			player.GPlayer().AmalgamateEnch = true;
			if (ClientConfig.Instance.GoldenEmpire)
			{
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.GoldenEmpire>()] < 1)
				{
					Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, player.velocity, ModContent.ProjectileType<Projectiles.Summon.GoldenEmpire>(), Item.damage, Item.knockBack, player.whoAmI);
				}
			}
			if (hideVisual)
			{
			}
			else
			{
				player.GPlayer().MayDress = true;
				if (ClientConfig.Instance.MultiplicateProj)
				{
					player.GPlayer().DevPlush = Item;
				}
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.MayAwakeSaw>()] < 1)
				{
					const int max = 6;
					Vector2 velocity = new Vector2(0f, -6f);
					for (int i = 0; i < max; i++)
					{
						Vector2 perturbedSpeed = velocity.RotatedBy(2 * Math.PI / max * i);
						Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Summon.MayAwakeSaw>(),
							Item.damage, Item.knockBack, player.whoAmI, 0, (player.Center - 250 * Vector2.UnitX - player.position).Length());
					}
				}
			}

			// Magmatic
			player.GetArmorPenetration(DamageClass.Generic) += 0.1f;
			player.GPlayer().MagmaSet = true;
			#endregion


			#region Force of Curses
			// Boost
			if (!hideVisual)
				player.GPlayer().BoostSet = Item;
			player.GPlayer().Thunderblade = Item;
			if (ClientConfig.Instance.NajaFireball)
			{
				player.GPlayer().NajaCharm = Item;
			}
			player.waterWalk = true;
			player.fireWalk = true;
			player.wingTimeMax += player.wingTimeMax / 5;
			player.runAcceleration += 0.15f;
			player.maxRunSpeed += 0.75f;
			player.manaCost -= 0.15f;

			// Ice Princess
			if (!hideVisual)
				player.GPlayer().IcePrincessEnch = Item;
			player.frostArmor = true;
			player.statManaMax += 40;


			// Infra-Red
			player.GPlayer().InfraRedSet = Item;
			player.jumpSpeedBoost += 0.20f;

			// Sandwave
			player.AddBuff(ModContent.BuffType<Buffs.Debuffs.DamnSun>(), 2);
			player.AddBuff(ModContent.BuffType<Buffs.Buff.Empowered>(), 2);
			if (!hideVisual && ++Timer % 120 == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.SandwaveKnife>()] < 1)
			{
				for (int i = 0; i < 1; i++)
				{
					Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, new Vector2(0f, -6f), ModContent.ProjectileType<Projectiles.SandwaveKnife>(), Item.damage, 6f, player.whoAmI);
				}
			}

			// Ark
			player.AddBuff(ModContent.BuffType<Buffs.Buff.BloodFountain>(), 2);
			player.maxMinions += 3;
			player.maxTurrets += 3;
			if (ClientConfig.Instance.AlloybloodDagger)
			{
				player.GPlayer().AlloybloodEnch = Item;
			}

			if (player.statLife < player.statLifeMax / 2)
			{
				player.manaCost -= 0.10f;
				player.GetDamage(DamageClass.Generic) += 0.14f;
				player.GetCritChance(DamageClass.Generic) += 14f;
				player.GetArmorPenetration(DamageClass.Generic) += 14f;
			}

			if (!hideVisual)
			{
				player.GPlayer().BLBook = true;
				flip = !flip;
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.BLFujoshi>()] < 1)
				{
					const int max = 6;
					Vector2 velocity = new Vector2(0f, -2f);
					for (int i = 0; i < max; i++)
					{
						Vector2 perturbedSpeed = velocity.RotatedBy(2 * Math.PI / max * i);
						Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Melee.BLFujoshi>(), Item.damage, 14f,
							player.whoAmI, 0, (player.Center - 100 * Vector2.UnitX - player.position).Length() * (flip ? 1 : 1));
					}
				}
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.BLPsycopathAxe>()] < 1)
				{
					const int max2 = 2;
					Vector2 velocity = new Vector2(0f, -6f);
					for (int y = 0; y < max2; y++)
					{
						Vector2 perturbedSpeed2 = velocity.RotatedBy(2 * Math.PI / max2 * y);
						Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed2, ModContent.ProjectileType<Projectiles.Melee.BLPsycopathAxe>(), Item.damage + 10, 4f,
							player.whoAmI, 0, (player.Center - 200 * Vector2.UnitX - player.position).Length() * (flip ? 1 : 1));
					}
				}
			}
			#endregion
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int numColors = itemNameCycleColors.Length;

			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.Mod == "Terraria" && line2.Name == "ItemName")
				{
					float fade = (Main.GameUpdateCount % 30) / 30f;
					int index = (int)((Main.GameUpdateCount / 30) % numColors);
					int nextIndex = (index + 1) % numColors;

					line2.OverrideColor = Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[nextIndex], fade);
				}
			}
		}

		public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
		{
			if (line.Mod == "Terraria" && line.Name == "ItemName")
			{
				DrawDedicatedTooltip(line);
				return false;
			}
			return true;
		}

		public static void DrawDedicatedTooltip(string text, int x, int y, float rotation, Vector2 origin, Vector2 baseScale, Color color)
		{
			float brightness = Main.mouseTextColor / 255f;
			float brightnessProgress = (Main.mouseTextColor - 225f) / (byte.MaxValue - 225f);
			color = Colors.AlphaDarken(color);
			color.A = 0;
			var font = FontAssets.MouseText.Value;
			ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text, new Vector2(x, y), new Color(0, 0, 0, 255), rotation, origin, baseScale);
			for (float f = 0f; f < MathHelper.TwoPi; f += MathHelper.PiOver2 + 0.01f)
			{
				var coords = new Vector2(x, y);
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text, coords, new Color(0, 0, 0, 255), rotation, origin, baseScale);
			}
			for (float f = 0f; f < MathHelper.TwoPi; f += MathHelper.PiOver2 + 0.01f)
			{
				var coords = new Vector2(x, y) + f.ToRotationVector2() * (brightness / 2f);
				ChatManager.DrawColorCodedString(Main.spriteBatch, font, text, coords, color * 0.8f, rotation, origin, baseScale);
			}
			for (float f = 0f; f < MathHelper.TwoPi; f += MathHelper.PiOver4 + 0.01f)
			{
				var coords = new Vector2(x, y) + (f + Main.GlobalTimeWrappedHourly).ToRotationVector2() * (brightnessProgress * 3f);
				ChatManager.DrawColorCodedString(Main.spriteBatch, font, text, coords, color * 0.2f, rotation, origin, baseScale);
			}
		}
		public static void DrawDedicatedTooltip(string text, int x, int y, Color color)
		{
			DrawDedicatedTooltip(text, x, y, 0f, Vector2.Zero, Vector2.One, color);
		}
		public static void DrawDedicatedTooltip(DrawableTooltipLine line)
		{
			DrawDedicatedTooltip(line.Text, line.X, line.Y, line.Rotation, line.Origin, line.BaseScale, line.OverrideColor.GetValueOrDefault(line.Color));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ForceOfUtilities");
			recipe.AddIngredient(null, "ForceOfReferences");
			recipe.AddIngredient(null, "ForceOfCurses");
			recipe.AddIngredient(null, "Halu");
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}