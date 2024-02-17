using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class AncientBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 62;
			Item.rare = 3;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 185);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 15;
			Item.crit = -3;
			Item.knockBack = 2f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 20f;
			Item.useAmmo = AmmoID.Arrow;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y + 4;
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.AncientArrow>();
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ScrapFragment", 15);
			recipe.AddIngredient(ItemID.Wire, 20);
			recipe.AddIngredient(ItemID.Bone, 40);
			recipe.AddRecipeGroup("GMR:AnyGem", 4);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}