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
	public class ChaosAngelEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Heals you by 10% of HP when you are below 20% HP and increases max HP by 10%" +
				"\nWhile in cooldown increases summon damage by 15% and decreases damage taken by 25% but can't naturally regenerate health" +
				"\nIncreases summon damage and attack speed by 9%\nIncreases health regeneration by 20%\nIncreases max sentries by 1\n'Im just tired man'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.rare = 7;
			Item.value = Item.sellPrice(silver: 100);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Summon) += 0.09f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.09f;
			player.maxTurrets += 1;
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
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ChaosAngelHalo");
			recipe.AddIngredient(null, "ChaosAngelShirt");
			recipe.AddIngredient(null, "ChaosAngelPants");
			recipe.AddIngredient(null, "ChaosAngelWings");
			recipe.AddIngredient(null, "AlloybloodWhip");
			recipe.AddIngredient(null, "BloodyMedkit");
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();
		}
	}
}