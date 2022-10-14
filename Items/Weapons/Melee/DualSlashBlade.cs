using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Melee
{
	public class DualSlashBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Blade mode'\nHitting an enemy gives the player 'Dual Cutting Edge' buff for 3 seconds\nRight-click to throw the sword as a boomerang");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; //Count of items to research
		}

		public override void SetDefaults()
		{
			Item.width = 60;
			Item.height = 58;
			Item.rare = 7;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 125);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 95;
			Item.crit = 4;
			Item.knockBack = 7f;
			Item.scale = 2f;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			// Ensures no more than one projectile can be thrown out, use this when using autoReuse
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.noMelee = true;
				Item.noUseGraphic = true;
				Item.UseSound = SoundID.Item7;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.DualSlashBladeThrow>();
				Item.shootSpeed = 14f;
				Item.autoReuse = true;
			}
			else
			{
				Item.noMelee = false;
				Item.noUseGraphic = false;
				Item.UseSound = SoundID.Item1;
				Item.shoot = 0;
				Item.shootSpeed = 0f;
			}
			return true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			player.AddBuff(ModContent.BuffType<Buffs.Buff.CuttingEdge>(), 180);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "DualSlashShooterDX");
			recipe.Register();
		}
	}
}