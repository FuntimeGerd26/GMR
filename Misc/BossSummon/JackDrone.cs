using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using GMR.NPCs.Bosses.Jack;

namespace GMR.Items.Misc.BossSummon
{
	public class JackDrone : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Summons an old machine'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12; // This helps sort inventory know that this is a boss summoning Item.
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.rare = 3;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(silver: 50);
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			// If you decide to use the below UseItem code, you have to include !NPC.AnyNPCs(id), as this is also the check the server does when receiving MessageID.SpawnBoss.
			// If you want more constraints for the summon item, combine them as boolean expressions:
			//    return !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<MinionBossBody>()); would mean "not daytime and no MinionBossBody currently alive"
			return !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<Jack>());
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				// If the player using the item is the client
				// (explicitely excluded serverside here)
				SoundEngine.PlaySound(SoundID.Roar, player.position);

				int type = ModContent.NPCType<Jack>();

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// If the player is not in multiplayer, spawn directly
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else
				{
					// If the player is in multiplayer, request a spawn
					// This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in MinionBossBody
					NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Bone, 20);
			recipe.AddIngredient(ItemID.Wire, 15);
			recipe.AddIngredient(ItemID.TungstenBar, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.Bone, 20);
			recipe2.AddIngredient(ItemID.Wire, 15);
			recipe2.AddIngredient(ItemID.TungstenBar, 20);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}