using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class ElementalSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amalgamated Spear");
			Tooltip.SetDefault($"'Organic materials were unharmed durning the making'\n Shoot petals that when hitting enemies become sun spikeballs\n Hitting enemies with the spear will create ocean coins that will fall on top of enemies");

			ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
			ItemID.Sets.Spears[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(1);
			Item.AddElement(2);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.rare = 7;
			Item.value = Item.sellPrice(silver: 230); 
			Item.useStyle = ItemUseStyleID.Shoot; 
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = true; 
			Item.damage = 85;
			Item.knockBack = 4f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.ElementalSpear>();
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool? UseItem(Player player)
		{
			if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}

			return null;
		}
	}
}