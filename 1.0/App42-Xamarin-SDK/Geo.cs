using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.geo
{
    public class Geo : App42Response
    {

        public String storageName;
        public String sourceLat;
        public String sourceLng;
        public Double distanceInKM;
        public DateTime createdOn;
        public IList<Point> pointList = new List<Geo.Point>();


        public String GetStorageName()
        {
            return storageName;
        }
        public void SetStorageName(String storageName)
        {
            this.storageName = storageName;
        }
        public String GetSourceLat()
        {
            return sourceLat;
        }
        public void SetSourceLat(String sourceLat)
        {
            this.sourceLat = sourceLat;
        }
        public String GetSourceLng()
        {
            return sourceLng;
        }
        public void SetSourceLng(String sourceLng)
        {
            this.sourceLng = sourceLng;
        }
        public Double GetDistanceInKM()
        {
            return distanceInKM;
        }
        public void SetDistanceInKM(Double distanceInKM)
        {
            this.distanceInKM = distanceInKM;
        }
        public DateTime GetCreatedOn()
        {
            return createdOn;
        }
        public void SetCreatedOn(DateTime createdOn)
        {
            this.createdOn = createdOn;
        }
        public IList<Point> GetPointList()
        {
            return pointList;
        }
        public void SetPointList(IList<Point> pointList)
        {
            this.pointList = pointList;
        }


        public class Point
        {

            public Point(Double lat, Double lng, String marker, Geo geo)
            {
                this.lat = lat;
                this.lng = lng;
                this.marker = marker;
                geo.pointList.Add(this);
            }

            public Point(Geo geo)
            {
                geo.pointList.Add(this);
            }

            public Double lat;
            public Double lng;
            public String marker;
            public Double GetLat()
            {
                return lat;
            }
            public void SetLat(Double lat)
            {
                this.lat = lat;
            }
            public Double GetLng()
            {
                return lng;
            }
            public void SetLng(Double lng)
            {
                this.lng = lng;
            }
            public String GetMarker()
            {
                return marker;
            }
            public void SetMarker(String marker)
            {
                this.marker = marker;
            }

            public override String ToString()
            {
                return "Lat : " + lat + " : Lang : " + lng + " : Marker : " + marker;
            }
        }
    }
}