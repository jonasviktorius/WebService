using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using SnydeWebService.Controllers;
using SnydeWebService.Models.Factory;

namespace SnydeWebService.Models.Repository
{
  public class GameRepository
  {
    private string _connectionString => ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    private GameFactory _gameFactory => new GameFactory();

    public IEnumerable<Game> GetActiveGames()
    {
      var items = new List<Item>();

      using (SqlConnection connect = new SqlConnection(_connectionString))
      {
        connect.Open();

        var query = "SELECT [G].[GameId], [G].[IsActive], [G].[IsOngoing], [G].[RoundNumber], " +
                     "Stuff((SELECT '|' + '{'+ CONVERT(VARCHAR(12), [P].[PersonId]) + ',' + [P].[Name] + '}' FROM [dbo].[PersonGamesNew] [PG] " +
                     "INNER JOIN [dbo].[PersonsNew] [P] ON[PG].[PersonId] = [P].[PersonId] " +
                     "WHERE[PG].[GameId] = [G].[GameId] " +
                     "FOR XML PATH('')), 1, 2, '') [Persons] " +
                     "FROM[dbo].[GamesNew][G] " +
                     "WHERE[IsActive] = @Active";

        var cmd = new SqlCommand(query, connect);
        cmd.Parameters.AddWithValue("@Active", 1);

        var reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
          while (reader.Read())
          {
            var item = new Item() {Fields = new Dictionary<string, object>()};
            for (var index = 0; index < reader.FieldCount; index++)
            {
              item.Fields.Add(reader.GetName(index), reader.GetValue(index));
            }
            if (item.Fields.Any())
              items.Add(item);
          }
        }
      }

      return items.Select(_gameFactory.Create);
    }
  }
}