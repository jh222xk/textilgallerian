using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NSpec;

namespace AdminView.Tests.Helpers
{
    static class ShouldBeJsonFor
    {
        public static void should_be_json_for(this String self, Object to)
        {
            var key = new Regex(@"""UniqueKey"":""([A-Z0-9]{20})""");
            var createdAt = new Regex(@"""CreatedAt"":""([A-Z0-9.:-]{10,30})""");
            var decimals = new Regex(@"([1-9])00.00");
            self = key.Replace(self, @"""UniqueKey"":null");
            self = createdAt.Replace(self, @"""CreatedAt"":""0001-01-01T00:00:00""");
            self = decimals.Replace(self, @"$100.0");
            self.should_be(JsonConvert.SerializeObject(to));
        }
    }
}
