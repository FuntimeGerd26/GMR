using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Weapons.Ranged.Others
{
	public class InfraRedFlamethrower : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 88;
			Item.height = 28;
			Item.rare = 5;
			Item.useTime = 8;
			Item.useAnimation = 56;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 385);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item34;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 30;
			Item.crit = 1;
			Item.knockBack = 8f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Flames;
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Gel;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, -2);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "InfraRedBar", 20);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 10);
			recipe.AddIngredient(ItemID.HallowedBar, 28);
			recipe.AddIngredient(ItemID.SoulofSight, 18);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}