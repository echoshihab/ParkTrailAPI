using Microsoft.EntityFrameworkCore;
using ParkApi.Data;
using ParkApi.Models.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkApi.Models.Repository
{
    public class TrailRespository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRespository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return _db.Trails.Include(x => x.NationalPark).FirstOrDefault(x => x.Id == trailId);

        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(x => x.NationalPark).OrderBy(x => x.Name).ToList();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int nationalParkId)
        {
            return _db.Trails.Include(x=> x.NationalPark).Where(x => x.NationalParkId == nationalParkId).OrderBy(x => x.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            bool value= _db.Trails.Any(x => x.Name.ToLower().Trim()== name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            bool value = _db.Trails.Any(x => x.Id == id);
            return value;
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

      
    }
}
