﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SnydeWebService.Models.Repository;
using SnydeWebService.Request;

namespace SnydeWebService.Controllers
{
  [RoutePrefix("api/game")]
  public class GameController : ApiController
  {
    private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    [HttpPost]
    [Route("setupgame")]
    public HttpResponseMessage SetupGame([FromBody]CreateGameRequest host)
    {
      //if (host.PersonId == Guid.Empty)
      //  host.PersonId = Guid.NewGuid();

      //var game = new Game
      //{
      //  RoundNumber = 1,
      //  Participants = new List<Person>
      //          {
      //              new Person
      //              {
      //                  Name = host.Name,
      //                  PersonId = host.PersonId
      //              }
      //          }
      //};


      //using (SqlConnection connect = new SqlConnection(_connectionString))
      //{
      //  connect.Open();

      //  var personExistsQuery = "SELECT 1 AS Success FROM PersonsNew WHERE PersonId = @Id";
      //  SqlCommand cmd = new SqlCommand(personExistsQuery, connect);
      //  cmd.Parameters.AddWithValue("@Id", host.PersonId);
      //  var reader = cmd.ExecuteReader();

      //  int success = 0;
      //  if (!reader.HasRows)
      //  {
      //    string sql = "Insert into PersonsNew(PersonId,Name) values (@Id, @Name);";
      //    cmd = new SqlCommand(sql, connect);
      //    cmd.Parameters.AddWithValue("@Name", host.Name);
      //    cmd.Parameters.AddWithValue("@Id", host.PersonId);
      //    success = cmd.ExecuteNonQuery();

      //    if (success != 1)
      //      return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to insert person");
      //  }

      //  reader.Close();

      //  string insertGame = "Insert into GamesNew(GameId,RoundNumber) values (@Id,@RoundNumber);";
      //  cmd = new SqlCommand(insertGame, connect);
      //  cmd.Parameters.AddWithValue("@RoundNumber", game.RoundNumber);
      //  cmd.Parameters.AddWithValue("@Id", game.GameId);
      //  success = cmd.ExecuteNonQuery();
      //  if (success != 1)
      //    return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to insert Game");

      //  string insertPersonGame = "Insert into PersonGamesNew(PersonId,GameId) Values(@PersonId,@GameId);";

      //  cmd = new SqlCommand(insertPersonGame, connect);
      //  cmd.Parameters.AddWithValue("@PersonId", host.PersonId);
      //  cmd.Parameters.AddWithValue("@GameId", game.GameId);
      //  success = cmd.ExecuteNonQuery();
      //  if (success != 1)
      //    return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to relate person to game");

      //  connect.Close();

      //}

      return Request.CreateResponse(HttpStatusCode.OK, -1);
    }

    [HttpPost]
    [Route("RegisterToGame")]
    public HttpResponseMessage RegisterToGame([FromBody] RegisterToGameRequest user)
    {
      if (user.GameId == Guid.Empty)
        return Request.CreateResponse(HttpStatusCode.BadRequest, "Please provide a game id");

      if (user.PersonId == Guid.Empty)
        user.PersonId = Guid.NewGuid();

      using (SqlConnection connect = new SqlConnection(_connectionString))
      {
        connect.Open();
        var personExistsQuery = "SELECT 1 AS Success FROM PersonsNew WHERE PersonId = @Id";
        SqlCommand cmd = new SqlCommand(personExistsQuery, connect);
        cmd.Parameters.AddWithValue("@Id", user.PersonId);
        var reader = cmd.ExecuteReader();

        int success = 0;
        if (!reader.HasRows)
        {
          string sql = "Insert into PersonsNew(PersonId,Name) values (@Id, @Name);";
          cmd = new SqlCommand(sql, connect);
          cmd.Parameters.AddWithValue("@Name", user.Name);
          cmd.Parameters.AddWithValue("@Id", user.PersonId);
          success = cmd.ExecuteNonQuery();

          if (success != 1)
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to insert person");
        }

        reader.Close();

        string insertPersonGame = "Insert into PersonGamesNew(PersonId,GameId) Values(@PersonId,@GameId);";
        cmd = new SqlCommand(insertPersonGame, connect);
        cmd = new SqlCommand(insertPersonGame, connect);
        cmd.Parameters.AddWithValue("@PersonId", user.PersonId);
        cmd.Parameters.AddWithValue("@GameId", user.GameId);
        success = cmd.ExecuteNonQuery();
        if (success != 1)
          return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to relate person to game");

        connect.Close();
      }

      return Request.CreateResponse(HttpStatusCode.OK, "Velkommen til spillet ö");
    }

    [HttpPost]
    [Route("Submit")]
    public HttpResponseMessage Submit()
    {
      return Request.CreateResponse(HttpStatusCode.OK, "Submit not implemented");
    }


    // Find spil metode
    [HttpGet]
    [Route("DiscoverGame")]
    public HttpResponseMessage DiscoverGame()
    {
      var gameRepository = new GameRepository();

      var activeGames = gameRepository.GetActiveGames();
      
      return Request.CreateResponse(HttpStatusCode.OK, activeGames);
    }

    [HttpGet]
    [Route("GameStatus")]
    public HttpResponseMessage GameStatus()
    {
      return Request.CreateResponse(HttpStatusCode.OK, "GameStatus not implemented");
    }
  }
}
