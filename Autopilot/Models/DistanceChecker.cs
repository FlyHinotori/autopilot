using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Autopilot.Models
{
    class DistanceChecker
    {
        public DistanceChecker()
        {
            FContent = new AutopilotEntities();
            FFlughaefen = new List<Autopilot.flughafen>();
        }

        //members
        AutopilotEntities FContent;
        List<Autopilot.flughafen> FFlughaefen;

        private double GetDistanceBetween(Autopilot.flughafen StartFH, Autopilot.flughafen ZielFH)
        {
            if (StartFH == null || ZielFH == null)
                throw new AuftragDatenUnvollstaendigException("Start- oder Zielflughafen auswählen!");

            //Fetch latitude and longitude
            double StartLat = double.Parse(StartFH.flh_latitude, CultureInfo.InvariantCulture);
            double StartLon = double.Parse(StartFH.flh_longitude, CultureInfo.InvariantCulture);
            double ZielLat = double.Parse(ZielFH.flh_latitude, CultureInfo.InvariantCulture);
            double ZielLon = double.Parse(ZielFH.flh_longitude, CultureInfo.InvariantCulture);

            //do the math
            double rlat1 = Math.PI * StartLat / 180;
            double rlat2 = Math.PI * ZielLat / 180;
            double theta = StartLon - ZielLon;
            double rtheta = Math.PI * theta / 180;
            double dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            return dist * 1.609344;
        }

        public void AddFlughafen(Autopilot.flughafen Flughafen)
        {
            if (Flughafen == null)
                throw new GeneralModelsException("Flughafen ist ungültig!");
            FFlughaefen.Add(Flughafen);
        }

        public bool CanFlugzeugDoTheFlight(Autopilot.flugzeugtyp FZTyp)
        {
            if (FZTyp == null)
                throw new GeneralModelsException("Flugzeugtyp ungültig!");
            if (FFlughaefen.Count < 2)
                return true;
            decimal MaxDistance = (decimal)FZTyp.ftyp_reichweite_km;
            bool ItCan = true;

            for (int i = 0; i < FFlughaefen.Count - 1; i++)
            {
                Autopilot.flughafen Start = FFlughaefen[i];
                Autopilot.flughafen Ziel = FFlughaefen[i+1];

                ItCan = ((double)MaxDistance >= GetDistanceBetween(Start, Ziel)) & ItCan;
            }
            return ItCan;
        }
    }
}
