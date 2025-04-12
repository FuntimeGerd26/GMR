using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace GMR.Items.Weapons.Melee.Others
{
	public class CyberNeonAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 100;
			Item.height = 100;
			Item.rare = 7;
			Item.useTime = 2;
			Item.useAnimation = 2;
			Item.reuseDelay = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 250);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 200;
			Item.crit = 0;
			Item.knockBack = 7f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useTurn = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.CyberNeonAxe>();
			Item.shootSpeed = 18f;
		}

		public override bool CanShoot(Player player)
		{
			return player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.CyberNeonAxeThrow>()] <= 0;
		}
	}
}