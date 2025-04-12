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

namespace GMR.Items.Weapons.Melee.Others
{
	public class PsycopathAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Throw an axe that spins in a spot after a little of being shot");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.width = 50;
			Item.height = 44;
			Item.rare = 2;
			Item.useTime = 80;
			Item.useAnimation = 80;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 150);
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 20;
			Item.crit = 0;
			Item.knockBack = 8f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.PsycopathAxe>();
			Item.shootSpeed = 6f;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Projectile.NewProjectile(Item.GetSource_FromThis(), position, velocity.RotatedBy(MathHelper.ToRadians(-90f)), type, damage, knockback, player.whoAmI);
			type = 0;
		}
	}
}