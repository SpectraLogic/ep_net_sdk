﻿using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Model
{
    [DataContract]
    public class EscapePodJob : IEscapePodJob
    {
        [DataMember] public string Id { get; set; }
    }
}
