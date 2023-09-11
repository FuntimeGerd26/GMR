using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Magic
{
	public class MaskedPlagueTome : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Tome");
			Tooltip.SetDefault("Shoots a plague bolt that inflicts poison and homes into enemies, having this in your inventory increases magic damage and mana by 5%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 58;
			Item.rare = 2;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 75);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 34;
			Item.crit = 14;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.PlagueScythe>();
			Item.shootSpeed = 5f;
			Item.mana = 12;
		}

		public override void UpdateInventory(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.05f;
			player.statManaMax2 += player.statManaMax / 20;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity / 100f;
			Projectile.NewProjectile(Item.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<Projectiles.Magic.PlagueScythe>(), damage, knockback, player.whoAmI);
			type = 0;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 18);
			recipe.AddIngredient(ItemID.Silk, 20);
			recipe.AddRecipeGroup("GMR:AnyGem", 7);
			recipe.AddIngredient(null, "UpgradeCrystal", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}