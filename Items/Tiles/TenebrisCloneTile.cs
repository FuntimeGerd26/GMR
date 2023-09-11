using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace GMR.Items.Tiles
{
	public class TenebrisCloneTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;

			AddMapEntry(new Color(0, 100, 100));

			DustType = 84;
			HitSound = SoundID.Tink;
			MineResist = 2f;
			MinPick = 80;
		}
	}
}
