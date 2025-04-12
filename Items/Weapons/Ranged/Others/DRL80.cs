using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Others
{
	public class DRL80 : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 68;
			Item.height = 24;
			Item.rare = 7;
			Item.useTime = 70;
			Item.useAnimation = 70;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = SoundID.Item61;
			Item.value = Item.sellPrice(silver: 285);
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 90;
			Item.crit = 0;
			Item.knockBack = 12f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.MechRocket>();
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Rocket;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			type = ModContent.ProjectileType<Projectiles.Ranged.MechRocket>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.RocketLauncher);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddIngredient(null, "InfraRedBar", 28);
			recipe.AddRecipeGroup("IronBar", 30); // Iron or Lead
			recipe.AddIngredient(ItemID.SoulofNight, 20);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
		}
	}
}