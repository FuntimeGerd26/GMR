using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Magic.Others
{
	public class Crucifix : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Crucifix");
			Tooltip.SetDefault("Shoots hallowed energy in random directions that after some time home into enemies");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 40;
			Item.rare = 5;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = Item.sellPrice(silver: 70);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 48;
			Item.crit = 4;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.CrucifixEnergy>();
			Item.shootSpeed = 2f;
			Item.mana = 6;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(45));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 24);
			recipe.AddIngredient(ItemID.SoulofSight, 12);
			recipe.AddIngredient(ItemID.SoulofMight, 12);
			recipe.AddIngredient(ItemID.SoulofFright, 12);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}