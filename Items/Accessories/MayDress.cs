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

namespace GMR.Items.Accessories
{
	public class MayDress : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Violet's Dress");
			Tooltip.SetDefault("'It's so sad'\nIncreases invincibility frames by 2 seconds\nWeapons have a chance to shoot 3 projectile that deal 75% damage and shoot an aditional special projectile" +
				"\nIncreases attack speed and damage taken by 10%" +
                "\nSummons 4 orbiting swords around you");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 64;
			Item.value = Item.sellPrice(silver: 240);
			Item.DamageType = ModContent.GetInstance<Classless>();
			Item.damage = 50;
			Item.crit = 4;
			Item.knockBack = 4f;
			Item.rare = 5;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GPlayer().MayDress = true;
			player.GPlayer().DevPlush = Item;
			player.GPlayer().DevInmune = true;
			player.GetAttackSpeed(DamageClass.Generic) += 0.10f;
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.MaySword>()] < 1)
			{
				const int max = 4;
				Vector2 velocity = new Vector2(0f, -7f);
				for (int i = 0; i < max; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(2 * Math.PI / max * i);
					Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Summon.MaySword>(), 50, 4f, player.whoAmI, 0, (player.Center - 140 * Vector2.UnitX - player.position).Length());
				}
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BlackDye, 1);
			recipe.AddIngredient(ItemID.Silk, 14);
			recipe.AddIngredient(ItemID.SoulofNight, 8);
			recipe.AddIngredient(null, "DevPlushie");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}