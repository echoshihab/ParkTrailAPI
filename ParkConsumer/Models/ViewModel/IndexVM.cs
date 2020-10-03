using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkConsumer.Models.ViewModel
{
    public class IndexVM
    {
        public IEnumerable<NationalPark> NationalParks { get; set; }
        public IEnumerable<Trail> Trails { get; set; }
    }
}
