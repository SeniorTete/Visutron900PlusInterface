﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Visutron900PlusInterface.Adapter.DTOs;

namespace Visutron900PlusInterface.Adapter
{
    internal class RefraktionDataGenerator
    {
        public RefraktionData GenerateRefraktionData(List<string> valueList)
        {
            var refraktionData = new RefraktionData();
            var properties = refraktionData.GetType().GetProperties();

            for (int i = 0; i < valueList.Count; i++)
            {
                foreach (var propertyInfo in properties)
                {
                    var attributes = propertyInfo.GetCustomAttributes(typeof(IndexAttribute));
                    foreach (var attribute in attributes)
                    {
                        var indexer = attribute as IndexAttribute;
                        if (indexer != null && indexer.Index == i)
                        {
                            var targetTypeString = propertyInfo.PropertyType.ToString();
                            var sourceStringValue = valueList[i];
                            var currentValue = propertyInfo.GetValue(refraktionData);

                            try
                            {
                                var targetValue = ValueConverter.ConvertFromString(targetTypeString, sourceStringValue, currentValue);
                                propertyInfo.SetValue(refraktionData, targetValue);
                            }
                            catch (Exception exception)
                            {
                                Trace.WriteLine("Exception in [GenerateRefraktionData] bei " + propertyInfo.Name);
                                Trace.WriteLine(exception);
                            }
                        }
                    }
                }
            }
            return refraktionData;
        }
    }
}