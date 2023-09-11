using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace GMR.Items.Tiles
{
	public class JuiceBoxTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.CountsAsWaterSource[Type] = true; // Make it so the tiles count as a water source for crafting
			TileID.Sets.CountsAsHoneySource[Type] = true; // Make it so the tiles count as a lava source for crafting
			TileID.Sets.CountsAsLavaSource[Type] = true; // Make it so the tiles count as a honey source for crafting

			Main.tileSolid[Type] = false; // Full block?
			Main.tileLavaDeath[Type] = true; // Does it break on contact with lava?
			Main.tileFrameImportant[Type] = true; // No idea tbh, it might be for non-1x1 tiles

			AddMapEntry(new Color(125, 80, 115));

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2); //Make this tile be 2x2
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 }; // Size of the tiles on the sheet (Keep 16 x 16)
			TileObjectData.addTile(Type);

			AdjTiles = new int[] { Type };

			DustType = 60;
			HitSound = SoundID.Tink;
			MineResist = 4f;
			MinPick = 1;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Tiles.JuiceBox>());
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.player[0];
				player.AddBuff(ModContent.BuffType<Buffs.Buff.Oilful>(), 30);
			}
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Tiles.JuiceBox>();
		}
	}
}
