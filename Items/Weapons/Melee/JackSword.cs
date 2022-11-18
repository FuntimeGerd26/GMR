using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class JackSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Sword");
			Tooltip.SetDefault("''\nInflicts Partially Crystalized debuff to enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 62;
			Item.height = 76;
			Item.rare = 5;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 270);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 60;
			Item.crit = 4;
			Item.knockBack = 3f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.JackSwordScythe>();
			Item.shootSpeed = 12f;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.PartiallyCrystallized>(), 120);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdOldSword");
			recipe.AddIngredient(ItemID.SoulofNight, 28);
			recipe.AddRecipeGroup("GMR:AnyGem", 8);
			recipe.AddIngredient(null, "UpgradeCrystal", 48);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}