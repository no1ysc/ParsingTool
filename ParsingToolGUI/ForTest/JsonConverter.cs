using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingToolGUI.Logic
{
    class JsonConverter<ConvertType>
    {
        public string Object2Json(ConvertType obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public ConvertType Json2Object(string json)
        {
            return (ConvertType)JsonConvert.DeserializeObject<ConvertType>(json);
        }
    }
}
