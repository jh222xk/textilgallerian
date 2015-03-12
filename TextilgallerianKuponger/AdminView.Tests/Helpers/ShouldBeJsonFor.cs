using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using NSpec;

namespace AdminView.Tests.Helpers
{
    static class ShouldBeJsonFor
    {
        public static void should_be_json_for(this String self, Object to)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var key = new Regex(@"""UniqueKey"":""([A-Z0-9]{20})""");
            var createdAt = new Regex(@"""CreatedAt"":""([T0-9.:-]{10,30})""");
            var decimals = new Regex(@"([1-9]0??)0\.000?");
//            var decimals2 = new Regex(@"([1-9])0\.000");
            self = key.Replace(self, @"""UniqueKey"":null");
            self = createdAt.Replace(self, @"""CreatedAt"":""0001-01-01T00:00:00""");
            self = decimals.Replace(self, @"${1}0.0");
//            self = decimals2.Replace(self, @"${1}0.0");
            self.should_be(JsonConvert.SerializeObject(to));
        }
    }
}
