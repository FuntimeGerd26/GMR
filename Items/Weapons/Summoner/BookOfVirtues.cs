using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using GMR;

namespace GMR.Items.Weapons.Summoner
{
	public class BookOfVirtues : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons wisps to fight for you\nWisps orbit around you shooting at close enemies\nWisps deal 0.005% of an enemy's max HP as damage" +
				"\nDamage is increased in hardmode by [c/66FF66:x3], and after beating Moonlord by [c/66FF66:x6]\nWisps inflict Frorstburn to enemies");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			Item.AddElement(0);
			Item.AddElement(1);
		}

		public override void SetDefaults()
		{
			Item.damage = 1;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 10;
			Item.width = 26;
			Item.height = 30;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.noMelee = true;
			Item.knockBack = 4f;
			Item.value = Item.buyPrice(silver: 260);
			Item.rare = 2;
			Item.UseSound = SoundID.Item44;
			Item.shoot = ModContent.ProjectileType<Projectiles.Summon.VirtueWisps>();
			Item.buffType = ModContent.BuffType<Buffs.Minions.VirtueWisps>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.AddBuff(Item.buffType, 2);
			if (player.ownedProjectileCounts[Item.shoot] < (int)(player.maxMinions))
			{
				int max = (int)(player.maxMinions) - player.ownedProjectileCounts[Item.shoot];
				Vector2 newVelocity = new Vector2(0f, -3f);
				for (int i = 0; i < max; i++)
				{
					Vector2 perturbedSpeed = newVelocity.RotatedBy(2 * Math.PI / max * i);
					Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed, Item.shoot, damage, Item.knockBack,
						player.whoAmI, 0, (player.Center - 100 * Vector2.UnitX - player.position).Length());
				}
			}
			return false;
		}
	}
}