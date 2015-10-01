﻿using Cairo;

namespace IMGUI
{
    public partial class Style
    {
        #region Box model styles
        #region Padding

        /// <summary>
        /// padding-top
        /// </summary>
        public Length PaddingTop { get; set; }

        /// <summary>
        /// padding-right
        /// </summary>
        public Length PaddingRight { get; set; }

        /// <summary>
        /// padding-bottom
        /// </summary>
        public Length PaddingBottom { get; set; }

        /// <summary>
        /// padding-left
        /// </summary>
        public Length PaddingLeft { get; set; }

        #endregion
        
        #region Border

        /// <summary>
        /// border-top-width
        /// </summary>
        public Length BorderTop { get; set; }

        /// <summary>
        /// border-right-width
        /// </summary>
        public Length BorderRight { get; set; }

        /// <summary>
        /// border-bottom-width
        /// </summary>
        public Length BorderBottom { get; set; }

        /// <summary>
        /// border-left-width
        /// </summary>
        public Length BorderLeft { get; set; }

        /// <summary>
        /// Color of the top
        /// </summary>
        public Color BorderTopColor { get; set; }

        /// <summary>
        /// Color of the right
        /// </summary>
        public Color BorderRightColor { get; set; }

        /// <summary>
        /// Color of the bottom
        /// </summary>
        public Color BorderBottomColor { get; set; }

        /// <summary>
        /// Color of the left
        /// </summary>
        public Color BorderLeftColor { get; set; }

        #endregion
        
        #region Margin

        /// <summary>
        /// margin-top
        /// </summary>
        public Length MarginTop { get; set; }

        /// <summary>
        /// margin-right
        /// </summary>
        public Length MarginRight { get; set; }

        /// <summary>
        /// margin-bottom
        /// </summary>
        public Length MarginBottom { get; set; }

        /// <summary>
        /// margin-left
        /// </summary>
        public Length MarginLeft { get; set; }

        #endregion

        #region Background

        public BackgroundStyle BackgroundStyle { get; set; }

        #endregion
        #endregion

        #region Text

        public Font Font { get; set; }

        public TextStyle TextStyle { get; set; }

        #endregion

        #region Cursor

        public Cursor Cursor { get; set; }

        #endregion

        #region Verctor graphic styles
        public Color LineColor { get; set; }
        public int LineWidth { get; set; }
        public Color FillColor { get; set; }
        #endregion

        /// <summary>
        /// Set defalut style for all kinds of box model
        /// </summary>
        private Style()
        {
            PaddingTop = PaddingRight = PaddingBottom = PaddingLeft = Length.Zero;
            BorderTop = BorderRight = BorderBottom = BorderLeft = Length.Zero;
            BorderTopColor = CairoEx.ColorBlack;
            BorderRightColor = CairoEx.ColorBlack;
            BorderBottomColor = CairoEx.ColorBlack;
            BorderLeftColor = CairoEx.ColorBlack;
            MarginTop = MarginRight = MarginBottom = MarginLeft = Length.Zero;

            BackgroundStyle = new BackgroundStyle
            {
                Color = CairoEx.ColorWhite,
                Image = null,
                Pattern = null
            };

            Font = new Font
            {
                FontFamily = "Consolas",
                FontStyle = FontStyle.Normal,
                FontWeight = FontWeight.Normal,
                FontStretch = FontStretch.Normal,
                Size = 12,
                Color = CairoEx.ColorBlack
            };

            TextStyle = new TextStyle
            {
                TextAlignment = TextAlignment.Center,
                LineSpacing = 0,
                TabSize = 4
            };

            Cursor = Cursor.Default;

            LineColor = CairoEx.ColorBlack;
            LineWidth = 1;
            FillColor = new Color(1,1,1);
        }


    }
}
