using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class AmethystHandgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Violet's Handgun");
			Tooltip.SetDefault("'There's no safe mode'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 42;
			Item.height = 22;
			Item.rare = 2;
			Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 50);
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item41;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 12;
			Item.crit = -2;
			Item.knockBack = 5f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 8f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, -4);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.FlintlockPistol);
			recipe.AddRecipeGroup("IronBar", 5); // Iron/Lead
			recipe.AddRecipeGroup("GMR:AnyGem", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}