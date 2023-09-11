using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR.NPCs.Bosses.Jack;

namespace GMR.Items.Misc.Consumable
{
	public class JackDroneOld : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Drone");
            Tooltip.SetDefault("Calls an ancient machine at day time");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12; // This helps sort inventory know that this is a boss summoning Item.
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.rare = 3;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(silver: 50);
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.JackSummon>();
			Item.shootSpeed = 0.01f;
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 heading = new Vector2(0f, -3f);
			Projectile.NewProjectile(source, position, heading, Item.shoot, damage, knockback, player.whoAmI);
			return false;
		}

		public override bool CanUseItem(Player player)
		{
			// If you decide to use the below UseItem code, you have to include !NPC.AnyNPCs(id), as this is also the check the server does when receiving MessageID.SpawnBoss.
			// If you want more constraints for the summon item, combine them as boolean expressions:
			//    return Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<Jack>()); would mean "is daytime and no Jack currently alive"
			return Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<Jack>()) && player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.JackSummon>()] < 1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wire, 15);
			recipe.AddIngredient(null, "AlloyBox", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}