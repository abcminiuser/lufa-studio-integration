﻿// Guids.cs
// MUST match guids.h
using System;

namespace FourWalledCubicle.LUFA
{
    static class GuidList
    {
        public const string guidLUFAVSIXManifestString = "FourWalledCubicle.LUFA.0e160d5c-e331-48d9-850b-e0387912171b";
        public const string guidASFVSIXManifestString = "4CE20911-D794-4550-8B94-6C66A93228B8";
        public const string guidLUFAPkgString = "2cfcbce0-84cb-45dc-8315-cad3f82b9467";
        public const string guidLUFACmdSetString = "1f3dc770-b782-4cfd-b11a-b9065cdc6e70";

        public static readonly Guid guidLUFACmdSet = new Guid(guidLUFACmdSetString);
    };
}