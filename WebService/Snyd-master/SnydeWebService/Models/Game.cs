using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace SnydeWebService.Controllers
{
  public class Game
  {
    public int GameId { get; set; }

    public IEnumerable<Person> Participants { get; set; }

    public int RoundNumber { get; set; }

    public bool IsOngoing { get; set; }

    public bool IsActive { get; set; }

  }
}