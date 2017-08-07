using System;
using Xamarin.Forms;

namespace MonkeyCode.Framework.Portable.XamarinForms.Controls
{
    public class SwipePage<TItem> : ContentView, IMultiItemPage<TItem>
    {
        public static BindableProperty ItemsSourceProperty = BindableProperty.CreateAttached(nameof(ItemsSource), typeof(TItem[]),
            typeof(SwipePage<TItem>), new TItem[0], propertyChanged:OnItemsSourceChanged);

        public static BindableProperty ItemTemplateProperty = BindableProperty.CreateAttached(nameof(ItemTemplate), typeof(DataTemplate),
            typeof(SwipePage<TItem>), GetDefaultTempalte(), propertyChanged:OnDataTemplateChanged);

        private static void OnDataTemplateChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            (bindable as SwipePage<TItem>)?.LayoutSetup();
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            (bindable as SwipePage<TItem>)?.ItemsSetup();
        }

        private static DataTemplate GetDefaultTempalte()
        {
            return null;
        }

        public TItem[] ItemsSource
        {
            get { return (TItem[])this.GetValue(ItemsSourceProperty); }
            set { this.SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)this.GetValue(ItemTemplateProperty); }
            set { this.SetValue(ItemTemplateProperty, value); }
        }

        //private void Setup()
        //{
        //    if (this.ItemsSource.Any())
        //    {
        //        this.BindingContext = this.ItemsSource.First();
        //    }

        //    if (this.ItemTemplate != null)
        //    {
        //        this.Content = (View) this.ItemTemplate.CreateContent();
        //    }
        //}

        // back card scale
        private const float BackCardScale = 0.8f;
        // speed of the animations
        private const int AnimLength = 250;
        // 180 / pi
        private const float DegreesToRadians = 57.2957795f;
        // higher the number less the rotation effect
        private const float CardRotationAdjuster = 0.3f;
        // distance a card must be moved to consider to be swiped off
        public int CardMoveDistance { get; set; }

        // two cards
        private const int NumCards = 2;
        private readonly View[] _views = new View[NumCards];
        // the card at the top of the stack
        private int _topCardIndex;
        // distance the card has been moved
        private float _cardDistance;
        // the last items index added to the stack of the cards
        private int _itemIndex;
        private bool _ignoreTouch;

        // called when a card is swiped left/right with the card index in the ItemSource
        public Action<int> SwipedRight = null;
        public Action<int> SwipedLeft = null;

        public void LayoutSetup()
        {
            var view = new RelativeLayout();

            // create a stack of cards
            for (var i = 0; i < NumCards; i++)
            {
                var card = (View) this.ItemTemplate.CreateContent();
                this._views[i] = card;
                card.InputTransparent = true;
                card.IsVisible = false;

                view.Children.Add(
                    card,
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent(p => p.Width),
                    Constraint.RelativeToParent(p => p.Height)
                );
            }

            this.BackgroundColor = Color.Black;

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += this.OnPanUpdated;
            this.GestureRecognizers.Add(panGesture);
            this.Content = view;
        }

        public void ItemsSetup()
        {
            // set the top card
            this._topCardIndex = 0;
            // create a stack of cards
            for (var i = 0; i < Math.Min(NumCards, this.ItemsSource.Length); i++)
            {
                if (this._itemIndex >= this.ItemsSource.Length) break;
                var card = this._views[i];
                card.BindingContext = this.ItemsSource[i];
                card.IsVisible = true;
                card.Scale = this.GetScale(i);
                card.RotateTo(0, 0);
                card.TranslateTo(0, -card.Y, 0);
                ((RelativeLayout)this.Content).LowerChild(card);
                this._itemIndex++;
            }
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    this.HandleTouchStart();
                    break;
                case GestureStatus.Running:
                    this.HandleTouch((float)e.TotalX);
                    break;
                case GestureStatus.Completed:
                    this.HandleTouchEnd();
                    break;
            }
        }

        // to hande when a touch event begins
        public void HandleTouchStart()
        {
            this._cardDistance = 0;
        }

        // to handle te ongoing touch event as the card is moved
        public void HandleTouch(float diffX)
        {
            if (this._ignoreTouch)
            {
                return;
            }

            var topCard = this._views[this._topCardIndex];
            var backCard = this._views[this.PrevCardIndex(this._topCardIndex)];

            // move the top card
            if (topCard.IsVisible)
            {

                // move the card
                topCard.TranslationX = diffX;

                // calculate a angle for the card
                float rotationAngel = (float)(CardRotationAdjuster * Math.Min(diffX / this.Width, 1.0f));
                topCard.Rotation = rotationAngel * DegreesToRadians;

                // keep a record of how far its moved
                this._cardDistance = diffX;
            }

            // scale the backcard
            if (backCard.IsVisible)
            {
                backCard.Scale = Math.Min(BackCardScale + Math.Abs(this._cardDistance /this.CardMoveDistance * (1.0f - BackCardScale)), 1.0f);
            }
        }

        // to handle the end of the touch event
        public async void HandleTouchEnd()
        {
            this._ignoreTouch = true;

            var topCard = this._views[this._topCardIndex];

            // if the card was move enough to be considered swiped off
            if (Math.Abs((int) this._cardDistance) > this.CardMoveDistance)
            {

                // move off the screen
                await topCard.TranslateTo(this._cardDistance > 0 ? this.Width : -this.Width, 0, AnimLength / 2, Easing.SpringOut);
                topCard.IsVisible = false;

                if (this.SwipedRight != null && this._cardDistance > 0)
                {
                    this.SwipedRight(this._itemIndex);
                }
                else
                {
                    this.SwipedLeft?.Invoke(this._itemIndex);
                }

                // show the next card
                this.ShowNextCard();

            }
            // put the card back in the center
            else
            {

                // move the top card back to the center
                await topCard.TranslateTo(-topCard.X, -topCard.Y, AnimLength, Easing.SpringOut);
                await topCard.RotateTo(0, AnimLength, Easing.SpringOut);

                // scale the back card down
                var prevCard = this._views[this.PrevCardIndex(this._topCardIndex)];
                await prevCard.ScaleTo(BackCardScale, AnimLength, Easing.SpringOut);

            }

            this._ignoreTouch = false;
        }

        // show the next card
        private void ShowNextCard()
        {
            if (this._views[0].IsVisible == false && this._views[1].IsVisible == false)
            {
                this.ItemsSetup();
                return;
            }

            var topCard = this._views[this._topCardIndex];
            this._topCardIndex = this.NextCardIndex(this._topCardIndex);

            // if there are more cards to show, show the next card in to place of 
            // the card that was swipped off the screen
            if (this._itemIndex < this.ItemsSource.Length)
            {
                // push it to the back z order
                ((RelativeLayout)this.Content).LowerChild(topCard);

                // reset its scale, opacity and rotation
                topCard.Scale = BackCardScale;
                topCard.RotateTo(0, 0);
                topCard.TranslateTo(0, -topCard.Y, 0);

                // set the data
                topCard.BindingContext = this.ItemsSource[this._itemIndex];

                topCard.IsVisible = true;
                this._itemIndex++;
            }
        }

        // return the next card index from the top
        private int NextCardIndex(int topIndex)
        {
            return topIndex == 0 ? 1 : 0;
        }

        // return the prev card index from the yop
        private int PrevCardIndex(int topIndex)
        {
            return topIndex == 0 ? 1 : 0;
        }

        // helper to get the scale based on the card index position relative to the top card
        private float GetScale(int index)
        {
            return index == this._topCardIndex ? 1.0f : BackCardScale;
        }
    }
}