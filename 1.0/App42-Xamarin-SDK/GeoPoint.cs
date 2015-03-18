using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

using System.IO;
using System.IO.MemoryMappedFiles;
using com.shephertz.app42.paas.sdk.csharp.connection;
using com.shephertz.app42.paas.sdk.csharp;




namespace com.shephertz.app42.paas.sdk.csharp.geo
{
    public class GeoPoint
    {

        private Double lat;
        private Double lng;
        private String marker;


        public GeoPoint()
        {

        }

        public GeoPoint(Double lat, Double lng, String marker)
        {
            this.lat = lat;
            this.lng = lng;
            this.marker = marker;
        }


        public void SetLat(Double lat)
        {
            this.lat = lat;

        }

        public Double GetLat()
        {
            return lat;
        }


        public void SetLng(Double lng)
        {
            this.lng = lng;

        }

        public Double GetLng()
        {
            return lat;
        }


        public void SetMarker(String marker)
        {
            this.marker = marker;

        }
        public String GetMarker()
        {
            return marker;
        }
        public StringBuilder GetJSONObject()
        {
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("lat");
            jsonWriter.WriteValue(this.lat);
            jsonWriter.WritePropertyName("lng");
            jsonWriter.WriteValue(this.lng);
            jsonWriter.WritePropertyName("marker");
            jsonWriter.WriteValue(this.marker);
            jsonWriter.WriteEndObject();
            return sbJson;
        }
    }
}