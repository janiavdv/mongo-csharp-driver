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

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoDB.Driver.GeoJsonObjectModel.Serializers
{
    /// <summary>
    /// Represents a serializer for a GeoJsonMultiPolygon value.
    /// </summary>
    /// <typeparam name="TCoordinates">The type of the coordinates.</typeparam>
    public class GeoJsonMultiPolygonSerializer<TCoordinates> : ClassSerializerBase<GeoJsonMultiPolygon<TCoordinates>> where TCoordinates : GeoJsonCoordinates
    {
        // private constants
        private static class Flags
        {
            public const long Coordinates = 16;
        }

        // private fields
        private readonly IBsonSerializer<GeoJsonMultiPolygonCoordinates<TCoordinates>> _coordinatesSerializer = BsonSerializer.LookupSerializer<GeoJsonMultiPolygonCoordinates<TCoordinates>>();
        private readonly GeoJsonObjectSerializerHelper<TCoordinates> _helper;

        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoJsonMultiPolygonSerializer{TCoordinates}"/> class.
        /// </summary>
        public GeoJsonMultiPolygonSerializer()
        {
            _helper = new GeoJsonObjectSerializerHelper<TCoordinates>
            (
                "MultiPolygon",
                new SerializerHelper.Member("coordinates", Flags.Coordinates)
            );
        }

        // public methods
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null)) { return false; }
            if (object.ReferenceEquals(this, obj)) { return true; }
            return
                base.Equals(obj) &&
                obj is GeoJsonMultiPolygonSerializer<TCoordinates> other &&
                object.Equals(_coordinatesSerializer, other._coordinatesSerializer);
        }

        /// <inheritdoc/>
        public override int GetHashCode() => 0;

        // protected methods
        /// <summary>
        /// Deserializes a value.
        /// </summary>
        /// <param name="context">The deserialization context.</param>
        /// <param name="args">The deserialization args.</param>
        /// <returns>The value.</returns>
        protected override GeoJsonMultiPolygon<TCoordinates> DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var geoJsonObjectArgs = new GeoJsonObjectArgs<TCoordinates>();
            GeoJsonMultiPolygonCoordinates<TCoordinates> coordinates = null;

            _helper.DeserializeMembers(context, (elementName, flag) =>
            {
                switch (flag)
                {
                    case Flags.Coordinates: coordinates = DeserializeCoordinates(context); break;
                    default: _helper.DeserializeBaseMember(context, elementName, flag, geoJsonObjectArgs); break;
                }
            });

            return new GeoJsonMultiPolygon<TCoordinates>(geoJsonObjectArgs, coordinates);
        }

        /// <summary>
        /// Serializes a value.
        /// </summary>
        /// <param name="context">The serialization context.</param>
        /// <param name="args">The serialization args.</param>
        /// <param name="value">The value.</param>
        protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, GeoJsonMultiPolygon<TCoordinates> value)
        {
            _helper.SerializeMembers(context, value, SerializeDerivedMembers);
        }

        // private methods
        private GeoJsonMultiPolygonCoordinates<TCoordinates> DeserializeCoordinates(BsonDeserializationContext context)
        {
            return _coordinatesSerializer.Deserialize(context);
        }

        private void SerializeCoordinates(BsonSerializationContext context, GeoJsonMultiPolygonCoordinates<TCoordinates> coordinates)
        {
            context.Writer.WriteName("coordinates");
            _coordinatesSerializer.Serialize(context, coordinates);
        }

        private void SerializeDerivedMembers(BsonSerializationContext context, GeoJsonMultiPolygon<TCoordinates> value)
        {
            SerializeCoordinates(context, value.Coordinates);
        }
    }
}
