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
        }

        //members
        AutopilotEntities FContent;
        List<Autopilot.flughafen> FFlughaefen;

        private double GetDistanceBetween(int StartID, int DestinationID)
        {
            //Identify Flughaefen
            Autopilot.flughafen StartFH = FContent.flughafen.Where(fh => fh.flh_id == StartID).FirstOrDefault();
            Autopilot.flughafen ZielFH = FContent.flughafen.Where(fh => fh.flh_id == DestinationID).FirstOrDefault();
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
    }
}
