﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkConsumer
{
    public class SD
    {
        public static string APIBaseUrl = "https://localhost:44334/";
        public static string NationalParkAPIPath = APIBaseUrl+"api/v1/nationalParks";
        public static string TrailAPIPath = APIBaseUrl+"api/v1/trails";
    }
}