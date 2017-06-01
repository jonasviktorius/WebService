using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SnydeWebService.Controllers;

namespace SnydeWebService.Models.Factory
{
  public class PersonFactory
  {
    public Person Create(Item item)
    {
      int id;
      if (!int.TryParse(item.Fields["PersonId"].ToString(), out id))
      {
        id = -1;
      }

      var name = (string) item.Fields["Name"];
      return new Person
      {
        PersonId = id,
        Name = name
      };
    }
  }
}