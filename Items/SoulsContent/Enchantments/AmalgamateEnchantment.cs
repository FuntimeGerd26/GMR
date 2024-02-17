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
using GMR.Items.Accessories;

namespace GMR.Items.Accessories.SoulsContent.Enchantments
{
	public class AmalgamateEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.DamageType = DamageClass.Generic;
			Item.crit = 4;
			Item.knockBack = 14f;
			Item.width = 30;
			Item.height = 34;
			Item.rare = 8;
			Item.value = Item.sellPrice(silver: 100);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statManaMax2 += 30;
			player.AddBuff(ModContent.BuffType<Buffs.Buff.PlagueRegen>(), 2);
			player.GetAttackSpeed(DamageClass.Generic) += 0.10f;
			player.maxTurrets += 1;
			player.GPlayer().DevInmune = true;
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
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdHead");
			recipe.AddIngredient(null, "GerdBody");
			recipe.AddIngredient(null, "GerdLegs");
			recipe.AddIngredient(null, "ThousandSunWrath");
			recipe.AddIngredient(null, "MaskedPlagueBand");
			recipe.AddIngredient(null, "MayAwakeDress");
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();
		}
	}
}