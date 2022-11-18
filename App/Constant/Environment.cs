using System;
using System.IO;

namespace amantiq.constant
{

    public class Environment
    {
        public static string ENVIRONMENT;
        public static string DATABASE_HOST;
        public static string DATABASE_PORT;
        public static string DATABASE_NAME;
        public static string DATABASE_USERNAME;
        public static string DATABASE_PASSWORD;
        public static string JAEGER_HOST;
        public static int JAEGER_PORT;
        public static string KUBEMQ_HOST;
        public static int KUBEMQ_PORT;

        public static void Initialize()
        {
            ENVIRONMENT = System.Environment.GetEnvironmentVariable("ENVIRONMENT");
            if (ENVIRONMENT == null)
            {
                ENVIRONMENT = "development";
            }

            DotNetEnv.Env.Load($"./Env/{ENVIRONMENT}.env");
            DATABASE_HOST = System.Environment.GetEnvironmentVariable("DATABASE_HOST");
            DATABASE_PORT = System.Environment.GetEnvironmentVariable("DATABASE_PORT");
            DATABASE_NAME = System.Environment.GetEnvironmentVariable("DATABASE_NAME");
            DATABASE_USERNAME = System.Environment.GetEnvironmentVariable("DATABASE_USERNAME");
            DATABASE_PASSWORD = System.Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
            JAEGER_HOST = System.Environment.GetEnvironmentVariable("JAEGER_AGENT_HOST");
            JAEGER_PORT = int.Parse(System.Environment.GetEnvironmentVariable("JAEGER_AGENT_PORT"));
            KUBEMQ_HOST = System.Environment.GetEnvironmentVariable("KUBEMQ_HOST");
            KUBEMQ_PORT = int.Parse(System.Environment.GetEnvironmentVariable("KUBEMQ_PORT"));
        }

        public static bool IsDevelopment
        {
            get
            {
                return ENVIRONMENT.ToLower() == "development";
            }
        }

        public static bool IsProduction
        {
            get
            {
                return ENVIRONMENT.ToLower() == "production";
            }
        }

        public static string KUBEMQ_ADDR
        {
            get
            {
                return String.Format(
                    "{0}:{1}",
                    KUBEMQ_HOST, KUBEMQ_PORT
                );
            }
        }

        public static string CONNECTION_STRING
        {
            get
            {
                return String.Format(
                    "Server={0};Port={1};Database={2};SearchPath=company-service;User Id={3};Password={4};",
                    DATABASE_HOST, DATABASE_PORT, DATABASE_NAME, DATABASE_USERNAME, DATABASE_PASSWORD
                );
            }
        }
    }
}