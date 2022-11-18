
using System;
using System.Reflection;

namespace amantiq
{

    class Service : Attribute
    {
        public string path { get; }
        public object instance { get; set; }
        public MethodInfo method { get; set; }
        public String validation { get; set; }

        public Service(string path)
        {
            this.path = path;
        }

        public Service(string path, string validation)
        {
            this.path = path;
            this.validation = validation;
        }
    }
}