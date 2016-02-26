using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended.UI
{
    public class UITimeRange : UIComponent
    {
        //[SerializeField]
        //protected UITextureAtlas m_Atlas;

        private UISlider CreateSlider(UISprite thumbObj)
        {
            var sliderSize = new Vector2(this.width - 50, 10);
            //var thumbOffset = new Vector2(0, 2);
            //var sliderBackgroundSprite = "BudgetSlider";

            var uiSlider = this.AddUIComponent<UISlider>();
            uiSlider.fillMode = UIFillMode.Fill;
            //uiSlider.backgroundSprite = sliderBackgroundSprite;
            uiSlider.canFocus = true;
            uiSlider.orientation = UIOrientation.Horizontal;
            uiSlider.minValue = 0;
            uiSlider.maxValue = 24f;
            uiSlider.stepSize = 1f;
            uiSlider.size = sliderSize;
            uiSlider.zOrder = 15;

            uiSlider.thumbObject = thumbObj;
            //uiSlider.thumbOffset = thumbOffset;
            uiSlider.thumbObject.zOrder = 20;

            return uiSlider;
        }

        //protected internal virtual void RenderBackground()
        //{
        //    if ((Object)this.atlas == (Object)null)
        //        return;
        //    UITextureAtlas.SpriteInfo spriteInfo = this.atlas[this.backgroundSprite];
        //    if (spriteInfo == (UITextureAtlas.SpriteInfo)null)
        //        return;
        //    Color32 color32 = this.ApplyOpacity(this.isEnabled ? this.color : this.disabledColor);
        //    UISprite.RenderOptions options = new UISprite.RenderOptions()
        //    {
        //        atlas = this.atlas,
        //        color = color32,
        //        fillAmount = 1f,
        //        flip = UISpriteFlip.None,
        //        offset = UIPivotExtensions.TransformToUpperLeft(this.pivot, this.size, this.arbitraryPivotOffset),
        //        pixelsToUnits = this.PixelsToUnits(),
        //        size = this.size,
        //        spriteInfo = spriteInfo
        //    };
        //    if (spriteInfo.isSliced)
        //        UISlicedSprite.RenderSprite(this.renderData, options);
        //    else
        //        UISprite.RenderSprite(this.renderData, options);
        //}

        //public UITextureAtlas atlas
        //{
        //    get
        //    {
        //        if ((Object)this.m_Atlas == (Object)null)
        //        {
        //            UIView uiView = this.GetUIView();
        //            if ((Object)uiView != (Object)null)
        //                this.m_Atlas = uiView.defaultAtlas;
        //        }
        //        return this.m_Atlas;
        //    }
        //    set
        //    {
        //        if (UITextureAtlas.Equals(value, this.m_Atlas))
        //            return;
        //        this.m_Atlas = value;
        //        this.Invalidate();
        //    }
        //}

        //public string backgroundSprite
        //{
        //    get
        //    {
        //        return this.m_BackgroundSprite;
        //    }
        //    set
        //    {
        //        if (!(value != this.m_BackgroundSprite))
        //            return;
        //        this.m_BackgroundSprite = value;
        //        this.Invalidate();
        //    }
        //}

        //protected override void OnRebuildRenderData()
        //{
        //    if ((Object)this.atlas == (Object)null)
        //        return;
        //    this.renderData.material = this.atlas.material;
        //    this.RenderBackground();
        //}

        public override void Start()
        {
            var background = this.AddUIComponent<UISprite>();
            background.spriteName = "SubcategoriesPanel";
            background.zOrder = 1;
            //background.color = new Color32(30, 38, 45, 255);
            background.position = new Vector3(0, 0, 0);
            background.width = this.width;
            background.height = this.height;

            //test code
            UISprite thumbObj1 = this.AddUIComponent<UISprite>();
            thumbObj1.spriteName = "SliderBudget";
            var uiSlider = CreateSlider(thumbObj1);
            uiSlider.position = new Vector3(0, -(this.height / 2) + uiSlider.size.y);

            UISprite thumbObj2 = this.AddUIComponent<UISprite>();
            thumbObj2.spriteName = "SliderBudget";
            thumbObj2.flip = UISpriteFlip.FlipVertical;
            var uiSlider2 = CreateSlider(thumbObj2);
            uiSlider2.position = new Vector3(0, -(this.height / 2));

            
            var sliderBackgroundSprite = this.AddUIComponent<UISprite>();
            sliderBackgroundSprite.spriteName = "BudgetSlider";
            sliderBackgroundSprite.fillDirection = UIFillDirection.Horizontal;
            sliderBackgroundSprite.position = new Vector3(0, -(this.height / 2) + 5);
            sliderBackgroundSprite.size = new Vector2(uiSlider.width, 10);
            sliderBackgroundSprite.zOrder = 2;

            //thumbObj1.zOrder = int.MaxValue;
            //thumbObj2.zOrder = int.MaxValue;

            base.Start();
        }
    }
}
