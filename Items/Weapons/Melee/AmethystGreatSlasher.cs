using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class AmethystGreatSlasher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Violet's Great Slasher");
			Tooltip.SetDefault("'Not only shouldn't you be holding it, but it somehow isn't falling apart'\nInflicts Crystal Sickness to enemies and Thoughtful to you");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 66;
			Item.height = 78;
			Item.rare = 6;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 120);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 50;
			Item.crit = 4;
			Item.knockBack = 5f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.AmethystSword>();
			Item.shootSpeed = 6f;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(1))
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 21);
			}
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Glimmering>(), 1200);
			player.AddBuff(ModContent.BuffType<Buffs.Debuffs.Thoughtful>(), 300);
		}
	}
}