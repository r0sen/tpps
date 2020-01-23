using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCoins
{

    public class Cryptozone
    {
        public List<Commonwealth> countries = new List<Commonwealth>();
        private List<Commonwealth> commonwealths;
        public List<Commonwealth> Commonwealths
        {
            get { return commonwealths; }
        }
        private const int NumberOfNeighbours = 4; // one to the north, east, west and south

        public Cryptozone()
        {
            commonwealths = new List<Commonwealth>();
        }

        public void addCommonwealth(Commonwealth country)
        {
            commonwealths.Add(country);
        }

        // links every city to its neighbours
        public void setCityNeighbours()
        {
            foreach (Commonwealth currentCountry in commonwealths)
            {
                foreach (City currentCity in currentCountry.Cities)
                {
                    if (currentCity.getNeighboursCount() == NumberOfNeighbours)
                        continue;
                    foreach (Commonwealth anotherCountry in commonwealths)
                    {
                        foreach (City anotherCity in anotherCountry.Cities)
                        {
                            if (currentCity.Equals(anotherCity))
                                continue;
                            if ((currentCity.XCoord + 1 == anotherCity.XCoord && currentCity.YCoord == anotherCity.YCoord) ||
                                (currentCity.XCoord - 1 == anotherCity.XCoord && currentCity.YCoord == anotherCity.YCoord) ||
                                (currentCity.XCoord == anotherCity.XCoord && currentCity.YCoord + 1 == anotherCity.YCoord) ||
                                (currentCity.XCoord == anotherCity.XCoord && currentCity.YCoord - 1 == anotherCity.YCoord))
                                currentCity.addNeighbour(anotherCity);
                        }
                    }
                }
            }
        }

        // checks if the entered countries are neighbours to each other
        public bool checkCountriesConnection()
        {
            foreach (Commonwealth currentCountry in commonwealths)
            {
                foreach (City currentCity in currentCountry.Cities)
                {
                    foreach (City neighbour in currentCity.Neighbours)
                    {
                        if (String.Compare(currentCity.CommonwealthName, neighbour.CommonwealthName) != 0)
                        {
                            currentCountry.linkedFlag = true;
                            return true;
                        }
                    }
                }
  
            }
            Console.WriteLine("Bad configuration of the countries. There's no connection of one of the commonwealth to the others");
            return false;
        }

        // distributes one representative portion of coins to the cities' neighbours
        public void distributeCoins()
        {
            // distribute coins to neighbours in 'new coins' lists
            foreach (Commonwealth currentCountry in commonwealths)
            {
                foreach (City currentCity in currentCountry.Cities)
                {
                    currentCity.distributeCoinsToNeighbours();
                }
            }

            // merge old and new ones
            foreach (Commonwealth currentCountry in commonwealths)
            {
                foreach (City currentCity in currentCountry.Cities)
                {
                    currentCity.mergeOldAndNewCoins();
                }
            }
        }

        public void sortCountries(int days)
        {
            // CommonwealthComparer cc = new CommonwealthComparer();
            List<Commonwealth> SortedList = commonwealths.OrderBy(o => o.days).ToList();
            // this.commonwealths.Sort(cc);
            commonwealths = SortedList;
            foreach (Commonwealth country in commonwealths)
            {
                Console.WriteLine(country.Name + " " + days);
            }
        }

        // checks complete cities and countries
        public bool checkCoinsDistribution(int days)
        {
            foreach (Commonwealth currentCountry in commonwealths)
            {
                int completeCityCount = 0;
                foreach (City currentCity in currentCountry.Cities)
                {
                    if (currentCity.CoinsBalances.Count == commonwealths.Count)
                    {
                        completeCityCount++;
                    }
                }
                if (completeCityCount == currentCountry.Cities.Count && !currentCountry.completeFlag)
                {
                    currentCountry.completeFlag = true;
                    currentCountry.days = days;
                    Console.WriteLine(" DAYS " + days);
                    //countries.Add(currentCountry);
                    //Console.WriteLine(currentCountry.Name + " " + days);
                }
                if (completeCityCount != currentCountry.Cities.Count)
                    return false;
            }

            return true;
        }
    }

}
