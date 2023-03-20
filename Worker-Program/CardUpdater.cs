using ClashFeeder.Models;
using ClashFeeder.Repos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ClashFeeder
{
    public static class CardUpdater
    {
        public static void UpdateCardImages()
        {            //config to get connection string
            var config = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            CardsRepo cardsRepo = new CardsRepo(client, context);

            TRContext context = new TRContext(optionsBuilder.Options, client);


            using (WebClient imgClient = new WebClient())
            {
                int counter = 1;
                imgClient.Headers.Add("User-Agent:https://cdn.royaleapi.com/");
                string urlBase = "https://cdn.royaleapi.com/static/img/cards-150/";
                foreach (Card c in cards)
                {
                    imgClient.Headers.Add("User-Agent:https://cdn.royaleapi.com/" + counter);
                    string cardName = c.Name.Replace(" ", "-");
                    cardName = cardName.Replace(".", string.Empty);


                    string cardURL = $"{urlBase}{cardName}.png";
                    cardURL = cardURL.ToLower();


                    byte[] dataArr = imgClient.DownloadData(new Uri(cardURL));
                    while (imgClient.IsBusy)
                    {
                    }

                    cardName = c.Name.Replace("-", " ");
                    //save file to local
                    File.WriteAllBytes(@$"C:\Users\Elodin\Documents\Codex\Images\{cardName}.png", dataArr);

                    imgClient.Headers.Remove("User-Agent");
                    counter++;

                }
            }
        }
    }
}

