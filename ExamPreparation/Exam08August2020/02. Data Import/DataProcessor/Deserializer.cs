namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var gamesDtos = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);
            var games = new List<Game>();
            var developers = new List<Developer>();
            var genres = new List<Genre>();
            var tags = new List<Tag>();

            foreach (var gameDto in gamesDtos)
            {
                if (!IsValid(gameDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                DateTime releaseDate;
                bool isDateValid = DateTime.TryParseExact(gameDto.ReleaseDate, "yyyy-MM-dd",
                                                CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                if (!isDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                var validGame = new Game
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = releaseDate,
             
                };

                var developer = developers.FirstOrDefault(d => d.Name == gameDto.Developer);

                if (developer==null)
                {
                   Developer newDeveloper = new Developer
                    {
                        Name = gameDto.Developer
                    };

                    developers.Add(newDeveloper);
                    validGame.Developer = newDeveloper;
                }

                else
                {
                    validGame.Developer = developer;
                }

                var genre = genres.FirstOrDefault(g => g.Name == gameDto.Genre);

                if (genre==null)
                {
                    Genre newGenre = new Genre
                    {
                        Name = gameDto.Genre
                    };

                    genres.Add(newGenre);
                    validGame.Genre = newGenre;
                }

                else
                {
                    validGame.Genre = genre;
                }
               
               

               
                foreach (var tagDto in gameDto.Tags)
                {

                    var tag = tags.FirstOrDefault(t => t.Name == tagDto);
                    if (tag==null)
                    {
                        var newTag = new Tag
                        {
                            Name = tagDto
                        };
                        tags.Add(newTag);
                        tag = newTag;
                    }
                    

                    var gameTag = new GameTag
                    {
                        Game = validGame,
                        Tag = tag
                    };

                  
                    validGame.GameTags.Add(gameTag);

                }

                if (validGame.GameTags.Count==0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                games.Add(validGame);
                sb.AppendLine($"Added {validGame.Name} ({validGame.Genre.Name}) with {validGame.GameTags.Count} tags");

            }

            context.Games.AddRange(games);
            context.SaveChanges();
            return sb.ToString().TrimEnd();

        }

        private static Tag GetTag(VaporStoreDbContext context, string currentTag)
        {
            var tag = context.Tags.FirstOrDefault(x => x.Name == currentTag);

            if (tag == null)
            {
                tag = new Tag
                {
                    Name = currentTag
                };
            }

            return tag;
        }

        private static Genre GetGenre(VaporStoreDbContext context, string gameDtoGenre)
        {
            var genre = context.Genres.FirstOrDefault(x => x.Name == gameDtoGenre);

            if (genre == null)
            {
                genre = new Genre
                {
                    Name=gameDtoGenre
                };

                context.Genres.Add(genre);
                context.SaveChanges();
            }

            return genre;
        }

        private static Developer GetDeveloper(VaporStoreDbContext context, string gameDtoDeveloper)
        {

            var developer = context.Developers.FirstOrDefault(x => x.Name == gameDtoDeveloper);

            if (developer == null)
            {
                developer = new Developer
                {
                    Name = gameDtoDeveloper
                };

                context.Developers.Add(developer);
                context.SaveChanges();
            }

            return developer;
        }



        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var usersDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);

            List<User> users = new List<User>();

            foreach (var userDto in usersDtos)
            {
                if (!IsValid(userDto) )
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

               
                var validUser = new User
                {
                    FullName = userDto.FullName,
                    Username= userDto.Username,
                    Email=userDto.Email,
                    Age=userDto.Age,
                   
                };

                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        break;
                    }

                     CardType type;
                    bool isValidCardType = Enum.TryParse(cardDto.Type, out type);
                    if (!isValidCardType)
                    {
                        sb.AppendLine(ErrorMessage);
                        break;
                    }
                    Card card=  new Card
                    {
                        Number = cardDto.Number,
                        Cvc = cardDto.CVC,
                        Type = type,
                        User=validUser
                    };

                    validUser.Cards.Add(card);
 
                }

                users.Add(validUser);
                sb.AppendLine($"Imported {validUser.Username} with {validUser.Cards.Count} cards");

            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        //For the purchase is required game
        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var xmlSerializer = new XmlSerializer(typeof(ImportPurchaseDto[]), new XmlRootAttribute("Purchases"));
            var purchaseDtos = ((ImportPurchaseDto[])xmlSerializer.Deserialize(new StringReader(xmlString)));

            List<Purchase> purchases = new List<Purchase>();

            foreach (var purchaseDto in purchaseDtos)
            {
                if (!IsValid(purchaseDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                PurchaseType type;

                bool isValidPurchaseType = Enum.TryParse(purchaseDto.Type, out type);

                if (!isValidPurchaseType)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime date;
                bool isValidDate = DateTime.TryParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.None,out date);

                if (!isValidDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var card = context.Cards.FirstOrDefault(c => c.Number == purchaseDto.Card);

                if (card==null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var game = context.Games.FirstOrDefault(g => g.Name == purchaseDto.Title);

                if (game==null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var purchase = new Purchase
                {
                    Type = type,
                    Game= game,
                    Card= card,
                    ProductKey=purchaseDto.Key,
                    Date=date,
              
                };

                purchases.Add(purchase);

                var userName = context.Users.FirstOrDefault(u => u.Cards.Any(c => c.Number == purchaseDto.Card)).Username;
                   
                sb.AppendLine($"Imported {purchaseDto.Title} for {userName}");

            }

            context.Purchases.AddRange(purchases);
            context.SaveChanges();
            return sb.ToString().Trim();


            
        }


        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
