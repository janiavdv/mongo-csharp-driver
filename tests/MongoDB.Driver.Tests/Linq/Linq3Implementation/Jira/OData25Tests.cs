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

using Castle.Core.Internal;
using FluentAssertions;
using MongoDB.Driver.Linq;
using Xunit;

namespace MongoDB.Driver.Tests.Linq.Linq3Implementation.Jira;

public class OData25Tests : Linq3IntegrationTest
{
    [Fact]
    public void Select_nullable_int_long_should_work()
    {
        var collection = GetCollection();

        // conversion fails
        var queryable = collection.AsQueryable()
            .Select(c => (int?)((long)c.x * 100));

        var result = queryable.ToList();
        result[0].Should().Be(100);
        result[1].Should().Be(200);
    }

    [Fact]
    public void Select_nullable_int_int_should_work()
    {
        var collection = GetCollection();

        var queryable = collection.AsQueryable()
            .Select(c => (int?)((int)c.x * 100));

        var result = queryable.ToList();
        result[0].Should().Be(100);
        result[1].Should().Be(200);
    }

    [Fact]
    public void Select_nullable_long_int_should_work()
    {
        var collection = GetCollection();

        var queryable = collection.AsQueryable()
            .Select(c => (long?)((int)c.x * 100));

        var result = queryable.ToList();
        result[0].Should().Be(100);
        result[1].Should().Be(200);
    }

    private IMongoCollection<OData25Tests.C> GetCollection()
    {
        var collection = GetCollection<OData25Tests.C>("test");
        CreateCollection(
            collection,
            new C { x = 1 }, new C { x = 2 }, new C { x = null } );
        return collection;
    }

    private class C
    {
        public int? x { get; set; }
    }
}
