using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SnydeWebService.Controllers;
using SnydeWebService.Models.Factory;

namespace SnydeWebService.Models.Repository
{
  public class PersonRepository
  {
    private PersonFactory _personFactory => new PersonFactory();

    public IEnumerable<Person> ExpandAggregation(string aggregation)
    {
      var unfoldedContent = aggregation.Split('|');
      var items = new List<Item>();

      foreach (var c in unfoldedContent)
      {
        var item = new Item() { Fields = new Dictionary<string, object>() };
        var valueSet = c.Trim('}');
        valueSet = valueSet.Trim('{');
        var values = valueSet.Split(',');
        item.Fields.Add("PersonId", values[0]);
        item.Fields.Add("Name", values[1]);
        items.Add(item);
      }

      return items.Select(_personFactory.Create);
    }
  }
}