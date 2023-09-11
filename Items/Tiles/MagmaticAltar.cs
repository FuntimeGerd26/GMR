using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Localization;
using GMR.NPCs.Bosses.MagmaEye;

namespace GMR.Items.Tiles
{
	public class MagmaticAltar : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.CountsAsLavaSource[Type] = true; // Make it so the tiles count as a lava source for crafting

			Main.tileSolid[Type] = false; // Count as solid for NPCs and more
			Main.tileLavaDeath[Type] = true; // Does it break on contact with lava?
			Main.tileFrameImportant[Type] = true; // Is it important for it to not overlap any tiles?

			AddMapEntry(new Color(125, 105, 25));

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2); //Make this tile be 2x2
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 }; // Size of the tiles on the sheet (Keep 16 x 16)
			TileObjectData.addTile(Type);

			AdjTiles = new int[] { Type };

			DustType = 6;
			HitSound = SoundID.Tink;
			MineResist = 10f;
			MinPick = 1;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Tiles.MagmaAltar>());
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;

			if (!NPC.AnyNPCs(ModContent.NPCType<MagmaEye>()))
			{
				player.noThrow = 2;
				player.cursorItemIconEnabled = true;
				player.cursorItemIconID = ModContent.ItemType<Items.Tiles.MagmaAltar>();
			}
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			if (!NPC.AnyNPCs(ModContent.NPCType<MagmaEye>()))
			{
				SoundEngine.PlaySound(SoundID.Roar, player.position);

				int type = ModContent.NPCType<MagmaEye>();
				int spawnX = (int)player.position.X + player.width / 2;
				int spawnY = (int)player.position.Y + player.height / 2 - 1100;
				NPC.NewNPC(player.GetSource_FromThis(), spawnX, spawnY, type, player.whoAmI, 0f, 0f, 0f, 0f);
			}

			return true;
		}
	}
}
