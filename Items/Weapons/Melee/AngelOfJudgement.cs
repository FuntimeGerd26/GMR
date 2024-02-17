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
	public class AngelOfJudgement : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; //Count of items to research
			Item.AddElement(1);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 112;
			Item.height = 112;
			Item.rare = 6;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 285);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 60;
			Item.noMelee = true;
			Item.crit = 0;
			Item.knockBack = 8f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.AngelOfJudgement>();
			Item.shootSpeed = 24f;
			Item.scale = 0.8f;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = 60;
				Item.useAnimation = 60;
				Item.noMelee = true;
				Item.UseSound = SoundID.Item7;
				Item.noUseGraphic = true;
			}
			else
			{
				Item.useTime = 35;
				Item.useAnimation = 35;
				Item.noMelee = false;
				Item.noUseGraphic = false;
				Item.UseSound = SoundID.Item1;
			}
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Melee.AngelOfJudgementSpear>(), damage, knockback, player.whoAmI);
			}
			else
			{
				Projectile.NewProjectile(source, position, velocity * 0, type, damage * 2,
					knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
				Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<Projectiles.Melee.AngelOfJudgementThrow>(), damage, knockback, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 11);
			recipe.AddIngredient(ItemID.LightShard, 2);
			recipe.AddIngredient(ItemID.SoulofFlight, 18);
			recipe.AddIngredient(ItemID.SoulofLight, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}