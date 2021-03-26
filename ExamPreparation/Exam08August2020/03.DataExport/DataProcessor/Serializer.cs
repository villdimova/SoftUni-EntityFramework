namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models.Enums;
    using VaporStore.ExportResults;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{

			var genres = context.Genres
				.Where(x => genreNames.Contains(x.Name))
				.ToList()
				.Select(g => new
				{
					Id = g.Id,
					Genre = g.Name,
					Games = g.Games.Where(b => b.Purchases.Any()).Select(a => new
					{
						Id = a.Id,
						Title = a.Name,
						Developer = a.Developer.Name,
						Tags = String.Join(", ", a.GameTags.Select(x=>x.Tag.Name)),
						Players = a.Purchases.Count
					}).ToList().OrderByDescending(x => x.Players).ThenBy(x => x.Id),
					TotalPlayers = g.Games.Sum(g => g.Purchases.Count)

				}).ToList().OrderByDescending(x=>x.TotalPlayers).ThenBy(x=>x.Id);

			var json = JsonConvert.SerializeObject(genres, Formatting.Indented);

			return json;
				
                
		}

//		The given method in the project skeleton receives an array of genre names.
//			Export all games in those genres, which have any purchases.
//			For each genre, export its id, genre name, games and total players(total purchase count). 
//			For each game, export its id, name, developer, tags(separated by ", ") and total player count(purchase count).
//			Order the games by player count(descending), then by game id(ascending).
//Order the genres by total player count(descending), then by genre id(ascending)

		//"Id": 4,
  //  "Genre": "Violent",
  //  "Games": [
  //    {
  //      "Id": 49,
  //      "Title": "Warframe",
  //      "Developer": "Digital Extremes",
  //      "Tags": "Single-player, In-App Purchases, Steam Trading Cards, Co-op, Multi-player, Partial Controller Support",
  //      "Players": 6


		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
			var sb = new StringBuilder();

			var enumType = Enum.Parse<PurchaseType>(storeType);

			var userPurchases = context.Users
				.Where(u => u.Cards.SelectMany(p => p.Purchases).Count()>=1)
				.ToList()
				.Select(x => new ExportUserPurchasesDto
				{
					Username=x.Username,
					Purchases=x.Cards.SelectMany(p=>p.Purchases)
					.Where(p=>p.Type==enumType)
					.OrderBy(x => x.Date)
					.Select(p=>new ExportPurchasesDto
					{
							Card= p.Card.Number,
							Cvc=p.Card.Cvc,
							Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
							Game= new ExportGameDto 
							{
									Title=p.Game.Name,
									Genre=p.Game.Genre.Name,
									Price=p.Game.Price

							}

					}).ToArray(),
					
					TotalSpent=x.Cards.SelectMany(x=>x.Purchases).Where(p=>p.Type==enumType).Sum(x=>x.Game.Price)
				}).Where(p=>p.Purchases.Any())
				.OrderByDescending(x=>x.TotalSpent)
				.ThenBy(x=>x.Username)
				.ToList();
				

			var xmlSerializer = new XmlSerializer(typeof(List<ExportUserPurchasesDto>)
												, new XmlRootAttribute("Users"));

			var namespaces = new XmlSerializerNamespaces();
			namespaces.Add("", "");


			using (var writer = new StringWriter(sb))
			{
				xmlSerializer.Serialize(writer, userPurchases, namespaces);
			}

			return sb.ToString().Trim();


		}
	}
}

