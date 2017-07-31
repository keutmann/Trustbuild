﻿using Microsoft.Practices.Unity;

namespace TrustbuildServer
{
    public sealed class UnitySingleton
    {
        private static readonly UnityContainer instance = new UnityContainer();

        private UnitySingleton() { }

        public static UnityContainer Container
        {
            get
            {
                return instance;
            }
        }
    }
}
