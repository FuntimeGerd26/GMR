using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class GerdHeroSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hero Training Sword");
			Tooltip.SetDefault("Shoots swords that rotate around you and over time shoot homing swords\nIncreases melee speed and all damage by 5% and increases critical chance by 7% while in your inventory");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 70;
			Item.height = 70;
			Item.rare = 4;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 75);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 58;
			Item.crit = 14;
			Item.knockBack = 1.5f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.GerdHeroSwordThrow>();
			Item.shootSpeed = 6f;
		}

		public override void UpdateInventory(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetDamage(DamageClass.Generic) += 0.05f;
			player.GetCritChance(DamageClass.Generic) += 7f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdOldSword");
			recipe.AddIngredient(ItemID.PearlstoneBlock, 38);
			recipe.AddIngredient(ItemID.SoulofNight, 14);
			recipe.AddIngredient(ItemID.SoulofLight, 14);
			recipe.AddIngredient(ItemID.Bone, 36);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}