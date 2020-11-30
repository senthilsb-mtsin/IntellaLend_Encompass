using MTSEntBlocks.ExceptionBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EncompassConnectorAPI
{
    public class JsonUtil
    {
        #region Methods

        /// <summary>
        /// Converts a json string to an object
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(string json)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw new MTSException("Exception in ConvertTo(): ", ex);
            }
        }

        /// <summary>
        /// Converts a json string to a list of objects
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(string json)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(json);
            }
            catch (Exception ex)
            {
                throw new MTSException("Exception in ConvertToList(): ", ex);
            }
        }

        public static Dictionary<TKey, TValue> ConvertToDictionary<TKey, TValue>(string json)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(json);
            }
            catch (Exception ex)
            {
                throw new MTSException("Exception in ConvertToDictionary(): ", ex);
            }
        }

        /// <summary>
        /// Convert a document object to a json string
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static string ConvertToJson(object obj)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, new Newtonsoft.Json.JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.Default });
            }
            catch (Exception ex)
            {
                throw new MTSException("Exception in ConvertToJson(): ", ex);
            }
        }

        #endregion
    }
}
