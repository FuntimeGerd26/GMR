using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class SG14 : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 104;
			Item.height = 36;
			Item.rare = 6;
			Item.useTime = 90;
			Item.useAnimation = 90;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 285);
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 100;
			Item.crit = 10;
			Item.knockBack = 6f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 32f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, -8);
		}
		public override bool CanUseItem(Player player)
		{
			SoundEngine.PlaySound(SoundID.Item40.WithPitchOffset(0.25f), player.Center);
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 4;
			if (type == ProjectileID.Bullet)
				type = ModContent.ProjectileType<Projectiles.Ranged.MechBullet>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 25); // Iron or Lead
			recipe.AddIngredient(null, "InfraRedBar", 24);
			recipe.AddIngredient(ItemID.SoulofFright, 18);
			recipe.AddIngredient(ItemID.SoulofNight, 20);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
		}
	}
}