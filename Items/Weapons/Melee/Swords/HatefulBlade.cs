using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class HatefulBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.NewSwords.HatefulBlade>(36);
			Item.SetWeaponValues(22, 2f, 2);
			Item.width = 74;
			Item.height = 74;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 195);
			Item.rare = 2;
			Item.autoReuse = true;
		}

		public override bool? UseItem(Player player)
		{
			Item.FixSwing(player);
			return null;
		}

		public override bool MeleePrefix()
		{
			return true;
		}
	}
}