using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SnydeWebService.Controllers;
using SnydeWebService.Models.Repository;

namespace SnydeWebService.Models.Factory
{
  public class GameFactory
  {
    private PersonRepository _personRepository => new PersonRepository();
    public Game Create(Item item)
    {
      return new Game()
      {
        GameId = (int)item.Fields["GameId"],
        RoundNumber = (int)item.Fields["RoundNumber"],
        IsOngoing = (bool)item.Fields["IsOngoing"],
        IsActive = (bool)item.Fields["IsActive"],
        Participants = _personRepository.ExpandAggregation((string)item.Fields["Persons"])
      };
    }
  }
}