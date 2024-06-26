﻿/* Copyright 2010-present MongoDB Inc.
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

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver.Encryption;
using Xunit;

namespace MongoDB.Driver.Tests.Encryption
{
    public class MongoEncryptionCreateCollectionExceptionTests
    {
        [Fact]
        public void Serialization_should_work()
        {
            var subject = new MongoEncryptionCreateCollectionException(new Exception("inner"), new BsonDocument("value", 1));

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
#pragma warning disable SYSLIB0011 // BinaryFormatter serialization is obsolete
                formatter.Serialize(stream, subject);
                stream.Position = 0;
                var rehydrated = (MongoEncryptionCreateCollectionException)formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011 // BinaryFormatter serialization is obsolete

                rehydrated.InnerException.Message.Should().Be(subject.InnerException.Message);
                rehydrated.EncryptedFields.Should().Be(subject.EncryptedFields).And.Should().NotBeNull();
            }
        }
    }
}
