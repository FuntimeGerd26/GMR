using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using GMR;
using GMR.Items.Accessories.SoulsContent.Enchantments;

namespace GMR.Items.Accessories.SoulsContent.Enchantments.Forces
{
	public class ForceOfReferences : ModItem
	{
		public static int[] Enchants => new int[]
   {
			ModContent.ItemType<AmalgamateEnchantment>(),
			ModContent.ItemType<AmalgamationEnchantment>(),
			ModContent.ItemType<ChaosAngelEnchantment>(),
			ModContent.ItemType<MemerEnchantment>(),
			ModContent.ItemType<MagmaticEnchantment>()
   };

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases invincibility frames by 3 seconds\nIncreases summon damage and attack speed by 10%\nIncreases knockback by 8% and increases health, health regen, armor penetration and damage reduction by 10%" +
				"\nGives the player the 'Plague Regen' buff\nWeapons have a chance to shoot 3 projectile that deal 75% damage and shoot an aditional special projectile" +
				"\nSummons 6 orbiting saws around you that inflict 'Mimir' to enemies, hide the accessory to distable this effect" +
				"\nIncreases luck cap and bullet damage by 20%\n" +
				"\nHeals you by 10% of HP when you are below 20% HP and increases max HP by 10%" +
				"\nWhile in cooldown increases summon damage by 15% and decreases damage taken by 25% but can't naturally regenerate health" +
				"\nGrants the effects of Star Veil, Shiny Stone, Honeycomb, Frozen Turtle Shell, Mana Flower and Celestial Cuffs" +
				"\nIncreases falling speed and melee size and increases defense by 10" +
				"\nMakes you explode into a rain of fireballs when hit\n'It's not orange'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.DamageType = DamageClass.Generic;
			Item.crit = 4;
			Item.knockBack = 14f;
			Item.width = 44;
			Item.height = 36;
			Item.rare = 11;
			Item.value = Item.sellPrice(silver: 340);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			// Amalgamation
			player.GetKnockback(DamageClass.Generic) += 0.08f;
			player.statLifeMax2 += (int)(player.statLifeMax / 10);
			player.lifeRegen += (int)(player.lifeRegen / 10);
			player.endurance += 0.1f;
			player.luckMaximumCap += player.luckMaximumCap / 5;
			player.bulletDamage += 0.2f;
			player.hasPaladinShield = true;

			// Memer
			player.GetDamage(DamageClass.Magic) += 0.15f;
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

			// Chaos Angel
			player.GetDamage(DamageClass.Summon) += 0.1f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.1f;
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
			if (ClientConfig.Instance.MultiplicateProj)
			{
				player.GPlayer().DevPlush = Item;
			}
			player.GPlayer().AwakeMayDress = true;
			if (!hideVisual)
			{
				player.GPlayer().MayDress = true;
				player.GPlayer().DevPlush = Item;
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
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			foreach (int ench in Enchants)
				recipe.AddIngredient(ench);

			recipe.AddTile(TileID.LunarCraftingStation);
			//recipe.AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"));
			recipe.Register();
		}
	}
}