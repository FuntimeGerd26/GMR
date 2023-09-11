using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Summoner.Whips
{
	public class PlanteraWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts Venom to enemies" +
			$"\n Hitting enemies has a chance for them to shoot thorns in random directions");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 56;
			Item.height = 46;
			Item.rare = 7;
			Item.value = Item.sellPrice(silver: 145);
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.Summon.Whips.PlanteraWhip>(), 58, 6, 14);
			Item.shootSpeed = 6f;
		}

		public override bool MeleePrefix()
		{
			return true;
		}
	}
}