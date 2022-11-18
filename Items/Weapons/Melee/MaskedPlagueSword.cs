using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class MaskedPlagueSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Sword");
			Tooltip.SetDefault($"[i:{ModContent.ItemType<UI.ItemEffectIcon>()}] Right-click to use as a spear that shoots extra projectiles");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.width = 68;
			Item.height = 78;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = 6;
			Item.value = Item.sellPrice(silver: 100);
			Item.DamageType = DamageClass.Melee;
			Item.crit = 4;
			Item.knockBack = 4f;
			Item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		
		public override bool CanUseItem(Player player)
		{
			// Ensures no more than one projectile can be thrown out, use this when using autoReuse
			return player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.MaskedPlagueSwordStinger>()] < 1;
		}

		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.damage = 38;
				Item.useTime = 20;
				Item.useAnimation = 20;
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noMelee = true;
				Item.UseSound = SoundID.Item7;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.MaskedPlagueSwordStinger>();
				Item.shootSpeed = 6f;
				Item.noUseGraphic = true;
			}
			else
			{
				Item.damage = 45;
				Item.useTime = 10;
				Item.useAnimation = 10;
				Item.useStyle = ItemUseStyleID.Swing;
				Item.noMelee = false;
				Item.noUseGraphic = false;
				Item.UseSound = SoundID.Item1;
				Item.shoot = 0;
				Item.scale = 1.5f;
			}
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 18);
			recipe.AddIngredient(ItemID.Silk, 20);
			recipe.AddIngredient(ItemID.Bone, 30);
			recipe.AddRecipeGroup("GMR:AnyGem", 7);
			recipe.AddIngredient(ItemID.SoulofNight, 14);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}