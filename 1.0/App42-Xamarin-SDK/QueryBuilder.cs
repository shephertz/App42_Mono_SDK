using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace com.shephertz.app42.paas.sdk.csharp.storage
{
    public sealed class Operator
    {
        public static readonly String EQUALS = "$eq";
        public static readonly String NOT_EQUALS = "$ne";
        public static readonly String GREATER_THAN = "$gt";
        public static readonly String LESS_THAN = "$lt";
        public static readonly String GREATER_THAN_EQUALTO = "$gte";
        public static readonly String LESS_THAN_EQUALTO = "$lte";
        public static readonly String LIKE = "$lk";
        public static readonly String AND = "$and";
        public static readonly String OR = "$or";
        private String value;
        private Operator(String value)
        {
            this.value = value;
        }

        public String GetValue()
        {
            return value;
        }
    }

    public class QueryBuilder : App42Response
    {
        public static Query Build(String key, object value, String op)
        {
            Query query = null;
            if (!Operator.EQUALS.ToString().Equals(op) && !Operator.NOT_EQUALS.ToString().Equals(op) && !Operator.GREATER_THAN.ToString().Equals(op) && !Operator.LESS_THAN.ToString().Equals(op)
               && !Operator.GREATER_THAN_EQUALTO.ToString().Equals(op) && !Operator.LESS_THAN_EQUALTO.ToString().Equals(op) && !Operator.LIKE.ToString().Equals(op)
               && !Operator.AND.ToString().Equals(op) && !Operator.OR.ToString().Equals(op))
            {
                return null;
            }
            try
            {
                StringBuilder profileJson = new StringBuilder();
                StringWriter sw = new StringWriter(profileJson);
                JsonWriter profileJsonWriter = new JsonTextWriter(sw);
                profileJsonWriter.WriteStartObject();

                profileJsonWriter.WritePropertyName("key");
                profileJsonWriter.WriteValue(key);


                profileJsonWriter.WritePropertyName("value");
                profileJsonWriter.WriteValue(value);

                profileJsonWriter.WritePropertyName("operator");
                profileJsonWriter.WriteValue(op);
                profileJsonWriter.WriteEndObject();
                StringBuilder sb = new StringBuilder();
                sb.Append(profileJson.ToString());
                JObject joBject = JObject.Parse(sb.ToString());
                query = new Query(joBject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return query;
        }

        public static Query CompoundOperator(Query q1, String op, Query q2)
        {
            JArray jsonArray = new JArray();
            Query query = new Query(jsonArray);
            try
            {
                if (!Operator.EQUALS.ToString().Equals(op) && !Operator.NOT_EQUALS.ToString().Equals(op) && !Operator.GREATER_THAN.ToString().Equals(op) && !Operator.LESS_THAN.ToString().Equals(op)
                    && !Operator.GREATER_THAN_EQUALTO.ToString().Equals(op) && !Operator.LESS_THAN_EQUALTO.ToString().Equals(op) && !Operator.LIKE.ToString().Equals(op)
                    && !Operator.AND.ToString().Equals(op) && !Operator.OR.ToString().Equals(op))
                {
                    return null;
                }
                if (q1.GetType() is JObject)
                    jsonArray.Add(q1.GetType());
                else
                    jsonArray.Add(q1.GetType());

                jsonArray.Add("{'compoundOpt':'" + op + "'}");

                if (q2.GetType() is JObject)
                    jsonArray.Add(q2.GetType());
                else
                    jsonArray.Add(q2.GetType());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return query;
        }
    }
}