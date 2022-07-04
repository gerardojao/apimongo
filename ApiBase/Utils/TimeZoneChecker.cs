using System;
using System.Linq;
using ApiBase.Data;
using Microsoft.Extensions.Configuration;

namespace ApiBase.Utils
{
    public class TimeZoneChecker
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public TimeZoneChecker(AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public DateTime DT()
        {
            TimeZoneInfo setTimeZoneInfo;
            DateTime currentDateTime;
            //var hours = Convert.ToDouble(_configuration["ServerSettings:GMTLocal"]);
            //return new NormalizedDateTime(_context, _appsettings).DT().AddHours(hours);
            //Set the time zone information to US Mountain Standard Time 
            setTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(_configuration["ServerSettings:GMTLocal"]);

            //Get date and time in US Mountain Standard Time
            currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, setTimeZoneInfo);
            return currentDateTime;
        }

        public DateTime Today()
        {
            TimeZoneInfo setTimeZoneInfo;
            DateTime currentDateTime;
            setTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(_configuration["ServerSettings:GMTLocal"]);
            currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, setTimeZoneInfo);
            return currentDateTime.Date;
        }
        public DateTime Tomorrow()
        {
            TimeZoneInfo setTimeZoneInfo;
            DateTime currentDateTime;
            var today = DateTime.Now;
            var tomorrow = today.AddDays(1);
            setTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(_configuration["ServerSettings:GMTLocal"]);
            currentDateTime = TimeZoneInfo.ConvertTime(tomorrow, setTimeZoneInfo);
            return currentDateTime.Date;
        }

        /*public DateTime LocalizeTime(string _sid, DateTime time)
        {
            var member = _context.UserAddresses.FirstOrDefault(e => e.UserId == _sid );
            var now = time;

            if (Enum.IsDefined(typeof(Noroeste), member.State)) 
            {
               return now.AddHours(-2);

            } else if (Enum.IsDefined(typeof(Pacifico), member.State)) 
            {
               return now.AddHours(-1);

            } else if (Enum.IsDefined(typeof(Sureste), member.State)) 
            {
                return now.AddHours(1);

            } else {
                return now.AddHours(0);
            }
        }*/

        public DateTime UtcToServerTime(DateTime time)
        {
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            TimeSpan currentOffset = localZone.GetUtcOffset( time );

            if ( Convert.ToInt32(_configuration["ServerSettings:GMTLocal"]) == currentOffset.Hours) return time;
        
            return time.AddHours(Convert.ToDouble(_configuration["ServerSettings:GMTLocal"]));
        }

        public static long UTCDateTimeToUnix(DateTime time)
        {

            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = (time-epoch);
            return (long) span.TotalSeconds;
        }

        public static DateTime UnixToUTCDateTime(long time)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(time);
        }        

        public enum Centro : int
        {
            Aguascalientes = 1,
            Campeche = 4,
            Coahuila = 5,
            Colima = 6,
            Chiapas = 7,
            CDMX = 9,
            Durango = 10,
            Guanajuato = 11,
            Guerrero = 12,
            Hidalgo = 13,
            Jalisco = 14,
            Mexico = 15,
            Michoacan = 16,
            Morelos = 17,
            NuevoLeon = 19,
            Oaxaca = 20,
            Puebla = 21,
            Queretaro = 22,
            SanLuisPotosi = 24,
            Tabasco = 27,
            Tamaulipas = 28,
            Tlaxcala = 29,
            Veracruz = 30,
            Yucatan = 31,
            Zacatecas = 32
            
        }

        public enum Pacifico : int 
        {
            BajaCaliforniaSur = 3,
            Chihuahua = 8,
            Nayarit = 18,
            Sinaloa = 25,
            Sonora = 26
        }

        public enum Noroeste : int
        {
            BajaCalifornia = 2
        }

        public enum Sureste : int
        {
            QuintanaRoo = 23
        }
    }
}