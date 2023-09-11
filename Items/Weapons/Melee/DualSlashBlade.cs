using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Melee
{
	public class DualSlashBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"'Blade mode'\nGrants 'Cutting Edge', 'Rapid Healing' and 'Wrath' buffs when hitting an enemy\nInflicts Venom on enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; //Count of items to research
		}

		public override void SetDefaults()
		{
			Item.width = 78;
			Item.height = 86;
			Item.rare = 8;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 125);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 85;
			Item.noMelee = true;
			Item.crit = 8;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.DualSlashBladeSwing>();
			Item.shootSpeed = 6f;
			Item.knockBack = 4f;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "DualSlashCutter");
			recipe.AddIngredient(ItemID.ChlorophyteBar, 24);
			recipe.AddIngredient(ItemID.Ectoplasm, 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "DualBlasterShooter");
			recipe2.Register();
		}
	}
}