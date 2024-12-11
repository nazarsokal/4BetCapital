using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class SportEvent
    {
        public string Team1Name { get; }
        public string Team2Name { get; }
        public DateTime DateStart { get; }
        public DateTime DateEnd { get; }
        public double CoefficientsTeam1Win { get; }
        public double CoefficientsTeam2Win { get; }
        public double CoefficientsDraw { get; }

        public SportEvent(string team1Name, string team2Name, DateTime dateStart, DateTime dateEnd, double coefficientsTeam1Win,
            double coefficientsTeam2Win, double coefficientsDraw)
        {
            Team1Name = team1Name;
            Team2Name = team2Name;
            DateStart = dateStart;
            DateEnd = dateEnd;
            CoefficientsTeam1Win = coefficientsTeam1Win;
            CoefficientsTeam2Win = coefficientsTeam2Win;
            CoefficientsDraw = coefficientsDraw;
        }

        public override string ToString()
        {
            return $"{Team1Name} vs {Team2Name}. From {DateStart} to {DateEnd}. Coefs on {Team1Name} winning: {CoefficientsTeam1Win}." +
                $"Coefs on {Team2Name} winning: {CoefficientsTeam2Win}. Coeffs on draw: {CoefficientsDraw}";
        }
    }
}
