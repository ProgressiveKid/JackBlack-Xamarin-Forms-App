using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        StackLayout stackLayout = new StackLayout();
        StackLayout stackLayoutForUserCards = new StackLayout();
        StackLayout MenuButtons = new StackLayout();
        StackLayout stackLayoutForDieler = new StackLayout()
        {
            Padding = new Thickness(0, 20, 0, 30)

        };
        List<Image> UserCard = new List<Image>();
        List<Image> DielerCards = new List<Image>();
        public int countCard = 0;
        public int countDealerCard = 0;
        Label MyCount;
        Label DealerCount;
        Button OneMore;
        Button Stop;
        public MainPage()
        {
            InitializeComponent();
            StartGamme();
        }

        public void StartGamme()
        {

            OneMore = new Button()
            {
                Text = "One more card",
                TextColor = Color.White,
                BackgroundColor = Color.GreenYellow
            };
            OneMore.Clicked += OneMore_Clicked;
            Stop = new Button()
            {
                Text = "Stop",
                TextColor = Color.White,
                BackgroundColor = Color.Green
            };
            Stop.Clicked += Stop_Clicked;
            MenuButtons.Orientation = StackOrientation.Horizontal;
            MenuButtons.Children.Add(OneMore);
            MenuButtons.Children.Add(Stop);
            MyCount = new Label();
            DealerCount = new Label();
            MenuButtons.Children.Add(MyCount);
            MenuButtons.Children.Add(DealerCount);

            stackLayout.Children.Add(stackLayoutForDieler);
            stackLayout.Children.Add(stackLayoutForUserCards);
            stackLayout.Children.Add(MenuButtons);
            GetUserCard(2);
            GetDealerCard(1);
            Content = stackLayout;
        }
        public void GetDealerCard(int number)
        {
            var Deck = GetDeck(number);
            stackLayoutForDieler.Orientation = StackOrientation.Horizontal;
            foreach (var card in Deck.cards)
            {
                Image image = new Image
                {
                    Source = card.image,
                    HeightRequest = CorrectHeight,
                    WidthRequest = CorrectWeight,
                    HorizontalOptions = LayoutOptions.Center
                };
                DielerCards.Add(image);
                if (card.value == "JACK" || card.value == "QUEEN" || card.value == "KING" || card.value == "ACE")
                {

                    countDealerCard += 10;
                }
                else
                    countDealerCard += Convert.ToInt32(card.value);



                stackLayoutForDieler.Children.Add(image);
            }
            Content = stackLayout;
            DealerCount.Text = countDealerCard.ToString();
            Thread.Sleep(5000);
            if (DielerCards.Count != 1)
            {
                if (countDealerCard < 16)
                {
                    GetDealerCard(1);
                }
                if (countDealerCard > countCard)
                {

                    DisplayAlert("Выйграл дилер", "проиграли", "Вы проиграли");
                    RemoveTable();
                }
                else
                {
                    DisplayAlert("Выйграл игрок", "выйиграли", "Вы выйграли");
                    RemoveTable();


                }


            }
            Content = stackLayout;




        }
        int CorrectHeight = 80; int CorrectWeight = 100;

        public void GetUserCard(int number)
        {
            var Deck = GetDeck(number);

            stackLayoutForUserCards.Orientation = StackOrientation.Horizontal;
            foreach (var card in Deck.cards)
            {
                Image image = new Image
                {
                    Source = card.image,
                    HeightRequest = CorrectHeight,
                    WidthRequest = CorrectWeight,
                    HorizontalOptions = LayoutOptions.Center
                };
                UserCard.Add(image);
                if (card.value == "JACK" || card.value == "QUEEN" || card.value == "KING" || card.value == "ACE")
                {

                    countCard += 10;
                }
                else
                    countCard += Convert.ToInt32(card.value);



                stackLayoutForUserCards.Children.Add(image);
            }
            MyCount.Text = countCard.ToString();
            stackLayout.Children.Add(stackLayoutForUserCards);
            if (UserCard.Count > 3)
            {
                CorrectHeight -= 20;
                CorrectWeight -= 25;
                IList<Xamarin.Forms.View> _list = stackLayoutForUserCards.Children;
                for (int i = 0; i < _list.Count; i++)
                {
                    var element = _list[i];
                    if (element is Image)
                    {
                        Image image = (Image)element;
                        image.HeightRequest = CorrectHeight;
                        image.WidthRequest = CorrectWeight;
                    }
                }
            }
            if (countCard > 21)
            {
                RemoveTable();

                // GetUserCard(2);
            }
            if (countCard == 21)
            {
                Thread.Sleep(5000);
                DisplayAlert("Выйграл игрок", "выйграли", "Вы выйграли");
                RemoveTable();


            }
            Content = stackLayout;

        }
        public void RemoveTable()
        {
            stackLayout.Children.Clear();
            stackLayoutForDieler.Children.Clear();
            MenuButtons.Children.Clear();
            stackLayoutForUserCards.Children.Clear();

            //MyCount.Text = "Вы проиграли";
           // DisplayAlert("Вы проиграли", "Проиграли", "Вы ");
            Stop.Text = "Сново";
            UserCard.Clear();
            DielerCards.Clear();
            IList<Xamarin.Forms.View> _list = stackLayoutForUserCards.Children;
            countCard = 0;
            countDealerCard = 0;
            CorrectHeight = 80; CorrectWeight = 100;
            StartGamme();
          

        }
        private void Stop_Clicked(object sender, EventArgs e)
        {
            
            GetDealerCard(1);
        }

        private void OneMore_Clicked(object sender, EventArgs e)
        {
            GetUserCard(1);

        }

        public Root mainDeck;
        public Root GetDeck(int countOfCard)
        {
            string DeckID = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1");
            httpWebRequest.ContentType = "text/json"; // сформировали колоду
            httpWebRequest.Method = "GET";//Можно GET
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                MakeDeckClass myDeserializedClass = JsonConvert.DeserializeObject<MakeDeckClass>(result);
                DeckID = myDeserializedClass.deck_id;
            }
            var httpWebRequestA = (HttpWebRequest)WebRequest.Create($"https://deckofcardsapi.com/api/deck/{DeckID}/draw/?count={countOfCard}");
            httpWebRequestA.ContentType = "text/json"; // сформировали колоду
            httpWebRequestA.Method = "GET";//Можно GET
            var httpResponseA = (HttpWebResponse)httpWebRequestA.GetResponse();

            using (var streamReader = new StreamReader(httpResponseA.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(result);
                // myDeserializedClass.card
                mainDeck = myDeserializedClass;
                //  DeckID = myDeserializedClass.deck_id;
            }
            //  string jsonFileName = "json1.json";
            return mainDeck;




        }

    }
}
