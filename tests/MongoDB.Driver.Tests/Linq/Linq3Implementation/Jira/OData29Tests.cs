/* Copyright 2010-present MongoDB Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using MongoDB.Driver.Linq;
using MongoDB.TestHelpers.XunitExtensions;
using Xunit;

namespace MongoDB.Driver.Tests.Linq.Linq3Implementation.Jira;

public class OData29Tests: Linq3IntegrationTest
{
    [Theory]
    [ParameterAttributeData]
    public void Select_Dictionary_Expression_should_work(
        [Values(LinqProvider.V2, LinqProvider.V3)] LinqProvider linqProvider)
    {
        var collection = GetCollection(linqProvider);

        var constantExpr = Expression.Constant(1);

        var queryable = collection.AsQueryable()
            .Select(x => new Dictionary<string,object>
            {
                {"constant", constantExpr}, // Just a ConstantExpression to show Expressions aren't serialized.
            }).ToList();

        // TODO: add stages tests

        queryable[0]["constant"].Should().Be(1);
        queryable[1]["constant"].Should().Be(1);
    }

    private IMongoCollection<C> GetCollection(LinqProvider linqProvider)
    {
        var collection = GetCollection<C>("test", linqProvider);
        CreateCollection(
            collection,
            new C { A = 1 },
            new C { A = 2 });
        return collection;
    }

    private class C
    {
        public int A { get; set; }
    }

}
