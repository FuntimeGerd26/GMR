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

namespace GMR.Items.Accessories.SoulsContent.Enchantments.Forces.Souls
{
	public class ChaosAngelSoul : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Angel Soul");
			Tooltip.SetDefault("Summons Chaos Hands to protect the player\nEnemies hit by the hands will have a severely lowered movement speed\nIncreases summon damage and attack speed by 5%\nIncreases wing time by 2 seconds\nIncreases all crit rate by 2%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
			Item.value = Item.sellPrice(silver: 285);
			Item.rare = 9;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Summon) += 0.05f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.05f;
			player.GetCritChance(DamageClass.Generic) += 2f;
			player.wingTimeMax += 120;
			player.GPlayer().ChaosSoul = true;
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.ChaosAngelSoulHand>()] < 1)
			{
				Vector2 velocity = new Vector2(0f, -1f);
				for (int i = 0; i < 1; i++)
				{
					Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, velocity, ModContent.ProjectileType<Projectiles.Summon.ChaosAngelSoulHand>(), 90, 14f, player.whoAmI);
				}
			}
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.ChaosAngelSoulHand0>()] < 1)
			{
				Vector2 velocity = new Vector2(0f, -1f);
				for (int y = 0; y < 1; y++)
				{
					Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, velocity, ModContent.ProjectileType<Projectiles.Summon.ChaosAngelSoulHand0>(), 90, 4f, player.whoAmI);
				}
			}
		}
	}
}