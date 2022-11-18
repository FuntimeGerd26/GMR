using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class InfraRedVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Visor");
			Tooltip.SetDefault("Increases crit chance and attack speed by 2%\nIncreases ranged and magic damage by 3%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 30;
			Item.value = Item.sellPrice(silver: 110);
			Item.rare = 5;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.03f;
			player.GetDamage(DamageClass.Ranged) += 0.03f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.02f;
			player.GetCritChance(DamageClass.Generic) += 2f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<InfraRedPlating>() && legs.type == ModContent.ItemType<InfraRedGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases all damage by 4%\n[i:{ ModContent.ItemType<UI.ItemEffectIcon>()}] Creates an aura around the player that damages enemies\n'Hello World'";
			player.GetDamage(DamageClass.Generic) += 0.04f;
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.InfraRedAura>()] < 1)
			{
				Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, player.velocity * 0f, ModContent.ProjectileType<Projectiles.InfraRedAura>(), 20, 0f, player.whoAmI, 0);
			}
			player.GPlayer().InfraRedSet = true;
		}
	}
}