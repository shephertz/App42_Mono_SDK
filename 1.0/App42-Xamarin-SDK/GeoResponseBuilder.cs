using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.geo
{
    public class GeoResponseBuilder : App42ResponseBuilder
    {

        public Geo BuildResponse(String json)
        {
            Geo geoObj = new Geo();
            IList<Geo.Point> pointList = new List<Geo.Point>();
            geoObj.SetPointList(pointList);

            geoObj.SetStrResponse(json);
            JObject jsonObj = JObject.Parse(json);
            JObject jsonObjApp42 = (JObject)jsonObj["app42"];
            JObject jsonObjResponse = (JObject)jsonObjApp42["response"];
            geoObj.SetResponseSuccess((Boolean)jsonObjResponse["success"]);
            JObject jsonObjGeoStorage = (JObject)jsonObjResponse["geo"]["storage"];
            App42Log.Debug("jsonObjGeoStorage" + jsonObjGeoStorage);

            BuildObjectFromJSONTree(geoObj, jsonObjGeoStorage);

            if (jsonObjGeoStorage["points"] == null)
                return geoObj;

            BuildInternalObj(geoObj, jsonObjGeoStorage);

            return geoObj;
        }



        public IList<Geo> BuildArrayResponse(String json)
        {
            IList<Geo> geoObjList = new List<Geo>();


            JObject jsonObj = JObject.Parse(json);
            JObject jsonObjApp42 = (JObject)jsonObj["app42"];
            JObject jsonObjResponse = (JObject)jsonObjApp42["response"];

            JObject jsonObjGeoStorage = (JObject)jsonObjResponse["geo"];
            if (jsonObjGeoStorage["storage"] != null && jsonObjGeoStorage["storage"] is JObject)
            {
                //Single Item 
                jsonObjGeoStorage = (JObject)jsonObjGeoStorage["storage"];
                Geo geoObj = new Geo();
                IList<Geo.Point> pointList = new List<Geo.Point>();
                geoObj.SetPointList(pointList);
                geoObj.SetStrResponse(json);
                geoObj.SetResponseSuccess((Boolean)jsonObjResponse["success"]);

                BuildObjectFromJSONTree(geoObj, jsonObjGeoStorage);
                geoObjList.Add(geoObj);
                if (jsonObjGeoStorage["points"] != null)
                    BuildInternalObj(geoObj, jsonObjGeoStorage);


            }
            else
            {
                //Multiple Item
                JArray jsonStorageArray = (JArray)jsonObjGeoStorage["storage"];
                for (int i = 0; i < jsonStorageArray.Count; i++)
                {
                    JObject jsonObjStorage = (JObject)jsonStorageArray[i];
                    Geo geoObj = new Geo();
                    IList<Geo.Point> pointList = new List<Geo.Point>();
                    geoObj.SetPointList(pointList);
                    geoObj.SetStrResponse(json);
                    geoObj.SetResponseSuccess((Boolean)jsonObjResponse["success"]);

                    BuildObjectFromJSONTree(geoObj, jsonObjStorage);
                    geoObjList.Add(geoObj);

                    if (jsonObjStorage["points"] != null)
                        BuildInternalObj(geoObj, jsonObjStorage);
                }
            }



            return geoObjList;
        }


        private Geo BuildInternalObj(Geo geoObj, JObject jsonObjGeoStorage)
        {
            JObject jsonGeoPoints = (JObject)jsonObjGeoStorage["points"];

            if (jsonGeoPoints["point"] == null)
                return geoObj;

            if (jsonGeoPoints["point"] != null && jsonGeoPoints["point"] is JObject)
            {
                // Only One attribute is there
                JObject jsonObjPoint = (JObject)jsonGeoPoints["point"];
                Geo.Point pointsItem = new Geo.Point(geoObj);
                BuildObjectFromJSONTree(pointsItem, jsonObjPoint);
            }
            else
            {
                // There is an Array of attribute
                JArray jsonObjPointsArray = (JArray)jsonGeoPoints["point"];
                for (int i = 0; i < jsonObjPointsArray.Count; i++)
                {
                    // Get Individual Attribute Node and set it into Object
                    JObject jsonObjPoint = (JObject)jsonObjPointsArray[i];
                    Geo.Point pointsItem = new Geo.Point(geoObj);
                    BuildObjectFromJSONTree(pointsItem, jsonObjPoint);
                }
            }
            return geoObj;
        }


        public static void main(String[] args)
        {
            //	/	Geo geo = new GeoResponseBuilder().buildResponse("{"app42":{"response":{"success":true,"geo":{"storage":{"storageName":"geoTest","points":{"point":[{"lat":-73.99171,"lng":40.73887,"marker":"10gen Office"},{"lat":-73.98814,"lng":40.741405,"marker":"Flatiron Building"},{"lat":-73.99781,"lng":40.73913,"marker":"Players Club"},{"lat":-73.99249,"lng":40.738674,"marker":"City Bakery"},{"lat":-73.99249,"lng":40.738674,"marker":"Splash Bar"},{"lat":-73.98584,"lng":40.731697,"marker":"Momofuku Milk Bar"},{"lat":-73.9882,"lng":40.74164,"marker":"Shake Shack"},{"lat":-73.99408,"lng":40.75057,"marker":"Penn Station"},{"lat":-73.98602,"lng":40.74894,"marker":"Empire State Building"},{"lat":-73.99756,"lng":40.73083,"marker":"Washington Square Park"},{"lat":106.9154,"lng":47.9245,"marker":"Ulaanbaatar, Mongolia"},{"lat":-74.2713,"lng":40.73137,"marker":"Maplewood, NJ"}]}}}}}}");
        }
    }
}