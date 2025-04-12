using Microsoft.Xna.Framework;
using Terraria.UI;

namespace GMR.GerdUI
{
    public static class GerdUI
    {
        public static void SetRectangle(this UIElement uiElement, float left, float top, float width, float height)
        {
            uiElement.Left.Set(left, 0f);
            uiElement.Top.Set(top, 0f);
            uiElement.Width.Set(width, 0f);
            uiElement.Height.Set(height, 0f);
        }

        public static void SetRectangle(this UIElement uiElement, Rectangle rect)
        {
            uiElement.Left.Set(rect.X, 0f);
            uiElement.Top.Set(rect.Y, 0f);
            uiElement.Width.Set(rect.Width, 0f);
            uiElement.Height.Set(rect.Height, 0f);
        }

        public static void SetRectangle(this UIElement uiElement, Vector2 pos, Vector2 dimensions)
        {
            uiElement.Left.Set(pos.X, 0f);
            uiElement.Top.Set(pos.Y, 0f);
            uiElement.Width.Set(dimensions.X, 0f);
            uiElement.Height.Set(dimensions.Y, 0f);
        }
    }
}
